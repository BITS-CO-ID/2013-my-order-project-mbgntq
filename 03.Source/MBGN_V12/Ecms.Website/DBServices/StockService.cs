using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz;
using Ecms.Biz.Class;
using Ecms.Biz.Entities;
using System.Web.UI;
using Ecms.Website.Common;

namespace Ecms.Website.DBServices
{
    public class StockService : BaseService
    {
        #region //Declares

        private readonly StockBiz _stockBiz = new StockBiz();

        #endregion

        public List<Inv_StockInOutModel> StockInOutGet(string fromDate, string toDate, string stockInOutNo, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var listResult = _stockBiz.StockInOutGet(fromDate, toDate, stockInOutNo, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return listResult;
            }catch(Exception ex)
            {                
                Utils.ShowExceptionBox(ex, page);
                return new List<Inv_StockInOutModel>();
            }
        }

        #region //StockInOutCreate
        
        public Inv_StockInOut StockInOutCreate(Inv_StockInOut stockInOut, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var stockInOutReturn = _stockBiz.StockInOutCreate(stockInOut, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return stockInOutReturn;
            }catch(Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        #endregion

        #region //StockInOutCreate

        public Inv_StockInOut StockInOutUpdate(Inv_StockInOut stockInOut, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var stockInOutReturn = _stockBiz.StockInOutUpdate(stockInOut, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return stockInOutReturn;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        #endregion

        #region //StockInOutDetailGet

        public List<Inv_StockInOutDetail> StockInOutDetailGet(string stockInOutDetailId, string productId, string serial, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var listResult = _stockBiz.StockInOutDetailGet(stockInOutDetailId, productId, serial, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return listResult;
            }
            catch(Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return null;
            }
        }

        #endregion

        #region //GoodsCreate
        public void GoodsCreate(List<Inv_Goods> listGoods, Page page)
        {
            try
            {
                string alParamsOutError = "";
                _stockBiz.GoodsCreate(listGoods, ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);

            }catch(Exception ex)
            {
                Utils.ShowExceptionBox(ex,page);
            }
        }

        #endregion
    }
}