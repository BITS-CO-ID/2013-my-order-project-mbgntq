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
using System.Globalization;

namespace Ecms.Website.Admin
{
    public partial class GroupLinkCart : PageBase
    {
        #region // Declaration

		private readonly OrderService _orderService = new OrderService();
		private readonly WebsiteService _websiteService = new WebsiteService();
		private readonly WebsiteService _websiteAccountService = new WebsiteService();
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

                    InitData();
					this.DoSearch();
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
				this.DoSearch();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }       

		protected void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				lblError.Visible = false;

                var accountWebsite = ddlWebsiteAccount.SelectedValue;
                var orderDate = new DateTime();
                if (ValidData(accountWebsite, ref orderDate) == false)
                    return;

				// lưu đơn hàng
				var orderOutbound = new OrderOutbound();

                orderOutbound.WebsiteAccountId = Convert.ToInt32(accountWebsite);
				orderOutbound.UserCreate = Session["User"].ToString();
				orderOutbound.Remark = txtRemark.Text;               
				
				orderOutbound.Status = OrderOutboundStatus.InProcess;
				orderOutbound.OrderDate = DatetimeUtils.GetDateTextbox(txtOrderDate.Text);
				if(!string.IsNullOrEmpty(ddlVisa.SelectedValue))
				{
					orderOutbound.VisaId = Convert.ToInt32(ddlVisa.SelectedValue);
				}
				// details
				orderOutbound.OrderOutboundDetails = new System.Data.Objects.DataClasses.EntityCollection<OrderOutboundDetail>();
			
				var details = (List<OrderDetailModel>)Session[Constansts.SESSION_ORDERCART];
				foreach (var item in details)
				{
					var ood = new OrderOutboundDetail();
					ood.OrderDetailId = item.OrderDetailId;
					orderOutbound.OrderOutboundDetails.Add(ood);
				}
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
            Response.Redirect("~/admin/order/grouplink.aspx");
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
            Response.Redirect("~/admin/order/grouplink.aspx");
		}

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdD.PageIndex = e.NewPageIndex;
				this.DoSearch();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        // đang xử lí
        protected void grdD_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //loadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        #endregion

        #region // Method

		private void DoSearch()
		{			

			// add detail
			var details = (List<OrderDetailModel>)Session[Constansts.SESSION_ORDERCART];

			grdD.DataSource = details;
			grdD.DataBind();
			if (details.Count > 0)
			{
				grdD.Visible = true;
			}
		}

        private void InitData()
        {
            if (Session["Website"] != null)
            {               
                ddlWebsiteAccount.DataSource = _websiteService.WebsiteAccountGet(
                                            ""
											, Session["Website"].ToString()
                                            , ""
                                            , ""
											, ""
                                            , this);

                ddlWebsiteAccount.DataTextField = "AccountWebsiteNo";
                ddlWebsiteAccount.DataValueField = "WebsiteAccountId";
                ddlWebsiteAccount.DataBind();
                ddlWebsiteAccount.Items.Insert(0, new ListItem("-- Chọn website account --", ""));

				var listVisaAgain = new CommonService().VisaAccountGet(
								""
								, ""
								, this);

				ddlVisa.DataTextField = "VisaNo";
				ddlVisa.DataValueField = "VisaId";
				ddlVisa.DataSource = listVisaAgain;
				ddlVisa.DataBind();
				ddlVisa.Items.Insert(0, new ListItem("-- Chọn Số TK TT --", ""));	
            }
        }

        private bool ValidData(string accountWebsite,ref DateTime orderDate)
        {           
            if (string.IsNullOrEmpty(accountWebsite))
            {
                lblError.Text = "Bạn chưa chọn tài khoản đặt hàng!";
                lblError.Visible = true;
                return false;
            }

			if (string.IsNullOrEmpty(ddlVisa.SelectedValue))
			{
				lblError.Text = "Bạn chưa chọn số TK thanh toán !";
				lblError.Visible = true;
				return false;
			}

            if (string.IsNullOrEmpty(txtOrderDate.Text))
            {
                lblError.Text = "Ngày đặt hàng không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                orderDate = DateTime.ParseExact(txtOrderDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Ngày đặt hàng không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }
            return true;
        }

        #endregion
    }
}