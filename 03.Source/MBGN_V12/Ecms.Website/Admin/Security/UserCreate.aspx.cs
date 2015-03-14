using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class UserCreate : PageBase
    {
        #region // Declares

        private IUserBiz cService = new UserBiz();
        private ICommonBiz _commonService = new CommonBiz();

        #endregion

        #region // Events

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
                if (Session["UserCode"] != null)
                {
                    if (ValidDataEdit() == false)
                        return;

                    string userCode = (string)Session["UserCode"];
                    var item = new Sys_User();
                    item.UserCode = userCode;
                    item.UserName = txtUserName.Text.Trim();
                    item.Remark = txtDescription.Text;
                    item.FlagActive = chkStatus.Checked ? "1" : "0";
					item.SupperAdmin = chkSupperAdmin.Checked ? "1" : "0";
                    item.Email = txtEmail.Text;
                    cService.UpdateUser(item);

                    mtvMain.ActiveViewIndex = 1;
                    lblResult.Text = "Sửa người dùng thành công";
                    Session.Remove("UserCode");
                }
                else
                {
                    if (ValidDataCreate() == false)
                        return;

                    var user = new Sys_User
                    {
                        UserName = txtUserName.Text.Trim(),
                        UserPassword = Utilities.Encrypt(txtPassword.Text.Trim()),
                        Remark = txtDescription.Text,
                        FlagActive = chkStatus.Checked ? "1" : "0",
                        FlagAdmin = "1",
						SupperAdmin = chkSupperAdmin.Checked ? "1" : "0",
                        UserCode = txtUserCode.Text.Trim(),
                        Email = txtEmail.Text
                    };
                    cService.AddUser(user);

                    mtvMain.ActiveViewIndex = 1;
                    lblResult.Text = "Thêm mới người dùng thành công";
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserList.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserList.aspx");
        }

        #endregion

        #region // Private methods

        private void LoadData()
        {
            try
            {
                if (Session["lbtEdit_Click"] != null)
                {
                    litFunction.Text = "Sửa";
                    string userCode = (string)Session["lbtEdit_Click"];
                    Session["UserCode"] = userCode;
                    var user = cService.GetUser(userCode, "", "", "", "").FirstOrDefault();
                    if (user != null)
                    {
                        txtUserCode.Enabled = trConfirmPassword.Visible = trPassword.Visible = false;
                        txtDescription.Text = user.Remark;
                        txtUserCode.Text = user.UserCode;
                        txtEmail.Text = user.Email;
                        txtUserName.Text = user.UserName;
                        chkStatus.Checked = (user.FlagActive == "1");
						chkSupperAdmin.Checked = (user.SupperAdmin == "1");
                        Session.Remove("lbtEdit_Click");
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private bool ValidDataCreate()
        {
            if (string.IsNullOrEmpty(txtUserCode.Text.Trim()))
            {
                lblError.Text = "Tên đăng nhập không được để trống.";
                lblError.Visible = true;
                return false;
            }
            try
            {
                var user = cService.GetUser(txtUserCode.Text.Trim(), "", "", "", "").FirstOrDefault();
                if (user != null)
                {
                    lblError.Text = "Tên đăng nhập này đã tồn tại.";
                    lblError.Visible = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                lblError.Text = "Mật khẩu không được để trống.";
                lblError.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtConfirm.Text.Trim()))
            {
                lblError.Text = "Mật khẩu xác nhận không được để trống.";
                lblError.Visible = true;
                return false;
            }
            if (!txtConfirm.Text.Trim().Equals(txtPassword.Text.Trim()))
            {
                lblError.Text = "Mật khẩu xác nhận không đúng.";
                lblError.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                lblError.Text = "Email không được để trống.";
                lblError.Visible = true;
                return false;
            }
            return true;
        }

        private bool ValidDataEdit()
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                lblError.Text = "Email không được để trống.";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

        #endregion
    }
}