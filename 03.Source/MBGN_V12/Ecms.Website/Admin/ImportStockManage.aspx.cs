using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using System.Globalization;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Biz.Class;

namespace Ecms.Website.Admin
{
    public partial class ImportStockManage : PageBase
    {        
        #region //Declares

        private readonly StockService _stockService = new StockService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/importintostock.aspx");
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Session[Constansts.SS_STOCK_INOUT_LIST_ADMIN] == null)
            {
                Response.Redirect("~/admin/importstockmanage.aspx");
            }

            int id = Convert.ToInt32(e.CommandArgument);
            var inv_stockInOut = (List<Inv_StockInOutModel>)Session[Constansts.SS_STOCK_INOUT_LIST_ADMIN];

            var inv_stockInOut_First = inv_stockInOut.Where(p => p.StockInOutId == id).FirstOrDefault();

            if (inv_stockInOut_First == null)
            {
                return;
            }
            Session[Constansts.SS_STOCK_INOUT_ADMIN] = inv_stockInOut_First;
            switch (e.CommandName)
            {
                case "StockInOutDetail":
                    Response.Redirect("~/admin/importstockdetail.aspx");
                    break;
            }
        }

        #endregion

        #region //Private methods

        private void InitData()
        {
            try
            {
                if (Session["User"] == null)
                    Response.Redirect("~/admin/security/login.aspx");

                var currentDate = DateTime.Now;
                //Lấy ngày đầu tháng
                DateTime fDate = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
                txtFromDate.Text = fDate.ToString("dd/MM/yyyy");
                txtToDate.Text = currentDate.ToString("dd/MM/yyyy");
                //Set data vào calendar conntrol
                cldFromDate.SelectedDate = fDate;
                cldToDate.SelectedDate = currentDate;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private void LoadData()
        { 
            try
            {
                DateTime fDate = new DateTime();
                DateTime tDate = new DateTime();
                if (ValidData(ref fDate, ref tDate) == false)
                    return;

                var listResult = _stockService.StockInOutGet(fDate.ToString("yyyy-MM-dd 00:00:00"),
                                                            tDate.ToString("yyyy-MM-dd 23:59:59"),
                                                            txtInOutCode.Text, this).Where(p=>p.Type==StockType.StockIn).ToList();
                if (listResult.Count == 0)
                {
                    gridMain.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                    return;
                }

                gridMain.Visible = true;
                lblError.Visible = false;
                gridMain.DataSource = listResult;
                gridMain.DataBind();
                Session[Constansts.SS_STOCK_INOUT_LIST_ADMIN] = listResult;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private bool ValidData(ref DateTime fDate, ref DateTime tDate)
        {
            if (string.IsNullOrEmpty(txtFromDate.Text))
            {
                lblError.Text = "Từ ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                fDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtToDate.Text))
            {
                lblError.Text = "Đến ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                tDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Đến ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            return true;
        }
        #endregion
    }
}