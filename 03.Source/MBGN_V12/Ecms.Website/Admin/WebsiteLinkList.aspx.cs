using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Biz.Class;

namespace Ecms.Website.Admin
{
    public partial class WebsiteLinkList : PageBase
    {
        #region // Declaration

        private readonly WebsiteService _websiteService = new WebsiteService();

        #endregion

        #region // Even

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                loadWebsiteParentIdComboBox();
                loadGrid();
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
            Response.Redirect("WebsiteLinkCreate.aspx");
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

        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        Session["lbtEdit_Click"] = id;
                        Response.Redirect("WebsiteLinkCreate.aspx");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
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

                        var query = _websiteService.WebsiteLinkGet("", "", "", id, this);
                        if (query != null && query.Count() > 0)
                        {
                            throw new Exception("Website đã được sử dụng, bạn không thể xóa.");
                        }

                        var result = _websiteService.WebsiteLinkDelete(id, this);
                        if(result)
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

        #endregion

        #region // Method
        private List<WebsiteLinkModel> loadGrid()
        {
            string parentId = "";

            parentId = ddWebsiteParentId.SelectedValue == "-1" ? "" : ddWebsiteParentId.SelectedValue;

            var list = _websiteService.WebsiteLinkGet("", txtWebsiteName.Text, txtWebsiteLink.Text, parentId, this);
            grdD.DataSource = list;
            grdD.DataBind();
            if (list.Count > 0)
            {
                grdD.Visible = true;
                lblError.Text = "";
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }

            return list;
        } // end LoadGrid

        private void loadWebsiteParentIdComboBox()
        {
            List<WebsiteLinkModel> temp = new List<WebsiteLinkModel>();
            temp.Add(new WebsiteLinkModel() { WebsiteId = -1, WebsiteName = "-- Tất cả --" });

            temp.AddRange(this._websiteService.WebsiteLinkGet("", "", "", "0", this));

            ddWebsiteParentId.DataSource = temp;
            ddWebsiteParentId.DataTextField = "WebsiteName";
            ddWebsiteParentId.DataValueField = "WebsiteId";
            ddWebsiteParentId.DataBind();
        }

        protected virtual void DisplayAlert(string message, string page)
        {
            try
            {
                ClientScript.RegisterStartupScript(
                  this.GetType(),
                  Guid.NewGuid().ToString(),
                  string.Format("alert('{0}');window.location.href = '" + page + "'",
                    message.Replace("'", @"\'").Replace("\n", "\\n").Replace("\r", "\\r")),
                    true);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }
        #endregion


    }
}