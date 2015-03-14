using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using System.Globalization;
using CommonUtils;

namespace Ecms.Website.Admin.Order
{
    public partial class ChangeDeliveryStatus : PageBase
    {
        #region //Declares

        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				this.LoadData();
			}
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
			this.setTextDateEnable(ddlStatus.SelectedValue == "" ? 0 : Convert.ToInt32(ddlStatus.SelectedValue),"");
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["dictChangeStatus"] != null)
                {
                    DateTime dateStatus = new DateTime();
					if (ValidData(ddlStatus.SelectedValue, ref dateStatus) == false) return;

                    var dictChangeStatus = (Dictionary<string, string>)Session["dictChangeStatus"];
                    string orderId = dictChangeStatus["orderId"];
                    string trackingNumber = dictChangeStatus["trackingNumber"];
					string dateStatusText = string.IsNullOrEmpty(txtFromDate.Text) ? "" : dateStatus.ToString("yyyy-MM-dd HH:mm:ss");
					if (_orderService.ChangeDeliveryStatus(orderId, trackingNumber, ddlStatus.SelectedValue, dateStatusText, this))
					{
						mtvMain.ActiveViewIndex = 1;
					}
                }
                else
                {
                    Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
			}
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}
			else
			{
				Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
			}
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
            if (Session["dictChangeStatus"] != null)
            {
                var dictChangeStatus = (Dictionary<string, string>)Session["dictChangeStatus"];
                string detailStatus = dictChangeStatus["DetailStatus"];
				string date = detailStatus == Convert.ToString(OrderOutboundStatus.InvOutbound) ? dictChangeStatus["DateToUsa"] :
								detailStatus == Convert.ToString(OrderOutboundStatus. InvInbound) ? dictChangeStatus["DeliveryVNDate"] :
								detailStatus == Convert.ToString(OrderOutboundStatus.InvInboundMBGN) ? dictChangeStatus["DeliveryDate"] : "";
                ddlStatus.SelectedValue = detailStatus;
				this.setTextDateEnable(detailStatus == "" ? 0 : Convert.ToInt32(detailStatus), date);
            }
        }

		private void setTextDateEnable(int status, string date)
		{
			//if (ddlStatus.SelectedValue.Equals("1") || ddlStatus.SelectedValue.Equals("3") || ddlStatus.SelectedValue.Equals("4"))
			if (status == 4)
			{
				lblDate.Text = "Ngày đến Mỹ";
			}

			if (status == 5)
			{
				lblDate.Text = "Ngày về VN";
			}

			if (status == 6)
			{
				lblDate.Text = "Ngày giao hàng";
			}

			if(status==4|| status==5 || status==6)
			{
				txtFromDate.Enabled = true;
			}
			else
			{
				txtFromDate.Enabled = false;
			}
			if (!string.IsNullOrEmpty(date))
			{
				txtFromDate.Text = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
			}
		}

        private bool ValidData(string status, ref DateTime fDate)
        {
            if (string.IsNullOrEmpty(status))
            {
                lblError.Text = "Bạn chưa chọn tình trạng!";
                lblError.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                try
                {
                    CultureInfo viVN = new CultureInfo("vi-VN");
                    fDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN);
                }
                catch
                {
                    lblError.Text = "Ngày không đúng định dạng!";
                    lblError.Visible = true;
                    return false;
                }
            }

			if (new string[] { Convert.ToString(OrderOutboundStatus.InvOutbound), Convert.ToString(OrderOutboundStatus.InvInbound), Convert.ToString(OrderOutboundStatus.InvInboundMBGN) }
				.Contains(ddlStatus.SelectedValue) && string.IsNullOrEmpty(txtFromDate.Text))
			{
				lblError.Text = "Bạn chưa nhập ngày!";
				lblError.Visible = true;
				return false;
			}

            return true;
        }

        #endregion        
    }
}