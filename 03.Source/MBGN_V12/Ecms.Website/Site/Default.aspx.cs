using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;

using Ecms.Website.DBServices;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz;
using CommonUtils;

namespace Ecms.Website.Site
{
    public partial class Default : System.Web.UI.Page
    {
        #region // Declares

        private readonly ProductService _productService = new ProductService();
        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region // Private methods

        private void LoadData()
        {
            try
            {
                //Get all product
                var strCate = new StringBuilder();
                var strToolTipProduct = new StringBuilder();
                int sticky = 0;
                var products = _productService.ProductGet(
								""
								, ""
								, ""
								, ""
								, ""
								, ""
								, CommonUtils.Constansts.FlagActive
								, ""
								, ""
								, ""
								, CommonUtils.Constansts.FlagActive // Chỉ lấy những sản phẩm đang BestSale
								, this);

				var pageIndex = string.IsNullOrEmpty(Request.QueryString["pageIndex"]) ? 1 : Convert.ToInt32(Request.QueryString["pageIndex"]);
				PagingModels<ProductModel> pagingModel = new PagingModels<ProductModel>(products.AsQueryable(), pageIndex, 8);
				var strPager = this.PageLinks(pagingModel.PagingInfo, pageIndex + "", ResolveUrl("~/site/default.aspx"));
				litPagerProduct.Text = strPager;

				foreach (var p in pagingModel.Models) //products.Take(8))
                {
                    var strProduct = new StringBuilder();
                    //strProduct.Append("<li><a alt='{0}' href='/site/mbgn/ProductDetail.aspx?proId={1}'><div class='img clearfix'>");
					// Tạm khóa xem trang chi tiết
					strProduct.Append("<li><a alt='{0}' href='#'><div class='img clearfix'>");
                    strProduct.Append("<img width='120' height='120' alt='{2}' src='{3}' data-tooltip='sticky{4}' />");
					strProduct.Append("</div><div class='name clearfix'><p>{5}</p>");
                    strProduct.Append("<div class='price-list clearfix'>");
                    strProduct.Append("<cite class='price'>{6} đ</cite> <del class='old-price'>{7} đ</del></div></div></a></li>");

                    string strProductFormat = string.Format(strProduct.ToString()
															, p.ProductName
															, p.ProductId
															, p.ProductName
															, p.Image
															, (sticky)
															, p.ProductName.Length > 12 ? string.Format("{0}...", p.ProductName.Substring(0, 12)) : p.ProductName
															//, p.ShortDescription.Length > 20 ? string.Format("{0}...", p.ShortDescription.Substring(0, 20)) : p.ShortDescription
															, p.ProductSaleValue == null ? (p.ProductValue ?? 0).ToString("N0") : (p.ProductSaleValue ?? 0).ToString("N0")
															, (p.ProductValue ?? 0).ToString("N0"));
                    strCate.Append(strProductFormat);

                    var strToolTip = new StringBuilder();
                    strToolTip.Append("<div id='sticky{0}' class='atip' style='line-height: 26px;'>");
					strToolTip.Append("Tên sản phẩm: <font style='color: red; font-weight: bold; text-transform: uppercase;'>{1}</font><br />");
                    strToolTip.Append("Mã sản phẩm: <strong>{2}</strong><br />");
					strToolTip.Append("Thương hiệu: <strong>{3}</strong><br />");
					strToolTip.Append("Xuất xứ: <strong>{4}</strong><br />");
                    strToolTip.Append("Giá bán: <font style='color: #0066cc; font-weight: bold;'>{5} đ</font><br />");
                    strToolTip.Append("Kho hàng: <strong>{6}</strong></div>");

					var strToolTipFormat = string.Format(strToolTip.ToString()
											, sticky
											, p.ProductName
											, p.ProductCode
											, p.ProductLable//p.CategoryName
											, p.GOrginal
											, p.ProductSaleValue == null ? (p.ProductValue ?? 0).ToString("N0") : (p.ProductSaleValue ?? 0).ToString("N0")
											, p.ProductType == Const_ProductType.ProductType_Order ? Const_ProductType.ProductType_OrderText
												: (p.ProductType == Const_ProductType.ProductType_InStock ? (p.StockBalance == 0 ? Const_QuantitySatus.QuantitySatus_NoHaveText : Const_QuantitySatus.QuantitySatus_HaveText) : "")
											);

                    strToolTipProduct.Append(strToolTipFormat);
                    sticky++;
                }
                litStickyToolTip.Text = strToolTipProduct.ToString();
                litProductBox.Text = strCate.ToString();

				

				//#region // Sale nóng
				//var listSaleHot = _commonService.NewsGet("", "", "", this).Where(x => x.Type == 1);
				////Begin paging

				//var pageIndex = string.IsNullOrEmpty(Request.QueryString["pageIndex"]) ? 1 : Convert.ToInt32(Request.QueryString["pageIndex"]);
				//PagingModels<News> pagingModel = new PagingModels<News>(listSaleHot.AsQueryable(), pageIndex, 5);
				//var strPager = this.PageLinks(pagingModel.PagingInfo, pageIndex + "", ResolveUrl("~/site/default.aspx"));
				//litPager.Text = strPager;

				////End Paging
				//rptSaleHot.DataSource = pagingModel.Models.OrderByDescending(x => x.CreatedDate);
				//rptSaleHot.DataBind();
				//#endregion
			}
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion
    }
}