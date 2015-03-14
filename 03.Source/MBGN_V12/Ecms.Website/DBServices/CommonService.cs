using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Net;
using CommonUtils;

namespace Ecms.Website.DBServices
{
    public class CommonService : BaseService
    {
        #region // Constructs

        public CommonService()
            : base()
        {

        }
        #endregion

        #region // Province

        public List<Province> ProvinceList(string provinceId, string provinceName, string provinceCode)
        {
            try
            {
                var provinceList = this._commonBiz.ProvinceList("", "", "");
                return provinceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region // Bank
        public List<Mst_Bank> BankList(string bankId, string bankName)
        {
            try
            {
                var bankList = this._commonBiz.BankGet("", "");
                return bankList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region // Website

        public List<WebsiteLink> WebsiteList(string websiteId, string websiteName, string link)
        {
            try
            {
                var websiteList = this._commonBiz.WebsiteLinkGet(websiteId, websiteName, link);
                return websiteList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region // News

        public List<News> NewsGet(string newsId, string title, string parentId, Page page)
        {
            try
            {
                var news = _commonBiz.NewsGet(newsId, title, parentId);
                return news;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<News>();
            }
        }

        #endregion

        #region // Menu

        public List<Menu> MenuGet(string menuId, string menuCode, string menuName, Page page)
        {
            try
            {
                var menus = _commonBiz.MenuGet(menuId, menuCode, menuName);
                return menus;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<Menu>();
            }
        }

        #endregion

        #region // Country

        public List<Country> CountryGet(string countryId, string countryCode, string countryName, Page page)
        {
            try
            {
                var countries = _commonBiz.CountryGet(countryId, countryCode, countryName);
                return countries;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<Country>();
            }
        }

        #endregion

		#region // VisaAccount

		public List<Mst_VisaAccount> VisaAccountGet(string visaId, string visaNo, Page page)
		{
			try
			{
				var countries = _commonBiz.VisaAccountGet(visaId, visaNo);
				return countries;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return new List<Mst_VisaAccount>();
			}
		}

		public bool DeleteVisaAccount(string visaId, Page page)
		{
			try
			{
				var result = _commonBiz.DeleteVisaAccount(visaId);
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}

		public Mst_VisaAccount AddVisaAccount(Mst_VisaAccount visaObj, Page page)
		{
			try
			{
				var result = _commonBiz.AddVisaAccount(visaObj);
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return null;
			}
		}

		public bool UpdateVisaAccount(Mst_VisaAccount visaObj, Page page)
		{
			try
			{
				var result = _commonBiz.UpdateVisaAccount(visaObj);
				return result;
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, page);
				return false;
			}
		}
		#endregion

        #region // SendMailConfirmedOrder

        public void SendMailConfirmedOrder(string orderId, string pathFileTemplate, Page page)
        {
            try
            {
                string alParamsOutError = "";
				var order = _orderBiz.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", ref alParamsOutError).FirstOrDefault();

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);

                var reader = new StreamReader(@pathFileTemplate);

                var file = reader.ReadToEnd();
                reader.Close();
                string strContent = "";
                string strTitle = "";

                #region //Trả lời báo giá
                if (order.OrderTypeId == 1 )
                {
                    strTitle = "Trả lời báo giá";
                    var strOrderDetail = new StringBuilder();
                    int stt = 0;
                    foreach (var item in order.lstOrderDetailModel)
                    {
                        strOrderDetail.Append("<tr>");

                        var str = string.Format("<td style='text-align:center;'>{0}</td><td>{1}</td><td>{2}</td><td style='text-align:center;'><a href='{3}' target='_blank'><img width='50' height='50' src='{4}'/></a></td><td><a href='{5}'>{6}</a></td><td>{7}</td><td style='text-align:center;'>{8}</td><td>{9}</td><td>{10}</td><td style='text-align:right;'>{11}</td><td style='text-align:right;'>{12}</td><td style='text-align:center;'>{13}</td><td style='text-align:center;'>{14}</td><td style='text-align:center;'>{15}</td><td style='text-align:right;'>{16}</td><td style='text-align:right;'>{17}</td><td style='text-align:right;'>{18}</td><td style='text-align:center;'>{19}</td>",
                            ++stt
							, item.WebsiteName
							, item.CountryName
							, item.ImageUrl
							, item.ImageUrl
							, item.ProductLink
							, item.ProductLink.Length < 30 ? item.ProductLink : (item.ProductLink.Substring(0, 30) + "...")
							, item.CategoryName
							, (item.Weight ?? 0).ToString("N0")
							, item.Color
							, item.Size
							, (item.PriceWeb ?? 0).ToString("N2")
							, item.PriceWebOff != null ? item.PriceWebOff.Value.ToString("N2") : "", (item.Quantity ?? 0).ToString("N0")
							, (item.EffortModified == null ? (item.EffortConfigValue ?? 0).ToString("N2") : (item.EffortModified ?? 0).ToString("N2"))
							, (item.TaxUsaConfigValue ?? 0).ToString("N2")
							//, (item.ShipConfigValue ?? 0).ToString("N2")
							, (item.ShipUSA ?? 0).ToString("N2")
							, (item.Surcharge ?? 0).ToString("N2")
							, item.CurrencyCode
							, (item.TotalMoney).ToString("N0")
                        );
                        strOrderDetail.Append(str);
                        strOrderDetail.Append("</tr>");
                    }
                    strContent = string.Format(file
						, order.OrderNo
						, order.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")
						, order.CustomerCode
						, order.Mobile
						, order.CustomerName
						, order.Address
						, order.Remark
						, strOrderDetail.ToString()
						, (order.SumAmount ?? 0).ToString("N0"));
                }

                #endregion

                #region //Đặt hàng mua hộ

				if (order.OrderTypeId == 2 && order.OrderStatus == OrderInStatus.OrderConfirmed)
                {
                    strTitle = "Xác nhận đặt hàng mua hộ";
					var strOrderBuilder = new StringBuilder();
					var strOrder = string.Format("<td style='text-align:center;'>{0}</td><td style='text-align:center;'>{1}</td><td style='text-align:center;'>{2}</td><td style='text-align:center;'>{3}</td><td style='text-align:center;'>{4}</td><td style='text-align:center; font-bold:true'>{5}</td>"
						, (order.SumAmount??0).ToString("N2")
						, (order.TotalPayAmountNormal).ToString("N2")
						, (order.SumFeeShip??0).ToString("N2")
						, (order.AmountCalFeeDelay).ToString("N2")
						, (order.AmountFeeDelay).ToString("N2")
						, (order.RemainAmount??0).ToString("N2")
						);
					strOrderBuilder.Append("<tr>");
					strOrderBuilder.Append(strOrder);
					strOrderBuilder.Append("</tr>");

                    var strOrderDetail = new StringBuilder();
                    int stt = 0;
                    foreach (var item in order.lstOrderDetailModel)
                    {
                        strOrderDetail.Append("<tr>");

                        var str = string.Format("<td style='text-align:center;'>{0}</td><td>{1}</td><td>{2}</td><td style='text-align:center;'><a href='{3}' target='_blank'><img width='50' height='50' src='{4}'/></a></td><td><a href='{5}'>{6}</a></td><td>{7}</td><td style='text-align:center;'>{8}</td><td>{9}</td><td>{10}</td><td style='text-align:right;'>{11}</td><td style='text-align:right;'>{12}</td><td style='text-align:center;'>{13}</td><td style='text-align:center;'>{14}</td><td style='text-align:center;'>{15}</td><td style='text-align:right;'>{16}</td><td style='text-align:right;'>{17}</td><td style='text-align:right;'>{18}</td><td style='text-align:center;'>{19}</td>",
                            ++stt
							, item.WebsiteName
							, item.CountryName
							, item.ImageUrl
							, item.ImageUrl
							, item.ProductLink
							, item.ProductLink.Length < 30 ? item.ProductLink : (item.ProductLink.Substring(0, 30) + "...")
							, item.CategoryName
							, (item.Weight ?? 0).ToString("N0")
							, item.Color
							, item.Size
							, (item.PriceWeb ?? 0).ToString("N2")
							, item.PriceWebOff != null ? item.PriceWebOff.Value.ToString("N2") : ""
							, (item.Quantity ?? 0).ToString("N0")
							, (item.EffortModified == null ? (item.EffortConfigValue ?? 0).ToString("N2") : (item.EffortModified ?? 0).ToString("N2"))
							, (item.TaxUsaConfigValue ?? 0).ToString("N2")
							//, (item.ShipModified != null ? (item.ShipModified ?? 0) : (item.ShipConfigValue ?? 0)).ToString("N2")
							, (item.ShipUSA ?? 0).ToString("N2")
							, (item.Surcharge ?? 0).ToString("N2")
							, item.CurrencyCode
							, (item.TotalMoney).ToString("N2")
                        );
                        strOrderDetail.Append(str);
                        strOrderDetail.Append("</tr>");
                    }
                    strContent = string.Format(file
						, order.OrderNo
						, order.CreatedDate.Value.ToString("dd/MM/yyyy")
						, order.TrackingNo
						, order.OrderNoDelivery
						, order.CustomerCode
						, order.Mobile
						, order.CustomerName
						, order.Address
						, order.Remark
						, strOrderBuilder.ToString() 
						, strOrderDetail.ToString());
                }

				if (order.OrderTypeId == 2 && order.OrderStatus == OrderInStatus.Finished)
				{
					strTitle = "Xác nhận hoàn thành đơn hàng mua hộ";
					var strOrderBuilder = new StringBuilder();
					var strOrder = string.Format("<td style='text-align:center;'>{0}</td><td style='text-align:center;'>{1}</td><td style='text-align:center;'>{2}</td><td style='text-align:center;'>{3}</td><td style='text-align:center;'>{4}</td><td style='text-align:center; font-bold:true'>{5}</td>"
						, (order.SumAmount ?? 0).ToString("N2")
						, (order.TotalPayAmountNormal).ToString("N2")
						, (order.SumFeeShip ?? 0).ToString("N2")
						, (order.AmountCalFeeDelay).ToString("N2")
						, (order.AmountFeeDelay).ToString("N2")
						, (order.RemainAmount ?? 0).ToString("N2")
						);
					strOrderBuilder.Append("<tr>");
					strOrderBuilder.Append(strOrder);
					strOrderBuilder.Append("</tr>");

					var strOrderDetail = new StringBuilder();
					int stt = 0;
					foreach (var item in order.lstOrderDetailModel)
					{
						strOrderDetail.Append("<tr>");

						var str = string.Format("<td style='text-align:center;'>{0}</td><td>{1}</td><td>{2}</td><td style='text-align:center;'><a href='{3}' target='_blank'><img width='50' height='50' src='{4}'/></a></td><td><a href='{5}'>{6}</a></td><td>{7}</td><td style='text-align:center;'>{8}</td><td>{9}</td><td>{10}</td><td style='text-align:right;'>{11}</td><td style='text-align:right;'>{12}</td><td style='text-align:center;'>{13}</td><td style='text-align:center;'>{14}</td><td style='text-align:center;'>{15}</td><td style='text-align:right;'>{16}</td><td style='text-align:right;'>{17}</td><td style='text-align:right;'>{18}</td><td style='text-align:center;'>{19}</td>",
							++stt
							, item.WebsiteName
							, item.CountryName
							, item.ImageUrl
							, item.ImageUrl
							, item.ProductLink
							, item.ProductLink.Length < 30 ? item.ProductLink : (item.ProductLink.Substring(0, 30) + "...")
							, item.CategoryName
							, (item.Weight ?? 0).ToString("N0")
							, item.Color
							, item.Size
							, (item.PriceWeb ?? 0).ToString("N2")
							, item.PriceWebOff != null ? item.PriceWebOff.Value.ToString("N2") : ""
							, (item.Quantity ?? 0).ToString("N0")
							, (item.EffortModified == null ? (item.EffortConfigValue ?? 0).ToString("N2") : (item.EffortModified ?? 0).ToString("N2"))
							, (item.TaxUsaConfigValue ?? 0).ToString("N2")
							//, (item.ShipModified != null ? (item.ShipModified ?? 0) : (item.ShipConfigValue ?? 0)).ToString("N2")
							, (item.ShipUSA ?? 0).ToString("N2")
							, (item.Surcharge ?? 0).ToString("N2")
							, item.CurrencyCode
							, (item.TotalMoney).ToString("N2")
						);
						strOrderDetail.Append(str);
						strOrderDetail.Append("</tr>");
					}
					strContent = string.Format(file
						, order.OrderNo
						, order.CreatedDate.Value.ToString("dd/MM/yyyy")
						, order.TrackingNo
						, order.OrderNoDelivery
						, order.CustomerCode
						, order.Mobile
						, order.CustomerName
						, order.Address
						, order.Remark
						, strOrderBuilder.ToString()
						, strOrderDetail.ToString());
				}

                #endregion

                #region //Nhờ chuyển hàng

                if (order.OrderTypeId == 3)
                {
                    strTitle = "Xác nhận vận chuyển gửi hàng";
                    var strOrderDetail = new StringBuilder();
                    int stt = 0;
                    foreach (var item in order.lstOrderDetailModel)
                    {
                        strOrderDetail.Append("<tr>");
						var str = string.Format("<td style='text-align:center;'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td style='text-align:right;'>{5}</td><td style='text-align:right;'>{6}</td><td style='text-align:right;'>{7}</td><td style='text-align:right;'>{8}</td><td style='text-align:center;'>{9}</td><td style='text-align:right;'>{10}</td>",
                                                ++stt
												, item.TrackingNo
												, item.OrderNoDelivery
												, item.CategoryName
												, item.ProductName
												, (item.Quantity ?? 0).ToString("N2")
												, (item.ShipModified != null ? (item.ShipModified ?? 0) : (item.ShipConfigValue ?? 0)).ToString("N2")
												, (item.Weight ?? 0).ToString("N2")
												, (item.Surcharge ?? 0).ToString("N2")
												, item.DeliveryVNDate==null?"":item.DeliveryVNDate.Value.ToString("dd/MM/yyyy")												
												, (item.TotalMoney).ToString("N2")
                                                );
                        strOrderDetail.Append(str);
                        strOrderDetail.Append("</tr>");
                    }
                    strContent = string.Format(file
								, order.OrderNo
								, order.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")
								, order.CustomerCode
								, order.Mobile
								, order.CustomerName
								, order.Address
								, order.Remark
								, strOrderDetail.ToString()
								, (order.SumAmount ?? 0).ToString("N0"));
                }
                #endregion

                CommonUtils.SendmailHelper.SendInfo(strContent, strTitle + " - MUABANGIAONHAN.COM", order.Email);
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
            }
        }

        #endregion
    }
}