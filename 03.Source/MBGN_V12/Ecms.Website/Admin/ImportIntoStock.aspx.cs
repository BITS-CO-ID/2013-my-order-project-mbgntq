using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Site.MBGN;
using Ecms.Website.Common;
using System.Configuration;
using System.Data;
using System.IO;
using Ecms.Biz.Entities;
using CommonUtils;

namespace Ecms.Website.Admin
{
    public partial class ImportIntoStock : PageBase
    {
        #region //Declares

        private readonly StockService _stockService = new StockService();
        private readonly ProductService _productService = new ProductService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/admin/security/login.aspx");
                LoadData();
            }
        }

        protected void ddlMethodImport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMethodImport.SelectedValue.Equals("0"))
            {
                trProductCode.Visible = trPrice.Visible = trSerial.Visible = true;
                trFileProduct.Visible = false;
                btnAdd.Text = "Thêm vào danh sách";
            }
            else
            {
                trProductCode.Visible = trPrice.Visible = trSerial.Visible = false;
                trFileProduct.Visible = true;
                btnAdd.Text = "Upload";
            }
            Session.Remove("CartImport");
            LoadData();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlMethodImport.SelectedValue.Equals("0"))
            {
                AddToListProduct(txtProductCode.Text,txtSerial.Text, txtPrice.Text);
            }
            else
            {
                UploadAndAddToListProduct();
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["CartImport"] != null)
                {
                    var carts = (List<CartModel>)Session["CartImport"];
                    var stockInOut = new Inv_StockInOut();
                    stockInOut.Type = 1;
                    if (Session["User"] != null)
                    {
                        stockInOut.CreatedUser = Session["User"].ToString();
                    }
					stockInOut.status = StockStatus.StockConfirmed;
					stockInOut.InOutDate = DateTime.Now;
					stockInOut.Remark = "Nhập kho";

                    foreach (var item in carts)
                    {
                        var listResult = _stockService.StockInOutDetailGet("", "", item.Serial, this);
                        if (listResult != null)
                        {
                            if (listResult.Count != 0)
                            {
                                lblError.Text = string.Format("Serial <span style='color:#000000;'>{0}</span> đã tồn tại trong hệ thống!",item.Serial);
                                lblError.Visible = true;
                                return;
                            }
                        }
                        var stockInOutDetail = new Inv_StockInOutDetail();
                        stockInOutDetail.ProductId = item.ProductId;
                        stockInOutDetail.Price = item.Price;
                        stockInOutDetail.Quantity = 1;
                        stockInOutDetail.Serial = item.Serial;                        
                        stockInOut.Inv_StockInOutDetail.Add(stockInOutDetail);
                    }
                    var stockInOutReturn = _stockService.StockInOutCreate(stockInOut, this);
                    if (stockInOutReturn != null)
                    {
						Session.Remove("CartImport");

                        //lblMessage.Text = "Bạn đã tạo đơn nhập hàng thành công, để xác nhận nhập kho vui lòng nhấn nút nhập kho.";
                        //btnConfirmImport.Visible = lblMessage.Visible = true;
                        //btnImport.Visible = false;
                        Session["StockInOutReturn"] = stockInOutReturn;						
						mtvMain.ActiveViewIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/importstockmanage.aspx");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/importstockmanage.aspx");
        }

        protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "productDelete":
                        var productId = Convert.ToInt32(e.CommandArgument);
                        if (Session["CartImport"] != null)
                        {
                            var productCart = (List<CartModel>)Session["CartImport"];
                            if (productCart.Count != 0)
                            {
                                var product = productCart.Where(x => x.ProductId == productId).FirstOrDefault();

                                if (product != null)
                                {
                                    productCart.Remove(product);
                                    Session["CartImport"] = productCart;
                                    LoadData();
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnConfirmImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["StockInOutReturn"] != null)
                {
                    var stockInOut = (Inv_StockInOut)Session["StockInOutReturn"];
                    if (stockInOut != null)
                    {
                        stockInOut.status = 2;
                        var stockInOutReturn = _stockService.StockInOutUpdate(stockInOut, this);
                        if (stockInOutReturn != null)
                        {
                            var listGoods = new List<Inv_Goods>();
                            foreach (var item in stockInOut.Inv_StockInOutDetail.ToList())
                            {
                                var goods = new Inv_Goods();
                                goods.ProductId = item.ProductId;
                                goods.Serial = item.Serial;
                                goods.StockInId = stockInOut.StockInOutId;
                                listGoods.Add(goods);
                            }
                            _stockService.GoodsCreate(listGoods, this);
                            mtvMain.ActiveViewIndex = 1;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData();
        }

        #endregion
        
        #region //Private methods

        private void AddToListProduct(string productCode,string serial, string productPrice)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                lblError.Text = "Bạn chưa nhập mã sản phẩm!";
                lblError.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(serial))
            {
                lblError.Text = "Bạn chưa nhập serial sản phẩm!";
                lblError.Visible = true;
                return;
            }


            if (string.IsNullOrEmpty(productPrice))
            {
                lblError.Text = "Bạn chưa nhập giá sản phẩm!";
                lblError.Visible = true;
                return;
            }

            try
            {
                //Lấy thông tin sản phẩm
				var productReturn = _productService.ProductGet("", txtProductCode.Text, "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                if (productReturn != null)
                {
                    List<CartModel> carts = new List<CartModel>();
                    CartModel productTemp = new CartModel();
                    if (Session["CartImport"] != null)
                    {
                        carts = (List<CartModel>)Session["CartImport"];                        
                        var product = carts.Where(x => x.Serial == serial).FirstOrDefault();
                        if (product != null)
                        {
                            lblError.Text = "Serial sản phẩm này đã có trong danh sách";
                            lblError.Visible = true;
                            return;
                        }
                        else
                        {
                            
                            productTemp.ProductId = productReturn.ProductId.Value;
                            productTemp.ProductName = productReturn.ProductName;
                            productTemp.ProductCode = productReturn.ProductCode;
                            productTemp.CategoryName = productReturn.CategoryName;
                            productTemp.Serial = serial;
                            productTemp.Price = Convert.ToInt32(txtPrice.Text.Replace(",", ""));
                        }
                    }
                    else
                    {
                        productTemp.ProductId = productReturn.ProductId.Value;
                        productTemp.ProductName = productReturn.ProductName;
                        productTemp.ProductCode = productReturn.ProductCode;
                        productTemp.CategoryName = productReturn.CategoryName;
                        productTemp.Serial = serial;
                        productTemp.Price = Convert.ToInt32(txtPrice.Text.Replace(",", ""));
                    }
                    carts.Add(productTemp);
                    Session["CartImport"] = carts;
                    txtPrice.Text = txtProductCode.Text = txtSerial.Text = "";
                    LoadData();
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Mã sản phẩm không tồn tại!";
                    return;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private void UploadAndAddToListProduct()
        {
            if (ValidData() == false)
                return;

            try
            {
                var uploadFolder = ConfigurationManager.AppSettings["UploadFolder"].ToString();
                var directory = Server.MapPath(string.Format("~/{0}/", uploadFolder));

                if (!Directory.Exists(directory))
                {
                    var dir = Directory.CreateDirectory(directory);
                }
                string pathFile = directory + fupProductList.FileName;
                fupProductList.SaveAs(pathFile);

                List<CartModel> listProduct = new List<CartModel>();
                if (fupProductList.FileName.EndsWith("csv"))
                {
                    listProduct = ParseCsv(pathFile);
                }

                if (listProduct == null)
                {
                    pnData.Visible = false;
                    return;
                }

                if (listProduct.Count != 0)
                {
                    Session["CartImport"] = listProduct;
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Text = "File không có dữ liệu!";
                    lblError.Visible = true;
                }
                LoadData();
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private bool ValidData()
        {
            if (!fupProductList.HasFile)
            {
                lblError.Text = "Bạn chưa chọn file!";
                lblError.Visible = true;
                return false;
            }

            if (fupProductList.PostedFile.ContentLength > 20 * 1024 * 1024)
            {
                lblError.Text = "Dung lượng file phải nhỏ hơn 20 MB.";
                lblError.Visible = true;
                return false;
            }

            if (!fupProductList.PostedFile.FileName.EndsWith(".csv"))
            {
                lblError.Text = "Tên file không đúng định dạng (.csv)";
                lblError.Visible = true;
                return false;
            }
            return true;
        }

        private void LoadData()
        {
            if (Session["CartImport"] != null)
            {
                List<CartModel> carts = (List<CartModel>)Session["CartImport"];
                if (carts.Count == 0)
                {
                    pnData.Visible = false;
                }
                else
                {
                    gridMain.DataSource = carts;
                    gridMain.DataBind();
                    pnData.Visible = true;
                }
            }
            else
            {
                pnData.Visible = false;
            }
        }
        
        public List<CartModel> ParseCsv(string pathFile)
        {
            List<string[]> parsedData = new List<string[]>();
            try
            {
                using (StreamReader readFile = new StreamReader(pathFile))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }

            FileStream stream = null;
            bool fileCanOpen;
            try
            {
                stream = File.Open(pathFile, FileMode.Open, FileAccess.Read);
                fileCanOpen = true;
            }
            catch
            {
                fileCanOpen = false;
            }
            List<CartModel> listResult = new List<CartModel>();
            try
            {
                if (fileCanOpen)
                {
                    foreach (var row in parsedData)
                    {
                        if (!string.IsNullOrEmpty(row[0].ToString()))
                        {
							var productReturn = _productService.ProductGet("", row[0].ToString(), "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                            CartModel productTemp = new CartModel();
                            if (productReturn != null)
                            {
                                if (listResult.Count != 0)
                                {
                                    var p = listResult.Where(x => x.Serial == row[1].ToString()).FirstOrDefault();
                                    if (p != null)
                                    {
                                        lblError.Text = string.Format("Serial <span style='color:#000000;'> {0} </span> trong danh sách trùng nhau", row[1].ToString());
                                        lblError.Visible = true;                                        
                                        return null;
                                    }
                                }
                                productTemp.ProductId = productReturn.ProductId.Value;
                                productTemp.ProductName = productReturn.ProductName;
                                productTemp.ProductCode = productReturn.ProductCode;
                                productTemp.CategoryName = productReturn.CategoryName;
                                productTemp.Serial = row[1].ToString();
                                productTemp.Price = Convert.ToDouble(row[2].ToString());
                                listResult.Add(productTemp);
                            }
                        }
                    }
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                stream.Close();
                Utils.ShowExceptionBox(ex, this);
            }
            finally {
                stream.Close();
            }
            return listResult;
        }

        #endregion
    }
}