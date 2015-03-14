using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
namespace Ecms.Website.Admin.PartControl
{
    public partial class LoginControl : System.Web.UI.UserControl
    {
        //private IUserService _userService = new UserService();
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        //protected void btnLogin_Click(object sender, EventArgs e)
        //{
        //    var userName = txtUsername.Text;
        //    var password = txtPassword.Text;

        //    var user = _userService.Login(userName,password);
        //    if (user != null)
        //    {
        //        Session[CONST.SS_USER] = user;
        //        FormsAuthentication.SetAuthCookie(user.Username, false);
        //        Response.Redirect("ImportLicense.aspx");
        //        return;
        //    }

        //    lblError.Text = CONST.MES_ACCOUNT_INVALID;
        //    lblError.Visible = true;
        //}
    }
}