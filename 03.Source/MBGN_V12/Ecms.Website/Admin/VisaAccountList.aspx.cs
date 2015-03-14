using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Biz;
using Ecms.Website.DBServices;

namespace Ecms.Website.Admin.Security
{
	public partial class VisaAccountList : PageBase
    {
        #region // Declaration

		private readonly CommonService _commonService = new CommonService();


        #endregion

        #region // Method

        private void LoadGrid()
        {
			var list = _commonService.VisaAccountGet("", txtVisaNo.Text.Trim(), this);
            grdD.DataSource = list;
            grdD.DataBind();
            lblError.Text = "";
            if (list.Count > 0)
            {
                grdD.Visible = true;
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }
        }

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
                    LoadGrid();
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
                LoadGrid();
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
						Response.Redirect(string.Format("~/admin/visaaccountcreate.aspx?id={0}",id));
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
			lblError.Text = "";
			lblError.Visible = false;
			
            switch (e.CommandName)
            {
                case "Delete":
                    try
                    {
						// validate
						string id = e.CommandArgument.ToString();
						var oolist = new OrderService().OrderOutboundGet(
										"",
										"",
										"",
										"",
										"",
										"",
										"",
										"",
										"",
										"",
										"",
										"",
										id,
										"",
										this);

						if (oolist.Count > 0)
						{
							lblError.Text = "Số visa đã được sử dụng ở đơn hàng nước ngoài, không thể xóa";
							lblError.Visible = true;
							return;
						}

                        var result = _commonService.DeleteVisaAccount(id, this);
						if (result)
						{
							mtvMain.ActiveViewIndex = 1;
							LoadGrid();
						}                        
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
            Response.Redirect("VisaAccountCreate.aspx");
        }

		protected void btnOK_Click(object sender, EventArgs e)
		{
			mtvMain.ActiveViewIndex = 0;
			LoadGrid();
		}

        #endregion
    }
}