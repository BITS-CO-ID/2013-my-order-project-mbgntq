using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using System.Text;

namespace Ecms.Website.Site.PartControl
{
    public partial class MenuTop : System.Web.UI.UserControl
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
                var menus = _commonService.MenuGet("", "", "", this.Page);
                var strMenu = new StringBuilder();
                foreach (var mnu in menus.Where(x=>x.ParentId == null && (x.Active??false) == true).OrderBy(x=>x.DisplayOrder))
                {
                    string mnuParam = string.Empty;
                    if (mnu.ObjectId != null)
                        mnuParam = "?NewsId=" + mnu.ObjectId;

                    strMenu.Append(string.Format("<li><a href='{0}'>{1}</a><ul>", ResolveUrl("~/") + mnu.Url + mnuParam, mnu.MenuName));
                    foreach (var mni in mnu.Menu1.Where(x=> (x.Active??false) == true).OrderBy(x=>x.DisplayOrder))
                    {
                        string mniParam = string.Empty;
						if (mni.ObjectId != null)
						{
							mniParam = "?NewsId=" + mni.ObjectId;
						}
                        strMenu.Append(string.Format("<li><a href='{0}'>{1}</a></li>", ResolveUrl("~/") + mni.Url + mniParam, mni.MenuName));
                    }
                    strMenu.Append("</ul></li>");
                }
                litMenu.Text = strMenu.ToString();
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this.Page);
            }
        }

        #endregion
    }
}