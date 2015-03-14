using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using System.Globalization;
using CommonUtils;

namespace Ecms.Website.Site.MBGN
{
    public partial class OrderDeliveryEditTrackingOrder : System.Web.UI.Page
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["Customer"] == null)
					Response.Redirect("~/site/mbgn/login.aspx");
                LoadData();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
			if (string.IsNullOrEmpty(Request.QueryString["orderDetailId"]))
			{
				Response.Redirect("~/site/mbgn/login.aspx");
			}

            string dateToUsa = "";
            if (ValidData(txtTrackingNumber.Text, ref dateToUsa) == false)
                return;
            try
            {
                string insuarance = "";
                if (chkInsuarance.Checked)
                {
                    var configBusiness = _customerService.ConfigBusinessGet("", "", "", "", "INSUARANCE", "", "", "", "", this).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    if (configBusiness != null)
                    {
                        insuarance = configBusiness.ConfigBusinessId + "";
                    }
                }

                var result = _orderService.UpdateTrackingOrder(
								Request.QueryString["orderDetailId"]
								, txtTrackingNumber.Text
								, ""
								, dateToUsa
								, insuarance
								, this);
                if (result)
                {
                    mtvMain.ActiveViewIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            var orderReturn = _orderService.OrderGet(Request.QueryString["orderId"].ToString(),
													"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this
                                                    ).FirstOrDefault();
            if (orderReturn != null)
            {
                Session[Constansts.SESSION_ORDERMODEL] = orderReturn;
                Response.Redirect("~/site/mbgn/orderbycheckeddetail.aspx");
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var orderReturn = _orderService.OrderGet(Request.QueryString["orderId"].ToString(),
													"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this
                                                    ).FirstOrDefault();
            if (orderReturn != null)
            {
                Session[Constansts.SESSION_ORDERMODEL] = orderReturn;
                Response.Redirect("~/site/mbgn/orderbycheckeddetail.aspx");
            }
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            try
            {
                if (Request.QueryString["orderId"] != null && Request.QueryString["trackingNo"] != null)
                {
                    string orderId = Request.QueryString["orderId"];
                    string trackingNumber = Request.QueryString["trackingNo"];
					var orderReturn = _orderService.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                    var orderDetailDelivery = orderReturn.lstOrderDetailModel.ToList().Where(x => x.TrackingNo == trackingNumber).FirstOrDefault();
                    if (orderDetailDelivery != null)
                    {
                        txtTrackingNumber.Text = orderDetailDelivery.TrackingNo;
                        //txtOrderNumber.Text = orderDetailDelivery.OrderNoDelivery;
                        //txtDateToUsa.Text = orderDetailDelivery.DateToUsa != null ? orderDetailDelivery.DateToUsa.Value.ToString("dd/MM/yyyy") : "";
                        chkInsuarance.Checked = orderDetailDelivery.InsuaranceConfigId != null ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private bool ValidData(string trackingNumber, ref string dateToUsa)
        {
            if (string.IsNullOrEmpty(trackingNumber))
            {
                lblError.Text = "Bạn chưa nhập BillNo!";
                lblError.Visible = true;
                return false;
            }

			//try
			//{
			//    if (!string.IsNullOrEmpty(txtDateToUsa.Text))
			//    {
			//        CultureInfo viVN = new CultureInfo("vi-VN");
			//        dateToUsa = DateTime.ParseExact(txtDateToUsa.Text, "dd/MM/yyyy", viVN).ToString("yyyy-MM-dd");
			//    }
			//}
			//catch
			//{
			//    lblError.Text = "Ngày đến Mỹ không đúng định dạng!";
			//    lblError.Visible = true;
			//    return false;
			//}

            return true;
        }


        #endregion
    }
}