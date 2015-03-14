using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Website.Common;
using Ecms.Biz;

namespace Ecms.Website.Site.MBGN
{
    public partial class Payment : System.Web.UI.Page
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();
        private readonly InvoiceService _invoiceService = new InvoiceService();
        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] == null)
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx");

                LoadData();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
				if (Session["Customer"] == null)
				{
					Response.Redirect("~/site/mbgn/loginRequirement.aspx");
				}

				lblError.Text = "";
				lblError.Visible = false;
                string userCode = "";                
				var customer = (UserCustomerModel)Session["Customer"];
				userCode = customer.UserCode;

				string orderId = "";
				////Check OrderNo
				//if (!string.IsNullOrEmpty(txtOrderNo.Text))
				//{
				//    var order = _orderService.OrderGet(""
				//                        , txtOrderNo.Text
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , ""
				//                        , this).SingleOrDefault();

				//    if (order == null)
				//    {
				//        lblError.Text = "Số đơn hàng không chính xác";
				//        lblError.Visible = true;
				//        return;
				//    }
				//    orderId =Convert.ToString(order.OrderId);
				//}
                var invoice = _invoiceService.InvoiceCreate(
								userCode
								, ddlBank.SelectedValue
								, txtAccountNo.Text
								, orderId
								, txtAmount.Text.Trim().Replace(",", "")
								, txtContentPay.Text
								, Const_BusinessCode.Business_201
								, this);

                if (invoice != null)
                {
                    mtvMain.ActiveViewIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnPayAffter_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            try
            {
                var banks = _commonService.BankList("", "");
                ddlBank.DataSource = banks;
                ddlBank.DataTextField = "BankName";
                ddlBank.DataValueField = "BankId";
                ddlBank.DataBind();
                ddlBank.Items.Insert(0, new ListItem("-- Chọn ngân hàng --", ""));

                string orderNo = "";
				if (Session["OrderCode"] != null)
				{
					
					orderNo = Session["OrderCode"].ToString();
				}

				if (Session[Constansts.SESSION_ORDERNO] != null)
				{
					//this.txtOrderNo.Enabled = false;
					this.trTotal.Visible = true;

					orderNo = Session[Constansts.SESSION_ORDERNO].ToString();

					//txtOrderNo.Text = orderNo;

					var order = _orderService.OrderGet("", orderNo, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
					if (order != null)
					{
						if (order.OrderTypeId == 2)
						{
							lblTotalMoney.Text = (order.RemainAmount ?? 0).ToString("N2");
						}

						if (order.OrderTypeId == 3)
						{
							lblTotalMoney.Text = (order.SumAmount ?? 0).ToString("N2");
						}
					}
				}
				else
				{
					// Trường hợp không có đơn hàng
					//txtOrderNo.Enabled = true;
					this.trTotal.Visible = false;
					//txtOrderNo.Text = "";
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion
    }
}