using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Interfaces;
using System.Collections;
using Ecms.Biz.Entities;
using CommonUtils;

namespace Ecms.Biz.Class
{
    public class WebsiteBiz : IWebsite
    {
        #region // Website Link

        public List<WebsiteLinkModel> WebsiteLinkGet(string websiteId
                                                , string websiteName
                                                , string websiteLink
                                                , string description
                                                , string parentId
                                                , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteLinkGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "websiteId", websiteId
                        , "websiteName",websiteName
						, "websiteLink",websiteLink
                        , "description",description
                        , "parentId",parentId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {

                    var result = from p in _db.WebsiteLinks
                                 join c in _db.WebsiteLinks on p.ParentId equals c.WebsiteId into c_join
                                 from c in c_join.DefaultIfEmpty()
                                 select new WebsiteLinkModel()
                                 {
                                     WebsiteId = p.WebsiteId,
                                     WebsiteName = p.WebsiteName,
                                     WebLink = p.WebLink,
                                     Description = p.Description,
                                     ParentId = p.ParentId,
                                     WebsiteImage = p.WebsiteImage,
                                     ParentName = c.WebsiteName,
                                 };


                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        int iFilter = Convert.ToInt32(websiteId);
                        result = result.Where(p => p.WebsiteId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(websiteName))
                        result = result.Where(p => p.WebsiteName.Contains(websiteName));

                    if (!string.IsNullOrEmpty(websiteLink))
                        result = result.Where(p => p.WebLink.Contains(websiteLink));

                    if (!string.IsNullOrEmpty(description))
                        result = result.Where(p => p.Description.Contains(description));

                    if (!string.IsNullOrEmpty(parentId))
                    {
                        int iFilter = Convert.ToInt32(parentId);
                        if (iFilter == 0)
                            result = result.Where(p => p.ParentId.Equals(null));
                        else if (iFilter == -1)
                            result = result.Where(p => p.ParentId != null);
                        else
                            result = result.Where(p => p.ParentId == iFilter);
                    }

                    return result.OrderBy(x => x.ParentId).ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<WebsiteLinkModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public bool WebsiteLinkDelete(string websiteId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteLinkDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "websiteId",websiteId
			            });
            #endregion

            #region // Init:
            alParamsOutError = "";
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    int iFilter = Convert.ToInt32(websiteId);
                    // Check in Department
                    var query = _db.WebsiteAccounts.Where(p => p.WebsiteId == iFilter);
                    if (query != null && query.Count() > 0)
                    {
                        throw new Exception("WebsiteLink đã được sử dụng, bạn không thể xóa.");
                    }
                    var query2 = _db.WebsiteLinks.Where(p => p.ParentId == iFilter);
                    if (query2 != null && query.Count() > 0)
                    {
                        throw new Exception("WebsiteLink đã được sử dụng, bạn không thể xóa.");
                    }

                    #endregion

                    #region // Delete
                    WebsiteLink webLi = new WebsiteLink() { WebsiteId = iFilter };
                    _db.WebsiteLinks.Attach(webLi);
                    _db.WebsiteLinks.Remove(webLi);
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
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public WebsiteLink WebsiteLinkCreate(WebsiteLink websiteLink, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteLinkCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.WebsiteLinks.Add(websiteLink);
                    _db.SaveChanges();
                }
                return websiteLink;
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new WebsiteLink();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public WebsiteLink WebsiteLinkUpdate(WebsiteLink websiteLink, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteLinkUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    WebsiteLink webLi = _db.WebsiteLinks.Where(p => p.WebsiteId == websiteLink.WebsiteId).SingleOrDefault();
                    webLi.WebsiteId = websiteLink.WebsiteId;
                    webLi.WebsiteName = websiteLink.WebsiteName;
                    webLi.WebLink = websiteLink.WebLink;
                    webLi.Description = websiteLink.Description;
                    webLi.ParentId = websiteLink.ParentId;
                    _db.SaveChanges();

                    return webLi;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new WebsiteLink();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // Website Account

        #region // WebsiteAccountGet
        public List<WebsiteAccountModel> WebsiteAccountGet(string websiteAccountId
                                                                , string websiteId
                                                                , string parentId
                                                                , string accountWebsiteNo
                                                                , string website
                                                                , string remark
                                                                , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteAccountGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "websiteId",websiteId
						, "parentId",parentId
						, "accountWebsiteNo",accountWebsiteNo
                        , "website",website
						, "remark",remark
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.WebsiteAccounts
                                 join c in _db.WebsiteLinks on p.WebsiteId equals c.WebsiteId into c_join
                                 from c in c_join.DefaultIfEmpty()
                                 select new WebsiteAccountModel()
                                 {
                                     WebsiteAccountId = p.WebsiteAccountId,
                                     WebsiteId = p.WebsiteId,
                                     AccountWebsiteNo = p.AccountWebsiteNo,
                                     Website = p.Website,
                                     Remark = p.Remark,

                                     WebsiteName = c.WebsiteName,
                                     WebsiteLink = c.WebLink,
                                     Description = c.Description,
                                     ParentId = c.ParentId
                                 };


                    if (!string.IsNullOrEmpty(websiteAccountId))
                    {
                        int iFilter = Convert.ToInt32(websiteAccountId);
                        result = result.Where(p => p.WebsiteAccountId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        int iFilter = Convert.ToInt32(websiteId);
                        result = result.Where(p => p.WebsiteId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(parentId))
                    {
                        int iFilter = Convert.ToInt32(parentId);
                        result = result.Where(p => p.ParentId == iFilter);
                    }
                    
                    if (!string.IsNullOrEmpty(accountWebsiteNo))
                        result = result.Where(p => p.AccountWebsiteNo.Equals(accountWebsiteNo));

                    if (!string.IsNullOrEmpty(website))
                        result = result.Where(p => p.Website.Contains(website));

                    if (!string.IsNullOrEmpty(remark))
                        result = result.Where(p => p.Remark.Contains(remark));

                    return result.OrderBy(x => x.WebsiteAccountId).ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<WebsiteAccountModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // WebsiteAccountDelete
        public bool WebsiteAccountDelete(string websiteAccountId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteAccountDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "websiteAccountId",websiteAccountId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    int iFilter = Convert.ToInt32(websiteAccountId);
                    // Check in Department
                    //var query = _db.WebsiteAccounts.Where(p => p.WebsiteAccountId == iFilter);
                    //if (query != null && query.Count() > 0)
                    //{
                    //    throw new Exception("Sản phẩm được sử dụng, bạn không thể xóa.");
                    //}

                    #endregion

                    #region // Delete
                    WebsiteAccount webAc = new WebsiteAccount() { WebsiteAccountId = iFilter };
                    _db.WebsiteAccounts.Attach(webAc);
                    _db.WebsiteAccounts.Remove(webAc);
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
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // WebsiteAccountCreate
        public WebsiteAccount WebsiteAccountCreate(WebsiteAccount websiteAccount, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteAccountCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.WebsiteAccounts.Add(websiteAccount);
                    _db.SaveChanges();
                }
                return websiteAccount;
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new WebsiteAccount();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // WebsiteAccountUpdate
        public WebsiteAccount WebsiteAccountUpdate(WebsiteAccount websiteAccount, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "WebsiteAccountUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    WebsiteAccount webAc = _db.WebsiteAccounts.Where(p => p.WebsiteAccountId == websiteAccount.WebsiteAccountId).SingleOrDefault();
                    webAc.WebsiteAccountId = websiteAccount.WebsiteAccountId;
                    webAc.WebsiteId = websiteAccount.WebsiteId;
                    webAc.AccountWebsiteNo = websiteAccount.AccountWebsiteNo;
                    webAc.WebsiteAccountId = websiteAccount.WebsiteAccountId;
                    webAc.Website = websiteAccount.Website;
                    webAc.Remark = websiteAccount.Remark;
                    
                    _db.SaveChanges();

                    return webAc;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new WebsiteAccount();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #endregion

		#region // Website Account Visa

		#region // WebsiteAccountVisaGet
		public List<WebsiteAccountVisa> WebsiteAccountVisaGet(string websiteAccountVisaId
																, string websiteAccountId
																, string visaNo
																, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "WebsiteAccountVisaGet";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "websiteAccountVisaId",websiteAccountVisaId
						, "websiteAccountId",websiteAccountId
						, "visaNo",visaNo
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					var result = from p in _db.WebsiteAccountVisas
								 select p;

					if (!string.IsNullOrEmpty(websiteAccountId))
					{
						int iFilter = Convert.ToInt32(websiteAccountId);
						result = result.Where(p => p.WebsiteAccountId == iFilter);
					}

					if (!string.IsNullOrEmpty(websiteAccountVisaId))
					{
						int iFilter = Convert.ToInt32(websiteAccountVisaId);
						result = result.Where(p => p.WebsiteAccountVisaId == iFilter);
					}

					if (!string.IsNullOrEmpty(visaNo))
						result = result.Where(p => p.VisaNo == visaNo);

					return result.OrderBy(x => x.VisaNo).ToList();
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<WebsiteAccountVisa>();
			}
			finally
			{
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

		#region // WebsiteAccountVisaCreate
		public WebsiteAccountVisa WebsiteAccountVisaCreate(WebsiteAccountVisa websiteAccountVisa, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "WebsiteAccountVisaCreate";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion
			try
			{
				using (var _db = new EcmsEntities())
				{
					_db.WebsiteAccountVisas.Add(websiteAccountVisa);
					_db.SaveChanges();
				}
				return websiteAccountVisa;
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new WebsiteAccountVisa();
			}
			finally
			{
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

		#region // WebsiteAccountVisaDelete
		public bool WebsiteAccountVisaDelete(string websiteAccountVisaId, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "WebsiteAccountDelete";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "websiteAccountVisaId",websiteAccountVisaId
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					#region // Check:
					int iFilter = Convert.ToInt32(websiteAccountVisaId);
					// Check in Department
					var query = _db.WebsiteAccountVisas.Where(p => p.WebsiteAccountId == iFilter);

					// Số visa mặc định
					var orders = _db.OrderOutbounds.Where(p => p.WebsiteAccountVisaId == iFilter);
					if (orders != null && orders.Count()>0)
					{
						throw new Exception("Số Visa đã được sử dụng, bạn không thể xóa.");
					}

					#endregion

					#region // Delete
					WebsiteAccountVisa webAc = new WebsiteAccountVisa() { WebsiteAccountVisaId = iFilter };
					_db.WebsiteAccountVisas.Attach(webAc);
					_db.WebsiteAccountVisas.Remove(webAc);
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
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

		#endregion
	}

    #region WebsiteAccountModel
    public class WebsiteAccountModel
    {
        public Int32? WebsiteAccountId { set; get; }
        public Int32? WebsiteId { set; get; }
        public String AccountWebsiteNo { set; get; }
		//public String VisaNoDefault { set; get; }
        public String Website { set; get; }
        public String Remark { set; get; }

        public String WebsiteName { set; get; }
        public String WebsiteLink { set; get; }
        public String Description { set; get; }
        public Int32? ParentId { get; set; }
    }
    #endregion

    #region WebsiteLinkModel
    public class WebsiteLinkModel
    {
        public Int32? WebsiteId { set; get; }
        public String WebsiteName { set; get; }
        public String WebLink { set; get; }
        public String Description { set; get; }
        public Int32? ParentId { get; set; }
        public String WebsiteImage { set; get; }
        public String ParentName { set; get; }
        
    }
    #endregion
}
