using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using CommonUtils;
using Ecms.Biz;

namespace Ecms.Website.Site.MBGN
{
    public partial class AddInfoDelivery : System.Web.UI.Page
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] == null)
                {
                    var returnUrl = Request.Url.AbsolutePath;
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx?returnUrl=" + returnUrl);
                }

                var userCustomer = (UserCustomerModel)Session["Customer"];
                if (userCustomer != null)
                {
                    if (string.IsNullOrEmpty(userCustomer.DeliveryAddress + userCustomer.DeliveryEmail + userCustomer.DeliveryName + userCustomer.DeliveryMobile))
                    {
                        this.txtAddress.Text = userCustomer.Address;
                        this.txtEmail.Text = userCustomer.Email;
                        this.txtFullName.Text = userCustomer.CustomerName;
                        this.txtMobile.Text = userCustomer.Mobile;
                    }
                    else
                    {
                        // load default delivery address
                        this.txtAddress.Text = userCustomer.DeliveryAddress;
                        this.txtEmail.Text = userCustomer.DeliveryEmail;
                        this.txtFullName.Text = userCustomer.DeliveryName;
                        this.txtMobile.Text = userCustomer.DeliveryMobile;
                    }
                }
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {                
                #region // tạo order
                if (Session["Order"] != null)
                {
                    var customer = (UserCustomerModel)Session["Customer"];
                    var order = (Order)Session["Order"];
                    order.DeliveryAddress = txtAddress.Text;
                    order.DeliveryEmail = txtEmail.Text;
                    order.DeliveryMobile = txtMobile.Text;
                    order.DeliveryName = txtFullName.Text;
					order.Remark = txtRemark.Text;
                    order.CustomerId = customer.CustomerId;
                    order.OrderStatus = 3;
					order.CreateUser = customer.UserCode;
                    if (order.OrderTypeId == 3)
                    {
                        order.CustomerCodeDelivery = customer.CustomerCodeDelivery;
                    }

					#region // Get some ConfigBusinessId for current detail

					#region // for orderDetail

					foreach (var item in order.OrderDetails)
					{
						#region // configBusiness for ShipConfig
						if (order.OrderTypeId == 2)
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

						if (order.OrderTypeId == 3)
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
											, this).Where(p => p.fromQuantity <= item.Weight && p.toQuantity >= item.Weight).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

							if (configBusiness != null)
							{
								item.ShipConfigId = configBusiness.ConfigBusinessId;
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
					
					#endregion

					#region // configBusiness for ConfigRateId in Order table
					var configBusinessRateOrder = _customerService.ConfigBusinessGet(
									""
									, ""
									, ""
									, ""//Convert.ToString(item.CountryId)
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
					#endregion

					var orderResturn = _orderService.OrderCreate(order, this);
                    if (orderResturn != null)
                    {
                        Session.Remove("Order");
                        Session.Remove("Cart");
                        Session.Remove("CartLink");
                        Session.Remove("CartTransport");
                        Session["OrderCode"] = orderResturn.OrderNo;
                        mtvMain.ActiveViewIndex = 1;
                    }
                    return;
                }
                #endregion

                #region // Chuyển sang Order từ báo giá
                if (Session[Constansts.SESSION_ORDERMODEL] != null)
                {
                    var order = (OrderModel)Session[Constansts.SESSION_ORDERMODEL];
                    order.DeliveryAddress = txtAddress.Text;
                    order.DeliveryEmail = txtEmail.Text;
                    order.DeliveryMobile = txtMobile.Text;
                    order.DeliveryName = txtFullName.Text;
					order.Remark = txtRemark.Text;

                    if (_orderService.ConvertQuoteOrder(
                        Convert.ToString(order.OrderId)
                        , ""//order.Remark
                        , ""
                        , order.DeliveryDate == null ? "" : order.DeliveryDate.Value.ToString("yyyy-MM-dd")
                        , order.DeliveryAddress
                        , order.DeliveryName
                        , order.DeliveryMobile
                        , order.DeliveryEmail
                        , this))
                    {
                        mtvMain.ActiveViewIndex = 1;
                        Session.Remove(Constansts.SESSION_ORDERMODEL);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/mbgn/orderproduct.aspx");
        }
        #endregion
    }
}