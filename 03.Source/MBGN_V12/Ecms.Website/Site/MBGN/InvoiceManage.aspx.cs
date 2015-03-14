using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Biz;
using Ecms.Website.Common;
using CommonUtils;
using System.Globalization;

namespace Ecms.Website.Site.MBGN
{
	public partial class InvoiceManage : System.Web.UI.Page
    {
        #region //Declares

		private readonly InvoiceService _invoiceService = new InvoiceService();
		protected double dSumAmount = 0;
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				InitData();
				LoadData(1);
            }
        }

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
			
        }

		protected void gridMain_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
		{

		}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //this.DoSearch(1);
				this.LoadData(1);
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		protected void btnSendPayment_Click(object sender, EventArgs e)
		{
			if (Session["Customer"] == null)
			{
				Response.Redirect("~/site/mbgn/login.aspx");
			}
			Response.Redirect("~/site/mbgn/payment.aspx");
		}

		protected void lblDelete_Click(object sender, CommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "delete":
					// Check:

					// Build
					var result = _invoiceService.InvoiceDelete(e.CommandArgument.ToString(), this);
					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
					}
					break;
			}
		}

		protected void btnExport_Click(object sender, EventArgs e)
		{

		}

		protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            this.LoadData(2);
        }

		protected void btnOK_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/site/mbgn/invoicemanage.aspx");
		}

        #endregion

        #region //Private methods

		private void InitData()
		{
			try
			{
				if (Session["Customer"] == null)
					Response.Redirect("~/site/mbgn/login.aspx");

				var currentDate = DateTime.Now;
				//Lấy ngày đầu tháng
				DateTime fDate = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
				txtFromDate.Text = fDate.ToString("dd/MM/yyyy");
				txtToDate.Text = currentDate.ToString("dd/MM/yyyy");
				
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

					var userCustomer = (UserCustomerModel)Session["Customer"];
					if (userCustomer == null)
					{
						return;
					}
					var listInvoice = _invoiceService.InvoiceGet(
										"",
										this.txtInvoiceCode.Text.Trim(),
										fDate.ToString("yyyy-MM-dd HH:mm:ss"),
										tDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"),
										Convert.ToString(userCustomer.CustomerId.Value),
										"", //this.txtCustomerCode.Text,
										"",
										"",
										"",
										"",
										Const_BusinessCode.Business_201,//ddlPaymentType.SelectedValue,
										"",
										"1",
										this
										).Where(p => !(p.BusinessCode == Const_BusinessCode.Business_208 && p.Status == InvoiceStatus.Pending) // Không hiện thị những thanh toán được phân bổ mà đã revert => pending
														&& p.BusinessCode != Const_BusinessCode.Business_206									// Không lấy những giao dịch hệ thống tự sinh khi kết thúc đơn hàng
														).ToList();

					if (listInvoice.Count == 0)
					{
						gridMain.Visible = false;
						lblError.Visible = true;
						lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
						return;
					}
					dSumAmount = listInvoice.Sum(p => p.SumAmount ?? 0);
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
						dSumAmount = listInvoice.Sum(p => p.SumAmount ?? 0);

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