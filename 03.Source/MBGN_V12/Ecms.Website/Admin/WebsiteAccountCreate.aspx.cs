using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Class;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using System.Data;

namespace Ecms.Website.Admin
{
    public partial class WebsiteAccountCreate : PageBase
    {
        #region // Declaration

        private readonly WebsiteService _websiteAccountService = new WebsiteService();
		private List<WebsiteAccountVisa> listVisa=new List<WebsiteAccountVisa>();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
			try
			{
				lblError.Text = "";
				lblError.Visible = false;

				if (Session["WebAccountCode"] != null)
				{
					// Check exist
					var webAccId = Convert.ToInt32(Session["WebAccountCode"]);
					var webs = _websiteAccountService.WebsiteAccountGet("", ddParrentWebsite.SelectedValue, "", txtAccountWebsiteNo.Text, "", this).Where(p => p.WebsiteAccountId != webAccId).ToList();
					if (webs != null && webs.Count > 0)
					{
						lblError.Text = "Website này đã có tài khoản này";
						lblError.Visible = true;
						return;
					}

					string id = Convert.ToString(Session["WebAccountCode"]);
					var item = new WebsiteAccount();

					item.WebsiteAccountId = Convert.ToInt32(id);
					item.WebsiteId = Convert.ToInt32(ddParrentWebsite.SelectedItem.Value);
					item.AccountWebsiteNo = txtAccountWebsiteNo.Text;
					
					item.Website = txtWebsite.Text;
					item.Remark = txtRemark.Text;

					Session["WebAccountCode"] = null;
					var result = this._websiteAccountService.WebsiteAccountUpdate(item, this);
					if (result.WebsiteAccountId != 0)
					{
						DisplayAlert("Hệ thống cập nhật thành công", "WebsiteAccountList.aspx");
					}
				}
				else
				{
					// Check exist
					var webs = _websiteAccountService.WebsiteAccountGet("", ddParrentWebsite.SelectedValue, "", txtAccountWebsiteNo.Text, "", this);
					if (webs != null && webs.Count > 0)
					{
						lblError.Text = "Website này đã có tài khoản này";
						lblError.Visible = true;
						return;
					}
					var item = new WebsiteAccount();
					item.WebsiteId = Convert.ToInt32(ddParrentWebsite.SelectedItem.Value);
					item.AccountWebsiteNo = txtAccountWebsiteNo.Text;					
					item.Website = txtWebsite.Text;
					item.Remark = txtRemark.Text;

					var result = this._websiteAccountService.WebsiteAccountCreate(item, this);
					Session["WebAccountCode"] = result.WebsiteAccountId;
					if (result.WebsiteAccountId != 0)
					{
						DisplayAlert("Hệ thống cập nhật thành công", "WebsiteAccountList.aspx");
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
            Response.Redirect("WebsiteAccountList.aspx");
        }

        
        #endregion

        #region // Method
        /// <summary>
        /// Load dữ liệu lên form
        /// </summary>
        private void loadEdit()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                lblFunction.Text = "Sửa";
                string id = (string)Session["lbtEdit_Click"];
				Session["WebAccountCode"] = id;

				// load combobox VisaDefault			

                var account = this._websiteAccountService.WebsiteAccountGet(id, "", "", "", "", this);

                if (null != account[0].WebsiteAccountId)
                    loadWebsiteParentComboBox(Convert.ToString(account[0].WebsiteId));

                txtAccountWebsiteNo.Text = account[0].AccountWebsiteNo;
                
                txtWebsite.Text = account[0].Website;
                txtRemark.Text = account[0].Remark;

                Session["lbtEdit_Click"] = null;
            }
            else
            {
                lblFunction.Text = "Thêm";
                Session["WebAccountCode"] = null;
                loadWebsiteParentComboBox("");				
            }
        }

        private void loadWebsiteParentComboBox(string categoryId)
        {
            // Lấy Nhóm sản phẩm đổ vào ddCategoryIdParent
            List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
            temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Tất cả --" });
            temp.AddRange(this._websiteAccountService.WebsiteLinkGet("", "", "", "0", this));
            ddParrentWebsite.DataSource = temp;
            ddParrentWebsite.DataTextField = "WebsiteName";
            ddParrentWebsite.DataValueField = "WebsiteId";
            ddParrentWebsite.DataBind();

			//if (String.IsNullOrEmpty(categoryId))
			//{

			//    loadWebsiteIdComboBox();
			//}
			//else
			//{
			//    var temp2 = this._websiteAccountService.WebsiteLinkGet(categoryId, "", "", "", this);
			//    ddParrentWebsite.ClearSelection();
			//    ddParrentWebsite.Items.FindByValue(temp2[0].ParentId.ToString()).Selected = true;
			//    loadWebsiteIdComboBox();
			//    ddWebsiteId.Items.FindByValue(categoryId).Selected = true;
			//}

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