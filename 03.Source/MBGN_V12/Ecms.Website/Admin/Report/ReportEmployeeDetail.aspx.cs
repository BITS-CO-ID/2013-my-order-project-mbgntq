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
	public partial class ReportEmployeeDetail : PageBase
    {
        #region //Declares

        private readonly ReportService _reportService = new ReportService();
		protected string[] Totals = new string[4];
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
			int cusId = Convert.ToInt32(e.CommandArgument);
			var rptModel = ((List<ReportLiabilityModel>)Session["ReportLiability"]).SingleOrDefault(p => p.CustomerId == cusId);

			switch (e.CommandName)
			{
				case "RptDetail":
					Session["rptModel"] = rptModel;
					Response.Redirect(string.Format("~/admin/report/ReportLiabilityDetail.aspx?fromDate={0}&toDate={1}&beforeBalance={2}", Request.QueryString["fromDate"], Request.QueryString["toDate"], rptModel.BeforeBalance.Value));
					break;
			}
		}
        #endregion

        #region //Private methods

        private void LoadData(int type)
        {
            try
            {
                if (type == 1)
                {
					this.lblDetail.Text = string.Format("<b>Chi tiết công nợ của nhân viên:</b> {0} - {1}", Request.QueryString["employeeCode"], Request.QueryString["employeeName"]);
					this.lblDateDetail.Text = string.Format("<b>Trong khoảng thời gian Từ ngày:</b> {0} <b>Tới ngày:</b> {1}", Convert.ToDateTime(Request.QueryString["fromDate"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(Request.QueryString["toDate"]).ToString("dd/MM/yyyy"));

					var listResult = _reportService.ReportLiabilityGet(Request.QueryString["fromDate"],
										string.IsNullOrEmpty(Request.QueryString["toDate"]) == true ? "" : Convert.ToDateTime(Request.QueryString["toDate"]).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"),
										Request.QueryString["employeeCode"],
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
                    Session["ReportLiability"] = listResult;

					Totals[0] = listResult.Sum(x => x.BeforeBalance ?? 0).ToString("N2");
					Totals[1] = listResult.Sum(x => x.TotalCharge ?? 0).ToString("N2");
					//Totals[2] = listResult.Sum(x => x.TotalConfirmedPayment ?? 0).ToString("N2");
					//Totals[3] = (listResult.Sum(x => x.BeforeBalance ?? 0) + listResult.Sum(x => x.TotalCharge ?? 0) - listResult.Sum(x => x.TotalConfirmedPayment ?? 0)).ToString("N2");
                }

                if (type == 2)
                {
                    if (Session["ReportLiability"] != null)
                    {
                        var listResult = (List<ReportLiabilityModel>)Session["ReportLiability"];
						Totals[0] = listResult.Sum(x => x.BeforeBalance ?? 0).ToString("N2");
						Totals[1] = listResult.Sum(x => x.TotalCharge ?? 0).ToString("N2");
						//Totals[2] = listResult.Sum(x => x.TotalConfirmedPayment ?? 0).ToString("N2");
						//Totals[3] = (listResult.Sum(x => x.BeforeBalance ?? 0) + listResult.Sum(x => x.TotalCharge ?? 0) - listResult.Sum(x => x.TotalConfirmedPayment ?? 0)).ToString("N2");

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

		private DataTable ReturnDatatable(List<ReportLiabilityModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Người bán hàng");
			objTable.Columns.Add("Mã khách hàng");
			objTable.Columns.Add("Tên khách hàng");
			objTable.Columns.Add("Dư đầu kỳ");
			objTable.Columns.Add("Phát sinh tăng");
			objTable.Columns.Add("Phát sinh giảm");
			objTable.Columns.Add("Dư cuối kỳ");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{

				var row = objTable.NewRow();
				row["STT"] = index;
				row["Người bán hàng"] = item.EmployeeName;
				row["Mã khách hàng"] = item.CustomerCode;
				row["Tên khách hàng"] = item.CustomerName;
				row["Dư đầu kỳ"] = item.TotalCharge;
				row["Phát sinh tăng"] = item.TotalCharge;
				//row["Phát sinh giảm"] = item.TotalConfirmedPayment;
				row["Dư cuối kỳ"] = item.AfterBalance;

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
			HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<BR><BR><BR>");
			HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
			int columnscount = table.Columns.Count;


			HttpContext.Current.Response.Write("<th colspan='8' style='font-size:14.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO TỔNG HỢP TÀI KHOẢN KHÁCH HÀNG");
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