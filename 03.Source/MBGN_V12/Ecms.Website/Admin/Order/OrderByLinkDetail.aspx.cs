using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Website.Common;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderByLinkDetail : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();
        private readonly CommonService _commonService = new CommonService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/admin/security/login.aspx");

                LoadData();

				// Set Auth
				this.btnSave.CommandName = MenuFunction.Func_MHConfirm;
				this.btnUpdate.CommandName = MenuFunction.Func_MHFinished;
				this.btnComplete.CommandName = MenuFunction.Func_MHDeliverly;
				this.btnReverFirst.CommandName = MenuFunction.Func_MHRevertFinished;
				this.btnRevert.CommandName = MenuFunction.Func_MHRevertPending;
				this.btnCancel.CommandName = MenuFunction.Func_MHCancel;

				this.SetButtonAuth();
            }
        }

		protected void btnSave_Click(object sender, EventArgs e)
		{
			mtvMain.ActiveViewIndex = 1;
			lblMessage.Text = "Bạn hãy nhập thông tin xác nhận đơn hàng này";
			btnConfirm.Visible = true;
			txtRemark.Visible = true;
			btnOK.Visible = false;
			btnConfirmCancel.Visible = false;
		}

		protected void btnConfirm_Click(object sender, EventArgs e)
        {
			try
			{
				if (Request.QueryString["orderId"] != null)
				{

					var orderId = Request.QueryString["orderId"].ToString();

					var result = _orderService.OrderChangeStatusWithRemark(
									orderId
									, Convert.ToString(OrderInStatus.OrderConfirmed)
									, this.txtRemark.Text
									, this);
					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblMessage.Text = "Bạn đã xác nhận đơn hàng thành công";

						btnConfirm.Visible = false;
						btnConfirmCancel.Visible = false;
						txtRemark.Visible = false;
						btnOK.Visible = true;

						////Gửi mail thông báo
						//string pathFile = Server.MapPath("~/Content/TemplateMail/OrderByLinkDetail.htm").Replace("\\", "/");
						//_commonService.SendMailConfirmedOrder(orderId, pathFile, this);
					}

				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

		protected void btnReverFirst_Click(object sender, EventArgs e)
		{
			try
			{
				lblError.Visible = false;
				if (Request.QueryString["orderId"] != null)
				{
					var orderId = Request.QueryString["orderId"].ToString();
					// Check Xem đã thanh toán chưa?
					var payments = new InvoiceService().InvoiceGet(
									""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, Convert.ToString(InvoiceStatus.Confirm)
									, orderId
									, ""
									, Const_BusinessCode.Business_201
									, ""
									, ""
									, this);
					if (payments != null && payments.Count > 0)
					{
						lblError.Text = "Đơn hàng đã được khớp thanh toán, bạn hãy Hủy khớp thanh toán trước khi Hoàn lại tình trạng!";
						lblError.Visible = true;
						return;
					}

					var result = _orderService.RevertFirstOrderStatus(
									orderId
									, this);
					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblMessage.Text = "Bạn đã Hoàn lại tình trạng Chưa xác nhận đơn hàng thành công";
						btnConfirm.Visible = false;
						btnConfirmCancel.Visible = false;
						txtRemark.Visible = false;
						btnOK.Visible = true;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

		protected void btnRevert_Click(object sender, EventArgs e)
		{
			try
			{
				
				if (Request.QueryString["orderId"] != null)
				{	
					var orderId = Request.QueryString["orderId"].ToString();				

					var result = _orderService.RevertOrderStatus(
									orderId
									, this);
					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblMessage.Text = "Bạn đã Hoàn lại tình trạng đơn hàng thành công";
						btnConfirm.Visible = false;
						btnConfirmCancel.Visible = false;
						txtRemark.Visible = false;
						btnOK.Visible = true;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
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
			lblMessage.Text = "Bạn hãy nhập thông tin xác nhận HỦY đơn hàng này";
			btnConfirm.Visible = false;
			txtRemark.Visible = true;
			btnOK.Visible = false;
			btnConfirmCancel.Visible = true;
		}
		protected void btnConfirmCancel_Click(object sender, EventArgs e)
        {
			try
			{
				if (Request.QueryString["orderId"] != null)
				{
					var orderId = Request.QueryString["orderId"].ToString();
					var result = _orderService.OrderChangeStatusWithRemark(
									orderId
									, Convert.ToString(OrderInStatus.OrderCancel)
									, this.txtRemark.Text
									, this);

					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblMessage.Text = "Bạn đã HỦY đơn hàng thành công";
						btnConfirm.Visible = false;
						btnConfirmCancel.Visible = false;
						txtRemark.Visible = false;
						btnOK.Visible = true;
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

        protected void btnComplete_Click(object sender, EventArgs e)
        {
			try
			{
				if (Request.QueryString["orderId"] != null)
				{
					var orderId = Request.QueryString["orderId"].ToString();
					var result = _orderService.OrderChangeStatus(
									orderId
									, Convert.ToString(OrderInStatus.Deliveried)
									, this);
					if (result)
					{
						lblMessage.Text = "Đơn hàng đã được xác nhận giao hàng.";
						mtvMain.ActiveViewIndex = 1;
						btnConfirm.Visible = false;
						btnConfirmCancel.Visible = false;
						txtRemark.Visible = false;
						btnOK.Visible = true;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
				if (Request.QueryString["orderId"] != null)
				{
					var orderId = Request.QueryString["orderId"].ToString();

					var result = _orderService.OrderChangeStatus(orderId
									, Convert.ToString(OrderInStatus.Finished)
									, this);
					if (result)
					{
						lblMessage.Text = "Đơn hàng đã được hoàn tất.";
						mtvMain.ActiveViewIndex = 1;
						btnConfirm.Visible = false;
						btnConfirmCancel.Visible = false;
						txtRemark.Visible = false;
						btnOK.Visible = true;

						//////Gửi mail thông báo
						//string pathFile = Server.MapPath("~/Content/TemplateMail/OrderByLinkDetailFinished.htm").Replace("\\", "/");
						//_commonService.SendMailConfirmedOrder(orderId, pathFile, this);
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "updateOrderDetail":
					if (Session["PolicyUserMH"] != null)
					{
						// show for permision
						Response.Redirect("~/admin/order/orderdetailremarkupdate.aspx");
					}
					else
					{
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
									Response.Redirect("~/admin/order/orderdetailupdate.aspx?orderId=" + Request.QueryString["orderId"].ToString());
								}
								else
								{
									Response.Redirect("~/admin/order/ordermanage.aspx");
								}
							}
						}
					}
					break;
				case "ChangeStatusDelivery":
					var param = e.CommandArgument.ToString().Split('|');
					Response.Redirect(string.Format("~/admin/order/orderstatusupdate.aspx?orderDetailId={0}&cur_status={1}", param[1], param[0]));
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
                    lblCreatedDate.Text = order.CreatedDate.Value.ToString("dd/MM/yyyy");
                    lblCustomerCode.Text = order.CustomerCode;
                    lblCustomerName.Text = order.CustomerName;
                    lblTrackingNumber.Text = order.TrackingNo;
					txtRemark.Text = order.Remark;
					lblRemark.Text = order.Remark;
					lblConfirmDate.Text = order.ConfirmDate == null ? "" : order.ConfirmDate.Value.ToString("dd/MM/yyyy");
                    lblPhone.Text = order.Mobile;
                    lblAddress.Text = order.Address;
                    lblTotalMoneyOrder.Text = (order.SumAmount ?? 0).ToString("N0");
					//lblAmountFeeDelay.Text = (order.AmountFeeDelay).ToString("N0");
					lblTotalAmountNormal.Text = order.TotalPayAmountNormal.ToString("N0");
					//lblAmountCalFeeDelay.Text = (order.AmountCalFeeDelay).ToString("N0");
					lblSumFeeShip.Text = (order.SumFeeShip ?? 0).ToString("N0");
					lblTotalAmount.Text = ((order.RemainAmount ?? 0)).ToString("N0");

                    gridMain.DataSource = order.lstOrderDetailModel;
                    gridMain.DataBind();

					if (order.OrderStatus == OrderInStatus.OrderPending)
					{
						btnUpdate.Enabled = btnComplete.Enabled = btnRevert.Enabled = btnReverFirst.Enabled=false;
						btnUpdate.CssClass = btnComplete.CssClass = btnRevert.CssClass = btnReverFirst.CssClass = Constansts.CssClass_buttonDisable;
					}

					if (order.OrderStatus == OrderInStatus.OrderConfirmed)
                    {
						btnSave.Enabled = btnCancel.Enabled = btnComplete.Enabled = btnRevert.Enabled = false;
						btnSave.CssClass = btnCancel.CssClass = btnComplete.CssClass = btnRevert.CssClass = Constansts.CssClass_buttonDisable;
                    }

                    if (order.OrderStatus == OrderInStatus.OrderCancel)
                    {
						btnSave.Enabled = btnComplete.Enabled = btnUpdate.Enabled = btnCancel.Enabled = btnRevert.Enabled = btnReverFirst.Enabled = false;
						btnSave.CssClass = btnComplete.CssClass = btnUpdate.CssClass = btnCancel.CssClass = btnRevert.CssClass = btnReverFirst.CssClass= Constansts.CssClass_buttonDisable;
                    }

                    if (order.OrderStatus == OrderInStatus.Finished)
                    {
						btnSave.Enabled = btnUpdate.Enabled = btnCancel.Enabled = btnRevert.Enabled = false;
						btnSave.CssClass = btnUpdate.CssClass = btnCancel.CssClass = btnRevert.CssClass= Constansts.CssClass_buttonDisable;

                    }

                    if (order.OrderStatus == OrderInStatus.Deliveried)
                    {
						btnSave.Enabled = btnUpdate.Enabled = btnCancel.Enabled = btnComplete.Enabled =btnReverFirst.Enabled= false;
						btnSave.CssClass = btnUpdate.CssClass = btnCancel.CssClass = btnComplete.CssClass = btnReverFirst.CssClass = Constansts.CssClass_buttonDisable;
                    }
                }
            }
            else
            {
                Response.Redirect("~/admin/order/ordermanage.aspx");
            }

			if (Session["PolicyUserMH"] != null)
			{
				this.gridMain.Columns[10].Visible = false;
				this.gridMain.Columns[11].Visible = false;
				this.gridMain.Columns[12].Visible = false;
				this.gridMain.Columns[14].Visible = false;
			}
			else
			{
				this.gridMain.Columns[10].Visible = true;
				this.gridMain.Columns[11].Visible = true;
				this.gridMain.Columns[12].Visible = true;
				this.gridMain.Columns[14].Visible = true;
			}
        }

        #endregion
    }
}