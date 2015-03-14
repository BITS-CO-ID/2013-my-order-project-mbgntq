using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Ecms.Website.Site.PartControl
{
    public partial class LeftNavigation2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] != null)
                {
                    var strBuilder = new StringBuilder();
                    strBuilder.Append("<h2>Quản lý tài khoản cá nhân</h2>");
                    strBuilder.Append("<ul class='nav'><li><a href='{0}'>Đổi mật khẩu</a></li>");
                    strBuilder.Append("<li><a href='{1}'>Thay đổi thông tin cá nhân</a></li>");
					//strBuilder.Append("<li><a href='{2}'>Quản lý thanh toán</a></li></ul>");
                    var strMenu = string.Format(strBuilder.ToString()
												, ResolveUrl("~/site/mbgn/ChangePassword.aspx")
												, ResolveUrl("~/site/mbgn/EditProfile.aspx")
												//, ResolveUrl("~/site/mbgn/InvoiceManage.aspx")
												);
                    litCustomerLoginFunction.Text = strMenu;
                    litCustomerLoginFunction.Visible = true;
                }
                else
                {
                    litCustomerLoginFunction.Visible = false;
                }
            }
        }
    }
}