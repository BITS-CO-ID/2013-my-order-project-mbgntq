 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Biz
{
    /*
     Biz xử lý các nghiệp vụ liên quan đến customer
     */
    public class CustomerBiz : ICustomerBiz
    {
        #region // UserCustomer

        public List<UserCustomerModel> UserCustomerGet(
                string customerId
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
                , ref string alParamsOutError)
        {
            #region // Temp
			string strFunctionName = "UserCustomerGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
                        "customerId",customerId,
                        "customerName",customerName,
                        "customerCode",customerCode,
                        "email",email,
						"mobile",mobile,
						"address",address,
						"cityId",cityId,
                        "userCode",userCode,
						"customerTypeId",customerTypeId,
						"status",status,
                        "flagAdmin",flagAdmin
			            });
            #endregion

            try
            {

                using (var dbContext = new EcmsEntities())
                {
					var customers = from u in dbContext.Sys_User
									join c in dbContext.Customers on u.UserCode equals c.UserCode into g1
									from c in g1.DefaultIfEmpty()
									join p in dbContext.Provinces on (c.CityId ?? 0) equals p.CityId into g3
									from p in g3.DefaultIfEmpty()
									join ep in dbContext.Sys_User on c.EmployeeCode equals ep.UserCode into epc
									from ep in epc.DefaultIfEmpty()
									select new UserCustomerModel()
											   {
												   CustomerId = c.CustomerId,
												   CustomerCode = c.CustomerCode,
												   CustomerName = c.CustomerName,
												   CustomerTypeId = c.CustomerTypeId,
												   CustomerTypeName = c.CustomerType.CustomerTypeName,
												   Address = c.Address,
												   Mobile = c.Mobile,
												   Email = c.Email,
												   Balance = c.Balance,
												   Status = c.Status,
												   CityId = c.CityId,
												   CityName = p.CityName,
												   ContactAccount = c.ContactAccount,
												   CreatedDate = c.CreatedDate,
												   LastDateModify = c.LastDateModify,
												   UserCode = u.UserCode,
												   Password = u.UserPassword,
												   UserName = u.UserName,
												   FlagAdmin = u.FlagAdmin,
												   DeliveryAddress = c.DeliveryAddress,
												   DeliveryEmail = c.DeliveryEmail,
												   DeliveryMobile = c.DeliveryMobile,
												   DeliveryName = c.DeliveryName,
                                                   CustomerCodeDelivery = c.CustomerCodeDelivery,
												   EmployeeCode=c.EmployeeCode,
												   EmployeeName=ep.UserName,
												   BalanceFreeze = ((from i in c.Invoices
																	 where (i.Order != null && (i.Order.OrderStatus == OrderInStatus.OrderConfirmed || i.Order.OrderStatus == OrderInStatus.Finished))
																			 && i.Status == 2
																			 && ((new string[] { Const_BusinessCode.Business_207, Const_BusinessCode.Business_208, Const_BusinessCode.Business_209 }).Contains(i.BusinessCode))
																	 select i).Sum(x => x.InvoiceDetails.Sum(y => y.Amount)) ?? 0),
											   };

                    if (!string.IsNullOrEmpty(customerId))
                    {
                        var cusId = Convert.ToInt32(customerId);
                        customers = customers.Where(x => x.CustomerId == cusId);
                    }

					if (!string.IsNullOrEmpty(customerTypeId))
					{
						var cusId = Convert.ToInt32(customerTypeId);
						customers = customers.Where(x => x.CustomerTypeId == cusId);
					}

					if (!string.IsNullOrEmpty(status))
					{
						var cusId = Convert.ToInt32(status);
						customers = customers.Where(x => x.Status == cusId);
					}

                    if (!string.IsNullOrEmpty(customerCode))
                    {
                        customers = customers.Where(x => x.CustomerCode == customerCode || x.UserCode ==customerCode);
                    }

                    if (!string.IsNullOrEmpty(customerName))
                    {
                        customers = customers.Where(x => x.CustomerName == customerName);
                    }

                    if (!string.IsNullOrEmpty(cityId))
                    {
                        var cusId = Convert.ToInt32(cityId);
                        customers = customers.Where(x => x.CityId == cusId);
                    }

                    if (!string.IsNullOrEmpty(mobile))
                    {
                        customers = customers.Where(x => x.Mobile.Contains(mobile));
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        customers = customers.Where(x => x.Email.Contains(email));
                    }

                    if (!string.IsNullOrEmpty(address))
                    {
                        customers = customers.Where(x => x.Address.Contains(address));
                    }

                    if (!string.IsNullOrEmpty(userCode))
                    {
                        customers = customers.Where(x => x.UserCode == userCode);
                    }

                    if (!string.IsNullOrEmpty(flagAdmin))
                    {
                        customers = customers.Where(x => x.FlagAdmin == flagAdmin);
                    }

                    return customers.OrderBy(x => x.CustomerName).ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<UserCustomerModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // Customer


        #region // CustomerCreate
        public Customer CustomerCreate(
            string userName
            , string password
            , string fullName
            , string mobile
            , string email
            , string address
            , string province
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CustomerCreate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "userName", userName
						, "password", password
						, "fullName", fullName
						, "mobile", mobile
						, "email", email
						, "address", address
						, "province", province
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:
                    // tạo mã kh, check mã kh
                    var newestCustomer = dbContext.Customers.OrderByDescending(x => x.CustomerCode).FirstOrDefault();
                    string newCusCode = newestCustomer == null ? "200300400" : Convert.ToString(Convert.ToInt32(newestCustomer.CustomerCode) + 1);

                    if (dbContext.Customers.Any(x => x.CustomerCode == newCusCode))
                    {
                        throw new Exception("Không được phép tạo trùng mã khách hàng");
                    }
                    // Check UserName
                    if (!string.IsNullOrEmpty(userName))
                    {
                        if (dbContext.Sys_User.Any(x => x.UserCode.Trim().ToUpper() == userName.Trim().ToUpper()))
                        {
                            throw new Exception("UserCode đã được đăng ký trong hệ thống, chọn user khác!");
                        }
                    }

                    // check email trung nhau:
                    if (!string.IsNullOrEmpty(email))
                    {
                        if (dbContext.Customers.Any(x => x.Email == email))
                        {
                            throw new Exception("Email đã được đăng ký trong hệ thống, chọn email khác!");
                        }
                    }
                    #endregion

                    #region // Build:
                    var sysUser = new Sys_User();
                    sysUser.UserCode = userName.ToLower();
                    sysUser.UserName = fullName;
                    sysUser.UserPassword = password;
                    sysUser.Email = email;
                    sysUser.FlagAdmin = "0";
                    sysUser.FlagActive = "1";

                    var customer = new Customer();
                    customer.CustomerName = fullName;
                    customer.CustomerCode = newCusCode;
                    customer.Mobile = mobile;
                    customer.Email = email;
                    customer.Address = address;
					customer.UserCode = userName.ToLower(); 
                    customer.Balance = 0;
                    customer.CreatedDate = DateTime.Now;
					if (!string.IsNullOrEmpty(province))
					{
						customer.CityId = Convert.ToInt32(province);
					}
                    customer.CustomerTypeId = 1;
                    customer.Status = 0;
                    customer.CustomerCodeDelivery = "GIA MINH " + userName.ToUpper();
					
                    dbContext.Sys_User.Add(sysUser);
                    dbContext.Customers.Add(customer);
                    dbContext.SaveChanges();
                    #endregion

                    #region // return:
                    return customer;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return null;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // CustomerUpdate
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
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CustomerUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "customerId", customerId
						, "fullName", fullName
						, "mobile", mobile
						, "email", email
						, "address", address
						, "province", province
						, "customerTypeId", customerTypeId
						, "deliveryName", deliveryName
						, "deliveryMobile", deliveryMobile
						, "deliveryEmail", deliveryEmail
						, "deliveryAddress", deliveryAddress
						, "customerCodeDelivery", customerCodeDelivery
						, "employeeCode", employeeCode
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:
                    // tạo mã kh, check mã kh
                    int id = Convert.ToInt32(customerId);
                    var updateCustomer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == id);

                    if (updateCustomer == null)
                    {
                        throw new Exception("Không có thông tin khách hàng");
                    }

                    #endregion

                    #region // Build:

                    updateCustomer.CustomerName = fullName;
                    updateCustomer.Mobile = mobile;
                    updateCustomer.Email = email;
                    updateCustomer.Address = address;
                    updateCustomer.LastDateModify = DateTime.Now;

					if (!string.IsNullOrEmpty(province))
					{
						updateCustomer.CityId = Convert.ToInt32(province);
					}

					if (!string.IsNullOrEmpty(customerTypeId))
					{
						updateCustomer.CustomerTypeId = Convert.ToInt32(customerTypeId);	
					}

                    if (!string.IsNullOrEmpty(deliveryName))
                    {
                        updateCustomer.DeliveryName = deliveryName;
                    }

                    if (!string.IsNullOrEmpty(deliveryMobile))
                    {
                        updateCustomer.DeliveryMobile = deliveryMobile;
                    }

                    if (!string.IsNullOrEmpty(deliveryEmail))
                    {
                        updateCustomer.DeliveryEmail = deliveryEmail;
                    }

                    if (!string.IsNullOrEmpty(deliveryAddress))
                    {
                        updateCustomer.DeliveryAddress = deliveryAddress;
                    }

                    if (!string.IsNullOrEmpty(customerCodeDelivery))
                    {
                        updateCustomer.CustomerCodeDelivery = customerCodeDelivery;
                    }

					if (!string.IsNullOrEmpty(employeeCode))
					{
						updateCustomer.EmployeeCode = employeeCode;
					}

                    dbContext.SaveChanges();
                    #endregion

                    #region // return:
                    return updateCustomer;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return null;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // CustomerDelete
        public bool CustomerDelete(
            string customerId
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CustomerDelete";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "customerId", customerId
			            });
            #endregion

            try
            {
                using (var ts = new System.Transactions.TransactionScope())
                {
                    using (var dbContext = new EcmsEntities())
                    {
                        #region // Check:
                        int id = Convert.ToInt32(customerId);
                        var delCustomer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == id);

                        var delUser = dbContext.Sys_User.SingleOrDefault(x => x.UserCode == delCustomer.UserCode);
                        #endregion

                        #region // Build:
                        // delete customer:
                        dbContext.Customers.Remove(delCustomer);

                        // delete user:
                        dbContext.Sys_User.Remove(delUser);
                        #endregion

                        #region // return:
                        ts.Complete();
                        return true;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return false;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

		#region // CustomerLockUnlock
		public bool CustomerLockUnlock(string customerId
			, string isLock // lock=1; unlock=0
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "CustomerLockUnlock";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "customerId", customerId
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					#region // Check:
					// tạo mã kh, check mã kh
					int id = Convert.ToInt32(customerId);
					var updateCustomer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == id);

					if (updateCustomer == null)
					{
						throw new Exception("Không có thông tin khách hàng");
					}

					if (isLock.Equals("0") && updateCustomer.Status==0)
					{
						throw new Exception("Khách hàng đang mở, không thể unlock");
					}

					if (isLock.Equals("1") && updateCustomer.Status == 1)
					{
						throw new Exception("Khách hàng đang khóa, không thể lock");
					}

					#endregion

					#region // Build:

					updateCustomer.LastDateModify = DateTime.Now;
					updateCustomer.Status = Convert.ToInt32(isLock);
					dbContext.SaveChanges();
					#endregion

					#region // return:
					return true;
					#endregion
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return false;
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion
        #endregion

        #region // CustomerType

        #region //CustomerTypeGet
        public List<CustomerType> CustomerTypeGet(string customerTypeId, string customerTypeName, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "GetCustomerType";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "customerTypeId",customerTypeId
						, "customerTypeName",customerTypeName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.CustomerTypes
                                 select p;


                    if (!string.IsNullOrEmpty(customerTypeId))
                    {
                        int iFilter = Convert.ToInt32(customerTypeId);
                        result = result.Where(p => p.CustomerTypeId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(customerTypeName))
                        result = result.Where(p => p.CustomerTypeName.Contains(customerTypeName));

                    var lstResult = new List<CustomerType>();
                    foreach (var item in result)
                    {
                        var customerType = new CustomerType
                        {
                            //Balance = ((from t in _db.BudgetBalances
                            //            where t.CompanyId == item.CompanyId
                            //            select new { t.Balance }).Sum(p => p.Balance) ?? 0)
                            //,
                            CustomerTypeId = item.CustomerTypeId,
                            CustomerTypeName = item.CustomerTypeName

                        };
                        lstResult.Add(customerType);
                    }
                    return lstResult.ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<CustomerType>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //CustomerTypeDelete
        public bool CustomerTypeDelete(string customerTypeId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "DeleteCustomerType";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "customerTypeId",customerTypeId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    int iFilter = Convert.ToInt32(customerTypeId);
                    // Check in Department
                    var query = _db.Customers.Where(p => p.CustomerTypeId == iFilter);
                    if (query != null && query.Count() > 0)
                    {
                        throw new Exception("Đối tượng khách hàng được sử dụng, bạn không thể xóa.");
                    }

                    #endregion

                    #region // Delete
                    CustomerType customerType = new CustomerType() { CustomerTypeId = iFilter };
                    _db.CustomerTypes.Attach(customerType);
                    _db.CustomerTypes.Remove(customerType);
                    _db.SaveChanges();
                    #endregion

                    return true;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return false;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //CustomerTypeCreate
        public CustomerType CustomerTypeCreate(CustomerType customerType, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "AddCustomerType";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.CustomerTypes.Add(customerType);
                    _db.SaveChanges();
                    return customerType;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new CustomerType();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //CustomerTypeUpdate
        public CustomerType CustomerTypeUpdate(CustomerType customerType, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "UpdateCustomerType";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    CustomerType cusType = _db.CustomerTypes.Where(p => p.CustomerTypeId == customerType.CustomerTypeId).SingleOrDefault();

                    cusType.CustomerTypeId = customerType.CustomerTypeId;
                    cusType.CustomerTypeName = customerType.CustomerTypeName;
                    _db.SaveChanges();
                    return customerType;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new CustomerType();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #endregion

		#region // ConfigBusinessGet

		#region // ConfigBusinessGet
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
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ConfigBusinessGet";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "configBusinessId",configBusinessId
						, "customerTypeId",customerTypeId
						, "orderTypeId",orderTypeId
						, "countryId",countryId
						, "businessCode",businessCode
						, "websiteId",websiteId
						, "categoryId",categoryId
						, "fromFromDate",fromFromDate
						, "toFromDate",toFromDate
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					var result = from p in _db.ConfigBusinesses
								 join c in _db.Categories on p.CategoryId equals c.CategoryId into c_join
								 from c in c_join.DefaultIfEmpty()
								 join w in _db.WebsiteLinks on p.WebsiteId equals w.WebsiteId into w_join
								 from w in w_join.DefaultIfEmpty()
								 select new ConfigBusinessModel
								 {
										BusinessCode=p.BusinessCode,
										BusinessName = p.Mst_Business.BusinessName,
										ConfigBusinessId = p.ConfigBusinessId,
										ConfigValue = p.ConfigValue,
										CreatedDate = p.CreatedDate,
										CustomerTypeId = p.CustomerTypeId,
										CustomerTypeName = p.CustomerType.CustomerTypeName,
										FromDate = p.FromDate,
										OrderTypeId = p.OrderTypeId,
										OrderTypeName = p.OrderType.OrderTypeName,
										CountryId=p.CountryId,
										CountryName=p.Country.CountryName,
										Remark = p.Remark,                         
										WebsiteId = p.WebsiteId,
										CategoryId = p.CategoryId,
										CategoryName=c.CategoryName,
										WebsiteName=w.WebsiteName,
										fromQuantity=p.fromQuantity,
										toQuantity=p.toQuantity,
										Unit=p.Unit
								 };

					if (!string.IsNullOrEmpty(configBusinessId))
					{
						int iFilter = Convert.ToInt32(configBusinessId);
						result = result.Where(p => p.ConfigBusinessId == iFilter);
					}

					if (!string.IsNullOrEmpty(customerTypeId))
					{
						int iFilter = Convert.ToInt32(customerTypeId);
						result = result.Where(p => p.CustomerTypeId == iFilter);
					}

					if (!string.IsNullOrEmpty(orderTypeId))
					{
						int iFilter = Convert.ToInt32(orderTypeId);
						result = result.Where(p => p.OrderTypeId == iFilter);
					}

					if (!string.IsNullOrEmpty(businessCode))
					{
						result = result.Where(p => p.BusinessCode==businessCode);
					}
					if (!string.IsNullOrEmpty(countryId))
					{
						int iFilter = Convert.ToInt32(countryId);
						result = result.Where(p => p.CountryId == iFilter);
					}

                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        int iFilter = Convert.ToInt32(websiteId);
						result = result.Where(p => p.WebsiteId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(categoryId))
                    {
                        int iFilter = Convert.ToInt32(categoryId);
                        result = result.Where(p => p.CategoryId == iFilter);
                    }

					if (!string.IsNullOrEmpty(fromFromDate))
					{
						var dTime = Convert.ToDateTime(fromFromDate);
						result = result.Where(p => p.FromDate >= dTime);
					}

					if (!string.IsNullOrEmpty(toFromDate))
					{
						var dTime = Convert.ToDateTime(toFromDate);
						result = result.Where(p => p.FromDate <= dTime);
					}

					return result.OrderByDescending(x=>x.CreatedDate).ToList();
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<ConfigBusinessModel>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

		#region // ConfigBusinessCreate
		public ConfigBusiness ConfigBusinessCreate(ConfigBusiness ctcb, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "CustomerTypeConfigBusinessCreate";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion
			try
			{
				using (var _db = new EcmsEntities())
				{
					_db.ConfigBusinesses.Add(ctcb);
					_db.SaveChanges();
					return ctcb;
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new ConfigBusiness();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

		#region // ConfigBusinessValueUpdate
		public bool ConfigBusinessValueUpdate(
				string configId
				, string configValue 
				, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ConfigBusinessValueUpdate";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "configId", configId
						, "configValue", configValue
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					#region // Check:
					
					int id = Convert.ToInt32(configId);
					var config = dbContext.ConfigBusinesses.SingleOrDefault(x => x.ConfigBusinessId == id);

					if (config == null)
					{
						throw new Exception("Không có thông tin");
					}

					#endregion

					#region // Build:
					config.ConfigValue = Convert.ToDouble(configValue);
					dbContext.SaveChanges();
					#endregion

					#region // return:
					return true;
					#endregion
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return false;
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion
		#endregion
	}

    #region // model
    public class UserCustomerModel
    {
        public int? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public double? Balance { get; set; }
        public string Mobile { get; set; }
        public int? Status { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string ContactAccount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastDateModify { get; set; }

        public string UserCode { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FlagAdmin { get; set; }

		public string DeliveryName { set; get; }
		public string DeliveryMobile { set; get; }
		public string DeliveryEmail { set; get; }
		public string DeliveryAddress { set; get; }
        public string CustomerCodeDelivery { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }

		public double? BalanceFreeze { get; set; }
		private double? balanceAvaiable { get; set; }
		public double BalanceAvaiable
		{
			get
			{
				return ((Balance ?? 0) - (BalanceFreeze ?? 0));
			}
			set
			{
				balanceAvaiable = value;
			}
		}
		
    }

    public class CustomerModel
    {
        public int? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public double? Balance { get; set; }
        public string Mobile { get; set; }
        public int? Status { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string ContactAccount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastDateModify { get; set; }
        public string CustomerCodeDelivery { get; set; }
    }

	public class ConfigBusinessModel
	{
		public int ConfigBusinessId { get; set; }
		public int? CustomerTypeId { get; set; }
		public int? OrderTypeId { get; set; }
		public int? CountryId { get; set; }
		public int? Type { get; set; }
		public string CountryName { get; set; }
		public string BusinessCode { get; set; }
		public string BusinessName { get; set; }
		public string OrderTypeName { get; set; }
		public string CustomerTypeName { get; set; }
		public string Remark { get; set; }
		public double? ConfigValue { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? CreatedDate { get; set; }
        public int? WebsiteId { get; set; }
		public string WebsiteName { get; set; }
        public int? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public double? fromQuantity { get; set; }
		public double? toQuantity { get; set; }
		public string Unit { get; set; }
	}
    #endregion
}
