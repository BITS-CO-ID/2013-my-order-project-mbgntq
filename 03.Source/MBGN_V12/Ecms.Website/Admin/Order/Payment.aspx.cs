using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz;
using CommonUtils;

namespace Ecms.Website.Admin.Order
{
    public partial class Payment : PageBase
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();
        private readonly CustomerService _customerService = new CustomerService();
        private readonly OrderService _orderService = new OrderService();
        private readonly InvoiceService _invoiceService = new InvoiceService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}

                LoadData();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
			try
			{
				var paymentType = ddlPaymentType.SelectedValue;
				var bank = ddlBank.SelectedValue;
				var accountNo = txtAccountNo.Text;
				var paymentUser = txtCreatedUser.Text;
				var paymentDate = new DateTime();
				var amount = txtAmount.Text;

				if (ValidData(paymentType, bank, accountNo, paymentUser, ref paymentDate, amount) == false)
					return;

				#region // Check người thanh toán:
				var customer = _customerService.CustomerList("", "", "", "", "", "", "", "", "", "", "", this).Where(x => x.CustomerCode == paymentUser || x.UserCode == paymentUser).ToList();
				if (customer == null || customer.Count == 0)
				{
					lblError.Text = "Người thanh toán không đúng!";
					lblError.Visible = true;
					return;
				}
				var customerId = customer.SingleOrDefault().CustomerId;
				// Nếu là Business 203: MBGN mua hàng từ đối tác
				if (paymentType.Equals(Const_BusinessCode.Business_203))
				{
					customerId =Constansts.CustomerIdAccount_MBGN; // Tài khoản MBGN
				}				

				#endregion

				#region // Check có thanh toán cho Order nào không?
				//var orderId = txtOrderNo.Text.Trim();
				//if (!string.IsNullOrEmpty(orderId))
				//{
				//    var orders = _orderService.OrderOnlyGet("", txtOrderNo.Text.Trim(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", this);

				//    if (orders == null || orders.Count == 0)
				//    {
				//        lblError.Text = "Số đơn hàng vừa nhập không đúng!";
				//        lblError.Visible = true;
				//        return;
				//    }
				//    // Check số tiền đã thanh toán cho đơn hàng này > tổng tiền cần thanh toán chưa?

				//    // Check xem đơn hàng có ở tình trạng đã giao hàng ko?
				//    var order = orders.SingleOrDefault();
				//    if (order.OrderStatus != OrderInStatus.OrderConfirmed && order.OrderStatus != OrderInStatus.Finished)
				//    {
				//        lblError.Text = "Đơn hàng này không ở tình trạng Xác Nhận hoặc Hoàn thành, không thể lập hóa đơn phân bổ thanh toán cho đơn hàng này!";
				//        lblError.Visible = true;
				//        return;
				//    }
				//    orderId = Convert.ToString(order.OrderId);
				//}
				#endregion
				if (paymentType.Equals(Const_BusinessCode.Business_209))
				{
					// Phân bổ thanh toán từ số dư khả dụng
					if (customer.SingleOrDefault().BalanceAvaiable <= 0)
					{
						lblError.Text = "Số dư khả dụng không đủ để phân bổ thanh toán!";
						lblError.Visible = true;
						return;
					}

					// Check OrderNo
					if (string.IsNullOrEmpty(txtOrderNo.Text))
					{
						lblError.Text = "Chưa nhập Mã đơn hàng!";
						lblError.Visible = true;
						return;
					}

					var lstOrders = _orderService.OrderGet(
									""
									, txtOrderNo.Text.Trim()
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
									);

					if (lstOrders == null || lstOrders.Count == 0)
					{
						lblError.Text = "Mã đơn hàng vừa nhập không đúng!";
						lblError.Visible = true;
						return;
					}
					// Check số tiền đã thanh toán cho đơn hàng này > tổng tiền cần thanh toán chưa?

					// Check xem đơn hàng có ở tình trạng đã giao hàng ko?
					var order = lstOrders.SingleOrDefault();
					if (order.OrderStatus != OrderInStatus.OrderConfirmed && order.OrderStatus != OrderInStatus.Finished)
					{
						lblError.Text = "Đơn hàng này không ở tình trạng Xác Nhận hoặc Hoàn thành, không thể lập hóa đơn phân bổ thanh toán cho đơn hàng này!";
						lblError.Visible = true;
						return;
					}
					if (Math.Round(order.RemainAmount??0, 0) <= 0)
					{
						lblError.Text = "Công nợ đơn hàng này đã hết. không cần phân bổ thanh toán cho đơn hàng này!";
						lblError.Visible = true;
						return;
					}

					double amountPayment = 0.0;
					if (customer.FirstOrDefault().BalanceAvaiable > order.RemainAmount)
					{
						amountPayment = order.RemainAmount ?? 0;
					}
					else
					{
						amountPayment = customer.FirstOrDefault().BalanceAvaiable;
					}

					var invoice = _invoiceService.InvoiceConfirmPayment01(
									Convert.ToString(customerId)
									, ""
									, ""
									, Convert.ToString(order.OrderId)
									, Math.Round(amountPayment,0).ToString()
									, txtContent.Text
									, paymentType
									, ""
									, paymentDate.ToString("yyyy-MM-dd")
									, this);

					if (invoice)
					{
						mtvMain.ActiveViewIndex = 1;
					}
				}
				else
				{
					var invoice = _invoiceService.InvoiceConfirmPayment(
									Convert.ToString(customerId)
									, bank
									, accountNo
									, ""
									, amount.Trim().Replace(",", "")
									, txtContent.Text
									, paymentType
									, ""
									, paymentDate.ToString("yyyy-MM-dd")
									, this);

					if (invoice)
					{
						mtvMain.ActiveViewIndex = 1;
					}
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentType.SelectedValue.Equals(Const_BusinessCode.Business_201))
            {
                trAccountNo.Visible = trBank.Visible = trCreatedUser.Visible = true;
            }

			if (ddlPaymentType.SelectedValue.Equals(Const_BusinessCode.Business_202))
            {
                trAccountNo.Visible = trBank.Visible = false;
                trCreatedUser.Visible = true;
            }

			if (ddlPaymentType.SelectedValue.Equals(Const_BusinessCode.Business_203))
            {
                trAccountNo.Visible = trBank.Visible = trCreatedUser.Visible = false;
            }

			if (ddlPaymentType.SelectedValue.Equals(Const_BusinessCode.Business_209))
			{
				this.ddlBank.Enabled = false;
				this.txtAccountNo.Enabled = false;
				this.txtAmount.Enabled = false;
				this.txtOrderNo.Enabled = true;
				this.btnCheckAvaiable.Visible = true;
				this.btnOrderDebit.Visible = true;
			}
			else
			{
				this.ddlBank.Enabled = true;
				this.txtAccountNo.Enabled = true;
				this.txtAmount.Enabled = true;
				this.txtOrderNo.Enabled = false;
				this.btnCheckAvaiable.Visible = false;
				this.btnOrderDebit.Visible = false;				
			}
        }		

		protected void btnCheckAvaiable_Click(object sender, EventArgs e)
		{
			lblError.Visible = false;
			var lstCustomer = _customerService.CustomerList("", "", "", "", "", "", "", "", "", "", "", this).Where(x => x.CustomerCode == txtCreatedUser.Text.Trim() || x.UserCode == txtCreatedUser.Text.Trim()).ToList();
			if (lstCustomer == null || lstCustomer.Count == 0)
			{
				lblError.Text = "Người thanh toán không đúng!";
				lblError.Visible = true;
				return;
			}
			var customer = lstCustomer.SingleOrDefault();
			this.lblBalanceAvaiable.Text = string.Format("{0} VNĐ", (customer==null?0:customer.BalanceAvaiable).ToString("N0"));
		}

		protected void btnOrderDebit_Click(object sender, EventArgs e)
		{
			lblError.Visible = false;
			if (!ddlPaymentType.SelectedValue.Equals(Const_BusinessCode.Business_209))
			{
				lblError.Text = "Chưa chọn đúng loại thanh toán là phân bổ từ số dư khả dụng";
				lblError.Visible = true;
				return;
			}
			// 1. Kiểm tra KH
			var lstCustomer = _customerService.CustomerList("", "", "", "", "", "", "", "", "", "", "", this).Where(x => x.CustomerCode == txtCreatedUser.Text.Trim() || x.UserCode == txtCreatedUser.Text.Trim()).ToList();
			if (lstCustomer == null || lstCustomer.Count == 0)
			{
				lblError.Text = "Người thanh toán không đúng!";
				lblError.Visible = true;
				return;
			}
			var customer = lstCustomer.SingleOrDefault();

			// 2. Kiểm tra đơn hàng
			if (string.IsNullOrEmpty(txtOrderNo.Text))
			{
				lblError.Text = "Chưa nhập Mã đơn hàng!";
				lblError.Visible = true;
				return;
			}
			var lstOrders = _orderService.OrderGet(
									""
									, txtOrderNo.Text.Trim()
									, ""
									, ""
									, ""
									, ""
									, Convert.ToString(customer.CustomerId)
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
									);

			if (lstOrders == null || lstOrders.Count == 0)
			{
				lblError.Text = "Mã đơn hàng vừa nhập không đúng, hoặc không phải của KH này!";
				lblError.Visible = true;
				return;
			}
			var order = lstOrders.SingleOrDefault();
			this.lblOrderDebit.Text = string.Format("{0} VNĐ", (order == null ? 0 : order.RemainAmount ?? 0).ToString("N0"));
			if (customer.BalanceAvaiable > order.RemainAmount)
			{
				this.txtAmount.Text = Convert.ToString(order.RemainAmount ?? 0);
			}
			else
			{
				this.txtAmount.Text = Convert.ToString(customer.BalanceAvaiable);
			}
		}
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/paymentmanage.aspx");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/paymentmanage.aspx");
        }

        #endregion

        #region //Private Methods

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

				if (ddlPaymentType.SelectedValue.Equals(Const_BusinessCode.Business_201))
				{
					trAccountNo.Visible = trBank.Visible =  trCreatedUser.Visible = true;
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private bool ValidData(
				string paymentType
				, string bank
				, string accountNo
				, string paymentUser
				, ref DateTime paymentDate
				, string amount)
        {
            if (string.IsNullOrEmpty(paymentType))
            {
                lblError.Text = "Bạn chưa chọn loại thanh toán!";
                lblError.Visible = true;
                return false;
            }

            if (paymentType.Equals(Const_BusinessCode.Business_201) && string.IsNullOrEmpty(bank))
            {
                lblError.Text = "Bạn chưa chọn ngân hàng!";
                lblError.Visible = true;
                return false;
            }

            if ((paymentType.Equals(Const_BusinessCode.Business_201) || paymentType.Equals(Const_BusinessCode.Business_202)) && string.IsNullOrEmpty(paymentUser))
            {
                lblError.Text = "Bạn chưa nhập người thanh toán!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(amount))
            {
                lblError.Text = "Bạn chưa nhập số tiền!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtPaymentDate.Text))
            {
                lblError.Text = "Ngày thanh toán không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                paymentDate = DateTime.ParseExact(txtPaymentDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Ngày thanh toán không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }
            return true;
        }

        #endregion
    }
}