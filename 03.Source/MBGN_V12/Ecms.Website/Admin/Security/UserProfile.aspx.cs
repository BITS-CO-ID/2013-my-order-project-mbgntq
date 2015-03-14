using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;

namespace Ecms.Website.Admin.Security
{
    public partial class UserProfile : PageBase
    {
        #region // Declares

        private readonly UserBiz comService = new UserBiz();

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    LoadData();
                }
            }
        }

        protected void RadTreeView1_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            var list = (List<UserModel>)Session["ListUserProfile"];
            treeListMain.DataSource = list;
        }

        #endregion

        #region // Private methods

        private void LoadData()
        {
            var listUserMap = comService.GetUserForMap(Request.QueryString["ID"], "", "", "", "", "");
            Session["ListUserProfile"] = listUserMap;
            var userMapFirst = listUserMap.FirstOrDefault();
            if (userMapFirst != null)
            {
                lblUserCode.Text = userMapFirst.UserCode;
                lblUserName.Text = userMapFirst.UserName;
                lblEmail.Text = userMapFirst.Email;
            }

            rptGroup.DataSource = listUserMap.Select(p => new { GroupName = p.GroupName }).Distinct();
            rptGroup.DataBind();

            treeListMain.DataSource = listUserMap;
            treeListMain.DataBind();
        }

        #endregion
    }
}