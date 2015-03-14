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
using Ecms.Biz.Entities;

namespace Ecms.Website.Admin.Order
{
    public partial class QuotationDetailUpdate : PageBase
    {
        #region //Declares

        private readonly ProductService _productService = new ProductService();
        private readonly OrderService _orderService = new OrderService();
        private readonly CustomerService _customerService = new CustomerService();
        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                LoadData();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
			try
			{
				if (ValidData(ddlWebsiteGroup.SelectedValue) == false)
					return;

				var categoryId = ddlCategory.SelectedValue;
				var weight = txtWeight.Text;


				if (Request.QueryString["orderId"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
				var orderDetail = new OrderDetail();
				orderDetail.OrderDetailId = Convert.ToInt32(hdOrrderDetailId.Value);
				

				var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
				if (order == null)
				{
					lblError.Text = "Báo giá không tồn tại!";
					lblError.Visible = true;
					return;
				}
				var currentOrderDetail = order.lstOrderDetailModel.ToList().Where(p => p.OrderDetailId == orderDetail.OrderDetailId).SingleOrDefault();

				orderDetail.CountryId = currentOrderDetail.CountryId;
				orderDetail.ShipConfigId = currentOrderDetail.ShipConfigId;
				orderDetail.RateCountryId = currentOrderDetail.RateCountryId;

				if (order.OrderStatus == 2)
				{
					lblError.Text = "Báo giá đã trả lời không được phép cập nhật!";
					lblError.Visible = true;
					return;
				}				

				orderDetail.WebsiteId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue);

				if (!string.IsNullOrEmpty(weight))
				{
					orderDetail.Weight = Convert.ToDouble(weight.Replace(",", ""));
				}

				if (!string.IsNullOrEmpty(txtQuantity.Text))
				{
					orderDetail.Quantity = Convert.ToDouble(txtQuantity.Text.Replace(",", ""));
				}
				else
				{
					orderDetail.Quantity = 1;
				}

				if (!string.IsNullOrEmpty(txtPriceWeb.Text))
				{
					orderDetail.PriceWeb = Convert.ToDouble(txtPriceWeb.Text.Replace(",", ""));
				}

				if (!string.IsNullOrEmpty(txtPriceWebOff.Text))
				{
					orderDetail.PriceWebOff = Convert.ToDouble(txtPriceWebOff.Text.Replace(",", ""));
				}

				if (!string.IsNullOrEmpty(txtColor.Text))
				{
					orderDetail.Color = txtColor.Text;
				}

				if (!string.IsNullOrEmpty(txtSurcharge.Text))
				{
					orderDetail.Surcharge = Convert.ToDouble(txtSurcharge.Text.Replace(",", ""));
				}

				if (!string.IsNullOrEmpty(txtShipModified.Text))
				{
					orderDetail.ShipModified = Convert.ToDouble(txtShipModified.Text.Replace(",", ""));
				}
				else
				{
					orderDetail.ShipModified = null;
				}

				orderDetail.Size = txtSize.Text;
				orderDetail.ImageUrl = txtImage.Text;
				orderDetail.Shop = txtShop.Text.Trim();

				//Get ship fee for quantity change
				if (orderDetail.Quantity != currentOrderDetail.Quantity && !chkChangeShip.Checked)
				{
					var configBusiness = _customerService.ConfigBusinessGet(
											""
											, ""
											, ""
											, ""
											, Const_BusinessCode.Business_401
											, ""
											, ""
											, ""
											, ""
											, this).Where(p => p.fromQuantity <= orderDetail.Quantity && p.toQuantity >= orderDetail.Quantity).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

					if (configBusiness != null)
					{
						orderDetail.ShipConfigId = configBusiness.ConfigBusinessId;
					}
				}

				var orderDetailreturn = _orderService.OrderDetailUpdate(orderDetail,true, this);
				if (orderDetailreturn != null)
				{
					mtvMain.ActiveViewIndex = 1;
					Session["orderId"] = orderDetailreturn.OrderId + "";
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

		protected void chkChangeShip_OnCheckedChanged(object sender, EventArgs e)
		{
			if (chkChangeShip.Checked)
			{
				txtShipModified.Enabled = true;
			}
			else
			{
				txtShipModified.Enabled = false;
			}
		}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/quotationreply.aspx?orderId=" + Request.QueryString["orderId"].ToString());
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/quotationreply.aspx?orderId=" + Request.QueryString["orderId"].ToString());
        }

        #endregion

        #region //Private methods

        private void InitData()
        {
            if (Session["User"] == null)
                Response.Redirect("~/admin/security/login.aspx");

            ddlCategory.DataSource = this._productService.CategoryGet("", "", "-1", this);
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- Chọn chủng loại sản phẩm --", ""));

            ddlWebsiteGroup.DataSource = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == null);
            ddlWebsiteGroup.DataTextField = "WebsiteName";
            ddlWebsiteGroup.DataValueField = "WebsiteId";
            ddlWebsiteGroup.DataBind();

            ddlWebsiteGroup.Items.Insert(0, new ListItem("-- Chọn website --", ""));
        }

        private void LoadData()
        {
            if (Session[Constansts.SS_ORDER_DETAIL_ADMIN] != null)
            {
                var orderDetailModel = (OrderDetailModel)Session[Constansts.SS_ORDER_DETAIL_ADMIN];
                hdOrrderDetailId.Value = orderDetailModel.OrderDetailId + "";
                txtPriceWeb.Text = orderDetailModel.PriceWeb != null ? (Convert.ToDouble(orderDetailModel.PriceWeb) + "") : "";
                txtQuantity.Text = (orderDetailModel.Quantity ?? 0).ToString("N2");
                lblCurencyCode.Text = orderDetailModel.CurrencyCode;
                lblStatusText.Text = orderDetailModel.DetailStatusText;
                lblProductLink.Text = string.Format("<a href='{0}' title='{1}' target='_blank'>{2}</a>",
                                                    orderDetailModel.ProductLink, orderDetailModel.ProductLink,
                                                    orderDetailModel.ProductLink.Length < 100 ? orderDetailModel.ProductLink : orderDetailModel.ProductLink.Substring(0, 100));
                lblProductImage.Text = string.Format("<a href='{0}' target='_blank'><img src='{1}' width='50' height='50' /></a>",
                                                        orderDetailModel.ImageUrl, orderDetailModel.ImageUrl);
                txtSize.Text = orderDetailModel.Size;
                txtImage.Text = orderDetailModel.ImageUrl;
                txtColor.Text = orderDetailModel.Color;
                txtPriceWebOff.Text = orderDetailModel.PriceWebOff != null ? (Convert.ToDouble(orderDetailModel.PriceWebOff) + "") : "";
                txtWeight.Text = orderDetailModel.Weight != null ? (Convert.ToDouble(orderDetailModel.Weight) + "") : "";
                txtSurcharge.Text = orderDetailModel.Surcharge != null ? (Convert.ToDouble(orderDetailModel.Surcharge) + "") : "";
                ddlCategory.SelectedValue = orderDetailModel.CategoryId != null ? (orderDetailModel.CategoryId + "") : "";
                txtShipModified.Text = orderDetailModel.ShipModified != null ? orderDetailModel.ShipModified.Value.ToString("N2") : "";
				lblShipConfigValue.Text = (orderDetailModel.ShipConfigValue ?? 0).ToString("N2");


				ddlWebsiteGroup.SelectedValue = orderDetailModel.WebsiteId + "";
				txtShop.Text = orderDetailModel.Shop;
            }
        }

        private bool ValidData(string websiteId)
        {
			if (string.IsNullOrEmpty(websiteId))
			{
				lblError.Text = "Bạn chưa chọn website!";
				lblError.Visible = true;
				return false;
			}

			if (string.IsNullOrEmpty(txtShop.Text))
			{
				lblError.Text = "Bạn chưa chọn Shop!";
				lblError.Visible = true;
				return false;
			}
			return true;
        }

        #endregion
    }
}