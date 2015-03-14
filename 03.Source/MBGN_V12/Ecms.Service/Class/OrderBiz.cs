using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz;
using Ecms.Biz.Entities;
using System.Collections;
using CommonUtils;
using System.Transactions;
using System.Globalization;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data;
using Ecms.Biz.Class;


namespace Ecms.Biz
{
    public class OrderBiz : IOrderBiz
    {
        #region // Order

        #region // OrderGet
        public List<OrderModel> OrderGet(
                string orderId
                , string orderNo
                , string orderOutboundNo
                , string trackingNo
                , string orderDateFrom
                , string orderDateTo
                , string customerId
                , string customerCode
                , string userCode
                , string customerCodeDelivery
                , string orderNoDelivery
                , string deliveryDateFrom
                , string deliveryDateTo
                , string orderStatus
                , string orderTypeId
                , string employeeCode
				, string createUser
				, string isAuthenticateUser
                , string isGetDetail
                , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
                        "orderId",orderId,
                        "orderNo",orderNo,
                        "orderOutboundNo",orderOutboundNo,
                        "trackingNo",trackingNo,
                        "orderDateFrom",orderDateFrom,
                        "orderDateTo",orderDateTo,
						"customerId",customerId,
						"customerCode",customerCode,
						"customerCodeDelivery",customerCodeDelivery,
						"orderNoDelivery",orderNoDelivery,						
						"deliveryDateFrom",deliveryDateFrom,
						"deliveryDateTo",deliveryDateTo,
						"orderStatus",orderStatus,
						"orderTypeId",orderTypeId,
						"employeeCode",employeeCode,
						"createUser",createUser,
						"isAuthenticateUser",isAuthenticateUser,
						"isGetDetail",isGetDetail
			            });
            #endregion

            try
            {

                using (var db = new EcmsEntities())
                {
                    #region // Query:
                    var query = from o in db.Orders
                                join c in db.Customers on o.CustomerId equals c.CustomerId into c_join
                                from c in c_join.DefaultIfEmpty()
                                join u in db.Sys_User on c.UserCode equals u.UserCode
                                join ep in db.Sys_User on o.EmployeeCode equals ep.UserCode into epc
                                from ep in epc.DefaultIfEmpty()
								where c.Status == 0 && o.OrderStatus != OrderInStatus.OrderDeleted
                                select new OrderModel()
                                {
                                    OrderId = o.OrderId,
                                    OrderNo = o.OrderNo,
                                    OrderOutboundNo = o.OrderOutboundNo,
                                    TrackingNo = o.TrackingNo,
                                    CreatedDate = o.CreatedDate,
                                    OrderDate = o.OrderDate,
                                    UserCode = u.UserCode,
                                    CustomerId = o.CustomerId,
                                    CustomerCode = c.CustomerCode,
                                    CustomerName = c.CustomerName,
                                    CustomerTypeId = c.CustomerTypeId,
                                    CustomerTypeName = c.CustomerType.CustomerTypeName,
                                    Mobile = c.Mobile,
                                    Address = c.Address,
                                    Email = c.Email,
                                    ContactChannel = o.ContactChannel,
                                    DeliveryName = o.DeliveryName,
                                    DeliveryMobile = o.DeliveryMobile,
                                    DeliveryEmail = o.DeliveryEmail,
                                    DeliveryAddress = o.DeliveryAddress,
                                    DeliveryDate = o.DeliveryDate,
                                    CustomerCodeDelivery = c.CustomerCodeDelivery,
                                    OrderNoDelivery = o.OrderNoDelivery,
                                    OrderStatus = o.OrderStatus,
                                    Remark = o.Remark,
                                    OrderTypeId = o.OrderTypeId,
                                    OrderTypeName = o.OrderType.OrderTypeName,
                                    IsInsurance = o.IsInsurance,
                                    NeedDate = o.NeedDate,
                                    DateToUsa = o.DateToUsa,
									CreateUser=o.CreateUser,
                                    EmployeeCode = o.EmployeeCode,
                                    EmployeeName = ep.UserName,
									ConfirmDate=o.ConfirmDate,
									DayAllowedDelay=o.DayAllowedDelay,
									CalFeeDelay=o.CalFeeDelay,
									IsCalFeeDelay = o.IsCalFeeDelay,
									AmountCalFeeDelay=o.AmountCalFeeDelay??0,
									AmountFeeDelay=o.AmountFeeDelay??0,
									FeeDelay = o.FeeDelay,
                                    // Tỷ giá vận chuyển
                                    ConfigRateId = o.ConfigRateId,
                                    ConfigRateValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == o.ConfigRateId).ConfigValue),

                                    lstOrderDetailModel = (from p in db.OrderDetails
                                                           join pr in db.Products on (p.ProductId ?? 0) equals pr.ProductId into pr_join
                                                           from pr in pr_join.DefaultIfEmpty()
                                                           join ci in db.Countries on p.CountryId equals ci.CountryId into ci_join
                                                           from ci in ci_join.DefaultIfEmpty()
                                                           join cc in db.Categories on p.CategoryId equals cc.CategoryId into cc_join
                                                           from cc in cc_join.DefaultIfEmpty()
                                                           join pc in db.Categories on cc.ParentId equals pc.CategoryId into pc_join
                                                           from pc in pc_join.DefaultIfEmpty()
                                                           where p.OrderId == o.OrderId
                                                           select new OrderDetailModel
                                                           {
															   DetailCode=p.DetailCode,
                                                               Color = p.Color,
                                                               EffortConfigId = p.EffortConfigId,
                                                               EffortConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.EffortConfigId).ConfigValue),
                                                               EffortModified = p.EffortModified,
                                                               ImageUrl = p.ImageUrl,
                                                               OrderDetailId = p.OrderDetailId,
                                                               OrderId = p.OrderId,
                                                               PriceWeb = p.PriceWeb,
                                                               PriceWebOff = p.PriceWebOff,
                                                               ProductId = p.ProductId,
                                                               ProductCode = pr == null ? p.ProductCode : pr.ProductCode,
                                                               ProductName = pr == null ? p.ProductName : pr.ProductName,
                                                               ProductLink = p.ProductLink,
                                                               Quantity = p.Quantity,
                                                               ShipConfigId = p.ShipConfigId,
                                                               ShipConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.ShipConfigId).ConfigValue),
                                                               ShipModified = p.ShipModified,
                                                               DeclarePrice = p.DeclarePrice,
                                                               ShipUSAVN = p.ShipUSAVN,
                                                               ShipUSA = p.ShipUSA,
                                                               Size = p.Size,
                                                               Surcharge = p.Surcharge,
                                                               TaxUsaConfigId = p.TaxUsaConfigId,
                                                               TaxUsaConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.TaxUsaConfigId).ConfigValue),

                                                               InsuaranceConfigId = p.InsuaranceConfigId,
                                                               InsuaranceConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.InsuaranceConfigId).ConfigValue),
                                                               WebsiteId = p.WebsiteId,
                                                               WebsiteName = p.WebsiteLink.WebsiteName,
															   ParentWebsiteId =p.WebsiteLink.ParentId,
															   ParentWebsiteName = p.WebsiteLink.WebsiteLink2.WebsiteName,
                                                               Weight = p.Weight,
															   DetailStatus = p.Order.OrderTypeId == 3 ? (p.DetailStatus == null ? 0 : p.DetailStatus) : p.DetailStatus,
                                                               CountryId = p.CountryId,
                                                               CountryName = ci.CountryName,
                                                               CurrencyCode = ci.CurrencyCode,
                                                               OrderTypeId = o.OrderTypeId.Value,
                                                               CategoryId = p.CategoryId,
                                                               CategoryName = cc.CategoryName,
                                                               ParentCategoryId = cc.ParentId,
                                                               ParentCategoryName = pc.CategoryName,
                                                               TrackingNo = p.TrackingNo,
                                                               OrderNoDelivery = p.OrderNoDelivery,
                                                               DateToUsa = p.DateToUsa,
                                                               UserCode = u.UserCode,
                                                               CustomerName = c.CustomerName,
                                                               DeliveryVNDate = p.DeliveryVNDate,
                                                               DeliveryDate = p.DeliveryDate,
															   LotPrice=p.LotPrice,
															   RateCountryId=p.RateCountryId,
															   RateCountryValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.RateCountryId).ConfigValue??0),
															   RateUSDValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.Order.ConfigRateId).ConfigValue??0),
															   OrderStatus = o.OrderStatus,
															   Shop=p.Shop,
															   Remark=p.Remark
                                                           }
                                )
                                };

                    #endregion

                    #region // Filter:
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        var id = Convert.ToInt32(orderId);
                        query = query.Where(x => x.OrderId == id);
                    }

                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        query = query.Where(x => x.OrderNo == orderNo);
                    }

                    if (!string.IsNullOrEmpty(orderOutboundNo))
                    {
                        query = query.Where(x => x.OrderOutboundNo == orderOutboundNo);
                    }

                    if (!string.IsNullOrEmpty(trackingNo))
                    {
                        query = query.Where(x => x.TrackingNo == trackingNo);
                    }

                    if (!string.IsNullOrEmpty(orderDateFrom))
                    {
                        var dateFilter = Convert.ToDateTime(orderDateFrom);
                        query = query.Where(x => x.CreatedDate >= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(orderDateTo))
                    {
                        var dateFilter = Convert.ToDateTime(orderDateTo);
                        query = query.Where(x => x.CreatedDate <= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(deliveryDateFrom))
                    {
                        var dateFilter = Convert.ToDateTime(deliveryDateFrom);
                        query = query.Where(x => x.DeliveryDate >= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(deliveryDateTo))
                    {
                        var dateFilter = Convert.ToDateTime(deliveryDateTo);
                        query = query.Where(x => x.DeliveryDate <= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(customerId))
                    {
                        var id = Convert.ToInt32(customerId);
                        query = query.Where(x => x.CustomerId == id);
                    }

                    if (!string.IsNullOrEmpty(customerCode))
                    {
						query = query.Where(x => x.CustomerCode == customerCode || x.UserCode.ToLower() == customerCode.ToLower());
                    }

					if (!string.IsNullOrEmpty(userCode))
					{
						query = query.Where(x => x.UserCode == userCode);
					}

                    if (!string.IsNullOrEmpty(customerCodeDelivery))
                    {
                        query = query.Where(x => x.CustomerCodeDelivery == customerCodeDelivery);
                    }

                    if (!string.IsNullOrEmpty(orderNoDelivery))
                    {
                        query = query.Where(x => x.OrderNoDelivery == orderNoDelivery);
                    }

                    if (!string.IsNullOrEmpty(orderStatus))
                    {
                        var id = Convert.ToInt32(orderStatus);
                        query = query.Where(x => x.OrderStatus == id);
                    }

                    if (!string.IsNullOrEmpty(orderTypeId))
                    {
                        var id = Convert.ToInt32(orderTypeId);
                        query = query.Where(x => x.OrderTypeId == id);
                    }

					if (!string.IsNullOrEmpty(employeeCode) && string.IsNullOrEmpty(isAuthenticateUser))
                    {
                        query = query.Where(x => x.EmployeeCode == employeeCode);
                    }

					if (!string.IsNullOrEmpty(createUser) && string.IsNullOrEmpty(isAuthenticateUser))
					{
						query = query.Where(x => x.CreateUser == createUser);
					}

					// search for authenticate with employeecode and createUser
					if (!string.IsNullOrEmpty(isAuthenticateUser) && !string.IsNullOrEmpty(createUser) && !string.IsNullOrEmpty(employeeCode))
					{
						query = query.Where(x => x.EmployeeCode == employeeCode || x.CreateUser == createUser);
					}

                    #endregion

                    #region // return:
                    return query.OrderByDescending(x => x.OrderId).ToList();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<OrderModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

		#region // OrderOnlyGet
		public List<Order> OrderOnlyGet(
				string orderId
				, string orderNo
				, string orderOutboundNo
				, string trackingNo
				, string orderDateFrom
				, string orderDateTo
				, string customerId
				, string customerCode
				, string userCode
				, string customerCodeDelivery
				, string orderNoDelivery
				, string deliveryDateFrom
				, string deliveryDateTo
				, string orderStatus
				, string orderTypeId
				, string employeeCode
				, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderOnlyGet";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
                        "orderId",orderId,
                        "orderNo",orderNo,
                        "orderOutboundNo",orderOutboundNo,
                        "trackingNo",trackingNo,
                        "orderDateFrom",orderDateFrom,
                        "orderDateTo",orderDateTo,
						"customerId",customerId,
						"customerCode",customerCode,
						"customerCodeDelivery",customerCodeDelivery,
						"orderNoDelivery",orderNoDelivery,						
						"deliveryDateFrom",deliveryDateFrom,
						"deliveryDateTo",deliveryDateTo,
						"orderStatus",orderStatus,
						"orderTypeId",orderTypeId,
						"employeeCode",employeeCode
			            });
			#endregion

			try
			{

				using (var db = new EcmsEntities())
				{
					#region // Query:
					var query = from o in db.Orders
								join t in db.OrderTypes on (o.OrderTypeId ?? 0) equals t.OrderTypeId into t_join
								from t in t_join.DefaultIfEmpty()
								join c in db.Customers on o.CustomerId equals c.CustomerId into c_join
								from c in c_join.DefaultIfEmpty()
								join u in db.Sys_User on c.UserCode equals u.UserCode
								join ep in db.Sys_User on o.EmployeeCode equals ep.UserCode into epc
								from ep in epc.DefaultIfEmpty()
								select o;

					#endregion

					#region // Filter:
					if (!string.IsNullOrEmpty(orderId))
					{
						var id = Convert.ToInt32(orderId);
						query = query.Where(x => x.OrderId == id);
					}

					if (!string.IsNullOrEmpty(orderNo))
					{
						query = query.Where(x => x.OrderNo==orderNo);
					}

					if (!string.IsNullOrEmpty(orderOutboundNo))
					{
						query = query.Where(x => x.OrderOutboundNo == orderOutboundNo);
					}

					if (!string.IsNullOrEmpty(trackingNo))
					{
						query = query.Where(x => x.TrackingNo == trackingNo);
					}

					if (!string.IsNullOrEmpty(orderDateFrom))
					{
						var dateFilter = Convert.ToDateTime(orderDateFrom);
						query = query.Where(x => x.CreatedDate >= dateFilter);
					}

					if (!string.IsNullOrEmpty(orderDateTo))
					{
						var dateFilter = Convert.ToDateTime(orderDateTo);
						query = query.Where(x => x.CreatedDate <= dateFilter);
					}

					if (!string.IsNullOrEmpty(deliveryDateFrom))
					{
						var dateFilter = Convert.ToDateTime(deliveryDateFrom);
						query = query.Where(x => x.DeliveryDate >= dateFilter);
					}

					if (!string.IsNullOrEmpty(deliveryDateTo))
					{
						var dateFilter = Convert.ToDateTime(deliveryDateTo);
						query = query.Where(x => x.DeliveryDate <= dateFilter);
					}

					if (!string.IsNullOrEmpty(customerId))
					{
						var id = Convert.ToInt32(customerId);
						query = query.Where(x => x.CustomerId == id);
					}

					//if (!string.IsNullOrEmpty(customerCode))
					//{
					//    query = query.Where(x => x.CustomerCode == customerCode || x.UserCode == customerCode);
					//}

					//if (!string.IsNullOrEmpty(userCode))
					//{
					//    query = query.Where(x => x.UserCode == userCode);
					//}

					if (!string.IsNullOrEmpty(customerCodeDelivery))
					{
						query = query.Where(x => x.CustomerCodeDelivery == customerCodeDelivery);
					}

					if (!string.IsNullOrEmpty(orderNoDelivery))
					{
						query = query.Where(x => x.OrderNoDelivery == orderNoDelivery);
					}

					if (!string.IsNullOrEmpty(orderStatus))
					{
						var id = Convert.ToInt32(orderStatus);
						query = query.Where(x => x.OrderStatus == id);
					}

					if (!string.IsNullOrEmpty(orderTypeId))
					{
						var id = Convert.ToInt32(orderTypeId);
						query = query.Where(x => x.OrderTypeId == id);
					}

					if (!string.IsNullOrEmpty(employeeCode))
					{
						query = query.Where(x => x.EmployeeCode == employeeCode);
					}
					#endregion

					#region // return:
					return query.OrderByDescending(x => x.OrderId).ToList();
					#endregion
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<Order>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

        #region // OrderCreate
        public Order OrderCreate(Order order, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderCreate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:

					#region //Check mã đơn hàng
					if (order.OrderTypeId == 1)
                    {
                        order.OrderNo = "B";
                        order.OrderStatus = 1;
                    }
                    if (order.OrderTypeId == 2)
                    {
                        order.OrderNo = "O";
                        order.OrderStatus = 3;
                    }

                    if (order.OrderTypeId == 3)
                    {
                        order.OrderNo = "P";
                    }

                    if (order.OrderTypeId == 4)
                    {
                        order.OrderNo = "S";
                    }
                    order.OrderNo = order.OrderNo + DateTime.Now.ToString("yyMMddHHmmss");
                    order.CreatedDate = DateTime.Now;

                    var orderNo = dbContext.Orders.FirstOrDefault(x => x.OrderNo == order.OrderNo);
					if (orderNo != null)
					{
						throw new Exception("Mã đơn hàng đã tồn tại trên hệ thống");
					}
					#endregion

					#region //Nếu mua hàng có sẵn trong kho, kiểm tra số lượng hàng
					if ((order.OrderTypeId ?? 0) == 4)
                    {
                        foreach (var p in order.OrderDetails)
                        {
                            var stockBalance = dbContext.Products.Where(x => x.ProductId == p.ProductId).FirstOrDefault();
                            if (stockBalance != null)
                            {
                                if (stockBalance.Balance < 1)
                                    throw new Exception("Sản phẩm đã hết hàng");
                                if (stockBalance.Balance < p.Quantity)
                                    throw new Exception("Không đủ số lượng");
                            }
                        }
					}
					#endregion

					#region // lấy thông tin khách hàng
					var customer = dbContext.Customers.SingleOrDefault(p => p.CustomerId == order.CustomerId);
                    if (!string.IsNullOrEmpty(customer.EmployeeCode))
                    {
                        order.EmployeeCode = customer.EmployeeCode;
					}
					#endregion

					#endregion

					#region // Build:

					// Sinh Mã DetailCode: Temp: MH1000001; MH1000002;
					var maxDetailCode = dbContext.OrderDetails.Max(p => p.DetailCode);

					int detailCode = string.IsNullOrEmpty(maxDetailCode) ? 1 : Convert.ToInt32(maxDetailCode.Substring(2));
					int count = 1;
					foreach (var item in order.OrderDetails)
					{
						item.DetailCode = "MH" + (detailCode + count);
						if (dbContext.OrderDetails.Any(p => p.DetailCode == item.DetailCode))
						{
							throw new Exception("Error create DetailCode");
						}
						count++;

						// Check TrackingNo:
						if (!string.IsNullOrEmpty(item.TrackingNo))
						{
							if (dbContext.OrderDetails.Any(p => p.TrackingNo == item.TrackingNo))
							{
								throw new Exception("TrackingNo is exist");
							}
						}
					}
					dbContext.Orders.Add(order);
                    dbContext.SaveChanges();

                    #endregion

                    #region // Return:

                    return order;

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

        #region // OrderUpdate
        public Order OrderUpdate(Order order, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var tranScope = new TransactionScope())
                {
                    var dbContext = new EcmsEntities();
                    #region // Check:

                    var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == order.OrderId);
                    if (orderReturn == null)
                        throw new Exception("Đơn hàng không tồn tại");

                    #endregion

                    #region // Build:

                    //Update thông tin order
                    orderReturn.DeliveryDate = order.DeliveryDate;
                    orderReturn.DeliveryAddress = order.DeliveryAddress;
                    orderReturn.DeliveryName = order.DeliveryName;
                    orderReturn.DeliveryMobile = order.DeliveryMobile;
                    orderReturn.DeliveryEmail = order.DeliveryEmail;

                    orderReturn.OrderOutboundNo = order.OrderOutboundNo;
                    orderReturn.TrackingNo = order.TrackingNo;
                    orderReturn.OrderStatus = order.OrderStatus;
                    orderReturn.IsInsurance = order.IsInsurance;
                    orderReturn.AmountDeposit = order.AmountDeposit;
                    orderReturn.AmountRemain = order.AmountRemain;
                    orderReturn.ConfigRateId = order.ConfigRateId;
                    orderReturn.DateToUsa = order.DateToUsa;

                    foreach (var item in order.OrderDetails.ToList())
                    {
                        var orderDetail = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == item.OrderDetailId);
                        orderDetail.ProductId = item.ProductId;
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.PriceWeb = item.PriceWeb;
                        orderDetail.PriceWebOff = item.PriceWebOff;
                        orderDetail.TaxUsaConfigId = item.TaxUsaConfigId;
                        orderDetail.ShipConfigId = item.ShipConfigId;
                        orderDetail.ShipUSAVN = item.ShipUSAVN;
                        orderDetail.Surcharge = item.Surcharge;
                        orderDetail.EffortConfigId = item.EffortConfigId;
                        orderDetail.ImageUrl = item.ImageUrl;
                        orderDetail.ProductLink = item.ProductLink;
                        orderDetail.Size = item.Size;
                        orderDetail.Color = item.Color;
                        orderDetail.Weight = item.Weight;
                        orderDetail.WebsiteId = item.WebsiteId;
                        orderDetail.ProductCode = item.ProductCode;
                        orderDetail.ProductName = item.ProductName;
                        orderDetail.CountryId = item.CountryId;
                        orderDetail.DetailStatus = item.DetailStatus;
                        orderDetail.OrderNoDelivery = item.OrderNoDelivery;
                        orderDetail.TrackingNo = item.TrackingNo;
                        orderDetail.DateToUsa = item.DateToUsa;
                        orderDetail.DeliveryStatus = item.DeliveryStatus;
                    }

                    //Lưu lịch sử order
                    var orderHistory = new OrderHistory();
                    orderHistory.OrderId = orderReturn.OrderId;
                    orderHistory.CreatedDate = DateTime.Now;

                    dbContext.SaveChanges();
                    tranScope.Complete();

                    #endregion

                    #region // Return:

                    return orderReturn;

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

        #region // OrderDelete
        public bool OrderDelete(
            string orderId
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderDelete";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "orderId", orderId
			            });
            #endregion

            try
            {
                using (var ts = new System.Transactions.TransactionScope())
                {
                    using (var dbContext = new EcmsEntities())
                    {
                        #region // Check:
                        int id = Convert.ToInt32(orderId);
                        var delOrder = dbContext.Orders.SingleOrDefault(x => x.OrderId == id);

                        #endregion

                        #region // Build:
                        // delete customer:
                        dbContext.Orders.Remove(delOrder);
						dbContext.SaveChanges();
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

        #region // ConvertQuoteOrder
        public bool ConvertQuoteOrder(string orderId
            , string needDate
            , string remark
            , string deliveryDate
            , string deliveryAddress
            , string deliveryName
            , string deliveryMobile
            , string deliveryEmail
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ConvertQuoteOrder";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
						, "orderId", orderId  
						, "needDate", needDate
						, "remark", remark
						, "deliveryDate", deliveryDate
						, "deliveryAddress", deliveryAddress
						, "deliveryName", deliveryName
						, "deliveryMobile", deliveryMobile
						, "deliveryEmail", deliveryEmail
			            });
            #endregion

            try
            {
                using (var tranScope = new TransactionScope())
                {
                    var dbContext = new EcmsEntities();

                    #region // Check:

                    int id = Convert.ToInt32(orderId);
                    var orderReturn = dbContext.Orders.FirstOrDefault(x => x.OrderId == id);
                    if (orderReturn == null)
                    {
                        throw new Exception("Đơn hàng không tồn tại");
                    }

                    if (orderReturn.OrderTypeId != 1)
                    {
                        throw new Exception("Đơn hàng hiện tại không phải yêu cầu báo giá");
                    }

                    #endregion

                    #region // Build:

                    //Update thông tin order
                    if (!string.IsNullOrEmpty(deliveryDate))
                    {
                        orderReturn.DeliveryDate = Convert.ToDateTime(deliveryDate);
                    }
                    if (!string.IsNullOrEmpty(deliveryAddress))
                    {
                        orderReturn.DeliveryAddress = deliveryAddress;
                    }
                    if (!string.IsNullOrEmpty(deliveryName))
                    {
                        orderReturn.DeliveryName = deliveryName;
                    }
                    if (!string.IsNullOrEmpty(deliveryMobile))
                    {
                        orderReturn.DeliveryMobile = deliveryMobile;
                    }
                    if (!string.IsNullOrEmpty(deliveryEmail))
                    {
                        orderReturn.DeliveryEmail = deliveryEmail;
                    }
                    orderReturn.OrderStatus = OrderInStatus.OrderPending;
                    orderReturn.OrderTypeId = OrderType_Const.OrderBylink;
                    orderReturn.OrderDate = DateTime.Now;
                    //Lưu lịch sử order
                    var orderHistory = new OrderHistory
                        {
                            OrderId = orderReturn.OrderId,
                            CreatedDate = DateTime.Now,
                            Remark = string.Format("Convert quote {0} to Order", orderReturn.OrderId)
                        };
                    dbContext.OrderHistories.Add(orderHistory);

                    dbContext.SaveChanges();
                    tranScope.Complete();

                    #endregion

                    #region // Return:
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

        #region // OrderGroupLinkGet
        public List<OrderDetailModel> OrderGroupLinkGet(
                string orderId
                , string orderNo
                , string orderOutboundNo
                , string trackingNo
                , string orderDateFrom
                , string orderDateTo
                , string customerId
                , string orderStatus
                , string orderTypeId
                , string websiteId
				, string shop
                , string employeeCode
                , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderGroupLinkGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
                        "orderId",orderId,
                        "orderNo",orderNo,
                        "orderOutboundNo",orderOutboundNo,
                        "trackingNo",trackingNo,
                        "orderDateFrom",orderDateFrom,
                        "orderDateTo",orderDateTo,
						"customerId",customerId,
						"orderStatus",orderStatus,
						"orderTypeId",orderTypeId,
						"websiteId",websiteId,
						"shop",shop,
						"employeeCode",employeeCode,
			            });
            #endregion

            try
            {

                using (var db = new EcmsEntities())
                {
                    #region // Query:
                    var query = from o in db.Orders
                                select o;

                    #region // Filter:
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        var id = Convert.ToInt32(orderId);
                        query = query.Where(x => x.OrderId == id);
                    }

                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        query = query.Where(x => x.OrderNo == orderNo);
                    }

                    if (!string.IsNullOrEmpty(orderOutboundNo))
                    {
                        query = query.Where(x => x.OrderOutboundNo == orderOutboundNo);
                    }

                    if (!string.IsNullOrEmpty(trackingNo))
                    {
                        query = query.Where(x => x.TrackingNo == trackingNo);
                    }

                    if (!string.IsNullOrEmpty(orderDateFrom))
                    {
                        var dateFilter = Convert.ToDateTime(orderDateFrom);
                        query = query.Where(x => x.OrderDate >= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(orderDateTo))
                    {
                        var dateFilter = Convert.ToDateTime(orderDateTo);
                        query = query.Where(x => x.OrderDate <= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(customerId))
                    {
                        var id = Convert.ToInt32(customerId);
                        query = query.Where(x => x.CustomerId == id);
                    }

                    if (!string.IsNullOrEmpty(orderStatus))
                    {
                        var id = Convert.ToInt32(orderStatus);
                        query = query.Where(x => x.OrderStatus == id);
                    }

                    if (!string.IsNullOrEmpty(orderTypeId))
                    {
                        var id = Convert.ToInt32(orderTypeId);
                        query = query.Where(x => x.OrderTypeId == id);
                    }

                    if (!string.IsNullOrEmpty(employeeCode))
                    {
                        query = query.Where(x => x.EmployeeCode == employeeCode);
                    }

					
                    #endregion

                    var details = from p in db.OrderDetails
                                  join o in query on p.OrderId equals o.OrderId
                                  join c in db.Countries on p.CountryId equals c.CountryId into c_join
								  from c in c_join.DefaultIfEmpty()
                                  where !db.OrderOutboundDetails.Any(x => x.OrderDetailId == p.OrderDetailId) 
										&& o.OrderStatus == 4 && o.OrderTypeId == 2 && (p.DetailStatus ?? 0) != 3
										&& p.Order.Customer.Status==0
                                  select new OrderDetailModel
                                  {
                                      OrderDetailId = p.OrderDetailId,
                                      OrderNo = o.OrderNo,
                                      OrderDate = o.CreatedDate,
                                      OrderTypeId = (o.OrderTypeId ?? 0),
                                      Color = p.Color,
                                      UserCode = o.Customer.UserCode,
                                      CustomerName = o.Customer.CustomerName,
                                      EffortConfigId = p.EffortConfigId,
                                      EffortConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.EffortConfigId).ConfigValue),
                                      ImageUrl = p.ImageUrl,
                                      OrderId = p.OrderId,
                                      PriceWeb = p.PriceWeb,
                                      PriceWebOff = p.PriceWebOff,
                                      ProductId = p.ProductId,
                                      ProductCode = p.ProductCode,
                                      ProductName = p.Product.ProductName,
                                      ProductLink = p.ProductLink,
                                      CountryId = p.CountryId,
                                      CountryName = c.CountryName,
                                      CurrencyCode = c.CurrencyCode,
                                      Quantity = p.Quantity,
                                      ShipConfigId = p.ShipConfigId,
                                      ShipConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.ShipConfigId).ConfigValue),
									  ShipModified=p.ShipModified,
                                      ShipUSAVN = p.ShipUSAVN,
                                      Size = p.Size,
                                      Surcharge = p.Surcharge,
                                      TaxUsaConfigId = p.TaxUsaConfigId,
                                      TaxUsaConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.TaxUsaConfigId).ConfigValue),
                                      WebsiteId = p.WebsiteId,
                                      WebsiteName = p.WebsiteLink.WebsiteName,
									  ParentWebsiteId = p.WebsiteLink.ParentId,
									  ParentWebsiteName = p.WebsiteLink.WebsiteLink2.WebsiteName,
                                      Weight = p.Weight,
                                      DetailStatus = p.DetailStatus,
									  CustomerCode=o.Customer.CustomerCode,
									  DetailCode=p.DetailCode,
									  RateCountryValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.RateCountryId).ConfigValue ?? 0),
									  Shop=p.Shop,
									  Remark=p.Remark
                                  };

                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        var id = Convert.ToInt32(websiteId);
                        details = details.Where(x => x.WebsiteId == id);
                    }

					if (!string.IsNullOrEmpty(shop))
					{
						details = details.Where(x => x.Shop.ToLower().Contains(shop.ToLower()));
					}
                    #endregion

                    #region // return:
                    return details.OrderBy(x => x.WebsiteId).OrderBy(p=>p.WebsiteId).OrderBy(p=>p.Shop).ToList();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<OrderDetailModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // OrderChangeStatus
        public bool OrderChangeStatus(
			string orderId
			, string orderStatus
			, string remark
			, ref string alParamsOutError)
        {
            #region // Temp
			string strFunctionName = "OrderChangeStatus";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "orderId",orderId
                        , "orderStatus",orderStatus
						, "remark",remark
			            });
            #endregion

            try
            {
                using (var tranScope = new TransactionScope())
                {
                    var dbContext = new EcmsEntities();
                    #region // Check:
                    int oId = Convert.ToInt32(orderId);
                    var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);

					if (orderReturn == null)
					{
						throw new Exception("Đơn hàng không tồn tại");
					}
                    #endregion

                    #region // Build:
                    orderReturn.OrderStatus = Convert.ToInt32(orderStatus);
					if (!string.IsNullOrEmpty(remark))
					{
						orderReturn.Remark = remark;
					}
					if (Convert.ToInt32(orderStatus) == OrderInStatus.OrderConfirmed)
					{
						orderReturn.ConfirmDate = DateTime.Now.Date;
					}
					#region //Nếu đã giao hàng trừ tiền trong tk khách hàng số tiền đã phân bổ cho đơn hàng đó.
					if (Convert.ToInt32(orderStatus) == OrderInStatus.Deliveried)
                    {
						orderReturn.DeliveryDate = DateTime.Now;
                        var customer = dbContext.Customers.Where(x => x.CustomerId == orderReturn.CustomerId).SingleOrDefault();
                        if (customer != null)
                        {
							var invoices = from p in dbContext.InvoiceDetails
										   where (p.Invoice.OrderId == oId
													&& p.Invoice.CustomerId == customer.CustomerId
													&& ((p.Invoice.BusinessCode == Const_BusinessCode.Business_201 && p.Invoice.Status == InvoiceStatus.Confirm)
													|| (p.Invoice.BusinessCode == Const_BusinessCode.Business_208 && p.Invoice.Status == InvoiceStatus.Confirm)
													|| (p.Invoice.BusinessCode == Const_BusinessCode.Business_209 && p.Invoice.Status == InvoiceStatus.Confirm)))
										   select p;
							if (invoices != null && invoices.Count() > 0)
							{
								var oAmount = Math.Round(invoices.Sum(p => p.Amount ?? 0), Constansts.NumberRoundDefault);
								if (oAmount > 0)
								{
									customer.Balance -= oAmount;

									//Lưu lịch sử số dư khách hàng
									var customerBalanceHistory = new CustomerBalanceHistory();
									customerBalanceHistory.Balance = customer.Balance;
									customerBalanceHistory.CustomerId = customer.CustomerId;
									customerBalanceHistory.CreatedDate = DateTime.Now;
									dbContext.CustomerBalanceHistories.Add(customerBalanceHistory);

									// Tạo Invoice
									var ivoi = new Invoice
									{
										CreatedDate = DateTime.Now,
										BusinessCode = Const_BusinessCode.Business_206,
										InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss"),
										InvoiceDate = DateTime.Now,
										Remark = string.Format("MBGN Xác nhận giao hàng cho Khách hàng={0}, orderNo={1}", customer.CustomerCode, orderReturn.OrderNo),
										OrderId = oId,
										CustomerId = customer.CustomerId,
										Status = InvoiceStatus.Confirm
									};

									var invoiceDetail = new InvoiceDetail
									{
										Amount = oAmount
									};
									ivoi.InvoiceDetails.Add(invoiceDetail);

									if (dbContext.Invoices.Any(x => x.InvoiceCode == ivoi.InvoiceCode))
									{
										throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
									}

									dbContext.Invoices.Add(ivoi);
								}
							}
                        }

						//// Cập nhật detail
						//var orderDetailReturn = orderReturn.OrderDetails.Where(x => x.TrackingNo == trackingNumber);
						//if (orderDetailReturn.Count() == 0)
						//{
						//    throw new Exception("Đơn hàng chi tiết không tồn tại");
						//}

						//if (!string.IsNullOrEmpty(dateStatusUpdate) && OrderOutboundStatus.InvInboundMBGN == Convert.ToInt32(status))
						//{
						//    item.DeliveryDate = Convert.ToDateTime(dateStatusUpdate);
						//}

						//item.DetailStatus = Convert.ToInt32(status);
					}
					#endregion

					#region // update detail
					if (Convert.ToInt32(orderStatus) == OrderInStatus.Deliveried && orderReturn.OrderTypeId==OrderType_Const.OrderShipOnly)
					{
						// Cập nhật lại tình trạng các món hàng:
						var orderDetais = from od in dbContext.OrderDetails
										  where od.OrderId == orderReturn.OrderId
										  select od;

						foreach (var item in orderDetais)
						{
							item.DetailStatus = Convert.ToInt32(orderStatus);
							item.DeliveryDate = DateTime.Now;
						}
					}
					#endregion

					#region // Create stockOut with OrderType=4
					if (Convert.ToInt32(orderStatus) == OrderInStatus.Deliveried && orderReturn.OrderTypeId == OrderType_Const.OrderByProduct)
					{
						// Sinh ra hóa đơn xuất:
						var stock = new Inv_StockInOut
						{
							CustomerId = orderReturn.CustomerId,
							Remark=string.Format("Hệ thống xác nhận xuất hàng cho KH"),
							status = StockStatus.StockConfirmed,
							InOutDate=DateTime.Now,
							Type= StockType.StockOut
						};

						foreach (var item in orderReturn.OrderDetails)
						{
							// Lấy serial của sản phẩm
							// Xử dụng phương pháp nhập trước xuất trước, dựa vào ngày nhập(DateIn) trong bảng Inv_Goods
							var stockGoods = dbContext.Inv_Goods.Where(p => p.ProductId == item.ProductId).OrderBy(p=>p.DateIn).FirstOrDefault(); 

							if (stockGoods != null)
							{
								// Update lại tình trạng trong kho
								stockGoods.Status = GoodStatus.GoodOutStock;
								stockGoods.DateOut = DateTime.Now.Date;

								var stockDetail = new Inv_StockInOutDetail
								{
									ProductId = item.ProductId,
									Price = ((item.PriceWebOff ?? 0) == 0 ? (item.PriceWeb ?? 0) : (item.PriceWebOff ?? 0)),
									Quantity = item.Quantity,
									Serial = stockGoods.Serial
								};
								stock.Inv_StockInOutDetail.Add(stockDetail);

								// Update lại số lượng tồn:
								var stockBalance = dbContext.Inv_StockBalance.SingleOrDefault(p => p.ProductId == item.ProductId);
								if (stockBalance != null)
								{
									stockBalance.Quantity = stockBalance.Quantity - 1;
								}
							}
						}
						var stockNew = new StockBiz().StockInOutCreate(dbContext, stock, ref alParamsOutError);
					}
					#endregion
					//Lưu lịch sử order
                    var orderHistory = new OrderHistory();
                    orderHistory.OrderId = orderReturn.OrderId;
                    orderHistory.CreatedDate = DateTime.Now;
					orderHistory.Remark = string.Format("Change status order to: {0}", orderStatus);
					dbContext.OrderHistories.Add(orderHistory);

                    dbContext.SaveChanges();
                    tranScope.Complete();
                    #endregion

                    #region // Return:

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

		#endregion OrderChangeStatus

		#region // RevertOrderStatus
		public bool RevertOrderStatus(
			string orderId
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "RevertOrderStatus";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "orderId",orderId
			            });
			#endregion

			try
			{
				using (var tranScope = new TransactionScope())
				{
					var dbContext = new EcmsEntities();
					#region // Check:
					int oId = Convert.ToInt32(orderId);
					var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);

					if (orderReturn == null)
					{
						throw new Exception("Đơn hàng không tồn tại");
					}

					if (orderReturn.OrderStatus != OrderInStatus.Deliveried)
					{
						throw new Exception("Đơn hàng không ở tình trạng Đã giao Hàng");
					}

					var customer = dbContext.Customers.Where(x => x.CustomerId == orderReturn.CustomerId).SingleOrDefault();
					if (customer == null)
					{
						throw new Exception("Thông tin KH không đúng");
					}
					#endregion

					#region // Build:
					
					// Lấy số tiền đã trừ:
					var invoices = from p in dbContext.InvoiceDetails
								   where (p.Invoice.OrderId == oId
											&& p.Invoice.CustomerId == customer.CustomerId
											&& (p.Invoice.BusinessCode == Const_BusinessCode.Business_206 && p.Invoice.Status == InvoiceStatus.Confirm)
											)
								   select p;

					if (invoices != null && invoices.Count() > 0)
					{
						var oAmount = invoices.Sum(p => p.Amount ?? 0);
						if (oAmount > 0)
						{
							// Hoàn lại số dư cho KH
							customer.Balance += oAmount;

							//Lưu lịch sử số dư khách hàng
							var customerBalanceHistory = new CustomerBalanceHistory();
							customerBalanceHistory.Balance = customer.Balance;
							customerBalanceHistory.CustomerId = customer.CustomerId;
							customerBalanceHistory.CreatedDate = DateTime.Now;
							dbContext.CustomerBalanceHistories.Add(customerBalanceHistory);

							foreach (var item in invoices)
							{
								item.Invoice.Status = InvoiceStatus.NotConfirm;
								item.Invoice.Description = string.Format("Hủy thanh toán do MBGN hoàn lại trạng thái Hoàn thành cho đơn hành");
								item.Invoice.LastDateModify = DateTime.Now;
							}
						}
					}

					// Revert status
					orderReturn.OrderStatus = OrderInStatus.Finished;

					//Lưu lịch sử order
					var orderHistory = new OrderHistory();
					orderHistory.OrderId = orderReturn.OrderId;
					orderHistory.CreatedDate = DateTime.Now;
					orderHistory.Remark = string.Format("revert Deliverly(7) to Finish(6)");
					dbContext.OrderHistories.Add(orderHistory);

					dbContext.SaveChanges();
					tranScope.Complete();
					#endregion

					#region // Return:

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

		#region // RevertFirstOrderStatus
		public bool RevertFirstOrderStatus(
			string orderId
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "RevertFirstOrderStatus";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "orderId",orderId
			            });
			#endregion

			try
			{
				using (var tranScope = new TransactionScope())
				{
					var dbContext = new EcmsEntities();
					#region // Check:
					int oId = Convert.ToInt32(orderId);
					var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);

					if (orderReturn == null)
					{
						throw new Exception("Đơn hàng không tồn tại");
					}

					// Chỉ cho revert ở 2 tình trạng là OrderConfirmed + Finished
					if (orderReturn.OrderStatus != OrderInStatus.OrderConfirmed && orderReturn.OrderStatus != OrderInStatus.Finished)
					{
						throw new Exception("Đơn hàng không ở tình trạng Confirm hoặc Finish");
					}

					var customer = dbContext.Customers.Where(x => x.CustomerId == orderReturn.CustomerId).SingleOrDefault();
					if (customer == null)
					{
						throw new Exception("Thông tin KH không đúng");
					}
					#endregion

					#region // Build:

					// Check created invoice for Order
					var invoices = from p in dbContext.InvoiceDetails
								   where (p.Invoice.OrderId == oId
											&& p.Invoice.CustomerId == customer.CustomerId
											&& (p.Invoice.BusinessCode == Const_BusinessCode.Business_201 && p.Invoice.Status == InvoiceStatus.Confirm)
											)
								   select p;

					if (invoices != null && invoices.Count() > 0)
					{
						throw new Exception("Đơn hàng đã được lập phiếu thanh toán, bạn không thể hoàn lại tình trạng");
					}

					// Revert status to OrderPending
					orderReturn.OrderStatus = OrderInStatus.OrderPending;
					orderReturn.ConfirmDate = null;
					orderReturn.Remark = string.Format("Hệ thống đã Hoàn lại tình trạng chưa xác nhận cho đơn hàng!");

					//Lưu lịch sử order
					var orderHistory = new OrderHistory();
					orderHistory.OrderId = orderReturn.OrderId;
					orderHistory.CreatedDate = DateTime.Now;
					orderHistory.Remark = string.Format("revert to Pending(3)");
					dbContext.OrderHistories.Add(orderHistory);

					#region // Nếu đã tính phí trả thì phải tính toán lại
					if (orderReturn.CalFeeDelay == Constansts.CalFeeDelay
						&& orderReturn.IsCalFeeDelay == Constansts.FlagActive
						&& orderReturn.OrderTypeId==OrderType_Const.OrderBylink
						)
					{
						// update lại thông tin Đã tính phí
						orderReturn.IsCalFeeDelay = null;
						orderReturn.AmountCalFeeDelay = 0;
						orderReturn.AmountFeeDelay = 0;
						orderReturn.CalDateFeeDelay = null;
					}
					#endregion

					dbContext.SaveChanges();
					tranScope.Complete();
					#endregion

					#region // Return:

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

		#region // OrderDetailChangeStatus
		public bool OrderDetailChangeStatus(string orderDetailId, string orderDetailStatus, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderDetailChangeStatus";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"orderDetailStatus",orderDetailStatus
			            });
            #endregion

            try
            {
                var dbContext = new EcmsEntities();

                #region // Check:

                int oDetailId = Convert.ToInt32(orderDetailId);
                var orderDetailReturn = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == oDetailId);

                if (orderDetailReturn == null)
                    throw new Exception("Đơn hàng không tồn tại");

                #endregion

                #region // Build:
                orderDetailReturn.DetailStatus = Convert.ToInt32(orderDetailStatus);

                dbContext.SaveChanges();
                #endregion

                #region // Return:

                return true;

                #endregion
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

		#region // OrderDetailChangeStatus

		public bool OrderDetailChangeStatus(
				string orderDetailId
				, string orderDetailStatusNew
				, string dateUpdate
				, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderDetailChangeStatus";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						,"orderDetailId",orderDetailId
                        ,"orderDetailStatusNew",orderDetailStatusNew
						,"dateUpdate",dateUpdate
			            });
			#endregion

			try
			{
				var dbContext = new EcmsEntities();

				#region // Check:

				int oDetailId = Convert.ToInt32(orderDetailId);
				var orderDetailReturn = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == oDetailId);

				if (orderDetailReturn == null)
					throw new Exception("Đơn hàng không tồn tại");

				#endregion

				#region // Build:
				if (!string.IsNullOrEmpty(orderDetailStatusNew))
				{
					orderDetailReturn.DetailStatus = Convert.ToInt32(orderDetailStatusNew);
				}

				if (!string.IsNullOrEmpty(orderDetailStatusNew) & OrderOutboundStatus.InvOutbound == Convert.ToInt32(orderDetailStatusNew))
				{
					orderDetailReturn.DateToUsa = Convert.ToDateTime(dateUpdate);
				}

				if (!string.IsNullOrEmpty(orderDetailStatusNew) & OrderOutboundStatus.InvInbound == Convert.ToInt32(orderDetailStatusNew))
				{
					orderDetailReturn.DeliveryVNDate = Convert.ToDateTime(dateUpdate);
				}

				if (!string.IsNullOrEmpty(orderDetailStatusNew) & OrderOutboundStatus.InvInboundMBGN == Convert.ToInt32(orderDetailStatusNew))
				{
					orderDetailReturn.DeliveryDate = Convert.ToDateTime(dateUpdate);
				}

				

				dbContext.SaveChanges();
				#endregion

				#region // Return:

				return true;

				#endregion
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

        #region // OrderEditTrackingNo

        public Order OrderEditTrackingNo(string orderId, string trackingNo, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderEditTrackingNo";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"trackingNo",trackingNo
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:
                    int oId = Convert.ToInt32(orderId);

                    var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);
                    if (orderReturn == null)
                        throw new Exception("Đơn hàng không tồn tại");

                    #endregion

                    #region // Build:

                    orderReturn.TrackingNo = trackingNo;
                    dbContext.SaveChanges();
                    #endregion

                    #region // Return:

                    return orderReturn;

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

        #region // UpdateTrackingOrder

		public bool UpdateTrackingOrder(
					string orderDetailId
					, string newTrackingNo
					, string orderDeliveryNo
					, string dateToUsa
					, string insuarance
					, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "UpdateTrackingOrder";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "orderDetailId",orderDetailId
                        , "newTrackingNo", newTrackingNo
                        , "orderDeliveryNo", orderDeliveryNo
                        , "dateToUsa", dateToUsa
                        , "insuarance",insuarance
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:
					int oId = Convert.ToInt32(orderDetailId);

                    var orderDetails = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == oId);
					if (orderDetails==null)
					{
						throw new Exception("Chi tiết đơn hàng không tồn tại.");
					}

                    #endregion

                    #region // Build:
					if (!string.IsNullOrEmpty(newTrackingNo))
					{
						orderDetails.TrackingNo = newTrackingNo;
					}
					
					orderDetails.OrderNoDelivery = orderDeliveryNo;

					if (!string.IsNullOrEmpty(dateToUsa))
					{
						orderDetails.DateToUsa = Convert.ToDateTime(dateToUsa);
					}
					else
					{
						orderDetails.DateToUsa = null;
					}

					if (!string.IsNullOrEmpty(insuarance))
					{
						orderDetails.InsuaranceConfigId = Convert.ToInt32(insuarance);
					}

                    dbContext.SaveChanges();

                    #endregion

                    #region // Return:

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

        #region // OrderUpdateEmployeeCode

        public Order OrderUpdateEmployeeCode(string orderId, string employeeCode, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderUpdateEmployeeCode";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						,"orderId",orderId
                        ,"employeeCode",employeeCode
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:
                    int oId = Convert.ToInt32(orderId);

                    var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);
                    if (orderReturn == null)
                    {
                        throw new Exception("Đơn hàng không tồn tại");
                    }
                    #endregion

                    #region // Build:

                    orderReturn.EmployeeCode = employeeCode;
                    dbContext.SaveChanges();
                    #endregion

                    #region // Return:

                    return orderReturn;

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

		#region // OrderUpdateConfigFee

		public bool OrderUpdateConfigFee(
					string orderId
					, string calFeeDelay
					, string dayAllowedFeeDelay
					, string feeDelay
					, string amountFeeDelayNew
					, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderUpdateEmployeeCode";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						,"orderId",orderId
                        ,"calFeeDelay",calFeeDelay
						,"dayAllowedFeeDelay",dayAllowedFeeDelay
						,"feeDelay",feeDelay
						,"amountFeeDelayNew",amountFeeDelayNew
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					#region // Check:
					int oId = Convert.ToInt32(orderId);

					var orderReturn = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);
					if (orderReturn == null)
					{
						throw new Exception("Đơn hàng không tồn tại");
					}
					#endregion

					#region // Build:
					var orderHistory = new OrderHistory()
					{
						OrderId = orderReturn.OrderId,
						CreatedDate = DateTime.Now,
						Remark = string.Format("Setting custom for feeDelay ")
					};

					if (!string.IsNullOrEmpty(calFeeDelay))
					{
						orderReturn.CalFeeDelay = calFeeDelay;
					}

					if (!string.IsNullOrEmpty(dayAllowedFeeDelay))
					{
						orderReturn.DayAllowedDelay = Convert.ToInt32(dayAllowedFeeDelay);
					}

					if (!string.IsNullOrEmpty(feeDelay))
					{
						orderReturn.FeeDelay = Convert.ToDouble(feeDelay);
					}

					if (!string.IsNullOrEmpty(amountFeeDelayNew))
					{
						orderHistory.Remark = orderHistory.Remark + string.Format("with changed old AmountFeeDelay={0}, AmountFeeDelayNew ={1}", orderReturn.AmountFeeDelay, amountFeeDelayNew);
						orderReturn.AmountFeeDelay = Convert.ToDouble(amountFeeDelayNew);
					}

					dbContext.OrderHistories.Add(orderHistory);

					dbContext.SaveChanges();
					#endregion

					#region // Return:

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

        #region // OrderDetail

		#region // OrderDetailUpdate
		public OrderDetail OrderDetailUpdate(OrderDetail orderDetail, bool isShipModify , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderDetailUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:

                    var orderDetailReturn = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == orderDetail.OrderDetailId);
                    if (orderDetailReturn == null)
                        throw new Exception("Chi tiết sản phẩm không tồn tại");

                    #endregion

                    #region //Build:

                    orderDetailReturn.WebsiteId = orderDetail.WebsiteId;
                    orderDetailReturn.CountryId = orderDetail.CountryId;
                    orderDetailReturn.CategoryId = orderDetail.CategoryId;
                    orderDetailReturn.PriceWebOff = orderDetail.PriceWebOff;
                    orderDetailReturn.TaxUsaConfigId = orderDetail.TaxUsaConfigId;
                    orderDetailReturn.ShipConfigId = orderDetail.ShipConfigId;
                    orderDetailReturn.Surcharge = orderDetail.Surcharge;
                    orderDetailReturn.Color = orderDetail.Color;
                    orderDetailReturn.Weight = orderDetail.Weight;
                    orderDetailReturn.ImageUrl = orderDetail.ImageUrl;
                    
                    orderDetailReturn.EffortModified = orderDetail.EffortModified;
                    orderDetailReturn.ShipUSA = orderDetail.ShipUSA;
                    orderDetailReturn.Size = orderDetail.Size;
                    orderDetailReturn.ShipModified = orderDetail.ShipModified;					
					orderDetailReturn.Remark = orderDetail.Remark;

                    if (orderDetail.PriceWeb != null)
                    {
                        orderDetailReturn.PriceWeb = orderDetail.PriceWeb;
                    }

					if (!string.IsNullOrEmpty(orderDetail.Shop)
						&& (orderDetailReturn.Shop != orderDetail.Shop || orderDetailReturn.Quantity != orderDetail.Quantity) 
						&& !isShipModify)
					{
						// update all item detail by shop 

						#region // shop new
						var orderByShop = from p in dbContext.OrderDetails
										  where p.Shop == orderDetail.Shop && p.OrderId == orderDetailReturn.OrderId
										  select p;
						if (orderDetailReturn.Shop == orderDetail.Shop)
						{
							orderByShop = orderByShop.Where(p => p.OrderDetailId != orderDetailReturn.OrderDetailId);
						}

						var shopQty = orderByShop.ToList().Sum(p => p.Quantity).Value +  orderDetail.Quantity.Value;

						var configBusiness = new CustomerBiz().ConfigBusinessGet(
											""
											, ""
											, ""
											, ""
											, Const_BusinessCode.Business_401
											, ""
											, ""
											, ""
											, ""
											, ref alParamsOutError
											).Where(p => p.fromQuantity <= shopQty && p.toQuantity >= shopQty).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

						foreach (var item in orderByShop)
						{
							if (item.ShipConfigId != configBusiness.ConfigBusinessId)
							{
								item.ShipConfigId = configBusiness.ConfigBusinessId;
							}
						}

						orderDetailReturn.ShipConfigId = configBusiness.ConfigBusinessId;
						#endregion

						#region // shop old
						if (orderDetailReturn.Shop != orderDetail.Shop && !string.IsNullOrEmpty(orderDetailReturn.Shop))
						{
							// update shipConfig for shop old changed

							var orderByShopOld = from p in dbContext.OrderDetails
												 where p.Shop == orderDetailReturn.Shop && p.OrderId == orderDetailReturn.OrderId && p.OrderDetailId != orderDetailReturn.OrderDetailId
												 select p;

							var shopQtyOld = orderByShopOld.ToList().Sum(p => p.Quantity).Value;

							var configBusinessOld = new CustomerBiz().ConfigBusinessGet(
											""
											, ""
											, ""
											, ""
											, Const_BusinessCode.Business_401
											, ""
											, ""
											, ""
											, ""
											, ref alParamsOutError
											).Where(p => p.fromQuantity <= shopQtyOld && p.toQuantity >= shopQtyOld).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

							foreach (var item in orderByShopOld)
							{
								if (item.ShipConfigId != configBusinessOld.ConfigBusinessId)
								{
									item.ShipConfigId = configBusinessOld.ConfigBusinessId;
								}
							}
						}

						#endregion
					}
					if (orderDetailReturn.Shop != orderDetail.Shop)
					{
						orderDetailReturn.Shop = orderDetail.Shop;
					}
					orderDetailReturn.Quantity = orderDetail.Quantity;

                    dbContext.SaveChanges();
                    #endregion

                    #region //Return:
                    return orderDetailReturn;
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

		#region // OrderDetailUpdate
		public bool OrderDetailUpdate(string orderDetailId
			, string statusNew
			, string dateStatusUpdate
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderDetailUpdate";
			var alParamsCoupleError = new ArrayList(new object[]{
                        "strFunctionName", strFunctionName
						, "orderDetailId", orderDetailId
						, "statusNew", statusNew
						, "dateStatusUpdate", dateStatusUpdate
                        });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					#region // Check:

					if (string.IsNullOrEmpty(orderDetailId))
					{
						throw new Exception("Invalid param orderDetailId");
					}

					if (string.IsNullOrEmpty(statusNew))
					{
						throw new Exception("Invalid param statusNew");
					}

					#endregion

					#region //Build:

					var iOrderDId = Convert.ToInt32(orderDetailId);
					var orderDetailReturn = dbContext.OrderDetails.Where(x => x.OrderDetailId == iOrderDId).FirstOrDefault();

					if (orderDetailReturn == null)
					{
						throw new Exception("Invalid OrderDetail");
					}

					if (!string.IsNullOrEmpty(dateStatusUpdate) && OrderInStatus.Deliveried == Convert.ToInt32(statusNew))
					{
						orderDetailReturn.DeliveryDate = Convert.ToDateTime(dateStatusUpdate);
					}

					orderDetailReturn.DetailStatus = Convert.ToInt32(statusNew);

					dbContext.SaveChanges();

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

		#region // DeliveryDetailUpdate
		public bool DeliveryDetailUpdate(List<OrderDetailModel> listOrderDetailModel, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "DeliveryDetailUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
                        "strFunctionName", strFunctionName                        
                        });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:

                    #endregion

                    #region // Build:

                    foreach (var item in listOrderDetailModel)
                    {
                        var orderDetailReturn = dbContext.OrderDetails.Where(x => x.OrderDetailId == item.OrderDetailId).FirstOrDefault();
                        if (orderDetailReturn == null)
                        {
                            //Nếu chưa có thì add vào
                            var orderDetail = new OrderDetail();
                            orderDetail.OrderId = item.OrderId;
                            orderDetail.Quantity = item.Quantity;
                            orderDetail.Weight = item.Weight;
                            orderDetail.ProductName = item.ProductName;
                            orderDetail.CategoryId = item.CategoryId;
                            orderDetail.ShipConfigId = item.ShipConfigId;
                            //orderDetail.DeliveryStatus = item.DeliveryStatus;
                            orderDetail.DateToUsa = item.DateToUsa;
                            orderDetail.Surcharge = item.Surcharge;
                            orderDetail.OrderNoDelivery = item.OrderNoDelivery;
                            orderDetail.TrackingNo = item.TrackingNo;
                            orderDetail.InsuaranceConfigId = item.InsuaranceConfigId;
                            orderDetail.DeclarePrice = item.DeclarePrice;
                            orderDetail.ShipModified = item.ShipModified;

							// Sinh Mã DetailCode: Temp: MH1000001; MH1000002;
							var maxDetailCode = dbContext.OrderDetails.Max(p => p.DetailCode);
							int detailCode = Convert.ToInt32(maxDetailCode.Substring(2));

							orderDetail.DetailCode = "MH" + (detailCode + 1);

							if (dbContext.OrderDetails.Any(p => p.DetailCode == orderDetail.DetailCode))
							{
								// Nếu trùng thì chuyển sang code tiếp theo
								throw new Exception("Error create DetailCode");
							}							

                            dbContext.OrderDetails.Add(orderDetail);
                        }
                    }
                    dbContext.SaveChanges();

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

		#region // ChangeDeliveryStatus
		public bool ChangeDeliveryStatus(
                    string orderId
                    , string trackingNumber
                    , string status
                    , string dateStatusUpdate
                    , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ChangeDeliveryStatus";
            var alParamsCoupleError = new ArrayList(new object[]{
                        "strFunctionName", strFunctionName,
                        "orderId", orderId,
                        "trackingNumber", trackingNumber,
						"dateStatusUpdate", dateStatusUpdate,
                        });
            #endregion

            try
            {
				using (var ts = new TransactionScope())
				{
					using (var dbContext = new EcmsEntities())
					{
						#region // Check:
						int oId = Convert.ToInt32(orderId);

						var orderReturn = dbContext.Orders.Where(x => x.OrderId == oId).FirstOrDefault();
						if (orderReturn == null)
						{
							throw new Exception("Đơn hàng không tồn tại");
						}
						var orderDetailReturn = orderReturn.OrderDetails.Where(x => x.TrackingNo == trackingNumber);
						if (orderDetailReturn.Count() == 0)
						{
							throw new Exception("Đơn hàng chi tiết không tồn tại");
						}
						#endregion

						#region // Build:

						foreach (var item in orderDetailReturn)
						{
							if (!string.IsNullOrEmpty(dateStatusUpdate) && OrderOutboundStatus.InvOutbound == Convert.ToInt32(status))
							{
								item.DateToUsa = Convert.ToDateTime(dateStatusUpdate);
							}

							if (!string.IsNullOrEmpty(dateStatusUpdate) && OrderOutboundStatus.InvInbound == Convert.ToInt32(status))
							{
								item.DeliveryVNDate = Convert.ToDateTime(dateStatusUpdate);
							}

							if (!string.IsNullOrEmpty(dateStatusUpdate) && OrderOutboundStatus.InvInboundMBGN == Convert.ToInt32(status))
							{
								item.DeliveryDate = Convert.ToDateTime(dateStatusUpdate);
							}

							item.DetailStatus = Convert.ToInt32(status);
						}
						dbContext.SaveChanges();

						#endregion

						ts.Complete();
						return true;
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

		#region // ChangeOrderStatus
		public bool ChangeOrderStatus(
					string orderId
					, string newStatus
					, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ChangeOrderStatus";
			var alParamsCoupleError = new ArrayList(new object[]{
                        "strFunctionName", strFunctionName,
                        "orderId", orderId,
                        "newStatus", newStatus,
                        });
			#endregion

			try
			{
				using (var ts = new TransactionScope())
				{
					using (var dbContext = new EcmsEntities())
					{
						#region // Check:
						int oId = Convert.ToInt32(orderId);

						var orderReturn = dbContext.Orders.Where(x => x.OrderId == oId).FirstOrDefault();
						if (orderReturn == null)
						{
							throw new Exception("Đơn hàng không tồn tại");
						}

						#endregion

						#region // Build:
						if (orderReturn.OrderStatus != Convert.ToInt32(newStatus))
						{
							orderReturn.OrderStatus = Convert.ToInt32(newStatus);

							if (orderReturn.OrderStatus == OrderInStatus.OrderDeleted)
							{
								orderReturn.Description = string.Format("Delete order by user: date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

								var orderHistory = new OrderHistory()
								{
									CreatedDate = DateTime.Now,
									OrderId = orderReturn.OrderId,
									Remark = orderReturn.Description
								};
								dbContext.OrderHistories.Add(orderHistory);
							}

							dbContext.SaveChanges();
						}
						#endregion

						ts.Complete();
						return true;
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

		#region // OrderDetailDeliveryUpdate
		public OrderDetail OrderDetailDeliveryUpdate(OrderDetail orderDetail, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderDetailDeliveryUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:

                    var orderDetailReturn = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == orderDetail.OrderDetailId);
                    if (orderDetailReturn == null)
                        throw new Exception("Chi tiết sản phẩm không tồn tại");

                    #endregion

                    #region //Build:

                    orderDetailReturn.CategoryId = orderDetail.CategoryId;
                    orderDetailReturn.ShipConfigId = orderDetail.ShipConfigId;
                    orderDetailReturn.Surcharge = orderDetail.Surcharge;
                    orderDetailReturn.Weight = orderDetail.Weight;
                    orderDetailReturn.Quantity = orderDetail.Quantity;
                    orderDetailReturn.ProductName = orderDetail.ProductName;
                    //orderDetailReturn.InsuaranceConfigId = orderDetail.InsuaranceConfigId;
                    orderDetailReturn.ShipModified = orderDetail.ShipModified;
                    orderDetailReturn.DeclarePrice = orderDetail.DeclarePrice;

                    dbContext.SaveChanges();
                    #endregion

                    #region //Return:
                    return orderDetailReturn;
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

		#region // OrderDetailTrackingUpdate
		public bool OrderDetailTrackingUpdate(List<OrderDetailModel> lstDetailModel, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderDetailTrackingUpdate";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					foreach (var item in lstDetailModel)
					{
						#region // Check:

						var orderDetailReturn = dbContext.OrderDetails.SingleOrDefault(x => x.OrderDetailId == item.OrderDetailId);
						if (orderDetailReturn == null)
						{
							throw new Exception("Chi tiết sản phẩm không tồn tại");
						}
						#endregion

						#region //Build:

						orderDetailReturn.TrackingNo = item.TrackingNo;
						dbContext.SaveChanges();
						#endregion
					}
					#region //Return:
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

		#region // GetDetailTotalMoney
		public double GetDetailTotalMoney(int orderDetailId)
		{
			
			using (var db = new EcmsEntities())
			{
				// Lấy Detail
				var detail = db.OrderDetails.SingleOrDefault(p => p.OrderDetailId == orderDetailId);

				// Tính tiền sản phẩm
				double moneyProduct = ((detail.PriceWebOff ?? 0) == 0 ? (detail.PriceWeb ?? 0) : (detail.PriceWebOff ?? 0)) * (detail.Quantity ?? 0);

				// tính effortConfigValue
				var configBs = db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == detail.EffortConfigId);
				double effortConfigValue = configBs == null ? 0 : configBs.ConfigValue ?? 0;

				// tính ShipConfigValue
				var configShip = db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == detail.ShipConfigId);
				double shipConfigValue = configShip == null ? 0 : configShip.ConfigValue ?? 0;

				// tính InsuaranceConfigValue
				var configIns = db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == detail.InsuaranceConfigId);
				double insuaranceConfigValue = configIns == null ? 0 : configIns.ConfigValue ?? 0;

				// tính TaxUsaConfigValue
				var configTax = db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == detail.TaxUsaConfigId);
				double taxUsaConfigValue = configTax == null ? 0 : configTax.ConfigValue ?? 0;

				double rateValue = 0;
				switch (detail.Order.OrderTypeId)
				{
					case 1: //Báo giá
					case 2: //Đặt hàng Link

						//Nếu link đã hủy
						if (detail.DetailStatus == 3) return 0;

						//Lấy tỷ giá theo xuất xứ
						rateValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == detail.RateCountryId).ConfigValue ?? 0);
						var fee = ((detail.Weight ?? 0) * (detail.ShipModified != null ? detail.ShipModified.Value : shipConfigValue));
						return (moneyProduct + fee + (moneyProduct * (detail.EffortModified == null ? effortConfigValue : (detail.EffortModified ?? 0)) * 0.01) + (moneyProduct * taxUsaConfigValue * 0.01) + (detail.Surcharge ?? 0) + (detail.ShipUSA ?? 0)) * rateValue;

					case 3: //Vận chuyển
						//Lấy tỷ giá USD
						rateValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == detail.Order.ConfigRateId).ConfigValue ?? 0);
						var feeVc = ((detail.Weight ?? 0) * (detail.ShipModified != null ? detail.ShipModified.Value : shipConfigValue));
						return (feeVc + (detail.Surcharge ?? 0) + ((detail.DeclarePrice ?? 0) * insuaranceConfigValue * 0.01)) * rateValue;
					case 4: //Mua trên site
						return moneyProduct;
					default:
						return 0;
				}
			}			
		}
		#endregion

		#region // GetTotalPayAmountNormal
		public double GetTotalPayAmountNormal(int orderId)
		{

			using (var dbContext = new EcmsEntities())
			{
				var money = (from id in dbContext.InvoiceDetails
							 join i in dbContext.Invoices on id.InvoiceId equals i.InvoiceId
							 where i.OrderId == orderId && i.Status == 2
							 //&& (i.BusinessCode == Const_BusinessCode.Business_201 || i.BusinessCode == Const_BusinessCode.Business_207 || i.BusinessCode == Const_BusinessCode.Business_208)
							 && (i.BusinessCode == Const_BusinessCode.Business_207 || i.BusinessCode == Const_BusinessCode.Business_208 || i.BusinessCode == Const_BusinessCode.Business_209)
							 select id).Sum(x => x.Amount);
				return (money ?? 0);
			}
		}
		#endregion

		#region // OrderDetailTrackingNoCheck
		public bool OrderDetailTrackingNoCheck(
				    string trackingNo
				   , ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderDetailTrackingNoCheck";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						,"trackingNo", trackingNo
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					return dbContext.OrderDetails.Any(p => p.TrackingNo == trackingNo);
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

		#region // OrderDetailTrackingNoUpdate
		public bool OrderDetailTrackingNoUpdate(
					string orderDetailId
					, string newTrackingNo
				    , ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderDetailTrackingNoUpdate";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						,"orderDetailId", orderDetailId
						,"newTrackingNo", newTrackingNo
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					// check:
					var iOrderDetail = Convert.ToInt32(orderDetailId);
					if (!string.IsNullOrEmpty(newTrackingNo) && dbContext.OrderDetails.Any(p => p.TrackingNo == newTrackingNo && p.OrderDetailId != iOrderDetail))
					{
						throw new Exception("Mã Bill đã tồn tại trong hệ thống !!!");
					}
					
					var orderDetail = dbContext.OrderDetails.SingleOrDefault(p => p.OrderDetailId == iOrderDetail);

					if (orderDetail !=null)
					{
						orderDetail.TrackingNo = newTrackingNo;
					}
					dbContext.SaveChanges();
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

		#region // OrderDetailModelGet
		public List<OrderDetailModel> OrderDetailModelGet(
				   string fromDate
				   , string toDate
				   , string detailCode
				   , string orderNo
				   , string customerCode
				   , string websiteId
				   , string parentWebsiteId
				   , string status
				   , string orderTypeId
				   , string trackingNo
				   , string shop
				   , string employeeCode
				   , ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderDetailModelGet";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
						,"detailCode",detailCode
						,"orderNo",orderNo
                        ,"customerCode",customerCode
                        ,"websiteId",websiteId
						,"parentWebsiteId",parentWebsiteId
                        ,"status",status
						,"orderTypeId",orderTypeId
						,"trackingNo",trackingNo
						,"shop",shop
			            });
			#endregion

			try
			{

				using (var dbContext = new EcmsEntities())
				{
					var query = from p in dbContext.OrderDetails
								join ood in dbContext.OrderOutboundDetails on p.OrderDetailId equals ood.OrderDetailId into oodp
								from ood in oodp.DefaultIfEmpty()
								join w in dbContext.WebsiteLinks on p.WebsiteId equals w.WebsiteId into wp
								from w in wp.DefaultIfEmpty()
								join pr in dbContext.Products on (p.ProductId ?? 0) equals pr.ProductId into pr_join
								from pr in pr_join.DefaultIfEmpty()
								join cc in dbContext.Categories on p.CategoryId equals cc.CategoryId into cc_join
								from cc in cc_join.DefaultIfEmpty()
								join rd in dbContext.Rpt_DeliverlyCPDetail on p.OrderDetailId equals rd.OrderDetailId into rd_join
								from rd in rd_join.DefaultIfEmpty()
								join ci in dbContext.Countries on p.CountryId equals ci.CountryId into ci_join
								from ci in ci_join.DefaultIfEmpty()
								where p.Order.Customer.Status == 0 && (p.Order.OrderTypeId==2 || p.Order.OrderTypeId==3) 
								&& p.Order.OrderStatus != OrderInStatus.OrderCancel
								&& p.Order.OrderStatus != OrderInStatus.OrderDeleted
								orderby new { p.Order.OrderId, p.Order.OrderDate, p.TrackingNo, p.OrderDetailId, } descending
								select new OrderDetailModel
								{
									DetailCode = p.DetailCode,
									Color = p.Color,
									EffortConfigId = p.EffortConfigId,
									EffortConfigValue = (dbContext.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.EffortConfigId).ConfigValue),
									EffortModified = p.EffortModified,
									ImageUrl = p.ImageUrl,
									OrderDetailId = p.OrderDetailId,
									OrderId = p.OrderId,
									PriceWeb = p.PriceWeb,
									PriceWebOff = p.PriceWebOff,
									ProductId = p.ProductId,
									ProductCode = pr == null ? p.ProductCode : pr.ProductCode,
									ProductName = pr == null ? p.ProductName : pr.ProductName,
									ProductLink = p.ProductLink,
									Quantity = p.Quantity,
									ShipConfigId = p.ShipConfigId,
									ShipConfigValue = (dbContext.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.ShipConfigId).ConfigValue),
									ShipModified = p.ShipModified,
									DeclarePrice = p.DeclarePrice,
									ShipUSAVN = p.ShipUSAVN,
									ShipUSA = p.ShipUSA,
									Size = p.Size,
									Surcharge = p.Surcharge,
									TaxUsaConfigId = p.TaxUsaConfigId,
									TaxUsaConfigValue = (dbContext.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.TaxUsaConfigId).ConfigValue),

									InsuaranceConfigId = p.InsuaranceConfigId,
									InsuaranceConfigValue = (dbContext.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.InsuaranceConfigId).ConfigValue),
									WebsiteId = p.WebsiteId,
									WebsiteName = p.WebsiteLink.WebsiteName,
									ParentWebsiteId = p.WebsiteLink.ParentId,
									ParentWebsiteName = p.WebsiteLink.WebsiteLink2.WebsiteName,
									Weight = p.Weight,
									DetailStatus = p.Order.OrderTypeId == 3 ? (p.DetailStatus == null ? 0 : p.DetailStatus) : (p.DetailStatus ?? 0),
									CountryId = p.CountryId,
									CountryName = ci.CountryName,
									CurrencyCode = ci.CurrencyCode,
									OrderTypeId = p.Order.OrderTypeId.Value,
									OrderTypeName = p.Order.OrderType.OrderTypeName,
									CategoryId = p.CategoryId,
									CategoryName = cc.CategoryName,
									ParentCategoryId = cc.ParentId,
									TrackingNo = p.TrackingNo,
									OrderNoDelivery = p.OrderNoDelivery,
									DateToUsa = p.DateToUsa,
									UserCode = p.Order.Customer.UserCode,
									CustomerName = p.Order.Customer.CustomerName,
									DeliveryVNDate = p.DeliveryVNDate,
									DeliveryDate = p.DeliveryDate,
									LotPrice = p.LotPrice,
									RateCountryValue = (dbContext.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.RateCountryId).ConfigValue ?? 0),
									RateUSDValue = (dbContext.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.Order.ConfigRateId).ConfigValue ?? 0),
									OrderStatus = p.Order.OrderStatus,
									OrderDate=p.Order.OrderDate,
									OrderNo=p.Order.OrderNo,
									CustomerCode = p.Order.Customer.CustomerCode,
									OrderOutboundId = ood.OrderOutboundId,
									OrderOutboundNo = ood.OrderOutbound.OrderOutboundNo,
									Shop=p.Shop,
									EmployeeCode=p.Order.EmployeeCode
								};

					if (!string.IsNullOrEmpty(fromDate))
					{
						var date = Convert.ToDateTime(fromDate);
						query = query.Where(p => p.OrderDate >= date);
					}

					if (!string.IsNullOrEmpty(toDate))
					{
						var date = Convert.ToDateTime(toDate);
						query = query.Where(p => p.OrderDate <= date);
					}

					if (!string.IsNullOrEmpty(detailCode))
					{
						query = query.Where(p => p.DetailCode == detailCode);
					}

					if (!string.IsNullOrEmpty(orderNo))
					{
						query = query.Where(p => p.OrderNo == orderNo || p.OrderOutboundNo == orderNo);
					}

					if (!string.IsNullOrEmpty(websiteId))
					{
						int id = Convert.ToInt32(websiteId);
						query = query.Where(p => p.WebsiteId == id);
					}

					if (!string.IsNullOrEmpty(parentWebsiteId))
					{
						int id = Convert.ToInt32(parentWebsiteId);
						query = query.Where(p => p.ParentWebsiteId == id);
					}

					if (!string.IsNullOrEmpty(status))
					{
						int id = Convert.ToInt32(status);
						query = query.Where(p => p.DetailStatus == id);
					}

					if (!string.IsNullOrEmpty(orderTypeId))
					{
						int id = Convert.ToInt32(orderTypeId);
						query = query.Where(p => p.OrderTypeId == id);
					}

					if (!string.IsNullOrEmpty(customerCode))
					{
						query = query.Where(p => p.CustomerCode == customerCode || p.UserCode == customerCode);
					}

					if (!string.IsNullOrEmpty(trackingNo))
					{
						query = query.Where(p => p.TrackingNo.Contains(trackingNo) || p.OrderNoDelivery.Contains(trackingNo));
					}

					if (!string.IsNullOrEmpty(shop))
					{
						query = query.Where(p => p.Shop.Contains(shop));
					}

					if (!string.IsNullOrEmpty(employeeCode))
					{
						query = query.Where(x => x.EmployeeCode == employeeCode);
					}

					return query.Take(1000).ToList();
				}

			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<OrderDetailModel>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}

		#endregion

		#endregion

		#region // OrderOutbound

		#region // OrderOutboundGet
		public List<OrderOutboundModel> OrderOutboundGet(
                string orderOutboundId
                , string orderOutboundNo
                , string trackingNo
                , string orderNumber
                , string orderDateFrom
                , string orderDateTo
                , string orderStatus
                , string accountWebsiteId
                , string accountWebsiteNo
                , string websiteId
                , string parentWebsiteId
                , string userCreate
				, string visaId
                , string isGetDetail
                , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderOutboundGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
                        "orderOutboundId",orderOutboundId,
                        "orderOutboundNo",orderOutboundNo,
                        "trackingNo",trackingNo,
                        "orderDateFrom",orderDateFrom,
                        "orderDateTo",orderDateTo,
						"orderStatus",orderStatus,
						"accountWebsiteId",accountWebsiteId,
						"accountWebsiteNo",accountWebsiteNo,
						"websiteId",websiteId,
						"parentWebsiteId",parentWebsiteId,
						"userCreate",userCreate,
						"visaId",visaId,
						"isGetDetail",isGetDetail
			            });
            #endregion

            try
            {

                using (var db = new EcmsEntities())
                {
                    #region // Query:
                    var query = from o in db.OrderOutbounds
                                join w in db.WebsiteAccounts on o.WebsiteAccountId equals w.WebsiteAccountId into w_join
                                from w in w_join.DefaultIfEmpty()
                                select new OrderOutboundModel()
                                {
                                    AccountWebsiteId = o.WebsiteAccountId,
                                    AccountWebsiteNo = w.AccountWebsiteNo,
                                    OrderOutboundId = o.OrderOutboundId,
                                    OrderOutboundNo = o.OrderOutboundNo,
                                    TrackingNo = o.TrackingNo,
                                    OrderNumber = o.OrderNumber,
                                    CreatedDate = o.CreatedDate,
                                    OrderDate = o.OrderDate,
                                    DeliverlyUSADate = o.DeliveryUSADate,
                                    DeliverlyVNDate = o.DeliveryVNDate,
                                    DeliverlyDate = o.DeliveryDate,
                                    Remark = o.Remark,
                                    UserCreate = o.UserCreate,
                                    Status = o.Status,
                                    //VisaNo = ws.VisaNo,
									WebsiteAccountVisaId=o.WebsiteAccountVisaId,
									VisaId=o.VisaId,
									VisaNo=o.Mst_VisaAccount.VisaNo,
                                    WebsiteId = w.WebsiteId,
                                    WebsiteName = w.WebsiteLink.WebsiteName,
                                    ParentWebsiteId = w.WebsiteLink.ParentId,
                                    lstOrderOutboundDetailModel = (from p in db.OrderOutboundDetails
                                                                   where p.OrderOutboundId == o.OrderOutboundId
                                                                   select new OrderOutboundDetailModel
                                                                   {
                                                                       OrderDetailId = p.OrderDetailId,
                                                                       OrderOutboundDetailId = p.OrderOutboundDetailId,
                                                                       OrderOutboundId = p.OrderOutboundId,
                                                                       Price = p.Price,
                                                                       Quantity = p.Quantity,
                                                                       OrderDetail = new OrderDetailModel()
                                                                       {
																		   DetailCode=p.OrderDetail.DetailCode,
                                                                           Color = p.OrderDetail.Color,
                                                                           CountryId = p.OrderDetail.CountryId,
                                                                           CurrencyCode = (from c in db.Countries
                                                                                           where c.CountryId == p.OrderDetail.CountryId
                                                                                           select c.CurrencyCode).FirstOrDefault(),
                                                                           CountryName = (from c in db.Countries
                                                                                          where c.CountryId == p.OrderDetail.CountryId
                                                                                          select c.CountryName).FirstOrDefault(),
                                                                           OrderTypeId = p.OrderDetail.Order.OrderTypeId.Value,
                                                                           EffortConfigId = p.OrderDetail.EffortConfigId,
                                                                           EffortConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.OrderDetail.EffortConfigId).ConfigValue),
                                                                           EffortModified = p.OrderDetail.EffortModified,
                                                                           ImageUrl = p.OrderDetail.ImageUrl,
                                                                           OrderDetailId = p.OrderDetail.OrderDetailId,
                                                                           OrderId = p.OrderDetail.Order.OrderId,
                                                                           OrderNo = p.OrderDetail.Order.OrderNo,
                                                                           PriceWeb = p.OrderDetail.PriceWeb,
                                                                           PriceWebOff = p.OrderDetail.PriceWebOff,
                                                                           ProductId = p.OrderDetail.ProductId,
                                                                           ProductLink = p.OrderDetail.ProductLink,
                                                                           Quantity = p.OrderDetail.Quantity,
                                                                           ShipUSAVN = p.OrderDetail.ShipUSAVN,
                                                                           Size = p.OrderDetail.Size,
                                                                           Surcharge = p.OrderDetail.Surcharge,
                                                                           TaxUsaConfigId = p.OrderDetail.TaxUsaConfigId,
                                                                           TaxUsaConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.OrderDetail.TaxUsaConfigId).ConfigValue),
                                                                           WebsiteId = p.OrderDetail.WebsiteId,
                                                                           WebsiteName = p.OrderDetail.WebsiteLink.WebsiteName,
                                                                           Weight = p.OrderDetail.Weight,
                                                                           DetailStatus = p.OrderDetail.DetailStatus,
                                                                           UserCode = p.OrderDetail.Order.Customer.UserCode,
																		   CustomerCode=p.OrderDetail.Order.Customer.CustomerCode,
                                                                           CustomerName = p.OrderDetail.Order.Customer.CustomerName,
																		   ShipModified = p.OrderDetail.ShipModified,
																		   ShipConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.OrderDetail.ShipConfigId).ConfigValue),
																		   RateCountryValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.OrderDetail.RateCountryId).ConfigValue ?? 0),
																		   RateUSDValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.OrderDetail.Order.ConfigRateId).ConfigValue ?? 0),
																		   Shop=p.OrderDetail.Shop,
																		   TrackingNo = p.OrderDetail.TrackingNo
                                                                       }
                                                                   }
                                                            )
                                };



                    #endregion

                    #region // Filter:
                    if (!string.IsNullOrEmpty(orderOutboundId))
                    {
                        var id = Convert.ToInt32(orderOutboundId);
                        query = query.Where(x => x.OrderOutboundId == id);
                    }

                    if (!string.IsNullOrEmpty(orderOutboundNo))
                    {
                        query = query.Where(x => x.OrderOutboundNo == orderOutboundNo);
                    }

                    if (!string.IsNullOrEmpty(trackingNo))
                    {
                        query = query.Where(x => x.TrackingNo == trackingNo);
                    }

                    if (!string.IsNullOrEmpty(orderNumber))
                    {
                        query = query.Where(x => x.OrderNumber == orderNumber);
                    }

                    if (!string.IsNullOrEmpty(userCreate))
                    {
                        query = query.Where(x => x.UserCreate == userCreate);
                    }

                    if (!string.IsNullOrEmpty(accountWebsiteId))
                    {
                        var id = Convert.ToInt32(accountWebsiteId);
                        query = query.Where(x => x.AccountWebsiteId == id);
                    }
                    if (!string.IsNullOrEmpty(accountWebsiteNo))
                    {
                        query = query.Where(x => x.AccountWebsiteNo == accountWebsiteNo);
                    }

                    if (!string.IsNullOrEmpty(orderDateFrom))
                    {
                        var dateFilter = Convert.ToDateTime(orderDateFrom);
                        query = query.Where(x => x.OrderDate >= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(orderDateTo))
                    {
                        var dateFilter = Convert.ToDateTime(orderDateTo);
                        query = query.Where(x => x.OrderDate <= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(orderStatus))
                    {
                        var id = Convert.ToInt32(orderStatus);
                        query = query.Where(x => x.Status == id);
                    }

                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        var id = Convert.ToInt32(websiteId);
                        query = query.Where(x => x.WebsiteId == id);
                    }
                    if (!string.IsNullOrEmpty(parentWebsiteId))
                    {
                        var id = Convert.ToInt32(parentWebsiteId);
                        query = query.Where(x => x.ParentWebsiteId == id);
                    }

					if (!string.IsNullOrEmpty(visaId))
					{
						var id = Convert.ToInt32(visaId);
						query = query.Where(x => x.VisaId == id);
					}
                    #endregion

                    #region // return:
                    return query.OrderByDescending(x => x.OrderOutboundId).ToList();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<OrderOutboundModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // OrderOutboundCreate
        public OrderOutbound OrderOutboundCreate(OrderOutbound orderOutbound, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderOutboundCreate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var tranScope = new TransactionScope())
                {
                    using (var dbContext = new EcmsEntities())
                    {
                        #region // Check:

                        //Check mã đơn hàng

                        orderOutbound.OrderOutboundNo = "OB" + DateTime.Now.ToString("yyMMddHHmmss");
                        orderOutbound.CreatedDate = DateTime.Now;

                        var orderOutboundNo = dbContext.OrderOutbounds.FirstOrDefault(x => x.OrderOutboundNo == orderOutbound.OrderOutboundNo);
                        if (orderOutboundNo != null)
                        {
                            throw new Exception("Mã đơn hàng đã tồn tại trên hệ thống");
                        }

                        #endregion

                        #region // Build:

                        dbContext.OrderOutbounds.Add(orderOutbound);

						////Thay đổi DetailStatus bên OrderDetail
						foreach (var item in orderOutbound.OrderOutboundDetails)
						{
							// Cập nhật lại tình trạng các món hàng:
							var orderDetais = dbContext.OrderDetails.SingleOrDefault(p => p.OrderDetailId == item.OrderDetailId);
							if(orderDetais !=null)	  
							{
								orderDetais.DetailStatus = OrderOutboundStatus.InProcess;
							}
						}

                        dbContext.SaveChanges();
                        tranScope.Complete();
                        #endregion

                        #region // Return:

                        return orderOutbound;

                        #endregion
                    }

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

        #region // OrderOutboundUpdate
        public OrderOutbound OrderOutboundUpdate(OrderOutbound orderOutbound, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderOutboundUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var tranScope = new TransactionScope())
                {
                    var dbContext = new EcmsEntities();
                    #region // Check:

                    var oobReturn = dbContext.OrderOutbounds.SingleOrDefault(x => x.OrderOutboundId == orderOutbound.OrderOutboundId);
                    if (oobReturn == null)
                    {
                        throw new Exception("Đơn hàng không tồn tại");
                    }
                    #endregion

                    #region // Build:

                    //Update thông tin OrderOutbound
                    oobReturn.OrderDate = orderOutbound.OrderDate;
                    oobReturn.Remark = orderOutbound.Remark;
                    oobReturn.TrackingNo = orderOutbound.TrackingNo;
                    oobReturn.WebsiteAccountId = orderOutbound.WebsiteAccountId;
                    oobReturn.OrderNumber = orderOutbound.OrderNumber;
                    oobReturn.Status = orderOutbound.Status;

                    //Xóa OrderOutbound detail
                    var OrderOutboundDetail = dbContext.OrderOutboundDetails.Where(x => x.OrderOutboundId == oobReturn.OrderOutboundId);
                    foreach (var item in OrderOutboundDetail)
                    {
                        dbContext.OrderOutboundDetails.Remove(item);
                    }

                    // Add lại OrderOutbound detail
					foreach (var od in orderOutbound.OrderOutboundDetails.ToList())
					{
						// Cập nhật lại tình trạng các món hàng:
						var orderDetais = dbContext.OrderDetails.SingleOrDefault(p => p.OrderDetailId == od.OrderDetailId);
						if (orderDetais != null)
						{
							orderDetais.DetailStatus = orderOutbound.Status;
						}
						oobReturn.OrderOutboundDetails.Add(od);
					}

					// add history
					var onHistory = new OrderOutboundHistory
					{
						CreatedDate = DateTime.Now
						,
						Remark = string.Format("Update OrderOutbound with status={0}", orderOutbound.Status)
					};
					oobReturn.OrderOutboundHistories.Add(onHistory);

                    dbContext.SaveChanges();
                    tranScope.Complete();

                    #endregion

                    #region // Return:

                    return oobReturn;

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

        #region // OrderOutboundUpdateTracking
        public bool OrderOutboundUpdateTracking(
            string orderOutboundId
            , string trackingNo
            , string orderNumber //???
            , string status
            , string dateUpdateForStatus // Cập nhật theo trạng thái
            , ref string alParamsOutError)
        {
            #region // Temp
			string strFunctionName = "OrderOutboundUpdateTracking";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"orderOutboundId", orderOutboundId,
						"trackingNo", trackingNo,
						"orderNumber", orderNumber,
						"status", status,
						"dateUpdateForStatus", dateUpdateForStatus,
			            });
            #endregion

            try
            {
				var boolCalFeeOutAgain = false;
                using (var tranScope = new TransactionScope())
                {
                    var dbContext = new EcmsEntities();
                    #region // Check:
                    int outId = Convert.ToInt32(orderOutboundId);
                    var oobReturn = dbContext.OrderOutbounds.SingleOrDefault(x => x.OrderOutboundId == outId);
                    if (oobReturn == null)
                    {
                        throw new Exception("Đơn hàng không tồn tại");
                    }

					var orderDetais = from od in dbContext.OrderDetails
									  join ood in dbContext.OrderOutboundDetails on od.OrderDetailId equals ood.OrderDetailId
									  where ood.OrderOutboundId == outId
									  select od;


                    #endregion

                    #region // Build:

                    //Update thông tin OrderOutbound
                    if (!string.IsNullOrEmpty(trackingNo))
                    {
                        oobReturn.TrackingNo = trackingNo;
                    }
                    if (!string.IsNullOrEmpty(orderNumber))
                    {
                        oobReturn.OrderNumber = orderNumber;
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        oobReturn.Status = Convert.ToInt32(status);
                    }

                    if (!string.IsNullOrEmpty(status) & OrderOutboundStatus.InvOutbound == Convert.ToInt32(status))
                    {
                        oobReturn.DeliveryUSADate = Convert.ToDateTime(dateUpdateForStatus);
                    }

                    if (!string.IsNullOrEmpty(status) & OrderOutboundStatus.InvInbound == Convert.ToInt32(status))
                    {
                        oobReturn.DeliveryVNDate = Convert.ToDateTime(dateUpdateForStatus);
                    }

                    if (!string.IsNullOrEmpty(status) & OrderOutboundStatus.InvInboundMBGN == Convert.ToInt32(status))
                    {
                        oobReturn.DeliveryDate = Convert.ToDateTime(dateUpdateForStatus);
                    }

					// update orderDetail with status
					var isCalFeeDelayAgain = false;
					foreach (var item in orderDetais)
					{
						if (!string.IsNullOrEmpty(status) & OrderOutboundStatus.InvOutbound == Convert.ToInt32(status))
						{
							item.DateToUsa = Convert.ToDateTime(dateUpdateForStatus);
						}

						if (!string.IsNullOrEmpty(status) & OrderOutboundStatus.InvInbound == Convert.ToInt32(status))
						{
							item.DeliveryVNDate = Convert.ToDateTime(dateUpdateForStatus);
						}

						if (!string.IsNullOrEmpty(status) & OrderOutboundStatus.InvInboundMBGN == Convert.ToInt32(status))
						{
							item.DeliveryDate = Convert.ToDateTime(dateUpdateForStatus);
						}

						if (!string.IsNullOrEmpty(status) && OrderOutboundStatus.Cancel == Convert.ToInt32(status))
						{
							// Nếu đã tính phí trả thì phải tính toán lại
							if (item.Order.CalFeeDelay == Constansts.CalFeeDelay
								&& item.Order.IsCalFeeDelay == Constansts.FlagActive
								&& (item.DetailStatus == OrderOutboundStatus.InProcess || item.DetailStatus == OrderOutboundStatus.IsBuy) // Chỉ Hủy dc khi ở tình trạng đang xử lý hoặc Đã mua
								)
							{
								// update lại thông tin Đã tính phí
								item.Order.IsCalFeeDelay = null;
								item.Order.AmountCalFeeDelay = 0;
								item.Order.AmountFeeDelay = 0;
								item.Order.CalDateFeeDelay = null;

								isCalFeeDelayAgain = true;
							}
						}

						if (!string.IsNullOrEmpty(status))
						{
							item.DetailStatus = Convert.ToInt32(status);
						}
					}

					// Nếu Hủy đơn hàng thì chạy lại với những món hàng đã tính phí trả chậm
					if (!string.IsNullOrEmpty(status) && OrderOutboundStatus.Cancel == Convert.ToInt32(status) && isCalFeeDelayAgain)
					{
						// Xóa bảng Sys_auto
						var currentDate = DateTime.Now.Date;
						var sysAuto = dbContext.Mst_SysAuto.Where(p => p.CreatedDate == currentDate).OrderByDescending(p => p.CreatedDate).FirstOrDefault();
						if (sysAuto != null)
						{
							NLogLogger.Info(string.Format("Delete sysAuto createDate={0}", currentDate.ToString("yyyy-MM-dd")));
							dbContext.Mst_SysAuto.Remove(sysAuto);
							//dbContext.SaveChanges();
						}
						boolCalFeeOutAgain = true;

						// Để login lại rồi tính
						//// call function cal fee not need transaction
						//var resultCalDelayFeeOrder = this.CalDelayFeeOrder(ref alParamsOutError);
						//if (!resultCalDelayFeeOrder)
						//{
						//    NLogLogger.Info(string.Format("cal fee delay agian with Cancel time ={0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
						//}
					}

					// Insert OO history
					var onHistory = new OrderOutboundHistory
					{
						OrderOutboundId = outId
						, CreatedDate = DateTime.Now
						,
						Remark = string.Format("Update OrderOutbound with status={0}, dateUpdateForStatus={1}", status, dateUpdateForStatus)
					};
					dbContext.OrderOutboundHistories.Add(onHistory);

                    dbContext.SaveChanges();
                    tranScope.Complete();

                    #endregion
                }
				if (boolCalFeeOutAgain)
				{
					// Để login lại rồi tính
					// call function cal fee not need transaction
					var resultCalDelayFeeOrder = this.CalDelayFeeOrder(ref alParamsOutError);
					if (!resultCalDelayFeeOrder)
					{
						NLogLogger.Info(string.Format("cal fee delay agian with Cancel time ={0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
					}
				}
				#region // Return:

				return true;

				#endregion
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

        #region // OrderOutboundDelete
        public bool OrderOutboundDelete(
            string orderOutboundId
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderOutboundDelete";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "orderOutboundId", orderOutboundId
			            });
            #endregion

            try
            {
                using (var ts = new System.Transactions.TransactionScope())
                {
                    using (var dbContext = new EcmsEntities())
                    {
                        #region // Check:
                        int id = Convert.ToInt32(orderOutboundId);
                        var delOrderOutbound = dbContext.OrderOutbounds.SingleOrDefault(x => x.OrderOutboundId == id);

                        if (delOrderOutbound == null)
                        {
                            throw new Exception(string.Format("Đơn hàng không tồn tại"));
                        }
						if (delOrderOutbound.Status != OrderOutboundStatus.InProcess)
						{
							throw new Exception(string.Format("Không được phép xóa đơn hàng này"));
						}
                        #endregion

                        #region // Build:
						var orderDetails = from p in dbContext.OrderDetails
										   join od in dbContext.OrderOutboundDetails on p.OrderDetailId equals od.OrderDetailId
										   where od.OrderOutboundId == id
										   select p;

						// Hoàn lại tình trạng cho các món hàng.
						foreach (var od in orderDetails)
						{
							od.DetailStatus = null;
						}

						// delete customer:
						dbContext.OrderOutbounds. Remove(delOrderOutbound);
						
                        #endregion

                        #region // return:
                        dbContext.SaveChanges();
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

        #region // OrderOutboundChangeStatus
        public bool OrderOutboundChangeStatus(string orderOutboundId, string orderStatus, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderOutboundChangeStatus";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "orderOutboundId",orderOutboundId
                        ,"orderStatus",orderStatus
			            });
            #endregion

            try
            {
                var dbContext = new EcmsEntities();

                #region // Check:
                int oId = Convert.ToInt32(orderOutboundId);
                var orderReturn = dbContext.OrderOutbounds.SingleOrDefault(x => x.OrderOutboundId == oId);

                if (orderReturn == null)
                {
                    throw new Exception("Đơn hàng không tồn tại");
                }
                #endregion

                #region // Build:
                orderReturn.Status = Convert.ToInt32(orderStatus);

                //Lưu lịch sử order

                dbContext.SaveChanges();
                #endregion

                #region // Return:

                return true;

                #endregion
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

		#region // OrderOutboundDetailDelete

		public bool OrderOutboundDetailDelete(string orderOutboundDetaiId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "OrderOutboundDetailDelete";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "orderOutboundDetaiId",orderOutboundDetaiId                        
			            });
            #endregion

            try
            {
                using (var tranScope = new TransactionScope())
                {
                    using (var dbContext = new EcmsEntities())
                    {
                        #region // Check:
                        int ooId = Convert.ToInt32(orderOutboundDetaiId);
                        var orderReturn = dbContext.OrderOutboundDetails.SingleOrDefault(x => x.OrderOutboundDetailId == ooId);

                        if (orderReturn == null)
                        {
                            throw new Exception("Link sản phẩm không tồn tại");
                        }
                        int orderDetailId = orderReturn.OrderDetailId.Value;
                        #endregion

                        #region // Build:

                        dbContext.OrderOutboundDetails.Remove(orderReturn);

						var orderDetail = dbContext.OrderDetails.SingleOrDefault(p => p.OrderDetailId == orderDetailId);
						if (orderDetail != null)
						{
							orderDetail.DetailStatus = null;
						}

                        dbContext.SaveChanges();
                        tranScope.Complete();
                        #endregion

                        #region // Return:
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

		#region // Cal delay fee

		public bool CalDelayFeeOrder(ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "CalDelayFeeOrder";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion

			try
			{
				using (var db = new EcmsEntities())
				{
					#region // Check:
					// 1. Check Có tính phí trả chậm không?
					var configBusiness = db.ConfigBusinesses.Where(x => x.BusinessCode == Const_BusinessCode.Business_301).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
					if (!(configBusiness!=null && (configBusiness.ConfigValue??0)==1))
					{
						return false;
					}

					// 2. Check xem đã tính phí trả chậm lần nào trong ngày chưa?
					var autoSys = db.Mst_SysAuto.OrderByDescending(p => p.CreatedDate).FirstOrDefault();
					if (autoSys != null && autoSys.CreatedDate.Value.Date == DateTime.Now.Date)
					{
						// Đã chạy tính phí trả chậm
						return false;
					}
					#endregion

					#region // Query:
					var configBusinessDayAloowed = db.ConfigBusinesses.Where(x => x.BusinessCode == Const_BusinessCode.Business_302).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
					var dayAllowedCalFee = Convert.ToInt32(configBusinessDayAloowed == null ? Constansts.DayFeeDefault : (configBusinessDayAloowed.ConfigValue ?? Constansts.DayFeeDefault));
					
					//var dateAllowed = DateTime.Now.Date.AddDays(-dayAllowedCalFee);

					var dateAllowed = DateTime.Now.Date;

					var query = from o in db.Orders
								where o.OrderStatus == OrderInStatus.OrderConfirmed
										&& (o.IsCalFeeDelay == null || o.IsCalFeeDelay=="0")// CHỉ lấy những đơn hàng chưa tính phí trả chậm
										&& o.ConfirmDate != null
										//&& o.ConfirmDate.Value < dateAllowed
										&& o.OrderTypeId==2 // Chỉ lấy đơn hàng mua hộ           
										&& EntityFunctions.AddDays(o.ConfirmDate, (o.DayAllowedDelay ?? dayAllowedCalFee)) < dateAllowed
										&& (o.CalFeeDelay ?? Constansts.CalFeeDelay) == Constansts.CalFeeDelay
										//&& o.CustomerId==1 // HungDV
								select new OrderModel()
								{
									OrderId = o.OrderId,
									OrderNo = o.OrderNo,
									OrderOutboundNo = o.OrderOutboundNo,
									TrackingNo = o.TrackingNo,
									CreatedDate = o.CreatedDate,
									OrderDate = o.OrderDate,
									CustomerId = o.CustomerId,
									ContactChannel = o.ContactChannel,
									DeliveryName = o.DeliveryName,
									DeliveryMobile = o.DeliveryMobile,
									DeliveryEmail = o.DeliveryEmail,
									DeliveryAddress = o.DeliveryAddress,
									DeliveryDate = o.DeliveryDate,
									OrderNoDelivery = o.OrderNoDelivery,
									OrderStatus = o.OrderStatus,
									Remark = o.Remark,
									OrderTypeId = o.OrderTypeId,
									IsInsurance = o.IsInsurance,
									NeedDate = o.NeedDate,
									DateToUsa = o.DateToUsa,
									EmployeeCode = o.EmployeeCode,
									ConfirmDate = o.ConfirmDate, // ConfirmDate sau khi add thêm số ngày dc phép tính trả chậm
									DayAllowedDelay = o.DayAllowedDelay,
									CalFeeDelay = o.CalFeeDelay,
									AmountCalFeeDelay = o.AmountCalFeeDelay ?? 0,
									AmountFeeDelay = o.AmountFeeDelay ?? 0,
									// Tỷ giá                                    
									ConfigRateId = o.ConfigRateId,
									ConfigRateValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == o.ConfigRateId).ConfigValue),

									lstOrderDetailModel = (from p in db.OrderDetails
														   where p.OrderId == o.OrderId
														   select new OrderDetailModel
														   {
															   Color = p.Color,
															   EffortConfigId = p.EffortConfigId,
															   EffortConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.EffortConfigId).ConfigValue),
															   EffortModified = p.EffortModified,
															   ImageUrl = p.ImageUrl,
															   OrderDetailId = p.OrderDetailId,
															   OrderId = p.OrderId,
															   PriceWeb = p.PriceWeb,
															   PriceWebOff = p.PriceWebOff,
															   ProductId = p.ProductId,
															   ProductLink = p.ProductLink,
															   Quantity = p.Quantity,
															   ShipConfigId = p.ShipConfigId,
															   ShipConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.ShipConfigId).ConfigValue),
															   ShipModified = p.ShipModified,
															   DeclarePrice = p.DeclarePrice,
															   ShipUSAVN = p.ShipUSAVN,
															   ShipUSA = p.ShipUSA,
															   Size = p.Size,
															   Surcharge = p.Surcharge,
															   TaxUsaConfigId = p.TaxUsaConfigId,
															   TaxUsaConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.TaxUsaConfigId).ConfigValue),
															   InsuaranceConfigId = p.InsuaranceConfigId,
															   InsuaranceConfigValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.InsuaranceConfigId).ConfigValue),
															   WebsiteId = p.WebsiteId,
															   WebsiteName = p.WebsiteLink.WebsiteName,
															   Weight = p.Weight,
															   DetailStatus = p.Order.OrderTypeId == 3 ? (p.DetailStatus == null ? 0 : p.DetailStatus) : p.DetailStatus,
															   CountryId = p.CountryId,
															   OrderTypeId = o.OrderTypeId.Value,
															   CategoryId = p.CategoryId,
															   TrackingNo = p.TrackingNo,
															   DateToUsa = p.DateToUsa,
															   DeliveryVNDate = p.DeliveryVNDate,
															   DeliveryDate = p.DeliveryDate,
															   LotPrice = p.LotPrice,
															   RateCountryValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.RateCountryId).ConfigValue ?? 0),
															   RateUSDValue = (db.ConfigBusinesses.FirstOrDefault(x => x.ConfigBusinessId == p.Order.ConfigRateId).ConfigValue ?? 0),
															   OrderStatus=o.OrderStatus
														   })
														};

					// Nếu số tiền còn lại ko có ship =0 thì sẽ ko tính
					//query = query.Where(p => (p.RemainAmountNoShip ?? 0) > 0 );

					#endregion

					#region // Build:
					// Update Order:

					// Lấy giá trị trả chậm chung:
					var configBusinessAddValue = db.ConfigBusinesses.Where(x => x.BusinessCode == Const_BusinessCode.Business_303).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
					var addValue = configBusinessAddValue == null ? Constansts.AddValueDefault : configBusinessAddValue.ConfigValue.Value;

					foreach (var item in query)
					{
						// Nếu số tiền còn lại ko có ship =0 thì sẽ ko tính
						if (item.RemainAmountNoShip == null || item.RemainAmountNoShip <= 0)
						{
							continue;
						}

						var order = db.Orders.SingleOrDefault(p => p.OrderId == item.OrderId);

						order.AmountCalFeeDelay = Math.Round((item.RemainAmountNoShip ?? 0), Constansts.NumberRoundDefault); // Lưu số tiền còn lại quá hạn cần phải tính phí
						order.AmountFeeDelay = Math.Round(((item.RemainAmountNoShip ?? 0) * (order.FeeDelay ?? addValue) / 100), Constansts.NumberRoundDefault);
						order.CalDateFeeDelay = DateTime.Now;
						order.IsCalFeeDelay = Constansts.FlagActive;
						if (order.DayAllowedDelay == null)
						{
							order.DayAllowedDelay = dayAllowedCalFee;
						}

						if (order.FeeDelay == null)
						{
							order.FeeDelay = addValue;
						}
						order.CalFeeDelay = Constansts.FlagActive;

						// add OrderHistory
						var orderHistory = new OrderHistory()
						{
							CreatedDate = DateTime.Now,
							OrderId = order.OrderId,
							Remark =string.Format("calculate fee delay for order")
						};
						if (order.FeeDelay == null && configBusinessAddValue!=null)
						{
							orderHistory.Remark = string.Format("calculate fee delay for order with add value configId= {0}", configBusinessAddValue.ConfigBusinessId);
						}
						db.OrderHistories.Add(orderHistory);
					}

					// Insert to AutoSys
					var autoSysAdd = new Mst_SysAuto
					{
						CreatedDate = DateTime.Now.Date,
						Remark = string.Format("Calculate fee delay time={0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"))
					};
					db.Mst_SysAuto.Add(autoSysAdd);

					db.SaveChanges();
					//tranScope.Complete();
					#endregion

					#region // Return:
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

		#region // OrderFix BusinessInvoiceCode

		public bool OrderFixBusinessInvoiceCode(ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderFixBusinessInvoiceCode";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion

			try
			{
				using (var db = new EcmsEntities())
				{
					#region // 1. Get:

					var query = from o in db.Orders
								where o.OrderStatus == OrderInStatus.Deliveried
									&& !(from iv in db.Invoices
										 where iv.BusinessCode == Const_BusinessCode.Business_206 && iv.Status == InvoiceStatus.Confirm
										 select new
										 {
											 iv.OrderId
										 }).Contains(new { OrderId = (Int32?)o.OrderId })

									&& (from id in db.InvoiceDetails
										where
										  (new string[] { Const_BusinessCode.Business_201, Const_BusinessCode.Business_208 }).Contains(id.Invoice.BusinessCode) &&
										  id.Invoice.OrderId == o.OrderId &&
										  id.Invoice.CustomerId == o.CustomerId &&
										  id.Invoice.Status == InvoiceStatus.Confirm
										select new
										{
											id.Amount
										}).Sum(p => p.Amount) > 0
								orderby
								  o.OrderDate descending
								select new
								{
									o.OrderId,
									o.OrderNo,
									o.OrderDate,
									o.OrderStatus,
									o.CustomerId,
									o.DeliveryDate
								};

					#endregion

					#region // 2. Check

					#endregion

					#region // 3. create invoice width businesscode =206
					int count = 1;
					List<Invoice> lst = new List<Invoice>();
					foreach (var item in query)
					{
						var invoices = from p in db.InvoiceDetails
									   where (p.Invoice.OrderId == item.OrderId
												&& p.Invoice.CustomerId == item.CustomerId
												&& ((p.Invoice.BusinessCode == Const_BusinessCode.Business_201 && p.Invoice.Status == InvoiceStatus.Confirm)
												|| (p.Invoice.BusinessCode == Const_BusinessCode.Business_208 && p.Invoice.Status == InvoiceStatus.Confirm)))
									   select p;
						if (invoices != null && invoices.Count() > 0)
						{
							var oAmount = Math.Round(invoices.Sum(p => p.Amount ?? 0), Constansts.NumberRoundDefault);
							if (oAmount > 0)
							{
								// Tạo Invoice
								var ivoi = new Invoice
								{
									CreatedDate = DateTime.Now,
									BusinessCode = Const_BusinessCode.Business_206,
									InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss") + Convert.ToString(count),
									InvoiceDate = item.DeliveryDate ?? DateTime.Now,
									Remark = string.Format("MBGN Xác nhận giao hàng cho Khách hàng(CustomerId)={0}, orderNo={1} - XN Hàng loạt", item.CustomerId, item.OrderNo),
									OrderId = item.OrderId,
									CustomerId = item.CustomerId,
									Status = InvoiceStatus.Confirm
								};

								var invoiceDetail = new InvoiceDetail
								{
									Amount = oAmount
								};
								ivoi.InvoiceDetails.Add(invoiceDetail);

								if (db.Invoices.Any(x => x.InvoiceCode == ivoi.InvoiceCode))
								{
									throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
								}
								lst.Add(ivoi);
								//db.Invoices.Add(ivoi);
								
							}
						}
						count++;
						//System.Threading.Thread.Sleep(200);
					}

					foreach (var item in lst)
					{
						db.Invoices.Add(item);
						db.SaveChanges();
					}

					#endregion

					#region // Return:
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

		#region // OrderFixInvoice
		private List<InvoiceDetail> InvoiceDetailFixInvoiceGet(EcmsEntities entities)
		{

			// Lấy danh sách thanh toán có OrderId !=null & status=2 & business=201 để phân bổ lại
			var lstInvoice = from p in entities.InvoiceDetails
							 where p.Invoice.OrderId != null && p.Invoice.Status == InvoiceStatus.Confirm && p.Invoice.BusinessCode == Const_BusinessCode.Business_201
							 //&& p.Invoice.CustomerId == 19 //&& p.Invoice.InvoiceId == 134
							 orderby new { p.Invoice.CustomerId, p.Invoice.CreatedDate }
							 select p;

			return lstInvoice.ToList();

		}

		public bool OrderFixInvoice(ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "OrderFixInvoice";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion

			try
			{
				var db = new EcmsEntities();
				var lstInvoice = this.InvoiceDetailFixInvoiceGet(db);

				while (lstInvoice.Count > 0)
				{
					
					List<Invoice> lstInvoiceNew = new List<Invoice>();
					List<int> lstOrderId = new List<int>();

					var itemInvoice = lstInvoice.FirstOrDefault();
					NLogLogger.Info(string.Format("start while with invoice count={0}, customerCode={1}", lstInvoice.Count, itemInvoice.Invoice.Customer.CustomerCode));
					#region // lấy danh sách Order cần phân bổ lại
					NLogLogger.Info(string.Format("bat dau phan bo lai, invoiceId={0}", itemInvoice.Invoice.InvoiceId));
					var orderList = from p in db.Orders
									where p.CustomerId == itemInvoice.Invoice.CustomerId
											&& (p.OrderStatus == OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished || p.OrderStatus == OrderInStatus.Deliveried)
									orderby p.OrderDate
									select p;

					foreach (var itemOrder in orderList)
					{
						lstOrderId.Add(itemOrder.OrderId);
					}
					#endregion

					#region // Phân bổ thanh toán
					int cout = 0;
					var amountNew = itemInvoice.Amount ?? 0;
					foreach (var item in lstOrderId)
					{
						cout++;
						NLogLogger.Info(string.Format("start create invoice with foreach and orderId={0}, amountNew={1}", item, amountNew));
						// Số tiền
						if (amountNew <= 0)
						{
							break;
						}

						// Tính tổng tiền đơn hàng
						var lstOrderDetail = db.OrderDetails.Where(p => p.OrderId == item);
						double sumOrderAmount = 0;
						foreach (var itemDetail in lstOrderDetail)
						{
							sumOrderAmount += new OrderBiz().GetDetailTotalMoney(itemDetail.OrderDetailId);
						}
						#region // Tính thêm phí trả chậm cho đơn hàng mua hộ
						sumOrderAmount = Math.Round((sumOrderAmount + (lstOrderDetail.FirstOrDefault().Order.AmountFeeDelay ?? 0)), Constansts.NumberRoundDefault);
						#endregion

						NLogLogger.Info(string.Format("sumAmount of Order sumOrderAmount={0}", sumOrderAmount));

						// Tính số tiền đã thanh toán KHÔNG CÓ MÃ 201 - thanh toán cụ thể cho 1 đơn hàng
						var paidOrderAmount = Math.Round((new OrderBiz().GetTotalPayAmountNormal(item)), Constansts.NumberRoundDefault);
						NLogLogger.Info(string.Format("amount paid for Order paidOrderAmount={0}", paidOrderAmount));

						// Check xem có thanh toán cho đơn hàng hay không?
						// Số tiền đã thanh toán cho đơn hàng > tổng tiền đơn hàng => ko thanh toán
						if (paidOrderAmount >= sumOrderAmount)
						{
							continue;
						}

						double detailAmout = 0;
						if (amountNew > sumOrderAmount - paidOrderAmount)
						{
							detailAmout = Math.Round((sumOrderAmount - paidOrderAmount), Constansts.NumberRoundDefault);
							amountNew = Math.Round((amountNew - detailAmout), Constansts.NumberRoundDefault); // Giảm số tiền thanh toán
						}
						else
						{
							detailAmout = amountNew;
							amountNew = 0;
						}
						NLogLogger.Info(string.Format("current amountNew={0}", amountNew));

						// xử lý invoiceDetail trước
						detailAmout = Math.Round(detailAmout, Constansts.NumberRoundDefault);
						if (detailAmout <= 0)
						{
							continue;
						}
						var invoicePaidDetail = new InvoiceDetail
						{
							Amount = detailAmout// Convert.ToDouble(amount)
						};

						var invoicePaid = new Invoice
						{
							OrderId = item
							,
							CustomerId = itemInvoice.Invoice.CustomerId
							,
							CreatedDate = DateTime.Now
							,
							BankId = itemInvoice.Invoice.BankId
							,
							FromAccount = itemInvoice.Invoice.FromAccount
							,
							InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmssfff") + Convert.ToString(cout)
							,
							BusinessCode = Const_BusinessCode.Business_208
							,
							Remark = string.Format("Hệ thống phân bổ thanh toán cho đơn hàng orderId={0} - Phân bổ lại do miss PP cụ thể cho  1 Order", item)
							,
							InvoiceDate = itemInvoice.Invoice.InvoiceDate
							,
							Status = 2
							,
							InvoiceRefId = itemInvoice.Invoice.InvoiceId
						};

						invoicePaid.InvoiceDetails.Add(invoicePaidDetail);
						// Check Mã hóa đơn:
						if (db.Invoices.Any(x => x.InvoiceCode == invoicePaid.InvoiceCode))
						{
							throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
						}
						lstInvoiceNew.Add(invoicePaid);
					}

					// sleep: 200
					System.Threading.Thread.Sleep(1000);
					#endregion

					#region // Excute
					using (var tranScope = new TransactionScope(TransactionScopeOption.Required))
					{
						using (var dbContext = new EcmsEntities())
						{
							var invoice201 = dbContext.Invoices.SingleOrDefault(p => p.InvoiceId == itemInvoice.InvoiceId);
							if (invoice201 == null)
							{
								return false;
							}

							// Kiểm tra và xóa invoice với Businesscode =206
							var order = dbContext.Orders.SingleOrDefault(p => p.OrderId == invoice201.OrderId);
							if (order != null && order.OrderStatus == OrderInStatus.Deliveried)
							{
								var lstInvoice206 = dbContext.Invoices.Where(p => p.OrderId == order.OrderId && p.CustomerId == order.CustomerId 
												&& p.BusinessCode==Const_BusinessCode.Business_206 && p.Status == InvoiceStatus.Confirm
												);
								foreach (var item in lstInvoice206)
								{
									var customer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == item.CustomerId);
									if (customer == null)
									{
										throw new Exception("Tài khoản khách hàng không tồn tại");
									}
									//Cộng giả tiền tk khách hàng
									var amountDecrease=Math.Round((invoice201.InvoiceDetails.FirstOrDefault().Amount ?? 0), Constansts.NumberRoundDefault);
									customer.Balance += amountDecrease;
									customer.LastDateModify = DateTime.Now;

									var customerBalanceHistory = new CustomerBalanceHistory();
									customerBalanceHistory.Balance = customer.Balance;
									customerBalanceHistory.CustomerId = customer.CustomerId;
									customerBalanceHistory.CreatedDate = DateTime.Now;
									dbContext.CustomerBalanceHistories.Add(customerBalanceHistory);

									//dbContext.Invoices.Remove(item);
									var invoiceDetail = dbContext.InvoiceDetails.SingleOrDefault(p => p.InvoiceId == item.InvoiceId);
									if (invoiceDetail!=null)
									{
										invoiceDetail.Amount -= amountDecrease;
									}
									dbContext.SaveChanges();
								}
							}

							// update lại invoice
							invoice201.OrderId = null;
							invoice201.LastDateModify = DateTime.Now;

							int count=0;
							foreach (var item in lstInvoiceNew)
							{
								count++;
								dbContext.Invoices.Add(item);
								dbContext.SaveChanges();

								#region // cập nhật lại số dư
								if (item.Order.OrderStatus == OrderInStatus.Deliveried)
								{
									// cập nhập lại số dư thực tế KH

									var customer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == item.CustomerId);
									if (customer == null)
									{
										throw new Exception("Tài khoản khách hàng không tồn tại");
									}

									// update lại Invoice với businesscode=206
									var invoice = dbContext.InvoiceDetails.Where(p => p.Invoice.OrderId == item.OrderId
													&& p.Invoice.CustomerId == item.CustomerId
													&& p.Invoice.Status == InvoiceStatus.Confirm
													&& p.Invoice.BusinessCode == Const_BusinessCode.Business_206).FirstOrDefault();
									if (invoice != null)
									{
										// update								
										invoice.Amount += Math.Round((item.InvoiceDetails.FirstOrDefault().Amount ?? 0), Constansts.NumberRoundDefault);
										invoice.Invoice.LastDateModify = DateTime.Now;

										//Trừ tiền tk khách hàng
										customer.Balance -= Math.Round((item.InvoiceDetails.FirstOrDefault().Amount ?? 0), Constansts.NumberRoundDefault);
										customer.LastDateModify = DateTime.Now;

										var customerBalanceHistory = new CustomerBalanceHistory();
										customerBalanceHistory.Balance = customer.Balance;
										customerBalanceHistory.CustomerId = customer.CustomerId;
										customerBalanceHistory.CreatedDate = DateTime.Now;

										dbContext.CustomerBalanceHistories.Add(customerBalanceHistory);
									}
									else
									{
										var invoices = from p in dbContext.InvoiceDetails
													   where (p.Invoice.OrderId == item.OrderId
																&& p.Invoice.CustomerId == item.CustomerId
																&& ((p.Invoice.BusinessCode == Const_BusinessCode.Business_201 && p.Invoice.Status == InvoiceStatus.Confirm)
																|| (p.Invoice.BusinessCode == Const_BusinessCode.Business_208 && p.Invoice.Status == InvoiceStatus.Confirm)))
													   select p;

										if (invoices != null && invoices.Count() > 0)
										{
											var oAmount = Math.Round(invoices.Sum(p => p.Amount ?? 0), Constansts.NumberRoundDefault);
											if (oAmount > 0)
											{
												// Tạo Invoice
												var ivoi = new Invoice
												{
													CreatedDate = DateTime.Now,
													BusinessCode = Const_BusinessCode.Business_206,
													InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmssfff") + Convert.ToString(count),
													InvoiceDate = DateTime.Now,
													Remark = string.Format("MBGN Xác nhận giao hàng cho Khách hàng(CustomerId)={0}, xác nhận thêm do phân bổ lại cho order cụ thể", item.CustomerId),
													OrderId = item.OrderId,
													CustomerId = item.CustomerId,
													Status = InvoiceStatus.Confirm
												};

												var invoiceDetail = new InvoiceDetail
												{
													Amount = oAmount
												};
												ivoi.InvoiceDetails.Add(invoiceDetail);

												if (db.Invoices.Any(x => x.InvoiceCode == ivoi.InvoiceCode))
												{
													throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
												}

												//Trừ tiền tk khách hàng
												customer.Balance -= oAmount;
												customer.LastDateModify = DateTime.Now;

												var customerBalanceHistory = new CustomerBalanceHistory();
												customerBalanceHistory.Balance = customer.Balance;
												customerBalanceHistory.CustomerId = customer.CustomerId;
												customerBalanceHistory.CreatedDate = DateTime.Now;
												ivoi.CustomerBalanceHistories.Add(customerBalanceHistory);

												dbContext.Invoices.Add(ivoi);
												dbContext.SaveChanges();
											}
										}
									}
								}
								#endregion
							}
							dbContext.SaveChanges();
						}
						tranScope.Complete();
					}
					#endregion

					lstInvoice = this.InvoiceDetailFixInvoiceGet(db);
				}

				#region // Return:
				return true;
				#endregion
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
	}

    #region // model
    public class OrderModel
	{
		#region // model normal
		public Int32 OrderId { set; get; }
        public string OrderNo { set; get; }
        public string OrderOutboundNo { set; get; }
        public string TrackingNo { set; get; }
        public string OrderNoDelivery { get; set; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? OrderDate { set; get; }
        public Int32? CustomerId { set; get; }
        public string CustomerCode { set; get; }
        public string CustomerName { set; get; }
        public int? CustomerTypeId { get; set; }
        public string UserCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string Mobile { set; get; }
        public string Address { set; get; }
        public string Email { get; set; }
        public DateTime? NeedDate { set; get; }
        public DateTime? DeliveryDate { set; get; }
        public string DeliveryName { set; get; }
        public string DeliveryMobile { set; get; }
        public string DeliveryEmail { set; get; }
        public string DeliveryAddress { set; get; }
        public string ContactChannel { set; get; }
        public string CustomerCodeDelivery { set; get; }
        public Int32? OrderStatus { set; get; }
        public Int32? OrderTypeId { set; get; }
        public string OrderTypeName { set; get; }
        public bool? IsInsurance { set; get; }
        public string Remark { set; get; }
        public DateTime? DateToUsa { get; set; }
        public int? ConfigRateId { get; set; }
        public double? ConfigRateValue { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string CreateUser { get; set; }
		public DateTime? ConfirmDate { get; set; }
		public int? DayAllowedDelay { get; set; }
		public string CalFeeDelay { get; set; } // Có cho phép tính phí trả chậm hay không?
		public string IsCalFeeDelay { get; set; } // Đã tính phí trả chậm chưa?
		public double AmountCalFeeDelay { get; set; }
		public double AmountFeeDelay { get; set; }
		public double? FeeDelay { get; set; }
		#endregion

		#region // Tổng tiền Phí vận chuyển của đơn hàng
		private double? sumFeeShip;
		public double? SumFeeShip
		{
			get
			{

				return lstOrderDetailModel.ToList().Sum(p => p.FeeShip);
			}
			set
			{
				sumFeeShip = value;
			}
		}
		#endregion

		#region // Tổng tiền chưa có phí vận chuyển của đơn hàng: 
		private double? sumFeeNoShip;
		public double? SumFeeNoShip
		{
			get
			{

				return lstOrderDetailModel.ToList().Sum(p => p.FeeNoShip);
			}
			set
			{
				sumFeeNoShip = value;
			}
		}
		#endregion

		#region // Tổng tiền đơn hàng đã có phí vận chuyển
		private double? sumAmount;
        public double? SumAmount
        {
            get
            {
				
                return lstOrderDetailModel.ToList().Sum(p => p.TotalMoney);
            }
            set
            {
                sumAmount = value;
            }
        }
		#endregion

		#region // Tổng cân nặng đơn hàng vận chuyển
		private double? sumWeight;
		public double? SumWeight
		{
			get
			{

				return OrderTypeId == 3 ? (lstOrderDetailModel.ToList().Sum(p => p.Weight ?? 0)) : 0;
			}
			set
			{
				sumWeight = value;
			}
		}
		#endregion

		#region // Tổng tiền đã trả thông thường
		private double totalPayAmountNormal;
		public double TotalPayAmountNormal
		{
			get
			{
				return new OrderBiz().GetTotalPayAmountNormal(this.OrderId);
			}
			set { totalPayAmountNormal = value; }
		}
		#endregion

		#region // Tổng tiền còn lại bao gồm cả phí vận chuyển
		private double? remainAmount;
		public double? RemainAmount
		{
			get
			{
				if (OrderStatus == OrderInStatus.OrderCancel)
				{
					return -(TotalPayAmountNormal);
				}
				else if (this.OrderTypeId == 2)
				{
					return ((SumAmount ?? 0) + AmountFeeDelay - (TotalPayAmountNormal));
				}
				else
				{
					return ((SumAmount ?? 0) + AmountFeeDelay - (TotalPayAmountNormal));
				}
			}
			set
			{
				remainAmount = value;
			}
		}
		#endregion

		#region // Tổng tiền còn lại CHƯA bao gồm cả phí vận chuyển
		private double? remainAmountNoShip;
		public double? RemainAmountNoShip
		{
			get
			{
				if (OrderStatus == OrderInStatus.OrderCancel)
				{
					return -(TotalPayAmountNormal);
				}
				else if (this.OrderTypeId == 2)
				{
					return ((SumFeeNoShip ?? 0) + AmountFeeDelay - (TotalPayAmountNormal));
				}
				else
				{
					return ((SumFeeNoShip ?? 0) + AmountFeeDelay - (TotalPayAmountNormal));
				}
			}
			set
			{
				remainAmountNoShip = value;
			}
		}
		#endregion

		#region // Order status
		private string orderStatusText;
        public string OrderStatusText
        {
            get
            {
                return OrderStatus == 1 ? OrderInStatus.QuotePendingText :
                                                            OrderStatus == 2 ? OrderInStatus.QuoteConfirmedText :
                                                            OrderStatus == 3 ? OrderInStatus.OrderPendingText :
                                                            OrderStatus == 4 ? OrderInStatus.OrderConfirmedText :
                                                            OrderStatus == 5 ? OrderInStatus.OrderCancelText :
                                                            OrderStatus == 6 ? OrderInStatus.FinishedText :
                                                            OrderStatus == 7 ? OrderInStatus.DeliveriedText : "";
            }
            set
            {
                orderStatusText = value;
            }
        }
		#endregion

		#region // detail

		public IQueryable<OrderDetailModel> lstOrderDetailModel
		{
			set;
			get;
		}

		#endregion
	}

    public class OrderDetailModel
    {
        public Int32 OrderDetailId { set; get; }
		public string DetailCode { set; get; }
        public Int32 OrderId { set; get; }
        public string OrderNo { set; get; }
		public Int32? OrderOutboundId { set; get; }
		public string OrderOutboundNo { set; get; }
        public DateTime? OrderDate { get; set; }
        public Int32? ProductId { set; get; }
        public string ProductCode { set; get; }
        public string ProductName { set; get; }
        public double? Quantity { set; get; }
        public double? PriceWeb { set; get; }
        public double? PriceWebOff { set; get; }
        public int? TaxUsaConfigId { set; get; }
        public double? TaxUsaConfigValue { get; set; }
        public int? InsuaranceConfigId { get; set; }
        public double? InsuaranceConfigValue { get; set; }
        public double? DeclarePrice { get; set; }
        public double? ShipModified { get; set; }
        public int? ShipConfigId { get; set; }
        public double? ShipConfigValue { get; set; }
        public double? ShipUSAVN { set; get; }
        public double? ShipUSA { set; get; }
        public double? Surcharge { set; get; }
        public int? EffortConfigId { set; get; }
        public double? EffortConfigValue { get; set; }
        public double? EffortModified { get; set; }
        public string ImageUrl { set; get; }
        public string ProductLink { set; get; }
        public string Size { set; get; }
        public string Color { set; get; }
        public double? Weight { set; get; }
        public Int32? WebsiteId { set; get; }
		public Int32? ParentWebsiteId { set; get; }
		public string ParentWebsiteName { set; get; }
        public string WebsiteName { set; get; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public Int32 OrderTypeId { set; get; }
		public string OrderTypeName { set; get; }
        public Int32? CategoryId { set; get; }
        public string CategoryName { get; set; }
        public Int32? ParentCategoryId { set; get; }
        public string ParentCategoryName { get; set; }
        public string TrackingNo { set; get; }
        public string OrderNoDelivery { get; set; }
        public DateTime? DateToUsa { get; set; }
        public string UserCode { get; set; }
		public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
		public int? CustomerTypeId { get; set; }
        public DateTime? DeliveryVNDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
		public string LotPrice { get; set; }
		public int? RateCountryId { get; set; }
		public double RateCountryValue { get; set; }
		
		public double RateUSDValue { set; get; }
		public Int32? OrderStatus { set; get; }
		public Int32? CPDetailId { set; get; } // In phiếu giao nhận
		public string Shop { get; set; }
		public string EmployeeCode { get; set; }
		public string Remark { get; set; }
		#region // FeeShip
		private double feeShip;
		public double FeeShip
		{
			get
			{
				if (OrderTypeId == 2)
				{
					//var fee = ((Weight ?? 0) * (ShipModified != null ? ShipModified.Value : (ShipConfigValue ?? 0)));
					var fee = ShipModified != null ? ShipModified.Value : (ShipConfigValue ?? 0);
					return fee * (Quantity ?? 0);
					//return 0;
				}
				if (OrderTypeId == 3)
				{
					var fee = ((Weight ?? 0) * (ShipModified != null ? ShipModified.Value : (ShipConfigValue ?? 0)));
					//return fee * RateUSDValue;
					return fee ;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				feeShip=value;
			}
		}
		#endregion

		#region // FeeNoShip
		private double? feeNoShip;
		public double FeeNoShip
		{
			get
			{
				switch (OrderTypeId)
				{
					case 1:
					//Báo giá
					case 2:
						//Đặt hàng Link
						//Nếu link đã hủy
						if (DetailStatus == 3) return 0;

						return (MoneyProduct + (MoneyProduct * (EffortModified == null ? (EffortConfigValue ?? 0) : (EffortModified ?? 0)) * 0.01) + (MoneyProduct * (TaxUsaConfigValue ?? 0) * 0.01) + (Surcharge ?? 0) + (ShipUSA ?? 0)) * RateCountryValue;
							   
					case 3:
						return ((Surcharge ?? 0) + ((DeclarePrice ?? 0) * (InsuaranceConfigValue ?? 0) * 0.01)) * RateUSDValue;
					case 4:
						//Mua trên site
						return MoneyProduct;
					default:
						return 0;
				}
			}
			set
			{
				feeNoShip = value;
			}
		}
		#endregion

		#region // Tổng tiền theo đơn giá và số lượng
		private double moneyProduct;
        public double MoneyProduct
        {
            get
            {
                return ((PriceWebOff ?? 0) == 0 ? (PriceWeb ?? 0) : (PriceWebOff ?? 0)) * (Quantity ?? 0);
            }
            set { moneyProduct = value; }
        }
		#endregion

		#region // Tổng tiền sản phẩm
		private double totalMoney;
		public double TotalMoney
		{
			get
			{
				switch (OrderTypeId)
				{
					case 1:
					//Báo giá
					case 2: //Đặt hàng Link
						//Nếu link đã hủy
						if (DetailStatus == 3) return 0;

						return FeeShip + MoneyProduct * RateCountryValue + (Surcharge ?? 0) * (Quantity ?? 0);

					case 3:
						//Vận chuyển
						//return FeeShip + ((Surcharge ?? 0) + ((DeclarePrice ?? 0) * (InsuaranceConfigValue ?? 0) * 0.01)) * RateUSDValue;
						return FeeShip + (Surcharge ?? 0);
					case 4:
						//Mua trên site
						return MoneyProduct;
					default:
						return 0;
				}
			}
			set
			{
				totalMoney = value;
			}
		}

		#endregion

		#region // Trạng thái sản phẩm
		public int? DetailStatus { set; get; }
        private string detailStatusText;
        public string DetailStatusText
        {
            get
            {
                return DetailStatus == 1 ? OrderOutboundStatus.InProcessText :
                                                            DetailStatus == 2 ? OrderOutboundStatus.IsBuyText :
                                                            DetailStatus == 3 ? OrderOutboundStatus.CancelText :
                                                            DetailStatus == 4 ? OrderOutboundStatus.InvOutboundText :
                                                            DetailStatus == 5 ? OrderOutboundStatus.InvInboundText :
                                                            DetailStatus == 6 ? OrderOutboundStatus.InvInboundMBGNText :
															DetailStatus == 7 ? OrderInStatus.DeliveriedText: // Tình trạng món hàng 
															(OrderTypeId==3 && DetailStatus == 0)?"Thay đổi" :"";
            }
            set
            {
                detailStatusText = value;
            }
        }
		#endregion
	}

    public class OrderOutboundModel
    {
        public Int32 OrderOutboundId { set; get; }
        public string OrderOutboundNo { set; get; }
        public string TrackingNo { set; get; }
        public string OrderNumber { get; set; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? OrderDate { set; get; }
        public Int32? AccountWebsiteId { set; get; }
        public string AccountWebsiteNo { set; get; }
        public Int32? Status { set; get; }
        public string UserCreate { set; get; }
        public string Remark { set; get; }
        public int? WebsiteId { get; set; }
        public int? ParentWebsiteId { get; set; }
        public string WebsiteName { get; set; }
		public int? WebsiteAccountVisaId { get; set; }
		public int? VisaId { get; set; }
		public string VisaNo { get; set; }
        public DateTime? DeliverlyUSADate { set; get; }
        public DateTime? DeliverlyVNDate { set; get; }
        public DateTime? DeliverlyDate { set; get; }

        private double? sumAmount;
        public double? SumAmount
        {
            get
            {
                return lstOrderOutboundDetailModel.ToList().Sum(p => p.OrderDetail.TotalMoney);
            }
            set
            {
                sumAmount = value;
            }
        }

        public IQueryable<OrderOutboundDetailModel> lstOrderOutboundDetailModel
        {
            set;
            get;
        }

        private string statusText;
        public string StatusText
        {
            get
            {
                return Status == 1 ? OrderOutboundStatus.InProcessText :
                                                            Status == 2 ? OrderOutboundStatus.IsBuyText :
                                                            Status == 3 ? OrderOutboundStatus.CancelText :
                                                            Status == 4 ? OrderOutboundStatus.InvOutboundText :
                                                            Status == 5 ? OrderOutboundStatus.InvInboundText :
                                                            Status == 6 ? OrderOutboundStatus.InvInboundMBGNText : "";
            }
            set
            {
                statusText = value;
            }
        }
    }

    public class OrderOutboundDetailModel
    {
        public Int32 OrderOutboundDetailId { set; get; }
        public Int32 OrderOutboundId { set; get; }
        public Int32? OrderDetailId { set; get; }
        public double? Quantity { set; get; }
        public double? Price { set; get; }
        private double totalMoney;
        public double TotalMoney
        {
            get
            {
                return (Quantity ?? 0) * (Price ?? 0);
            }
            set
            {
                totalMoney = value;
            }
        }

        public OrderDetailModel OrderDetail { get; set; }
    }

    #endregion
}
