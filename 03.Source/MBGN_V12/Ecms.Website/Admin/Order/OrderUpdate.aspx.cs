using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Biz;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderUpdate : PageBase
    {
        #region Declares

        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/admin/security/login.aspx");                
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTrackingNumber.Text))
                {
                    lblError.Text = "Bạn chưa nhập BillNo!";
                    lblError.Visible = true;
                    return;
                }
                if (Session[Constansts.SS_ORDERMODEL_ADMIN] != null)
                {
                    var order = (OrderModel)Session[Constansts.SS_ORDERMODEL_ADMIN];
                    if (order != null)
                    {
                        var orderReturn = _orderService.OrderEditTrackingNo(order.OrderId + "", txtTrackingNumber.Text, this);
                        if (orderReturn != null)
                        {
                            lblResult.Text = "Cập nhật BillNo thành công!";
                            mtvMain.ActiveViewIndex = 1;
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/admin/order/ordermanage.aspx");
                }
            }catch(Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

        #endregion
    }
}