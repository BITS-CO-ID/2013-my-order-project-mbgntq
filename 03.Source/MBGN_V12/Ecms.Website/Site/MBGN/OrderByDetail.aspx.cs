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
	public partial class OrderByDetail : System.Web.UI.Page
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
                if (!IsPostBack)
                {
                    if (Session["Customer"] == null)
                        Response.Redirect("~/site/mbgn/login.aspx");
                }
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
			Response.Redirect("~/site/mbgn/OrderProduct.aspx");
		}

        #endregion

        #region //Private methods

		private void LoadData()
		{
			if (Session[Constansts.SESSION_ORDERMODEL] != null)
			{
				var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];

				dTotalMoney = order.SumAmount.Value;

				lblOrderNo.Text = order.OrderNo;				
				lblOrderDate.Text = order.OrderDate==null?"":order.OrderDate.Value.ToString("dd/MM/yyyy");
				lblOrderStatus.Text = order.OrderStatusText;

				gridCart.DataSource = order.lstOrderDetailModel;
				gridCart.DataBind();
			}
		}

        #endregion   
    }
}