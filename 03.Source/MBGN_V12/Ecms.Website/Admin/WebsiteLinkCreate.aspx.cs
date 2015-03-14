using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Class;

namespace Ecms.Website.Admin
{
    public partial class WebsiteLinkCreate : PageBase
    {

        #region // Declaration

        private readonly WebsiteService _websiteService = new WebsiteService();

        #endregion


        #region // Even
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (Session["User"] == null)
                        Response.Redirect("~/Admin/Security/Login.aspx");

                    loadParentIdComboBox();
                    loadEdit();
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
                if (Session["WebsiteLinkCode"] != null)
                {
                    string id = (string)Session["WebsiteLinkCode"];
                    var item = new WebsiteLink();
                    item.WebsiteId = Convert.ToInt32(id);
                    item.WebsiteName = txtWebsiteName.Text;
                    item.WebLink = txtWebsiteLink.Text;
                    int parentId = Convert.ToInt32(ddParentId.SelectedItem.Value);
                    if (parentId == -1)
                        item.ParentId = null;
                    else
                        item.ParentId = parentId;

                    item.Description = txtDescription.Text;
                    Session["WebsiteLinkCode"] = null;
                    var result = this._websiteService.WebsiteLinkUpdate(item, this);
                    if (result.WebsiteId != 0)
                            DisplayAlert("Hệ thống cập nhật thành công", "WebsiteLinkList.aspx");
                }
                else
                {
                    var item = new WebsiteLink();
                    item.WebsiteName = txtWebsiteName.Text;
                    item.WebLink = txtWebsiteLink.Text;

                    int parentId = Convert.ToInt32(ddParentId.SelectedItem.Value);
                    if (parentId == -1)
                        item.ParentId = null;
                    else
                        item.ParentId = parentId;

                    item.Description = txtDescription.Text;
                    var result = this._websiteService.WebsiteLinkCreate(item, this);
                    if (result.WebsiteId != 0)
                    DisplayAlert("Hệ thống cập nhật thành công", "WebsiteLinkList.aspx");
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebsiteLinkList.aspx");
        }

        protected void ddParentId_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region // Method

        private void loadEdit()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                btnSave.Text = "Lưu";
                lblFunction.Text = "Sửa";
                string id = (string)Session["lbtEdit_Click"];
                Session["WebsiteLinkCode"] = id;
                var websiteLink = this._websiteService.WebsiteLinkGet(id, "", "", "", this);
                ddParentId.ClearSelection();

                if (null != websiteLink[0].ParentId)
                    ddParentId.Items.FindByValue(Convert.ToString(websiteLink[0].ParentId)).Selected = true;

                txtWebsiteName.Text = websiteLink[0].WebsiteName;
                txtWebsiteLink.Text = websiteLink[0].WebLink;
                Session["lbtEdit_Click"] = null;
            }
            else
            {
                lblFunction.Text = "Thêm";
                txtWebsiteName.Text = "";
                txtWebsiteLink.Text = "";

            }
        }

        private void loadParentIdComboBox()
        {
            List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
            temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Tất cả --" });
            temp.AddRange(this._websiteService.WebsiteLinkGet("", "", "", "0", this));

            ddParentId.DataSource = temp;
            ddParentId.DataTextField = "WebsiteName";
            ddParentId.DataValueField = "WebsiteId";
            ddParentId.DataBind();
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