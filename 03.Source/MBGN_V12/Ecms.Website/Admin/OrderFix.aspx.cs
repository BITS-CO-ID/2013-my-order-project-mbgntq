using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Biz;
using Ecms.Website.DBServices;
using Ecms.Website.Common;

namespace Ecms.Website.Admin
{
	public partial class OrderFix : Page
    {

        #region // Declaration

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Admin/Security/Login.aspx");
                }
            }
        }

		protected void btnOrderFix_OnClick(object sender, EventArgs e)
		{ 
			string strRef="";
			var orderfix = new OrderBiz().OrderFixBusinessInvoiceCode(ref strRef);
		}

		protected void btnOrderFixInvoice_OnClick(object sender, EventArgs e)
		{
			string strRef = "";
			var orderfix = new OrderBiz().OrderFixInvoice(ref strRef);
		}
        #endregion

        #region // Method

        #endregion
    }
}