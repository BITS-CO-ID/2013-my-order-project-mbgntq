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
using Ecms.Biz.Entities;

namespace Ecms.Website.Admin.Report
{
	public partial class ReportGoodDeliverlyCPrint : PageBase
    {
        #region //Declares

        private readonly ReportService _reportService = new ReportService();
		protected double dSumTotalAmount = 0;
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null || Session["CustomerFind"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
                
                LoadData(1);
            }
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData(2);
        }      

		protected void btnConfirmPrintDeliverly_Click(object sender, EventArgs e)
		{
			// Check:
			if (string.IsNullOrEmpty(txtDeliverlyFullName.Text))
			{
				lblError.Text = "Bạn chưa nhập Họ & tên người giao hàng";
				lblError.Visible = true;
				return;
			}

			var listResult = (List<OrderDetailModel>)Session["ReportGood"];
			if (listResult == null || listResult.Count==0)
			{
				lblError.Text = "Không có thông tin chi tiết giao hàng";
				lblError.Visible = true;
				return;
			}

			// Save:
			var customer = (UserCustomerModel)Session["CustomerFind"];
			

			var rptPrint=new Rpt_DeliverlyConfirmPrint()
			{
				CreatedUser=Session["User"].ToString()
				, CustomerId=customer.CustomerId
				, DeliverlyFullName=txtDeliverlyFullName.Text
				, DeliverlyPosition=txtDeliverlyPosition.Text
				, Remark=txtRemark.Text
			};

			foreach (var item in listResult)
			{
				// Check Print:
				if (_reportService.Rpt_DeliverlyCheckPrint(Convert.ToString(item.OrderDetailId), this))
				{
					lblError.Text = string.Format("Món hàng có tracking=<b>{0}</b> đã được xác nhận in giao nhận trước đó, hãy kiểm tra lại.", item.TrackingNo);
					lblError.Visible = true;
					return;
				}
				var rptPrintDetail = new Rpt_DeliverlyCPDetail()
				{
					OrderDetailId=item.OrderDetailId
				};
				rptPrint.Rpt_DeliverlyCPDetail.Add(rptPrintDetail);
			}

			var result = _reportService.Rpt_DeliverlyConfirmPrintCreate(rptPrint, this);
			if (result!=null)
			{
				mtvMain.ActiveViewIndex = 1;
			}			
		}

		protected void btnOk_Click(object sender, EventArgs e)
		{
			Response.Redirect(string.Format("~/admin/report/ReportGoodDeliverly.aspx"));
		}

		protected void btnReturn_Click(object sender, EventArgs e)
		{
			Response.Redirect(string.Format("~/admin/report/ReportGoodDeliverly.aspx"));
		}

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
		}

		protected void gridMain_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
		{

		}

		protected void lbtnDelete_Click(object sender, CommandEventArgs e)
		{
			var detailId = Convert.ToInt32(e.CommandArgument.ToString());
			var listResult = (List<OrderDetailModel>)Session["ReportGood"];
			var orderDetail = listResult.SingleOrDefault(p => p.OrderDetailId == detailId);
			if (orderDetail != null)
			{
				listResult.Remove(orderDetail);
				Session["ReportGood"] = listResult;
				this.LoadData(2);
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
					// bind customer

					var customer = (UserCustomerModel)Session["CustomerFind"];
					lblCusCode.Text = customer.CustomerCode;
					lblCusName.Text = customer.CustomerName;
					lblCusDeliverAddress.Text = customer.DeliveryAddress;
					lblCusMobile.Text = customer.Mobile;

					var listResult = (List<OrderDetailModel>)Session["ReportGood"];

					if (listResult.Count == 0)
					{
						gridMain.Visible = false;
						lblError.Visible = true;
						lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
						return;
					}

					dSumTotalAmount = listResult.Sum(p => p.TotalMoney);

					gridMain.Visible = true;
					lblError.Visible = false;
					gridMain.DataSource = listResult;
					gridMain.DataBind();
					Session["ReportGood"] = listResult;
				}

                if (type == 2)
                {
					if (Session["ReportGood"] != null)
                    {
						var listResult = (List<OrderDetailModel>)Session["ReportGood"];

						dSumTotalAmount = listResult.Sum(p => p.TotalMoney);

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

		private DataTable ReturnDatatable(List<OrderDetailModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Mã đơn hàng");
			objTable.Columns.Add("Ngày đặt hàng");
			objTable.Columns.Add("TrackingNumber");
			objTable.Columns.Add("Nhóm sản phẩm");
			objTable.Columns.Add("Website");
			objTable.Columns.Add("SL");
			objTable.Columns.Add("Trọng lượng");
			objTable.Columns.Add("Thành tiền");
			objTable.Columns.Add("Ghi chú");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{

				var row = objTable.NewRow();
				row["STT"] = index;
				row["Mã đơn hàng"] = item.OrderNo;				
				row["Ngày đặt hàng"] = item.OrderDate == null ? "" : item.OrderDate.Value.ToString("dd/MM/yyyy");
				row["TrackingNumber"] = item.TrackingNo.ToString();
				row["Nhóm sản phẩm"] = item.CategoryName;
				row["Website"] = item.WebsiteName;
				row["SL"] = item.Quantity??0;
				row["Trọng lượng"] = item.Weight??0;
				row["Thành tiền"] = item.TotalMoney;
				row["Ghi chú"] = "";

				objTable.Rows.Add(row);
				index += 1;
			}

			var rowFuter = objTable.NewRow();
			rowFuter["STT"] = "Tổng";
			rowFuter["Thành tiền"] = lstToTable.Sum(p => p.TotalMoney);

			objTable.Rows.Add(rowFuter);
			return objTable;
		}

		protected void btnExcel_Click(object sender, EventArgs e)
		{
			var listResult = (List<OrderDetailModel>)Session["ReportGood"];
			var customer = (UserCustomerModel)Session["CustomerFind"];

			var table = ReturnDatatable(listResult);
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ClearContent();
			HttpContext.Current.Response.ClearHeaders();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ContentType = "application/ms-excel";
			HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
			HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=PhieuGiaoNhan.xls");

			HttpContext.Current.Response.Charset = "utf-8";
			//HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
			//sets font
			HttpContext.Current.Response.Write("<font style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<BR><BR><BR>");
			HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:11.0pt; font-family:Calibri; background:white;'> <TR>");
			int columnscount = table.Columns.Count;


			HttpContext.Current.Response.Write("<th colspan='10' style='font-size:14.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("PHIẾU GIAO NHẬN");
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			HttpContext.Current.Response.Write("<TR>");
			HttpContext.Current.Response.Write("<th colspan='10'>");
			HttpContext.Current.Response.Write(string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			#region // row 1
			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format(""));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='4' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Thông tin khách hàng"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='5' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Thông tin người giao hàng"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");
			#endregion

			#region // row 2
			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format(""));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='center' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Mã khách hàng:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='3' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", customer.CustomerCode));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='center' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Họ & Tên:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='4' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", txtDeliverlyFullName.Text));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");
			#endregion

			#region // row 3
			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format(""));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='center' style='font-size:11.0pt;  font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Tên khách hàng:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='3' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", customer.CustomerName));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt;  font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Chức vụ:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='4' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", txtDeliverlyPosition.Text));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");
			#endregion

			#region // row 4
			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format(""));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='center' style='font-size:11.0pt;  font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Đ/C nhận hàng:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='3' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", customer.DeliveryAddress));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Điện thoại:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='4' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", txtDeliverlyMobile.Text));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");
			#endregion

			#region // row 5
			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format(""));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-family:Calibri;'>");			
			HttpContext.Current.Response.Write(string.Format("Điện thoại:"));			
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='3' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", customer.Mobile));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='1' align='left' style='font-size:11.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("Ghi Chú:"));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");

			HttpContext.Current.Response.Write("<th colspan='4' align='left' style='font-size:11.0pt; font-bold:true; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write(string.Format("{0}", txtRemark.Text));
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");
			#endregion

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
