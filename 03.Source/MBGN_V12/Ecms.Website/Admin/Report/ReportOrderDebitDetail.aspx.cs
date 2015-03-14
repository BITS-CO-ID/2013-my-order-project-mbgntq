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
	public partial class ReportOrderDebitDetail : PageBase
    {
        #region //Declares
		private readonly CustomerService _customerService = new CustomerService();
		private readonly OrderService _orderService = new OrderService();
		private readonly InvoiceService _invoiceService = new InvoiceService();
		protected string[] Totals = new string[6];
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

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			var param = e.CommandArgument.ToString().Split('|');
			switch (e.CommandName)
			{
				case "RptPaidPay":
					Response.Redirect(string.Format("~/admin/report/PaymentCustomerDetail.aspx?cusId={0}&fromDate={1}&toDate={2}&orderNo={3}", param[0], Request.QueryString["fromDate"], Request.QueryString["toDate"], param[1]));
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
					if (Session["rptModelOrderDebit"] == null)
					{
						Response.Redirect("~/admin/security/login.aspx");
					}
					var rptModel = (RptOrderDebitModel)Session["rptModelOrderDebit"];
					// Lấy thông tin customer:

					var customer = _customerService.CustomerList(
									Convert.ToString(rptModel.CustomerId)
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
									).SingleOrDefault();
					this.lblCurrentBalance.Text = (customer.Balance ?? 0).ToString("N0");

					// Lấy thông tin số dư:

					var invoices = _invoiceService.InvoiceGet(
								""
								, ""
								, ""
								, ""
								, Convert.ToString(rptModel.CustomerId)
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this).Where(p => p.OrderId != null && (p.OrderStatus == OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished)// không lấy những đơn hàng đã hoàn thành
									&& p.Status == InvoiceStatus.Confirm); // chỉ lấy trạng thái =2 đã khớp thanh toán

					var balanceFreeze = invoices.Sum(p => p.SumAmount ?? 0);
					lblBalanceFreeze.Text = balanceFreeze.ToString("N0");
					lblBalanceAvaiabilyty.Text = ((customer.Balance ?? 0) - balanceFreeze).ToString("N0");

					
					this.lblDetail.Text = string.Format("Chi tiết công nợ Đơn hàng của khách hàng: <b>{0} - {1}</b>", rptModel.CustomerCode, rptModel.CustomerName);
					this.lblDateDetail.Text = string.Format("Trong khoảng thời gian Từ ngày: <b>{0}</b> Tới ngày:<b>{1}</b> ", Convert.ToDateTime(Request.QueryString["fromDate"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(Request.QueryString["toDate"]).ToString("dd/MM/yyyy"));

					this.lblOpenBalance.Text = string.Format("{0}", rptModel.OpenBalance.ToString("N0"));
					this.lblIncreaseBalance.Text = string.Format("{0}", rptModel.IncreaseBalance.ToString("N0"));
					this.lblFinishBlance.Text = string.Format("{0}", (rptModel.OpenBalance + rptModel.IncreaseBalance).ToString("N0"));
					this.lblPaiPayBalance.Text = string.Format("{0}", rptModel.PaidPayBalance.ToString("N0"));
					this.lblRemainBalance.Text = string.Format("{0}", (rptModel.RemainBalance).ToString("N0"));

					this.lblCusDebit.Text = string.Format("{0}", (rptModel.RemainBalance - ((customer.Balance ?? 0) - balanceFreeze)).ToString("N0"));

					var list = _orderService.OrderGet(""
									, ""
									, ""
									, ""
									, Request.QueryString["fromDate"]
									, Request.QueryString["toDate"]
									, Convert.ToString(rptModel.CustomerId)
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
									, this).Where(p=>p.OrderStatus==OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished || p.OrderStatus == OrderInStatus.Deliveried).ToList();


					var listResult = (from p in list
									  select new RptOrderDebitDetailModel
									 {
										 CustomerId=p.CustomerId.Value
										 , OrderId=p.OrderId
										 , OrderNo=p.OrderNo
										 , OrderStatus=p.OrderStatus??0
										 , OrderStatusText= p.OrderStatusText
										 , OrderDate=p.OrderDate
										 , RemainAmount=p.RemainAmount??0
										 , SumAmount= p.OrderStatus==OrderInStatus.OrderCancel?0: p.SumAmount??0
										 , AmountFeeDelay = p.AmountFeeDelay
										 , TotalPayAmountNormal=p.TotalPayAmountNormal
										 , AmountCalFeeDelay = p.AmountCalFeeDelay
										 , SumFeeShip=p.SumFeeShip??0
									 }).ToList();

                    if (listResult.Count == 0)
                    {
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

					Totals[0] = listResult.Sum(p => p.SumAmount).ToString("N0"); // Tổng tiền đơn hàng
					Totals[1] = listResult.Sum(p => p.TotalPayAmountNormal).ToString("N0");
					Totals[2] = listResult.Sum(p => p.SumFeeShip).ToString("N0");
					Totals[3] = listResult.Sum(p => p.AmountCalFeeDelay).ToString("N0");
					Totals[4] = listResult.Sum(p => p.AmountFeeDelay).ToString("N0");
					Totals[5] = listResult.Sum(p => p.RemainAmount).ToString("N0");

                    gridMain.Visible = true;
                    lblError.Visible = false;
                    gridMain.DataSource = listResult;
                    gridMain.DataBind();
					Session["ReportOrderDebitDetail"] = listResult;
                }

                if (type == 2)
                {
					if (Session["ReportOrderDebitDetail"] != null)
                    {
						var listResult = (List<RptOrderDebitDetailModel>)Session["ReportOrderDebitDetail"];

						Totals[0] = listResult.Sum(p => p.SumAmount).ToString("N0"); // Tổng tiền đơn hàng
						Totals[1] = listResult.Sum(p => p.TotalPayAmountNormal).ToString("N0");
						Totals[2] = listResult.Sum(p => p.SumFeeShip).ToString("N0");
						Totals[3] = listResult.Sum(p => p.AmountCalFeeDelay).ToString("N0");
						Totals[4] = listResult.Sum(p => p.AmountFeeDelay).ToString("N0");
						Totals[5] = listResult.Sum(p => p.RemainAmount).ToString("N0");

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

		private DataTable ReturnDatatable(List<RptOrderDebitDetailModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Ngày tháng");
			objTable.Columns.Add("Mã đơn hàng");
			objTable.Columns.Add("Tổng tiền - CN trong kỳ");
			objTable.Columns.Add("Đã TT Cuối kỳ");
			objTable.Columns.Add("TT vận chuyển CK");
			//objTable.Columns.Add("TT tính phí trả chậm");
			//objTable.Columns.Add("Phí trả chậm trong kỳ");
			objTable.Columns.Add("CN còn lại trong kỳ");
			
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{
				var row = objTable.NewRow();
				row["STT"] = index;
				row["Ngày tháng"] = item.OrderDate == null ? "" : item.OrderDate.Value.ToString("dd/MM/yyyy");
				row["Mã đơn hàng"] = item.OrderNo;
				row["Tổng tiền - CN trong kỳ"] = Math.Round(item.SumAmount, Constansts.NumberRoundMin);
				row["Đã TT Cuối kỳ"] = Math.Round(item.TotalPayAmountNormal, Constansts.NumberRoundMin);
				row["TT vận chuyển CK"] = Math.Round(item.SumFeeShip, Constansts.NumberRoundMin);
				//row["TT tính phí trả chậm"] = item.AmountCalFeeDelay;
				//row["Phí trả chậm trong kỳ"] = item.AmountFeeDelay;
				row["CN còn lại trong kỳ"] = Math.Round(item.RemainAmount, Constansts.NumberRoundMin);					

				objTable.Rows.Add(row);
				index += 1;
			}

			var rowFuter = objTable.NewRow();
			rowFuter["STT"] = "Tổng";
			rowFuter["Tổng tiền - CN trong kỳ"] = Math.Round(lstToTable.Sum(p => p.SumAmount), Constansts.NumberRoundMin);
			rowFuter["Đã TT Cuối kỳ"] = Math.Round(lstToTable.Sum(p => p.TotalPayAmountNormal), Constansts.NumberRoundMin);
			rowFuter["TT vận chuyển CK"] = Math.Round(lstToTable.Sum(p => p.SumFeeShip), Constansts.NumberRoundMin);
			//rowFuter["TT tính phí trả chậm"] = lstToTable.Sum(p => p.AmountCalFeeDelay);
			//rowFuter["Phí trả chậm trong kỳ"] = lstToTable.Sum(p => p.AmountFeeDelay);
			rowFuter["CN còn lại trong kỳ"] = Math.Round(lstToTable.Sum(p => p.RemainAmount), Constansts.NumberRoundMin);
			
			objTable.Rows.Add(rowFuter);
			return objTable;
		}

		protected void btnExcel_Click(object sender, EventArgs e)
		{
			
			
			if (Session["rptModelOrderDebit"] == null || Session["ReportOrderDebitDetail"]==null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			var listResult = (List<RptOrderDebitDetailModel>)Session["ReportOrderDebitDetail"];
			var table = ReturnDatatable(listResult);
			var rptModel = (RptOrderDebitModel)Session["rptModelOrderDebit"];

			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ClearContent();
			HttpContext.Current.Response.ClearHeaders();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ContentType = "application/ms-excel";
			HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
			HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=RptDebitDetail.xls");

			HttpContext.Current.Response.Charset = "utf-8";
			//HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
			//sets font
			HttpContext.Current.Response.Write("<font style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<BR><BR><BR>");
			HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:11.0pt; font-family:Calibri; background:white;'> <TR>");
			int columnscount = table.Columns.Count;

			// Report header
			HttpContext.Current.Response.Write("<th colspan='9' style='font-size:14.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO CHI TIẾT CÔNG NỢ ĐƠN HÀNG");
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			// add info
			HttpContext.Current.Response.Write("<th colspan='9' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Chi tiết công nợ Đơn hàng của khách hàng: {0}-{1}", rptModel.CustomerCode, rptModel.CustomerName));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			HttpContext.Current.Response.Write("<th colspan='9' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("<b>Trong khoảng thời gian Từ ngày:</b> {0} <b>Tới ngày:</b> {1}", Convert.ToDateTime(Request.QueryString["fromDate"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(Request.QueryString["toDate"]).ToString("dd/MM/yyyy")));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			// Đầu kỳ
			HttpContext.Current.Response.Write("<th colspan='2' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("<b>Đầu kỳ:</b>");
			HttpContext.Current.Response.Write("</B>");

			HttpContext.Current.Response.Write("<th colspan='1' align='right' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("<b>{0}</b>", Math.Round(rptModel.OpenBalance, Constansts.NumberRoundMin)));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");


			// Phát sinh trong kỳ
			HttpContext.Current.Response.Write("<th colspan='2' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("<b>Phát sinh trong kỳ:</b>");
			HttpContext.Current.Response.Write("</B>");

			HttpContext.Current.Response.Write("<th colspan='1' align='right' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("<b>{0}</b>",Math.Round(rptModel.IncreaseBalance, Constansts.NumberRoundMin)));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			// CN cuối kỳ
			HttpContext.Current.Response.Write("<th colspan='2' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("<b>CN cuối kỳ:</b>");
			HttpContext.Current.Response.Write("</B>");

			HttpContext.Current.Response.Write("<th colspan='1' align='right' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("<b>{0}</b>", Math.Round((rptModel.OpenBalance + rptModel.IncreaseBalance), Constansts.NumberRoundMin)));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			// Đã TT trong kỳ
			HttpContext.Current.Response.Write("<th colspan='2' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("<b>Đã TT trong kỳ:</b>");
			HttpContext.Current.Response.Write("</B>");

			HttpContext.Current.Response.Write("<th colspan='1' align='right' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("<b>{0}</b>",Math.Round((rptModel.PaidPayBalance), Constansts.NumberRoundMin)));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			// CN Còn lại
			HttpContext.Current.Response.Write("<th colspan='2' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("<b>CN Còn lại:</b>");
			HttpContext.Current.Response.Write("</B>");

			HttpContext.Current.Response.Write("<th colspan='1' align='right' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("<b>{0}</b>", Math.Round((rptModel.OpenBalance + rptModel.IncreaseBalance - rptModel.PaidPayBalance), Constansts.NumberRoundMin)));
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
