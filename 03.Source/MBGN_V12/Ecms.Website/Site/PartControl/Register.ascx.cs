using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using System.Web.Security;

namespace Ecms.Website.Site.PartControl
{
    public partial class Register : System.Web.UI.UserControl
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
                var newCustomer= _customerService.CustomerRegister(
								userName.Trim().ToUpper()
								, txtPassword.Text
								, txtFullName.Text
								, txtMobile.Text
								, email
								, txtAddress.Text
								, ddlProvince.SelectedValue
								, this.Page);

				if (newCustomer != null)
				{
					var sysUser = new CustomerService().Logon(userName, txtPassword.Text, "0", this.Page);
					if (sysUser != null)
					{
						Session["Customer"] = sysUser;
						FormsAuthentication.SetAuthCookie(sysUser.UserCode, false);

						if(!string.IsNullOrEmpty(Request.QueryString["ordercart"]) && Request.QueryString["ordercart"].Equals(CommonUtils.Constansts.FlagActive))
						{
							Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");

						}else if (Request.QueryString["returnUrl"] != null)
						{
							Response.Redirect(Request.QueryString["returnUrl"]);
						}
						else
						{
							mtvMain.ActiveViewIndex = 1;
						}
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
                    lblError.Text = "Email đã được đăng ký trên hệ thống!";
                    lblError.Visible = true;
                    return false;
                }
                customer = _customerService.CustomerList("", "", "", "", "", "", "", userName, "", "", "", this.Page).FirstOrDefault();
                if (customer != null)
                {
                    lblError.Text = "Tài khoản này đã được đăng ký trên hệ thống trước đó!";
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
    }
}