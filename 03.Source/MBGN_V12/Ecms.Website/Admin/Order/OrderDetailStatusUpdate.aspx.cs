using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Class;
using System.Data;
using Ecms.Biz;
using CommonUtils;

namespace Ecms.Website.Admin.Report
{
	public partial class OrderDetailStatusUpdate : PageBase
    {
        #region // Declares

		private readonly OrderService _orderService = new OrderService();
        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        protected void gridMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridMain.PageIndex = e.NewPageIndex;
            LoadData(2);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(1);

			if (string.IsNullOrEmpty(txtFromDate.Text))
			{
				cldFromDate.SelectedDate = DateTime.Now;
			}

			if (string.IsNullOrEmpty(txtFromDate.Text))
			{
				cldToDate.SelectedDate = DateTime.Now;
			}
        }

		protected void gridMain_DataBound(object sender, EventArgs e)
		{
			MergeRows(gridMain);
		}

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				var dictChangeStatus = new Dictionary<string, string>();
				var param = e.CommandArgument.ToString().Split('|');
				
				if (param.Count() != 10)
				{
					return;
				}
				var orderId = param[7];
				if (string.IsNullOrEmpty(orderId))
				{
					return;
				}
				var orderTypeId =Convert.ToInt32(param[9]);
				var orderDetailId = Convert.ToInt32(param[3]);

				dictChangeStatus.Add("orderId", orderId);
				dictChangeStatus.Add("trackingNumber", param[0]);
				dictChangeStatus.Add("orderNumber", param[1]);
				dictChangeStatus.Add("DetailStatus", param[2]);
				dictChangeStatus.Add("orderDetailId",Convert.ToString(orderDetailId));
				dictChangeStatus.Add("DateToUsa", param[4]);
				dictChangeStatus.Add("DeliveryVNDate", param[5]);
				dictChangeStatus.Add("DeliveryDate", param[6]);
				dictChangeStatus.Add("customerTypeId", param[8]);
				Session["dictChangeStatus"] = dictChangeStatus;

				switch (e.CommandName)
				{
					case "ChangeStatus":
						{
							if (orderTypeId == OrderType_Const.OrderShipOnly)
							{
								// Đơn hàng vận chuyển
								Response.Redirect(string.Format("~/admin/order/changedeliverystatus.aspx?orderId={0}&Odsu=1", orderId));
							}

							if (orderTypeId == OrderType_Const.OrderBylink)
							{
								Response.Redirect(string.Format("~/admin/order/orderstatusupdate.aspx?orderDetailId={0}&cur_status={1}&Odsu=1", orderDetailId, param[2]));
							}
						}
						break;
					case "UpdateProduct":
						{
							if (orderTypeId == OrderType_Const.OrderShipOnly)
							{
								Response.Redirect(string.Format("~/admin/order/orderdetaildeliveryupdate.aspx?orderId={0}&&Odsu=1", orderId));
							}
						}
						break;
					case "UpdateOrderDetail":
						{
							if (orderTypeId == OrderType_Const.OrderShipOnly)
							{
								// ĐH mua hộ
								Response.Redirect(string.Format("~/admin/order/orderdetailproductupdate.aspx?orderId={0}&&Odsu=1", orderId));
							}

							if (orderTypeId == OrderType_Const.OrderBylink)
							{
								// Đơn hàng mua hộ

								var order = _orderService.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
								if (order != null)
								{
									var orderDetailFirst = order.lstOrderDetailModel.ToList().SingleOrDefault(x => x.OrderDetailId == orderDetailId);
									if (orderDetailFirst == null)
									{
										return;
									}
									Session[Constansts.SS_ORDER_DETAIL_ADMIN] = orderDetailFirst;
									Response.Redirect(string.Format("~/admin/order/orderdetailupdate.aspx?orderId={0}&Odsu=1", orderId));
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

		protected void lbtnDetail_Click(object sender, CommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "OrderDetail":
					var param = e.CommandArgument.ToString().Split('|');
					var id = param[0];
					if (param[1].Equals("1")) // Báo giá
                    {
                        Response.Redirect("~/admin/order/quotationreply.aspx?orderId=" + id);
                    }

                    if (param[1].Equals("2")) // order link
                    {
                        Response.Redirect("~/admin/order/orderbylinkdetail.aspx?orderId=" + id);
                    }

                    if (param[1].Equals("3")) // order hàng gửi
                    {
                        Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + id);
                    }

                    if (param[1].Equals("4")) // order sản phẩm có sẵn
                    {
                        Response.Redirect("~/admin/order/orderbydetail.aspx?orderId=" + id);
                    }
					break;
			}
		}

		protected void lbtnShopDetail_Click(object sender, CommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "ShopDetail":
					var param = e.CommandArgument.ToString().Split('|');
					Response.Redirect(string.Format("~/admin/order/orderdetailtrackingupdate.aspx?orderId={0}&shop={1}", param[0], param[1]));
					break;
			}
		}

		protected void lbtnOrderOutBoundDetail_Click(object sender, CommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "OrderOutBoundDetail":
					var outboundId = e.CommandArgument.ToString();
					if (string.IsNullOrEmpty(outboundId))
					{
						return;
					}
					var outbound = new OrderService().OrderOutboundGet(
									outboundId
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, ""
									, this
									).SingleOrDefault();
					if (outbound != null)
					{
						Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] = outbound;

						Response.Redirect("~/admin/order/orderoutbounddetail.aspx");
					}
					break;
			}
		}

		protected void gridMain_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var hdn = (HiddenField)e.Row.FindControl("hdnOrderTypeId");
				var orderTypeId=hdn==null?0:Convert.ToInt32(hdn.Value);
				if (orderTypeId == OrderType_Const.OrderShipOnly)
				{
					e.Row.Cells[2].ToolTip = OrderType_Const.OrderShipOnly_Text;
				}

				if (orderTypeId == OrderType_Const.OrderBylink)
				{
					e.Row.Cells[2].ToolTip = OrderType_Const.OrderBylink_Text;
				}

				//var hdnUserCode = (HiddenField)e.Row.FindControl("hdnUserCode");
				//var hdnCustomerName = (HiddenField)e.Row.FindControl("hdnCustomerName");
				//e.Row.Cells[7].ToolTip = string.Format("Tên khách hàng: {0}<br/>Tên đăng nhập: {1}", (hdnCustomerName == null ? "" : hdnCustomerName.Value), (hdnUserCode == null ? "" : hdnUserCode.Value));
			}
		}

        #endregion

        #region // Private methods

        private void InitData()
        {
            try
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
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

        private void LoadData(int type)
        {
            try
            {
                if (type == 1)
                {
                    string fDate = "";
					string tDate = "";
                    if (ValidData(ref fDate, ref tDate) == false) return;

					var employeeCode = ((List<UserModel>)Session["UserModel"]).FirstOrDefault().SupperAdmin == Constansts.FlagActive ? "" : Session["User"].ToString();

					var listResult = _orderService.OrderDetailModelGet(
											fDate,
											tDate,
											txtDetailCode.Text,
											txtOrderNo.Text,
											txtCustomerCode.Text,
											"",
											"",
											this.ddlStatus.SelectedValue,
											this.ddlOrderType.SelectedValue,
											this.txtTrackingNo.Text.Trim(),
											this.txtShop.Text.Trim(),
											employeeCode,
											this).Take(150).ToList();
					
                    if (listResult.Count == 0)
                    {
						Session["OrderDetailStatusUpdate"] = listResult;
                        gridMain.Visible = false;
                        lblError.Visible = true;
                        lblError.Text = "Không tìm thấy dữ liệu theo điều kiện tìm kiếm!";
                        return;
                    }

                    gridMain.Visible = true;
                    lblError.Visible = false;
                    gridMain.DataSource = listResult;
                    gridMain.DataBind();
					Session["OrderDetailStatusUpdate"] = listResult;
                }

                if (type == 2)
                {
					if (Session["OrderDetailStatusUpdate"] != null)
                    {
						var listResult = (List<OrderDetailModel>)Session["OrderDetailStatusUpdate"];
                        gridMain.Visible = true;
                        lblError.Visible = false;
                        gridMain.DataSource = listResult;
                        gridMain.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		private bool ValidData(ref string fDate, ref string tDate)
        {
            try
            {
				if (!string.IsNullOrEmpty(txtFromDate.Text))
				{
					CultureInfo viVN = new CultureInfo("vi-VN");
					fDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN).ToString("yyyy-MM-dd");
				}
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            try
            {
				if (!string.IsNullOrEmpty(txtToDate.Text))
				{
					CultureInfo viVN = new CultureInfo("vi-VN");
					tDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", viVN).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
				}
            }
            catch
            {
                lblError.Text = "Đến ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

		public void MergeRows(GridView gridView)
		{
			for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
			{
				GridViewRow row = gridView.Rows[rowIndex];
				GridViewRow previousRow = gridView.Rows[rowIndex + 1];
				var lit1 = (LinkButton)row.Cells[1].FindControl("lbtnTrackingNumber");
				var lit2 = (LinkButton)previousRow.Cells[1].FindControl("lbtnTrackingNumber");

				if (lit1.Text == lit2.Text)
				{
					//Gộp tracking
					row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 : previousRow.Cells[4].RowSpan + 1;
					previousRow.Cells[4].Visible = false;

					

					//Gộp trạng thái cho đơn hàng vận chuyển
					var hdnOrderTypeId = (HiddenField)row.Cells[1].FindControl("hdnOrderTypeId");					
					if (!string.IsNullOrEmpty(hdnOrderTypeId.Value) && Convert.ToInt32(hdnOrderTypeId.Value) == OrderType_Const.OrderShipOnly)
					{
						row.Cells[17].RowSpan = previousRow.Cells[17].RowSpan < 2 ? 2 : previousRow.Cells[17].RowSpan + 1;
						previousRow.Cells[17].Visible = false;
					}
				}

				var litShop1 = (LinkButton)row.Cells[1].FindControl("lbtnShopDetail");
				var litShop2 = (LinkButton)previousRow.Cells[1].FindControl("lbtnShopDetail");
				if (litShop1.Text == litShop2.Text)
				{
					//Gộp Shop
					row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 : previousRow.Cells[8].RowSpan + 1;
					previousRow.Cells[8].Visible = false;
				}
			}
		}
        #endregion
    }
}