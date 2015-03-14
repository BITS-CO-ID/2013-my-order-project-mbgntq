using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;


namespace Ecms.Biz
{
    public interface ICustomerBiz
    {
		List<UserCustomerModel> UserCustomerGet(
				string customerId
				, string customerName
				, string customerCode
				, string email
                , string mobile
                , string address
                , string cityId
				, string userCode
				, string customerTypeId
				, string status
				, string flagAdmin
				, ref string alParamsOutError);

		Customer CustomerCreate(string userName, string password, string fullName, string mobile, string email, string address, string province, ref string alParamsOutError);

		bool CustomerLockUnlock(string customerId
			, string isLock // lock=1; unlock=0
			, ref string alParamsOutError);

        #region // Customer Type
        List<CustomerType> CustomerTypeGet(string customerTypeId, string customerTypeName, ref string alParamsOutError);

        bool CustomerTypeDelete(string customerTypeId, ref string alParamsOutError);

        CustomerType CustomerTypeCreate(CustomerType customerType, ref string alParamsOutError);

        CustomerType CustomerTypeUpdate(CustomerType customerType, ref string alParamsOutError);
        #endregion
    }
}
