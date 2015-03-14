using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using CommonUtils;
using Ecms.Biz;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Order
{
	public partial class OrderUpdateConfigFee : PageBase
    {
        #region // Declares

        private readonly OrderService _orderService = new OrderService();
		private IUserBiz cService = new UserBiz();

        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null || Request.QueryString["orderId"]==null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}

				if (string.IsNullOrEmpty(Request.QueryString["calFeeDelay"].ToString()))
				{
					chkCalFeeDelay.Checked = true;
				}
				else
				{
					chkCalFeeDelay.Checked = Request.QueryString["calFeeDelay"].ToString() == "1" ? true : false;
				}
								
				if (string.IsNullOrEmpty(Request.QueryString["dayAllowedDelay"].ToString()))
				{
					// Lấy giá trị mặc định
					var config302 = new CustomerService().ConfigBusinessGet("", "", "", "", Const_BusinessCode.Business_302, "", "", "", "", this).OrderByDescending(p => p.CreatedDate).FirstOrDefault();
					txtDayAloowed.Text = config302 == null ? "" : config302.ConfigValue.Value.ToString();
				}
				else
				{
					txtDayAloowed.Text = Request.QueryString["dayAllowedDelay"].ToString();
				}

				if (string.IsNullOrEmpty(Request.QueryString["feeDelay"].ToString()))
				{
					// Lấy giá trị mặc định
					var config303 = new CustomerService().ConfigBusinessGet("", "", "", "", Const_BusinessCode.Business_303, "", "", "", "", this).OrderByDescending(p => p.CreatedDate).FirstOrDefault();
					txtFee.Text = config303 == null ? "" : config303.ConfigValue.Value.ToString();
				}
				else
				{
					txtFee.Text = Request.QueryString["feeDelay"].ToString();
				}

				if (!string.IsNullOrEmpty(Request.QueryString["amountFeeDelay"].ToString()))
				{
					txtAmountFeeDelay.Text = Convert.ToDouble((Request.QueryString["amountFeeDelay"].ToString())).ToString("N0");
				}

				if (!string.IsNullOrEmpty(Request.QueryString["isCalFeeDelay"].ToString()) && Request.QueryString["isCalFeeDelay"] == Constansts.FlagActive)
				{
					chkCalFeeDelay.Enabled = false;
					txtDayAloowed.Enabled = false;
					txtFee.Enabled = false;

					//lblError.Text = "ĐH này đã được HT áp phí trả chậm, không được thay đổi các chỉ số tính phí mà chỉ được phép thay đổi phí trả chậm";
					//lblError.Visible = true;
					btnConfirmConfigFee.Enabled = false;
					btnConfirmConfigFee.CssClass = Constansts.CssClass_buttonDisable;
				}
				else
				{
					btnAccept.Enabled = false;
					btnAccept.CssClass = Constansts.CssClass_buttonDisable;

					txtAmountFeeDelayNew.Enabled = false;
				}

				// Check status:
				if (!string.IsNullOrEmpty(Request.QueryString["status"].ToString()))
				{
					var status = Convert.ToInt32(Request.QueryString["status"].ToString());
					if (status == OrderInStatus.OrderConfirmed)
					{

					}

					if (status == OrderInStatus.Deliveried || status==OrderInStatus.OrderCancel)
					{
						btnAccept.Enabled = false;
						btnAccept.CssClass = Constansts.CssClass_buttonDisable;

						txtAmountFeeDelayNew.Enabled = false;
						chkCalFeeDelay.Enabled = false;
						txtDayAloowed.Enabled = false;
						txtFee.Enabled = false;

						lblError.Text = "Đơn hàng đã giao hàng, không được phép thay đổi các chỉ số và phí trả chậm";
						lblError.Visible = true;

						btnConfirmConfigFee.Enabled = false;
						btnConfirmConfigFee.CssClass = Constansts.CssClass_buttonDisable;

						btnAccept.Enabled = false;
						btnAccept.CssClass = Constansts.CssClass_buttonDisable;
					}
				}
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
			try
			{
				lblError.Text = "";
				lblError.Visible = false;

				if (Request.QueryString["orderId"] != null)
				{
					// validate

					try
					{
						if (string.IsNullOrEmpty(txtAmountFeeDelayNew.Text))
						{
							lblError.Text = "Chưa nhập thông tin phí trả chậm mới!";
							lblError.Visible = true;
							return;
						}

						if (!string.IsNullOrEmpty(txtAmountFeeDelayNew.Text))
						{
							var amountFeeDelayNew = Convert.ToDouble(txtAmountFeeDelayNew.Text.Trim());
							if (amountFeeDelayNew < 0)
							{
								lblError.Text = "Phí trả chậm mới phải >=0!";
								lblError.Visible = true;
								return;
							}
						}
						
					}
					catch (Exception)
					{
						lblError.Text = "Thông tin phí chưa chính xác";
						lblError.Visible = true;
						return;
					}

					var orderId = Request.QueryString["orderId"].ToString();

					// Chỉ lưu phí mới
					var result = _orderService.OrderUpdateConfigFee(
											orderId
											, ""
											, ""
											, ""
											, txtAmountFeeDelayNew.Text.Trim()
											, this);

					if (result)
					{
						lblResult.Text = "Cập nhật thông tin thanh toán chậm cho đơn hàng thành công!";
						mtvMain.ActiveViewIndex = 1;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

		protected void btnConfirmConfigFee_Click(object sender, EventArgs e)
		{
			try
			{
				lblError.Text = "";
				lblError.Visible = false;

				if (Request.QueryString["orderId"] != null)
				{
					// validate

					try
					{
						if (!string.IsNullOrEmpty(txtDayAloowed.Text))
						{
							var dayDelay = Convert.ToInt32(txtDayAloowed.Text.Trim());
							if (dayDelay <= 0 || dayDelay >99)
							{
								lblError.Text = "Số ngày trả chậm cho phép phải >0 và <=99";
								lblError.Visible = true;
								return;		
							}
						}

						if (!string.IsNullOrEmpty(txtFee.Text))
						{
							var feeDelay = Convert.ToDouble(txtFee.Text.Trim());
							if (feeDelay <= 0 || feeDelay > 99)
							{
								lblError.Text = "Mức áp phí trả chậm cho phép phải >0 và <=99";
								lblError.Visible = true;
								return;
							}
						}
					}
					catch (Exception)
					{
						lblError.Text = "Thông tin cấu hình chưa chính xác";
						lblError.Visible = true;
						return;
					}

					var orderId = Request.QueryString["orderId"].ToString();

					var result = _orderService.OrderUpdateConfigFee(
											orderId
											, chkCalFeeDelay.Checked ? "1" : "0"
											, txtDayAloowed.Text.Trim()
											, txtFee.Text.Trim()
											, ""
											, this);

					if (result)
					{
						lblResult.Text = "Cập nhật thông tin cấu hình thanh toán chậm cho đơn hàng thành công!";
						mtvMain.ActiveViewIndex = 1;
					}
				}
				else
				{
					Response.Redirect("~/admin/order/ordermanage.aspx");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
        }

        #endregion
    }
}