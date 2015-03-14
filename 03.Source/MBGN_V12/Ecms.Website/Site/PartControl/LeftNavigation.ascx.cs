using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using System.Text;

namespace Ecms.Website.Site.PartControl
{
    public partial class LeftNavigation : System.Web.UI.UserControl
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            try
            {
                var websites = _commonService.WebsiteList("", "", "");
                var strParentWebsite = new StringBuilder();
                int type = 1;
                foreach (var item in websites.Where(x=>x.ParentId == null))
                {
					string strParentWebFormat = string.Format("<li class='type{0}'><a href='#'>{1}</a><div><div class='nav-column'><h3>{2}</h3><ul>"
												, type
												, item.WebsiteName
												, item.WebsiteName);

                    strParentWebsite.Append(strParentWebFormat);

                    foreach (var item1 in websites.Where(x=>x.ParentId == item.WebsiteId).Take(9)) // Only show 9 item
                    {
						strParentWebsite.Append(string.Format("<li><a href='{0}'>{1}</a></li>", item1.WebLink, item1.WebsiteName));
                    }

                    strParentWebsite.Append("</ul></div>");
                    type++;
                }
                litWebsiteLink.Text = strParentWebsite.ToString();
            }catch(Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
            }
        }

        #endregion
    }
}