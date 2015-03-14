using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CommonUtils;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using System.Text;

namespace Ecms.Website.DBServices
{
    public class CustomerService : BaseService
    {
        #region // Constructs

        public CustomerService()
            : base()
        {

        }
        #endregion

        #region // Customer

        public Customer CustomerRegister(string userName, string password, string fullName, string mobile, string email, string address, string province, Page page)
        {
            try
            {
                string alParamsOutError = "";

                var returnCustomer = this._customerBiz.CustomerCreate(
										userName
										, Utilities.Encrypt(password)
										, fullName
										, mobile
										, email
										, address
										, province
										, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }

                //Send mail thông báo đăng ký thành công
                if (returnCustomer != null)
                {
                    var strBuilder = new StringBuilder();
                    strBuilder.Append("Xin chào Quý khách!<br/>");
                    strBuilder.Append("<br />Chúc mừng Quý khách đã đăng ký thành công tài khoản trên <a href='http://QuangChau247.vn'>quangchau247.vn</a><br />.");
                    strBuilder.Append("<br />Thông tin tài khoản của Quý khách là:<br />");
                    strBuilder.Append("Tên đăng nhập: {0}<br />");
                    strBuilder.Append("Mật khẩu: {1}<br />");
					strBuilder.Append("Với tài khoản này, Quý khách có thể sử dụng các dịch vụ trên <a href='http://QuangChau247.vn'>quangchau247.vn</a><br /><br />");
                    strBuilder.Append("<br />Cảm ơn Quý khách đã sử dụng dịch vụ!<br />");
                    var strContent = string.Format(strBuilder.ToString(), userName.ToLower(), password);
                    CommonUtils.SendmailHelper.SendInfo(strContent, "Đăng ký tài khoản - QuangChau247.vn", returnCustomer.Email);
                }
				return returnCustomer;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
				return null;
            }
        }

        public List<UserCustomerModel> CustomerList(string customerId
            , string customerName
            , string customerCode
            , string email
            , string mobile
            , string address
            , string cityId
            , string userCode
            , string customerTypeId
            , string status
            , string flagAdmin
            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var customers = this._customerBiz.UserCustomerGet(
                    customerId
                    , customerName
                    , customerCode
                    , email
                    , mobile
                    , address
                    , cityId
                    , userCode
                    , customerTypeId
                    , status
                    , flagAdmin
                    , ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return customers;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<UserCustomerModel>();
            }
        }

        public UserCustomerModel Logon(string userName, string password, string type, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var sysUser = this._userBiz.GetUserSigin(userName, Utilities.Encrypt(password), type, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw new ServiceException();
                }

                if (sysUser != null)
                {
                    var customer = CustomerList("", "", "", "", "", "", "", sysUser.UserCode, "", "", "", page).FirstOrDefault();
                    return customer;
                }
                return null;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        public void ChangePassword(string userName, string currentPassword, string newPassword, Page page)
        {
            try
            {
                string alParamsOutError = "";
                _userBiz.UserChangedPassword(userName, currentPassword, newPassword, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
            }
        }

        public bool ResetPassword(string userCodeOrEmail, string type, Page page)
        {
            try
            {
                string alParamsOutError = "";
				string email = "";
				if (type == "0")
				{
					// Tài khoản KH
					var userCustomer = _customerBiz.UserCustomerGet("", "", "", userCodeOrEmail, "", "", "", "", "", "", "", ref alParamsOutError).SingleOrDefault();

					email = userCustomer == null ? "" : userCustomer.Email;
					NLogLogger.Info(string.Format("change pass for customer, email  = {0}", email));
				}
				else
				{ 
					// Tài khoản admin
					var user = _userBiz.GetUser(userCodeOrEmail, "", "", "1", "").SingleOrDefault();
					email = user == null ? "" : user.Email;
					NLogLogger.Info(string.Format("change pass for user admin, email  = {0}", email));
				}

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);

				if (!string.IsNullOrEmpty(email))
				{
					NLogLogger.Info(string.Format("reset pass with email = {0}", email));
					var user = _userBiz.UserResetPassword(userCodeOrEmail, type, ref alParamsOutError);

					if (!string.IsNullOrEmpty(alParamsOutError))
						throw GenServiceException(alParamsOutError);

					if (user != null)
					{
						NLogLogger.Info(string.Format("user != null: email = {0}", user.Email));
						//Send mail
						var strBuilder = new StringBuilder();
						strBuilder.Append("Xin chào Quý khách!<br />");
						strBuilder.Append("Quý khách đã phục hồi thành công mật khẩu tài khoản trên hệ thống <a href='http://muabangiaonhan.com'>muabangiaonhan.com</a><br />");
						strBuilder.Append("<br />Tên đăng nhập: {0}<br />Mật khẩu mới của Quý khách là: {1}<br/>");
						strBuilder.Append("<br/>Cảm ơn Quý khách đã sử dụng dịch vụ của chúng tôi!");

						var strContent = string.Format(strBuilder.ToString(), user.UserCode, Utilities.Decrypt(user.UserPassword));
						CommonUtils.SendmailHelper.SendInfo(strContent, "Cấp lại mật khẩu - MUABANGIAONHAN.COM", email);
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
				return false;
            }
        }

        public bool CustomerLockUnlock(string customerId
            , string isLock // lock=1; unlock=0
            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result;
                result = this._customerBiz.CustomerLockUnlock(customerId, isLock, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }

        public Customer CustomerUpdate(
            string customerId
            , string fullName
            , string mobile
            , string email
            , string address
            , string province
            , string customerTypeId
            , string deliveryName
            , string deliveryMobile
            , string deliveryEmail
            , string deliveryAddress
            , string customerCodeDelivery
			, string employeeCode
            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                Customer result;
                result = this._customerBiz.CustomerUpdate(
					customerId
					, fullName
					, mobile
					, email
					, address
					, province
					, customerTypeId
					, deliveryName
					, deliveryMobile
					, deliveryEmail
					, deliveryAddress
					, customerCodeDelivery
					, employeeCode
					, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new Customer(); ;
            }
        }

        public bool CustomerDelete(
            string customerId
            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result = false;
                result = this._customerBiz.CustomerDelete(customerId, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }

        #endregion

        #region // CustomerType

        public List<CustomerType> CustomerTypeGet(string customerTypeId, string customerTypeName, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var customerType = this._customerBiz.CustomerTypeGet(customerTypeId, customerTypeName, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return customerType;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<CustomerType>();
            }
        }

        //public List<CustomerType> CustomerTypeSearch(string customerTypeName, Page page)
        //{
        //    try
        //    {
        //        string alParamsOutError = "";
        //        var customerType = this._customerBiz.CustomerTypeGet("", customerTypeName, ref alParamsOutError);
        //        if (!string.IsNullOrEmpty(alParamsOutError))
        //            throw GenServiceException(alParamsOutError);
        //        return customerType;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.ShowExceptionBox(ex, page);
        //        return new List<CustomerType>();
        //    }
        //}

        public bool CustomerTypeDelete(string customerTypeId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result = false;
                result = this._customerBiz.CustomerTypeDelete(customerTypeId, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return true;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }

        public CustomerType CustomerTypeCreate(CustomerType customerType, Page page)
        {
            try
            {
                string alParamsOutError = "";
                CustomerType result;
                result = this._customerBiz.CustomerTypeCreate(customerType, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new CustomerType();
            }
        }

        public CustomerType CustomerTypeUpdate(CustomerType customerType, Page page)
        {
            try
            {
                string alParamsOutError = "";
                CustomerType result;
                result = this._customerBiz.CustomerTypeUpdate(customerType, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new CustomerType();
            }
        }


        #endregion

		#region // ConfigBusiness

		#region //ConfigBusinessGet
		public List<ConfigBusinessModel> ConfigBusinessGet(
            string configBusinessId
            , string customerTypeId
            , string orderTypeId
            , string countryId
            , string businessCode
            , string websiteId
            , string categoryId
            , string fromFromDate
			, string toFromDate
			, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var listConfigBussinessModel = _customerBiz.ConfigBusinessGet(configBusinessId
												, customerTypeId
												, orderTypeId
												, countryId
												, businessCode
												, websiteId
												, categoryId
												, fromFromDate
												, toFromDate
												, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return listConfigBussinessModel;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<ConfigBusinessModel>();
            }
        }

        #endregion

		#region // ConfigBusinessCreate
		public ConfigBusiness ConfigBusinessCreate(ConfigBusiness ctcb, Page page)
		{
			try
			{
				string alParamsOutError = "";

				var result = _customerBiz.ConfigBusinessCreate(ctcb, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new ConfigBusiness();
			}
		}
		#endregion

		#region // ConfigBusinessCreate
		public bool ConfigBusinessValueUpdate(string configId, string configValue, Page page)
		{
			try
			{
				string alParamsOutError = "";

				var result = _customerBiz.ConfigBusinessValueUpdate(configId, configValue, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}
		#endregion

		#endregion
	}
}