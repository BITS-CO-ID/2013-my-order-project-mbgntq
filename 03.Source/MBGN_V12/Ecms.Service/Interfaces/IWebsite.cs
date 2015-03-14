using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz.Class;

namespace Ecms.Biz.Interfaces
{
    public interface IWebsite
    {
        #region // Website Link
        List<WebsiteLinkModel> WebsiteLinkGet(string websiteId, string websiteName, string websiteLink, string description, string parentId, ref string alParamsOutError);

        bool WebsiteLinkDelete(string websiteId, ref string alParamsOutError);

        WebsiteLink WebsiteLinkCreate(WebsiteLink websiteLink, ref string alParamsOutError);

        WebsiteLink WebsiteLinkUpdate(WebsiteLink websiteLink, ref string alParamsOutError);
        #endregion


        #region // Website Account
        List<WebsiteAccountModel> WebsiteAccountGet(string websiteAccountId, string websiteId, string parentId, string accountWebsiteNo, string website, string remark, ref string alParamsOutError);

        bool WebsiteAccountDelete(string websiteAccountId, ref string alParamsOutError);

        WebsiteAccount WebsiteAccountCreate(WebsiteAccount websiteAccount, ref string alParamsOutError);

        WebsiteAccount WebsiteAccountUpdate(WebsiteAccount websiteAccount, ref string alParamsOutError);
        #endregion


    }
}
