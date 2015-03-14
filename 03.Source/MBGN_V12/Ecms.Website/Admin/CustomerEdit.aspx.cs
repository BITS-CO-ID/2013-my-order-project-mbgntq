using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz;

namespace Ecms.Website.Admin
{
    public partial class CustomerEdit : PageBase
    {
        #region // Declaration

        private readonly CustomerService _customerService = new CustomerService();
        private readonly CommonService _commonService = new CommonService();
		private IUserBiz cService = new UserBiz();

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    loadComboBox();
                    loadEdit();
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {

                #region Check

                lblError.Text = "";
                if (ddCityId.SelectedItem.Value == "-1")
                {
                    lblError.Text = "Chưa chọn thành phố";
                    return;
                }

                if (ddCustomerTypeId.SelectedItem.Value == "-1")
                {
                    lblError.Text = "Chưa chọn nhóm khách hàng";
                    return;
                }

                #endregion

                if (Session["CustomerIdCode"] != null)
                {
                    string id = (string)Session["CustomerIdCode"];

                    Session["CustomerIdCode"] = null;
                    var result = this._customerService.CustomerUpdate(
						id
                        , txtCustomerName.Text
                        , txtMobile.Text
                        , txtEmail.Text
                        , txtAddress.Text
                        , ddCityId.SelectedItem.Value
                        , ddCustomerTypeId.SelectedItem.Value
                        , ""
						, ""
						, ""
						, ""
						, "" //txtCustomerCodeDelivery.Text
						, dllEmployee.SelectedValue
						, this);
                    if (result.CustomerId != 0)
                        DisplayAlert("Hệ thống cập nhật thành công", "CustomerList.aspx");
                }
                else
                {
                    DisplayAlert("Chưa chọn khách hàng", "CustomerList.aspx");
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerList.aspx");
        }


        #endregion

        #region // Method

        /// <summary>
        /// Load dữ liệu lên form
        /// </summary>
        private void loadEdit()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                btnSave.Text = "Lưu";
                lblFunction.Text = "Sửa";
                string id = (string)Session["lbtEdit_Click"];
                Session["CustomerIdCode"] = id;
                var customer = this._customerService.CustomerList(id, "", "", "", "", "", "", "", "", "", "", this);
                string cityId = String.IsNullOrEmpty(customer[0].CityId.ToString()) ? "-1" : customer[0].CityId.ToString();
                ddCityId.Items.FindByValue(cityId).Selected = true;
                if (!String.IsNullOrEmpty(customer[0].CustomerTypeId.ToString()))
                    ddCustomerTypeId.Items.FindByValue(customer[0].CustomerTypeId.ToString()).Selected = true;

                txtCustomerCode.Text = customer[0].CustomerCode;
                txtCustomerName.Text = customer[0].CustomerName;
                txtAddress.Text = customer[0].Address;
                txtEmail.Text = customer[0].Email;
                txtMobile.Text = customer[0].Mobile;
                //txtCustomerCodeDelivery.Text = customer[0].CustomerCodeDelivery;
                Session["lbtEdit_Click"] = null;
				if (!string.IsNullOrEmpty(customer[0].EmployeeCode))
				{
					dllEmployee.Text = customer[0].EmployeeCode;
				}
            }
            else
            {
                Response.Redirect("CustomerList.aspx");

            }
        }


        private void loadComboBox()
        {
            // Lấy Thành phố
            List<Province> province = new List<Province>();
            province.Add(new Province() { CityId = -1, CityName = "-- Chọn thành phố --" });
            province.AddRange(this._commonService.ProvinceList("", "", ""));
            ddCityId.DataSource = province;
            ddCityId.DataTextField = "CityName";
            ddCityId.DataValueField = "CityId";
            ddCityId.DataBind();


            // Lấy nhóm khách hàng
            List<CustomerType> customerType = new List<CustomerType>();
            customerType.Add(new CustomerType() { CustomerTypeId = -1, CustomerTypeName = "-- Chọn --" });
            customerType.AddRange(this._customerService.CustomerTypeGet("", "", this));
            ddCustomerTypeId.DataSource = customerType;
            ddCustomerTypeId.DataTextField = "CustomerTypeName";
            ddCustomerTypeId.DataValueField = "CustomerTypeId";
            ddCustomerTypeId.DataBind();

			// Lấy thông tin người bán hàng
			var list = cService.GetUser("", "", "", "1", "").Where(x => x.UserCode != "sysadmin" && x.FlagActive == "1" && x.FlagAdmin == "1").ToList();
			dllEmployee.DataSource = list;
			dllEmployee.DataTextField = "UserName";
			dllEmployee.DataValueField = "UserCode";
			dllEmployee.DataBind();
			dllEmployee.Items.Insert(0, new ListItem("-- Chọn --", ""));
			
        }

        protected virtual void DisplayAlert(string message, string page)
        {
            try
            {
                ClientScript.RegisterStartupScript(
                  this.GetType(),
                  Guid.NewGuid().ToString(),
                  string.Format("alert('{0}');window.location.href = '" + page + "'",
                    message.Replace("'", @"\'").Replace("\n", "\\n").Replace("\r", "\\r")),
                    true);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }


        #endregion
    }
}