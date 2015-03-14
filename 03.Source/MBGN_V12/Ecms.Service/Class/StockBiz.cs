using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using System.Collections;
using CommonUtils;

namespace Ecms.Biz.Class
{
    public class StockBiz
    {
        #region //StockInOut

        public List<Inv_StockInOutModel> StockInOutGet(string fromDate, string toDate, string stockInOutNo, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "StockInOutGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName     
                        ,"fromDate",fromDate
                        ,"stockInOutNo",stockInOutNo
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    var stockInOuts = from sio in dbContext.Inv_StockInOut.AsQueryable()
                                      join su in dbContext.Sys_User on sio.CreatedUser equals su.UserCode into su_join
                                      from su in su_join.DefaultIfEmpty()
                                      join c in dbContext.Customers on sio.CustomerId equals c.CustomerId into c_join
                                      from c in c_join.DefaultIfEmpty()
                                      select new Inv_StockInOutModel()
                                      {
                                          StockInOutId = sio.StockInOutId,
                                          StockOutNo = sio.StockOutNo,
                                          CreatedDate = sio.CreatedDate,
                                          InOutDate = sio.InOutDate,
                                          Remark = sio.Remark,
                                          Type = sio.Type,
                                          CustomerId = sio.CustomerId,
                                          CustomerCode = c.CustomerCode,
                                          CustomerName = c.CustomerName,
                                          UserCustomerCode = c.UserCode,
                                          UserCode = su.UserCode,
                                          Status = sio.status,
                                          lstStockInOutDetailModel = (from dt in sio.Inv_StockInOutDetail
                                                                      select new Inv_StockInOutDetailModel()
                                                                      {
                                                                          Id = dt.Id,
                                                                          Price = dt.Price,
                                                                          ProductId = dt.ProductId,
                                                                          ProductName = dt.Product.ProductName,
                                                                          ProductCode = dt.Product.ProductCode,
                                                                          CategoryId = dt.Product.CategoryId,
                                                                          CategoryName = dt.Product.Category.CategoryName,
                                                                          Quantity = dt.Quantity,
                                                                          Remark = dt.Remark,
                                                                          Serial = dt.Serial,
																		  StockInOutId = dt.StockInOutId
                                                                      }).AsQueryable()

                                      };
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        var dateFilter = Convert.ToDateTime(fromDate);
                        stockInOuts = stockInOuts.Where(x => x.CreatedDate >= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(toDate))
                    {
                        var dateFilter = Convert.ToDateTime(toDate);
                        stockInOuts = stockInOuts.Where(x => x.CreatedDate <= dateFilter);
                    }

                    if (!string.IsNullOrEmpty(stockInOutNo))
                    {
                        stockInOuts = stockInOuts.Where(x => x.StockOutNo == stockInOutNo);
                    }
                    return stockInOuts.OrderByDescending(x => x.InOutDate).ToList();
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
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public Inv_StockInOut StockInOutCreate(Inv_StockInOut stockInOut, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "StockInOutCreate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                var dbContext = new EcmsEntities();

                #region // Check:

                stockInOut.StockOutNo = "ST" + DateTime.Now.ToString("yyMMddHHmmss");
                stockInOut.CreatedDate = DateTime.Now;

                var stockInOutValidate = dbContext.Inv_StockInOut.FirstOrDefault(x => x.StockOutNo == stockInOut.StockOutNo);
				if (stockInOutValidate != null)
				{
					throw new Exception("Mã phiếu đã tồn tại trên hệ thống");
				}
                #endregion

                #region // Build:
				// Insert Good
				if (stockInOut.Type == StockType.StockIn && stockInOut.status == StockStatus.StockConfirmed)
				{
					foreach (var item in stockInOut.Inv_StockInOutDetail)
					{
						var good = new Inv_Goods
						{
							ProductId = item.ProductId,
							Serial = item.Serial,
							Status = GoodStatus.GoodInStock,
							DateIn = stockInOut.InOutDate,
						};
						stockInOut.Inv_Goods.Add(good);

						// Update stockbalance
						var stockBalance = dbContext.Inv_StockBalance.SingleOrDefault(p => p.ProductId == item.ProductId);
						if (stockBalance == null)
						{
							// insert new
							var newStockBalance = new Inv_StockBalance
							{
								Quantity = 1,
								ProductId = item.ProductId
							};
							dbContext.Inv_StockBalance.Add(newStockBalance);
						}
						else
						{
							// update
							stockBalance.Quantity = stockBalance.Quantity + 1;
						}
					}
				}				

                dbContext.Inv_StockInOut.Add(stockInOut);
                dbContext.SaveChanges();

                #endregion

                #region // Return:

                return stockInOut;

                #endregion

            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return null;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

		public Inv_StockInOut StockInOutCreate(EcmsEntities context, Inv_StockInOut stockInOut, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "StockInOutCreate";
			var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
			#endregion

			try
			{
				#region // Check:

				stockInOut.StockOutNo = "ST" + DateTime.Now.ToString("yyMMddHHmmss");
				stockInOut.CreatedDate = DateTime.Now;
				var stockInOutValidate = context.Inv_StockInOut.Any(x => x.StockOutNo == stockInOut.StockOutNo);
				if (stockInOutValidate)
				{
					throw new Exception("Mã phiếu đã tồn tại trên hệ thống");
				}

				#endregion

				#region // Build:

				context.Inv_StockInOut.Add(stockInOut);

				

				context.SaveChanges();

				#endregion

				#region // Return:

				return stockInOut;

				#endregion
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return null;
			}
			finally
			{
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}

        public Inv_StockInOut StockInOutUpdate(Inv_StockInOut stockInOut, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "StockInOutUpdate";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName                      
			            });
            #endregion

            try
            {
                var dbContext = new EcmsEntities();

                #region // Check:

                var stockInOutValidate = dbContext.Inv_StockInOut.FirstOrDefault(x => x.StockInOutId == stockInOut.StockInOutId);
                if (stockInOutValidate == null)
                    throw new Exception("Phiếu nhập không tồn tại trên hệ thống");

                stockInOutValidate.InOutDate = DateTime.Now;
                stockInOutValidate.StockOutNo = stockInOut.StockOutNo;
                stockInOutValidate.CreatedDate = stockInOut.CreatedDate;
                stockInOutValidate.CreatedUser = stockInOut.CreatedUser;                
                stockInOutValidate.Remark = stockInOut.Remark;
                stockInOutValidate.Type = stockInOut.Type;
                stockInOutValidate.CustomerId = stockInOut.CustomerId;
                stockInOutValidate.status = stockInOut.status;
                
                #endregion

                #region // Build:
                
                dbContext.SaveChanges();

                #endregion

                #region // Return:

                return stockInOut;

                #endregion

            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return null;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //StockInOutDetail

        public List<Inv_StockInOutDetail> StockInOutDetailGet(string stockInOutDetailId, string productId, string serial, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "StockInOutDetailGet";
            var alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName        
                        ,"stockInOutDetailId" , stockInOutDetailId
                        ,"productId",productId
                        ,"serial",serial
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    var stockInoutDetailList = from sd in dbContext.Inv_StockInOutDetail.AsQueryable()
                                               select sd;
                    if (!string.IsNullOrEmpty(stockInOutDetailId))
                    {
                        int iFill = Convert.ToInt32(stockInOutDetailId);
                        stockInoutDetailList = stockInoutDetailList.Where(x => x.Id == iFill);
                    }

                    if (!string.IsNullOrEmpty(productId))
                    {
                        int iFill = Convert.ToInt32(productId);
                        stockInoutDetailList = stockInoutDetailList.Where(x => x.ProductId == iFill);
                    }

                    if (!string.IsNullOrEmpty(serial))
                    {
                        stockInoutDetailList = stockInoutDetailList.Where(x => x.Serial == serial);
                    }
                    return stockInoutDetailList.ToList();
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
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region //Goods

        public void GoodsCreate(List<Inv_Goods> listGoods, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "GoodsCreate";
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

                    #region //Build

                    foreach (var item in listGoods)
                    {
                        dbContext.Inv_Goods.Add(item);
                    }
                    dbContext.SaveChanges();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion
    }

    #region //Models

    public class Inv_StockInOutModel
    {
        public int StockInOutId { get; set; }
        public string StockOutNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? InOutDate { get; set; }
		public DateTime? LastDateModify { get; set; }
        public string Remark { get; set; }
        public byte? Type { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string UserCustomerCode { get; set; }
        public string UserCode { get; set; }
        public int? Status { get; set; }

        private string statusText;
        public string StatusText
        {
            get
            {
                return Status == 1 ? StockStatus.StockPendingText :
					   Status == 2 ? StockStatus.StockConfirmedText : "";
            }
            set { statusText = value; }
        }

        public IQueryable<Inv_StockInOutDetailModel> lstStockInOutDetailModel { get; set; }

        public double? sumAmount;
        public double? SumAmount
        {
            get
            {
                return lstStockInOutDetailModel.ToList().Sum(x => x.Price);
            }
            set
            {
                sumAmount = value;
            }
        }
    }

    public class Inv_StockInOutDetailModel
    {
        public int Id { get; set; }
		public int? StockInOutId { get; set; }
        public int? ProductId { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public string Remark { get; set; }
        public string Serial { get; set; }

        public string ProductName { get; set; }
        public string ProductCode { get; set; }

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    #endregion
}
