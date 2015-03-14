using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz.Entities;
using System.Web.UI;
using Ecms.Website.Common;
using Ecms.Biz;

namespace Ecms.Website.DBServices
{
    public class ProductService : BaseService
    {
        #region // Constructs

        public ProductService()
            : base()
        {

        }
        #endregion

        #region // CategoryGet

        public List<Category> CategoryGet(string categoryId, string categoryName, string parentId, Page page)
        {
			try
			{
				string alParamsOutError = "";
				var category = this._productBiz.CategoryGet(categoryId
								, categoryName
								, parentId
								, ""
								, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return category;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<Category>();
			}
        }

        #endregion

        #region // CategoryDelete
        public bool CategoryDelete(string categoryId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result = false;
                result = this._productBiz.CategoryDelete(categoryId, ref alParamsOutError);
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

        #region // CategoryCreate
        public Category CategoryCreate(Category category, Page page)
        {
            try
            {
                string alParamsOutError = "";
                Category result;
                result = this._productBiz.CategoryCreate(category, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new Category();
            }
        }
        #endregion

        #region // CategoryUpdate
        public Category CategoryUpdate(Category category, Page page)
        {
            try
            {
                string alParamsOutError = "";
                Category result;
                result = this._productBiz.CategoryUpdate(category, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new Category();
            }
        }
        #endregion


		#region // ProductGet

		public List<ProductModel> ProductGet(
					string productId
					, string productCode
					, string productName
					, string productValue
					, string categoryId
					, string parentCategoryId
					, string published
					, string tag
					, string vendorId
					, string original
					, string bestSale
					, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var product = this._productBiz.ProductGet(productId,
															productCode,
															productName,
															productValue,
															categoryId,parentCategoryId,
															published,
															tag,
															vendorId,
															original,
															bestSale,
															ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return product;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<ProductModel>();
			}
		}
		#endregion

		#region // ProductyDelete
		public bool ProductyDelete(string productId, Page page)
		{
			try
			{
				string alParamsOutError = "";
				bool result = false;
				result = this._productBiz.ProductDelete(productId, ref alParamsOutError);
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

		#region // ProductCreate
		public Product ProductCreate(Product product, Page page)
		{
            try
            {
                string alParamsOutError = "";
                Product result;
                result = this._productBiz.ProductCreate(product, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new Product();
            }
		}
		#endregion

		#region // ProductUpdate
		public Product ProductUpdate(Product product, Page page)
		{
			try
			{
				string alParamsOutError = "";
				Product result;
				result = this._productBiz.ProductUpdate(product, ref alParamsOutError);
				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new Product();
			}
		}
		#endregion
		#region // ProductUpdate
		public bool ProductBestSaleUpdate(string strProductId, string bestSale, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var result = this._productBiz.ProductBestSaleUpdate(strProductId, bestSale, ref alParamsOutError);
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

		#region // VendorGet

		public List<Vendor> VendorGet(string vendorId, string vendorName, Page page)
		{
			try
			{
				string alParamsOutError = "";
				var vendor = this._productBiz.VendorGet(vendorId, vendorName, ref alParamsOutError);

				if (!string.IsNullOrEmpty(alParamsOutError))
					throw GenServiceException(alParamsOutError);
				return vendor;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<Vendor>();
			}
		}
		#endregion
	}
}