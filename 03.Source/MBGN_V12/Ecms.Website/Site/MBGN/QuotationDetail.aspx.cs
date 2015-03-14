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
using System.Globalization;

namespace Ecms.Website.Site.MBGN
{
	public partial class QuotationDetail : System.Web.UI.Page
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();
		protected double dSumAmount = 0;

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
            // Xác nhận đặt hàng
			try
			{
				// Check:
				var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];

				if (order.OrderStatus == OrderInStatus.QuotePending)
				{
					lblError.Text = "Báo giá chưa trả lời, quý khách không thể đặt hàng";
					lblError.Visible = true;
					return;
				}

				order.Remark = txtRemark.Text;
				Session[Constansts.SESSION_ORDERMODEL] = order;
				Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");
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

        #endregion

        #region //Private methods

		private void LoadData()
		{
			if (Session[Constansts.SESSION_ORDERMODEL] != null)
			{
				var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];

				dSumAmount = order.SumAmount??0;
				txtOrderNo.Text = order.OrderNo;
				txtRemark.Text = order.Remark;

				gridCart.DataSource = order.lstOrderDetailModel;
				gridCart.DataBind();
                //lblTotalMoney.Text = order.SumAmount.Value.ToString("N2");
			}
		}
		

        #endregion   
    }
}