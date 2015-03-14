using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;
using System.Collections;

namespace Ecms.Biz
{
    public class ProductBiz : IProductBiz
    {
        #region // Category

        #region // CategoryGet
        public List<Category> CategoryGet(string categoryId, string categoryName, string parentId, string remark, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CategoryGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "categoryId",categoryId
						, "categoryName",categoryName
                        , "parentId",parentId
                        , "remark",remark
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Categories
                                 select p;


                    if (!string.IsNullOrEmpty(categoryId))
                    {
                        int iFilter = Convert.ToInt32(categoryId);
                        result = result.Where(p => p.CategoryId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(categoryName))
                        result = result.Where(p => p.CategoryName.Contains(categoryName));

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

                    if (!string.IsNullOrEmpty(remark))
                        result = result.Where(p => p.Remark.Contains(remark));

                    var lstResult = new List<Category>();
                    foreach (var item in result)
                    {
                        var category = new Category
                        {
                            CategoryId = item.CategoryId,
                            CategoryName = item.CategoryName,
                            ParentId = item.ParentId,
                            Remark = item.Remark
                        };
                        lstResult.Add(category);
                    }
                    return lstResult.ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Category>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // CategoryDelete
        public bool CategoryDelete(string categoryId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CategoryDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "categoryId",categoryId
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
                    int iFilter = Convert.ToInt32(categoryId);
                    // Check in Category
                    var query = _db.Categories.Where(p => p.ParentId == iFilter);
                    if (query != null && query.Count() > 0)
                    {
                        throw new Exception("Danh mục đã được sử dụng, bạn không thể xóa.");
                    }

                    #endregion

                    #region // Delete
                    Category cate = new Category() { CategoryId = iFilter };
                    _db.Categories.Attach(cate);
                    _db.Categories.Remove(cate);
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

        #region // CategoryCreate
        public Category CategoryCreate(Category category, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CategoryCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return category;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new Category();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // CategoryUpdate
        public Category CategoryUpdate(Category category, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "CategoryUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Category cate = _db.Categories.Where(p => p.CategoryId == category.CategoryId).SingleOrDefault();

                    cate.CategoryId = category.CategoryId;
                    cate.CategoryName = category.CategoryName;
                    cate.ParentId = category.ParentId;
                    cate.Remark = category.Remark;
                    _db.SaveChanges();
                    return category;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new Category();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        

        #endregion

		#region // Product

		public List<ProductModel> ProductGet(string productId
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
				, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "CategoryGet";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "productId",productId
						, "productCode",productCode
						, "productName",productName
                        , "productValue",productValue
                        , "categoryId",categoryId
						, "parentCategoryId",parentCategoryId
						, "tag",tag
						, "vendorId",vendorId
						, "original",original
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					var result = from p in _db.Products
								 join sb in _db.Inv_StockBalance on p.ProductId equals sb.ProductId into sb_join
								 from sb in sb_join.DefaultIfEmpty()
								 select new ProductModel()
								 {
									 ProductId = p.ProductId,
									 ProductCode = p.ProductCode,
									 ProductName = p.ProductName,
									 ProductValue = p.ProductValue,
									 ProductSaleValue = p.ProductSaleValue,
									 CategoryId = p.CategoryId,
									 ParentCategoryId =p.Category.ParentId,
									 Size = p.Size,
									 Image = p.Image,
									 Published = p.Published,
									 Tag = p.Tag,
									 ShortDescription = p.ShortDescription,
									 Description = p.Description,
									 VendorId = p.VendorId,
									 Orginal = p.Orginal,
									 CategoryName = p.Category.CategoryName,
									 gVendorId =p.Vendor.VendorName,
									 BestSale=p.BestSale,
									 ProductLable=p.ProductLable,
									 ProductType=p.ProductType,
									 PPriority=p.PPriority,
									 LastDateModify=p.LastDateModify,
									 ProductStatus=p.ProductStatus,
									 Balance= p.Balance??0,
									 StockBalance = sb.Quantity??0
								 };

					if (!string.IsNullOrEmpty(productId))
					{
						int iFilter = Convert.ToInt32(productId);
						result = result.Where(p => p.ProductId == iFilter);
					}

					if (!string.IsNullOrEmpty(productCode))
						result = result.Where(p => p.ProductCode==productCode);

					if (!string.IsNullOrEmpty(productName))
						result = result.Where(p => p.ProductName.Contains(productName));

					if (!string.IsNullOrEmpty(productValue))
					{
						double iFilter = Convert.ToDouble(productValue);
						result = result.Where(p => p.ProductValue == iFilter);
					}

					if (!string.IsNullOrEmpty(categoryId))
					{
						int iFilter = Convert.ToInt32(categoryId);
						result = result.Where(p => p.CategoryId == iFilter);
					}

					if (!string.IsNullOrEmpty(parentCategoryId))
					{
						int iFilter = Convert.ToInt32(parentCategoryId);
						result = result.Where(p => p.ParentCategoryId == iFilter);
					}

					if (!string.IsNullOrEmpty(published))
					{
						if (published.Equals(Constansts.FlagActive))
						{
							result = result.Where(p => p.Published == true);
						}

						if (published.Equals(Constansts.FlagInActive))
						{
							result = result.Where(p => p.Published == false);
						}
					}

					if (!string.IsNullOrEmpty(tag))
					{
						result = result.Where(p => p.Tag.Contains(tag));
					}

					if (!string.IsNullOrEmpty(vendorId))
					{
						int iFilter = Convert.ToInt32(vendorId);
						result = result.Where(p => p.VendorId == iFilter);
					}

					if (!string.IsNullOrEmpty(original))
					{
						result = result.Where(p => p.Orginal == original);
					}

					if (!string.IsNullOrEmpty(bestSale))
					{
						result = result.Where(p => p.BestSale == bestSale);
					}

					List<ProductModel> resultProductModel = new List<ProductModel>();
					resultProductModel = result.OrderByDescending(x => x.LastDateModify).ToList();

					return resultProductModel;
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new List<ProductModel>();
			}
			finally
			{
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}

		public bool ProductDelete(string productId, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "DeleteCustomerType";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "productId",productId
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					#region // Check:
					int iFilter = Convert.ToInt32(productId);
					// Check in Department
					var query = _db.OrderDetails.Where(p => p.ProductId == iFilter);
					if (query != null && query.Count() > 0)
					{
						throw new Exception("Sản phẩm được sử dụng, bạn không thể xóa.");
					}

					#endregion

					#region // Delete
					Product product = new Product() { ProductId = iFilter };
					_db.Products.Attach(product);
					_db.Products.Remove(product);
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

		public Product ProductCreate(Product product, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ProductCreate";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion
			try
			{
				using (var _db = new EcmsEntities())
				{
					_db.Products.Add(product);
					_db.SaveChanges();
				}
				return product;
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new Product();
			}
			finally
			{
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}

		public Product ProductUpdate(Product product, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ProductUpdate";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					Product pro = _db.Products.Where(p => p.ProductId == product.ProductId).SingleOrDefault();

					pro.ProductId = product.ProductId;
					pro.ProductName = product.ProductName;
					pro.ProductValue = product.ProductValue;
					pro.CategoryId = product.CategoryId;
					pro.Size = product.Size;
					pro.Image = product.Image;
					pro.Published = product.Published;
					pro.Tag = product.Tag;
					pro.ShortDescription = product.ShortDescription;
					pro.Description = product.Description;
					pro.VendorId = product.VendorId;
					pro.Orginal = product.Orginal;
					pro.BestSale = product.BestSale;
					pro.ProductSaleValue = product.ProductSaleValue;
					pro.ProductLable = product.ProductLable;
					pro.ProductType = product.ProductType;
					pro.PPriority = product.PPriority;

					pro.LastDateModify = DateTime.Now;
					_db.SaveChanges();

					return pro;
				}
			}
			catch (Exception ex)
			{
				alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
				NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
				return new Product();
			}
			finally
			{
				NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
		}
		

		public bool ProductBestSaleUpdate(string strProductId, string bestSale, ref string alParamsOutError)
		{
			#region // Temp
			string strFunctionName = "ProductBestSaleUpdate";
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "productId", strProductId
						, "bestSale", bestSale
			            });
			#endregion

			try
			{
				using (var _db = new EcmsEntities())
				{
					int productId = Convert.ToInt32(strProductId);
					var product = _db.Products.SingleOrDefault(p => p.ProductId == productId);
					if (product == null)
					{
						throw new Exception("Không có thông tin sản phẩm");
					}

					if (!string.IsNullOrEmpty(bestSale))
					{
						product.BestSale = bestSale;
					}
					_db.SaveChanges();

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

        #region // Vendor

        public List<Vendor> VendorGet(string vendorId, string vendorName, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "VendorGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "vendorId",vendorId
						,"vendorName", vendorName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Vendors
                                 select p;


                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        int iFilter = Convert.ToInt32(vendorId);
                        result = result.Where(p => p.VendorId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(vendorName))
                        result = result.Where(p => p.VendorName.Contains(vendorName));



                    var lstResult = new List<Vendor>();
                    foreach (var item in result)
                    {
                        var vendor = new Vendor
                        {
                            VendorId = item.VendorId,
                            VendorName = item.VendorName,
                            Remark = item.Remark
                        };
                        lstResult.Add(vendor);
                    }
                    return lstResult.ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Vendor>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

    }

    #region // ProductModel
    public class ProductModel
    {
        public Int32? ProductId { set; get; }
        public String ProductName { set; get; }
        public Double? ProductValue { set; get; }
		public Double? ProductSaleValue { set; get; }
        public Int32? CategoryId { get; set; }
        public Int32? ParentCategoryId { get; set; }
        public String Size { set; get; }
        public String Image { set; get; }
        public Boolean? Published { set; get; }
        public String Tag { set; get; }
        public String ShortDescription { set; get; }
        public String Description { set; get; }
        public Int32? VendorId { set; get; }
        public String gVendorId { set; get; }

		private string gOrginal;
        public String GOrginal { 
			get
			{
				switch (Orginal)
							{
								case "0":
									 return "Mỹ";
									//break;
								case "1":
									return "Anh";
									//break;
								case "2":
									return "Khác";
									//break;
								default:
									return "";
									//break;
							}
			}
			set
			{
				gOrginal = value;
			}
		}
        public String Orginal { set; get; }
        public String CategoryName { set; get; }
        public String ProductCode { get; set; }
		public String BestSale { get; set; }
		public Double Balance { set; get; }
		public Double StockBalance { set; get; }
		public String ProductLable { get; set; }
		public String ProductType { get; set; }
		public Int32? PPriority { get; set; }
		public DateTime? LastDateModify { get; set; }
		public Int32? ProductStatus { get; set; }

		private String productStatusText;
		public String ProductStatusText
		{
			get
			{
				if (ProductStatus == Const_ProductStatus.ProductStatus_New)
				{
					return Const_ProductStatus.ProductStatus_NewText;
				}

				if (ProductStatus == Const_ProductStatus.ProductStatus_Secondhand)
				{
					return Const_ProductStatus.ProductStatus_SecondhandText;
				}
				return "";
			}

			set
			{
				productStatusText = value;
			}
		}

		private String productTypeText;
		public String ProductTypeText
		{
			get
			{
				if (ProductType == Const_ProductType.ProductType_Order)
				{
					return Const_ProductType.ProductType_OrderText;
				}

				if (ProductType == Const_ProductType.ProductType_InStock)
				{
					return Const_ProductType.ProductType_InStockText;
				}
				return "";
			}

			set
			{
				productTypeText = value;
			}
		}
    }

    #endregion

}
