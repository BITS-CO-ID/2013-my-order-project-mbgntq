using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz;
using Ecms.Website.DBServices;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class UserList : PageBase
    {
        #region //Declares

        private IUserBiz cService = new UserBiz();
        private ICommonBiz commonService = new CommonBiz();
        private IInvoiceBiz tran = new InvoiceBiz();
        private CustomerService _customerService = new CustomerService();

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
            Response.Redirect("UserCreate.aspx");
        }

        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    string userCode = e.CommandArgument.ToString();
                    Session["lbtEdit_Click"] = userCode;
                    Response.Redirect("UserCreate.aspx");
                    break;
            }
        }

        protected void btnLinkDetail_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Detail":
                    string id = e.CommandArgument.ToString();
                    Response.Redirect("UserProfile.aspx?ID=" + id);
                    break;
            }
        }

        protected void btnLinkReset_Click(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Reset":
                        string userCode = e.CommandArgument.ToString();

						if (_customerService.ResetPassword(userCode, "1", this))
						{
							mtvMain.ActiveViewIndex = 1;
							lblResult.Text = "Đã reset mật khẩu thành công.";
						}
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void lbtDelete_Click(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Delete":

                        //string id = e.CommandArgument.ToString();
                        //cService.DeleteUser(id);
                        //LoadData();
                        break;
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserList.aspx");
        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdD.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void grdD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string userName = DataBinder.Eval(e.Row.DataItem, "UserCode").ToString();

                // loop all data rows
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    // check all cells in one row
                    foreach (Control control in cell.Controls)
                    {
                        // Must use LinkButton here instead of ImageButton
                        // if you are having Links (not images) as the command button.
                        var button = control as LinkButton;
                        if (button != null && button.CommandName == "Reset")
                            // Add edit confirmation
                            button.OnClientClick = "return confirm('Bạn chắc chắn  đổi mật khẩu tài khoản " + userName + " ?');";
                    }
                }
            }
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            var list = cService.GetUser(txtUserCode.Text.Trim(), txtUserName.Text.Trim(), "", cboStatus.SelectedValue, "");
            if (list.Count != 0)
            {
                lblError.Visible = false;
                grdD.Visible = true;
				grdD.DataSource = list.Where(x => x.UserCode != "sysadmin" && x.FlagActive == "1" && x.FlagAdmin == "1").ToList();
                grdD.DataBind();
            }
            else
            {
                lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm";
                lblError.Visible = true;
                grdD.Visible = false;
            }
        }

        #endregion
    }
}