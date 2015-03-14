using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;
using System.Collections;
using System.Transactions;


namespace Ecms.Biz
{
    public class CommonBiz : ICommonBiz
    {
        #region // Company

        #region // GetCompany
        public List<Company> GetCompany(string companyId, string companyName, string director)
        {
            #region // Temp
            string strFunctionName = "GetCompany";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "companyId",companyId
						, "companyName",companyName
						, "director",director
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Companies
                                 select p;


                    if (!string.IsNullOrEmpty(companyId))
                    {
                        int iFilter = Convert.ToInt32(companyId);
                        result = result.Where(p => p.CompanyId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(companyName))
                        result = result.Where(p => p.CompanyName == companyName);

                    if (!string.IsNullOrEmpty(director))
                        result = result.Where(p => p.Director == director);

                    var lstResult = new List<Company>();
                    foreach (var item in result)
                    {
                        var company = new Company
                        {
                            //Balance = ((from t in _db.BudgetBalances
                            //            where t.CompanyId == item.CompanyId
                            //            select new { t.Balance }).Sum(p => p.Balance) ?? 0)
                            //,
                            Balance = 0
                            ,
                            BalanceBank = 0
                                    ,
                            CompanyId = item.CompanyId
                                    ,
                            CompanyName = item.CompanyName
                                     ,
                            Director = item.Director
                                     ,
                            DUser = item.DUser
                                     ,
                            Description = item.Description
                        };
                        lstResult.Add(company);
                    }
                    return lstResult.ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Company>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // DeleteCompany
        public void DeleteCompany(string companyId)
        {
            #region // Temp
            string strFunctionName = "DeleteCompany";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "companyId",companyId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    //int iFilter = Convert.ToInt32(companyId);
                    //// Check in Department
                    //var query = _db.Departments.Where(p => p.CompanyId == iFilter);
                    //if (query != null && query.Count() > 0)
                    //{
                    //    throw new Exception("Công ty đã được sử dụng, bạn không thể xóa.");
                    //}

                    //// Check transaction
                    //var queryTran = _db.Transactions.Where(p => p.CompanyId == iFilter);
                    //if (queryTran != null && queryTran.Count() > 0)
                    //{
                    //    throw new Exception("Công ty đã được sử dụng, bạn không thể xóa.");
                    //}
                    #endregion

                    #region // Delete
                    Company company = new Company() { CompanyId = Convert.ToInt32(companyId) };
                    _db.Companies.Attach(company);
                    _db.Companies.Remove(company);
                    _db.SaveChanges();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // AddCompany
        public void AddCompany(Company company)
        {
            #region // Temp
            string strFunctionName = "AddCompany";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.Companies.Add(company);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // UpdateCompany
        public void UpdateCompany(Company company)
        {
            #region // Temp
            string strFunctionName = "UpdateCompany";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Company com = _db.Companies.Where(p => p.CompanyId == company.CompanyId).SingleOrDefault();

                    com.CompanyName = company.CompanyName;
                    com.Description = company.Description;
                    com.Director = company.Director;
                    com.DUser = company.DUser;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #endregion

        #region // News

        #region //NewsCreate

        public void NewsCreate(News news)
        {
            #region // Temp
            string strFunctionName = "NewsCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
                "strFunctionName", strFunctionName
                    });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.News.Add(news);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region //NewsUpdate

        public void NewsUpdate(News news)
        {
            #region // Temp
            string strFunctionName = "NewsUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    News tut = _db.News.Where(p => p.NewsId == news.NewsId).SingleOrDefault();
                    tut.Title = news.Title;
                    tut.ShortContent = news.ShortContent;
                    tut.NewsContent = news.NewsContent;
                    tut.ParentId = news.ParentId;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // NewsDelete

        public void NewsDelete(string newsId)
        {
            #region // Temp
            string strFunctionName = "NewsDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "newsId",newsId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    News news = new News() { NewsId = Convert.ToInt32(newsId) };
                    _db.News.Attach(news);
                    _db.News.Remove(news);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // NewsGet

        public List<News> NewsGet(string newsId, string title, string parentId)
        {
            #region // Temp
            string strFunctionName = "NewsGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "newsId",newsId
						, "title",title
						, "parentId",parentId
			            });
            #endregion

            try
            {
                var _db = new EcmsEntities();
                var listTutorial = _db.News.AsQueryable();
                if (!string.IsNullOrEmpty(newsId))
                {
                    int tutId = Convert.ToInt32(newsId);
                    listTutorial = listTutorial.Where(x => x.NewsId == tutId);
                }

                if (!string.IsNullOrEmpty(title))
                {
                    listTutorial = listTutorial.Where(x => x.Title == title);
                }

                if (!string.IsNullOrEmpty(parentId))
                {
                    int parId = Convert.ToInt32(parentId);
                    listTutorial = listTutorial.Where(x => x.ParentId == parId);
                }

                return listTutorial.ToList();

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<News>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #endregion

        #region // Province

        public List<Province> ProvinceList(string provinceId, string provinceName, string provinceCode)
        {
            #region // Temp
            string strFunctionName = "ProvinceList";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "provinceId",provinceId
						, "provinceName",provinceName
						, "provinceCode",provinceCode
			            });
            #endregion

            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    var provinceList = dbContext.Provinces.AsQueryable();
                    if (!string.IsNullOrEmpty(provinceId))
                    {
                        int proId = Convert.ToInt32(provinceId);
                        provinceList = provinceList.Where(x => x.CityId == proId);
                    }

                    if (!string.IsNullOrEmpty(provinceName))
                    {
                        provinceList = provinceList.Where(x => x.CityName == provinceName);
                    }

                    if (!string.IsNullOrEmpty(provinceCode))
                    {
                        provinceList = provinceList.Where(x => x.CityCode == provinceCode);
                    }

                    return provinceList.OrderBy(x => x.CityName).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Province>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // Bank

        public List<Mst_Bank> BankGet(string bankId, string bankName)
        {
            #region // Temp
            string strFunctionName = "BankGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "bankId",bankId
						, "bankName",bankName
			            });
            #endregion
            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    var banks = dbContext.Mst_Bank.AsQueryable();
                    if (!string.IsNullOrEmpty(bankId))
                    {
                        int bId = Convert.ToInt32(bankId);
                        banks = banks.Where(x => x.BankId == bId);
                    }

                    if (!string.IsNullOrEmpty(bankName))
                    {
                        banks = banks.Where(x => x.BankName == bankName);
                    }
                    return banks.OrderBy(x => x.BankName).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Mst_Bank>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // Website

        public List<WebsiteLink> WebsiteLinkGet(string websiteId, string websiteName, string link)
        {
            #region // Temp
            string strFunctionName = "WebsiteLinkGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "websiteId",websiteId
						, "websiteName",websiteName
                        , "link",link
			            });
            #endregion
            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    var websites = dbContext.WebsiteLinks.AsQueryable();
                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        int wId = Convert.ToInt32(websiteId);
                        websites = websites.Where(x => x.WebsiteId == wId);
                    }

                    if (!string.IsNullOrEmpty(websiteName))
                    {
                        websites = websites.Where(x => x.WebsiteName == websiteName);
                    }

                    if (!string.IsNullOrEmpty(link))
                    {
                        websites = websites.Where(x => x.WebLink == link);
                    }
                    return websites.OrderBy(x => x.WebsiteName).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<WebsiteLink>();
            }
            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // Menu
        public List<Menu> MenuGet(string menuId, string menuCode, string menuName)
        {
            #region // Temp
            string strFunctionName = "MenuGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "menuId",menuId
						, "menuCode",menuCode
                        , "menuName",menuName
			            });
            #endregion
            try
            {
                var dbContext = new EcmsEntities();
                var menu = dbContext.Menus.AsQueryable();
                if (!string.IsNullOrEmpty(menuId))
                {
                    int mnId = Convert.ToInt32(menuId);
                    menu = menu.Where(x => x.MenuId == mnId);
                }

                if (!string.IsNullOrEmpty(menuCode))
                {
                    menu = menu.Where(x => x.MenuCode == menuCode);
                }

                if (!string.IsNullOrEmpty(menuName))
                {
                    menu = menu.Where(x => x.MenuName == menuName);
                }

                return menu.ToList();
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Menu>();
            }

            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // Country

        public List<Country> CountryGet(string countryId, string countryCode, string countryName)
        {
            #region // Temp
            string strFunctionName = "CountryGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "countryId",countryId
						, "countryCode",countryCode
                        , "countryName",countryName
			            });
            #endregion
            try
            {
                using (var dbContext = new EcmsEntities())
                {
                    var countries = dbContext.Countries.AsQueryable();

                    if (!string.IsNullOrEmpty(countryId))
                    {
                        var Id = Convert.ToInt32(countryId);
                        countries = countries.Where(x => x.CountryId == Id);
                    }

                    if (!string.IsNullOrEmpty(countryCode))
                        countries = countries.Where(x => x.CountryCode == countryCode);

                    if (!string.IsNullOrEmpty(countryCode))
                        countries = countries.Where(x => x.CountryName == countryName);

                    return countries.OrderBy(x => x.CountryName).OrderBy(p=>p.CountryCode).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Country>();
            }

            finally
            {
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion        

		#region // VisaAccount
		#region // VisaAccountGet
		public List<Mst_VisaAccount> VisaAccountGet(string visaId, string visaNo)
		{
			#region // Temp
			string strFunctionName = "VisaAccountGet";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "visaId",visaId
						, "visaNo",visaNo
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					var result = from p in _db.Mst_VisaAccount
								 select p;

					if (!string.IsNullOrEmpty(visaId))
					{
						int iFilter = Convert.ToInt32(visaId);
						result = result.Where(p => p.VisaId == iFilter);
					}

					if (!string.IsNullOrEmpty(visaNo))
						result = result.Where(p => p.VisaNo == visaNo);

					return result.ToList();
				}
			}
			catch (Exception ex)
			{
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<Mst_VisaAccount>();
			}
			finally
			{
				NLogLogger.PublishParamater(alParamsCoupleError.ToArray());
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		#endregion

		#region // DeleteVisaAccount
		public bool DeleteVisaAccount(string visaId)
		{
			#region // Temp
			string strFunctionName = "DeleteVisaAccount";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "visaId",visaId
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					#region // Check:

					#endregion

					#region // Delete
					Mst_VisaAccount visa = new Mst_VisaAccount() { VisaId = Convert.ToInt32(visaId) };
					_db.Mst_VisaAccount.Attach(visa);
					_db.Mst_VisaAccount.Remove(visa);
					_db.SaveChanges();
					return true;
					#endregion
				}
			}
			catch (Exception ex)
			{
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

		#region // AddVisaAccount
		public Mst_VisaAccount AddVisaAccount(Mst_VisaAccount visaObj)
		{
			#region // Temp
			string strFunctionName = "AddVisaAccount";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion
			try
			{
				using (var _db = new EcmsEntities())
				{
					_db.Mst_VisaAccount.Add(visaObj);
					_db.SaveChanges();
					return visaObj;
				}
			}
			catch (Exception ex)
			{
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

		#region // UpdateVisaAccount
		public bool UpdateVisaAccount(Mst_VisaAccount visaObj)
		{
			#region // Temp
			string strFunctionName = "UpdateVisaAccount";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					Mst_VisaAccount vsa = _db.Mst_VisaAccount.Where(p => p.VisaId == visaObj.VisaId).SingleOrDefault();

					vsa.VisaNo = visaObj.VisaNo;
					vsa.Remark = visaObj.Remark;
					_db.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
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
	}

    #region // Model


    #endregion
}
