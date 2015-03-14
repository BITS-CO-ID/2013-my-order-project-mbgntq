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

namespace Ecms.Website.Site.MBGN
{
    public partial class OrderByLink : System.Web.UI.Page
    {
        #region //Declares

        private readonly CommonService _commonService = new CommonService();
        private readonly CustomerService _customerService = new CustomerService();

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
				lblError.Text = "";
				lblError.Visible = false;

				//if (cbbShop1.SelectedValue.ToString().Equals("-1"))
				//{
				//    lblError.Text = "Bạn chưa chọn thông tin Shop";
				//    lblError.Visible = true;
				//    return;
				//}

				//if (string.IsNullOrEmpty(cbbShop1.SelectedValue))
				//{
				//    lblError.Text = "Shop vừa nhập không có trong hệ thống website 247";
				//    lblError.Visible = true;
				//    return;
				//}

                List<OrderDetailModel> cartLinks = new List<OrderDetailModel>();
                OrderDetailModel orderDetailTemp = new OrderDetailModel();

				if (!string.IsNullOrEmpty(ddlWebsiteGroup.SelectedValue))
				{
					orderDetailTemp.WebsiteId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue); ;// Convert.ToInt32(ddlWebsite.SelectedValue);
					orderDetailTemp.WebsiteName = ddlWebsiteGroup.SelectedItem.Text;//ddlWebsite.SelectedItem.Text;
				}

				orderDetailTemp.CountryId = Constansts.CountryIdChina;
                orderDetailTemp.ProductLink = txtLinkProduct.Text;
                orderDetailTemp.ImageUrl = txtLinkProductImage.Text;

				if (string.IsNullOrEmpty(txtPriceWeb.Text))
				{
					orderDetailTemp.PriceWeb = 0;
				}
				else
				{
					orderDetailTemp.PriceWeb = Convert.ToDouble(txtPriceWeb.Text.Replace(",", ""));
				}
                orderDetailTemp.Quantity = Convert.ToDouble(txtQuantity.Text.Replace(",", ""));
                orderDetailTemp.Color = txtColor.Text;
                orderDetailTemp.Size = txtSize.Text;
				if (!string.IsNullOrEmpty(txtShop.Text))
				{
					orderDetailTemp.Shop = txtShop.Text;
				}

                if (Session["CartLink"] != null)
                {
                    cartLinks = (List<OrderDetailModel>)Session["CartLink"];
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
                Session["CartLink"] = cartLinks;
                txtColor.Text = txtLinkProduct.Text = txtLinkProductImage.Text = txtPriceWeb.Text
				= txtQuantity.Text = txtSize.Text = txtShop.Text = ddlWebsiteGroup.SelectedValue = "";
                LoadData();
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
			Session.Remove("Order");
			Session.Remove("CartLink");
            Response.Redirect("~/site/default.aspx");
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {               
                if (Session["CartLink"] != null)
                {

                    var cartLinks = (List<OrderDetailModel>)Session["CartLink"];
                    var customer = (UserCustomerModel)Session["Customer"];

                    Order order = new Order();
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

                    Session["Order"] = order;
                    Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");
                }
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
                        if (Session["CartLink"] != null)
                        {
                            var cartLinks = (List<OrderDetailModel>)Session["CartLink"];
                            if (cartLinks.Count != 0)
                            {
                                var orderDetail = cartLinks.Where(x => x.OrderDetailId == orderDetailId).FirstOrDefault();

                                if (orderDetail != null)
                                {
                                    cartLinks.Remove(orderDetail);
                                    Session["CartLink"] = cartLinks;
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

                if (Request.QueryString["WebsiteId"] != null)
                {
                    var website = _commonService.WebsiteList(Request.QueryString["WebsiteId"].ToString(), "", "").FirstOrDefault();
                    if (website != null)
                    {
                        ddlWebsiteGroup.SelectedValue = website.ParentId + "";
                        //var websiteBind = _commonService.WebsiteList("", "", "").Where(x=>x.ParentId == website.ParentId);

						//cbbShop1.DataSource = websiteBind;
						//cbbShop1.DataTextField = "WebsiteName";
						//cbbShop1.DataValueField = "WebsiteId";
						//cbbShop1.DataBind();
                    }
                }
				//cbbShop1.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("-- Chọn Shop --", "-1"));
				//cbbShop1.SelectedValue = "-1";
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private void LoadData()
        {
            if (Session["CartLink"] != null)
            {
                var cartLinks = (List<OrderDetailModel>)Session["CartLink"];
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

        #endregion
    }
}