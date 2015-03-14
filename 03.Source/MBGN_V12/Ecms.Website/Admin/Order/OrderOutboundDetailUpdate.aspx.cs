using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.DBServices;
using System.Globalization;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Order
{
	public partial class OrderOutboundDetailUpdate : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null || string.IsNullOrEmpty(Request.QueryString["cur_status"]))
				{
					Response.Redirect("~/admin/security/login.aspx");
				}

                LoadData();
            }
        }

		protected void btnAccept_Click(object sender, EventArgs e)
		{
			string returnDate = "";
			if (ValidData(ref returnDate) == false)
			{
				return;
			}
			//NLogLogger.Info(string.Format("outBound status:{0}, ", orderOutbound.Status));

			var orderOutboundReturn = _orderService.OrderDetailChangeStatus(
				Request.QueryString["orderDetailId"]
				, ddlStatus.SelectedValue
				, returnDate
				, this);

			if (orderOutboundReturn)
			{
				lblResult.Text = "Đã cập nhật tình trạng món hàng thành công!";
				mtvMain.ActiveViewIndex = 1;
			}
		}

        protected void btnOK_Click(object sender, EventArgs e)
        {
			Response.Redirect("~/admin/order/orderoutbounddetail.aspx?isReUp=true");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			Response.Redirect("~/admin/order/orderoutbounddetail.aspx?");
        }

		protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.setTextDateEnable(ddlStatus.SelectedValue == "" ? 0 : Convert.ToInt32(ddlStatus.SelectedValue), "");
		}
        #endregion

        #region //Private methods

        private void LoadData()
        {               
			ddlStatus.SelectedValue = Request.QueryString["cur_status"].ToString();
        }

		private bool ValidData(ref string returnDate)
        {
			if (ddlStatus.SelectedValue.Equals(Request.QueryString["cur_status"]))
			{
				lblError.Text = "Bạn chưa chọn trạng thái khác để cập nhật!";
				lblError.Visible = true;
				return false;
			}

			if (!string.IsNullOrEmpty(Request.QueryString["cur_status"]) && Convert.ToInt32(Request.QueryString["cur_status"])==OrderOutboundStatus.Cancel)
			{
				lblError.Text = "Món hàng đã hủy, bạn không thể cập nhật tình trạng khác!";
				lblError.Visible = true;
				return false;
			}

            if (ddlStatus.SelectedValue.Equals(""))
            {
                lblError.Text = "Bạn chưa chọn trạng thái!";
                lblError.Visible = true;
                return false;
            }

			if (new string[]{"4","5"}.Contains(ddlStatus.SelectedValue) && !string.IsNullOrEmpty(txtFromDate.Text))
			{
				try
				{
					CultureInfo viVN = new CultureInfo("vi-VN");
					returnDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN).ToString("yyyy-MM-dd");
				}
				catch
				{
					lblError.Text = "Ngày không đúng định dạng!";
					lblError.Visible = true;
					return false;
				}
			}

			if (new string[] { "4", "5"}.Contains(ddlStatus.SelectedValue) && string.IsNullOrEmpty(txtFromDate.Text))
			{
				lblError.Text = "Bạn chưa nhập ngày!";
				lblError.Visible = true;
				return false;
			}

			return true;
        }

		private void setTextDateEnable(int status, string date)
		{
			if (status == 4 || status == 5 || status == 6)
			{
				txtFromDate.Enabled = true;
			}
			else
			{
				txtFromDate.Enabled = false;
			}
			if (!string.IsNullOrEmpty(date))
			{
				txtFromDate.Text = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
			}
		}
        #endregion
    }
}