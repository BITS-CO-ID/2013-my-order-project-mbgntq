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
	public partial class OrderUpdateEmployee : PageBase
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
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
				// load danh sách nhân viên
				ddlEmployee.DataSource = cService.GetUser("","", "", "1", "").Where(p=>p.FlagAdmin=="1");
				ddlEmployee.DataTextField = "UserName";
				ddlEmployee.DataValueField = "UserCode";
				ddlEmployee.DataBind();
				ddlEmployee.Items.Insert(0, new ListItem("-- Chọn NV bán hàng --", ""));

				ddlEmployee.SelectedValue = Session[Constansts.SESSION_EMPLOYEE].ToString();
            }
        }

		protected void btnAccept_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(ddlEmployee.SelectedValue))
				{
					lblError.Text = "Bạn chưa chọn người bán hàng!";
					lblError.Visible = true;
					return;
				}

				if (Session[Constansts.SESSION_ORDERID] != null)
				{
					var orderid = Convert.ToString(Session[Constansts.SESSION_ORDERID]);
					if (!string.IsNullOrEmpty(orderid))
					{
						var orderReturn = _orderService.OrderUpdateEmployeeCode(orderid, ddlEmployee.SelectedValue, this);
						if (orderReturn != null)
						{
							lblResult.Text = "Cập nhật người bán hàng thành công!";
							mtvMain.ActiveViewIndex = 1;
						}
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