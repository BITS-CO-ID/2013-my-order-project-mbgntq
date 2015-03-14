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
using System.Collections;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderDetailUpdate : PageBase
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
				if (ValidData(ddlWebsiteGroup.SelectedValue) == false) return;

				var categoryId = ddlCategory.SelectedValue;
				var weight = txtWeight.Text;

				var orderDetail = new OrderDetail();
				orderDetail.OrderDetailId = Convert.ToInt32(hdOrrderDetailId.Value);
				if (Request.QueryString["orderId"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
				var order = _orderService.OrderGet(
								Request.QueryString["orderId"].ToString()
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
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, ""
								, this).FirstOrDefault();

				if (order == null)
				{
					lblError.Text = "Đơn hàng không tồn tại!";
					lblError.Visible = true;
					return;
				}
				var currentOrderDetail = order.lstOrderDetailModel.ToList().Where(p => p.OrderDetailId == orderDetail.OrderDetailId).SingleOrDefault();
				if (order.OrderStatus == OrderInStatus.Deliveried)
				{
					lblError.Text = "Đơn hàng đã hoàn thành và giao hàng không được phép cập nhật!";
					lblError.Visible = true;
					return;
				}

				if (!string.IsNullOrEmpty(categoryId))
				{
					orderDetail.CategoryId = Convert.ToInt32(categoryId);
				}

				orderDetail.WebsiteId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue);
				orderDetail.CountryId = currentOrderDetail.CountryId;
				orderDetail.ShipConfigId = currentOrderDetail.ShipConfigId;
				orderDetail.RateCountryId = currentOrderDetail.RateCountryId;

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

				if (!string.IsNullOrEmpty(txtColor.Text))
				{
					orderDetail.Color = txtColor.Text;
				}

				if (!string.IsNullOrEmpty(txtSurcharge.Text))
				{
					orderDetail.Surcharge = Convert.ToDouble(txtSurcharge.Text.Replace(",", ""));
				}

				if (!string.IsNullOrEmpty(txtShipModified.Text) && chkChangeShip.Checked)
				{
					orderDetail.ShipModified = Convert.ToDouble(txtShipModified.Text.Replace(",", ""));
				}
				else
				{
					orderDetail.ShipModified = null;
				}

				orderDetail.Size = txtSize.Text;
				orderDetail.ImageUrl = txtImage.Text;
				orderDetail.Shop = txtShop.Text;
				orderDetail.Remark = txtRemark.Text;

				//Get ship fee for quantity change
				if (orderDetail.Quantity != currentOrderDetail.Quantity && !chkChangeShip.Checked)
				{
					//var configBusiness = _customerService.ConfigBusinessGet(
					//						""
					//						, ""
					//						, ""
					//						, ""
					//						, Const_BusinessCode.Business_401
					//						, ""
					//						, ""
					//						, ""
					//						, ""
					//						, this).Where(p => p.fromQuantity <= orderDetail.Quantity && p.toQuantity >= orderDetail.Quantity).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

					//if (configBusiness != null)
					//{
					//	orderDetail.ShipConfigId = configBusiness.ConfigBusinessId;
					//}
				}

				if (orderDetail.Quantity != currentOrderDetail.Quantity || orderDetail.Surcharge != currentOrderDetail.Surcharge)
				{
					// validate payment

				}

				var orderDetailreturn = _orderService.OrderDetailUpdate(orderDetail, this.chkChangeShip.Checked, this);
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			if(Request.QueryString["ubl"] != null)
			{
				// groupLink
				Response.Redirect("~/admin/order/grouplink.aspx");
			}
			else if (Request.QueryString["Odsu"] != null)
			{
				// statusUpdate
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				if (Request.QueryString["orderDetailId"] != null && Request.QueryString["orderId"] != null && Request.QueryString["orderOutboundId"] != null)
				{
					var orderOutbound = _orderService.OrderOutboundGet(
											Request.QueryString["orderOutboundId"].ToString()
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
											, ""
											, ""
											, this).FirstOrDefault();
					if (orderOutbound != null)
					{
						Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] = orderOutbound;
						Response.Redirect("~/admin/order/orderoutbounddetail.aspx");
					}
				}
				else
				{
					Response.Redirect("~/admin/order/orderbylinkdetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
				}
			}
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
			if (Request.QueryString["ubl"] != null)
			{
				// groupLink
				Response.Redirect("~/admin/order/grouplink.aspx");
			}
			else if (Request.QueryString["Odsu"] != null)
			{
				// statusUpdate
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				if (Request.QueryString["orderDetailId"] != null && Request.QueryString["orderId"] != null && Request.QueryString["orderOutboundId"] != null)
				{
					var orderOutbound = _orderService.OrderOutboundGet(
											Request.QueryString["orderOutboundId"].ToString()
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
											, ""
											, ""
											, this).FirstOrDefault();
					if (orderOutbound != null)
					{
						Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] = orderOutbound;
						Response.Redirect("~/admin/order/orderoutbounddetail.aspx");
					}
				}
				else
				{
					Response.Redirect("~/admin/order/orderbylinkdetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
				}
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

        #endregion

        #region //Private methods

        private void InitData()
        {
			if (Session["User"] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}

			
			// assign permisstion

			if ((Session["PolicyUserMH"] != null))
			{
				//ArrayList controlList = new ArrayList();
				//this.AddControls(Page.Controls, controlList);

				//foreach (Control control in controlList)
				//{
				//	if (control is TextBox)
				//	{
				//		if (control.ID == "txtRemark")
				//		{
				//			((TextBox)control).Enabled = true;
				//		}
				//		else
				//		{
				//			((TextBox)control).Enabled = false;
				//		}
				//	}

				//	if (control is DropDownList)
				//	{
				//		((DropDownList)control).Enabled = false;
				//	}

				//	if (control is CheckBox)
				//	{
				//		((CheckBox)control).Enabled = false;
				//	}
				//}
			}


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
            var orderDetailModel = new OrderDetailModel();
            if (Request.QueryString["orderDetailId"] != null && Request.QueryString["orderId"] != null && Request.QueryString["orderOutboundId"] != null)
            {
				var order = _orderService.OrderGet(Request.QueryString["orderId"].ToString(),
													"", "", "", "", "", "", "", "", "", "",
													"", "", "", "", "", "", "", "", this).FirstOrDefault();
                if (order != null)
                {
                    var orderDetailId = Convert.ToInt32(Request.QueryString["orderDetailId"].ToString());
                    orderDetailModel = order.lstOrderDetailModel.ToList().FirstOrDefault(x => x.OrderDetailId == orderDetailId);

					if (order.OrderStatus == OrderInStatus.OrderCancel
						 // || order.OrderStatus == OrderInStatus.OrderConfirmed // comment tam
						 || order.OrderStatus == OrderInStatus.Finished
						 || order.OrderStatus == OrderInStatus.Deliveried
						)
					{
						// if order staus in status abow then not allow edit fee(not fee ship)
						
						//// comment tam
						txtSurcharge.Enabled = false;
						txtQuantity.Enabled = false;
					}
                }
            }
            else
            {
                if (Session[Constansts.SS_ORDER_DETAIL_ADMIN] != null)
                {
                    orderDetailModel = (OrderDetailModel)Session[Constansts.SS_ORDER_DETAIL_ADMIN];

					if (orderDetailModel.OrderStatus == OrderInStatus.OrderCancel
						 //|| orderDetailModel.OrderStatus == OrderInStatus.OrderConfirmed // comment tam
						 || orderDetailModel.OrderStatus == OrderInStatus.Finished
						 || orderDetailModel.OrderStatus == OrderInStatus.Deliveried
						)
					{
						// if order staus in status abow then not allow edit fee(not fee ship)
						
						txtSurcharge.Enabled = false;
						txtQuantity.Enabled = false;
					}
                }
            }

            if (orderDetailModel != null)
            {
                hdOrrderDetailId.Value = orderDetailModel.OrderDetailId + "";
                
                txtPriceWeb.Text = (orderDetailModel.PriceWeb ?? 0).ToString("N2");
                txtQuantity.Text = (orderDetailModel.Quantity ?? 0).ToString("N2");
                //lblCurencyCode.Text = orderDetailModel.CurrencyCode;
                lblStatusText.Text = orderDetailModel.DetailStatusText;
                lblProductLink.Text = string.Format("<a href='{0}' title='{1}' target='_blank'>{2}</a>",
                                                    orderDetailModel.ProductLink, orderDetailModel.ProductLink,
                                                    orderDetailModel.ProductLink.Length < 100 ? orderDetailModel.ProductLink : orderDetailModel.ProductLink.Substring(0, 100));
                lblProductImage.Text = string.Format("<a href='{0}' target='_blank'><img src='{1}' width='100' height='100' /></a>",
                                                        orderDetailModel.ImageUrl, orderDetailModel.ImageUrl);
                txtSize.Text = orderDetailModel.Size;
                txtImage.Text = orderDetailModel.ImageUrl;
                txtColor.Text = orderDetailModel.Color;
                //txtPriceWebOff.Text = orderDetailModel.PriceWebOff != null ? (Convert.ToDouble(orderDetailModel.PriceWebOff) + "") : "";
                txtWeight.Text = orderDetailModel.Weight != null ? (Convert.ToDouble(orderDetailModel.Weight) + "") : "";
                txtSurcharge.Text = orderDetailModel.Surcharge != null ? (Convert.ToDouble(orderDetailModel.Surcharge) + "") : "";
                ddlCategory.SelectedValue = orderDetailModel.CategoryId != null ? (orderDetailModel.CategoryId + "") : "";
                txtShipModified.Text = orderDetailModel.ShipModified != null ? orderDetailModel.ShipModified.Value.ToString("N2") : "";
				lblShipConfigValue.Text = (orderDetailModel.ShipConfigValue ?? 0).ToString("N2");

				if (orderDetailModel.ShipModified != null)
				{
					chkChangeShip.Checked = true;
					txtShipModified.Enabled = true;
				}
				else
				{
					chkChangeShip.Checked = false;
					txtShipModified.Enabled = false;
				}
				ddlWebsiteGroup.SelectedValue = orderDetailModel.WebsiteId + "";
				txtShop.Text = orderDetailModel.Shop;
				txtRemark.Text = orderDetailModel.Remark;
            }

			// for permision
			if (Session["PolicyUserMH"] != null)
			{
				txtPriceWeb.Enabled = false;
			}
			else
			{
				txtPriceWeb.Enabled = true;
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