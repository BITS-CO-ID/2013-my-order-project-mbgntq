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
	public partial class ReportGoodDeliverly : PageBase
    {
        #region // Declares

        private readonly ReportService _reportService = new ReportService();
		private readonly WebsiteService _websiteAccountService = new WebsiteService();
        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				loadWebsiteParentComboBox();

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

		protected void btnPrintDeliverly_Click(object sender, EventArgs e)
		{
			
			// Check:
			if (string.IsNullOrEmpty(txtCustomerCode.Text))
			{
				lblError.Text = "Bạn chưa tìm kiếm khách hàng để giao hàng";
				lblError.Visible = true;
				return;
			}
			var customer = new CustomerService().CustomerList(
								""
								, ""
								, this.txtCustomerCode.Text.Trim()
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this).SingleOrDefault();

			if (customer == null)
			{
				lblError.Text = "Thông tin khách hàng không chính xác";
				lblError.Visible = true;
				return;
			}
			else
			{
				Session["CustomerFind"] = customer;
			}

			var chooseStaus = string.IsNullOrEmpty(ddlStatus.SelectedValue) ? 0 : Convert.ToInt32(ddlStatus.SelectedValue);

			if (chooseStaus != OrderOutboundStatus.InvInbound)
			{
				lblError.Text = "Bạn chỉ được In phiếu giao nhận cho những món hàng 'Đã về VN'";
				lblError.Visible = true;
				return;
			}

			// find again
			this.LoadData(1);

			Response.Redirect(string.Format("~/admin/report/reportgooddeliverlycprint.aspx?cusId={0}", 1));
		}
		protected void ddCategoryIdParent_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				loadWebsiteIdComboBox();
			}
			catch (Exception exc)
			{
				Utils.ShowExceptionBox(exc, this);
			}
		}
        #endregion

        #region // Private methods

		private void loadWebsiteParentComboBox()
		{
			// Lấy Nhóm sản phẩm đổ vào ddCategoryIdParent
			List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
			temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Tất cả --" });
			temp.AddRange(this._websiteAccountService.WebsiteLinkGet("", "", "", "0", this));
			ddParrentWebsite.DataSource = temp;
			ddParrentWebsite.DataTextField = "WebsiteName";
			ddParrentWebsite.DataValueField = "WebsiteId";
			ddParrentWebsite.DataBind();

			loadWebsiteIdComboBox();
		}

		private void loadWebsiteIdComboBox()
		{
			// Lấy loại sản phẩm
			List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
			temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Tất cả --" });

			// xem danh mục cha có đang để mặc định không
			if (ddParrentWebsite.SelectedValue != "-1")
				temp.AddRange(this._websiteAccountService.WebsiteLinkGet("", "", "", ddParrentWebsite.SelectedValue, this));
		}

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

					var listResult = _reportService.ReportGoodDeliverlyGet(
											fDate,
											tDate,
											txtDetailCode.Text,
											txtOrderNo.Text,
											txtCustomerCode.Text,
											this.txtShop.Text,
											this.ddParrentWebsite.SelectedValue == "-1" ? "" : this.ddParrentWebsite.SelectedValue,
											"",
											this.ddlStatus.SelectedValue,
											this.ddlOrderType.SelectedValue,
											this.txtBillNo.Text.Trim(),
											this);

                    if (listResult.Count == 0)
                    {
						Session["ReportGood"] = listResult;
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

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

		private DataTable ReturnDatatable(List<OrderDetailModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Mã MH");
			objTable.Columns.Add("Loại đơn hàng");
			objTable.Columns.Add("Mã đơn hàng");
			objTable.Columns.Add("Mã đơn hàng NN");
			objTable.Columns.Add("Ngày đặt hàng");
			objTable.Columns.Add("BillNo");
			objTable.Columns.Add("Mã khách hàng");
			objTable.Columns.Add("Tên khách hàng");
			objTable.Columns.Add("Nhóm sản phẩm");
			objTable.Columns.Add("Tên sản phẩm");
			objTable.Columns.Add("WebsiteName");
			objTable.Columns.Add("Shop");
			objTable.Columns.Add("Ngày về VN");
			objTable.Columns.Add("Ngày giao hàng");
			objTable.Columns.Add("Tình trạng");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{

				var row = objTable.NewRow();
				row["STT"] = index;
				row["Mã MH"] = item.DetailCode;
				row["Loại đơn hàng"] = item.OrderTypeName;
				row["Mã đơn hàng"] = item.OrderNo;
				row["Mã đơn hàng NN"] = item.OrderOutboundNo;
				row["Ngày đặt hàng"] = item.OrderDate == null ? "" : item.OrderDate.Value.ToString("dd/MM/yyyy");
				row["BillNo"] = string.Format("'{0}", item.TrackingNo);
				row["Mã khách hàng"] = item.CustomerCode;
				row["Tên khách hàng"] = item.CustomerName;
				row["Nhóm sản phẩm"] = item.CategoryName;
				row["Tên sản phẩm"] = item.ProductName;
				row["WebsiteName"] = item.WebsiteName;
				row["Shop"] = item.Shop;
				row["Ngày về VN"] = item.DeliveryVNDate == null ? "" : item.DeliveryVNDate.Value.ToString("dd/MM/yyyy");
				row["Ngày giao hàng"] = item.DeliveryDate == null ? "" : item.DeliveryDate.Value.ToString("dd/MM/yyyy"); 
				row["Tình trạng"] = item.DetailStatusText;

				objTable.Rows.Add(row);
				index += 1;
			}
			return objTable;
		}

		protected void btnExcel_Click(object sender, EventArgs e)
		{
			var listResult = (List<OrderDetailModel>)Session["ReportGood"];
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


			HttpContext.Current.Response.Write("<th colspan='16' style='font-size:14.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO TÌNH TRẠNG MÓN HÀNG");
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