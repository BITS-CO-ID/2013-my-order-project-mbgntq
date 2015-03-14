using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;

namespace Ecms.Website.Admin.Security
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region //Declares

        private IUserBiz _userBiz = new UserBiz();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var listUserModel = (List<UserModel>)Session["UserModel"];
                var userFirst = listUserModel.FirstOrDefault();
                if (userFirst != null)
                {
                    txtUserCode.Text = userFirst.UserCode;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidData() == false)
                return;

            var listUserModel = (List<UserModel>)Session["UserModel"];
            var userFirst = listUserModel.FirstOrDefault();
            if (userFirst != null)
            {
                string alParamsOutError = "";
                _userBiz.UserChangedPassword(userFirst.UserCode, Utilities.Decrypt(userFirst.UserPassword), txtNewPass.Text, ref alParamsOutError);
                userFirst.UserPassword = Utilities.Encrypt(txtNewPass.Text.Trim());
                listUserModel.Clear();
                listUserModel.Add(userFirst);
                Session["UserModel"] = listUserModel;
                lblResult.Text = "Đổi mật khẩu thành công.";
                mtvMain.ActiveViewIndex = 1;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/admin.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/admin.aspx");
        }

        #endregion

        #region //Private methods

        public bool ValidData()
        {
            if (string.IsNullOrEmpty(txtOlderPassword.Text.Trim()))
            {
                lblError.Text = "Bạn chưa nhập mật khẩu hiện tại.";
                lblError.Visible = true;
                return false;
            }

            if (Session["UserModel"] != null)
            {
                var listUserModel = (List<UserModel>)Session["UserModel"];
                var userFirst = listUserModel.FirstOrDefault();
                if (userFirst != null)
                {
                    if (!userFirst.UserPassword.Equals(Utilities.Encrypt(txtOlderPassword.Text.Trim())))
                    {
                        lblError.Text = "Mật khẩu hiện tại không đúng.";
                        lblError.Visible = true;
                        return false;
                    }
                }
            }


            if (string.IsNullOrEmpty(txtNewPass.Text.Trim()))
            {
                lblError.Text = "Bạn chưa nhập mật khẩu mới.";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtConfirm.Text.Trim()))
            {
                lblError.Text = "Bạn chưa nhập mật khẩu xác nhận.";
                lblError.Visible = true;
                return false;
            }

            if (txtNewPass.Text.Trim() != txtConfirm.Text.Trim())
            {
                lblError.Text = "Mật khẩu xác nhận không đúng.";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

        #endregion
    }
}