using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Biz;
using System.Globalization;
using Ecms.Website.Common;
using CommonUtils;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderByLink : System.Web.UI.Page
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();
        private readonly CustomerService _customerService = new CustomerService();
		private readonly OrderService _orderService = new OrderService();

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

        protected void btnAddToCartLink_Click(object sender, EventArgs e)
        {
            try
            {
				// validate
				if (!validate()) return;

                List<OrderDetailModel> cartLinks = new List<OrderDetailModel>();
                OrderDetailModel orderDetailTemp = new OrderDetailModel();

				if (!string.IsNullOrEmpty(ddlWebsiteGroup.SelectedValue))
				{
					orderDetailTemp.WebsiteId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue);
					orderDetailTemp.WebsiteName = ddlWebsiteGroup.SelectedItem.Text;
				}

				orderDetailTemp.CountryId = Constansts.CountryIdChina;
                orderDetailTemp.ProductLink = txtLinkProduct.Text;
                orderDetailTemp.ImageUrl = txtLinkProductImage.Text;
                orderDetailTemp.PriceWeb = Convert.ToDouble(txtPriceWeb.Text.Replace(",", ""));
                orderDetailTemp.Quantity = Convert.ToDouble(txtQuantity.Text.Replace(",", ""));
                orderDetailTemp.Color = txtColor.Text;
				orderDetailTemp.Size = txtSize.Text;
				if(!string.IsNullOrEmpty(txtShop.Text))
				{
					orderDetailTemp.Shop = txtShop.Text;
				}

                if (Session["AdminCartLink"] != null)
                {
                    cartLinks = (List<OrderDetailModel>)Session["AdminCartLink"];
                    if (cartLinks.Count != 0)
                    {
                        var orderDetailId = cartLinks.OrderByDescending(x => x.OrderDetailId).FirstOrDefault().OrderDetailId + 1;
                        orderDetailTemp.OrderDetailId = orderDetailId;
                    }
                }
                else
                {
                    orderDetailTemp.OrderDetailId = 1;
                }

                cartLinks.Add(orderDetailTemp);
                Session["AdminCartLink"] = cartLinks;
                txtColor.Text = txtLinkProduct.Text = txtLinkProductImage.Text = txtPriceWeb.Text = txtQuantity.Text = ddlWebsiteGroup.SelectedValue = "";
                LoadData();
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			Session.Remove("AdminOrder");
			Session.Remove("AdminCartLink");
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

		//Bước 2: tạo đơn hàng
		protected void btnOrderTemp_Click(object sender, EventArgs e)
		{
			mtvMain.ActiveViewIndex = 1;
		}

		// order save
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
			{
				if (Session["User"] ==null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}

				lblError.Visible = false;
				#region // temp
				if (Session["AdminCartLink"] != null)
                {
                    var cartLinks = (List<OrderDetailModel>)Session["AdminCartLink"];
                    var customer = (UserCustomerModel)Session["Customer"];

					Ecms.Biz.Entities.Order order = new Ecms.Biz.Entities.Order();
                    order.OrderDate = order.CreatedDate = DateTime.Now;
                    order.OrderTypeId = 2;                    

                    foreach (var item in cartLinks)
                    {
                        var orderDetailTemp = new OrderDetail();
                        orderDetailTemp.WebsiteId = item.WebsiteId;
                        orderDetailTemp.ProductLink = item.ProductLink;
                        orderDetailTemp.ImageUrl = item.ImageUrl;
                        orderDetailTemp.CountryId = item.CountryId;
                        orderDetailTemp.PriceWeb = item.PriceWeb;

                        if ((item.PriceWebOff ?? 0) != 0)
                        {
                            orderDetailTemp.PriceWebOff = item.PriceWebOff;
                        }                                   
                        orderDetailTemp.Quantity = item.Quantity;
                        orderDetailTemp.Color = item.Color;
                        orderDetailTemp.Size = item.Size;
						orderDetailTemp.Shop = item.Shop;
                        order.OrderDetails.Add(orderDetailTemp);
                    }

                    Session["AdminOrder"] = order;
				}
				#endregion

				#region // tạo order
				if (Session["AdminOrder"] != null)
				{
					var customer = _customerService.CustomerList(""
						, ""
						, this.txtCustomerUserCode.Text.Trim()
						, ""
						, ""
						, ""
						, ""
						, ""
						, ""
						, ""
						, ""
						, this).SingleOrDefault();

					if (customer == null)
					{
						lblError.Text = string.Format("Tài khoản {0} yêu cầu đặt hàng không chính xác!", this.txtCustomerUserCode.Text.Trim());
						lblError.Visible = true;
						return;
					}
					var order = (Ecms.Biz.Entities.Order)Session["AdminOrder"];
					order.DeliveryAddress = customer.DeliveryAddress;
					order.DeliveryEmail = customer.DeliveryEmail;
					order.DeliveryMobile = customer.DeliveryMobile;
					order.DeliveryName = customer.DeliveryName;
					order.Remark = txtNote.Text;
					order.CustomerId = customer.CustomerId;
					order.OrderStatus = 3;

					order.CreateUser = Session["User"].ToString();

					foreach (var item in order.OrderDetails)
					{
						#region // configBusiness for ShipConfig
						if (order.OrderTypeId == 2)
						{
							if (!string.IsNullOrEmpty(item.Shop))
							{
								var shopQty = order.OrderDetails.Where(m => m.Shop == item.Shop).ToList().Sum(p => p.Quantity).Value;

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
												, this).Where(p => p.fromQuantity <= shopQty && p.toQuantity >= shopQty).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

								if (configBusiness != null)
								{
									item.ShipConfigId = configBusiness.ConfigBusinessId;
								}
							}
						}
						#endregion

						#region // configBusiness for RateCountryId
						if (order.OrderTypeId == 2)
						{
							if (item.CountryId != null)
							{
								var configBusinessRate = _customerService.ConfigBusinessGet(
												""
												, ""
												, ""
												, Convert.ToString(item.CountryId)
												, Const_BusinessCode.Business_ORGRATE
												, ""
												, ""
												, ""
												, ""
												, this).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

								if (configBusinessRate != null)
								{
									item.RateCountryId = configBusinessRate.ConfigBusinessId;
								}
							}
						}
						#endregion
					}

					#region // configBusiness for ConfigRateId in AdminOrder table
					var configBusinessRateOrder = _customerService.ConfigBusinessGet(
									""
									, ""
									, ""
									, ""
									, Const_BusinessCode.Business_ORGRATEDE
									, ""
									, ""
									, ""
									, ""
									, this).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

					if (configBusinessRateOrder != null)
					{
						order.ConfigRateId = configBusinessRateOrder.ConfigBusinessId;
					}
					#endregion

					var orderResturn = _orderService.OrderCreate(order, this);
					if (orderResturn != null)
					{
						Session.Remove("AdminOrder");
						Session.Remove("AdminCartLink");
						mtvMain.ActiveViewIndex = 2;
					}
					return;
				}
				#endregion
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void gridCartByLink_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "deleteProduct":
                        int orderDetailId = Convert.ToInt32(e.CommandArgument);
                        if (Session["AdminCartLink"] != null)
                        {
                            var cartLinks = (List<OrderDetailModel>)Session["AdminCartLink"];
                            if (cartLinks.Count != 0)
                            {
                                var orderDetail = cartLinks.Where(x => x.OrderDetailId == orderDetailId).FirstOrDefault();

                                if (orderDetail != null)
                                {
                                    cartLinks.Remove(orderDetail);
                                    Session["AdminCartLink"] = cartLinks;
                                    Response.Redirect("");
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		protected void btnBack_Click(object sender, EventArgs e)
		{
			mtvMain.ActiveViewIndex = 0;
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/admin/order/ordermanage.aspx");
		}

        #endregion

        #region //Private methods

        private void InitData()
        {
            try
            {
				ddlWebsiteGroup.DataSource = new WebsiteService().WebsiteLinkGet("", "", "", "0", this);
                ddlWebsiteGroup.DataTextField = "WebsiteName";
                ddlWebsiteGroup.DataValueField = "WebsiteId";
                ddlWebsiteGroup.DataBind();

                ddlWebsiteGroup.Items.Insert(0, new ListItem("-- Chọn website --", ""));
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private void LoadData()
        {
            if (Session["AdminCartLink"] != null)
            {
                var cartLinks = (List<OrderDetailModel>)Session["AdminCartLink"];
                if (cartLinks.Count != 0)
                {
                    gridCartByLink.DataSource = cartLinks;
                    gridCartByLink.DataBind();
                    pnCartLink.Visible = true;
                }
                else
                {
                    pnCartLink.Visible = false;
                }
            }
        }

		private bool validate()
		{
			lblError.Visible = false;
			if (string.IsNullOrEmpty(ddlWebsiteGroup.SelectedValue))
			{
				lblError.Text = "Chưa chọn nhóm website";
				lblError.Visible = true;
				return false;
			}

			if (string.IsNullOrEmpty(txtLinkProduct.Text.Trim()))
			{
				lblError.Text = "Chưa link sản phẩm";
				lblError.Visible = true;
				return false;
			}

			if (string.IsNullOrEmpty(txtShop.Text))
			{
				lblError.Text = "Chưa nhập Shop";
				lblError.Visible = true;
				return false;
			}

			if (string.IsNullOrEmpty(txtQuantity.Text))
			{
				lblError.Text = "Chưa nhập số lượng";
				lblError.Visible = true;
				return false;
			}

			if (string.IsNullOrEmpty(txtPriceWeb.Text))
			{
				lblError.Text = "Chưa nhập giá web";
				lblError.Visible = true;
				return false;
			}

			return true;
		}

        #endregion
    }
}