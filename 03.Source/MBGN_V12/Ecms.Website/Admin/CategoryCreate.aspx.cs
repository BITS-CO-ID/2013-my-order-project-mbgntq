using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class CategoryCreate : PageBase
    {
        #region //Declaration

        private readonly ProductService _productService = new ProductService();

        #endregion

        #region //Even

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    loadDDList();
                    if (Session["lbtEdit_Click"] != null)
                    {
                        lblFunction.Text = "Sửa";
                        btnSave.Text = "Lưu";
                        string id = (string)Session["lbtEdit_Click"];
                        Session["CategoryCode"] = id;
                        var category = this._productService.CategoryGet(id, "", "", this);
                        ddtxtParentId.ClearSelection();

                        if (null != category[0].ParentId)
                            ddtxtParentId.Items.FindByValue(Convert.ToString(category[0].ParentId)).Selected = true;

                        txtCategoryName.Text = category[0].CategoryName;
                        txtRemark.Text = category[0].Remark;
                        Session["lbtEdit_Click"] = null;
                    }
                    else
                    {
                        lblFunction.Text = "Thêm";
                        txtCategoryName.Text = "";
                        txtRemark.Text = "";

                    }
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCategoryName.Text.Trim()))
                {
                    lblError.Text = "Tên nhóm sản phẩm không được để trống.";
                    return;
                }

                if (Session["CategoryCode"] != null)
                {
                    string id = (string)Session["CategoryCode"];
                    var item = new Category();
                    item.CategoryId = Convert.ToInt32(id);
                    item.CategoryName = txtCategoryName.Text;
                    int parentId = Convert.ToInt32(ddtxtParentId.SelectedItem.Value);
                    if (parentId == -1)
                        item.ParentId = null;
                    else
                        item.ParentId = parentId;
                    item.Remark = txtRemark.Text;
                    Session["CategoryCode"] = null;
                    var result = this._productService.CategoryUpdate(item, this);
                    if(result.CategoryId != 0)
                        DisplayAlert("Hệ thống cập nhật thành công", "CategoryList.aspx");
                }
                else
                {
                    var item = new Category();
                    item.CategoryName = txtCategoryName.Text;
                    int parentId = Convert.ToInt32(ddtxtParentId.SelectedItem.Value);
                    if (parentId == -1)
                        item.ParentId = null;
                    else
                        item.ParentId = parentId;
                    item.Remark = txtRemark.Text;
                    var result = this._productService.CategoryCreate(item, this);
                    if (result.CategoryId != 0)
                        DisplayAlert("Hệ thống cập nhật thành công", "CategoryList.aspx");
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CategoryList.aspx");
        }

        #endregion

        #region // Method

        private void loadDDList()
        {
            List<Category> temp = new List<Category>();
            temp.Add(new Category() { CategoryId = -1, CategoryName = "-- Chọn loại sản phẩm --" });
            temp.AddRange(this._productService.CategoryGet("", "", "0", this));

            ddtxtParentId.DataSource = temp;
            ddtxtParentId.DataTextField = "CategoryName";
            ddtxtParentId.DataValueField = "CategoryId";
            ddtxtParentId.DataBind();
        }

        protected virtual void DisplayAlert(string message, string page)
        {
            try
            {
                ClientScript.RegisterStartupScript(
                  this.GetType(),
                  Guid.NewGuid().ToString(),
                  string.Format("alert('{0}');window.location.href = '" + page + "'",
                    message.Replace("'", @"\'").Replace("\n", "\\n").Replace("\r", "\\r")),
                    true);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        #endregion



    }
}