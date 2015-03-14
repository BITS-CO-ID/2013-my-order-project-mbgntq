
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;

namespace Ecms.Website.Admin.PartControl
{
    public partial class UserInfo : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserModel"] != null)
                {
                    var user = ((List<UserModel>)Session["UserModel"]).FirstOrDefault();
                    lblAccountInfo.Text = string.Format("Xin chào: {0} | ", user.UserName);
                    lbtnLogout.Visible = true;
                }
            }
        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/admin/security/login.aspx");
        }
    }
}