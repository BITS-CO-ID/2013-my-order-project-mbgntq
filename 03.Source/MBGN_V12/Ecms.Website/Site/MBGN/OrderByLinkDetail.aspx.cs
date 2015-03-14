using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Biz;
using CommonUtils;
using Ecms.Website.Common;

namespace Ecms.Website.Site.MBGN
{
    public partial class OrderByLinkDetail : System.Web.UI.Page
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] == null)
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx");
                LoadData();
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];
                Session[Constansts.SESSION_ORDERNO] = order.OrderNo;
                Response.Redirect("~/site/mbgn/Payment.aspx");
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/mbgn/orderproduct.aspx");
        }

		protected void gridCartByLink_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				gridCart.PageIndex = e.NewPageIndex;

				if (Session[Constansts.SESSION_ORDERMODEL] != null)
				{
					var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];

					gridCart.DataSource = order.lstOrderDetailModel.ToList();
					gridCart.DataBind();
				}
			}
			catch (Exception exc)
			{
				Utils.ShowExceptionBox(exc, this);
			}
		}

        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Session[Constansts.SESSION_ORDERMODEL] != null)
            {
                var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];

                lblOrderNo.Text = order.OrderNo;
                lblOrderDate.Text = order.OrderDate == null ? "" : order.OrderDate.Value.ToString("dd/MM/yyyy");
				lblDeliveryDate.Text = order.DeliveryDate == null ? "" : order.DeliveryDate.Value.ToString("dd/MM/yyyy");
				lblConfirmDate.Text = order.ConfirmDate == null ? "" : order.ConfirmDate.Value.ToString("dd/MM/yyyy");
                lblOrderStatus.Text = order.OrderStatusText;
				lblRemark.Text = order.Remark;

                lblTotalMoneyOrder.Text = (order.SumAmount ?? 0).ToString("N0");
				//lblAmountFeeDelay.Text = (order.AmountFeeDelay).ToString("N0");
				lblTotalAmountNormal.Text = order.TotalPayAmountNormal.ToString("N0");
				lblSumFeeShip.Text = (order.SumFeeShip ?? 0).ToString("N0");
				lblTotalRemainAmount.Text = (order.RemainAmount ?? 0).ToString("N0");
				//lblAmountCalFeeDelay.Text = (order.AmountCalFeeDelay).ToString("N0");                

                gridCart.DataSource = order.lstOrderDetailModel.ToList();
                gridCart.DataBind();
            }
        }

        #endregion
    }
}