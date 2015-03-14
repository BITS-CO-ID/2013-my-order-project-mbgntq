using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;

namespace Ecms.Website.Site.MBGN
{
    public partial class NewsDetail : System.Web.UI.Page
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
                if (Request.QueryString["NewsId"] != null)
                {
                    var news = _commonService.NewsGet(Request.QueryString["NewsId"], "", "", this).FirstOrDefault();
                    if (news != null)
                    {
                        if (Request.QueryString["NewsId"].ToString().Equals("53"))
                        {
                            this.fbMain.Visible = true;
                        }

                        lblNewsTitle.Text  = news.Title;
                        lblNewsContent.Text = news.NewsContent;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }
        #endregion
    }
}