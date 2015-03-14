using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Entities;

namespace Ecms.Website.Site.PartControl
{
    public partial class Feedback : System.Web.UI.UserControl
    {
        #region //Declares

        private readonly NewsService _newsService = new NewsService();
        
        #endregion

        #region //Events

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                var news = new News();
                news.CreatedDate = DateTime.Now;
                news.Email = txtEmail.Text;
                news.FullName = txtFullName.Text;
                news.NewsContent = txtContent.Text;
                news.Title = txtTitle.Text;
                news.Type = 2;
                var newsReturn = _newsService.NewsCreate(news, this.Page);
                if (newsReturn != null)
                {
                    mtvMain.ActiveViewIndex = 1;    
                }                
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
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