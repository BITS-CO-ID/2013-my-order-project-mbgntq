using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Biz;
using CommonUtils;

namespace Ecms.Website.Admin
{
    public partial class GroupLinkCartUpdate : PageBase
    {
        #region // Declaration

		private readonly OrderService _orderService = new OrderService();
		private readonly WebsiteService _websiteService = new WebsiteService();

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (Session["User"] == null)
                        Response.Redirect("~/admin/security/login.aspx");

					loadComboBox();					
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

		protected void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (Session["User"] == null)
				{
					Response.Redirect("");
				}

				// lưu đơn hàng
				var orderOutbound = new OrderOutbound();

				orderOutbound.WebsiteAccountId = Convert.ToInt32(ddlWebsiteAccount.SelectedValue);
				orderOutbound.UserCreate = Session["User"].ToString();
				orderOutbound.Remark = txtRemark.Text;
				orderOutbound.TrackingNo = txtTrackingNo.Text;
				orderOutbound.Status = OrderOutboundStatus.IsBuy;
				orderOutbound.OrderDate = DatetimeUtils.GetDateTextbox(txtOrderDate.Text);

				var orderNew = _orderService.OrderOutboundCreate(orderOutbound, this);
				if (orderNew != null)
				{
					mtvMain.ActiveViewIndex = 1;
				}
			}
			catch (Exception exc)
			{
				Utils.ShowExceptionBox(exc, this);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("GroupLink.aspx");
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			Response.Redirect("GroupLink.aspx");
		}       

		protected void ddlGroupWebsite_SelectedIndexChanged(object sender, EventArgs e)
		{
			string groupWebsite = ddlGroupWebsite.SelectedValue.Equals("0") ? "" : ddlGroupWebsite.SelectedValue;
			if (string.IsNullOrEmpty(groupWebsite))
			{
				return;
			}
			ddlWebsite.DataSource = _websiteService.WebsiteLinkGet(""
										, ""
										, ""
										, groupWebsite
										, this);
			ddlWebsite.DataTextField = "WebsiteName";
			ddlWebsite.DataValueField = "WebsiteId";
			ddlWebsite.DataBind();
			ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
		}

		protected void ddlWebsite_SelectedIndexChanged(object sender, EventArgs e)
		{
			string websiteId = ddlWebsite.SelectedValue.Equals("0") ? "" : ddlWebsite.SelectedValue;
			if (string.IsNullOrEmpty(websiteId))
			{
				return;
			}
			ddlWebsiteAccount.DataSource = _websiteService.WebsiteAccountGet(
										""
										, websiteId
										, ""
										, ""
										, ""
										, this);

			ddlWebsiteAccount.DataTextField = "AccountWebsiteNo";
			ddlWebsiteAccount.DataValueField = "WebsiteAccountId";
			ddlWebsiteAccount.DataBind();
			ddlWebsiteAccount.Items.Insert(0, new ListItem("-- Chọn website account --", "0"));
		}

        #endregion

        #region // Method

		private void loadComboBox()
		{
			ddlGroupWebsite.DataSource = _websiteService.WebsiteLinkGet("", "", "", "0", this);
			ddlGroupWebsite.DataTextField = "WebsiteName";
			ddlGroupWebsite.DataValueField = "WebsiteId";
			ddlGroupWebsite.DataBind();
			ddlGroupWebsite.Items.Insert(0, new ListItem("-- Chọn nhóm website --", ""));

			ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
			ddlWebsiteAccount.Items.Insert(0, new ListItem("-- Chọn website account --", "0"));
		}

        #endregion


    }
}