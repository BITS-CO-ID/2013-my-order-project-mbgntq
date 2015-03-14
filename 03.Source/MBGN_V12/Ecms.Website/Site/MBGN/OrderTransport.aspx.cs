using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Entities;
using Ecms.Biz;
using System.Globalization;

namespace Ecms.Website.Site.MBGN
{
    public partial class OrderTransport : System.Web.UI.Page
    {
        #region // Declares

        private readonly ProductService _productService = new ProductService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

		protected void btnSendOrder_Click(object sender, EventArgs e)
		{
			mtvMain.ActiveViewIndex = 1;

		}
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["CartTransport"] != null)
                {
                    var cartTransport = (List<TransportModel>)Session["CartTransport"];

                    Order order = new Order();
                    order.OrderDate = order.CreatedDate = DateTime.Now;
                    order.OrderTypeId = 3;
					order.Remark = txtRemark.Text;

                    foreach (var item in cartTransport)
                    {
                        var orderDetailTemp = new OrderDetail();
                        orderDetailTemp.OrderNoDelivery = item.OrderNoDelivery;
                        orderDetailTemp.TrackingNo = item.TrackingNo;
                        orderDetailTemp.DateToUsa = item.DateToUsa;
                        orderDetailTemp.DeliveryStatus = 0;
						orderDetailTemp.LotPrice = item.LotPrice;
                        if (item.InsuaranceConfigId != null)
                            orderDetailTemp.InsuaranceConfigId = item.InsuaranceConfigId;
                        else
                            orderDetailTemp.InsuaranceConfigId = null;
                        order.OrderDetails.Add(orderDetailTemp);
                    }

                    Session["Order"] = order;
                    Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        protected void gridCartTransport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "deleteCartTransport":
                        int Id = Convert.ToInt32(e.CommandArgument);
                        if (Session["CartTransport"] != null)
                        {
                            var cartTransport = (List<TransportModel>)Session["CartTransport"];
                            if (cartTransport.Count != 0)
                            {
                                var transport = cartTransport.Where(x => x.Id == Id).FirstOrDefault();

                                if (transport != null)
                                {
                                    cartTransport.Remove(transport);
                                    Session["CartTransport"] = cartTransport;
                                    Response.Redirect("");
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void gridCartTransport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridMain.PageIndex = e.NewPageIndex;
                LoadData();
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            DateTime dateToUsa = new DateTime();
            if (ValidData(txtTrackingNumber.Text, ref dateToUsa) == false)
                return;

            var tranModel = new TransportModel();
            tranModel.TrackingNo = txtTrackingNumber.Text;
            //tranModel.OrderNoDelivery = txtOrderNumber.Text;
			

            if (chkInsuarance.Checked)
            {
                var configBusiness = _customerService.ConfigBusinessGet("", "", "", "", "INSUARANCE", "", "", "", "", this).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (configBusiness != null)
                {
                    tranModel.InsuaranceConfigId = configBusiness.ConfigBusinessId;
                    tranModel.InsuaranceConfigValue = configBusiness.ConfigValue;
                }

				if (!string.IsNullOrEmpty(txtLotPrice.Text))
				{
					tranModel.LotPrice = txtLotPrice.Text;
				}
            }
            var cartTransportList = new List<TransportModel>();
            if (Session["CartTransport"] != null)
            {
                cartTransportList = (List<TransportModel>)Session["CartTransport"];
                if (cartTransportList.Count != 0)
                {
                    var Id = cartTransportList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                    tranModel.Id = Id;
                }
            }
            else
            {
                tranModel.Id = 1;
            }
            cartTransportList.Add(tranModel);
            Session["CartTransport"] = cartTransportList;
            //txtDateToUsa.Text = txtOrderNumber.Text = txtTrackingNumber.Text = txtLotPrice.Text= "";
            chkInsuarance.Checked = false;
			trLot.Visible = false;
            LoadData();
        }

        #endregion

        #region // Private methods

        private void LoadData()
        {
			//if (Session["Customer"] == null)
			//{
			//    var returnUrl = Request.Url.AbsolutePath;
			//    Response.Redirect("~/site/mbgn/loginRequirement.aspx?returnUrl=" + returnUrl);
			//}

            if (Session["CartTransport"] != null)
            {
                var transportModel = (List<TransportModel>)Session["CartTransport"];
                if (transportModel.Count != 0)
                {
                    gridMain.DataSource = transportModel;
                    gridMain.DataBind();
                    pnCartTransport.Visible = btnOrder.Visible = true;
                }
                else
                {
                    pnCartTransport.Visible = btnOrder.Visible = false;
                }
            }
            else
            {
                pnCartTransport.Visible = btnOrder.Visible = false;
            }
        }

        private bool ValidData(string trackingNumber, ref DateTime dateToUsa)
        {
			lblError.Visible = false;
            if (string.IsNullOrEmpty(trackingNumber))
            {
                lblError.Text = "Vui lòng nhập BillNo!";
                lblError.Visible = true;
                return false;
            }

			var trackingNoCheck = new OrderService().OrderDetailTrackingNoCheck(trackingNumber, this);
			if (trackingNoCheck)
			{
				lblError.Text = string.Format("Số TrackingNo=<strong>{0}</strong> đã có trong hệ thống", trackingNumber);
				lblError.Visible = true;
				return false;
			}

            return true;
        }        

		protected void chkInsuarance_OnCheckedChanged(object sender, EventArgs e)
		{
			if (chkInsuarance.Checked)
			{
				trLot.Visible = true;
			}
			else
				trLot.Visible = false;
		}

		#endregion
	}

    #region //TransportModel

    public class TransportModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public double? Quantity { get; set; }
        public double? Weight { get; set; }
        public double? Surcharge { get; set; }

        public string TrackingNo { set; get; }
        public string OrderNoDelivery { get; set; }
        public DateTime? DateToUsa { get; set; }
        public int? DeliveryStatus { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? InsuaranceConfigId { get; set; }
        public double? InsuaranceConfigValue { get; set; }
		public string LotPrice { set; get; }
    }

    #endregion
}