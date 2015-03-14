using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz;

namespace Ecms.Website.Site.MBGN
{
    public partial class EditProfile : System.Web.UI.Page
    {
        #region //Declares

        private readonly CustomerService _customerService = new CustomerService();
        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitData();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Customer"] != null)
                {
                    var customer = (UserCustomerModel)Session["Customer"];
                    var customerReturn = _customerService.CustomerUpdate(customer.CustomerId + ""
																		 , txtFullName.Text
																		 , txtMobile.Text
																		 , txtEmail.Text
																		 , txtAddress.Text
																		 , ddlProvince.SelectedValue
																		 , ""
																		 , txtDeliveryName.Text
																		 , txtDeliveryMobile.Text
																		 , txtDeliveryEmail.Text
																		 , txtDeliveryAddress.Text
																		 , ""
																		 , ""
																		 , this);
                    if (customerReturn != null)
                    {
                        var userCustomer = _customerService.CustomerList("", "", "", "","", "", "", customerReturn.UserCode, "", "", "", this).FirstOrDefault();
                        if (userCustomer != null)
                        {
                            Session["Customer"] = userCustomer;
                            mtvMain.ActiveViewIndex = 1;
                        }
                    }
                }
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
            Response.Redirect("~/site/default.aspx");
        }

        #endregion

        #region //Private Methods

        private void InitData()
        {
            try
            {
                if (Session["Customer"] == null)
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx");

                ddlProvince.DataSource = _commonService.ProvinceList("", "", "");
                ddlProvince.DataTextField = "CityName";
                ddlProvince.DataValueField = "CityId";
                ddlProvince.DataBind();
                ddlProvince.Items.Insert(0, new ListItem("-- Tỉnh/ Thành phố", ""));

                if (Session["Customer"] != null)
                {
                    var customer = (UserCustomerModel)Session["Customer"];
                    txtFullName.Text = customer.CustomerName;
                    txtAddress.Text = customer.Address;
                    txtEmail.Text = customer.Email;
                    txtMobile.Text = customer.Mobile;
                    ddlProvince.SelectedValue = (customer.CityId ?? 0) + "";
                    txtDeliveryName.Text = customer.DeliveryName;
                    txtDeliveryMobile.Text = customer.DeliveryMobile;
                    txtDeliveryEmail.Text = customer.DeliveryEmail;
                    txtDeliveryAddress.Text = customer.DeliveryAddress;
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