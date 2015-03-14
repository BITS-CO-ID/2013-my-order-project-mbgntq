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

namespace Ecms.Website.Admin.Report
{
    public partial class ReportLiability : PageBase
    {
        #region //Declares

        private readonly ReportService _reportService = new ReportService();
		protected double dBeforeBalance = 0;
		protected double dAfterBalance = 0;
		protected double dTotalCharge = 0;
		protected double dBalance = 0;
		protected double dBalanceFreeze = 0;
		protected double dBalanceAvaiable = 0;
		protected double dRemainBalance = 0;
		protected double dRemainBalanceReceivable = 0;

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                // LoadData(1);
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

			int cusId = Convert.ToInt32(e.CommandArgument);
			if (Session["ReportLiability"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			var rptModel = ((List<ReportLiabilityModel>)Session["ReportLiability"]).SingleOrDefault(p => p.CustomerId == cusId);
			DateTime fDate = new DateTime();
			DateTime tDate = new DateTime();
			if (ValidData(ref fDate, ref tDate) == false) return;

			var fromDate = new DateTime(fDate.Year, fDate.Month, fDate.Day, 0, 0, 0);
			var toDate = new DateTime(tDate.Year, tDate.Month, tDate.Day,23, 59, 59);

			switch (e.CommandName)
			{
				case "RptDetail":
					Session["rptModel"] = rptModel;
					Response.Redirect(string.Format("~/admin/report/ReportLiabilityDetail.aspx?fromDate={0}&toDate={1}&beforeBalance={2}", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), rptModel.BeforeBalance.Value));
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

                    var listResult = _reportService.ReportLiabilityGet(fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                        toDate.ToString("yyyy-MM-dd HH:mm:ss"),
																		"",
                                                                        txtCustomerCode.Text.Trim(),
																		this).OrderBy(p => p.CustomerId).ToList();//.Where(p=>p.AfterBalance !=0 || p.BalanceAvaiable !=0 || p.RemainBalanceReceivable !=0).ToList();
                    if (listResult.Count == 0)
                    {
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

					dBeforeBalance = listResult.Sum(p => p.BeforeBalance ?? 0);
					dAfterBalance = listResult.Sum(p => p.AfterBalance ?? 0);
					dTotalCharge = listResult.Sum(p => p.TotalCharge ?? 0);
					dBalance = listResult.Sum(p => p.Balance);
					dBalanceFreeze = listResult.Sum(p => p.BalanceFreeze);
					dBalanceAvaiable = listResult.Sum(p => p.BalanceAvaiable);
					dRemainBalance = listResult.Sum(p => p.RemainBalance);
					dRemainBalanceReceivable = listResult.Sum(p => p.RemainBalanceReceivable);

                    gridMain.Visible = true;
                    lblError.Visible = false;
                    gridMain.DataSource = listResult;
                    gridMain.DataBind();
                    Session["ReportLiability"] = listResult;
                }

                if (type == 2)
                {
                    if (Session["ReportLiability"] != null)
                    {
                        var listResult = (List<ReportLiabilityModel>)Session["ReportLiability"];

						dBeforeBalance = listResult.Sum(p => p.BeforeBalance ?? 0);
						dAfterBalance = listResult.Sum(p => p.AfterBalance ?? 0);
						dTotalCharge = listResult.Sum(p => p.TotalCharge ?? 0);
						dBalance = listResult.Sum(p => p.Balance);
						dBalanceFreeze = listResult.Sum(p => p.BalanceFreeze);
						dBalanceAvaiable = listResult.Sum(p => p.BalanceAvaiable);
						dRemainBalance = listResult.Sum(p => p.RemainBalance);
						dRemainBalanceReceivable = listResult.Sum(p => p.RemainBalanceReceivable);

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

		private DataTable ReturnDatatable(List<ReportLiabilityModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Người bán hàng");
			objTable.Columns.Add("Mã KH");
			objTable.Columns.Add("Tên Đăng nhập");
			objTable.Columns.Add("Tên KH");
			objTable.Columns.Add("Đầu kỳ");
			objTable.Columns.Add("Phát sinh trong kỳ");
			objTable.Columns.Add("Cuối kỳ");
			objTable.Columns.Add("Số dư hiện tại");
			objTable.Columns.Add("Số dư đóng băng");
			objTable.Columns.Add("Số dư khả dụng");
			objTable.Columns.Add("Tổng CN đơn hàng");
			objTable.Columns.Add("CN Phải thu");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{

				var row = objTable.NewRow();
				row["STT"] = index;
				row["Người bán hàng"] = item.EmployeeName;
				row["Mã KH"] = item.CustomerCode;
				row["Tên Đăng nhập"] = item.UserCode;
				row["Tên KH"] = item.CustomerName;
				row["Đầu kỳ"] = Math.Round(item.BeforeBalance??0,CommonUtils.Constansts.NumberRoundMin);
				row["Phát sinh trong kỳ"] = Math.Round(item.TotalCharge??0,CommonUtils.Constansts.NumberRoundMin);
				row["Cuối kỳ"] = Math.Round(item.AfterBalance??0,CommonUtils.Constansts.NumberRoundMin);
				row["Số dư hiện tại"] = Math.Round(item.Balance,CommonUtils.Constansts.NumberRoundMin);
				row["Số dư đóng băng"] = Math.Round(item.BalanceFreeze,CommonUtils.Constansts.NumberRoundMin);
				row["Số dư khả dụng"] = Math.Round(item.BalanceAvaiable,CommonUtils.Constansts.NumberRoundMin);
				row["Tổng CN đơn hàng"] = Math.Round(item.RemainBalance,CommonUtils.Constansts.NumberRoundMin);
				row["CN Phải thu"] = Math.Round(item.RemainBalanceReceivable,CommonUtils.Constansts.NumberRoundMin);

				objTable.Rows.Add(row);
				index += 1;
			}

			//var rowFuter = objTable.NewRow();
			//rowFuter["STT"] = "Tổng";
			////rowFuter["Hoa hồng tạm tính"] = totalT;
			////rowFuter["Hoa hồng thực tính"] = totalNT;
			//objTable.Rows.Add(rowFuter);
			return objTable;
		}

		protected void btnExcel_Click(object sender, EventArgs e)
		{
			var listResult = (List<ReportLiabilityModel>)Session["ReportLiability"];
			var table = ReturnDatatable(listResult);
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ClearContent();
			HttpContext.Current.Response.ClearHeaders();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ContentType = "application/ms-excel";
			HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
			HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");

			HttpContext.Current.Response.Charset = "utf-8";
			//HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
			//sets font
			HttpContext.Current.Response.Write("<font style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<BR><BR><BR>");
			HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:11.0pt; font-family:Calibri; background:white;'> <TR>");
			int columnscount = table.Columns.Count;


			HttpContext.Current.Response.Write("<th colspan='13' style='font-size:14.0pt;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO TỔNG HỢP TÀI KHOẢN - CÔNG NỢ KHÁCH HÀNG");
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			HttpContext.Current.Response.Write("<TR>");
			for (int j = 0; j < columnscount; j++)
			{
				//Makes headers bold
				HttpContext.Current.Response.Write("<Td>");
				HttpContext.Current.Response.Write("<B>");
				HttpContext.Current.Response.Write(table.Columns[j].ColumnName);
				HttpContext.Current.Response.Write("</B>");
				HttpContext.Current.Response.Write("</Td>");
			}
			HttpContext.Current.Response.Write("</TR>");
			foreach (DataRow row in table.Rows)
			{
				HttpContext.Current.Response.Write("<TR>");
				for (int i = 0; i < table.Columns.Count; i++)
				{
					HttpContext.Current.Response.Write("<Td>");
					HttpContext.Current.Response.Write(row[i].ToString());
					HttpContext.Current.Response.Write("</Td>");
				}
				HttpContext.Current.Response.Write("</TR>");
			}
			HttpContext.Current.Response.Write("</Table>");
			HttpContext.Current.Response.Write("</font>");
			HttpContext.Current.Response.Flush();
			HttpContext.Current.Response.End();

		}

        #endregion
    }
}