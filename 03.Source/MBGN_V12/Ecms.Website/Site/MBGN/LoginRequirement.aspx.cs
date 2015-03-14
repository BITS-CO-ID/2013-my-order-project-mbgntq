using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using System.Web.Security;

namespace Ecms.Website.Site.MBGN
{
    public partial class LoginRequirement : System.Web.UI.Page
    {
        #region //Declares

        private readonly CustomerService _customerService = new CustomerService();
        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                var userName = txtUsername.Text;
                var email = txtEmail.Text;
                if (ValidData(userName, email) == false)
                    return;
                _customerService.CustomerRegister(userName.Trim(), txtPassword.Text, txtFullName.Text, txtMobile.Text,
                                        email, txtAddress.Text, ddlProvince.SelectedValue, this.Page);

                var sysUser = new CustomerService().Logon(userName, txtPassword.Text, "0", this.Page);
                if (sysUser != null)
                {
                    Session["Customer"] = sysUser;
                    FormsAuthentication.SetAuthCookie(sysUser.UserCode, false);

                    if (Request.QueryString["returnUrl"] != null)
                    {
                        Response.Redirect(Request.QueryString["returnUrl"]);
                    }
                    else
                    {
                        mtvMain.ActiveViewIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/mbgn/login.aspx");
        }

        #endregion

        #region //Private Methods

        private void InitData()
        {
            try
            {
                if (Session["Customer"] != null)
                    Response.Redirect("~/site/default.aspx");                

                ddlProvince.DataSource = _commonService.ProvinceList("", "", "");
                ddlProvince.DataTextField = "CityName";
                ddlProvince.DataValueField = "CityId";
                ddlProvince.DataBind();
                ddlProvince.Items.Insert(0, new ListItem("-- Tỉnh/ Thành phố", ""));

				if(rdLogin.Checked)
				{
					btnLogin1.Focus();
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
            }
        }

        private bool ValidData(string userName, string email)
        {
            try
            {
                var customer = _customerService.CustomerList("", "", "", email, "", "", "", "", "", "", "", this.Page).FirstOrDefault();
                if (customer != null)
                {
                    lblError.Text = "Email đã tồn tại trên hệ thống!";
                    lblError.Visible = true;
                    return false;
                }
                customer = _customerService.CustomerList("", "", "", "", "", "", "", userName, "", "", "", this.Page).FirstOrDefault();
                if (customer != null)
                {
                    lblError.Text = "Tài khoản đã tồn tại trên hệ thống!";
                    lblError.Visible = true;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
                return false;
            }
        }

        #endregion

        protected void btnLogin1_Click(object sender, EventArgs e)
        {
            try
            {
                var userName = txtUserName1.Text;
                var password = txtPassword1.Text;

                var sysUser = new CustomerService().Logon(userName, password, "0", this.Page);
                if (sysUser != null)
                {
                    Session["Customer"] = sysUser;
                    FormsAuthentication.SetAuthCookie(sysUser.UserCode, false);
                    if (Request.QueryString["returnUrl"] != null)
                    {
                        Response.Redirect(Request.QueryString["returnUrl"]);
                    }
                    else
                    {
                        Response.Redirect("~/site/mbgn/OrderProduct.aspx");
                    }
                    return;
                }
                lblError1.Text = "Tên đăng nhập hoặc mật khẩu không đúng";
                lblError1.Visible = true;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
            }
        }

        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }        
    }
}