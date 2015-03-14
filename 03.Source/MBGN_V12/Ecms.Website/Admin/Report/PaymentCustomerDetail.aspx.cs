using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Biz;

namespace Ecms.Website.Admin.Report
{
	public partial class PaymentCustomerDetail : PageBase
    {
        #region //Declares

        private readonly InvoiceService _invoiceService = new InvoiceService();
		protected double dSumAmount = 0;
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                LoadData(1);
            }
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData(2);
        }

        #endregion

        #region //Private methods

        private void InitData()
        {
            try
            {
				if (Session["User"] == null || string.IsNullOrEmpty(Request.QueryString["cusId"]))
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		private void LoadData(int type)
		{
			try
			{
				if (type == 1)
				{
					var listInvoice = _invoiceService.InvoiceGet(
										"",
										""
										, ""//Request.QueryString["fromDate"]
										, Request.QueryString["toDate"],
										Request.QueryString["cusId"],
										"",
										"",
										Convert.ToString(InvoiceStatus.Confirm),
										"",
										Request.QueryString["orderNo"] == null ? "" : Request.QueryString["orderNo"],
										"",
										"",
										"1",
										this
										).Where(p => ((p.BusinessCode == Const_BusinessCode.Business_208) || p.BusinessCode==Const_BusinessCode.Business_201 || p.BusinessCode == Const_BusinessCode.Business_209)
 														//&& p.BusinessCode !=Const_BusinessCode.Business_206
														&& p.OrderId !=null
														&& p.Status==InvoiceStatus.Confirm
										).ToList();

					if (listInvoice.Count == 0)
					{
						gridMain.Visible = false;
						lblError.Visible = true;
						lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
						return;
					}

					dSumAmount = listInvoice.Sum(p => p.SumAmount ?? 0);

					gridMain.Visible = true;
					lblError.Visible = false;
					gridMain.DataSource = listInvoice;
					gridMain.DataBind();
					Session["PaymentCustomerDetail"] = listInvoice;
				}

				if (type == 2)
				{
					if (Session["PaymentCustomerDetail"] != null)
					{
						var listInvoice = (List<InvoiceModel>)Session["PaymentCustomerDetail"];
						dSumAmount = listInvoice.Where(p => p.OrderNo != null).Sum(p => p.SumAmount ?? 0);

						gridMain.DataSource = listInvoice;
						gridMain.DataBind();
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
}