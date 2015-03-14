using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Biz;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
namespace Ecms.Website.Admin.Order
{
    public partial class QuotationReply : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();
        private readonly CommonService _commonService = new CommonService();
        private readonly CustomerService _customerService = new CustomerService();
		protected double dTotalMoneyOrder = 0;
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/admin/security/login.aspx");

                LoadData();
            }
        }

		protected void btnConfirm_Click(object sender, EventArgs e)
        {
			mtvMain.ActiveViewIndex = 1;
			btnOK.Visible = false;
			btnConfirm.Visible = true;
			txtRemark.Visible = true;
			btnReplyCancel.Visible = false;
			lblMessage.Text = "Bạn hãy nhập thông tin xác nhận trả lời báo giá(Ghi chú)";
        }

		protected void btnReply_Click(object sender, EventArgs e)
		{
			try
			{
				if (Request.QueryString["orderId"] != null)
				{
					var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
					if (order != null)
					{
						var result = _orderService.OrderChangeStatusWithRemark(order.OrderId + "", Convert.ToString(OrderInStatus.QuoteConfirmed), this.txtRemark.Text, this);
						if (result)
						{
							mtvMain.ActiveViewIndex = 1;
							lblMessage.Text = "Bạn đã gửi trả lời báo giá tới khách hàng";
							btnReply.Visible = false;
							btnOK.Visible = true;
							txtRemark.Visible = false;

							////Gửi mail thông báo
							//string pathFile = Server.MapPath("~/Content/TemplateMail/QuotationReply.htm").Replace("\\", "/");
							//_commonService.SendMailConfirmedOrder(order.OrderId + "", pathFile, this);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			mtvMain.ActiveViewIndex = 1;
			btnOK.Visible = false;
			btnConfirm.Visible = false;
			btnReply.Visible = false;
			btnReplyCancel.Visible = true;
			txtRemark.Visible = true;
			lblMessage.Text = "Bạn hãy nhập thông tin xác nhận HỦY báo giá(Ghi chú)";
		}

		protected void btnReplyCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["orderId"] != null)
                {
					var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();

                    if (order != null)
                    {
						var result = _orderService.OrderChangeStatusWithRemark(order.OrderId + "", Convert.ToString(OrderInStatus.OrderCancel), this.txtRemark.Text, this);
                        if (result)
                        {
                            mtvMain.ActiveViewIndex = 1;
                            lblMessage.Text = "Bạn đã hủy Báo giá thành công";
							btnOK.Visible = true;
							btnReply.Visible = false;
							btnReplyCancel.Visible = false;
							txtRemark.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx?returnBack=return");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx?returnBack=return");
        }

        protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "updateOrderDetail":
                    if (Request.QueryString["orderId"] != null)
                    {
						var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                        if (order != null)
                        {
                            var orderDetailId = Convert.ToInt32(e.CommandArgument + "");
                            var orderDetailFirst = order.lstOrderDetailModel.ToList().Where(x => x.OrderDetailId == orderDetailId).FirstOrDefault();
                            if (orderDetailFirst != null)
                            {
                                Session[Constansts.SS_ORDER_DETAIL_ADMIN] = orderDetailFirst;
                                Response.Redirect("~/admin/order/quotationdetailupdate.aspx?orderId=" + Request.QueryString["orderId"].ToString());
                            }
                            else
                            {
                                Response.Redirect("~/admin/order/ordermanage.aspx");
                            }
                        }
                    }
                    break;
            }
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Request.QueryString["orderId"] != null)
            {
				var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();

                if (order != null)
                {
                    lblOrderNo.Text = order.OrderNo;
                    lblCreatedDate.Text = order.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    lblCustomerCode.Text = order.CustomerCode;
                    lblCustomerName.Text = order.CustomerName;
                    lblPhone.Text = order.Mobile;
                    lblAddress.Text = order.Address;
                    //lblTotalMoneyOrder.Text = (order.SumAmount ?? 0).ToString("N1");
					dTotalMoneyOrder = (order.SumAmount ?? 0);
					txtRemark.Text = order.Remark;
					lblRemark.Text = order.Remark;
                    gridMain.DataSource = order.lstOrderDetailModel;
                    gridMain.DataBind();

					if (order.OrderStatus != OrderInStatus.QuotePending)
					{
						btnConfirm.Enabled = btnCancel.Enabled = false;
						btnConfirm.CssClass = btnCancel.CssClass = Constansts.CssClass_buttonDisable;
					}

                }
            }
            else
            {
                Response.Redirect("~/admin/order/ordermanage.aspx");
            }
        }

        #endregion
    }
}