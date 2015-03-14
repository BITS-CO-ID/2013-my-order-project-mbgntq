using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz;
using Ecms.Website.Common;
using System.Web.UI;
using Ecms.Biz.Entities;
using System.Configuration;

namespace Ecms.Website.DBServices
{
    public class InvoiceService : BaseService
    {
        #region //Declares

        private readonly InvoiceBiz _invoiceBiz = new InvoiceBiz();

        #endregion

        #region // Constructs

        public InvoiceService()
            : base()
        {

        }
        #endregion

        #region // Invoice

        public Invoice InvoiceCreate(
			string userCode
			, string bankId
			, string fromAccount
			, string orderId
			, string amount
			, string contentPayment
			, string bussinessCode
			, Page page)
        {
            try
            {
                var alParamsOutError = "";
                Invoice invoice = new Invoice();

                var customer = _customerBiz.UserCustomerGet("", "", "", "", "", "", "", userCode, "", "", "0", ref alParamsOutError).FirstOrDefault();

                if (!string.IsNullOrEmpty(alParamsOutError) || customer == null)
                {
                    throw GenServiceException(alParamsOutError);
                }

				

				if (!string.IsNullOrEmpty(orderId))
				{
					invoice.OrderId = Convert.ToInt32(orderId);
				}
                invoice.BankId = Convert.ToInt32(bankId);
                invoice.BusinessCode = bussinessCode;
                invoice.CustomerId = customer.CustomerId;
                invoice.FromAccount = fromAccount;
                invoice.Remark = contentPayment;               

                var invoiceDetail = new InvoiceDetail();
                invoiceDetail.Amount = Math.Round(Convert.ToDouble(amount),CommonUtils.Constansts.NumberRoundDefault);

				invoice.InvoiceDetails.Add(invoiceDetail);
                var invoiceReturn = _invoiceBiz.InvoiceCreate(invoice, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return invoiceReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

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
				, Page page)
        {
            try
            {
                var alParamsOutError = "";
                var result = _invoiceBiz.InvoiceCreateBackend(
											 customerId
											, bankId
											, fromAccount
											, orderId // orderNo
											, amount
											, remark
											, bussinessCode
											, createdUserId
											, paymentDate
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
				, Page page)
		{
			try
			{
				var alParamsOutError = "";
				var result = _invoiceBiz.InvoiceConfirmPayment(
											 customerId
											, bankId
											, fromAccount
											, orderId // orderNo
											, amount
											, remark
											, bussinessCode
											, createdUserId
											, paymentDate
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

		// Phân bổ từ số dư khả dụng
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
				, Page page)
		{
			try
			{
				var alParamsOutError = "";
				var result = _invoiceBiz.InvoiceConfirmPayment01(
											 customerId
											, bankId
											, fromAccount
											, orderId // orderNo
											, amount
											, remark
											, bussinessCode
											, createdUserId
											, paymentDate
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
                , Page page)
        {
            try
            {
                var alParamsOutError = "";
                var listInvoice = _invoiceBiz.InvoiceGet(invoiceId,
                                    invoiceCode,
                                    invoiceDateFrom,
                                    invoiceDateTo,
                                    customerId,
									customerCode,
                                    subCustomerId,
                                    status,
									orderId,
                                    orderCode,
                                    businessCode,
									invoiceRefId, 
                                    isGetDetail,
                                    ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                {
                    throw GenServiceException(alParamsOutError);
                }
                return listInvoice;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        public bool ConfirmPayment(
				string invoiceId
				, string invoiceStatus
				, string createdUserId
				, string replyContent
				, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var ressult= _invoiceBiz.ConfirmPayment(
						invoiceId
						, invoiceStatus
						, createdUserId
						, replyContent
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

		public bool CancelPayment(
				string invoiceId
				, string createdUserId
				, string replyContent
				, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var ressult = _invoiceBiz.CancelPayment(
						invoiceId
						, createdUserId
						, replyContent
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

		public Invoice InvoiceForwardCreate(Invoice invoice, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var invoiceNew= _invoiceBiz.InvoiceForwardCreate(invoice, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return invoiceNew;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return null;
			}
		}

		public bool InvoiceDelete(string invoiceId, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var invoiceNew = _invoiceBiz.InvoiceDelete(invoiceId, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
				{
					throw GenServiceException(alParamsOutError);
				}
				return invoiceNew;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}

        public bool RevertPayment(
                string invoiceId
                , string invoiceStatus
                , string createdUserCode
                , string replyContent
                , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var result= _invoiceBiz.RevertPayment(
                        invoiceId
                        , invoiceStatus
                        , createdUserCode
                        , replyContent
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
    }
}