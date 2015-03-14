using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using CommonUtils;

namespace Ecms.Website.Site.MBGN
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        #region //Declares

        private readonly ProductService _productService = new ProductService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnAddToCard_Click(object sender, EventArgs e)
        {
            try
            {
                List<CartModel> carts = new List<CartModel>();
                CartModel productTemp = new CartModel();
                if (Request.QueryString["proId"] != null)
                {
                    var productId = Convert.ToInt32(Request.QueryString["proId"].ToString());
                    if (Session["Cart"] != null)
                    {
                        carts = (List<CartModel>)Session["Cart"];
                        var product = carts.SingleOrDefault(x => x.ProductId == productId);

                        if (product != null)
                        {
                            productTemp = product;
                            //Update quantity product
                            carts.Remove(product);
                            productTemp.Quantity += 1;
                        }
                        else
                        {
							var productReturn = _productService.ProductGet(productId + "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                            if (productReturn != null)
                            {
                                //Add new product
                                productTemp.ProductId = productId;
                                productTemp.ProductName = productReturn.ProductName;
                                productTemp.Quantity = 1;
								productTemp.Price = (productReturn.ProductSaleValue == null ? (productReturn.ProductValue ?? 0) : (productReturn.ProductSaleValue ?? 0));
                            }
                        }
                    }
                    else
                    {
						var productReturn = _productService.ProductGet(productId + "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                        if (productReturn != null)
                        {
                            //Add new product
                            productTemp.ProductId = productId;
                            productTemp.ProductName = productReturn.ProductName;
                            productTemp.Quantity = 1;
							productTemp.Price = (productReturn.ProductSaleValue == null ? (productReturn.ProductValue ?? 0) : (productReturn.ProductSaleValue ?? 0));
                        }
                    }
                    carts.Add(productTemp);
                    Session["Cart"] = carts;
                    Response.Redirect("~/site/mbgn/Cart.aspx");
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            try
            {
                if (Request.QueryString["proId"] != null)
                {
					var product = _productService.ProductGet(Request.QueryString["proId"], "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                    if (product != null)
                    {
                        litImage.Text = string.Format("<a href=\"{0}\" class=\"MYCLASS\"><img width='300' height='300' src=\"{1}\" /></a>", product.Image, product.Image);
                        lblProductName.Text = product.ProductName;
                        lblShortDescription.Text = product.ShortDescription;
						lblProductType.Text = product.ProductType == Const_ProductType.ProductType_Order ? Const_ProductType.ProductType_OrderText
												: (product.ProductType == Const_ProductType.ProductType_InStock ? (product.StockBalance == 0 ? Const_QuantitySatus.QuantitySatus_NoHaveText : Const_QuantitySatus.QuantitySatus_HaveText) : "");
						lblProductStatus.Text = product.ProductStatusText;
						lblPrice.Text = (product.ProductSaleValue==null? (product.ProductValue ?? 0):(product.ProductSaleValue??0)).ToString("N0") + " đ";
                        lblDescription.Text = product.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion
    }

    public class CartModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Serial { get; set; }
        public double Money { get { return Quantity * Price; } set { value = Quantity * Price; } }
    }

}