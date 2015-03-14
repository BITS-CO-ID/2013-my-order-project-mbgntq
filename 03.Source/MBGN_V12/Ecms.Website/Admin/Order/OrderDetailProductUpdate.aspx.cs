using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Entities;
using CommonUtils;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderDetailProductUpdate : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();
        private readonly CustomerService _customerService = new CustomerService();
        private readonly ProductService _productService = new ProductService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
			}
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["dictChangeStatus"] != null)
                {
                    var dictChangeStatus = (Dictionary<string, string>)Session["dictChangeStatus"];
                    string orderId = dictChangeStatus["orderId"];
                    var orderDetailId = Convert.ToInt32(dictChangeStatus["orderDetailId"]);

					var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();

					if (order != null && order.OrderStatus == CommonUtils.OrderInStatus.Deliveried)
					{
						lblError.Text = "Đã hàng đã được xác nhận GIAO HÀNG không thể cập nhật thông tin!";
						lblError.Visible = true;
						return;
					}

                    if (order != null)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.OrderDetailId = Convert.ToInt32(orderDetailId);

                        if (!string.IsNullOrEmpty(txtQuantity.Text))
                        {
                            orderDetail.Quantity = Convert.ToDouble(txtQuantity.Text.Replace(",", ""));
                        }

                        if (!string.IsNullOrEmpty(txtWeight.Text))
                        {
                            orderDetail.Weight = Convert.ToDouble(txtWeight.Text.Replace(",", ""));
                        }

                        if (!string.IsNullOrEmpty(txtSurcharge.Text))
                        {
                            orderDetail.Surcharge = Convert.ToDouble(txtSurcharge.Text.Replace(",", ""));
                        }

                        if (!string.IsNullOrEmpty(txtDeclarePrice.Text))
                        {
                            orderDetail.DeclarePrice = Convert.ToDouble(txtDeclarePrice.Text.Replace(",", ""));
                        }

                        if (!string.IsNullOrEmpty(txtShipModified.Text))
                        {
                            orderDetail.ShipModified = Convert.ToDouble(txtShipModified.Text.Replace(",", ""));
                        }
                        else
                        {
                            orderDetail.ShipModified = null;
                        }

                        orderDetail.ProductName = txtProductName.Text;

						var detailId = Convert.ToInt32(orderDetailId);
						var currentDetail = order.lstOrderDetailModel.ToList().Where(p => p.OrderDetailId == detailId).SingleOrDefault();
						if (order.CustomerTypeId != null && currentDetail.Weight != orderDetail.Weight)
                        {
							var configBusiness = _customerService.ConfigBusinessGet(
											""
											, ""
											, ""
											, ""
											, Const_BusinessCode.Business_402
											, ""
											, ""
											, ""
											, ""
											, this).Where(p => p.fromQuantity <= orderDetail.Weight && p.toQuantity >= orderDetail.Weight).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

                            if (configBusiness != null)
                            {
                                orderDetail.ShipConfigId = configBusiness.ConfigBusinessId;
                            }
                        }
                        var orderDetailReturn = _orderService.OrderDetailDeliveryUpdate(orderDetail, this);
                        if (orderDetailReturn != null)
                        {
                            mtvMain.ActiveViewIndex = 1;
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
                }

            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            btnBack_Click(sender, e);
        }

        #endregion

        #region //Private methods

        private void InitData()
        {
            try
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}


                if (Session["dictChangeStatus"] != null)
                {
                    var dictChangeStatus = (Dictionary<string, string>)Session["dictChangeStatus"];
                    string orderId = dictChangeStatus["orderId"];
                    var orderDetailId = Convert.ToInt32(dictChangeStatus["orderDetailId"]);
					var orderReturn = _orderService.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                    var orderDetailDelivery = orderReturn.lstOrderDetailModel.ToList().Where(x => x.OrderDetailId == orderDetailId).ToList().FirstOrDefault();

                    if (orderDetailDelivery != null)
                    {
                        //ddlCategory.SelectedValue = orderDetailDelivery.CategoryId + "";
                        txtProductName.Text = orderDetailDelivery.ProductName;
                        txtQuantity.Text = (orderDetailDelivery.Quantity ?? 0).ToString("N2");
                        txtWeight.Text = (orderDetailDelivery.Weight ?? 0).ToString("N2");
                        txtSurcharge.Text = (orderDetailDelivery.Surcharge ?? 0).ToString("N2");
                        txtDeclarePrice.Text = orderDetailDelivery.DeclarePrice != null ? (orderDetailDelivery.DeclarePrice ?? 0).ToString("N2") : "";
                        txtShipModified.Text = orderDetailDelivery.ShipModified != null ? (orderDetailDelivery.ShipModified ?? 0).ToString("N2") : "";
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion
    }
}