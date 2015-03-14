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

namespace Ecms.Website.Admin.Report
{
	public partial class ReportEmployee : PageBase
    {
        #region //Declares

        private readonly ReportService _reportService = new ReportService();

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

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData(2);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(1);
        }

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (Session["ReportEmployee"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			var rptModel = ((List<ReportEmployeeModel>)Session["ReportEmployee"]).SingleOrDefault(p => p.EmployeeCode == e.CommandArgument.ToString());
			DateTime fDate = new DateTime();
			DateTime tDate = new DateTime();
			if (ValidData(ref fDate, ref tDate) == false) return;

			var fromDate = new DateTime(fDate.Year, fDate.Month, fDate.Day, 0, 0, 0);
			var toDate = new DateTime(tDate.Year, tDate.Month, tDate.Day,23, 59, 59);

			switch (e.CommandName)
			{
				case "RptDetail":
					Response.Redirect(string.Format("~/admin/report/ReportemployeeDetail.aspx?fromDate={0}&toDate={1}&beforeBalance={2}&employeeCode={3}&employeeName={4}", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), rptModel.BeforeBalance.Value, rptModel.EmployeeCode, rptModel.EmployeeName));
					break;
			}
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

                    var fromDate = new DateTime(fDate.Year, fDate.Month, fDate.Day, 0, 0, 0);
                    var toDate = new DateTime(tDate.Year, tDate.Month, tDate.Day,23, 59, 59);

                    var listResult = _reportService.ReportEmployeeGet(fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                        toDate.ToString("yyyy-MM-dd HH:mm:ss"),
																		txtEmpCode.Text,
																		"",
																		this);
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
                    Session["ReportEmployee"] = listResult;
                }

                if (type == 2)
                {
					if (Session["ReportEmployee"] != null)
                    {
						var listResult = (List<ReportEmployeeModel>)Session["ReportEmployee"];
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