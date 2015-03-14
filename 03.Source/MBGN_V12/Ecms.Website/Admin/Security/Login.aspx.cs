using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using CommonUtils;
using Ecms.Website.DBServices;

namespace Ecms.Website.Admin.Security
{
    public partial class Login : System.Web.UI.Page
    {
        private IUserBiz cService = new UserBiz();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["LoginErrors"] = 0;
            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            if (Validate(Login1.UserName, Login1.Password))
            {
                Response.Redirect("~/admin/order/ordermanage.aspx");
            }
            else
            {
                e.Authenticated = false;
            }
        }

        private bool Validate(string userName, string pass)
        {
            bool returnValue = false;

            var login = cService.GetUser(userName, "", "", "1", "");
            if (login.Count > 0)
            {
                foreach (var user in login)
                {
                    if (user.UserPassword == Utilities.Encrypt(pass))
                    {
                        returnValue = true;
                        Session["UserModel"] = login;
                        Session["User"] = login.SingleOrDefault().UserCode;
                    }
                }
            }

			// calfeeDelay
			//var fee = new OrderService().CalDelayFeeOrder(this);

            return returnValue;
        }

        protected void Login1_LoginError(object sender, EventArgs e)
        {
            if (ViewState["LoginErrors"] == null)
                ViewState["LoginErrors"] = 0;

            int errorCount = (int)ViewState["LoginErrors"] + 1;
            ViewState["LoginErrors"] = errorCount;

            if ((errorCount > 3) && (Login1.PasswordRecoveryUrl != string.Empty))
                Response.Redirect(Login1.PasswordRecoveryUrl);
        }
    }
}