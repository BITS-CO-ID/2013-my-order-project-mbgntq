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
    public partial class OrderOutboundUpdate : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
			string returnDate="";
			if (ValidData(ref returnDate) == false)
			{
				return;
			}
            if (Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] != null)
            {
                var orderOutbound = (OrderOutboundModel)Session[Constansts.SS_ORDER_OUTBOUND_ADMIN];
                var orderOutBoundNew = new OrderOutbound();
             
                orderOutBoundNew.TrackingNo = txtTrackingNumber.Text;
                //orderOutBoundNew.OrderNumber = txtOrderNumber.Text;
                orderOutBoundNew.Status = Convert.ToInt32(ddlStatus.SelectedValue);

				var orderOutboundReturn = _orderService.OrderOutboundUpdateTracking(
					Convert.ToString(orderOutbound.OrderOutboundId)
					, txtTrackingNumber.Text
					, ""//txtOrderNumber.Text
					, ddlStatus.SelectedValue
					, returnDate
					, this);
                if (orderOutboundReturn)
                {
                    lblResult.Text = "Đã cập nhật đơn hàng thành công!";
                    mtvMain.ActiveViewIndex = 1;
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/orderoutboundmanage.aspx?returnBack=return");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/orderoutboundmanage.aspx?returnBack=return");
        }

		protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.setTextDateEnable(ddlStatus.SelectedValue == "" ? 0 : Convert.ToInt32(ddlStatus.SelectedValue), "");
		}
        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] != null)
            {
                var orderOutbound = (OrderOutboundModel)Session[Constansts.SS_ORDER_OUTBOUND_ADMIN];
                txtTrackingNumber.Text = orderOutbound.TrackingNo;
                //txtOrderNumber.Text = orderOutbound.OrderNumber;

				//if (orderOutbound.Status == OrderOutboundStatus.InProcess)
				//{
				//    // Chỉ dc chuyển sang Đã mua
				//    //ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.CancelText, Convert.ToString(OrderOutboundStatus.Cancel)));
				//    ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.InvOutboundText, Convert.ToString(OrderOutboundStatus.InvOutbound)));
				//    ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.InvInboundText, Convert.ToString(OrderOutboundStatus.InvInbound)));

				//} else if (orderOutbound.Status == OrderOutboundStatus.IsBuy)
				//{
				//    ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.IsBuyText, Convert.ToString(OrderOutboundStatus.IsBuy)));
				//    ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.InvInboundText, Convert.ToString(OrderOutboundStatus.InvInbound)));
				//}
				//else if (orderOutbound.Status == OrderOutboundStatus.Cancel)
				//{
				//    // no need
				//}
				//else if (orderOutbound.Status == OrderOutboundStatus.InvOutbound)
				//{
				//    ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.IsBuyText, Convert.ToString(OrderOutboundStatus.IsBuy)));
				//    ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.CancelText, Convert.ToString(OrderOutboundStatus.Cancel)));
				//}
				//else if (orderOutbound.Status == OrderOutboundStatus.InvInbound)
				//{
				//    // no need
				//}

				if (orderOutbound.Status == OrderOutboundStatus.Cancel)
				{
					ddlStatus.Items.Clear();
					ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.CancelText, Convert.ToString(OrderOutboundStatus.Cancel)));
				}

				if (orderOutbound.Status != OrderOutboundStatus.InProcess && orderOutbound.Status != OrderOutboundStatus.IsBuy && orderOutbound.Status != OrderOutboundStatus.Cancel)
				{
					ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.InProcessText, Convert.ToString(OrderOutboundStatus.InProcess)));
					ddlStatus.Items.Remove(new ListItem(OrderOutboundStatus.CancelText, Convert.ToString(OrderOutboundStatus.Cancel)));
				}

                ddlStatus.SelectedValue = orderOutbound.Status + "";
				NLogLogger.Info(string.Format("outBound status:{0}, ", orderOutbound.Status));
				this.setTextDateEnable(orderOutbound.Status??0, "");
            }
        }

		private bool ValidData(ref string returnDate)
        {
			// Check tracking Input
			if (!string.IsNullOrEmpty(txtTrackingNumber.Text))
			{
				if (Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] != null)
				{
					var orderOutbound = (OrderOutboundModel)Session[Constansts.SS_ORDER_OUTBOUND_ADMIN];
					var orderOutboundList = _orderService.OrderOutboundGet(""
										, ""
										, ""//this.txtTrackingNumber.Text
										, ""
										, ""
										, ""
										, ""
										, ""
										, ""
										, ""
										, ""
										, ""
										, ""
										, ""
										, this).Where(p => p.OrderOutboundId != orderOutbound.OrderOutboundId && p.TrackingNo == txtTrackingNumber.Text.Trim()).ToList();

					if (orderOutboundList != null && orderOutboundList.Count > 0)
					{
						lblError.Text = "Số TrackingNo đã có trong hệ thống";
						lblError.Visible = true;
						return false;
					}
				}
			}

            if (ddlStatus.SelectedValue.Equals(""))
            {
                lblError.Text = "Bạn chưa chọn trạng thái!";
                lblError.Visible = true;
                return false;
            }

			if (new string[] { Convert.ToString(OrderOutboundStatus.InvOutbound), Convert.ToString(OrderOutboundStatus.InvInbound), Convert.ToString(OrderOutboundStatus.InvInboundMBGN)}
				.Contains(ddlStatus.SelectedValue) && !string.IsNullOrEmpty(txtFromDate.Text))
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

			if (new string[] { Convert.ToString(OrderOutboundStatus.InvOutbound), Convert.ToString(OrderOutboundStatus.InvInbound), Convert.ToString(OrderOutboundStatus.InvInboundMBGN) }
				.Contains(ddlStatus.SelectedValue) && string.IsNullOrEmpty(txtFromDate.Text))
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