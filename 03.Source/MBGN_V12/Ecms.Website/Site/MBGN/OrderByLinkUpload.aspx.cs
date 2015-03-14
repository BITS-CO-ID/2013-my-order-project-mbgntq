using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Biz;
using System.Globalization;
using Ecms.Website.Common;
using CommonUtils;
using System.Data.OleDb;
using System.IO;
using System.Data;

namespace Ecms.Website.Site.MBGN
{
	public partial class OrderByLinkUpload : System.Web.UI.Page
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

		protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
				lblError.Text = "";
				lblError.Visible = false;

				if (fileuploadExcel.HasFile)
				{
					string FileName = Path.GetFileName(fileuploadExcel.PostedFile.FileName);
					string Extension = Path.GetExtension(fileuploadExcel.PostedFile.FileName);
					if (!Extension.Contains(".xls"))
					{
						lblError.Text = "File import không đúng định dạng!";
						lblError.Visible = true;
						return;
					}
					string FolderPath = "UploadTempFolder/";

					string FilePath = Server.MapPath(FolderPath + FileName);
					fileuploadExcel.SaveAs(FilePath);
					Import_To_Grid(FilePath, Extension, "");
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		private void Import_To_Grid(string FilePath, string Extension, string isHDR)
		{
			string conStr = "";
			switch (Extension)
			{
				case ".xls": //Excel 97-03
					conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
					break;
				case ".xlsx": //Excel 07
					conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
					break;
			}
			conStr = String.Format(conStr, FilePath, "Yes");
			OleDbConnection connExcel = new OleDbConnection(conStr);
			OleDbCommand cmdExcel = new OleDbCommand();
			OleDbDataAdapter oda = new OleDbDataAdapter();
			DataTable dt = new DataTable();
			cmdExcel.Connection = connExcel;

			//Get the name of First Sheet
			connExcel.Open();
			DataTable dtExcelSchema;
			dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
			connExcel.Close();

			//Read Data from First Sheet
			connExcel.Open();
			cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
			oda.SelectCommand = cmdExcel;
			oda.Fill(dt);
			connExcel.Close();

			if (dt.Rows.Count > 0)
			{
				#region // validate file excel

					#region // validate column name
					if (!dt.Columns.Contains("tên shop"))
					{
						lblError.Text = "File import không có cột thông tin shop.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("link sản phẩm"))
					{
						lblError.Text = "File import không có cột thông tin 'Link sản phẩm'.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("tên shop"))
					{
						lblError.Text = "File import không có cột thông tin 'mầu sắc'.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("mầu sắc"))
					{
						lblError.Text = "File import không có cột thông tin 'mầu sắc'.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("size"))
					{
						lblError.Text = "File import không có cột thông tin 'Size'.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("số lượng"))
					{
						lblError.Text = "File import không có cột thông tin 'Số lượng'.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("Giá web"))
					{
						lblError.Text = "File import không có cột thông tin 'Giá web'.";
						lblError.Visible = true;
						return;
					}

					if (!dt.Columns.Contains("Mô tả"))
					{
						lblError.Text = "File import không có cột thông tin 'Mô tả'.";
						lblError.Visible = true;
						return;
					}
					#endregion

					#region // validate item data such as: link, price and quantity
					dt.Columns["tên shop"].ColumnName = "Shop";
					dt.Columns["link sản phẩm"].ColumnName = "Link";
					dt.Columns["link ảnh sản phẩm"].ColumnName = "ImageUrl";
					dt.Columns["mầu sắc"].ColumnName = "Color";
					dt.Columns["size"].ColumnName = "Size";
					dt.Columns["số lượng"].ColumnName = "Quantity";
					dt.Columns["giá web"].ColumnName = "Price";
					dt.Columns["mô tả"].ColumnName = "Remark";
					
					foreach (DataRow item in dt.Rows)
					{
						if (string.IsNullOrEmpty(item["Shop"].ToString())
							&& string.IsNullOrEmpty(item["Link"].ToString())
							&& string.IsNullOrEmpty(item["ImageUrl"].ToString())
							&& string.IsNullOrEmpty(item["Color"].ToString())
							&& string.IsNullOrEmpty(item["Size"].ToString())
							&& string.IsNullOrEmpty(item["Quantity"].ToString())
							&& string.IsNullOrEmpty(item["Price"].ToString())
							&& string.IsNullOrEmpty(item["Remark"].ToString())
							)
						{
							item.Delete();
							continue;
						}

						if (string.IsNullOrEmpty(item["link"].ToString()))
						{
							lblError.Text = "File import có trường 'Link sản phẩm' chưa nhập, kiểm tra lại file excel.";
							lblError.Visible = true;
							return;
						}

						if (string.IsNullOrEmpty(item["Quantity"].ToString()))
						{
							lblError.Text = "File import có trường 'Số lượng' chưa nhập, kiểm tra lại file excel.";
							lblError.Visible = true;
							return;
						}
						else
						{
							int quantity = 0;
							if (!Int32.TryParse(item["Quantity"].ToString(), out quantity))
							{
								lblError.Text = "File import có trường 'Số lượng' không chính xác, kiểm tra lại file excel.";
								lblError.Visible = true;
								return;
							}
						}

						if (string.IsNullOrEmpty(item["Price"].ToString()))
						{
							lblError.Text = "File import có trường 'Giá web' chưa nhập, kiểm tra lại file excel.";
							lblError.Visible = true;
							return;
						}
						else
						{
							double price = 0;
							if (!double.TryParse(item["Price"].ToString(), out price))
							{
								lblError.Text = "File import có trường 'Giá web' không chính xác, kiểm tra lại file excel.";
								lblError.Visible = true;
								return;
							}
						}
					}
					dt.AcceptChanges();
					#endregion
				#endregion

				Session["CartLink"] = dt;

				gridCartByLink.DataSource = dt;
				gridCartByLink.DataBind();

				pnCartLink.Visible = true;
			}
			else
			{
				pnCartLink.Visible = false;
			}
		}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			Session.Remove("Order");
			Session.Remove("CartLink");
            Response.Redirect("~/site/default.aspx");
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
				if (Session["CartLink"] != null)
                {
                    var cartLinks = (DataTable)Session["CartLink"];
                    var customer = (UserCustomerModel)Session["Customer"];

                    Order order = new Order();
                    order.OrderDate = order.CreatedDate = DateTime.Now;
                    order.OrderTypeId = 2;

                    foreach (DataRow item in cartLinks.Rows)
                    {
                        var orderDetailTemp = new OrderDetail();
						//orderDetailTemp.WebsiteId = websiteId;
                        orderDetailTemp.ProductLink = item["link"].ToString();
						orderDetailTemp.ImageUrl = item["ImageUrl"].ToString();
                        orderDetailTemp.CountryId = Constansts.CountryIdChina;
						orderDetailTemp.PriceWeb = Convert.ToDouble(item["Price"]);
						orderDetailTemp.Quantity = Convert.ToDouble(item["Quantity"]);
						orderDetailTemp.Color = item["Color"].ToString();
						orderDetailTemp.Size = item["Size"].ToString();
						orderDetailTemp.Shop = item["Shop"].ToString();
						orderDetailTemp.Remark = item["Remark"].ToString();
                        order.OrderDetails.Add(orderDetailTemp);
                    }

                    Session["Order"] = order;
                    Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		protected void gridCartByLink_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				gridCartByLink.PageIndex = e.NewPageIndex;
				//loadGrid();

				if (Session["CartLink"] != null)
				{
					gridCartByLink.DataSource = (DataTable)Session["CartLink"];
					gridCartByLink.DataBind();
				}
			}
			catch (Exception exc)
			{
				Utils.ShowExceptionBox(exc, this);
			}
		}

        #endregion

        #region //Private methods

        #endregion
    }
}