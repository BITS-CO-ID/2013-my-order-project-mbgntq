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
    public partial class OrderByCheckedDetail : System.Web.UI.Page
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
                    Response.Redirect("~/site/mbgn/login.aspx");
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

        protected void gridCart_DataBound(object sender, EventArgs e)
        {
            MergeRows(gridCart);
        }
        
        protected void gridCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "updateTrackingOrder":
					var param = e.CommandArgument.ToString().Split('|');
					Response.Redirect(string.Format("~/site/mbgn/orderdeliveryedittrackingorder.aspx?orderId={0}&trackingNo={1}&orderDetailId={2}", param[0], param[1], param[2]));
                    break;
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
                lblOrderStatus.Text = order.OrderStatusText;
				lblRemark.Text = order.Remark;

                lblTotalMoneyOrder.Text = (order.SumAmount ?? 0).ToString("N0");
                lblTotalAmountNormal.Text = order.TotalPayAmountNormal.ToString("N0");
                //lblDateToUsa.Text = order.DateToUsa != null ? order.DateToUsa.Value.ToString("dd/MM/yyyy") : "";
				lblDeliverlyDate.Text = order.DeliveryDate != null ? order.DeliveryDate.Value.ToString("dd/MM/yyyy") : "";
				lblConfirmDate.Text = order.ConfirmDate != null ? order.ConfirmDate.Value.ToString("dd/MM/yyyy") : "";
				lblTotalRemain.Text = (order.RemainAmount ?? 0).ToString("N0");
                
                gridCart.DataSource = order.lstOrderDetailModel.ToList().OrderBy(x => x.TrackingNo).ToList();
                gridCart.DataBind();
            }
        }

        public void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];
                var lit1 = (Literal)row.Cells[1].FindControl("litTrackingNumber");
                var lit2 = (Literal)previousRow.Cells[1].FindControl("litTrackingNumber");

                if (lit1.Text == lit2.Text)
                {
                    //Gộp tracking
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 : previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
					////Gộp Order Number
					//row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 : previousRow.Cells[2].RowSpan + 1;
					//previousRow.Cells[2].Visible = false;
					////Gộp ngày đến Mỹ
					//row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 : previousRow.Cells[11].RowSpan + 1;
					//previousRow.Cells[11].Visible = false;
                    //Gộp trạng thái
					row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 : previousRow.Cells[11].RowSpan + 1;
					previousRow.Cells[11].Visible = false;
                    //Gộp trạng thái
					row.Cells[12].RowSpan = previousRow.Cells[12].RowSpan < 2 ? 2 : previousRow.Cells[12].RowSpan + 1;
					previousRow.Cells[12].Visible = false;
                }
            }
        }

        #endregion
    }
}