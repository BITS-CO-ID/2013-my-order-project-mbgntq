using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;

namespace Ecms.Website.Site.MBGN
{
    public partial class Complaints : System.Web.UI.Page
    {

        #region // Declaration

        private readonly ComplaintsService _complaintsService = new ComplaintsService();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] == null)
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx");
                Session["lbtEdit_Click"] = null;
                loadGrid();
            }
        }

        private List<Complaint> loadGrid()
        {
            try
            {
                UserCustomerModel user = (UserCustomerModel)Session["Customer"];
                var list = this._complaintsService.ComplaintGet("", user.CustomerId.ToString(), "", "", this);
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

        protected void btnAddnew_Click(object sender, EventArgs e)
        {
            setView(false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            setView(true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UserCustomerModel user = (UserCustomerModel)Session["Customer"];
                Complaint comp = new Complaint();
                comp.CreatedUser = user.CustomerId;
                comp.CreatedDate = DateTime.Now;
                //comp.CloseDate
                comp.ComplaintsDate = comp.CreatedDate;
                comp.LastModifyDate = comp.CreatedDate;
                comp.Title = txtTitle.Text;
                comp.ContentComplaints = txtContent.Text;
                //comp.ReceiverComplaintsId
                //comp.Status
                //comp.CloseDate
                //comp.Remark

                var result = this._complaintsService.ComplaintCreate(comp, this);
                if (result.Id != 0)
                {
                    Response.Write("<script type='text/javascript'>alert('Đăng thành công');</script>");
                    loadGrid();
                    setView(true);

                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert('Không đăng được');</script>");
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        private void setView(bool bo)
        {
            if (bo)
            {
                grdD.Visible = true;
                txtContent.Text = "";
                txtTitle.Text = "";
                btnAddnew.Enabled = true;
                pnAddNew.Visible = false;
            }
            else
            {
                grdD.Visible = false;
                pnAddNew.Visible = true;
                btnAddnew.Enabled = false;
                txtTitle.Focus();
            }
        }


    }
}