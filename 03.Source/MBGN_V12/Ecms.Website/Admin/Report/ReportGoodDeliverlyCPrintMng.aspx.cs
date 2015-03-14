using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Class;
using System.Data;
using Ecms.Biz;
using CommonUtils;

namespace Ecms.Website.Admin.Report
{
	public partial class ReportGoodDeliverlyCPrintMng : PageBase
    {
        #region // Declares

        private readonly ReportService _reportService = new ReportService();
        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
                InitData();
                LoadData(1);
            }
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData(2);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(1);

			if (string.IsNullOrEmpty(txtFromDate.Text))
			{
				cldFromDate.SelectedDate = DateTime.Now;
			}

			if (string.IsNullOrEmpty(txtFromDate.Text))
			{
				cldToDate.SelectedDate = DateTime.Now;
			}
        }

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			
		}

		protected void gridMain_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
		{

		}

		protected void lbtnDelete_Click(object sender, CommandEventArgs e)
		{
			if (Session["User"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}

			var rptDeliverId = e.CommandArgument.ToString();
			var result = _reportService.Rpt_DeliverlyConfirmPrintDelete(rptDeliverId, this);
			if (result)
			{
				this.LoadData(1);
			}
		}

		protected void lbtnDetail_Click(object sender, CommandEventArgs e)
		{
			if (Session["User"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			var rptDeliverId = e.CommandArgument.ToString();
			Response.Redirect(string.Format("~/admin/report/reportgooddeliverlycprint.aspx?rptId={}",rptDeliverId));
		}

        #endregion

        #region // Private methods

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
                    string fDate = "";
					string tDate = "";
                    if (ValidData(ref fDate, ref tDate) == false) return;

					var listResult = _reportService.Rpt_DeliverlyConfirmPrintGet(
											fDate
											, tDate
											, ""
 											, ""
											, this.txtCustomerCode.Text
											, ""
											, this.txtDeliverFullName.Text
											, this);

                    if (listResult.Count == 0)
                    {
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

                    gridMain.Visible = true;
                    lblError.Visible = false;
                    gridMain.DataSource = listResult;
                    gridMain.DataBind();
					Session["ReportGoodDeliverlyCPrintMng"] = listResult;
                }

                if (type == 2)
                {
					if (Session["ReportGoodDeliverlyCPrintMng"] != null)
                    {
						var listResult = (List<Rpt_DeliverlyConfirmPrintModel>)Session["ReportGoodDeliverlyCPrintMng"];
                        gridMain.Visible = true;
                        lblError.Visible = false;
                        gridMain.DataSource = listResult;
                        gridMain.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		private bool ValidData(ref string fDate, ref string tDate)
        {
            try
            {
				if (!string.IsNullOrEmpty(txtFromDate.Text))
				{
					CultureInfo viVN = new CultureInfo("vi-VN");
					fDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN).ToString("yyyy-MM-dd");
				}
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            try
            {
				if (!string.IsNullOrEmpty(txtToDate.Text))
				{
					CultureInfo viVN = new CultureInfo("vi-VN");
					tDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", viVN).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
				}
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