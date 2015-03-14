using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using Ecms.Biz;
using Ecms.Biz.Class;

namespace Ecms.Website.Admin.Report
{
    public partial class ReportLiabilityDetail : System.Web.UI.Page
    {
        #region //Declares

        private readonly ReportService _reportService = new ReportService();
        protected string[] Totals = new string[2];
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData(1);
            }
        }

        #endregion

        #region //Private methods

		protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gridMain.PageIndex = e.NewPageIndex;
			LoadData(1);
		}

        private void LoadData(int type)
        {
            if (type == 1)
            {
                try
                {
					var rptModel=(ReportLiabilityModel)Session["rptModel"];

					this.lblDetail.Text = string.Format("<b>Chi tiết giao dịch của khách hàng:</b> {0} - {1}", rptModel.CustomerCode, rptModel.CustomerName);
					this.lblDateDetail.Text = string.Format("<b>Trong khoảng thời gian Từ ngày:</b> {0} <b>Tới ngày:</b> {1}", Convert.ToDateTime(Request.QueryString["fromDate"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(Request.QueryString["toDate"]).ToString("dd/MM/yyyy"));
					this.lblSDDK.Text = string.Format("<b>Số dư đầu kỳ:</b> {0}", Convert.ToDouble(Request.QueryString["beforeBalance"]).ToString("N0"));

					var listResult = _reportService.ReportLiabilityDetailGet(
										Request.QueryString["fromDate"]
										, string.IsNullOrEmpty(Request.QueryString["toDate"]) == true ? "" : Convert.ToDateTime(Request.QueryString["toDate"]).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")
										, Convert.ToString(rptModel.CustomerId)
										, ""
										, ""
										, ""
										, this);

                    if (listResult.Count != 0)
                    {
						Totals[0] = (listResult.Sum(x => x.Amount ?? 0).ToString("N0")) ;//+ Convert.ToDouble(Request.QueryString["beforeBalance"])).ToString("N0");

                        lblError.Visible = false;
                        gridMain.Visible = true;
                        gridMain.DataSource = listResult;
                        gridMain.DataBind();						
                        Session["ReportLiabilityDetailGet"] = listResult;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Data not found!";
                        gridMain.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    Utils.ShowExceptionBox(ex, this);
                }
            }

            if (type == 2)
            {
                if (Session["ReportLiabilityDetailGet"] != null)
                {
                    var listResult = (List<InvoiceDetailModel>)Session["ReportLiabilityDetailGet"];
                    if (listResult.Count != 0)
                    {
						Totals[0] = (listResult.Sum(x => x.Amount ?? 0)).ToString("N0"); // + Convert.ToDouble(Request.QueryString["beforeBalance"])).ToString("N0");

                        lblError.Visible = false;
                        gridMain.Visible = true;
                        gridMain.DataSource = listResult;
                        gridMain.DataBind();
						
                    }
                }
            }
        }

        #endregion
    }
}