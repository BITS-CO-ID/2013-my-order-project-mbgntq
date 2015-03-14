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
using Ecms.Biz.Entities;

namespace Ecms.Website.Admin.Order
{
	public partial class PaymentForward : PageBase
    {
        #region //Declares

        private readonly InvoiceService _invoiceService = new InvoiceService();
		private readonly OrderService _orderService = new OrderService();
		private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null || string.IsNullOrEmpty(Request.QueryString["orderId"]))
                    Response.Redirect("~/admin/security/login.aspx");

                LoadData();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {

				lblError.Text = "";
				lblError.Visible = false;
				// Validate
				if(string.IsNullOrEmpty(txtAmount.Text))
				{
					lblError.Text = "Bạn chưa nhập số tiền chuyển thanh toán cho đơn hàng";
					lblError.Visible = true;
					return ;
				}
				double amount=0;
				try
				{
					amount = Convert.ToDouble(txtAmount.Text.Trim());
				}
				catch
				{
					lblError.Text = "Số tiền thanh toán không đúng";
					lblError.Visible = true;
					return;
				}

				if (amount> Convert.ToDouble(hdBalanceAvaibility.Value))
				{
					lblError.Text = "Số dư khả dụng không đủ để chuyển cho đơn hàng này, hãy nạp thềm tiền vào tài khoản KH";
					lblError.Visible = true;
					return;
				}


				var invoice = new Invoice
					{
						BusinessCode = Const_BusinessCode.Business_207
						,
						CreatedDate = DateTime.Now
						,
						CustomerId = Convert.ToInt32(hdCustomerId.Value)
						,
						OrderId = Convert.ToInt32(hdOrderId.Value)
						,
						InvoiceDate = DateTime.Now
						,
						Remark = string.Format("Chuyển thanh toán từ tài khoản KH cho đơn hàng OrderId={0}", hdOrderId.Value)
						,
						Status = 2
					};

				var ivoiceDetail = new InvoiceDetail
				{
					Amount=amount
				};

				invoice.InvoiceDetails.Add(ivoiceDetail);

				var invoiceNew = _invoiceService.InvoiceForwardCreate(invoice, this);

				if (invoiceNew != null)
				{
					mtvMain.ActiveViewIndex = 1;					
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }		

        protected void btnOk_Click(object sender, EventArgs e)
        {
			Response.Redirect("~/admin/order/ordermanage.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            try
            {
				// lấy thông tin order

				var order = _orderService.OrderGet(
							Request.QueryString["orderId"].ToString()
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, this
							).SingleOrDefault();

				hdOrderId.Value = Convert.ToString(order.OrderId);
				lblOrderNo.Text = order.OrderNo;
				lblSumAmount.Text = (order.SumAmount??0).ToString("N2");
				lblPaidAmount.Text = (order.TotalPayAmountNormal).ToString("N2");
				lblRemainAmount.Text = (order.RemainAmount??0).ToString("N2");

				// Lấy thông tin customer:

				var customer = _customerService.CustomerList(
								Convert.ToString(order.CustomerId)
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this
								).SingleOrDefault();

				this.hdCustomerId.Value =Convert.ToString(customer.CustomerId);
				this.lblCustomerCode.Text = customer.CustomerCode;
				this.lblCustomerName.Text = customer.CustomerName;
				this.lblCurrentBalance.Text = (customer.Balance??0).ToString("N2");

				// Lấy thông tin số dư:

				var invoices = _invoiceService.InvoiceGet(
							""
							, ""
							, ""
							, ""
							, Convert.ToString(order.CustomerId)
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, ""
							, this).Where(p => p.OrderId != null && (p.OrderStatus==OrderInStatus.OrderConfirmed || p.OrderStatus==OrderInStatus.OrderCancel)// không lấy những đơn hàng đã hoàn thành
								&& p.Status ==2); // chỉ lấy trạng thái =2 đã khớp thanh toán

				var balanceFreeze = invoices.Sum(p => p.SumAmount??0);
				lblBalanceFreeze.Text = balanceFreeze.ToString("N2");
				lblBalanceAvaiabilyty.Text = hdBalanceAvaibility.Value = ((customer.Balance??0) - balanceFreeze).ToString("N2");

            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion
    }
}