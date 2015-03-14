using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Ecms.Website.Common;
using Ecms.Website.DBServices;

namespace Ecms.Website.Site.PartControl
{
    public partial class Login : System.Web.UI.UserControl
    {
        #region Declares

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["Customer"] != null)
				{
					Response.Redirect("~/site/default.aspx");
				}
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var userName = txtUserName.Text;
                var password = txtPassword.Text;

                var sysUser = new CustomerService().Logon(userName, password, "0", this.Page);
                if (sysUser != null)
				{

					#region // calfeeDelay
					//var fee = new OrderService().CalDelayFeeOrder(this.Page);
					#endregion

					Session["Customer"] = sysUser;
                    FormsAuthentication.SetAuthCookie(sysUser.UserCode, false);

					if (Session["Quotation"] != null && !string.IsNullOrEmpty(Request.QueryString["qtacart"]) && Request.QueryString["qtacart"].Equals(CommonUtils.Constansts.FlagActive))
					{
						Response.Redirect("~/site/mbgn/confirmquotation.aspx");
					}
					else if (Session["Order"] != null && !string.IsNullOrEmpty(Request.QueryString["ordercart"]) && Request.QueryString["ordercart"].Equals(CommonUtils.Constansts.FlagActive))
					{
						// if orderCart before login then forword to below url
						Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");
					}
					else
					{
						if (Request.QueryString["returnUrl"] != null)
						{
							Response.Redirect(Request.QueryString["returnUrl"]);
						}
						else
						{
							Response.Redirect("~/site/mbgn/OrderProduct.aspx");
						}
					}
					
                    return;
                }
                lblError.Text = "Tên đăng nhập hoặc mật khẩu không đúng";
                lblError.Visible = true;
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

		protected void link1_OnClick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request.QueryString["ordercart"]) && Request.QueryString["ordercart"].Equals(CommonUtils.Constansts.FlagActive))
			{
				Response.Redirect("~/site/mbgn/Register.aspx?ordercart=1");
			}
			else
			{
				Response.Redirect("~/site/mbgn/Register.aspx");
			}
		}

        #endregion
    }
}