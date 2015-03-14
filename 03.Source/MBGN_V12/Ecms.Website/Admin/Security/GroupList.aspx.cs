using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class GroupList : PageBase
    {
        #region //Declares

        private IUserBiz cService = new UserBiz();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupCreate.aspx");
        }

        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "EditGroup":
                    string id = e.CommandArgument.ToString();
                    Session["lbtEdit_Click"] = id;
                    Response.Redirect("GroupCreate.aspx");
                    break;
            }
        }

        protected void lbtDelete_Click(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "DeleteGroup":
                        string id = e.CommandArgument.ToString();
                        mtvMain.ActiveViewIndex = 1;
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void lbtGroupUser_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "GroupUser":
                    Response.Redirect(string.Format("/Admin/Security/MapGroupUsers.aspx?grpCode={0}", Convert.ToString(e.CommandArgument)));
                    break;
            }
        }

        protected void lbtGroupObj_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "GroupObj":
                    Response.Redirect(string.Format("/Admin/Security/MapGroupObject.aspx?grpCode={0}",
                        Convert.ToString(e.CommandArgument)));
                    break;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupList.aspx");
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            var list = cService.GetGroup(txtGroupCode.Text.Trim(), txtGroupName.Text.Trim());
            if (list.Count != 0)
            {
                lblError.Visible = false;
                grdD.Visible = true;
                grdD.DataSource = list;
                grdD.DataBind();
            }
            else
            {
                lblError.Text = "Không tìm thấy dữ liệu theo điều kiến tìm kiếm!";
                lblError.Visible = true;
                grdD.Visible = false;
            }

        }

        #endregion
    }
}