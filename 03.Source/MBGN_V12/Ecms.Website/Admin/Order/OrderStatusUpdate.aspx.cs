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
	public partial class OrderStatusUpdate : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				var currentStatus = Request.QueryString["cur_status"];
				if(!string.IsNullOrEmpty(currentStatus))
				{
					ddlStatus.SelectedValue = currentStatus;
				}
            }
        }

		protected void btnAccept_Click(object sender, EventArgs e)
		{
			string returnDate = "";
			if (ValidData(ref returnDate) == false)
			{
				return;
			}

			var currentStatus = Request.QueryString["cur_status"];
			var newStatus = ddlStatus.SelectedValue;
			if (!currentStatus.Equals(newStatus))
			{
				var orderDetail = _orderService.OrderDetailUpdate(
					Request.QueryString["orderDetailId"]
					, newStatus // Convert.ToString(OrderInStatus.Deliveried)
					, returnDate
					, this);

				if (orderDetail)
				{
					lblResult.Text = "Đã cập nhật tình trạng món hàng thành công!";
					mtvMain.ActiveViewIndex = 1;
				}
			}
		}

        protected void btnOK_Click(object sender, EventArgs e)
        {
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				Response.Redirect("~/admin/order/orderbylinkdetail.aspx");
			}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				Response.Redirect("~/admin/order/orderbylinkdetail.aspx");
			}
        }

        #endregion

        #region //Private methods


		private bool ValidData(ref string returnDate)
        {
			// Check curent status
			if (string.IsNullOrEmpty(Request.QueryString["cur_status"]))
			{
				var currentStatus = Request.QueryString["cur_status"];
				if (Convert.ToInt32(currentStatus) != OrderOutboundStatus.IsBuy
					|| Convert.ToInt32(currentStatus) != OrderOutboundStatus.InvInbound)
				{
					lblError.Text = "Món hàng này hiện tại chưa được cập nhật ở tình trạng Đã mua";
					lblError.Visible = true;
					return false;
				}
			}

			if (string.IsNullOrEmpty(txtFromDate.Text))
			{
				lblError.Text = "Bạn chưa chọn ngày giao hàng!";
				lblError.Visible = true;
				return false;
			}
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

			return true;
        }
		
        #endregion
    }
}