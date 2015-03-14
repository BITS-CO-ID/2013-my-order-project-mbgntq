using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz;
using Ecms.Website.Common;
using System.Web.UI;
using Ecms.Biz.Entities;

namespace Ecms.Website.DBServices
{
    public class OrderService : BaseService
    {

        #region // Constructs

        public OrderService()
            : base()
        {

        }
        #endregion

        #region // Order

        #region // OrderGet
        public List<OrderModel> OrderGet(
            string orderId
                , string orderNo
                , string orderOutboundNo
                , string trackingOutboundNo
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
				, string IsAuthenticateUser
                , string isGetDetail
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var returnCustomer = this._orderBiz.OrderGet(
                                            orderId
                                        , orderNo
                                        , orderOutboundNo
                                        , trackingOutboundNo
                                        , orderDateFrom
                                        , orderDateTo
                                        , customerId
                                        , customerCode
                                        , userCode
                                        , customerCodeDelivery
                                        , orderNoDelivery
                                        , deliveryDateFrom
                                        , deliveryDateTo
                                        , orderStatus
                                        , orderTypeId
                                        , employeeCode
										, createUser
										, IsAuthenticateUser
                                        , isGetDetail
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }

                return returnCustomer;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<OrderModel>();
            }
        }
        #endregion

		#region // OrderOnlyGet
		public List<Order> OrderOnlyGet(
				string orderId
				, string orderNo
				, string orderOutboundNo
				, string trackingOutboundNo
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
				, Page page
			)
		{
			try
			{
				string alParamsOutError = "";

				var returnCustomer = this._orderBiz.OrderOnlyGet(
										orderId
										, orderNo
										, orderOutboundNo
										, trackingOutboundNo
										, orderDateFrom
										, orderDateTo
										, customerId
										, customerCode
										, userCode
										, customerCodeDelivery
										, orderNoDelivery
										, deliveryDateFrom
										, deliveryDateTo
										, orderStatus
										, orderTypeId
										, employeeCode
										, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				return returnCustomer;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<Order>();
			}
		}
		#endregion

		#region // OrderNoCheck
		public bool OrderNoCheck(
				string orderNo
				, Page page
			)
		{
			try
			{
				string alParamsOutError = "";

				//var orderNos = orderNo.Split(';');

				var result = this._orderBiz.OrderOnlyGet(
											""
											, orderNo
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
											, ""
											, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}

				if (result != null && result.Count > 0)
				{
					return false;
				}
				return false;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}
		#endregion

        #region // OrderCreate
        public Order OrderCreate(
                Order order
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var orderReturn = this._orderBiz.OrderCreate(
                                        order
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return orderReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }
        #endregion

        #region // OrderUpdate
        public void OrderUpdate(
                Order order
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var returnCustomer = this._orderBiz.OrderUpdate(
                                        order
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
            }
        }
        #endregion

        #region // OrderDelete
        public bool OrderDelete(
                string orderId
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

				var result = this._orderBiz.OrderDelete(
                                        orderId
                                        , ref alParamsOutError);

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

        #region // ConvertQuoteOrder
        public bool ConvertQuoteOrder(
                string orderId
                , string needDate
                , string remark
                , string deliveryDate
                , string deliveryAddress
                , string deliveryName
                , string deliveryMobile
                , string deliveryEmail
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var returnCustomer = this._orderBiz.ConvertQuoteOrder(
                                         orderId
                                        , needDate
                                        , remark
                                        , deliveryDate
                                        , deliveryAddress
                                        , deliveryName
                                        , deliveryMobile
                                        , deliveryEmail
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return returnCustomer;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
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
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var result = this._orderBiz.OrderGroupLinkGet(
                                             orderId
                                            , orderNo
                                            , orderOutboundNo
                                            , trackingNo
                                            , orderDateFrom
                                            , orderDateTo
                                            , customerId
                                            , orderStatus
                                            , orderTypeId
                                            , websiteId
											, shop
                                            , employeeCode
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }

                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<OrderDetailModel>();
            }
        }
        #endregion

        #region // OrderChangeStatus

        public bool OrderChangeStatus(string orderId, string orderStatus, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result = _orderBiz.OrderChangeStatus(orderId, orderStatus, "", ref alParamsOutError);

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

		public bool OrderChangeStatusWithRemark(string orderId, string orderStatus, string remark, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.OrderChangeStatus(orderId, orderStatus, remark, ref alParamsOutError);

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

		#region // RevertOrderStatus
		public bool RevertOrderStatus(string orderId, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.RevertOrderStatus(orderId, ref alParamsOutError);

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

		#region // RevertFirstOrderStatus
		public bool RevertFirstOrderStatus(string orderId, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.RevertFirstOrderStatus(orderId, ref alParamsOutError);

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

		#region // OrderEditTrackingNo

		public Order OrderEditTrackingNo(string orderId, string trackingNo, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result = _orderBiz.OrderEditTrackingNo(orderId, trackingNo, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
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
					, Page page)
        {
            try
            {
                string alParamsOutError = "";
				var result = _orderBiz.UpdateTrackingOrder(
							orderDetailId
							, newTrackingNo
							, orderDeliveryNo
							, dateToUsa
							, insuarance
							, ref alParamsOutError);

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

		#endregion

		#region // OrderUpdateEmployeeCode

		public Order OrderUpdateEmployeeCode(string orderId, string employeeCode, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result = _orderBiz.OrderUpdateEmployeeCode(orderId, employeeCode, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        #endregion

		#region // DeliveryDetailUpdate
		public bool DeliveryDetailUpdate(List<OrderDetailModel> listOrderDetailModel, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result=_orderBiz.DeliveryDetailUpdate(listOrderDetailModel, ref alParamsOutError);

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

		#region // ChangeDeliveryStatus
		public bool ChangeDeliveryStatus(string orderId
                    , string trackingNumber
                    , string status
                    , string dateStatus
                    , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var ressult = _orderBiz.ChangeDeliveryStatus(
                        orderId
                        , trackingNumber
                        , status
                        , dateStatus
                        , ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
				return ressult;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
				return false;
            }
        }

		#endregion

		#region // ChangeOrderStatus
		public bool ChangeOrderStatus(string orderId
					, string newStatus
					, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var ressult = _orderBiz.ChangeOrderStatus(
						orderId
						, newStatus
						, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return ressult;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
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
					, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.OrderUpdateConfigFee(orderId, calFeeDelay, dayAllowedFeeDelay, feeDelay, amountFeeDelayNew, ref alParamsOutError);

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

		#region // OrderDetail

		public OrderDetail OrderDetailUpdate(OrderDetail orderDetail, bool isShipModify, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var orderDetailReturn = _orderBiz.OrderDetailUpdate(orderDetail, isShipModify, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return orderDetailReturn;

            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

		#region // ChangeDeliveryStatus
		public bool OrderDetailUpdate(string orderDetailId
					, string statusNew
					, string dateStatusUpdate
					, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.OrderDetailUpdate(
						orderDetailId
						, statusNew
						, dateStatusUpdate
						, ref alParamsOutError);

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

        public bool OrderDetailChangeStatus(string orderDetailId, string orderDetailStatus, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result = _orderBiz.OrderDetailChangeStatus(orderDetailId, orderDetailStatus, ref alParamsOutError);

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

		public bool OrderDetailChangeStatus(
				string orderDetailId
				, string orderDetailStatusNew
				, string dateUpdate
				, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.OrderDetailChangeStatus(
								orderDetailId
								, orderDetailStatusNew
								, dateUpdate
								, ref alParamsOutError);

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

        public OrderDetail OrderDetailDeliveryUpdate(OrderDetail orderDetail, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var orderDetailReturn = _orderBiz.OrderDetailDeliveryUpdate(orderDetail, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return orderDetailReturn;

            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

		public bool OrderDetailDeliveryUpdate(List<OrderDetailModel> lstDetailModel, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var orderDetailReturn = _orderBiz.OrderDetailTrackingUpdate(lstDetailModel, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return orderDetailReturn;

			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}

		public bool OrderDetailTrackingNoCheck(string trackingNo, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.OrderDetailTrackingNoCheck(trackingNo, ref alParamsOutError);
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

		public bool OrderDetailTrackingNoUpdate(string orderDetailId, string newTrackingNo, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.OrderDetailTrackingNoUpdate(orderDetailId, newTrackingNo, ref alParamsOutError);
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
				   , Page page)
		{
			try
			{
				string alParamsOutError = "";
				var listResult = this._orderBiz.OrderDetailModelGet(
									fromDate
									, toDate
									, detailCode
									, orderNo
									, customerCode
									, websiteId
									, parentWebsiteId
									, status
									, orderTypeId
									, trackingNo
									, shop
									, employeeCode
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
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var result = this._orderBiz.OrderOutboundGet(
                                              orderOutboundId
                                            , orderOutboundNo
                                            , trackingNo
                                            , orderNumber
                                            , orderDateFrom
                                            , orderDateTo
                                            , orderStatus
                                            , accountWebsiteId
                                            , accountWebsiteNo
                                            , websiteId
                                            , parentWebsiteId
                                            , userCreate
											, visaId
                                            , isGetDetail
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }

                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<OrderOutboundModel>();
            }
        }
        #endregion

        #region // OrderOutboundCreate
        public OrderOutbound OrderOutboundCreate(
                OrderOutbound order
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var orderReturn = this._orderBiz.OrderOutboundCreate(
                                        order
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return orderReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }
        #endregion

        #region //OrderOutboundUpdate

        public OrderOutbound OrderOutboundUpdate(OrderOutbound orderOutbound, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var orderOutboundReturn = _orderBiz.OrderOutboundUpdate(orderOutbound, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }

                return orderOutboundReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        #endregion

        #region // OrderOutboundDelete
        public bool OrderOutboundDelete(
                string orderOutboundId
                , Page page
            )
        {
            try
            {
                string alParamsOutError = "";

                var orderReturn = this._orderBiz.OrderOutboundDelete(
                                        orderOutboundId
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return orderReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }
        #endregion

        #region //OrderOutboundUpdateTracking

        public bool OrderOutboundUpdateTracking(string orderOutboundId
                            , string trackingNo
                            , string orderNumber //???
                            , string status
                            , string dateUpdateForStatus
                            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var orderOutboundReturn = _orderBiz.OrderOutboundUpdateTracking(
                                        orderOutboundId
                                        , trackingNo
                                        , orderNumber
                                        , status
                                        , dateUpdateForStatus
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }

                return orderOutboundReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }

        #endregion
        #endregion

        #region //OrderOutboundDetailDelete

        public bool OrderOutboundDetailDelete(string orderOutboundDetaiId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result = _orderBiz.OrderOutboundDetailDelete(orderOutboundDetaiId, ref alParamsOutError);

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

		#region //CalDelayFeeOrder

		public bool CalDelayFeeOrder(Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = _orderBiz.CalDelayFeeOrder(ref alParamsOutError);

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
    }
}