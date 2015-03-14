using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz.Entities;
using System.Web.UI;
using Ecms.Website.Common;
using Ecms.Biz.Class;

namespace Ecms.Website.DBServices
{
    public class WebsiteService : BaseService
    {
        #region // Constructs

        public WebsiteService()
            : base()
        {

        }
        #endregion


        #region // WebsiteLinkGet
        public List<WebsiteLinkModel> WebsiteLinkGet(string websiteId
                                                , string websiteName
                                                , string websiteLink
                                                , string parentId
                                                , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var websiteLi = this._websiteBiz.WebsiteLinkGet(websiteId
                                                                , websiteName
                                                                , websiteLink
                                                                , ""
                                                                , parentId
                                                                , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return websiteLi;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<WebsiteLinkModel>();
            }
        }
        #endregion

        #region // WebsiteLinkDelete
        public bool WebsiteLinkDelete(string websiteId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result = false;
                result = this._websiteBiz.WebsiteLinkDelete(websiteId, ref alParamsOutError);
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

        #region // WebsiteLinkDelete
        public WebsiteLink WebsiteLinkCreate(WebsiteLink websiteLink, Page page)
        {
            try
            {
                string alParamsOutError = "";
                WebsiteLink result;
                result = this._websiteBiz.WebsiteLinkCreate(websiteLink , ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new WebsiteLink();
            }
        }
        #endregion

        #region // WebsiteLinkDelete
        public WebsiteLink WebsiteLinkUpdate(WebsiteLink websiteLink, Page page)
        {
            try
            {
                string alParamsOutError = "";
                WebsiteLink result;
                result = this._websiteBiz.WebsiteLinkUpdate(websiteLink, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new WebsiteLink();
            }
        }
        #endregion


        #region // Website Account

        #region WebsiteAccountGet
        public List<WebsiteAccountModel> WebsiteAccountGet(string websiteAccountId
                                                                , string websiteId
                                                                , string parentId
																, string accountWebsiteNo
                                                                , string website
                                                                , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var websiteAcc = this._websiteBiz.WebsiteAccountGet(websiteAccountId
                                                                    , websiteId
																	, parentId
																	, accountWebsiteNo
                                                                    , website
                                                                    , ""
                                                                    ,ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return websiteAcc;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<WebsiteAccountModel>();
            }
        }
        #endregion

        #region // WebsiteAccountDelete
        public bool WebsiteAccountDelete(string websiteAccountId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result = false;
                result = this._websiteBiz.WebsiteAccountDelete(websiteAccountId, ref alParamsOutError);
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

        #region // WebsiteAccountCreate
        public WebsiteAccount WebsiteAccountCreate(WebsiteAccount websiteAccount, Page page)
        {
            try
            {
                string alParamsOutError = "";
                WebsiteAccount result;
                result = this._websiteBiz.WebsiteAccountCreate(websiteAccount , ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new WebsiteAccount();
            }
        }
        #endregion

        #region // WebsiteAccountUpdate
        public WebsiteAccount WebsiteAccountUpdate(WebsiteAccount websiteAccount, Page page)
        {
            try
            {
                string alParamsOutError = "";
                WebsiteAccount result;
                result = this._websiteBiz.WebsiteAccountUpdate(websiteAccount, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new WebsiteAccount();
            }
        }
        #endregion

        #endregion

		#region // WebsiteAccountVisaGet

		#region WebsiteAccountVisaGet
		public List<WebsiteAccountVisa> WebsiteAccountVisaGet(string websiteAccountVisaId
																, string websiteAccountId
																, string visaNo
																, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var websiteAcc = this._websiteBiz.WebsiteAccountVisaGet(websiteAccountVisaId
																	, websiteAccountId
																	, visaNo
																	, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return websiteAcc;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<WebsiteAccountVisa>();
			}
		}
		#endregion

		#region // WebsiteAccountCreate
		public WebsiteAccountVisa WebsiteAccountVisaCreate(WebsiteAccountVisa websiteAccountVisa, Page page)
		{
			try
			{
				string alParamsOutError = "";
				WebsiteAccountVisa result;
				result = this._websiteBiz.WebsiteAccountVisaCreate(websiteAccountVisa, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new WebsiteAccountVisa();
			}
		}
		#endregion

		#region // WebsiteAccountVisaDelete
		public bool WebsiteAccountVisaDelete(string websiteAccountVisaId, Page page)
		{
			try
			{
				string alParamsOutError = "";
				bool result = false;
				result = this._websiteBiz.WebsiteAccountVisaDelete(websiteAccountVisaId, ref alParamsOutError);
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

		#endregion
	}
}