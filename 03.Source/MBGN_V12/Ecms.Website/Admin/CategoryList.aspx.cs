using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Website.DBServices;

namespace Ecms.Website.Admin.Security
{
    public partial class CategoryList : PageBase
    {
        #region // Declaration

        private readonly ProductService _productService = new ProductService();

        #endregion

        #region //Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    LoadGrid();
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
                LoadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
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
                        Response.Redirect("CategoryCreate.aspx");
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

                        var query = _productService.CategoryGet("", "", id, this);
                        if (query != null && query.Count() > 0)
                        {
                            throw new Exception("Nhóm sản phẩm đã được sử dụng, bạn không thể xóa.");
                        }

						var query2 = _productService.ProductGet("", "", "", "", id, "", "", "", "", "", "", this);
                        if (query != null && query.Count() > 0)
                        {
                            throw new Exception("Nhóm sản phẩm đã được dùng cho sản phẩm, bạn không thể xóa.");
                        }

                        var result = this._productService.CategoryDelete(id, this);
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
            Response.Redirect("CategoryCreate.aspx");
        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdD.PageIndex = e.NewPageIndex;
                LoadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void grdD_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                LoadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }
        #endregion

        #region // Method

        private List<Category> LoadGrid()
        {
            var list = this._productService.CategoryGet(""
                        , txtCategoryName.Text
                        , ""
                        , this);

            if (list.Count > 0)
            {
                grdD.DataSource = list;
                grdD.DataBind();
                grdD.Visible = true;
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy nhóm sản phẩm.";
            }
            return list;
        }

		public string getCategoryParentName(string parentId)
		{
			if(!string.IsNullOrEmpty(parentId))
			{
			
			var category = this._productService.CategoryGet(
						parentId						
						, ""
						, ""
						, this).SingleOrDefault();
				
				return category==null?"":category.CategoryName;
				
			}else
			{
				return "";
			}
		}
        #endregion


    }
}