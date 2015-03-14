using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Biz;

namespace Ecms.Website.Admin.Order
{
    public partial class PaymentManage : PageBase
    {
        #region //Declares

        private readonly InvoiceService _invoiceService = new InvoiceService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                //LoadData(1);
            }
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData(2);
        }

        protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Session[Constansts.SS_INVOICE_LIST_ADMIN] == null)
            {
                Response.Redirect("~/admin/order/paymentmanage.aspx");
            }

            int invoiceId = Convert.ToInt32(e.CommandArgument);
            var invoices = (List<InvoiceModel>)Session[Constansts.SS_INVOICE_LIST_ADMIN];
            var invoiceFirst = invoices.Where(p => p.InvoiceId == invoiceId).FirstOrDefault();
            Session[Constansts.SS_INVOICE_ADMIN] = invoiceFirst;
            switch (e.CommandName)
            {
                case "ConfirmPayment":
                    Response.Redirect("~/admin/order/confirmpayment.aspx");
                    break;
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(1);
        }

        protected void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/payment.aspx");
        }

        #endregion

        #region //Private methods

        private void InitData()
        {
            try
            {
                if (Session["User"] == null)
                    Response.Redirect("~/admin/security/login.aspx");

                var currentDate = DateTime.Now;
                //Lấy ngày đầu tháng
                DateTime fDate = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
                txtFromDate.Text = fDate.ToString("dd/MM/yyyy");
                txtToDate.Text = currentDate.ToString("dd/MM/yyyy");
                //Set data vào calendar conntrol
                cldFromDate.SelectedDate = fDate;
                cldToDate.SelectedDate = currentDate;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private void LoadData(int type)
        {
			try
			{
				if (type == 1)
				{
					DateTime fDate = new DateTime();
					DateTime tDate = new DateTime();
					if (ValidData(ref fDate, ref tDate) == false)
						return;

					var listInvoice = _invoiceService.InvoiceGet(
										"",
										this.txtInvoiceCode.Text,
										fDate.ToString("yyyy-MM-dd HH:mm:ss"),
										tDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"),
										"",
										this.txtCustomerCode.Text,
										"",
										ddlStatus.SelectedValue,
										"",
										"",
										ddlPaymentType.SelectedValue,
										"",
										"1",
										this
										).Where(p => !(p.BusinessCode == Const_BusinessCode.Business_208 && p.Status == InvoiceStatus.Pending) // Không hiện thị những thanh toán được phân bổ mà đã revert => pending
														//&& (p.BusinessCode == Const_BusinessCode.Business_209 && p.Status == InvoiceStatus.Pending)
														&& p.BusinessCode != Const_BusinessCode.Business_206									// Không lấy những giao dịch hệ thống tự sinh khi kết thúc đơn hàng
														).ToList();

					if (listInvoice.Count == 0)
					{
						gridMain.Visible = false;
						lblError.Visible = true;
						lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
						return;
					}

					gridMain.Visible = true;
					lblError.Visible = false;
					gridMain.DataSource = listInvoice;
					gridMain.DataBind();
					Session[Constansts.SS_INVOICE_LIST_ADMIN] = listInvoice;
				}

				if (type == 2)
				{
					if (Session[Constansts.SS_INVOICE_LIST_ADMIN] != null)
					{
						var listInvoice = (List<InvoiceModel>)Session[Constansts.SS_INVOICE_LIST_ADMIN];
						gridMain.DataSource = listInvoice;
						gridMain.DataBind();
					}
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

        private bool ValidData(ref DateTime fDate, ref DateTime tDate)
        {
            if (string.IsNullOrEmpty(txtFromDate.Text))
            {
                lblError.Text = "Từ ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                fDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtToDate.Text))
            {
                lblError.Text = "Đến ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                tDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Đến ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

        #endregion
    }
}