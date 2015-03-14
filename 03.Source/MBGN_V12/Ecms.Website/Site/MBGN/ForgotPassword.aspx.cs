using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using System.Text;

namespace Ecms.Website.Site.MBGN
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        #region //Declares

        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region //Events

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = _customerService.CustomerList("", "", "", txtEmail.Text,"","","", "","","", "", this).FirstOrDefault();
                if (customer == null)
                {
                    lblError.Text = "Email chưa tồn tại trên hệ thống!";
                    lblError.Visible = true;
                    return;
                }
                _customerService.ResetPassword(txtEmail.Text, "0", this);
                mtvMain.ActiveViewIndex = 1;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex,this);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        #endregion
    }
}