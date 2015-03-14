using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Biz
{
    public interface IProductBiz
    {
        #region // Customer Type
        List<Category> CategoryGet(string categoryId, string categoryName, string parentId, string remark, ref string alParamsOutError);

        bool CategoryDelete(string categoryId, ref string alParamsOutError);

        Category CategoryCreate(Category category, ref string alParamsOutError);

        Category CategoryUpdate(Category category, ref string alParamsOutError);
        #endregion

		#region // Product
		List<ProductModel> ProductGet(string productId, string productCode, string productName, string productValue, string parentCategoryId, string categoryId
										, string published, string tag, string vendorId, string original, string bestSale, ref string alParamsOutError);

		bool ProductDelete(string productId, ref string alParamsOutError);

		Product ProductCreate(Product product, ref string alParamsOutError);

		Product ProductUpdate(Product product, ref string alParamsOutError);
		#endregion

		#region // VendorGet
		List<Vendor> VendorGet(string vendorId,string vendorName, ref string alParamsOutError);
		#endregion
	}
}
