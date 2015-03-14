using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Ecms.Biz.Class;
using Ecms.Website.Common;
using Ecms.Biz;
using Ecms.Biz.Entities;

namespace Ecms.Website.DBServices
{
    public class ReportService : BaseService
    {

        #region //ReportLiabilityGet

        public List<ReportLiabilityModel> ReportLiabilityGet(
				string fromDate
				, string toDate
				, string employeeCode
				, string customerCode
				, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var listResult = this._reportBiz.ReportLiabilityGet(
									fromDate
									, toDate
									, employeeCode
									, customerCode
									, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
                return listResult;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<ReportLiabilityModel>();
            }
        }

        #endregion

        #region //ReportLiabilityDetailGet

        public List<InvoiceDetailModel> ReportLiabilityDetailGet(
				string fromDate
				, string toDate
				, string customerId
				, string customerCode
				, string orderId
				, string chargeOrPay
				, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var listResult = _reportBiz.ReportLiabilityDetailGet(
									fromDate
									, toDate
									, customerId
									, customerCode
									, orderId
									, chargeOrPay
									, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return listResult;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<InvoiceDetailModel>();
            }
        }

        #endregion

		#region // Báo cáo tổng hợp đơn hàng vận chuyển
		public List<RptOrderTransportModel> ReportOrderTranportBuy(
				string fromDate
				, string toDate
				, string orderNo
				, string customerCode
				, string status
				, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var returnCustomer = this._orderBiz.OrderGet(
										""
										, orderNo
										, ""
										, ""
										, fromDate
										, toDate
										, ""
										, customerCode
										, ""
										, ""
										, ""
										, ""
										, ""
										, status
										, "3"
										, ""
										, ""
										, ""
										, ""
										, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				var result = from p in returnCustomer
							 select new RptOrderTransportModel
							 { 
								CustomerCode=p.CustomerCode,
								CustomerName = p.CustomerName,
								DeliveryAddress = p.DeliveryAddress,
								OrderDate = p.OrderDate,
								OrderId = p.OrderId,
								OrderNo = p.OrderNo,
								Remark = p.Remark,
								OrderStatus = p.OrderStatus,
								SumAmount = p.SumAmount??0,
								PayAmount = p.TotalPayAmountNormal,
								AmountFeeDelay = p.AmountFeeDelay,
								StatusText=p.OrderStatusText,
								SumWeight=p.SumWeight??0
							 };

				return result.ToList();
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<RptOrderTransportModel>();
			}
		}
		#endregion

		#region // Báo cáo tổng hợp đơn hàng sản phẩm
		public List<RptOrderTransportModel> ReportByProduct(
				string fromDate
				, string toDate
				, string orderNo
				, string customerCode
				, string status
				, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var returnCustomer = this._orderBiz.OrderGet(
										""
										, orderNo
										, ""
										, ""
										, fromDate
										, toDate
										, ""
										, customerCode
										, ""
										, ""
										, ""
										, ""
										, ""
										, status
										, Convert.ToString(CommonUtils.OrderType_Const.OrderByProduct)
										, ""
										, ""
										, ""
										, ""
										, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				var result = from p in returnCustomer
							 select new RptOrderTransportModel
							 {
								 CustomerCode = p.CustomerCode,
								 CustomerName = p.CustomerName,
								 DeliveryAddress = p.DeliveryAddress,
								 OrderDate = p.OrderDate,
								 OrderId = p.OrderId,
								 OrderNo = p.OrderNo,
								 Remark = p.Remark,
								 OrderStatus = p.OrderStatus,
								 SumAmount = p.SumAmount ?? 0,
								 PayAmount = p.TotalPayAmountNormal,
								 AmountFeeDelay = p.AmountFeeDelay,
								 StatusText = p.OrderStatusText,
								 SumWeight = p.SumWeight ?? 0
							 };

				return result.ToList();
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<RptOrderTransportModel>();
			}
		}
		#endregion

		#region // Báo cáo tổng hợp đơn hàng mua hộ
		public List<RptOrderBuyModel> ReportOrderBuy(
				string fromDate
				, string toDate
				, string orderNo
				, string trackingNo
				, string customerCode
				, string status
				, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var returnCustomer = this._orderBiz.OrderGet(
										""
										, orderNo
										, ""
										, trackingNo
										, fromDate
										, toDate
										, ""
										, customerCode
										, ""
										, ""
										, ""
										, ""
										, ""
										, status
										, "2"
										, ""
										, ""
										, ""
										, ""
										, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				var result = from p in returnCustomer
							 select new RptOrderBuyModel
							 {
								 CustomerCode = p.CustomerCode,
								 CustomerName = p.CustomerName,
								 DeliveryAddress = p.DeliveryAddress,
								 OrderDate = p.OrderDate,
								 OrderId = p.OrderId,
								 OrderNo = p.OrderNo,
								 Remark = p.Remark,
								 OrderStatus = p.OrderStatus,
								 SumAmount = p.SumAmount??0,
								 PayAmount = p.TotalPayAmountNormal,
								 StatusText = p.OrderStatusText,
								 AmountFeeDelay = p.AmountFeeDelay,
								 RemainAmount=p.RemainAmount??0,
								 TrackingNo=p.TrackingNo
							 };

				return result.ToList();
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<RptOrderBuyModel>();
			}
		}
		#endregion

		#region // Báo cáo tổng hợp đơn hàng nước ngoài

		#endregion

		#region // Báo cáo tổng hợp tài khoản Khách hàng

		#endregion

		#region // Báo cáo tổng hợp tài khoản Nhân viên
		public List<ReportEmployeeModel> ReportEmployeeGet(
			string fromDate
			, string toDate
			, string employeeCode
			, string customerCode
			, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var listResult = this._reportBiz.ReportLiabilityGet(
									fromDate
									, toDate
									, employeeCode
									, customerCode
									, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				var query = from p in listResult
							group p by new { p.EmployeeCode, p.EmployeeName } into gNew
							select new ReportEmployeeModel
							{
								EmployeeCode=gNew.Key.EmployeeCode,
								EmployeeName = gNew.Key.EmployeeName,
								BeforeBalance=gNew.Sum(x=>x.BeforeBalance.Value),
								TotalCharge = gNew.Sum(x => x.TotalCharge.Value),
								Balance = gNew.Sum(x => x.Balance),
								BalanceFreeze = gNew.Sum(x => x.BalanceFreeze),
								BalanceAvaiable = gNew.Sum(x => x.BalanceAvaiable),
								RemainBalance = gNew.Sum(x => x.RemainBalance),
								RemainBalanceReceivable = gNew.Sum(x => x.RemainBalanceReceivable),
								
							};

				return query.ToList();
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<ReportEmployeeModel>();
			}
		}
		#endregion

		#region // Báo cáo tổng hợp công nợ Đơn hàng
		public List<RptOrderDebitModel> ReportOrderDebitGet(
				string fromDate
				, string toDate
				, string employeeCode
				, string customerCode
				, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var listResult = this._reportBiz.ReportOrderDebitGet(
									fromDate
									, toDate
									, employeeCode
									, customerCode
									, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				return listResult;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<RptOrderDebitModel>();
			}
		}
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
				   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var listResult = this._reportBiz.ReportGoodDeliverlyGet(
									fromDate
									, toDate
									, detailCode
									, orderNo
									, customerCode
									, shop
									, websiteId
									, parentWebsiteId
									, status
									, orderTypeId
									, trackingNo
									, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				return listResult.ToList();
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<OrderDetailModel>();
			}
		}
		#endregion

		#region // Xác nhận giao hàng
		public Rpt_DeliverlyConfirmPrint Rpt_DeliverlyConfirmPrintCreate(
			   Rpt_DeliverlyConfirmPrint rptPrint
			   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var ressult = _reportBiz.Rpt_DeliverlyConfirmPrintCreate(
						rptPrint
						, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return ressult;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return null;
			}
		}

		public bool Rpt_DeliverlyCheckPrint(
			   string rptPrintDetialId
			   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var ressult = _reportBiz.Rpt_DeliverlyCheckPrint(
						rptPrintDetialId
						, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return ressult;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}

		public bool Rpt_DeliverlyConfirmPrintDelete(
			   string rptPrintId
			   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var ressult = _reportBiz.Rpt_DeliverlyConfirmPrintDelete(
						rptPrintId
						, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return ressult;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}

		public List<Rpt_DeliverlyConfirmPrintModel> Rpt_DeliverlyConfirmPrintGet(
				string fromDate
				   , string toDate
				   , string rptDeliverlyId
				   , string customerId
				   , string customerCode
				   , string createdUser
				   , string deliverlyFullName
			   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var ressult = _reportBiz.Rpt_DeliverlyConfirmPrintGet(
							  fromDate
						   ,  toDate
						   ,  rptDeliverlyId
						   ,  customerId
						   , customerCode
						   ,  createdUser
						   ,  deliverlyFullName
						, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return ressult;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<Rpt_DeliverlyConfirmPrintModel>();
			}
		}
		#endregion

		#region // Tình trạng món hàng
		public List<Rpt_StockOInOut> ReportStockOInOutGet(
					string fromDate
				   , string toDate
				   , string productId
				   , string productCode
				   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var listResult = this._reportBiz.ReportStockOInOutGet(
									fromDate
									, toDate
									, productId
									, productCode									
									, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				return listResult.ToList();
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<Rpt_StockOInOut>();
			}
		}
		#endregion
	}
}