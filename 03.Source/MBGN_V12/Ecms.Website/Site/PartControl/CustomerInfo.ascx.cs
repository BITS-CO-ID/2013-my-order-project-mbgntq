using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Website.DBServices;
using CommonUtils;

namespace Ecms.Website.Site.PartControl
{
    public partial class CustomerInfo : System.Web.UI.UserControl
    {
        #region //Declares
        private readonly CustomerService _customerService = new CustomerService();
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Session["Customer"] != null)
            {
                this.Visible = true;
                var customer = (UserCustomerModel)Session["Customer"];

				if (customer != null)
                {
					lblCustomerCode.Text = customer.CustomerCode;
					lblUserCode.Text = customer.UserCode.ToUpper();
					lblFullName.Text = customer.CustomerName;
					lblAddress.Text = customer.Address;
					lblEmail.Text = customer.Email;
					lblPhone.Text = customer.Mobile;
					lblBalance.Text = (customer.Balance ?? 0).ToString("N0");
					lblCustomerCodeDelivery.Text = customer.CustomerCodeDelivery;

					// Lấy thông tin số dư

					// Lấy thông tin customer:
					var customerNew = new CustomerService().CustomerList(
									Convert.ToString(customer.CustomerId.Value)
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
									, this.Page
									).SingleOrDefault();

					// Lấy invoice
					var invoices = new InvoiceService().InvoiceGet(
								""
								, ""
								, ""
								, ""
								, Convert.ToString(customer.CustomerId.Value)
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this.Page).Where(p => p.OrderId != null && (p.OrderStatus == OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished)// không lấy những đơn hàng đã hoàn thành
									&& p.Status == InvoiceStatus.Confirm); // chỉ lấy trạng thái =2 đã khớp thanh toán

					var orders = new OrderService().OrderGet(
											""
											, ""
											, ""
											, ""
											, ""
											, ""
											, Convert.ToString(customer.CustomerId.Value)
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
											, this.Page);
					var dRemainAmount = orders.Where(x => x.OrderTypeId != 1 && x.OrderStatus > 3 && x.OrderStatus != 5).Sum(x => (x.RemainAmount ?? 0));

					var balanceFreeze = invoices.Sum(p => p.SumAmount ?? 0);
					lblBalanceAvailable.Text = string.Format("{0} (VNĐ)", ((customerNew.Balance ?? 0) - balanceFreeze).ToString("N0"));

					lblDebit.Text = string.Format("{0} (VNĐ)", (dRemainAmount - ((customerNew.Balance ?? 0) - balanceFreeze)).ToString("N0"));
                }
            }
            else
            {
                this.Visible = false;
            }
        }

        #endregion
    }
}