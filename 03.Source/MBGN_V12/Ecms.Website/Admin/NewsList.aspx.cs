using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Class;
using System.Data;

namespace Ecms.Website.Admin
{
    public partial class NewsList : PageBase
    {
        #region // Declaration

        private readonly NewsService _newsService = new NewsService();

        #endregion

        #region // Even

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    Session["NewsCode"] = null;
                    //loadComboBox();
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


        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        Session["lbtEdit_Click"] = id;
                        Response.Redirect("NewsCreate.aspx");
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
                        var result = this._newsService.NewsDelete(id, this);
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


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewsCreate.aspx");
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

        // đang sử lí
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

        private List<NewsModel> loadGrid()
        {
            string typeId = "";
            lblError.Text = "";
            // "-1" là chọn load tất cả
            typeId = ddType.SelectedValue == "-1" ? "" : ddType.SelectedValue;
            string fDate = "";
            string tDate = "";

            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                fDate = CommonUtils.DatetimeUtils.GetDateTextbox(txtDateFrom.Text).ToString("yyyy-MM-dd 00:00:00");
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                tDate = CommonUtils.DatetimeUtils.GetDateTextbox(txtDateTo.Text).ToString("yyyy-MM-dd 23:59:59");
            }
            var list = this._newsService.NewsGet(""
                                                , txtTitle.Text
                                                , ""
                                                , ""
                                                , typeId
                                                , ""
                                                , fDate
                                                , tDate
                                                , this
                                                );
            grdD.DataSource = list;
            grdD.DataBind();
            if (list.Count > 0)
            {
                grdD.Visible = true;
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }

            return list;
        }
        #endregion
    }
}