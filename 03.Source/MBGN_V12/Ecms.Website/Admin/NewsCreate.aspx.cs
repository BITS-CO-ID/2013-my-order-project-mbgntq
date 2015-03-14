using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Entities;
using CommonUtils;
using System.Web.Security;
using System.IO;
using Ecms.Biz;
using Ecms.Biz.Class;

namespace Ecms.Website.Admin
{
    public partial class NewsCreate : PageBase
    {
        #region // Declaration

        private readonly NewsService _newsService = new NewsService();
        private readonly WebsiteService _websiteService = new WebsiteService();

        #endregion


        #region // Even

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
                        String filePath = "~/Upload/NewsImage/" + fupImage.FileName;
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
                if (ddType.SelectedItem.Value == "-1")
                {
                    lblError.Text = "Loại bài viết không được để trống.";
                    return;
                }

                if (ddType.SelectedItem.Value == "1" && ddWebsiteId.SelectedItem.Value == "-1")
                {
                        lblError.Text = "WebsiteId không được để trống.";
                        return;
                }


                List<UserModel> user = (List<UserModel>)Session["UserModel"];
                if (user== null)
                {
                    Response.Redirect("./Security/Login.aspx");
                }

                if (Session["NewsCode"] != null)
                {
                    string id = (string)Session["NewsCode"];
                    var item = new News();
                    item.NewsId = Convert.ToInt32(id);
                    item.Title = txtTitle.Text;
                    item.Type = Convert.ToInt32(ddType.SelectedItem.Value);

                    if (ddType.SelectedItem.Value == "1")
                    {
                        item.WebsiteId = Convert.ToInt32(ddWebsiteId.SelectedItem.Value);
                    }

                    item.Published = ckbPublished.Checked;
                    item.NewsImage = txtImage.Text;
                    if ((txtImageTempOld.Text != txtImage.Text) && Utilities.existFile(txtImageTempOld.Text))
                        Utilities.deleteFile(Server.MapPath(".") + txtImageTempOld.Text);
                    item.CreatedDate = DateTime.Now;
                    item.ShortContent = txtShortContentCK.Text;
                    item.NewsContent = txtNewsContentCK.Text;
                    item.CreateUser = user[0].UserName;
                    var result = this._newsService.NewsUpdate(item, this);
                    if (result.NewsId !=0)
                        DisplayAlert("Hệ thống cập nhật thành công", "NewsList.aspx");
                }
                else
                {
                    var item = new News();
                    item.Title = txtTitle.Text;
                    item.Type = Convert.ToInt32(ddType.SelectedItem.Value);

                    if (ddType.SelectedItem.Value == "1")
                    {
                        item.WebsiteId = Convert.ToInt32(ddWebsiteId.SelectedItem.Value);
                    }
                    item.AllowDelete = true;
                    item.Published = ckbPublished.Checked;
                    item.NewsImage = txtImage.Text;
                    item.ShortContent = txtShortContentCK.Text;
                    item.NewsContent = txtNewsContentCK.Text;
                    item.CreatedDate = DateTime.Now;
                    item.CreateUser = user[0].UserName;
                    var result = this._newsService.NewsCreate(item, this);
                    if (result.NewsId != 0)
                        DisplayAlert("Hệ thống cập nhật thành công", "NewsList.aspx");
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
                    Utilities.deleteFile(Server.MapPath(".") +txtImage.Text);
                Response.Redirect("NewsList.aspx");
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

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddType.SelectedItem.Value == "1")
            {
                ddWebsiteId.Enabled = true;
            }
            else
            {
                ddWebsiteId.Enabled = false;
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
                Session["NewsCode"] = id;
                var news = this._newsService.NewsGet(id, "", "", "", "", "", "", "", this);
                txtTitle.Text = news[0].Title;
                string type = String.IsNullOrEmpty(news[0].Type.ToString()) ? "-1" : news[0].Type.ToString();

                if (type == "1")
                {
                    ddWebsiteId.Enabled = true;
                }

                ddType.Items.FindByValue(type).Selected = true;
                loadWebsiteComboBox(Convert.ToString(news[0].WebsiteId));
                txtImage.Text = news[0].NewsImage;
                txtImageTempOld.Text = news[0].NewsImage;
                imgView.ImageUrl = news[0].NewsImage;
                if (news[0].Published != null)
                    ckbPublished.Checked = (bool)news[0].Published;
                txtShortContentCK.Text = news[0].ShortContent;
                txtNewsContentCK.Text = news[0].NewsContent;
                Session["lbtEdit_Click"] = null;
            }
            else
            {
                lblFunction.Text = "Thêm";
                Session["ProductCode"] = null;
                loadWebsiteComboBox("");

            }
        }



        private void loadWebsiteComboBox(string websiteId)
        {
            // Lấy nhà cung cấp
            List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
            temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Chọn website --" });
            temp.AddRange(this._websiteService.WebsiteLinkGet("","","","-1",this));
            ddWebsiteId.DataSource = temp;
            ddWebsiteId.DataTextField = "WebsiteName";
            ddWebsiteId.DataValueField = "WebsiteId";
            ddWebsiteId.DataBind();
            if (!String.IsNullOrEmpty(websiteId))
                ddWebsiteId.Items.FindByValue(websiteId).Selected = true;
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