using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;

namespace Ecms.Website.Admin.Security
{
    public partial class MapGroupUsers : PageBase
    {
        #region // Delares
        private IUserBiz cService = new UserBiz();
        #endregion

        #region // Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["grpCode"] == null)
                    {
                        Response.Redirect("GroupList.aspx");
                    }
                    this.lblGroupCode.Text = "Nhóm: " + cService.GetGroup(Request.QueryString["grpCode"].ToString(), "").FirstOrDefault().GroupName;
                    LoadData();
                    string groupCode = Request.QueryString["grpCode"];
                    var list = cService.GetSysMapGroupUsers(
                                groupCode,
                                "",
                                "1"
                                );
                    if (list.Count > 0)
                    {
                        Session["CHECKED_ITEMS"] = list.Select(x => x.UserCode).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                NLogLogger.Info(string.Format("Error in btnExport_Click: Time: {0}\r\n{1}\r\n{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace));
                MessageLtr.Text = "<div class=\"notification error png_bg\"><a href=\"#\" class=\"close\"><img src=\"/App_Layouts/UIT/images/icons/cross_grey_small.png\" title=\"Đóng thông báo\"alt=\"close\" /></a><div>Có lỗi xảy ra trong quá trình lấy thông tin gán nhóm!</div></div>";
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoSave();
            }
            catch (Exception ex)
            {
                NLogLogger.Info(string.Format("Error in btnExport_Click: Time: {0}\r\n{1}\r\n{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace));
                MessageLtr.Text = "<div class=\"notification error png_bg\"><a href=\"#\" class=\"close\"><img src=\"/App_Layouts/UIT/images/icons/cross_grey_small.png\" title=\"Đóng thông báo\"alt=\"close\" /></a><div>Có lỗi xảy ra trong quá trình lưu gán nhóm!</div></div>";
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupList.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupList.aspx");
        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValues();
            grdD.PageIndex = e.NewPageIndex;
            LoadData();
            PopulateCheckedValues();
        }

        #endregion

        #region //Private methods

        private void DoSave()
        {
            string groupCode = Request.QueryString["grpCode"];
            var lstMaps = new List<Sys_MapUserGroup>();

            #region //Tạm
            /*
            if (Session["CHECKED_ITEMS"] != null)
            {
                List<string> userCodes = (List<string>)Session["CHECKED_ITEMS"];
                foreach (GridViewRow gvRow in grdD.Rows)
                {
                    CheckBox chkBox = (CheckBox)gvRow.FindControl("checkBox");
                    if (chkBox.Checked == true)
                    {
                        string index = (string)grdD.DataKeys[gvRow.RowIndex].Value;
                        if (!userCodes.Contains(index))
                        {
                            userCodes.Add(index);
                        }
                    }
                }
                if (userCodes.Count != 0)
                {
                    foreach (var userCode in userCodes)
                    {
                        var mapUser = new Sys_MapUserGroup
                        {
                            GroupCode = groupCode,
                            UserCode = userCode
                        };
                        lstMaps.Add(mapUser);
                    }
                }
            }
            else
            {
                foreach (GridViewRow gvRow in grdD.Rows)
                {
                    CheckBox chkBox = (CheckBox)gvRow.FindControl("checkBox");
                    if (chkBox.Checked == true)
                    {
                        var mapUser = new Sys_MapUserGroup
                        {
                            GroupCode = groupCode,
                            UserCode = gvRow.Cells[1].Text
                        };
                        lstMaps.Add(mapUser);
                    }
                }
            }
            */
            #endregion

            foreach (GridViewRow gvRow in grdD.Rows)
            {
                CheckBox chkBox = (CheckBox)gvRow.FindControl("checkBox");
                if (chkBox.Checked == true)
                {
                    string index = (string)grdD.DataKeys[gvRow.RowIndex].Value;
                    var mapUser = new Sys_MapUserGroup
                    {
                        GroupCode = groupCode,
                        UserCode = index
                    };
                    lstMaps.Add(mapUser);
                }
            }

            cService.UpdateMapGroupUsers(groupCode, lstMaps);
            if (Session["CHECKED_ITEMS"] != null)
            {
                Session.Remove("CHECKED_ITEMS");
            }
            mtvMain.ActiveViewIndex = 1;
        }

        private void LoadData()
        {
			var list = cService.GetUser("", "", "", "1", "").Where(x => x.UserCode != "sysadmin" && x.FlagActive == "1" && x.FlagAdmin == "1").ToList();

            if (list.Count > 0)
            {
                lblError.Visible = false;
                grdD.Visible = true;
                grdD.DataSource = list;
                grdD.DataBind();
            }
            else
            {
                lblError.Visible = true;
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }
        }

        public bool checkIsMapGroupSecurity(string userCode)
        {
            string groupCode = Request.QueryString["grpCode"];
            var list = cService.GetSysMapGroupUsers(
                        groupCode,
                        userCode,
                        "1"
                        );
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void SaveCheckedValues()
        {
            List<string> Ids = new List<string>();
            string index = "";
            foreach (GridViewRow gvrow in grdD.Rows)
            {
                index = (string)grdD.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("checkBox")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    Ids = (List<string>)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!Ids.Contains(index))
                        Ids.Add(index);
                }
                else
                {
                    Ids.Remove(index);
                }

            }

            Session["CHECKED_ITEMS"] = Ids;
        }

        private void PopulateCheckedValues()
        {
            List<string> Ids = (List<string>)Session["CHECKED_ITEMS"];
            foreach (GridViewRow gvrow in grdD.Rows)
            {
                string index = (string)grdD.DataKeys[gvrow.RowIndex].Value;

                CheckBox myCheckBox = (CheckBox)gvrow.FindControl("checkBox");
                if (Ids.Contains(index))
                {
                    myCheckBox.Checked = true;
                }
                else
                {
                    myCheckBox.Checked = false;
                }
            }
        }

        #endregion
    }
}