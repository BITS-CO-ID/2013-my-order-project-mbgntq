using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using System.Text;
using Ecms.Website.Common;

namespace Ecms.Website.Site.PartControl
{
    public partial class SlideAdverties : System.Web.UI.UserControl
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

        #region // Private methods
        private void LoadData()
        {
            //try
            //{
            //    var websiteLink = _commonService.WebsiteList("", "","").Where(x=>x.ParentId != null).Take(8);
            //    var strWebsiteLink = new StringBuilder();

            //    foreach (var item in websiteLink)
            //    {
            //        strWebsiteLink.Append("<li class='span2'><div class='client-box'>");
            //        strWebsiteLink.Append(string.Format("<a href='{2}' target='_blank'><img alt='{0}' src='{1}' /></a></div></li>", item.WebsiteName, item.WebsiteImage,item.WebLink));
            //    }
            //    litWebsiteLink.Text = strWebsiteLink.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Utils.ShowExceptionBox(ex, this.Page);
            //}
        }
        #endregion
    }
}