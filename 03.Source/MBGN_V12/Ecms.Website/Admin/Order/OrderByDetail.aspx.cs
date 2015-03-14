using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Biz;
using Ecms.Website.DBServices;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderByDetail : PageBase
    {
        #region //Declares
        
        private readonly OrderService _orderService = new OrderService();
		protected double dTotalMoney = 0;

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
                LoadData();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			try
			{
				if (Request.QueryString["orderId"] != null)
				{
					var orderId = Request.QueryString["orderId"].ToString();
					var result = _orderService.OrderChangeStatusWithRemark(
									orderId
									, Convert.ToString(OrderInStatus.OrderCancel)
									, ""
									, this);

					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblMessage.Text = "Bạn đã HỦY đơn hàng thành công";
						//btnConfirm.Visible = false;
						//btnConfirmCancel.Visible = false;
						//txtRemark.Visible = false;
						btnOK.Visible = true;
					}

				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

		protected void btnReturn_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/admin/order/ordermanage.aspx");
		}

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

		protected void btnConfirm_Click(object sender, EventArgs e)
		{
			try
			{
				if (Request.QueryString["orderId"] != null)
				{

					var orderId = Request.QueryString["orderId"].ToString();

					var result = _orderService.OrderChangeStatusWithRemark(
									orderId
									, Convert.ToString(OrderInStatus.OrderConfirmed)
									, "" //this.txtRemark.Text
									, this);
					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblMessage.Text = "Bạn đã xác nhận đơn hàng thành công";

						//btnConfirm.Visible = false;
						btnOK.Visible = true;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

		protected void btnConfirmDelivery_Click(object sender, EventArgs e)
		{
			try
			{
				if (Request.QueryString["orderId"] != null)
				{
					var orderId = Request.QueryString["orderId"].ToString();
					var result = _orderService.OrderChangeStatus(
									orderId
									, Convert.ToString(OrderInStatus.Deliveried)
									, this);
					if (result)
					{
						lblMessage.Text = "Đơn hàng đã được xác nhận giao hàng.";
						mtvMain.ActiveViewIndex = 1;
						//btnConfirm.Visible = false;
						btnOK.Visible = true;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Request.QueryString["orderId"] != null)
            {
				var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                
                if (order != null)
                {
                    lblOrderNo.Text = order.OrderNo;
                    lblCreatedDate.Text = order.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    lblCustomerCode.Text = order.CustomerCode;
                    lblCustomerName.Text = order.CustomerName;
                    lblPhone.Text = order.Mobile;
                    lblAddress.Text = order.Address;

					dTotalMoney = (order.SumAmount ?? 0);
                    gridMain.DataSource = order.lstOrderDetailModel;
                    gridMain.DataBind();

					if (order.OrderStatus == OrderInStatus.OrderPending)
					{
						btnConfirmDelivery.Enabled = false;
						btnConfirmDelivery.CssClass = Constansts.CssClass_buttonDisable;
					}

					if (order.OrderStatus == OrderInStatus.OrderConfirmed)
					{
						btnConfirm.Enabled = btnCancel.Enabled = false;
						btnConfirm.CssClass = btnCancel.CssClass = Constansts.CssClass_buttonDisable;
					}

					if (order.OrderStatus == OrderInStatus.OrderCancel)
					{
						btnConfirm.Enabled = btnConfirmDelivery.Enabled = btnCancel.Enabled = false;
						btnConfirm.CssClass = btnConfirmDelivery.CssClass = btnCancel.CssClass = Constansts.CssClass_buttonDisable;
					}

					//if (order.OrderStatus == OrderInStatus.Finished)
					//{
					//    btnSave.Enabled = btnUpdate.Enabled = btnCancel.Enabled = btnRevert.Enabled = false;
					//    btnSave.CssClass = btnUpdate.CssClass = btnCancel.CssClass = btnRevert.CssClass = Constansts.CssClass_buttonDisable;
					//}

					if (order.OrderStatus == OrderInStatus.Deliveried)
					{
						btnConfirm.Enabled = btnConfirmDelivery.Enabled = btnCancel.Enabled = false;
						btnConfirm.CssClass = btnConfirmDelivery.CssClass = btnCancel.CssClass = Constansts.CssClass_buttonDisable;
					}
                }
            }
            else
            {
                Response.Redirect("~/admin/order/ordermanage.aspx");
            }

			//if (Session["PolicyUserMH"] != null)
			//{
			//    this.gridMain.Columns[13].Visible = false;
			//    this.gridMain.Columns[14].Visible = false;
			//    this.gridMain.Columns[15].Visible = false;
			//    this.gridMain.Columns[16].Visible = false;
			//}
			//else
			//{
			//    this.gridMain.Columns[13].Visible = true;
			//    this.gridMain.Columns[14].Visible = true;
			//    this.gridMain.Columns[15].Visible = true;
			//    this.gridMain.Columns[16].Visible = true;
			//}
        }

        #endregion   
    }
}