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
	public partial class ReportStockOInOut : PageBase
    {
        #region // Declares

        private readonly ReportService _reportService = new ReportService();
		protected double dBalanceOpenQuantity = 0;
		protected double dBalanceOpenPrice = 0;
		protected double dStockInQuantity = 0;
		protected double dStockInPrice = 0;
		protected double dStockOutQuantity = 0;
		protected double dStockOutPrice = 0;
		protected double dBalanceQuantity = 0;
		protected double dBalancePrice = 0;

        #endregion

        #region // Events

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

			//int cusId = Convert.ToInt32(e.CommandArgument);
			//if (Session["ReportStockOInOut"] == null)
			//{
			//    Response.Redirect("~/admin/security/login.aspx");
			//}
			//var rptModel = ((List<ReportLiabilityModel>)Session["ReportStockOInOut"]).SingleOrDefault(p => p.CustomerId == cusId);
			//DateTime fDate = new DateTime();
			//DateTime tDate = new DateTime();
			//if (ValidData(ref fDate, ref tDate) == false) return;

			//var fromDate = new DateTime(fDate.Year, fDate.Month, fDate.Day, 0, 0, 0);
			//var toDate = new DateTime(tDate.Year, tDate.Month, tDate.Day,23, 59, 59);

			//switch (e.CommandName)
			//{
			//    case "RptDetail":
			//        Session["rptModel"] = rptModel;
			//        Response.Redirect(string.Format("~/admin/report/ReportLiabilityDetail.aspx?fromDate={0}&toDate={1}&beforeBalance={2}", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), rptModel.BeforeBalance.Value));
			//        break;
			//}
		}
        #endregion

        #region // Private methods

        private void InitData()
        {
            try
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
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

					var listResult = _reportService.ReportStockOInOutGet(fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                        toDate.ToString("yyyy-MM-dd HH:mm:ss"),
																		"",
                                                                        txtProductCode.Text.Trim(),
																		this).OrderBy(p => p.ProductCode).ToList();//.Where(p=>p.AfterBalance !=0 || p.BalanceAvaiable !=0 || p.RemainBalanceReceivable !=0).ToList();
                    if (listResult.Count == 0)
                    {
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

					dBalanceOpenQuantity = listResult.Sum(p => p.BalanceOpenQuantity);
					dBalanceOpenPrice = listResult.Sum(p => p.BalanceOpenPrice);
					dStockInQuantity = listResult.Sum(p => p.StockInQuantity);
					dStockInPrice = listResult.Sum(p => p.StockInPrice);
					dStockOutQuantity = listResult.Sum(p => p.StockOutQuantity);
					dStockOutPrice = listResult.Sum(p => p.StockOutPrice);
					dBalanceQuantity = listResult.Sum(p => p.BalanceQuantity);
					dBalancePrice = listResult.Sum(p => p.BalancePrice);

                    gridMain.Visible = true;
                    lblError.Visible = false;
                    gridMain.DataSource = listResult;
                    gridMain.DataBind();
					Session["ReportStockOInOut"] = listResult;
                }

                if (type == 2)
                {
					if (Session["ReportStockOInOut"] != null)
                    {
						var listResult = (List<Rpt_StockOInOut>)Session["ReportStockOInOut"];

						dBalanceOpenQuantity = listResult.Sum(p => p.BalanceOpenQuantity);
						dBalanceOpenPrice = listResult.Sum(p => p.BalanceOpenPrice);
						dStockInQuantity = listResult.Sum(p => p.StockInQuantity);
						dStockInPrice = listResult.Sum(p => p.StockInPrice);
						dStockOutQuantity = listResult.Sum(p => p.StockOutQuantity);
						dStockOutPrice = listResult.Sum(p => p.StockOutPrice);
						dBalanceQuantity = listResult.Sum(p => p.BalanceQuantity);
						dBalancePrice = listResult.Sum(p => p.BalancePrice);

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

		private DataTable ReturnDatatable(List<Rpt_StockOInOut> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Mã sản phẩm");
			objTable.Columns.Add("Tên sản phẩm");
			objTable.Columns.Add("SL Đầu kỳ");
			objTable.Columns.Add("Giá trị đầu kỳ");
			objTable.Columns.Add("Số lượng nhập");
			objTable.Columns.Add("Giá trị nhập");
			objTable.Columns.Add("SL xuất");
			objTable.Columns.Add("Giá trị xuất");
			objTable.Columns.Add("Số lượng tồn");
			objTable.Columns.Add("Giá trị tồn");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{

				var row = objTable.NewRow();
				row["STT"] = index;
				row["Mã sản phẩm"] = item.ProductCode;
				row["Tên sản phẩm"] = item.ProductName;
				row["SL Đầu kỳ"] = item.BalanceOpenQuantity;
				row["Giá trị đầu kỳ"] = item.BalanceOpenPrice;
				row["Số lượng nhập"] = item.StockInQuantity;
				row["Giá trị nhập"] = item.StockInPrice;
				row["SL xuất"] = item.StockOutQuantity;
				row["Giá trị xuất"] = item.StockOutPrice;
				row["Số lượng tồn"] = item.BalanceQuantity;
				row["Giá trị tồn"] = item.BalancePrice;
				

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
			var listResult = (List<Rpt_StockOInOut>)Session["ReportStockOInOut"];
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


			HttpContext.Current.Response.Write("<th colspan='11' style='font-size:14.0pt;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO TỔNG HỢP NHẬP XUẤT TỒN KHO");
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