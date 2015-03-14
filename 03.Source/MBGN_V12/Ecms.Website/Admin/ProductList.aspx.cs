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

namespace Ecms.Website.Admin
{
    public partial class ProductList : PageBase
    {
        #region // Declaration

        private readonly ProductService _productService = new ProductService();

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    //Session["ProductCode"] = null;
                    loadComboBox();
                    loadGrid();
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }


        protected void ddCategoryIdParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                loadCategoryComboBox();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

		protected void lblBestSale_Click(object sender, CommandEventArgs e)
		{
			// Thay đổi BestSale

			switch (e.CommandName)
			{
				case "BestSale":
					try
					{
						var param = e.CommandArgument.ToString().Split('|');

						// Nếu đã BestSale thì =0; Nếu chưa hoặc không bestSale thì =1
						var bestSale="";
						if(string.IsNullOrEmpty(param[1]))
						{
							bestSale="1";
						}else
						{
							if(param[1]=="1")
							{
								bestSale="0";
							}else
							{
								bestSale="1";
							}
						}

						var result = _productService.ProductBestSaleUpdate(
										param[0]
										, bestSale 
										, this);

						if (result)
						{
							loadGrid();
						}
					}
					catch (Exception exc)
					{
						Utils.ShowExceptionBox(exc, this);
					}
					break;
			}

		}
        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        Session["lbtEdit_Click"] = id;
                        Response.Redirect("ProductCreate.aspx");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }
        }


        protected void lbtDelete_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    try
                    {
                        string id = e.CommandArgument.ToString();
						var list = _productService.ProductGet(id, "", "", "", "", "", "", "", "", "", "", this);

                        
                        // xem có ảnh không để xóa
                        if (!list[0].Image.Contains("://") && Utilities.existFile(Server.MapPath("0")+list[0].Image))
                            Utilities.deleteFile(Server.MapPath(list[0].Image));
                        var result = _productService.ProductyDelete(id, this);
                        if (result)
                            Response.Write("<script type='text/javascript'>alert('Đã xóa dữ liệu thành công.');</script>");
                        
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }

        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductCreate.aspx");
        }


        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdD.PageIndex = e.NewPageIndex;
                loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        // đang sử lí
        protected void grdD_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        #endregion

        #region // Method

        private List<ProductModel> loadGrid()
        {
            string orginalId = "";
            string vendorId = "";
            string parentCategoryId = "";
            string categoryId = "";
            lblError.Text = "";
            // "-1" là chọn load tất cả
            orginalId = ddOrginal.SelectedValue == "-1" ? "" : ddOrginal.SelectedValue;
            vendorId = ddVendorId.SelectedValue == "-1" ? "" : ddVendorId.SelectedValue;
            categoryId = ddCategoryId.SelectedValue == "-1" ? "" : ddCategoryId.SelectedValue;
            parentCategoryId = ddCategoryIdParent.SelectedValue == "-1" ? "" : ddCategoryIdParent.SelectedValue;

            var list = _productService.ProductGet(
							""
							, txtProductCode.Text
							, txtProductName.Text
							, ""
							, categoryId
							, parentCategoryId
							, this.ddlPublished.SelectedValue
							, ""
							, vendorId
							, orginalId
							, ddlBestSale.SelectedValue
							, this);

            grdD.DataSource = list;
            grdD.DataBind();
            if (list.Count > 0)
            {
                grdD.Visible = true;
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }

            return list;
        } // end LoadGrid

        private void loadComboBox()
        {
            // Lấy Nhóm sản phẩm đổ vào ddCategoryIdParent
            List<Category> temp = new List<Category>();
            temp.Add(new Category() { CategoryId = -1, CategoryName = "-- Tất cả --" });
            temp.AddRange(this._productService.CategoryGet("", "", "0", this));
            ddCategoryIdParent.DataSource = temp;
            ddCategoryIdParent.DataTextField = "CategoryName";
            ddCategoryIdParent.DataValueField = "CategoryId";
            ddCategoryIdParent.DataBind();

            // Lấy Loại nhà cung cấp đổ vào ddVendorId
            List<Vendor> temp2 = new List<Vendor>();
            temp2.Add(new Vendor() { VendorId = -1, VendorName = "-- Tất cả --" });
            temp2.AddRange(this._productService.VendorGet("", "", this));
            ddVendorId.DataSource = temp2;
            ddVendorId.DataTextField = "VendorName";
            ddVendorId.DataValueField = "VendorId";
            ddVendorId.DataBind();

            loadCategoryComboBox();
        }

        private void loadCategoryComboBox()
        {
            // Lấy loại sản phẩm
            List<Category> temp = new List<Category>();
            temp.Add(new Category() { CategoryId = -1, CategoryName = "-- Tất cả --" });

            // xem danh mục cha có đang để mặc định không
            if (ddCategoryIdParent.SelectedValue != "-1")
                temp.AddRange(this._productService.CategoryGet("", "", ddCategoryIdParent.SelectedValue, this));

            ddCategoryId.DataSource = temp;
            ddCategoryId.DataTextField = "CategoryName";
            ddCategoryId.DataValueField = "CategoryId";
            ddCategoryId.DataBind();
        }

        #endregion


    }
}