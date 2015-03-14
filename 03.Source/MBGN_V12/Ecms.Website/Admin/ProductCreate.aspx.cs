using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using CommonUtils;
using System.IO;
using Ecms.Website.Common;

namespace Ecms.Website.Admin
{
    public partial class ProductCreate : PageBase
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
                    loadEdit();
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (fupImage.HasFile)
                {
                    if (CheckFileType(fupImage.FileName))
                    {
                        String filePath = "~/Upload/ProductImage/" + fupImage.FileName;
                        fupImage.SaveAs(MapPath(filePath));
                        txtImage.Text = filePath;
                        imgView.ImageUrl = filePath;
                    }
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
				if (string.IsNullOrEmpty(txtProductCode.Text))
				{
					lblError.Text = "Mã sản phẩm không được để trống.";
					return;
				}

				if (string.IsNullOrEmpty(txtProductCode.Text))
				{
					lblError.Text = "Tên sản phẩm không được để trống.";
					return;
				}

                if (ddCategoryId.SelectedItem.Value == "-1")
                {
                    lblError.Text = "Nhóm sản phẩm không được để trống.";
                    return;
                }

				if (string.IsNullOrEmpty(txtImage.Text) && string.IsNullOrEmpty(txtImageTempOld.Text))
				{
					lblError.Text = "Bạn chưa upload hoặc chọn link ảnh sản phẩm.";
					return;
				}

				if (ddVendorId.SelectedItem.Value == "-1")
				{
					lblError.Text = "Nhà cung cấp không được để trống.";
					return;
				}

				//if (ddOrginal.SelectedItem.Value == "-1")
				//{
				//    lblError.Text = "Xuất xứ không được để trống.";
				//    return;
				//}


                if (Session["ProductCode"] != null)
                {
					string id = (string)Session["ProductCode"];
					int productId = Convert.ToInt32(id);
					var item = new Product();
					// Check ProductCode:
					var query = _productService.ProductGet(
								""
								, this.txtProductCode.Text.Trim()
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this).Where(p => p.ProductId != productId).ToList();

					if (query != null && query.Count > 0)
					{
						lblError.Text = "Mã sản phẩm đã có trong hệ thống.";
						return;
					}
                    
                    item.ProductId = Convert.ToInt32(id);
                    item.ProductCode = txtProductCode.Text;
                    item.ProductName = txtProductName.Text;
					item.ProductLable = txtProductLable.Text;
					if (!string.IsNullOrEmpty(txtProductValue.Text))
					{
						item.ProductValue = Convert.ToDouble(txtProductValue.Text);
					}
					else
					{
						item.ProductValue = 0;
					}

					if (!string.IsNullOrEmpty(txtProductSaleValue.Text))
					{
						item.ProductSaleValue = Convert.ToDouble(txtProductSaleValue.Text);
					}
					else
					{
						item.ProductSaleValue = 0;
					}
                    item.CategoryId = Convert.ToInt32(ddCategoryId.SelectedItem.Value);
                    item.Size = txtSize.Text;

                    item.Image = txtImage.Text;
					if ((txtImageTempOld.Text != txtImage.Text) && Utilities.existFile(txtImageTempOld.Text))
					{
						Utilities.deleteFile(Server.MapPath(".") + txtImageTempOld.Text);
					}

                    item.Published = ckbPublished.Checked;
                    item.Tag = txtTag.Text;
                    item.ShortDescription = txtShortDescription.Text;
                    item.Description = txtDescription.Text;
                    item.VendorId = Convert.ToInt32(ddVendorId.SelectedItem.Value);
					if (!string.IsNullOrEmpty(ddOrginal.SelectedItem.Value))
					{
						item.Orginal = ddOrginal.SelectedItem.Value;
					}
					else
					{
						item.Orginal = null;
					}
					item.BestSale = chkBestSale.Checked?"1":"0";

					if (!string.IsNullOrEmpty(ddlProductType.SelectedItem.Value))
					{
						item.ProductType = ddlProductType.SelectedItem.Value;
					}

					if (!string.IsNullOrEmpty(ddlProductStatus.SelectedItem.Value))
					{
						item.ProductStatus =Convert.ToInt32(ddlProductStatus.SelectedItem.Value);
					}

					if (!string.IsNullOrEmpty(ddlStockStatus.SelectedItem.Value))
					{
						item.Balance = Convert.ToInt32(ddlStockStatus.SelectedItem.Value);
					}

                    Session["ProductCode"] = null;
                    var result = this._productService.ProductUpdate(item, this);
					if (result.ProductId != 0)
					{
						DisplayAlert("Hệ thống cập nhật thành công", "ProductList.aspx");
					}
                }
                else
                {
					var query = _productService.ProductGet(
								""
								, this.txtProductCode.Text.Trim()
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this);

					if (query != null && query.Count > 0)
					{
						lblError.Text = "Mã sản phẩm đã có trong hệ thống.";
						return;
					}

                    var item = new Product();
                    item.ProductCode = txtProductCode.Text;
                    item.ProductName = txtProductName.Text;
					item.ProductLable = txtProductLable.Text;
					if (!string.IsNullOrEmpty(txtProductValue.Text))
					{
						item.ProductValue = Convert.ToDouble(txtProductValue.Text);
					}
					else
					{
						item.ProductValue = 0;
					}

					if (!string.IsNullOrEmpty(txtProductSaleValue.Text))
					{
						item.ProductSaleValue = Convert.ToDouble(txtProductSaleValue.Text);
					}
					else
					{
						item.ProductSaleValue = 0;
					}
                    item.CategoryId = Convert.ToInt32(ddCategoryId.SelectedItem.Value);
                    item.Size = txtSize.Text;
                    item.Image = txtImage.Text;
                    item.Published = ckbPublished.Checked;
                    item.Tag = txtTag.Text;
                    item.ShortDescription = txtShortDescription.Text;
                    item.Description = txtDescription.Text;
                    item.VendorId = Convert.ToInt32(ddVendorId.SelectedItem.Value);
					if (!string.IsNullOrEmpty(ddOrginal.SelectedItem.Value))
					{
						item.Orginal = ddOrginal.SelectedItem.Value;
					}
					else
					{
						item.Orginal = null;
					}
					item.BestSale = chkBestSale.Checked ? "1" : "0";
					if (!string.IsNullOrEmpty(ddlProductType.SelectedItem.Value))
					{
						item.ProductType = ddlProductType.SelectedItem.Value;
					}
					if (!string.IsNullOrEmpty(ddlProductStatus.SelectedItem.Value))
					{
						item.ProductStatus = Convert.ToInt32(ddlProductStatus.SelectedItem.Value);
					}
					if (!string.IsNullOrEmpty(ddlStockStatus.SelectedItem.Value))
					{
						item.Balance = Convert.ToInt32(ddlStockStatus.SelectedItem.Value);
					}

                    var result = this._productService.ProductCreate(item, this);
					if (result.ProductId != 0)
					{
						DisplayAlert("Hệ thống cập nhật thành công", "ProductList.aspx");
					}
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtImageTempOld.Text != txtImage.Text) && Utilities.existFile(txtImageTempOld.Text))
                    Utilities.deleteFile(Server.MapPath("." + txtImage.Text));
                Response.Redirect("ProductList.aspx");
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
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

        protected void txtImage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imgView.ImageUrl = txtImage.Text;
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        #endregion

        #region // Method

        /// <summary>
        /// Kiểm tra phần mở rộng file có là ảnh
        /// </summary>
        /// <param name="fileName"> tên file </param>
        /// <returns></returns>
        private bool CheckFileType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".png":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Load dữ liệu lên form
        /// </summary>
        private void loadEdit()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                btnSave.Text = "Lưu";
                lblFunction.Text = "Sửa";
                string id = (string)Session["lbtEdit_Click"];
                Session["ProductCode"] = id;
				var product = this._productService.ProductGet(id, "", "", "", "", "", "", "", "", "", "", this);

                string cateroryId = String.IsNullOrEmpty(product[0].CategoryId.ToString()) ? "-1" : product[0].CategoryId.ToString();
                loadParentComboBox(Convert.ToString(cateroryId));

                loadVenderComboBox(Convert.ToString(product[0].VendorId));
				if (!String.IsNullOrEmpty(product[0].Orginal))
				{
					ddOrginal.Items.FindByValue(product[0].Orginal).Selected = true;
				}

				if (!String.IsNullOrEmpty(product[0].ProductType))
				{
					ddlProductType.Text = product[0].ProductType;
				}

				if (product[0].ProductStatus!=null)
				{
					ddlProductStatus.Text = (product[0].ProductStatus).ToString();
				}
				
				ddlStockStatus.Text = product[0].Balance == 0 ? "0" : "1";

                txtProductCode.Text = product[0].ProductCode;
                txtProductName.Text = product[0].ProductName;
                txtProductValue.Text = (product[0].ProductValue??0).ToString();
				txtProductSaleValue.Text = (product[0].ProductSaleValue??0).ToString();
				txtProductLable.Text = product[0].ProductLable;

                txtSize.Text = product[0].Size;
                txtImage.Text = product[0].Image;
                txtImageTempOld.Text = product[0].Image;
                imgView.ImageUrl = product[0].Image;
				if (product[0].Published != null)
				{
					ckbPublished.Checked = (bool)product[0].Published;
				}
                txtTag.Text = product[0].Tag;
                txtShortDescription.Text = product[0].ShortDescription;
                txtDescription.Text = product[0].Description;

				if (!string.IsNullOrEmpty(product[0].BestSale))
				{
					chkBestSale.Checked = product[0].BestSale == "1" ? true : false;
				}

                Session["lbtEdit_Click"] = null;
            }
            else
            {
                lblFunction.Text = "Thêm";
                Session["ProductCode"] = null;
                loadParentComboBox("");
                loadVenderComboBox("");

            }
        }

        private void loadCategoryComboBox()
        {
            if (string.IsNullOrEmpty(ddCategoryIdParent.SelectedItem.Value) && ddCategoryIdParent.SelectedItem.Value == "-1")
            {
                List<Category> temp = new List<Category>();
                temp.Add(new Category() { CategoryId = -1, CategoryName = "-- Chọn loại sản phẩm --" });
                ddCategoryId.DataSource = temp;
                ddCategoryId.DataTextField = "CategoryName";
                ddCategoryId.DataValueField = "CategoryId";
                ddCategoryId.DataBind();
            }
            else
            {
                // Lấy loại sản phẩm
                List<Category> temp = new List<Category>();
                temp.Add(new Category() { CategoryId = -1, CategoryName = "-- Chọn nhóm sản phẩm --" });
                temp.AddRange(this._productService.CategoryGet("", "", ddCategoryIdParent.SelectedItem.Value, this));
                ddCategoryId.DataSource = temp;
                ddCategoryId.DataTextField = "CategoryName";
                ddCategoryId.DataValueField = "CategoryId";
                ddCategoryId.DataBind();
            }

        }

        private void loadVenderComboBox(string venderId)
        {           
			var lst = _productService.VendorGet("", "", this);
			ddVendorId.DataSource = lst;
			ddVendorId.DataTextField = "VendorName";
			ddVendorId.DataValueField = "VendorId";
			ddVendorId.DataBind();
			ddVendorId.Items.Insert(0, new ListItem("-- Chọn nhà cung cấp --", "-1"));

			if (!String.IsNullOrEmpty(venderId))
				ddVendorId.Items.FindByValue(venderId).Selected = true;
        }

        private void loadParentComboBox(string categoryId)
        {
            // Lấy Nhóm sản phẩm
            List<Category> temp = new List<Category>();
            temp.Add(new Category() { CategoryId = -1, CategoryName = "-- Chọn loại sản phẩm --" });
            temp.AddRange(this._productService.CategoryGet("", "", "0", this));
            ddCategoryIdParent.DataSource = temp;
            ddCategoryIdParent.DataTextField = "CategoryName";
            ddCategoryIdParent.DataValueField = "CategoryId";
            ddCategoryIdParent.DataBind();

            if (String.IsNullOrEmpty(categoryId))
            {

                loadCategoryComboBox();
            }
            else
            {
                var temp2 = this._productService.CategoryGet(categoryId, "", "", this);
                string parentid = String.IsNullOrEmpty(temp2[0].ParentId.ToString()) ? "-1" : temp2[0].ParentId.ToString();
                ddCategoryIdParent.Items.FindByValue(parentid).Selected = true;
                loadCategoryComboBox();
                ddCategoryId.Items.FindByValue(categoryId).Selected = true;
            }
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