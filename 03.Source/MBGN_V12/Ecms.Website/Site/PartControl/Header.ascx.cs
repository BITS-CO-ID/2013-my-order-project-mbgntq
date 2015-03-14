using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Website.Site.PartControl
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] != null)
                {
                    var sysUser = (UserCustomerModel)Session["Customer"];

                    lblAccountInfo.Text = string.Format("Xin chào: {0}", string.Format("<a href='{0}'>"+sysUser.UserName.ToString()+"</a>",ResolveUrl("~/site/mbgn/editprofile.aspx")));
                    pnInfoAccount.Visible = true;
                    pnLinkLoginAndRegister.Visible = false;
                }
                else
                {
                    pnInfoAccount.Visible = false;
                    pnLinkLoginAndRegister.Visible = true;
                }
            }
        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/site/default.aspx");
        }
    }
}