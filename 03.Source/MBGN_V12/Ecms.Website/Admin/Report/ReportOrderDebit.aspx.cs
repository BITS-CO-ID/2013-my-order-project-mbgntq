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
	public partial class ReportOrderDebit : PageBase
    {
        #region // Declares
		private IUserBiz cService = new UserBiz();
        private readonly ReportService _reportService = new ReportService();
		protected double dOpenBalance = 0;
		protected double dIncreaseBalance = 0;
		protected double dPaidPayBalance = 0;
		protected double dAmountFeeDelayBalance = 0;
		protected double dAmountCalFeeDelayBalance = 0;
		protected double dSumFeeShipBalance = 0;
		protected double dRemainBalance = 0;
        #endregion

        #region // Events

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(1);
        }

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (Session["ReportOrderDebit"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			int cusId = Convert.ToInt32(e.CommandArgument);
			var rptModel = ((List<RptOrderDebitModel>)Session["ReportOrderDebit"]).SingleOrDefault(p => p.CustomerId == cusId);
			DateTime fDate = new DateTime();
			DateTime tDate = new DateTime();
			if (ValidData(ref fDate, ref tDate) == false) return;

			var fromDate = new DateTime(fDate.Year, fDate.Month, fDate.Day, 0, 0, 0);
			var toDate = new DateTime(tDate.Year, tDate.Month, tDate.Day,23, 59, 59);

			switch (e.CommandName)
			{
				case "RptDetail":
					Session["rptModelOrderDebit"] = rptModel;
					Response.Redirect(string.Format("~/admin/report/ReportOrderDebitDetail.aspx?fromDate={0}&toDate={1}", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss")));
					break;
			}

			switch (e.CommandName)
			{
				case "RptPaidPay":
					//Session["rptModelOrderDebit"] = rptModel;
					Response.Redirect(string.Format("~/admin/report/PaymentCustomerDetail.aspx?cusId={0}&fromDate={1}&toDate={2}",cusId, fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss")));
					break;
			}
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

				// load danh sách nhân viên
				ddlEmployee.DataSource = cService.GetUser("", "", "", "1", "").Where(p => p.FlagAdmin == "1");
				ddlEmployee.DataTextField = "UserName";
				ddlEmployee.DataValueField = "UserCode";
				ddlEmployee.DataBind();
				ddlEmployee.Items.Insert(0, new ListItem("-- Chọn NV bán hàng --", ""));
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

                    var listResult = _reportService.ReportOrderDebitGet(
										fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        toDate.ToString("yyyy-MM-dd HH:mm:ss"),
										ddlEmployee.SelectedValue,
                                        txtCustomerCode.Text,
										this);

                    if (listResult.Count == 0)
                    {
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

					dOpenBalance = listResult.Sum(p => p.OpenBalance);
					dIncreaseBalance = listResult.Sum(p => p.IncreaseBalance);
					dPaidPayBalance= listResult.Sum(p => p.PaidPayBalance);
					dAmountFeeDelayBalance = listResult.Sum(p => p.AmountFeeDelay);
					dAmountCalFeeDelayBalance = listResult.Sum(p => p.AmountCalFeeDelay);
					dSumFeeShipBalance = listResult.Sum(p => p.SumFeeShip);
					dRemainBalance = listResult.Sum(p => p.RemainBalance);

                    gridMain.Visible = true;
                    lblError.Visible = false;
                    gridMain.DataSource = listResult;
                    gridMain.DataBind();
					Session["ReportOrderDebit"] = listResult;
                }

                if (type == 2)
                {
					if (Session["ReportOrderDebit"] != null)
                    {
						var listResult = (List<RptOrderDebitModel>)Session["ReportOrderDebit"];
						dOpenBalance = listResult.Sum(p => p.OpenBalance);
						dIncreaseBalance = listResult.Sum(p => p.IncreaseBalance);
						dPaidPayBalance = listResult.Sum(p => p.PaidPayBalance);
						dAmountFeeDelayBalance = listResult.Sum(p => p.AmountFeeDelay);
						dAmountCalFeeDelayBalance = listResult.Sum(p => p.AmountCalFeeDelay);
						dSumFeeShipBalance = listResult.Sum(p => p.SumFeeShip);
						dRemainBalance = listResult.Sum(p => p.RemainBalance);

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

		private DataTable ReturnDatatable(List<RptOrderDebitModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			
			objTable.Columns.Add("Mã khách hàng");
			objTable.Columns.Add("Tên đăng nhập");
			objTable.Columns.Add("Tên khách hàng");
			objTable.Columns.Add("CN đầu kỳ");
			objTable.Columns.Add("Phát sinh trong kỳ");
			objTable.Columns.Add("CN cuối kỳ");
			objTable.Columns.Add("Đã TT cuối kỳ");
			objTable.Columns.Add("TT vận chuyển CK");
			objTable.Columns.Add("TT Tính phí trả chậm");
			objTable.Columns.Add("Phí trả chậm Cuối kỳ");
			objTable.Columns.Add("CN còn lại");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{

				var row = objTable.NewRow();
				row["STT"] = index;
				row["Mã khách hàng"] = item.CustomerCode;
				row["Tên đăng nhập"] = item.UserCode;
				row["Tên khách hàng"] = item.CustomerName;
				row["CN đầu kỳ"] = Math.Round(item.OpenBalance, Constansts.NumberRoundMin);
				row["Phát sinh trong kỳ"] = Math.Round(item.IncreaseBalance, Constansts.NumberRoundMin);
				row["CN cuối kỳ"] = Math.Round(item.AfterBalance??0, Constansts.NumberRoundMin); 
				row["Đã TT cuối kỳ"] = Math.Round(item.PaidPayBalance, Constansts.NumberRoundMin);
				row["TT vận chuyển CK"] = Math.Round(item.SumFeeShip, Constansts.NumberRoundMin); 
				row["TT Tính phí trả chậm"] = Math.Round(item.AmountCalFeeDelay, Constansts.NumberRoundMin); 
				row["Phí trả chậm Cuối kỳ"] = Math.Round(item.AmountFeeDelay, Constansts.NumberRoundMin); 
				row["CN còn lại"] = Math.Round(item.RemainBalance, Constansts.NumberRoundMin); 

				objTable.Rows.Add(row);
				index += 1;
			}

			dOpenBalance = Math.Round(lstToTable.Sum(p => p.OpenBalance), Constansts.NumberRoundMin); 
			dIncreaseBalance = Math.Round(lstToTable.Sum(p => p.IncreaseBalance), Constansts.NumberRoundMin); 
			dPaidPayBalance = Math.Round(lstToTable.Sum(p => p.PaidPayBalance), Constansts.NumberRoundMin); 
			dSumFeeShipBalance = Math.Round(lstToTable.Sum(p => p.SumFeeShip), Constansts.NumberRoundMin); 
			dAmountCalFeeDelayBalance = Math.Round(lstToTable.Sum(p => p.AmountCalFeeDelay), Constansts.NumberRoundMin); 
			dAmountFeeDelayBalance = Math.Round(lstToTable.Sum(p => p.AmountFeeDelay), Constansts.NumberRoundMin);
			dRemainBalance = Math.Round(lstToTable.Sum(p => p.RemainBalance), Constansts.NumberRoundMin); 

			var rowFuter = objTable.NewRow();
			rowFuter["STT"] = "Tổng";
			rowFuter["CN đầu kỳ"] = dOpenBalance;
			rowFuter["Phát sinh trong kỳ"] = dIncreaseBalance;
			rowFuter["CN cuối kỳ"] = dOpenBalance + dIncreaseBalance;
			rowFuter["Đã TT cuối kỳ"] = dPaidPayBalance;
			rowFuter["TT vận chuyển CK"] = dSumFeeShipBalance;
			rowFuter["TT Tính phí trả chậm"] = dAmountCalFeeDelayBalance;
			rowFuter["Phí trả chậm Cuối kỳ"] = dAmountFeeDelayBalance;
			rowFuter["CN còn lại"] = dRemainBalance;
			objTable.Rows.Add(rowFuter);
			return objTable;
		}

		protected void btnExcel_Click(object sender, EventArgs e)
		{
			var listResult = (List<RptOrderDebitModel>)Session["ReportOrderDebit"];
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

			HttpContext.Current.Response.Write("<th colspan='12' style='font-size:14.0pt;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO TỔNG HỢP CÔNG NỢ ĐƠN HÀNG");
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