using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class MapGroupObject : PageBase
    {
        #region // Declares

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
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
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
                Utils.ShowExceptionBox(ex, this);
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

        protected void RadTreeView1_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                var list = cService.GetObject("", "", "", "1");
                RadTreeList.DataSource = list;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void RadTreeList_DataBound(object sender, EventArgs e)
        {
            foreach (var item in RadTreeList.Items)
            {
                item.Selected = checkIsMapGroup(item["ObjectCode"].Text);
            }
        }

        #endregion

        #region //Private Methods

        private void LoadData()
        {
            var list = cService.GetObject(
                        "",
                        "",
                        "",
                        "1"
                        );
            if (list.Count > 0)
            {
                lblError.Visible = false;
                RadTreeList.Visible = btnConfirm.Visible = true;
                RadTreeList.DataSource = list;
                RadTreeList.DataBind();

				RadTreeList.ExpandAllItems();
            }
            else
            {
                lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm.";
                lblError.Visible = true;
                RadTreeList.Visible = btnConfirm.Visible = false;
            }
        }

        private void DoSave()
        {

            //if (grdD.Rows.Count > 0)
            //{
            //    string groupCode = Request.QueryString["grpCode"];
            //    var lstMaps = new List<Sys_MapGroupObject>();
            //    foreach (GridViewRow gvRow in grdD.Rows)
            //    {//find your checkbox
            //        CheckBox chkBox = (CheckBox)gvRow.FindControl("checkBox");
            //        if (chkBox.Checked == true)
            //        {
            //            var mapUser = new Sys_MapGroupObject
            //            {
            //                GroupCode = groupCode,
            //                ObjectCode = gvRow.Cells[1].Text
            //            };
            //            lstMaps.Add(mapUser);
            //        }
            //    }

            //    cService.UpdateMapGroupObject(groupCode, lstMaps);
            //    mtvMain.ActiveViewIndex = 1;
            //}

            string groupCode = Request.QueryString["grpCode"];
            var lstMaps = new List<Sys_MapGroupObject>();
            foreach (var row in RadTreeList.SelectedItems)
            {
                var objectCode = row["ObjectCode"].Text;

                var mapUser = new Sys_MapGroupObject
                {
                    GroupCode = groupCode,
                    ObjectCode = objectCode
                };
                lstMaps.Add(mapUser);
            }
            cService.UpdateMapGroupObject(groupCode, lstMaps);
            mtvMain.ActiveViewIndex = 1;
        }

        public bool checkIsMapGroup(string objectCode)
        {
            string groupCode = Request.QueryString["grpCode"];
            var list = cService.GetSysMapGroupObject(
                        groupCode,
                        objectCode,
                        "",
                        "1"
                        );
            if (list.Count > 0)
                return true;
            return false;
        }

        #endregion
    }
}