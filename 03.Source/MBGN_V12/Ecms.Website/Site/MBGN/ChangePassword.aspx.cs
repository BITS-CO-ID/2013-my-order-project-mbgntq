using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using System.Threading;

namespace Ecms.Website.Site.MBGN
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region //Declares

        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] == null)
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx");
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Customer"] != null)
                {
                    var customer = (UserCustomerModel)Session["Customer"];
					var currentPassword = Utilities.Encrypt(txtCurrentPassword.Text);
                    if (!customer.Password.Equals(currentPassword))
                    {
                        lblError.Text = "Mật khẩu hiện tại không đúng!";
                        lblError.Visible = true;
                        return;
                    }
                    var newPassword = txtNewPassword.Text;
                    _customerService.ChangePassword(customer.UserCode, txtCurrentPassword.Text, newPassword
                                                    , this);
					customer.Password = Utilities.Encrypt(newPassword);
                    Session["Customer"] = customer;
                    mtvMain.ActiveViewIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
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
        #endregion
    }
}