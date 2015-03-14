using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using System.Globalization;
using Ecms.Biz;
using CommonUtils;

namespace Ecms.Website.Site.MBGN
{
    public partial class Quotation : System.Web.UI.Page
    {
        #region //Declares

        private readonly CustomerService _customerService = new CustomerService();
        private readonly CommonService _commonService = new CommonService();


        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InitData();
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		void validateShop(string shopId)
		{
			lblError.Text = "";
			lblError.Visible = false;
			if (string.IsNullOrEmpty(shopId))
			{
				lblError.Text = "Thông tin shop không chính xác";
				lblError.Visible = true;
				return;
			}
		}

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                // Check:
                if (!IsValidData())
                {
                    return;
                }

                // Build:
                var order = new Order
                {
                    OrderTypeId = 1,
					Remark=txtRemark.Text
                };
				#region // configBusiness for ConfigRateId in Order table
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

                order.OrderDetails = new System.Data.Objects.DataClasses.EntityCollection<OrderDetail>();
                if (!string.IsNullOrEmpty(txtLink1.Text.Trim())
                    & !ddlWebsiteGroup1.SelectedValue.Equals("0"))
                {
                    var detail = new OrderDetail
                    {
                        ProductLink = txtLink1.Text.Trim(),
                        WebsiteId = Convert.ToInt32(ddlWebsiteGroup1.SelectedValue),
						CountryId = Constansts.CountryIdChina
                    };
                    order.OrderDetails.Add(detail);
                }

                if (!string.IsNullOrEmpty(txtLink2.Text.Trim())
					& !ddlWebsiteGroup2.SelectedValue.Equals("0"))
                {
                    var detail = new OrderDetail
                    {
                        ProductLink = txtLink2.Text.Trim(),
						WebsiteId = Convert.ToInt32(ddlWebsiteGroup2.SelectedValue),
						CountryId = Constansts.CountryIdChina
                    };
                    order.OrderDetails.Add(detail);
                }

                if (!string.IsNullOrEmpty(txtLink3.Text.Trim())
					& !ddlWebsiteGroup3.SelectedValue.Equals("0"))
                {
                    var detail = new OrderDetail
                    {
                        ProductLink = txtLink3.Text.Trim(),
						WebsiteId = Convert.ToInt32(ddlWebsiteGroup3.SelectedValue),
						CountryId = Constansts.CountryIdChina
                    };
                    order.OrderDetails.Add(detail);
                }

                if (!string.IsNullOrEmpty(txtLink4.Text.Trim())
					& !ddlWebsiteGroup4.SelectedValue.Equals("0"))
                {
                    var detail = new OrderDetail
                    {
                        ProductLink = txtLink4.Text.Trim(),
						WebsiteId = Convert.ToInt32(ddlWebsiteGroup4.SelectedValue),
						CountryId = Constansts.CountryIdChina
                    };
                    order.OrderDetails.Add(detail);
                }

                if (!string.IsNullOrEmpty(txtLink5.Text.Trim())
					& !ddlWebsiteGroup5.SelectedValue.Equals("0"))
                {
                    var detail = new OrderDetail
                    {
                        ProductLink = txtLink5.Text.Trim(),
						WebsiteId = Convert.ToInt32(ddlWebsiteGroup5.SelectedValue),
						CountryId = Constansts.CountryIdChina
                    };
                    order.OrderDetails.Add(detail);
                }

				foreach (var item in order.OrderDetails)
				{
					#region // configBusiness for ShipConfig

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
									, this).Where(p => p.fromQuantity <= item.Quantity && p.toQuantity >= item.Quantity).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

					if (configBusiness != null)
					{
						item.ShipConfigId = configBusiness.ConfigBusinessId;
					}

					#endregion

					#region // configBusiness for RateCountryId
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
					#endregion
				}

                Session["Quotation"] = order;
				if (Session["Customer"] == null)
				{
					Response.Redirect("~/site/mbgn/login.aspx?qtacart=1");
				}
				else
				{
					Response.Redirect("~/site/mbgn/confirmquotation.aspx");
				}
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

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/mbgn/orderproduct.aspx");
        }

        #endregion

        #region //Private Methods

        private void InitData()
        {
			var parentWebsite = new WebsiteService().WebsiteLinkGet("", "", "", "0", this);
            ddlWebsiteGroup1.DataSource = parentWebsite;
            ddlWebsiteGroup1.DataTextField = "WebsiteName";
            ddlWebsiteGroup1.DataValueField = "WebsiteId";
            ddlWebsiteGroup1.DataBind();

            ddlWebsiteGroup1.Items.Insert(0, new ListItem("-- Chọn website --", ""));
			//ddlWebsite1.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("-- Chọn Shop --", ""));
			//ddlWebsite1.SelectedValue = "";
            
            ddlWebsiteGroup2.DataSource = parentWebsite;
            ddlWebsiteGroup2.DataTextField = "WebsiteName";
            ddlWebsiteGroup2.DataValueField = "WebsiteId";
            ddlWebsiteGroup2.DataBind();

            ddlWebsiteGroup2.Items.Insert(0, new ListItem("-- Chọn website --", ""));
			//ddlWebsite2.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("-- Chọn Shop --", ""));
			//ddlWebsite2.SelectedValue = "";

            ddlWebsiteGroup3.DataSource = parentWebsite;
            ddlWebsiteGroup3.DataTextField = "WebsiteName";
            ddlWebsiteGroup3.DataValueField = "WebsiteId";
            ddlWebsiteGroup3.DataBind();

            ddlWebsiteGroup3.Items.Insert(0, new ListItem("-- Chọn website --", ""));
			//ddlWebsite3.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("-- Chọn Shop --", ""));
			//ddlWebsite3.SelectedValue = "";

            ddlWebsiteGroup4.DataSource = parentWebsite;
            ddlWebsiteGroup4.DataTextField = "WebsiteName";
            ddlWebsiteGroup4.DataValueField = "WebsiteId";
            ddlWebsiteGroup4.DataBind();

            ddlWebsiteGroup4.Items.Insert(0, new ListItem("-- Chọn website --", ""));
			//ddlWebsite4.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("-- Chọn Shop --", ""));
			//ddlWebsite4.SelectedValue = "";

            ddlWebsiteGroup5.DataSource = parentWebsite;
            ddlWebsiteGroup5.DataTextField = "WebsiteName";
            ddlWebsiteGroup5.DataValueField = "WebsiteId";
            ddlWebsiteGroup5.DataBind();

            ddlWebsiteGroup5.Items.Insert(0, new ListItem("-- Chọn website --", ""));
			//ddlWebsite5.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("-- Chọn Shop --", ""));
			//ddlWebsite5.SelectedValue = "";
        }

        private bool IsValidData()
        {
			string text1 = txtLink1.Text + (ddlWebsiteGroup1.SelectedValue.Equals("0") ? "" : ddlWebsiteGroup1.SelectedValue);
			string text2 = txtLink2.Text + (ddlWebsiteGroup2.SelectedValue.Equals("0") ? "" : ddlWebsiteGroup2.SelectedValue);
			string text3 = txtLink3.Text + (ddlWebsiteGroup3.SelectedValue.Equals("0") ? "" : ddlWebsiteGroup3.SelectedValue);
			string text4 = txtLink4.Text + (ddlWebsiteGroup4.SelectedValue.Equals("0") ? "" : ddlWebsiteGroup4.SelectedValue);
			string text5 = txtLink5.Text + (ddlWebsiteGroup5.SelectedValue.Equals("0") ? "" : ddlWebsiteGroup5.SelectedValue);

            if (string.IsNullOrEmpty(text1 + text2 + text3 + text4 + text5))
            {
                lblError.Text = "Quý khách Chọn Website và nhập ít nhất 1 link sản phẩm!";
                lblError.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(text1) & (string.IsNullOrEmpty(txtLink1.Text)))
            {
				lblError.Text = "Quý khách Chọn Website và nhập ít nhất 1 link sản phẩm!";
                lblError.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(text2) & (string.IsNullOrEmpty(txtLink2.Text)))
            {
				lblError.Text = "Quý khách Chọn Website và nhập ít nhất 1 link sản phẩm!";
                lblError.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(text3) & (string.IsNullOrEmpty(txtLink3.Text)))
            {
				lblError.Text = "Quý khách Chọn Website và nhập ít nhất 1 link sản phẩm!";
                lblError.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(text4) & (string.IsNullOrEmpty(txtLink4.Text)))
            {
				lblError.Text = "Quý khách Chọn Website và nhập ít nhất 1 link sản phẩm!";
                lblError.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(text5) & (string.IsNullOrEmpty(txtLink5.Text)))
            {
				lblError.Text = "Quý khách Chọn Website xứ và nhập ít nhất 1 link sản phẩm!";
                lblError.Visible = true;
                return false;
            }
            return true;
        }

        #endregion
    }
}