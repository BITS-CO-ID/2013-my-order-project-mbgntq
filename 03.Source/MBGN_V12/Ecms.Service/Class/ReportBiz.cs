using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using System.Collections;
using CommonUtils;

namespace Ecms.Biz.Class
{
    public class ReportBiz
	{
		#region // Báo cáo tổng hợp tài khoản Nhân viên

		#endregion

		#region // Báo cáo tổng hợp tài khoản Khách hàng

		public List<ReportLiabilityModel> ReportLiabilityGet(
				string fromDate
				, string toDate
				, string employeeCode
				, string customerCode
				, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ReportLiabilityGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
						,"employeeCode",employeeCode
                        ,"customerCode",customerCode
			            });
            #endregion

            try
            {
                var fDate = Convert.ToDateTime(fromDate);
                var tDate = Convert.ToDateTime(toDate);

                using (var dbContext = new EcmsEntities())
                {
					var orders = new OrderBiz().OrderGet(
									""
									, ""
									, ""
									, ""
									, "" //fromDate
									, "" //toDate
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ref alParamsOutError
									).Where(p => p.OrderStatus == OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished || p.OrderStatus == OrderInStatus.Deliveried).ToList();

					var rpt = from c in dbContext.Customers.ToList()
							  join u in dbContext.Sys_User on c.EmployeeCode equals u.UserCode into u_join
							  from u in u_join.DefaultIfEmpty()
							  where c.Status == 0 && c.CustomerCode != Constansts.CustomerAccount_MBGN
							  select new ReportLiabilityModel()
							  {
								  FromDate = fDate,
								  ToDate = tDate,
								  CustomerId = c.CustomerId,
								  CustomerCode = c.CustomerCode,
								  UserCode = c.UserCode,
								  CustomerName = c.CustomerName,
								  EmployeeCode = c.EmployeeCode,
								  EmployeeName = u == null ? "" : u.UserName,
								  BeforeBalance = ((from i in c.Invoices
													where i.InvoiceDate < fDate
														  && i.Status == 2
														  && ((new string[] { Const_BusinessCode.Business_201, Const_BusinessCode.Business_202, Const_BusinessCode.Business_205 }).Contains(i.BusinessCode))
													select i).Sum(x => x.InvoiceDetails.Sum(y => y.Amount)) ?? 0),
								  TotalCharge = ((from i in c.Invoices
												  where i.InvoiceDate >= fDate && i.InvoiceDate <= tDate
														&& i.Status == 2
														&& ((new string[] { Const_BusinessCode.Business_201, Const_BusinessCode.Business_202, Const_BusinessCode.Business_205 }).Contains(i.BusinessCode))
												  select i).Sum(x => x.InvoiceDetails.Sum(y => y.Amount)) ?? 0),
								  Balance = c.Balance ?? 0,
								  BalanceFreeze = ((from i in c.Invoices
													where (i.Order != null && (i.Order.OrderStatus == OrderInStatus.OrderConfirmed || i.Order.OrderStatus == OrderInStatus.Finished))
															&& i.Status == 2
															&& ((new string[] { Const_BusinessCode.Business_207, Const_BusinessCode.Business_208, Const_BusinessCode.Business_209 }).Contains(i.BusinessCode))
													select i).Sum(x => x.InvoiceDetails.Sum(y => y.Amount)) ?? 0),
								  RemainBalance = (from o in orders
												   where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate <= tDate
												   select o.RemainAmount ?? 0).Sum()

							  };

					if (!string.IsNullOrEmpty(employeeCode))
					{
						rpt = rpt.Where(x => x.EmployeeCode == employeeCode);
					}

                    if (!string.IsNullOrEmpty(customerCode))
                    {
						rpt = rpt.Where(x => x.CustomerCode == customerCode || x.UserCode.ToLower() == customerCode.ToLower());
                    }
                    return rpt.ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<ReportLiabilityModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

		#region // Báo cáo chi tiết tài khoản Khách hàng

		public List<InvoiceDetailModel> ReportLiabilityDetailGet(
					string fromDate
					, string toDate
					, string customerId
					, string customerCode
					, string orderId
					, string chargeOrPay
					, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ReportLiabilityDetailGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
						,"customerId",customerId
                        ,"customerCode",customerCode
                        ,"orderId",orderId
                        ,"chargeOrPay",chargeOrPay
			            });
            #endregion
            try
            {
                var fDate = Convert.ToDateTime(fromDate);
                var tDate = Convert.ToDateTime(toDate);

                using (var dbContext = new EcmsEntities())
                {
                    var invoiceDetails = from id in dbContext.InvoiceDetails
                                         where id.Invoice.InvoiceDate.Value >= fDate && id.Invoice.InvoiceDate.Value <= tDate
										 && (id.Invoice.Status == 2 & (id.Invoice.BusinessCode == Const_BusinessCode.Business_201 || id.Invoice.BusinessCode == Const_BusinessCode.Business_202 || id.Invoice.BusinessCode == Const_BusinessCode.Business_205 ))
                                         select new InvoiceDetailModel()
                                         {
                                             InvoiceDetailId = id.InvoiceDetailId,
                                             Amount = id.Amount,
                                             InvoiceId = id.InvoiceId,
                                             OverDuoConfigId = id.OverDueConfigId,
                                             Type = id.Type,
                                             InvoiceModel = new InvoiceModel()
                                                            {
                                                                InvoiceId = id.InvoiceId.Value,
                                                                InvoiceCode = id.Invoice.InvoiceCode,
                                                                InvoiceDate = id.Invoice.InvoiceDate,
                                                                CreatedDate = id.Invoice.CreatedDate,
                                                                CustomerId = id.Invoice.CustomerId,
                                                                CustomerCode = id.Invoice.Customer.CustomerCode,
                                                                CustomerName = id.Invoice.Customer.CustomerName,
                                                                UserCode = id.Invoice.Customer.UserCode,
                                                                OrderId = id.Invoice.OrderId,
                                                                OrderNo = id.Invoice.Order.OrderNo,
                                                                OrderStatus = id.Invoice.Order.OrderStatus,
                                                                BusinessCode = id.Invoice.BusinessCode,
                                                                BankId = id.Invoice.BankId,
                                                                FromAccount = id.Invoice.FromAccount,
                                                                Status = id.Invoice.Status,
                                                                Remark = id.Invoice.Remark
                                                            }
                                         };

					if (!string.IsNullOrEmpty(customerId))
					{
						int cusId = Convert.ToInt32(customerId);
						invoiceDetails = invoiceDetails.Where(p => p.InvoiceModel.CustomerId == cusId);
					}

					if(!string.IsNullOrEmpty(customerCode))
					{
						invoiceDetails = invoiceDetails.Where(p => p.InvoiceModel.CustomerCode == customerCode);
					}

                    return invoiceDetails.OrderByDescending(x=>x.InvoiceModel.CreatedDate).ToList();
                }

            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<InvoiceDetailModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

		#region // Báo cáo tổng hợp công nợ Đơn hàng
		public List<RptOrderDebitModel> ReportOrderDebitGet(
				string fromDate
				, string toDate
				, string employeeCode
				, string customerCode
				, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ReportOrderDebitGet";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
						,"employeeCode",employeeCode
                        ,"customerCode",customerCode
			            });
			#endregion

			try
			{
				var fDate = Convert.ToDateTime(fromDate);
                var tDate = Convert.ToDateTime(toDate);

				using (var dbContext = new EcmsEntities())
				{
					var orders = new OrderBiz().OrderGet(
									""
									, ""
									, ""
									, ""
									, "" //fromDate
									, "" //toDate
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, employeeCode
									, ""
									, ""
									, ""
									, ref alParamsOutError
									).Where(p => p.OrderStatus == OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished || p.OrderStatus == OrderInStatus.Deliveried).ToList();


					var rpt = from c in dbContext.Customers
							  where c.Status == 0 && c.CustomerCode != Constansts.CustomerAccount_MBGN
							  select new RptOrderDebitModel
							  {
								  CustomerId = c.CustomerId,
								  CustomerCode = c.CustomerCode,
								  CustomerName = c.CustomerName,
								  UserCode=c.UserCode
							  };

					if (!string.IsNullOrEmpty(customerCode))
					{
						rpt = rpt.Where(x => x.CustomerCode == customerCode || x.UserCode.ToLower() == customerCode.ToLower());
					}

					var news = (from c in rpt.ToList()
							   select new RptOrderDebitModel
							   {
								   CustomerId = c.CustomerId,
								   CustomerCode = c.CustomerCode,
								   CustomerName = c.CustomerName,
								   UserCode=c.UserCode,
								   EmployeeCode = c.EmployeeCode,
								   EmployeeName = c.EmployeeName,
								   OpenBalance = (from o in orders
												  where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate < fDate
												  select o).Sum(o => o.OrderStatus==OrderInStatus.OrderCancel?0:(o.SumAmount ?? 0)),
								   IncreaseBalance = (from o in orders
													  where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate >= fDate && o.OrderDate <= tDate
													  select o.OrderStatus==OrderInStatus.OrderCancel?0:(o.SumAmount ?? 0)).Sum(),
								   PaidPayBalance = (from o in orders
													 where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate <= tDate
													 select o.TotalPayAmountNormal).Sum(),
								   SumFeeShip = (from o in orders
													where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate <= tDate
													select o.SumFeeShip??0).Sum(),
								   AmountCalFeeDelay = (from o in orders
													where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate <= tDate
														select o.AmountCalFeeDelay).Sum(),
								   AmountFeeDelay = (from o in orders
														where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate <= tDate
													 select o.AmountFeeDelay).Sum(),
								   RemainBalance = (from o in orders
													where (o.CustomerId ?? 0) == c.CustomerId && o.OrderDate <= tDate
													select o.RemainAmount ?? 0).Sum()

							   }).OrderBy(p => p.CustomerId);//.Where(p=>p.AfterBalance!=0).OrderBy(p=>p.CustomerId);

					return news.ToList();
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<RptOrderDebitModel>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}				

		#endregion

		#region // Báo cáo tổng hợp đơn hàng vận chuyển
		
		#endregion

		#region // Báo cáo tổng hợp đơn hàng mua hộ
		
		#endregion

		#region // Báo cáo tổng hợp đơn hàng nước ngoài

		#endregion

		#region // Tình trạng món hàng
		public List<OrderDetailModel> ReportGoodDeliverlyGet(
				   string fromDate
				   , string toDate
				   , string detailCode
				   , string orderNo
				   , string customerCode
				   , string shop
				   , string websiteId
				   , string parentWebsiteId
				   , string status
				   , string orderTypeId
				   , string trackingNo
				   , ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ReportGoodDeliverlyGet";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
						,"detailCode",detailCode
						,"orderNo",orderNo
                        ,"shop",shop
						,"customerCode",customerCode
                        ,"websiteId",websiteId
						,"parentWebsiteId",parentWebsiteId
                        ,"status",status
						,"orderTypeId",orderTypeId
						,"trackingNo",trackingNo
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
								where (ood !=null || p.Order.OrderTypeId ==3) && p.Order.Customer.Status ==0
								orderby new { p.Order.OrderId, p.Order.OrderDate, p.TrackingNo, p.OrderDetailId, } descending
								select new OrderDetailModel
								{
									OrderId=p.OrderId,
									DetailCode=p.DetailCode,
									OrderDetailId=p.OrderDetailId,
									OrderOutboundId = ood.OrderOutbound.OrderOutboundId,
									OrderOutboundNo = ood.OrderOutbound.OrderOutboundNo,
									OrderNo=p.Order.OrderNo,
									DateToUsa = p.DateToUsa,
									DeliveryVNDate = p.DeliveryVNDate,
									DeliveryDate = p.DeliveryDate,
									OrderDate=p.Order.OrderDate,
									DetailStatus = p.Order.OrderTypeId == 3 ? (p.DetailStatus == null ? 0 : p.DetailStatus) : (p.DetailStatus??0),
									//Remark=p.Remark,
									TrackingNo = p.Order.OrderTypeId == 3 ? p.TrackingNo : ood.OrderOutbound.TrackingNo,
									WebsiteId=p.WebsiteId,
									ParentWebsiteId=w.ParentId,
									WebsiteName=w.WebsiteName,
									UserCode = p.Order.Customer.UserCode,
									CustomerCode = p.Order.Customer.CustomerCode,
									CustomerName = p.Order.Customer.CustomerName,
									CustomerTypeId = p.Order.Customer.CustomerTypeId,
									ProductLink=p.ProductLink,
									Weight=p.Weight??0,
									Quantity = p.Quantity??0,
									ProductName = pr == null ? p.ProductName : pr.ProductName,
									CategoryName = cc.CategoryName,
									OrderTypeId=p.Order.OrderTypeId.Value,
									OrderTypeName=p.Order.OrderType.OrderTypeName,
									CPDetailId=rd.CPDetailId,
									Shop=p.Shop
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
						query = query.Where(p => p.CustomerCode == customerCode || p.UserCode.ToLower() == customerCode.ToLower());
					}

					if (!string.IsNullOrEmpty(trackingNo))
					{
						query = query.Where(p => p.TrackingNo.Contains(trackingNo));
					}
					if (!string.IsNullOrEmpty(shop))
					{
						query = query.Where(p => p.Shop.ToLower().Contains(shop.ToLower()));
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

		#region // In xác nhận giao hàng

		#region // Rpt_DeliverlyConfirmPrintCreate
		public Rpt_DeliverlyConfirmPrint Rpt_DeliverlyConfirmPrintCreate(Rpt_DeliverlyConfirmPrint rptPrint, ref string alParamsOutError)
        {
            #region // Temp
			string strFunctionName = "Rpt_DeliverlyConfirmPrintCreate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:


					rptPrint.CreatedDate = DateTime.Now;

                    #endregion

                    #region // Build:

					dbContext.Rpt_DeliverlyConfirmPrint.Add(rptPrint);
                    dbContext.SaveChanges();

                    #endregion

                    #region // Return:

					return rptPrint;

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

		#region // Rpt_DeliverlyConfirmPrintGet
		public List<Rpt_DeliverlyConfirmPrintModel> Rpt_DeliverlyConfirmPrintGet(
				   string fromDate
				   , string toDate
				   , string rptDeliverlyId
				   , string customerId
				   , string customerCode
				   , string createdUser
				   , string deliverlyFullName
				   , ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "Rpt_DeliverlyConfirmPrintGet";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
                        ,"rptDeliverlyId",rptDeliverlyId
						,"customerId",customerId
                        ,"createdUser",createdUser
						,"deliverlyFullName",deliverlyFullName
			            });
			#endregion

			try
			{

				using (var dbContext = new EcmsEntities())
				{
					var query = from p in dbContext.Rpt_DeliverlyConfirmPrint
								join c in dbContext.Customers on p.CustomerId equals c.CustomerId into co
								from c in co.DefaultIfEmpty()
								where c.Status == 0
								select new Rpt_DeliverlyConfirmPrintModel
								{
									RptDeliverlyId = p.RptDeliverlyId,
									CustomerId = p.CustomerId.Value,
									CustomerCode=c.CustomerCode,
									UserCode = c.UserCode,
									Mobile=c.Mobile,
									CustomerName=c.CustomerName,
									CreatedDate = p.CreatedDate,
									CreatedUser = p.CreatedUser,
									DeliverlyFullName = p.DeliverlyFullName,
									DeliverlyMobile = p.DeliverlyMobile,
									DeliverlyPosition = p.DeliverlyPosition,
								};

					if (!string.IsNullOrEmpty(fromDate))
					{
						var date = Convert.ToDateTime(fromDate);
						query = query.Where(p => p.CreatedDate >= date);
					}

					if (!string.IsNullOrEmpty(toDate))
					{
						var date = Convert.ToDateTime(toDate);
						query = query.Where(p => p.CreatedDate <= date);
					}

					if (!string.IsNullOrEmpty(rptDeliverlyId))
					{
						int id = Convert.ToInt32(rptDeliverlyId);
						query = query.Where(p => p.RptDeliverlyId == id);
					}

					if (!string.IsNullOrEmpty(customerId))
					{
						int id = Convert.ToInt32(customerId);
						query = query.Where(p => p.CustomerId == id);
					}

					if (!string.IsNullOrEmpty(deliverlyFullName))
					{
						query = query.Where(p => p.DeliverlyFullName == deliverlyFullName);
					}

					if (!string.IsNullOrEmpty(customerCode))
					{
						query = query.Where(p => p.CustomerCode == customerCode || p.UserCode == customerCode);
					}

					return query.OrderByDescending(p => p.RptDeliverlyId).Take(1000).ToList();
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<Rpt_DeliverlyConfirmPrintModel>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}

		#endregion

		#region // Rpt_DeliverlyConfirmPrintDelete
		public bool Rpt_DeliverlyConfirmPrintDelete(string rptPrintId, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "Rpt_DeliverlyConfirmPrintDelete";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
			#endregion

			try
			{
				using (var dbContext = new EcmsEntities())
				{
					#region // Return:

					int detailId = Convert.ToInt32(rptPrintId);

					var rptPrint=new Rpt_DeliverlyConfirmPrint(){RptDeliverlyId=Convert.ToInt32(rptPrintId)};
					dbContext.Rpt_DeliverlyConfirmPrint.Attach(rptPrint);
					dbContext.Rpt_DeliverlyConfirmPrint.Remove(rptPrint);
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

		#region // Rpt_DeliverlyCheckPrint
		public bool Rpt_DeliverlyCheckPrint(string rptPrintDetailId, ref string alParamsOutError)
        {
            #region // Temp
			string strFunctionName = "Rpt_DeliverlyCheckPrint";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                   #region // Return:
                
					int detailId=Convert.ToInt32(rptPrintDetailId);
					return dbContext.Rpt_DeliverlyCPDetail.Any(p=>p.OrderDetailId==detailId);

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

		#region // Báo cáo nhập xuất tồn
		public List<Rpt_StockOInOut> ReportStockOInOutGet(
				   string fromDate
				   , string toDate
				   , string productId
				   , string productCode
				   , ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ReportStockOInOutGet";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        ,"fromDate",fromDate
                        ,"toDate",toDate
						,"productId",productId
						,"productCode",productCode
			            });
			#endregion

			try
			{
				using (var db = new EcmsEntities())
				{
					var fDate = Convert.ToDateTime(fromDate);
					var tDate = Convert.ToDateTime(toDate);

					var query = from p in db.Products
								join sb in db.Inv_StockBalance on p.ProductId equals sb.ProductId into sb_join
								from sb in sb_join.DefaultIfEmpty()
								select new Rpt_StockOInOut
								{
									ProductId = p.ProductId,
									ProductCode = p.ProductCode,
									ProductName = p.ProductName,
									//StockBalance = sb.Quantity??0,

									// Số lượng đầu kỳ
									BalanceOpenQuantity = ((from sod in db.Inv_StockInOutDetail
															from ig in db.Inv_Goods
															where sod.ProductId == sb.Product.ProductId && ig.ProductId == sod.ProductId
															&& sod.Inv_StockInOut.Type == StockType.StockIn && ig.Status == GoodStatus.GoodInStock && sod.Inv_StockInOut.InOutDate < fDate
															select new
															{
																sod,
																sod.Inv_StockInOut,
																ig
															}).Count()),

									// Giá trị đầu kỳ
									BalanceOpenPrice = ((from sod in db.Inv_StockInOutDetail
														 from ig in db.Inv_Goods
														 where sod.ProductId == sb.Product.ProductId && ig.ProductId == sod.ProductId
														 && sod.Inv_StockInOut.Type == StockType.StockIn && ig.Status == GoodStatus.GoodInStock && sod.Inv_StockInOut.InOutDate < fDate
														 select new
														 {
															 sod.Price
														 }).Sum(x => x.Price) ?? 0),

									// Số lượng nhập
									StockInQuantity = ((from sod in db.Inv_StockInOutDetail
														where sod.ProductId == sb.Product.ProductId && sod.Inv_StockInOut.Type == StockType.StockIn
														&& sod.Inv_StockInOut.InOutDate >= fDate && sod.Inv_StockInOut.InOutDate <= tDate
														select new
														{
															Quantity = sod.Quantity
														}).Sum(x => x.Quantity) ?? 0),

									// Giá trị nhập
									StockInPrice = ((from sod in db.Inv_StockInOutDetail
													 where sod.ProductId == sb.Product.ProductId && sod.Inv_StockInOut.Type == StockType.StockIn
													 && sod.Inv_StockInOut.InOutDate >= fDate && sod.Inv_StockInOut.InOutDate <= tDate
													 select new
													 {
														 amount = (sod.Quantity * sod.Price)
													 }).Sum(x => x.amount) ?? 0),

									// Số lượng xuất
									StockOutQuantity = ((from sod in db.Inv_StockInOutDetail
														 where sod.ProductId == sb.Product.ProductId && sod.Inv_StockInOut.Type == StockType.StockOut
														 && sod.Inv_StockInOut.InOutDate >= fDate && sod.Inv_StockInOut.InOutDate <= tDate
														 select new
														 {
															 sod.Quantity
														 }).Sum(x => x.Quantity) ?? 0),

									// Giá trị xuất
									StockOutPrice = ((from sod in db.Inv_StockInOutDetail
													  where sod.ProductId == sb.Product.ProductId && sod.Inv_StockInOut.Type == StockType.StockOut
													  && sod.Inv_StockInOut.InOutDate >= fDate && sod.Inv_StockInOut.InOutDate <= tDate
													  select new
													  {
														  amount = (sod.Quantity * sod.Price)
													  }).Sum(x => x.amount) ?? 0),

									// Số lượng tồn
									BalanceQuantity = ((from sod in db.Inv_StockInOutDetail
														from ig in db.Inv_Goods
														where sod.ProductId == sb.Product.ProductId && ig.ProductId == sod.ProductId
														&& sod.Inv_StockInOut.Type == StockType.StockIn && ig.Status == GoodStatus.GoodInStock 
														&& sod.Inv_StockInOut.InOutDate >= fDate
														select new
														{
															sod,
															sod.Inv_StockInOut,
															ig
														}).Count()),
									// Giá trị tồn
									BalancePrice = ((from sod in db.Inv_StockInOutDetail
													 from ig in db.Inv_Goods
													 where sod.ProductId == sb.Product.ProductId && ig.ProductId == sod.ProductId
													 && sod.Inv_StockInOut.Type == StockType.StockIn && ig.Status == GoodStatus.GoodInStock && sod.Inv_StockInOut.InOutDate >= fDate
													 select new
													 {
														 sod.Price
													 }).Sum(x => x.Price) ?? 0)
								};

					if (!string.IsNullOrEmpty(productCode))
					{
						query = query.Where(p => p.ProductCode == productCode);
					}

					if (!string.IsNullOrEmpty(productId))
					{
						int id = Convert.ToInt32(productId);
						query = query.Where(p => p.ProductId == id);
					}

					return query.ToList();
				}

			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<Rpt_StockOInOut>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}



		#endregion
	}

    #region //Models
	public class ReportEmployeeModel
	{
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		//Số dư đầu kỳ
		public double? BeforeBalance { get; set; }
		//Số dư cuối kỳ
		private double? afterBalance;
		public double? AfterBalance
		{
			get
			{
				return (BeforeBalance + TotalCharge);
			}
			set
			{
				afterBalance = value;
			}
		}

		//Số tiền nạp + phát sinh trong kỳ
		public double? TotalCharge { get; set; }

		//Số dư hiện tại
		public double Balance { get; set; }

		//Số dư hiện tại
		public double BalanceFreeze { get; set; }

		//Số dư khả dụng
		private double balanceAvaiable;
		public double BalanceAvaiable
		{
			get
			{
				return (Balance - BalanceFreeze);
			}
			set
			{
				balanceAvaiable = value;
			}
		}

		// công nợ ĐH
		public double RemainBalance { get; set; }

		// công nợ phải thu
		private double remainBalanceReceivable;
		public double RemainBalanceReceivable
		{
			get
			{
				// - số dư khả dụng - công nợ đơn hàng
				var remain = -(BalanceAvaiable - RemainBalance);
				return remain;//remain >= 0 ? remain : 0;
			}
			set
			{
				remainBalanceReceivable = value;
			}
		}
	}

    public class ReportLiabilityModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
		public string UserCode { get; set; }
        public double? FactBalance { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
        //Số dư đầu kỳ
        public double? BeforeBalance { get; set; }

        //Số dư cuối kỳ
        private double? afterBalance;
        public double? AfterBalance
        {
            get
            {
                return (BeforeBalance + TotalCharge);
            }
            set
            {
                afterBalance = value;
            }
        }

        //Số tiền nạp + phát sinh tăng trong kỳ
        public double? TotalCharge { get; set; }

        // Số dư hiện tại
		public double Balance { get; set; }

		// Số dư hiện đóng băng
		public double BalanceFreeze { get; set; }

		// Số dư hiện khả dụng
		private double balanceAvaiable;
		public double BalanceAvaiable
		{
			get
			{
				return (Balance - BalanceFreeze);
			}
			set
			{
				balanceAvaiable = value;
			}
		}
		
		// Công nợ đơn hàng còn lại
		public double RemainBalance { get; set; }

		// Công nợ phải thu
		private double remainBalanceReceivable;
		public double RemainBalanceReceivable
		{
			get
			{
				// - số dư khả dụng - công nợ đơn hàng
				var remain = -(BalanceAvaiable - RemainBalance);
				return remain;//remain >= 0 ? remain : 0;
			}
			set
			{
				remainBalanceReceivable = value;
			}
		}
        
    }

	public class RptOrderDebitModel
	{
		public int CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerCode { get; set; }
		public string UserCode { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		//Số dư đầu kỳ
		public double OpenBalance { get; set; }
		// Phát sinh trong kỳ
		public double IncreaseBalance { get; set; }
		// Đã thanh toán
		public double PaidPayBalance { get; set; }
		// Phí trả chậm
		public double SumFeeShip { get; set; }
		// Phí trả chậm
		public double AmountFeeDelay { get; set; }
		// Tổng tiền tính phí trả chậm
		public double AmountCalFeeDelay { get; set; }
		//Số dư cuối kỳ
		private double? afterBalance;
		public double? AfterBalance
		{
			get
			{
				return (OpenBalance + IncreaseBalance);
			}
			set
			{
				afterBalance = value;
			}
		}

		// Công nợ đơn hàng còn lại
		public double RemainBalance { get; set; }		
	}

	public class RptOrderDebitDetailModel
	{
		public int OrderStatus { get; set; }
		public int OrderId { get; set; }
		public int CustomerId { get; set; }
		public DateTime? OrderDate { get; set; }
		public string OrderNo { get; set; }
		public string OrderStatusText { get; set; }
		public double SumAmount { get; set; }
		public double TotalPayAmountNormal { get; set; }
		public double RemainAmount { get; set; }
		public double AmountFeeDelay { get; set; }
		public double AmountCalFeeDelay { get; set; }
		public double SumFeeShip { get; set; }
	}

	public class RptOrderTransportModel
	{
		public int OrderId { get; set; }
		public string OrderNo { get; set; }
		public string CustomerCode { get; set; }
		public string CustomerName { get; set; }
		public DateTime? OrderDate { get; set; }
		public string DeliveryAddress { get; set; }
		public double SumAmount { get; set; }
		public double PayAmount { get; set; }
		public double AmountFeeDelay { get; set; }
		public int? OrderStatus { get; set; }
		public string Remark { get; set; }
		public string StatusText { get; set; }
		public DateTime? DeliverlyVNDate { get; set; }
		public DateTime? DeliverlyUSADate { get; set; }
		public DateTime? DeliverlyDate { get; set; }
		public double SumWeight { get; set; }
	}

	public class RptOrderBuyModel
	{
		public int OrderId { get; set; }
		public string OrderNo { get; set; }
		public string TrackingNo { get; set; }
		public string CustomerCode { get; set; }
		public string CustomerName { get; set; }
		public DateTime? OrderDate { get; set; }
		public string DeliveryAddress { get; set; }
		public double SumAmount { get; set; }
		public double PayAmount { get; set; }
		public double AmountFeeDelay { get; set; }
		public double RemainAmount { get; set; }
		public int? OrderStatus { get; set; }
		public string Remark { get; set; }
		public string StatusText { get; set; }
	}

	public class Rpt_DeliverlyConfirmPrintModel
	{
		public int RptDeliverlyId { get; set; }
		public int CustomerId { get; set; }
		public string CustomerCode { get; set; }
		public string UserCode { get; set; }
		public string Mobile { get; set; }
		public string CustomerName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedUser { get; set; }
		public string DeliverlyFullName { get; set; }
		public string DeliverlyPosition { get; set; }
		public string DeliverlyMobile { get; set; }
		public string Remark { get; set; }		
	}

	public class Rpt_StockOInOut
	{
		public int ProductId { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }

		public double BalanceOpenQuantity { get; set; }
		public double BalanceOpenPrice { get; set; }

		public double StockInQuantity { get; set; }
		public double StockInPrice { get; set; }

		public double StockOutQuantity { get; set; }
		public double StockOutPrice { get; set; }

		public double BalanceQuantity { get; set; }
		public double BalancePrice { get; set; }
		
	}

    #endregion
}
