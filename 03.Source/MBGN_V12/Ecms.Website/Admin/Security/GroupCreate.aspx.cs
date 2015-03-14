using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class GroupCreate : PageBase
    {
        #region //Declares

        private IUserBiz cService = new UserBiz();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (Session["GroupCode"] != null)
                {
                    string id = (string)Session["GroupCode"];
                    var item = new Sys_Group
                    {
                        GroupCode = id,
                        GroupName = txtGroupName.Text.Trim()
                    };
                    cService.UpdateGroup(item);
                    Session.Remove("GroupCode");
                    mtvMain.ActiveViewIndex = 1;
                    lblResult.Text = "Sửa nhóm thành công";
                }
                else
                {
                    if (ValidData() == false)
                        return;

                    var project = new Sys_Group
                    {
                        GroupCode = txtGroupCode.Text.Trim(),
                        GroupName = txtGroupName.Text.Trim()
                    };
                    cService.AddGroup(project);
                    mtvMain.ActiveViewIndex = 1;
                    lblResult.Text = "Thêm mới nhóm thành công";
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupList.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupList.aspx");
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                litContentTitle.Text = litPageTitle.Text = "Sửa";
                string id = (string)Session["lbtEdit_Click"];
                Session["GroupCode"] = id;
                var project = cService.GetGroup(id, "").FirstOrDefault();
                if (project != null)
                {
                    txtGroupCode.Enabled = false;
                    txtGroupCode.Text = project.GroupCode;
                    txtGroupName.Text = project.GroupName;
                    Session.Remove("lbtEdit_Click");
                }
            }
        }

        private bool ValidData()
        {
            if (string.IsNullOrEmpty(txtGroupCode.Text))
            {
                lblError.Text = "Bạn chưa nhập mã nhóm";
                lblError.Visible = true;
                return false;
            }

            if (txtGroupCode.Text.Contains(" ") == true)
            {
                lblError.Text = "Mã nhóm không chứa khoảng trắng!";
                lblError.Visible = true;
                return false;
            }
            var groups = cService.GetGroup(txtGroupCode.Text, "").FirstOrDefault();
            if (groups != null)
            {
                lblError.Text = "Mã nhóm này đã tồn tại!";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

        #endregion
    }
}