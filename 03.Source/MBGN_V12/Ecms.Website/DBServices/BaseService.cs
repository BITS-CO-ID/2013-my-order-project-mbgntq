using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz;
using Ecms.Biz.Class;

namespace Ecms.Website.DBServices
{
	public class BaseService
	{
		#region // Declaration
		protected InvoiceBiz _invoiceBiz = new InvoiceBiz();
		protected UserBiz _userBiz = new UserBiz();
		protected CommonBiz _commonBiz = new CommonBiz();
		protected OrderBiz _orderBiz = new OrderBiz();
        protected CustomerBiz _customerBiz = new CustomerBiz();
        protected ProductBiz _productBiz = new ProductBiz();
        protected WebsiteBiz _websiteBiz = new WebsiteBiz();
        protected NewsBiz _newsBiz = new NewsBiz();
        protected ComplaintsBiz _complaintsBiz = new ComplaintsBiz();
		protected ReportBiz _reportBiz = new ReportBiz();

		protected string _strGwUserCode;

		#endregion

		#region // Init
		public BaseService()
		{
			try
			{
				
			}
			catch (Exception)
			{ 

			}

            //if (HttpContext.Current.Session["User"] != null)
            //{
            //    _strGwUserCode = Convert.ToString(HttpContext.Current.Session["User"]);
            //}
		}
		#endregion

		#region // Method

		protected ServiceException GenServiceException(string alParamError)
		{
			ServiceException ex = new ServiceException();
			if (alParamError == null)
			{
				ex.ErrorCode = "ERR0001";
				ex.ErrorMessage = "Null dataset return";
				ex.ErrorDetail = "Null dataset return";
				//ex.Tag = tag;
				return ex;
			}
			//

			string errorCode = "01";//Convert.ToString(CMyDataSet.GetErrorCode(ds));
			string errorTid = "011";// Convert.ToString(CMyDataSet.GetTid(ds));
			string errorMessage = "M01";//Convert.ToString(CMyDataSet.GetErrorCode(ds));
			string errorDetail = alParamError;//"No detail!";
			string errorNewLine = String.Format("{0}-----------------------------------------{1}", System.Environment.NewLine, System.Environment.NewLine);

			
			//object[] arrObj = null;
			//if (arrObj != null && arrObj.Length > 1)
			//{

			//    errorDetail = "ErrCode=" + errorCode + System.Environment.NewLine;
			//    errorDetail += errorNewLine;
			//    errorDetail += "Tid=" + errorTid + System.Environment.NewLine;
			//    errorDetail += errorNewLine;

			//    for (int i = 0; i < arrObj.Length; i++)
			//    {
			//        errorDetail += Convert.ToString(arrObj[i]);
			//        if (i % 2 == 0)
			//            errorDetail += " = ";
			//        else
			//            errorDetail += errorNewLine;
			//    }
			//}

			ex.ErrorCode = errorCode;
			ex.ErrorMessage = errorMessage;
			ex.ErrorDetail = alParamError;
			return ex;
		}

		#endregion
	}
}