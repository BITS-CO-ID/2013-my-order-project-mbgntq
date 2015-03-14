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

namespace Ecms.Biz
{
    public class InvoiceBiz : IInvoiceBiz
    {

        #region // InvoiceGet
        public List<InvoiceModel> InvoiceGet(
                string invoiceId
                , string invoiceCode
                , string invoiceDateFrom
                , string invoiceDateTo
                , string customerId
				, string customerCode
                , string subCustomerId
                , string status
				, string orderId
                , string orderCode
                , string businessCode
				, string invoiceRefId
                , string isGetDetail
                , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "InvoiceGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
                        "invoiceId",invoiceId,
                        "invoiceNo",invoiceCode,
                        "invoiceDateFrom",invoiceDateFrom,
                        "invoiceDateTo",invoiceDateTo,
                        "customerId",customerId,
						"customerCode",customerCode,
						"subCustomerId",subCustomerId,
						"status",status,
						"orderId",orderId,
						"orderCode",orderCode,
						"businessCode",businessCode,
						"invoiceRefId",invoiceRefId,
						"isGetDetail",isGetDetail
			            });
            #endregion

            try
            {

                using (var db = new EcmsEntities())
                {
                    #region // Query:
                    var query = from o in db.Invoices
                                join b in db.Mst_Bank on o.BankId equals b.BankId into b_join
								from b in b_join.DefaultIfEmpty()
								join of in db.Invoices on o.InvoiceRefId equals of.InvoiceId into of_join
                                from of in of_join.DefaultIfEmpty()
                                select new InvoiceModel()
                                {
                                    BusinessCode = o.BusinessCode,
                                    CreatedDate = o.CreatedDate,
                                    CustomerId = o.CustomerId,
                                    CustomerCode = o.Customer.CustomerCode,
									CustomerName = o.Customer.CustomerName,
                                    InvoiceCode = o.InvoiceCode,
                                    InvoiceDate = o.InvoiceDate,
                                    InvoiceId = o.InvoiceId,
                                    BankId = o.BankId,
                                    BankName = b.BankName,
                                    OrderId = o.OrderId,
									OrderNo = o.Order.OrderNo,
									OrderStatus = o.Order.OrderStatus,
                                    Remark = o.Remark,
                                    SubCustomerId = o.SubCustomerId,
                                    Status = o.Status,
                                    FromAccount = o.FromAccount,
									UserCode = o.Customer.UserCode,
									ReplyContent=o.ReplyContent,
									InvoiceRefId=o.InvoiceRefId,
									InvoiceRefCode=of.InvoiceCode,
                                    lstInvoiceDetailModel = (from p in o.InvoiceDetails.AsQueryable()
																 select new InvoiceDetailModel
																 {
																	 Amount = p.Amount,
																	 InvoiceDetailId = p.InvoiceDetailId,
																	 InvoiceId = p.InvoiceId,
																 }
                                                            )
                                };



                    #endregion

                    #region // Filter:
                    if (!string.IsNullOrEmpty(invoiceId))
                    {
                        var id = Convert.ToInt32(invoiceId);
                        query = query.Where(x => x.InvoiceId == id);
                    }

                    if (!string.IsNullOrEmpty(invoiceCode))
                    {
						query = query.Where(x => x.InvoiceCode == invoiceCode || x.OrderNo == invoiceCode);
                    }

                    if (!string.IsNullOrEmpty(customerId))
                    {
                        var id = Convert.ToInt32(customerId);
                        query = query.Where(x => x.CustomerId == id);
                    }

                    if (!string.IsNullOrEmpty(subCustomerId))
                    {
                        var id = Convert.ToInt32(subCustomerId);
                        query = query.Where(x => x.SubCustomerId == id);
                    }

					if (!string.IsNullOrEmpty(customerCode))
					{
						query = query.Where(x => x.CustomerCode == customerCode || x.UserCode ==customerCode);
					}

					if (!string.IsNullOrEmpty(orderId))
					{
						var id = Convert.ToInt32(orderId);
						query = query.Where(x => x.OrderId == id);
					}

                    if (!string.IsNullOrEmpty(orderCode))
					{
                        query = query.Where(x => x.OrderNo == orderCode);
                    }

                    if (!string.IsNullOrEmpty(invoiceDateFrom))
                    {
                        var dateFilter = Convert.ToDateTime(invoiceDateFrom);
                        query = query.Where(x => x.InvoiceDate >= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(invoiceDateTo))
                    {
                        var dateFilter = Convert.ToDateTime(invoiceDateTo);
                        query = query.Where(x => x.InvoiceDate <= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(businessCode))
                    {
                        query = query.Where(x => x.BusinessCode == businessCode);
                    }

					if (!string.IsNullOrEmpty(status))
					{
						var id = Convert.ToInt32(status);
						query = query.Where(x => x.Status == id);
					}

					if (!string.IsNullOrEmpty(invoiceRefId))
					{
						var id = Convert.ToInt32(invoiceRefId);
						query = query.Where(x => x.InvoiceRefId == id);
					}

                    #endregion

                    #region // return:
                    return query.OrderByDescending(x => x.InvoiceId).ToList();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<InvoiceModel>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // InvoiceCreate
        public Invoice InvoiceCreate(Invoice invoice, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "InvoiceCreate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    #region // Check:


                    invoice.InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss");
                    invoice.InvoiceDate = invoice.CreatedDate = DateTime.Now;
                    invoice.Status = 1;

                    var invoiceValidate = dbContext.Invoices.FirstOrDefault(x => x.InvoiceCode == invoice.InvoiceCode);
                    if (invoiceValidate != null)
                    {
                        throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
                    }

                    #endregion

                    #region // Build:

                    dbContext.Invoices.Add(invoice);
                    dbContext.SaveChanges();

                    #endregion

                    #region // Return:

                    return invoice;

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

        #region // InvoiceCreateBackend
        public bool InvoiceCreateBackend(
				string customerId
				, string bankId
				, string fromAccount
				, string orderId // orderNo
				, string amount
				, string remark
				, string bussinessCode
				, string createdUserId
				, string paymentDate
			, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "InvoiceCreateBackend";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"customerId", customerId
						, "bankId", bankId
						, "fromAccount", fromAccount
						, "orderId", orderId
						, "amount", amount
						, "remark", remark
						, "bussinessCode", bussinessCode
						, "createdUserId", createdUserId
						, "paymentDate", paymentDate
			            });
            #endregion

            try
            {
				//using (var tranScope = new TransactionScope(TransactionScopeOption.Required))
				//{
					using (var dbContext = new EcmsEntities())
					{
						#region // Check:

						List<int> lstOrderId = new List<int>();
						var cusId = Convert.ToInt32(customerId);
						double amountNew = Convert.ToInt32(amount);
						// Lấy số dư khả dụng hiện tại của KH
						var customer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == cusId);
						if (customer == null)
						{
							throw new Exception("Tài khoản khách hàng không tồn tại");
						}

						// Nếu không nhập orderId thì sẽ phân bổ số tiền cho các đơn hàng đang chờ phân bổ:
						if (string.IsNullOrEmpty(orderId))
						{
							//var invoiceBalances = from p in dbContext.Invoices
							//                      where p.OrderId != null
							//                      && (p.Order.OrderStatus == OrderInStatus.OrderConfirmed || p.Order.OrderStatus == OrderInStatus.OrderCancel)
							//                      && p.Status == 2
							//                      && p.CustomerId == cusId
							//                      select p;
							// double balanceFreeze = (customer.Balance ?? 0) - invoiceBalances.Sum(p => p.InvoiceDetails.Sum(x => x.Amount ?? 0));

							// Lấy danh sách các đơn hàng cần thanh toán
							// Chỉ lấy những đơn hàng của khách hàng đó ở tình trạng "Đã xác nhận"
							// Sắp xếp những Order xử lý phân bổ cho những order cũ trước.
							var orderList = from p in dbContext.Orders
											where p.CustomerId == cusId && p.OrderStatus == OrderInStatus.OrderConfirmed
											orderby p.OrderDate
											select p;

							foreach (var item in orderList)
							{
								lstOrderId.Add(item.OrderId);
							}
						}
						else
						{
							NLogLogger.Info(string.Format("With orderId={0}", orderId));
							lstOrderId.Add(Convert.ToInt32(orderId)); // Nếu có orderId cụ thể
						}
						#endregion

						#region // Build:
						//List<Invoice> lstInvoice = new List<Invoice>();
						int cout = 0;
						foreach (var item in lstOrderId)
						{
							cout++;
							NLogLogger.Info(string.Format("start create invoice with foreach and orderId={0}, amountNew={1}", orderId, amountNew));
							// Số tiền
							if (amountNew <= 0)
							{
								break;
							}
							Invoice invoice = new Invoice();
							// Tính tổng tiền đơn hàng
							var lstOrderDetail = dbContext.OrderDetails.Where(p => p.OrderId == item);
							double sumOrderAmount = 0;
							foreach (var itemDetail in lstOrderDetail)
							{
								sumOrderAmount += new OrderBiz().GetDetailTotalMoney(itemDetail.OrderDetailId);
							}
							NLogLogger.Info(string.Format("Tổng tiền đơn hàng sumOrderAmount={0}", sumOrderAmount));

							// Tính số tiền đã thanh toán
							var paidOrderAmount = new OrderBiz().GetTotalPayAmountNormal(item);
							NLogLogger.Info(string.Format("Số tiền đã thanh toán paidOrderAmount={0}", paidOrderAmount));

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
								amountNew -= (sumOrderAmount - paidOrderAmount);
							}
							else
							{
								detailAmout = Math.Round(amountNew, Constansts.NumberRoundDefault);
								amountNew = 0;
							}
							NLogLogger.Info(string.Format("current amountNew", amountNew));

							// xử lý invoiceDetail trước
							var invoiceDetail = new InvoiceDetail
							{
								Amount = detailAmout// Convert.ToDouble(amount)
							};
							// Giảm số tiền đã thanh toán cho từng đơn hàng

							invoice.InvoiceDetails.Add(invoiceDetail);
							invoice.OrderId = item;
							if (!string.IsNullOrEmpty(paymentDate))
							{
								invoice.CreatedDate = Convert.ToDateTime(paymentDate);
							}
							invoice.BusinessCode = bussinessCode;
							
							if (cout > 1)
							{
								invoice.InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss")+Convert.ToString(cout);	
							}
							else
							{
								invoice.InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss");	
							}
							
							invoice.InvoiceDate = invoice.CreatedDate = DateTime.Now;
							invoice.Status = 2;
							if (bussinessCode.Equals(Const_BusinessCode.Business_201))
							{
								invoice.BankId = Convert.ToInt32(bankId);
								invoice.FromAccount = fromAccount;
							}
							invoice.CustomerId = Convert.ToInt32(customerId);
							invoice.Remark = remark;

							// Check Mã hóa đơn:
							if (dbContext.Invoices.Any(x => x.InvoiceCode == invoice.InvoiceCode))
							{
								throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
							}

							var currentDate = DateTime.Now;

							#region // Xác nhận thanh toán cho khách hàng
							if (invoice.BusinessCode == "201")
							{
								//Check tài khoản chủ muabangiaonhan
								var accountMaster = dbContext.Customers.SingleOrDefault(x => x.CustomerCode == Constansts.CustomerAccount_MBGN);
								if (accountMaster == null)
								{
									throw new Exception("Tài khoản mua bán giao nhận không tồn tại");
								}

								//Cộng tiền vào tk khách hàng
								customer.Balance += Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)),2);
								customer.LastDateModify = currentDate;

								//Cộng tiền vào tk muabangiaonhan
								accountMaster.Balance += Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)),2);
								accountMaster.LastDateModify = currentDate;

								//Lưu lịch sử số dư khách hàng
								var customerBalanceHistory = new CustomerBalanceHistory();
								customerBalanceHistory.Balance = customer.Balance;
								customerBalanceHistory.CustomerId = customer.CustomerId;
								customerBalanceHistory.CreatedDate = currentDate;
								invoice.CustomerBalanceHistories.Add(customerBalanceHistory);

								//Lưu lịch sử số dư mua bán giao nhận
								var customerBalanceHistoryMaster = new CustomerBalanceHistory();
								customerBalanceHistoryMaster.Balance = accountMaster.Balance;
								customerBalanceHistoryMaster.CustomerId = accountMaster.CustomerId;
								customerBalanceHistoryMaster.CreatedDate = currentDate;
								invoice.CustomerBalanceHistories.Add(customerBalanceHistoryMaster);
							}
							#endregion

							#region // Hoàn tiền cho khách hàng
							if (invoice.BusinessCode == "202")
							{
								//Check tài khoản chủ muabangiaonhan
								var accountMaster = dbContext.Customers.FirstOrDefault(x => x.CustomerCode == Constansts.CustomerAccount_MBGN);
								if (accountMaster == null)
									throw new Exception("Tài khoản mua bán giao nhận không tồn tại");

								//Cộng tiền vào tk khách hàng
								customer.Balance += Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)),2);
								customer.LastDateModify = currentDate;

								//Cộng tiền vào tk muabangiaonhan
								accountMaster.Balance -= Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)),2);
								accountMaster.LastDateModify = currentDate;

								//Lưu lịch sử số dư khách hàng
								var customerBalanceHistory = new CustomerBalanceHistory();
								customerBalanceHistory.Balance = customer.Balance;
								customerBalanceHistory.CustomerId = customer.CustomerId;
								customerBalanceHistory.CreatedDate = currentDate;
								invoice.CustomerBalanceHistories.Add(customerBalanceHistory);

								//Lưu lịch sử số dư mua bán giao nhận
								var customerBalanceHistoryMaster = new CustomerBalanceHistory();
								customerBalanceHistoryMaster.Balance = accountMaster.Balance;
								customerBalanceHistoryMaster.CustomerId = accountMaster.CustomerId;
								customerBalanceHistoryMaster.CreatedDate = currentDate;
								invoice.CustomerBalanceHistories.Add(customerBalanceHistoryMaster);


							}
							#endregion

							#region // Tạo hóa đơn cho partner mua hàng
							if (invoice.BusinessCode == "203")
							{
								//Check tài khoản chủ muabangiaonhan
								var accountMaster = dbContext.Customers.FirstOrDefault(x => x.CustomerCode == Constansts.CustomerAccount_MBGN);
								if (accountMaster == null)
									throw new Exception("Tài khoản mua bán giao nhận không tồn tại");

								//Cộng tiền vào tk muabangiaonhan
								accountMaster.Balance -= Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)),2);
								accountMaster.LastDateModify = currentDate;

								//Lưu lịch sử số dư mua bán giao nhận
								var customerBalanceHistoryMaster = new CustomerBalanceHistory();
								customerBalanceHistoryMaster.Balance = accountMaster.Balance;
								customerBalanceHistoryMaster.CustomerId = accountMaster.CustomerId;
								customerBalanceHistoryMaster.CreatedDate = currentDate;
								customerBalanceHistoryMaster.InvoiceId = invoice.InvoiceId;
								invoice.CustomerBalanceHistories.Add(customerBalanceHistoryMaster);
							}
							#endregion

							#region // Excute
							//lstInvoice.Add(invoice);
							dbContext.Invoices.Add(invoice);
							dbContext.SaveChanges();	
							#endregion
						}
						
						
						//tranScope.Complete();
						#endregion
					//}
                   

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

		#region // InvoiceConfirmPayment
		public bool InvoiceConfirmPayment(
				string customerId
				, string bankId
				, string fromAccount
				, string orderId // orderNo
				, string amount
				, string remark
				, string bussinessCode
				, string createdUserId
				, string paymentDate
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "InvoiceConfirmPayment";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"customerId", customerId
						, "bankId", bankId
						, "fromAccount", fromAccount
						, "orderId", orderId
						, "amount", amount
						, "remark", remark
						, "bussinessCode", bussinessCode
						, "createdUserId", createdUserId
						, "paymentDate", paymentDate
			            });
			#endregion

			try
			{
				#region // PreBuild
				List<Invoice> lstInvoice = new List<Invoice>();
				using (var dbContext = new EcmsEntities())
				{
					#region // Check:

					List<int> lstOrderId = new List<int>();
					var cusId = Convert.ToInt32(customerId);
					double amountNew = Math.Round(Convert.ToDouble(amount),Constansts.NumberRoundDefault);
					// Lấy số dư khả dụng hiện tại của KH
					// Nếu không nhập orderId thì sẽ phân bổ số tiền cho các đơn hàng đang chờ phân bổ:
					if (string.IsNullOrEmpty(orderId))
					{
						// Lấy danh sách các đơn hàng cần thanh toán
						// Chỉ lấy những đơn hàng của khách hàng đó ở tình trạng "Đã xác nhận"
						// Sắp xếp những Order xử lý phân bổ cho những order cũ trước.
						var orderList = from p in dbContext.Orders
										where p.CustomerId == cusId && (p.OrderStatus == OrderInStatus.OrderConfirmed 
																		|| p.OrderStatus == OrderInStatus.Finished 
																		//|| p.OrderStatus == OrderInStatus.Deliveried
																		)
										orderby p.OrderDate
										select p;

						foreach (var item in orderList)
						{
							lstOrderId.Add(item.OrderId);
						}
					}
					else
					{
						NLogLogger.Info(string.Format("With orderId={0}", orderId));
						//lstOrderId.Add(Convert.ToInt32(orderId)); // Nếu có orderId cụ thể
					}
					#endregion

					#region // Build:

					#region // Insert to BusinessCode 201
					Invoice invoice = new Invoice();

					if (!string.IsNullOrEmpty(orderId))
					{
						invoice.OrderId = Convert.ToInt32(orderId);
					}
					if (!string.IsNullOrEmpty(paymentDate))
					{
						invoice.CreatedDate = Convert.ToDateTime(paymentDate);
					}
					invoice.BusinessCode = bussinessCode;
					invoice.InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss");
					invoice.InvoiceDate = invoice.CreatedDate = DateTime.Now;
					invoice.Status = 2;
					if (bussinessCode.Equals(Const_BusinessCode.Business_201))
					{
						invoice.BankId = Convert.ToInt32(bankId);
						invoice.FromAccount = fromAccount;
					}
					invoice.CustomerId = Convert.ToInt32(customerId);
					invoice.Remark = remark;

					var invoiceDetail = new InvoiceDetail
					{
						Amount =Math.Round(Convert.ToDouble(amount),Constansts.NumberRoundDefault)
					};

					invoice.InvoiceDetails.Add(invoiceDetail);

					#region // Xác nhận thanh toán cho khách hàng
					var currentDate = DateTime.Now;
					// Check Mã hóa đơn:
					if (dbContext.Invoices.Any(x => x.InvoiceCode == invoice.InvoiceCode))
					{
						throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
					}

					NLogLogger.Info(string.Format("create invoice normal with orderId={0}, amount={1}", orderId, amount));

					lstInvoice.Add(invoice);
					#endregion

					#endregion

					#region // Phân bổ thanh toán
					int cout = 0;
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
						var lstOrderDetail = dbContext.OrderDetails.Where(p => p.OrderId == item);
						double sumOrderAmount = 0;
						foreach (var itemDetail in lstOrderDetail)
						{
							sumOrderAmount += new OrderBiz().GetDetailTotalMoney(itemDetail.OrderDetailId);
						}
						#region // Tính thêm phí trả chậm cho đơn hàng mua hộ
						sumOrderAmount = Math.Round((sumOrderAmount + (lstOrderDetail.FirstOrDefault().Order.AmountFeeDelay ?? 0)),Constansts.NumberRoundDefault);
						#endregion

						NLogLogger.Info(string.Format("sumAmount of Order sumOrderAmount={0}", sumOrderAmount));

						// Tính số tiền đã thanh toán
						var paidOrderAmount = Math.Round((new OrderBiz().GetTotalPayAmountNormal(item)),Constansts.NumberRoundDefault);
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
							detailAmout = Math.Round((sumOrderAmount - paidOrderAmount),Constansts.NumberRoundDefault);
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
						if(detailAmout <=0)
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
							CustomerId = invoice.CustomerId
							,
							CreatedDate = DateTime.Now
							,
							BankId = invoice.BankId
							,
							FromAccount = invoice.FromAccount
							,
							InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss") + Convert.ToString(cout)
							,
							BusinessCode = Const_BusinessCode.Business_208
							,
							Remark = string.Format("Hệ thống phân bổ thanh toán cho đơn hàng orderId={0}", item)
							,
							InvoiceDate = invoice.InvoiceDate
							,
							Status = 2
							, 
							InvoiceRefId = invoice.InvoiceId
						};
						
						invoicePaid.InvoiceDetails.Add(invoicePaidDetail);
						// Check Mã hóa đơn:
						if (dbContext.Invoices.Any(x => x.InvoiceCode == invoicePaid.InvoiceCode))
						{
							throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
						}
						lstInvoice.Add(invoicePaid);						
					}
					#endregion

					#endregion
				}
				#endregion

				#region // Excute
				using (var tranScope = new TransactionScope(TransactionScopeOption.Required))
				{
					using (var dbContext = new EcmsEntities())
					{
						var invoice = lstInvoice.SingleOrDefault(p => p.BusinessCode == Const_BusinessCode.Business_201);
						
						// TK KH
						var cusId = Convert.ToInt32(customerId);
						var customer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == cusId);
						if (customer == null)
						{
							throw new Exception("Tài khoản khách hàng không tồn tại");
						}

						//Check tài khoản chủ muabangiaonhan
						var accountMaster = dbContext.Customers.SingleOrDefault(x => x.CustomerCode == Constansts.CustomerAccount_MBGN);
						if (accountMaster == null)
						{
							throw new Exception("Tài khoản mua bán giao nhận không tồn tại");
						}

						//Cộng tiền vào tk khách hàng
						customer.Balance += Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)), Constansts.NumberRoundDefault);
						customer.LastDateModify = DateTime.Now;

						//Cộng tiền vào tk muabangiaonhan
						accountMaster.Balance += Math.Round(invoice.InvoiceDetails.Sum(x => (x.Amount ?? 0)), Constansts.NumberRoundDefault);
						accountMaster.LastDateModify = DateTime.Now;

						//Lưu lịch sử số dư khách hàng
						var customerBalanceHistory = new CustomerBalanceHistory();
						customerBalanceHistory.Balance = customer.Balance;
						customerBalanceHistory.CustomerId = customer.CustomerId;
						customerBalanceHistory.CreatedDate = DateTime.Now;
						invoice.CustomerBalanceHistories.Add(customerBalanceHistory);

						//Lưu lịch sử số dư mua bán giao nhận
						var customerBalanceHistoryMaster = new CustomerBalanceHistory();
						customerBalanceHistoryMaster.Balance = accountMaster.Balance;
						customerBalanceHistoryMaster.CustomerId = accountMaster.CustomerId;
						customerBalanceHistoryMaster.CreatedDate = DateTime.Now;
						invoice.CustomerBalanceHistories.Add(customerBalanceHistoryMaster);

						dbContext.Invoices.Add(invoice);
						dbContext.SaveChanges();

						foreach (var item in lstInvoice.Where(p=>p.BusinessCode!=Const_BusinessCode.Business_201))
						{
							item.InvoiceRefId = invoice.InvoiceId;
							dbContext.Invoices.Add(item);
							dbContext.SaveChanges();
							
						}
					}
					tranScope.Complete();
				}
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

		#region // InvoiceConfirmPayment01
		// Phân bổ cho đơn hàng từ số dư khả dụng: businesscode =209
		public bool InvoiceConfirmPayment01(
				string customerId
				, string bankId
				, string fromAccount
				, string orderId // orderNo
				, string amount
				, string remark
				, string bussinessCode
				, string createdUserId
				, string paymentDate
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "InvoiceConfirmPayment01";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"customerId", customerId
						, "bankId", bankId
						, "fromAccount", fromAccount
						, "orderId", orderId
						, "amount", amount
						, "remark", remark
						, "bussinessCode", bussinessCode
						, "createdUserId", createdUserId
						, "paymentDate", paymentDate
			            });
			#endregion

			try
			{
				#region // PreBuild

				using (var tranScope = new TransactionScope(TransactionScopeOption.Required))
				{
					using (var dbContext = new EcmsEntities())
					{
						#region // Check:

						var cusId = Convert.ToInt32(customerId);

						#endregion

						#region // Build:

						#region // Insert to BusinessCode 209
						Invoice invoice = new Invoice();

						if (!string.IsNullOrEmpty(orderId))
						{
							invoice.OrderId = Convert.ToInt32(orderId);
						}
						if (!string.IsNullOrEmpty(paymentDate))
						{
							invoice.CreatedDate = Convert.ToDateTime(paymentDate);
						}
						invoice.BusinessCode = bussinessCode;
						invoice.InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss");
						invoice.InvoiceDate = invoice.CreatedDate = DateTime.Now;
						invoice.Status = 2;
						invoice.CustomerId = Convert.ToInt32(customerId);
						invoice.Remark = remark;

						var invoiceDetail = new InvoiceDetail
						{
							Amount = Math.Round(Convert.ToDouble(amount), Constansts.NumberRoundDefault)
						};

						invoice.InvoiceDetails.Add(invoiceDetail);

						// Check Mã hóa đơn:
						if (dbContext.Invoices.Any(x => x.InvoiceCode == invoice.InvoiceCode))
						{
							throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
						}

						#endregion

						dbContext.Invoices.Add(invoice);
						dbContext.SaveChanges();

						#endregion

						tranScope.Complete();
					}
				}
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

		#region // InvoiceForwardCreate
		public Invoice InvoiceForwardCreate(
			Invoice invoice
			, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "InvoiceForwardCreate";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
			            });
			#endregion

			try
			{
				using (var tranScope = new TransactionScope())
				{
					using (var dbContext = new EcmsEntities())
					{
						#region // Check:

						invoice.InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss");
						invoice.InvoiceDate = invoice.CreatedDate = DateTime.Now;
						invoice.Status = 2;

						var invoiceValidate = dbContext.Invoices.FirstOrDefault(x => x.InvoiceCode == invoice.InvoiceCode);
						if (invoiceValidate != null)
						{
							throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
						}

						#endregion

						#region // Build:
						dbContext.Invoices.Add(invoice);
						dbContext.SaveChanges();										
						tranScope.Complete();
					}
						#endregion

					#region // Return:

					return invoice;

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

        #region // InvoiceUpdate
        public Invoice InvoiceUpdate(Invoice invoice, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "InvoiceUpdate";
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

                    var invoiceReturn = dbContext.Invoices.FirstOrDefault(x => x.InvoiceCode == invoice.InvoiceCode);
                    if (invoiceReturn == null)
                    {
                        throw new Exception("Hóa đơn không tồn tại");
                    }

                    #endregion

                    #region // Build:

                    invoiceReturn.BusinessCode = invoice.BusinessCode;
                    invoiceReturn.CustomerId = invoice.CustomerId;
                    invoiceReturn.InvoiceDate = invoice.InvoiceDate;
                    invoiceReturn.OrderId = invoice.OrderId;
                    invoiceReturn.Remark = invoice.Remark;
                    invoiceReturn.SubCustomerId = invoice.SubCustomerId;

                    //Xóa Invoice detail
                    var InvoiceDetail = dbContext.InvoiceDetails.Where(x => x.InvoiceId == invoiceReturn.InvoiceId);
                    foreach (var item in InvoiceDetail)
                    {
                        dbContext.InvoiceDetails.Remove(item);
                    }

                    //Add lại Invoice detail mới
                    foreach (var od in invoice.InvoiceDetails)
                    {
                        dbContext.InvoiceDetails.Add(od);
                    }

                    dbContext.SaveChanges();
                    tranScope.Complete();

                    #endregion

                    #region // Return:

                    return invoiceReturn;

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

        #region // InvoiceDelete
        public bool InvoiceDelete(
            string invoiceId
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "InvoiceDelete";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "invoiceId", invoiceId
			            });
            #endregion

            try
            {
                using (var ts = new System.Transactions.TransactionScope())
                {
                    using (var dbContext = new EcmsEntities())
                    {
                        #region // Check:
                        int id = Convert.ToInt32(invoiceId);
                        var delInvoice = dbContext.Invoices.SingleOrDefault(x => x.InvoiceId == id);

						// Chỉ cho xóa Invoice của khách hàng gửi thanh toán
						if (delInvoice != null && delInvoice.Status != InvoiceStatus.Pending && delInvoice.BusinessCode !=Const_BusinessCode.Business_201)
						{
							throw new Exception("Invoice này không ở tình trạng Pending, không được phép xóa");
						}
                        #endregion

                        #region // Build:
                        dbContext.Invoices.Remove(delInvoice);
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

        #region // ConfirmPayment

        public bool ConfirmPayment(
				string invoiceId
				, string invoiceStatus
				, string createdUserId
				, string replyContent
				, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ConfirmPayment";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"invoiceId", invoiceId,
						"invoiceStatus", invoiceStatus,
						"replyContent", replyContent,
			            });
            #endregion

            try
            {
					List<Invoice> lstInvoice = new List<Invoice>();
                    var dbContext = new EcmsEntities();
					
                    #region // Check:
                    int ivId = Convert.ToInt32(invoiceId);
                    var invoiceReturn = dbContext.Invoices.SingleOrDefault(x => x.InvoiceId == ivId);
					if (invoiceReturn == null)
					{
						throw new Exception("Đơn thanh toán không tồn tại");
					}

					if (invoiceReturn.OrderId != null)
					{ 
						// if invoice have OrderId then must to be check status in Order object
						// only status 4 or 6
						var order= dbContext.Orders.SingleOrDefault(p => p.OrderId == invoiceReturn.OrderId);
						if (order == null || !(order.OrderStatus == OrderInStatus.OrderConfirmed || order.OrderStatus == OrderInStatus.Finished || order.OrderStatus == OrderInStatus.Deliveried))
						{
							throw new Exception("Không có đơn hàng thanh toán hoặc đơn hàng thanh toán không ở trạng thái xác nhận, hoàn thành");
						}
					}

                    #endregion

					#region // Phân bổ thanh toán:
					//Nếu đơn thanh toán là đơn gửi yêu cầu thanh toán
					if (!string.IsNullOrEmpty(invoiceStatus) && Convert.ToInt32(invoiceStatus) == InvoiceStatus.Confirm && invoiceReturn.OrderId==null)
                    {
							List<int> lstOrderId = new List<int>();
							double amountNew = invoiceReturn.InvoiceDetails.Sum(p => p.Amount ?? 0);
							// Lấy danh sách các đơn hàng cần thanh toán
							// Chỉ lấy những đơn hàng của khách hàng đó ở tình trạng "Đã xác nhận"
							// Sắp xếp những Order xử lý phân bổ cho những order cũ trước.
							var orderList = from p in dbContext.Orders
											where p.CustomerId == invoiceReturn.CustomerId 
													&& (p.OrderStatus == OrderInStatus.OrderConfirmed 
														|| p.OrderStatus == OrderInStatus.Finished 
														//|| p.OrderStatus == OrderInStatus.Deliveried
														)
											orderby p.OrderDate
											select p;

							foreach (var item in orderList)
							{
								lstOrderId.Add(item.OrderId);
							}						
							
							int cout = 0;
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
								var lstOrderDetail = dbContext.OrderDetails.Where(p => p.OrderId == item);
								double sumOrderAmount = 0;
								foreach (var itemDetail in lstOrderDetail)
								{
									sumOrderAmount += new OrderBiz().GetDetailTotalMoney(itemDetail.OrderDetailId);
								}
								#region // Tính thêm phí trả chậm cho đơn hàng mua hộ
									sumOrderAmount = Math.Round((sumOrderAmount + (lstOrderDetail.FirstOrDefault().Order.AmountFeeDelay ?? 0)), Constansts.NumberRoundDefault);
								#endregion
								NLogLogger.Info(string.Format("sumAmount of Order sumOrderAmount={0}", sumOrderAmount));

								// Tính số tiền đã thanh toán
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
								if(detailAmout <=0)
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
									CustomerId = invoiceReturn.CustomerId
									,
									CreatedDate = DateTime.Now
									,
									BankId = invoiceReturn.BankId
									,
									FromAccount = invoiceReturn.FromAccount
									,
									InvoiceCode = "I" + DateTime.Now.ToString("yyMMddHHmmss") + Convert.ToString(cout)
									,
									BusinessCode = Const_BusinessCode.Business_208
									,
									Remark = string.Format("Hệ thống phân bổ thanh toán cho đơn hàng orderId={0} khi confirm TT cho khách hàng", item)
									,
									InvoiceDate = invoiceReturn.InvoiceDate
									,
									Status = 2
								};
								invoicePaid.InvoiceDetails.Add(invoicePaidDetail);
								// Check Mã hóa đơn:
								if (dbContext.Invoices.Any(x => x.InvoiceCode == invoicePaid.InvoiceCode))
								{
									throw new Exception("Mã hóa đơn đã tồn tại trên hệ thống");
								}
								lstInvoice.Add(invoicePaid);
							}
					}
					#endregion

					#region // Excute
					using (var tranScope = new TransactionScope())
					{
						using (var db = new EcmsEntities())
						{
							// update status
							NLogLogger.Info(string.Format("update customer with Status={0}", invoiceStatus));
							var invoiceResult = db.Invoices.SingleOrDefault(x => x.InvoiceId == ivId);
							invoiceResult.Status = Convert.ToInt32(invoiceStatus);
							invoiceResult.ReplyContent = replyContent;

							var currentDate = DateTime.Now;
							var accountMaster = db.Customers.SingleOrDefault(x => x.CustomerCode == Constansts.CustomerAccount_MBGN);
							if (accountMaster == null)
							{
								throw new Exception("Tài khoản mua bán giao nhận không tồn tại");
							}
							accountMaster.Balance += Math.Round(invoiceResult.InvoiceDetails.Sum(x => (x.Amount ?? 0)), Constansts.NumberRoundDefault);
							accountMaster.LastDateModify = currentDate;

							var customer = db.Customers.SingleOrDefault(x => x.CustomerId == invoiceResult.CustomerId);
							customer.Balance += Math.Round(invoiceResult.InvoiceDetails.Sum(x => (x.Amount ?? 0)), Constansts.NumberRoundDefault);
							customer.LastDateModify = currentDate;

							//Lưu lịch sử số dư khách hàng
							var customerBalanceHistory = new CustomerBalanceHistory();
							customerBalanceHistory.Balance = customer.Balance;
							customerBalanceHistory.CustomerId = customer.CustomerId;
							customerBalanceHistory.CreatedDate = currentDate;
							customerBalanceHistory.InvoiceId = invoiceReturn.InvoiceId;

							//Lưu lịch sử số dư mua bán giao nhận
							var customerBalanceHistoryMaster = new CustomerBalanceHistory();
							customerBalanceHistoryMaster.Balance = accountMaster.Balance;
							customerBalanceHistoryMaster.CustomerId = accountMaster.CustomerId;
							customerBalanceHistoryMaster.CreatedDate = currentDate;
							customerBalanceHistoryMaster.InvoiceId = invoiceReturn.InvoiceId;

							db.CustomerBalanceHistories.Add(customerBalanceHistory);
							db.CustomerBalanceHistories.Add(customerBalanceHistoryMaster);

							foreach (var item in lstInvoice)
							{
								NLogLogger.Info(string.Format("update list invoice if count list>0 with OrderId={0}", item.OrderId));
								item.InvoiceRefId = invoiceResult.InvoiceId;
								db.Invoices.Add(item);
								db.SaveChanges();
							}
							db.SaveChanges();
							tranScope.Complete();
						}
					}
					#endregion

					return true;
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

		#region // RevertPayment
		public bool RevertPayment(string invoiceId
					, string invoiceStatus
					, string createdUserCode
					, string replyContent
					, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "RevertPayment";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"invoiceId", invoiceId,
						"createdUserCode", createdUserCode,
						"replyContent", replyContent,
			            });
			#endregion

			try
			{
				using (var tranScope = new TransactionScope())
				{
					var dbContext = new EcmsEntities();

					#region // Check:
					int ivId = Convert.ToInt32(invoiceId);
					var invoiceReturn = dbContext.Invoices.SingleOrDefault(x => x.InvoiceId == ivId);

					if (invoiceReturn == null)
					{
						throw new Exception("Đơn thanh toán không tồn tại");
					}

					#endregion

					#region // Build:
					//Thay đổi trạng thái Invoice
					if (invoiceReturn.Status == InvoiceStatus.Pending)
					{
						throw new Exception("Thanh toán chưa khớp hoặc đã hủy");
					}

					//Nếu đơn thanh toán là đơn gửi yêu cầu thanh toán
					if (invoiceReturn.Status == InvoiceStatus.Confirm && invoiceReturn.BusinessCode ==Const_BusinessCode.Business_201)
					{
						//Check tài khoản gửi yêu cầu
						var customer = dbContext.Customers.SingleOrDefault(x => x.CustomerId == invoiceReturn.CustomerId);
						if (customer == null)
						{
							throw new Exception("Tài khoản khách hàng không tồn tại");
						}
						//Check tài khoản chủ muabangiaonhan 
						var accountMaster = dbContext.Customers.FirstOrDefault(x => x.CustomerCode == Constansts.CustomerAccount_MBGN);
						if (accountMaster == null)
						{
							throw new Exception("Tài khoản mua bán giao nhận không tồn tại");
						}
						var currentDate = DateTime.Now;
						//Trừ tiền tk khách hàng đã cộng trước đó
						customer.Balance -= Math.Round(invoiceReturn.InvoiceDetails.Sum(x => (x.Amount ?? 0)), Constansts.NumberRoundDefault); 
						customer.LastDateModify = currentDate;

						////Trừ tiền tk muabangiaonhan đã cộng trước đó 
						accountMaster.Balance -= Math.Round(invoiceReturn.InvoiceDetails.Sum(x => (x.Amount ?? 0)), Constansts.NumberRoundDefault); 
						accountMaster.LastDateModify = currentDate;

						//Lưu lịch sử số dư khách hàng
						var customerBalanceHistory = new CustomerBalanceHistory();
						customerBalanceHistory.Balance = customer.Balance;
						customerBalanceHistory.CustomerId = customer.CustomerId;
						customerBalanceHistory.CreatedDate = currentDate;
						customerBalanceHistory.InvoiceId=invoiceReturn.InvoiceId;
						if (!string.IsNullOrEmpty(createdUserCode))
						{
							customerBalanceHistory.CreatedUserCode = createdUserCode;
						}

						//Lưu lịch sử số dư mua bán giao nhận
						var customerBalanceHistoryMaster = new CustomerBalanceHistory();
						customerBalanceHistoryMaster.Balance = accountMaster.Balance;
						customerBalanceHistoryMaster.CustomerId = accountMaster.CustomerId;
						customerBalanceHistoryMaster.CreatedDate = currentDate;
						customerBalanceHistoryMaster.InvoiceId = invoiceReturn.InvoiceId;
						if (!string.IsNullOrEmpty(createdUserCode))
						{
							customerBalanceHistoryMaster.CreatedUserCode = createdUserCode;
						}

						dbContext.CustomerBalanceHistories.Add(customerBalanceHistory);
						dbContext.CustomerBalanceHistories.Add(customerBalanceHistoryMaster);
					}
					invoiceReturn.Status = Convert.ToInt32(invoiceStatus);

					// Revert cả những Invoice có Mã TT tham chiếu = invoiceId
					var lstInvoiceRef = dbContext.Invoices.Where(p => p.InvoiceRefId == invoiceReturn.InvoiceId && p.Status ==InvoiceStatus.Confirm);
					foreach (var item in lstInvoiceRef)
					{
						item.Status = Convert.ToInt32(invoiceStatus);
						item.LastDateModify = DateTime.Now;
						item.Description = string.Format("Revert invoice to status={0}", invoiceStatus);
					}
					dbContext.SaveChanges();
					tranScope.Complete();
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

		#region // CancelPayment

		public bool CancelPayment(
				string invoiceId
				, string createdUserId
				, string replyContent
				, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "CancelPayment";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"invoiceId", invoiceId,
						"replyContent", replyContent,
			            });
			#endregion

			try
			{
				#region // Excute
				using (var tranScope = new TransactionScope())
				{
					using (var db = new EcmsEntities())
					{
						int ivId = Convert.ToInt32(invoiceId);
						var invoiceResult = db.Invoices.SingleOrDefault(x => x.InvoiceId == ivId);
						if (invoiceResult == null)
						{
							throw new Exception("Đơn thanh toán không tồn tại");
						}						
						
						invoiceResult.Status = InvoiceStatus.NotConfirm;
						invoiceResult.ReplyContent = replyContent;

						db.SaveChanges();
						tranScope.Complete();
					}
				}
				#endregion

				return true;
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
    public class InvoiceModel
    {
        public Int32 InvoiceId { set; get; }
        public String InvoiceCode { set; get; }
        public DateTime? InvoiceDate { set; get; }
        public DateTime? CreatedDate { set; get; }
        public Int32? CustomerId { set; get; }
        public String CustomerCode { set; get; }
        public String CustomerName { set; get; }
        public string UserCode { get; set; }
        public Int32? SubCustomerId { set; get; }
        public Int32? OrderId { set; get; }
        public string OrderNo { get; set; }
        public int? OrderStatus { get; set; }
        public Int32? Status { set; get; }
        public string FromAccount { get; set; }
        public String BusinessCode { set; get; }
        public int? BankId { get; set; }
        public String BankName { get; set; }
		public String ReplyContent { get; set; }
		public Int32? InvoiceRefId { set; get; }
		public String InvoiceRefCode { set; get; }

        private string businessName;
        public string BusinessName
        {
            set
            {
                businessName = value;
            }
            get
            {
                switch (BusinessCode)
                {
                    case Const_BusinessCode.Business_201:
						return Const_BusinessCode.Business_201_Text;
					case Const_BusinessCode.Business_202:
						return Const_BusinessCode.Business_202_Text;
					case Const_BusinessCode.Business_203:
						return Const_BusinessCode.Business_203_Text;
					case Const_BusinessCode.Business_204:
						return Const_BusinessCode.Business_204_Text;
					case Const_BusinessCode.Business_205:
						return Const_BusinessCode.Business_205_Text;
					case Const_BusinessCode.Business_206:
						return Const_BusinessCode.Business_206_Text;
					case Const_BusinessCode.Business_207:
						return Const_BusinessCode.Business_207_Text;
					case Const_BusinessCode.Business_208:
						return Const_BusinessCode.Business_208_Text;
					case Const_BusinessCode.Business_209:
						return Const_BusinessCode.Business_209_Text;
                    default:
                        return "";
                }
            }
        }

        private String statusText;
        public String StatusText
        {
            get
            {
                return Status == 1 ? InvoiceStatus.PendingText :
                                        Status == 2 ? InvoiceStatus.ConfirmText :
                                        Status == 3 ? InvoiceStatus.NotConfirmText : "";
            }
            set
            {
                statusText = value;
            }
        }


        public String Remark { set; get; }
        public IQueryable<InvoiceDetailModel> lstInvoiceDetailModel;

        private double? sumAmount;
        public double? SumAmount
        {
            get
            {
                return lstInvoiceDetailModel.ToList().Sum(p => p.Amount);
            }
            set
            {
                sumAmount = value;
            }
        }
    }

    public class InvoiceDetailModel
    {
        public Int32 InvoiceDetailId { set; get; }
        public Int32? InvoiceId { set; get; }
        public double? Amount { set; get; }

        //Phí trả chậm
        public int? OverDuoConfigId { get; set; }
        private double? overDuoConfigValue;
        public double? OverDuoConfigValue { get; set; }
        public int? Type { get; set; }
        public InvoiceModel InvoiceModel { get; set; }
    }

    #endregion

}
