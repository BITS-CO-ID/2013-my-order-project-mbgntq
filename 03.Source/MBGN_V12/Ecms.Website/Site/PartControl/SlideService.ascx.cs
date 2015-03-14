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
    public partial class SlideService : System.Web.UI.UserControl
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // LoadData();
            }
        }

        #endregion

        #region // Private methods
        //private void LoadData()
        //{
        //    try
        //    {
        //        var news = _commonService.NewsGet("", "", "", this.Page).Where(x => x.Type == 0);
        //        var strService = new StringBuilder();

        //        foreach (var item in news)
        //        {
        //            strService.Append("<li><div class='client-box'>");

        //            strService.Append(string.Format("<a href='{4}'><img alt='{0}' src='{1}' /></a><a href='{2}'>{3}</a></div></li>", item.Title, item.NewsImage,
        //                                            ResolveUrl("~/site/mbgn/NewsDetail.aspx?NewsId=" + item.NewsId), item.Title,
        //                                            ResolveUrl("~/site/mbgn/NewsDetail.aspx?NewsId=" + item.NewsId)));
        //        }
        //        litService.Text = strService.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.ShowExceptionBox(ex, this.Page);
        //    }
        //}
        #endregion
    }
}