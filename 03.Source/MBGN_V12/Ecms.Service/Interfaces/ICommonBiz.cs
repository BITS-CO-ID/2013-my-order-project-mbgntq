using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Biz
{
    public interface ICommonBiz
    {
        #region // Company
        List<Company> GetCompany(string companyId, string companyName, string director);

        void DeleteCompany(string companyId);

        void AddCompany(Company company);

        void UpdateCompany(Company company);
        #endregion

        #region //News

        void NewsCreate(News tutorial);
        void NewsUpdate(News tutorial);
        void NewsDelete(string tutorialId);
        List<News> NewsGet(string tutorialId, string tutorialTitle, string parentId);

        #endregion

        #region //Province

        List<Province> ProvinceList(string provinceId, string provinceName, string provinceCode);

        #endregion

        #region //Bank

        List<Mst_Bank> BankGet(string bankId, string bankName);

        #endregion

        #region //Website

        List<WebsiteLink> WebsiteLinkGet(string websiteId, string websiteName, string link);

        #endregion

        #region //Menu
        List<Menu> MenuGet(string menuId, string menuCode, string menuName);
        #endregion

        #region //Country

        List<Country> CountryGet(string countryId, string countryCode, string countryName);
        
        #endregion
    }
}
