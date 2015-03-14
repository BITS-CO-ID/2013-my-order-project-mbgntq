using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Biz;
using CommonUtils;
using System.Globalization;
using System.Data;


namespace Ecms.Website.Admin.Report
{
    public partial class ReportOutbound : PageBase
    {
        #region // Declaration

        private readonly OrderService _orderService = new OrderService();
        private readonly WebsiteService _websiteService = new WebsiteService();
        private readonly CommonService _commonService = new CommonService();
		protected double rptSumAmount = 0;
        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
					if (Session["User"] == null)
					{
						Response.Redirect("~/admin/security/login.aspx");
					}
                    InitData();
                    this.DoSearch(1);
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoSearch(1);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdD.PageIndex = e.NewPageIndex;
                this.DoSearch(2);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void ddlWebsiteGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebsiteGroup.SelectedValue.Equals(""))
            {
                ddlWebsite.Items.Clear();
                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
            }
            else
            {
                int websiteParentId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue);
                var websiteList = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == websiteParentId);
                ddlWebsite.DataSource = websiteList;
                ddlWebsite.DataTextField = "WebsiteName";
                ddlWebsite.DataValueField = "WebsiteId";
                ddlWebsite.DataBind();
                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
            }
        }        

        #endregion

        #region // Method

        private void InitData()
        {
            var currentDate = DateTime.Now;
            this.txtDateFrom.Text = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0).ToString("dd/MM/yyyy");
            this.txtDateTo.Text = currentDate.ToString("dd/MM/yyyy");

            var parentWebsite = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == null);
            ddlWebsiteGroup.DataSource = parentWebsite;
            ddlWebsiteGroup.DataTextField = "WebsiteName";
            ddlWebsiteGroup.DataValueField = "WebsiteId";
            ddlWebsiteGroup.DataBind();
            ddlWebsiteGroup.Items.Insert(0, new ListItem("-- Chọn nhóm website --", ""));
            ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
        }

        private void DoSearch(int type)
        {
            if (type == 1)
            {
                lblError.Text = "";
                var fDate = new DateTime();
                var tDate = new DateTime();
                if (ValidData(ref fDate, ref tDate) == false)
                    return;

                var searchParamDict = new Dictionary<string, string>();

                var list = _orderService.OrderOutboundGet(
                            ""
                            , txtOOD.Text
                            , txtTrackingNo.Text
                            , ""
							, fDate.ToString("yyyy-MM-dd")
							, tDate.ToString("yyyy-MM-dd HH:mm:ss")
                            , ddlStatus.SelectedValue
                            , ddlWebsite.SelectedValue
                            , ""
                            , ""
                            , ddlWebsiteGroup.SelectedValue
							, ""
                            , ""
							, ""
                            , this);

                if (list.Count > 0)
                {
					rptSumAmount = list.Sum(p => p.SumAmount ?? 0);
                    grdD.Visible = true;
                    lblError.Visible = false;
                    grdD.DataSource = list;
                    grdD.DataBind();
					
					Session["ReportOutbound"] = list;
                }
                else
                {
                    grdD.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
                }
            }

            if (type == 2)
            {
				if (Session["ReportOutbound"] != null)
                {
					var list = (List<OrderOutboundModel>)Session["ReportOutbound"];
					rptSumAmount = list.Sum(p => p.SumAmount ?? 0);
                    grdD.DataSource = list;
                    grdD.DataBind();
                }
            }
        }

        private bool ValidData(ref DateTime fDate, ref DateTime tDate)
        {
            if (string.IsNullOrEmpty(txtDateFrom.Text))
            {
                lblError.Text = "Từ ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                fDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtDateTo.Text))
            {
                lblError.Text = "Đến ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                tDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", viVN).AddDays(1).AddSeconds(-1);
            }
            catch
            {
                lblError.Text = "Đến ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

		private DataTable ReturnDatatable(List<OrderOutboundModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Mã đơn hàng");
			objTable.Columns.Add("TrackingNumber");
			objTable.Columns.Add("OrderNumber");
			objTable.Columns.Add("Ngày đặt hàng");
			objTable.Columns.Add("Người đặt");
			objTable.Columns.Add("Website");
			objTable.Columns.Add("Tài khoản đặt hàng");
			objTable.Columns.Add("Visa");
			objTable.Columns.Add("Tình trạng");
			objTable.Columns.Add("Ngày về Mỹ");
			objTable.Columns.Add("Ngày về VN");
			//objTable.Columns.Add("Ngày giao hàng");			
			objTable.Columns.Add("Thành tiền");
			#endregion
			int index = 1;
			foreach (var item in lstToTable)
			{
				var row = objTable.NewRow();
				row["STT"] = index;
				row["Mã đơn hàng"] = item.OrderOutboundNo;
				row["TrackingNumber"] = item.TrackingNo;
				row["OrderNumber"] = item.OrderNumber;
				row["Ngày đặt hàng"] = item.OrderDate == null ? "" : item.OrderDate.Value.ToString("dd/MM/yyyy");
				row["Người đặt"] = item.UserCreate;
				row["Website"] = item.WebsiteName;
				row["Tài khoản đặt hàng"] = item.AccountWebsiteNo;
				row["Visa"] = item.VisaNo;
				row["Tình trạng"] = item.StatusText;
				row["Ngày về Mỹ"] = item.DeliverlyUSADate == null ? "" : item.DeliverlyUSADate.Value.ToString("dd/MM/yyyy"); ;
				row["Ngày về VN"] = item.DeliverlyVNDate == null ? "" : item.DeliverlyVNDate.Value.ToString("dd/MM/yyyy"); ;
				//row["Ngày giao hàng"] = item.DeliverlyDate == null ? "" : item.DeliverlyDate.Value.ToString("dd/MM/yyyy"); ;
				row["Thành tiền"] = item.SumAmount;

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
			if (Session["ReportOutbound"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			var listResult = (List<OrderOutboundModel>)Session["ReportOutbound"];
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


			HttpContext.Current.Response.Write("<th colspan='13' style='font-size:14.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("BÁO CÁO TỔNG HỢP ĐƠN HÀNG NƯỚC NGOÀI");
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