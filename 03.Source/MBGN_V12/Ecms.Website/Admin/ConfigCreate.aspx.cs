using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Biz;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using CommonUtils;

namespace Ecms.Website.Admin
{
    public partial class ConfigCreate : PageBase
    {

        #region // Declaration

        private readonly CustomerService _customerService = new CustomerService();
        private readonly CommonService _commonService = new CommonService();
        private readonly ProductService _productService = new ProductService();
        private readonly WebsiteService _websiteService = new WebsiteService();

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Admin/Security/Login.aspx");
                }

                try
                {
                    this.LoadData();
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var orderType = ddlOrderType.SelectedValue;
            var website = ddlWebsite.SelectedValue;
            var country = ddlOrg.SelectedValue;
            var applyService = ddlApplyService.SelectedValue;
            var customerType = ddlCustomerType.SelectedValue;
            var category = ddlCategory.SelectedValue;
            var configValue = txtConfigValue.Text;
            if (ValidData(orderType, website, country, applyService, customerType, category, configValue) == false)
                return;

            try
            {
                var config = new ConfigBusiness();

                config.ConfigValue = Convert.ToDouble(configValue);
                config.CreatedDate = config.FromDate = DateTime.Now;

                if (orderType.Equals("101")) // công
                {
                    config.WebsiteId = Convert.ToInt32(website);
                    config.CustomerTypeId = Convert.ToInt32(customerType);
                    config.OrderTypeId = 2;
                }

                if (orderType.Equals("ORGRATE")) // tỉ giá
                {
                    config.CountryId = Convert.ToInt32(country);
                }

				if (ddlOrderType.SelectedValue.Equals(Const_BusinessCode.Business_401)
					|| ddlOrderType.SelectedValue.Equals(Const_BusinessCode.Business_402)) // phí
				{
					config.fromQuantity = Convert.ToDouble(txtFromQuantity.Text.Trim());
					config.toQuantity = Convert.ToDouble(txtToQuantity.Text.Trim());
				}

				if (orderType.Equals((Const_BusinessCode.Business_401)))
				{
					config.Unit = "Số lượng";
				}

				if (orderType.Equals((Const_BusinessCode.Business_402)))
				{
					config.Unit = "Kg";
				}

                if (orderType.Equals("FEE"))
                {
                    if (applyService.Equals("FEEMH")) //Phí mua hộ
                    {
                        config.OrderTypeId = 2;
                    }

                    if (applyService.Equals("FEE"))//Phí vận chuyển
                    {
                        config.OrderTypeId = 3;
                    }
                    config.CategoryId = Convert.ToInt32(category);
                    config.CustomerTypeId = Convert.ToInt32(customerType);
                    config.BusinessCode = applyService;
                }
                else
                {
                    config.BusinessCode = orderType;
                }

                var result = _customerService.ConfigBusinessCreate(config, this);
                mtvMain.ActiveViewIndex = 1;
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfigList.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfigList.aspx");
        }

        protected void ddlOrderType_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlOrderType.SelectedValue.Equals("101")) // công
            {
                trWebsite.Visible = trWebsiteGroup.Visible = trCustomerType.Visible = true;
                trCountry.Visible = trCategory.Visible = trApplyService.Visible = false;
                lblCurrency.Text = "%";
            }

            if (ddlOrderType.SelectedValue.Equals("ORGRATE")) // tỉ giá
            {
                trCountry.Visible = true;
				trWebsite.Visible = trCustomerType.Visible = trCategory.Visible = trApplyService.Visible = trFromQuantity.Visible=trToQuantity.Visible = false;
                lblCurrency.Text = "VND";

				ddlOrg.Text = Convert.ToString(Constansts.CountryIdChina);
				ddlOrg.Enabled = false;
            }

            if (ddlOrderType.SelectedValue.Equals("FEE")) // phí
            {
                trWebsiteGroup.Visible = trWebsite.Visible = trCountry.Visible = false;
                trCustomerType.Visible = trCategory.Visible = trApplyService.Visible = true;
                lblCurrency.Text = "USD";
            }

            if (ddlOrderType.SelectedValue.Equals("303") || ddlOrderType.SelectedValue.Equals("TAXUSA") || ddlOrderType.SelectedValue.Equals("INSUARANCE")) //Thuế và giá trị cộng thêm 
            {
                trCustomerType.Visible = trCategory.Visible = trApplyService.Visible = trWebsite.Visible = trWebsiteGroup.Visible = trCountry.Visible = false;
                lblCurrency.Text = "%";
            }

            if (ddlOrderType.SelectedValue.Equals("ORGRATEDE")) // phí
            {
                trCustomerType.Visible = trCategory.Visible = trApplyService.Visible
					= trWebsite.Visible = trWebsiteGroup.Visible = trCountry.Visible = trFromQuantity.Visible = trToQuantity.Visible = false;
                lblCurrency.Text = "VND";
            }

			if (ddlOrderType.SelectedValue.Equals(Const_BusinessCode.Business_401)) // phí ship mua hộ
			{
				trCustomerType.Visible = trCategory.Visible = trApplyService.Visible= trWebsite.Visible = trWebsiteGroup.Visible = trCountry.Visible = false;
				trFromQuantity.Visible = trToQuantity.Visible =trConfigValue.Visible= true;
				lblFrom.Text = "SL Từ";
				lblTo.Text = "SL Đến";
				lblCurrency.Text = "VNĐ";
			}

			if (ddlOrderType.SelectedValue.Equals(Const_BusinessCode.Business_402)) // phí ship vận chuyển
			{
				trCustomerType.Visible = trCategory.Visible = trApplyService.Visible = trWebsite.Visible = trWebsiteGroup.Visible = trCountry.Visible = false;
				trFromQuantity.Visible = trToQuantity.Visible = trConfigValue.Visible = true;
				lblFrom.Text = "Từ(Kg)";
				lblTo.Text = "Đến(Kg)";
				lblCurrency.Text = "VNĐ";
			}

            if (string.IsNullOrEmpty(ddlOrderType.SelectedValue))//Tất cả
            {
                trCustomerType.Visible = trCategory.Visible = trApplyService.Visible
                    = trWebsite.Visible = trWebsiteGroup.Visible = trCountry.Visible
                        = trConfigValue.Visible = false;
            }
            else
            {
                trConfigValue.Visible = true;
            }
        }

        protected void ddlWebsiteGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebsiteGroup.SelectedValue.Equals(""))
            {
                ddlWebsite.Items.Clear();
                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
            }
            else
            {
                int websiteParentId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue);
                var websiteList = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == websiteParentId);
                ddlWebsite.DataSource = websiteList;
                ddlWebsite.DataTextField = "WebsiteName";
                ddlWebsite.DataValueField = "WebsiteId";
                ddlWebsite.DataBind();
                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
            }
        }

        #endregion

        #region // Method

        private void LoadData()
        {
            var parentWebsite = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == null);
            ddlWebsiteGroup.DataSource = parentWebsite;
            ddlWebsiteGroup.DataTextField = "WebsiteName";
            ddlWebsiteGroup.DataValueField = "WebsiteId";
            ddlWebsiteGroup.DataBind();
            ddlWebsiteGroup.Items.Insert(0, new ListItem("-- Chọn nhóm website --", ""));
            ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));


            ddlOrg.DataSource = _commonService.CountryGet("", "", "", this);
            ddlOrg.DataTextField = "CountryName";
            ddlOrg.DataValueField = "CountryId";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new ListItem("-- Chọn xuất xứ --", ""));


            ddlCustomerType.DataSource = _customerService.CustomerTypeGet("", "", this);
            ddlCustomerType.DataTextField = "CustomerTypeName";
            ddlCustomerType.DataValueField = "CustomerTypeId";
            ddlCustomerType.DataBind();
            ddlCustomerType.Items.Insert(0, new ListItem("-- Chọn đối tượng khách hàng --", ""));


            ddlCategory.DataSource = this._productService.CategoryGet("", "", "-1", this);
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- Chọn chủng loại sản phẩm --", ""));
        }

        private bool ValidData(string orderType, string websiteId, string countryId, string applyService, string customerType, string category, string configValue)
        {
            if (string.IsNullOrEmpty(ddlOrderType.SelectedValue))
            {
                lblError.Text = "Bạn chưa chọn chính sách!";
                lblError.Visible = true;
                return false;
            }

            if (orderType.Equals("101"))
            {
                if (string.IsNullOrEmpty(websiteId))
                {
                    lblError.Text = "Bạn chưa chọn website!";
                    lblError.Visible = true;
                    return false;
                }
                if (string.IsNullOrEmpty(customerType))
                {
                    lblError.Text = "Bạn chưa chọn đối tượng khách hàng!";
                    lblError.Visible = true;
                    return false;
                }
            }

            if (orderType.Equals("ORGRATE"))
            {
                if (string.IsNullOrEmpty(countryId))
                {
                    lblError.Text = "Bạn chưa chọn xuất xứ!";
                    lblError.Visible = true;
                    return false;
                }
            }

            if (orderType.Equals("FEE"))
            {
                if (string.IsNullOrEmpty(applyService))
                {
                    lblError.Text = "Bạn chưa chọn dịch vụ áp dụng!";
                    lblError.Visible = true;
                    return false;
                }

                if (string.IsNullOrEmpty(customerType))
                {
                    lblError.Text = "Bạn chưa chọn đối tượng khách hàng!";
                    lblError.Visible = true;
                    return false;
                }

                if (string.IsNullOrEmpty(category))
                {
                    lblError.Text = "Bạn chưa chọn chủng loại sản phẩm!";
                    lblError.Visible = true;
                    return false;
                }

            }

			if (orderType.Equals(Const_BusinessCode.Business_401)) // phí
			{
				if (string.IsNullOrEmpty(txtFromQuantity.Text))
				{
					lblError.Text = "Bạn chưa nhập SL từ!";
					lblError.Visible = true;
					return false;
				}

				if (string.IsNullOrEmpty(txtToQuantity.Text))
				{
					lblError.Text = "Bạn chưa nhập SL đến!";
					lblError.Visible = true;
					return false;
				}
			}

            if (string.IsNullOrEmpty(configValue))
            {
                lblError.Text = "Bạn chưa nhập giá trị!";
                lblError.Visible = true;
                return false;
            }
            return true;
        }

        #endregion
    }
}