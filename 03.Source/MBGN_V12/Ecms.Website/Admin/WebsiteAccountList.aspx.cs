using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Website.DBServices;
using Ecms.Biz.Class;
using Ecms.Website.Common;

namespace Ecms.Website.Admin
{
    public partial class WebsiteAccountList : PageBase
    {

        #region // Declaration

        private readonly WebsiteService _websiteAccountService = new WebsiteService();

        #endregion


        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");
                try
                {
                    

                    loadWebsiteParentComboBox();
                    loadGrid();
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebsiteAccountCreate.aspx");
        }

       

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdD.PageIndex = e.NewPageIndex;
                loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void lbtDelete_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        var result = _websiteAccountService.WebsiteAccountDelete(id, this);
                        if (result)
                            Response.Write("<script type='text/javascript'>alert('Đã xóa dữ liệu thành công.');</script>");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }

        }

        protected void grdD_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        Session["lbtEdit_Click"] = id;
                        Response.Redirect("WebsiteAccountCreate.aspx");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }
        }
        #endregion

        #region // Method

        private List<WebsiteAccountModel> loadGrid()
        {
            string websiteParrentId = "";
            string websiteId = "";

            // "-1" là chọn load tất cả
            websiteParrentId = ddParrentWebsite.SelectedValue == "-1" ? "" : ddParrentWebsite.SelectedValue;
            //websiteId = ddWebsiteId.SelectedValue == "-1" ? "" : ddWebsiteId.SelectedValue;

			var list = _websiteAccountService.WebsiteAccountGet(
						""
						, ""
						, websiteParrentId
						, txtWebsiteAccountNo.Text
						, ""
						, this);

            grdD.DataSource = list;
            grdD.DataBind();
            if (list.Count > 0)
            {
                grdD.Visible = true;
                // reset error label
                lblError.Text = "";
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }

            return list;
        } // end LoadGrid

        private void loadWebsiteParentComboBox()
        {
            // Lấy Nhóm sản phẩm đổ vào ddCategoryIdParent
            List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
            temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Tất cả --" });
            temp.AddRange(this._websiteAccountService.WebsiteLinkGet("", "", "", "0", this));
            ddParrentWebsite.DataSource = temp;
            ddParrentWebsite.DataTextField = "WebsiteName";
            ddParrentWebsite.DataValueField = "WebsiteId";
            ddParrentWebsite.DataBind();
        }

        #endregion


    }
}