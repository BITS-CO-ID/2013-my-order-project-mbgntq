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

namespace Ecms.Website.Admin
{
    public partial class CustomerLists : PageBase
    {
        #region // Declaration

        private readonly CustomerService _customerService = new CustomerService();

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
                        Response.Redirect("CustomerEdit.aspx");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }
        }



        protected void lbtLock_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Lock":
                    try
                    {
                        string id = e.CommandArgument.ToString();

                        var result = _customerService.CustomerLockUnlock(id, "1", this);
                        if (result)
                            Response.Write("<script type='text/javascript'>alert('Hệ thống cập nhật thành công.');</script>");
                        loadGrid();

                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
                case "UnLock":
                    try
                    {
                        string id = e.CommandArgument.ToString();

                        var result = _customerService.CustomerLockUnlock(id, "0", this);
                        if (result)
                            Response.Write("<script type='text/javascript'>alert('Hệ thống cập nhật thành công.');</script>");
                        loadGrid();

                    }
                    catch (Exception exc)
                    {
                        //Response.Write("<script type='text/javascript'>alert('" + alParamsOutError + "');</script>");
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }
        }

		protected void lbtEmployee_Click(object sender, CommandEventArgs e)
		{

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

        #endregion

        #region // Method

        private List<UserCustomerModel> loadGrid()
        {
            string status = "";

            // "-1" là chọn load tất cả
            status = ddStatus.SelectedValue == "-1" ? "" : ddStatus.SelectedValue;
            var list = _customerService.CustomerList(
                 ""
                , txtCustomerName.Text
                , txtCustomerCode.Text
                , txtEmail.Text
                , txtMobile.Text
                , ""
                , ""
                , ""
                , ""
                , status
                , ""
                , this);

            list = list.Where(x => x.CustomerId != null).OrderByDescending(x=>x.CreatedDate).ToList();
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


        #endregion

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
    }
}