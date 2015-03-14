using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Biz;
using Ecms.Website.Common;
using Ecms.Website.DBServices;

namespace Ecms.Website.Admin.Order
{
    public partial class ConfirmPayment : PageBase
    {
        #region //Declares

        private readonly InvoiceService _invoiceService = new InvoiceService();

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

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[Constansts.SS_INVOICE_ADMIN] != null)
                {
                    var invoice = (InvoiceModel)Session[Constansts.SS_INVOICE_ADMIN];
					// Check xem đơn hàng có ở tình trạng đã giao hàng ko? Nếu đã giao hàng ko dc khớp
					if (invoice.OrderId !=null && invoice.OrderStatus != OrderInStatus.OrderConfirmed && invoice.OrderStatus != OrderInStatus.Finished)
					{
						lblError.Text = "Đơn hàng này không ở tình trạng Xác Nhận hoặc Hoàn thành, không thể khớp thanh toán phân bổ cho đơn hàng này!";
						lblError.Visible = true;
						return;
					}
                    if (invoice != null)
                    {
						if (_invoiceService.ConfirmPayment(
							Convert.ToString(invoice.InvoiceId)
							, Convert.ToString(InvoiceStatus.Confirm)
							, ""
							, txtReplyContent.Text
							, this))
						{
							mtvMain.ActiveViewIndex = 1;
						}
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[Constansts.SS_INVOICE_ADMIN] != null)
                {
                    var invoice = (InvoiceModel)Session[Constansts.SS_INVOICE_ADMIN];
					// Check xem đơn hàng có ở tình trạng đã giao hàng ko? Nếu đã giao hàng cũng ko dc revert
					if (invoice.OrderStatus!=null && invoice.OrderStatus != OrderInStatus.OrderConfirmed && invoice.OrderStatus != OrderInStatus.Finished)
					{
						lblError.Text = "Đơn hàng này không ở tình trạng Xác Nhận hoặc Hoàn thành, không thể khớp thanh toán phân bổ cho đơn hàng này!";
						lblError.Visible = true;
						return;
					}

                    if (invoice != null)
                    {
						if (_invoiceService.CancelPayment(
							Convert.ToString(invoice.InvoiceId)
							, ""
							, txtReplyContent.Text
							, this))
						{
							mtvMain.ActiveViewIndex = 1;
						}
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		protected void btnRevertPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[Constansts.SS_INVOICE_ADMIN] != null)
                {
                    var invoice = (InvoiceModel)Session[Constansts.SS_INVOICE_ADMIN];
					
					if (invoice.OrderStatus == null)
					{
						// Check các thanh toán phân bổ cho các đơn hàng xem có phân bổ nào được giao hàng rồi chưa?
						var lstInvoiceOrderDeliverly = _invoiceService.InvoiceGet(
													""
													, ""
													, ""
													, ""
													, Convert.ToString(invoice.CustomerId)
													, ""
													, ""
													, ""
													, ""
													, ""
													, ""													
													, Convert.ToString(invoice.InvoiceId)
													, ""
													, this).Where(p => p.OrderStatus != null && p.OrderStatus == OrderInStatus.Deliveried && p.Status == InvoiceStatus.Confirm).ToList();

						if (lstInvoiceOrderDeliverly.Count > 0)
						{
							lblError.Text = "Thanh toán này đã được hệ thống phân bổ cho đơn các đơn hàng & có đơn hàng đã được xác nhân GIAO HÀNG không thể hoàn lại thanh toán cho đơn hàng này!";
							lblError.Visible = true;
							return;
						}
					}
					else if (invoice.OrderStatus != null && invoice.OrderStatus == OrderInStatus.Deliveried)
					{
						// Check xem đơn hàng có ở tình trạng đã giao hàng ko? Nếu đã giao hàng cũng ko dc revert
						lblError.Text = "Đơn hàng này đã đước xác nhân GIAO HÀNG, không thể hoàn lại thanh toán cho đơn hàng này!";
						lblError.Visible = true;
						return;
					}

                    if (invoice != null)
                    {
						if (_invoiceService.RevertPayment(
								Convert.ToString(invoice.InvoiceId),
								Convert.ToString(InvoiceStatus.Pending),
								Session["User"] == null ? "" : Session["User"].ToString(),
								txtReplyContent.Text,
								this))
						{
							mtvMain.ActiveViewIndex = 1;
						}
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/paymentmanage.aspx");
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            try
            {
                if (Session[Constansts.SS_INVOICE_ADMIN] != null)
                {
                    var invoice = (InvoiceModel)Session[Constansts.SS_INVOICE_ADMIN];
                    if (invoice != null)
                    {
                        lblCreatedUser.Text = invoice.CustomerName;
                        lblCreatedDate.Text = (invoice.InvoiceDate.Value).ToString("dd/MM/yyyy");
                        lblTotalMoney.Text = (invoice.SumAmount ?? 0).ToString("N2");
                        lblTypePayment.Text = invoice.BusinessName;
						lblOrderNo.Text = invoice.OrderNo;

						if (invoice.Status == InvoiceStatus.Pending)
						{
							btnAccept.Enabled = true;
							btnAccept.CssClass = Constansts.CssClass_button;

							btnReject.Enabled = true;
							btnReject.CssClass = Constansts.CssClass_button;

							btnRevertPayment.Enabled = false;
							btnRevertPayment.CssClass = Constansts.CssClass_buttonDisable;
						}
						else if (invoice.Status == InvoiceStatus.Confirm)
						{
							btnAccept.Enabled = false;
							btnAccept.CssClass = Constansts.CssClass_buttonDisable;

							btnReject.Enabled = false;
							btnReject.CssClass = Constansts.CssClass_buttonDisable;

							btnRevertPayment.Enabled = true;
							btnRevertPayment.CssClass = Constansts.CssClass_button;
						}
						else if (invoice.Status == InvoiceStatus.NotConfirm)
						{
							btnAccept.Enabled = false;
							btnAccept.CssClass = Constansts.CssClass_buttonDisable;

							btnReject.Enabled = false;
							btnReject.CssClass = Constansts.CssClass_buttonDisable;

							btnRevertPayment.Enabled = false;
							btnRevertPayment.CssClass = Constansts.CssClass_buttonDisable;
						}

                        if (invoice.BusinessCode.Equals(Const_BusinessCode.Business_201))
                        {
                            litTitle.Text = "Xác nhận thanh toán";
                            trFromAccount.Visible = true;
                            lblFromAccount.Text = invoice.FromAccount;
                        }
						else if (invoice.BusinessCode.Equals(Const_BusinessCode.Business_202))
						{
							litTitle.Text = "Thông tin hoàn tiền cho khách hàng";
							trRemark.Visible = trFromAccount.Visible = false;
						}
						else if (invoice.BusinessCode.Equals(Const_BusinessCode.Business_203))
						{
							litTitle.Text = "Thông tin MBGN thanh toán mua hàng";
							trCreatedUser.Visible = false;
							trRemark.Visible = trFromAccount.Visible = false;
						}
						else if (invoice.BusinessCode.Equals(Const_BusinessCode.Business_208))
						{
							litTitle.Text = "Xác nhận thanh toán";
							trFromAccount.Visible = true;
							lblFromAccount.Text = invoice.FromAccount;

							btnAccept.Enabled = false;
							btnAccept.CssClass = Constansts.CssClass_buttonDisable;

							btnReject.Enabled = false;
							btnReject.CssClass = Constansts.CssClass_buttonDisable;

							btnRevertPayment.Enabled = false;
							btnRevertPayment.CssClass = Constansts.CssClass_buttonDisable;
						}
                        txtContent.Text = invoice.Remark;
						txtReplyContent.Text = invoice.ReplyContent;                        
                    }
                }
                else
                {
                    Response.Redirect("~/admin/order/paymentmanage.aspx");
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