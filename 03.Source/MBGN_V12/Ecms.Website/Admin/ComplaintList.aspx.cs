using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Website.Admin
{
    public partial class ComplaintList : PageBase
    {

        #region // Declaration

        private readonly ComplaintsService _complaintsService = new ComplaintsService();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");
                Session["lbtEdit_Click"] = null;
                loadGrid();
            }
        }

        private List<Complaint> loadGrid()
        {
            try
            {
                var list = this._complaintsService.ComplaintGet("", "", "", "", this);
                grdD.DataSource = list;
                grdD.DataBind();
                if (list.Count > 0)
                {
                    grdD.Visible = true;
                }
                else
                {
                    grdD.Visible = false;
                }

                return list;
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
                return new List<Complaint>();
            }
        } // end

        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        Session["lbtEdit_Click"] = id;
                        Response.Redirect("ComplaintDetail.aspx");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }
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

                        var result = this._complaintsService.ComplaintDelete(id, this);
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

    }
}