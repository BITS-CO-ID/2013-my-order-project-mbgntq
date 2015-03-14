using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Biz;
using Ecms.Website.Common;
using CommonUtils;
using System.Globalization;
using System.Data;

namespace Ecms.Website.Site.MBGN
{
    public partial class OrderProduct : System.Web.UI.Page
    {
        #region // Declares

        private readonly OrderService _orderService = new OrderService();
		protected double dRemainAmount = 0;
		protected double dPayAmount = 0;
		protected double dTotalAmount = 0;
		protected double dSumFeeShip = 0;
		protected double dSumAmountCalFeeDelay = 0;
		protected double dSumAmountFeeDelay = 0;
		//protected double sumRemainAmount = 0;

        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				var firstTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);

				this.txtFromDate.Text = firstTime.ToString("dd/MM/yyyy");
				this.txtToDate.Text = firstTime.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");

                this.DoSearch(1);
            }
        }

        protected void gridCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Session[Constansts.SESSION_ORDERMODEL_LIST] == null)
            {
                Response.Redirect("~/site/mbgn/orderproduct.aspx");
            }
            int id = Convert.ToInt32(e.CommandArgument);
            var orders = (List<OrderModel>)Session[Constansts.SESSION_ORDERMODEL_LIST];
            var orderFirst = orders.Where(p => p.OrderId == id).FirstOrDefault();

            if (orderFirst == null)
            {
                return;
            }
            Session[Constansts.SESSION_ORDERMODEL] = orderFirst;
            switch (e.CommandName)
            {
                case "OrderDetail":
                    if (orderFirst.OrderTypeId == 1) // Báo giá
                    {
                        Response.Redirect("~/site/mbgn/quotationdetail.aspx");
                    }

                    if (orderFirst.OrderTypeId == 2) // order link
                    {
                        Response.Redirect("~/site/mbgn/orderbylinkdetail.aspx");
                    }

                    if (orderFirst.OrderTypeId == 3) // order hàng gửi
                    {
                        Response.Redirect("~/site/mbgn/orderbycheckeddetail.aspx");
                    }

                    if (orderFirst.OrderTypeId == 4) // order sản phẩm có sẵn
                    {
                        Response.Redirect("~/site/mbgn/orderbydetail.aspx");
                    }
                    break;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoSearch(1);
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

		protected void btnSendPayment_Click(object sender, EventArgs e)
		{
			if (Session["Customer"] == null)
			{
				Response.Redirect("~/site/mbgn/login.aspx");
			}
			Response.Redirect("~/site/mbgn/payment.aspx");
		}

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            if (Session["Customer"] == null)
                Response.Redirect("~/site/mbgn/login.aspx");

            var customer = (UserCustomerModel)Session["Customer"];
            Order order = new Order();
            order.OrderDate = order.CreatedDate = DateTime.Now;
            order.OrderTypeId = 4;
            order.CustomerId = customer.CustomerId;

            if (Session["Cart"] != null)
            {
                var listProductCart = (List<CartModel>)Session["Cart"];
                foreach (var item in listProductCart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductId = item.ProductId;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.PriceWeb = item.Price;
                    order.OrderDetails.Add(orderDetail);
                }
            }
            Session["Order"] = order;
            Response.Redirect("~/site/mbgn/AddInfoDelivery.aspx");
        }

        protected void btnContinues_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/default.aspx");
        }

		

        protected void gridCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridCart.PageIndex = e.NewPageIndex;
            this.DoSearch(2);
        }

		protected void gridMain_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var hdn = (HiddenField)e.Row.FindControl("hdOrderTypeId");
				var orderTypeId = hdn == null ? 0 : Convert.ToInt32(hdn.Value);
				if (orderTypeId == OrderType_Const.Quote)
				{
					e.Row.Cells[1].ToolTip = OrderType_Const.Quote_Text;
				}

				if (orderTypeId == OrderType_Const.OrderShipOnly)
				{
					e.Row.Cells[1].ToolTip = OrderType_Const.OrderShipOnly_Text;
				}

				if (orderTypeId == OrderType_Const.OrderBylink)
				{
					e.Row.Cells[1].ToolTip = OrderType_Const.OrderBylink_Text;
				}

				if (orderTypeId == OrderType_Const.OrderByProduct)
				{
					e.Row.Cells[1].ToolTip = OrderType_Const.OrderByProduct_Text;
				}

				//var hdnUserCode = (HiddenField)e.Row.FindControl("hdnUserCode");
				//e.Row.Cells[4].ToolTip = hdnUserCode == null ? "" : hdnUserCode.Value;
			}
		}

		

        #endregion

        #region //Private methods

        private void DoSearch(int type)
        {
            if (Session["Customer"] == null)
            {
                Response.Redirect("~/site/mbgn/loginRequirement.aspx");
            }

            if (type == 1)
            {


                string fDate = "";
                string tDate = "";

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                {
                    fDate = CommonUtils.DatetimeUtils.GetDateTextbox(txtFromDate.Text).ToString("yyyy-MM-dd 00:00:00");
                }

                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    tDate = CommonUtils.DatetimeUtils.GetDateTextbox(txtToDate.Text).ToString("yyyy-MM-dd 23:59:59");
                }

                var userCustomerModel = (UserCustomerModel)Session["Customer"];
                var orders = _orderService.OrderGet(
                                            ""
                                            , txtOrderNo.Text.Trim()
                                            , ""
                                            , ""
                                            , fDate
                                            , tDate
                                            , Convert.ToString(userCustomerModel.CustomerId.Value)
                                            , ""
                                            , ""
                                            , ""
                                            , ""
                                            , ""
                                            , ""
                                            , ddlOrderStatus.SelectedValue == "0" ? "" : ddlOrderStatus.SelectedValue
                                            , ""
                                            , ""
											, ""
											, ""
											, ""
                                            , this);
				Session[Constansts.SESSION_ORDERMODEL_LIST] = orders;
				this.gridCart.DataSource = orders;
				this.gridCart.DataBind();

                if (orders.Count != 0)
                {
					pnCartNotEmty.Visible = true;
					pnOrderEmpty.Visible = false;
					lblError.Visible = false;
                    
                    var o = orders.Where(x => x.OrderTypeId != 1 && x.OrderStatus > 3 && x.OrderStatus != 5);
                    dPayAmount = o.Sum(x=>(x.TotalPayAmountNormal));
					dTotalAmount = o.Sum(x => (x.SumAmount??0));
					dSumFeeShip = o.Sum(x => (x.SumFeeShip ?? 0));
					dSumAmountCalFeeDelay = o.Sum(x => (x.AmountCalFeeDelay));
					dSumAmountFeeDelay = o.Sum(x => (x.AmountFeeDelay));
					dRemainAmount = o.Sum(x => (x.RemainAmount ?? 0));

					//// Lấy thông tin customer:
					//var customer = new CustomerService().CustomerList(
					//                Convert.ToString(userCustomerModel.CustomerId.Value)
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , ""
					//                , this
					//                ).SingleOrDefault();

					//// Lấy invoice
					//var invoices = new InvoiceService().InvoiceGet(
					//            ""
					//            , ""
					//            , ""
					//            , ""
					//            , Convert.ToString(customer.CustomerId.Value)
					//            , ""
					//            , ""
					//            , ""
					//            , ""
					//            , ""
					//            , ""
					//            , ""
					//            , this).Where(p => p.OrderId != null && (p.OrderStatus == OrderInStatus.OrderConfirmed || p.OrderStatus == OrderInStatus.Finished)// không lấy những đơn hàng đã hoàn thành
					//                && p.Status == InvoiceStatus.Confirm); // chỉ lấy trạng thái =2 đã khớp thanh toán

					//var balanceFreeze = invoices.Sum(p => p.SumAmount ?? 0);

					//var avaiableBalance = (customer.Balance ?? 0) - balanceFreeze;
					//Session["avaiableBalance"] = avaiableBalance;

					//lblDebit.Text = string.Format("<h2>{0} (VNĐ)</h2>", (dRemainAmount - avaiableBalance).ToString("N0"));
                }
                else
                {
					pnCartNotEmty.Visible = false;
					pnOrderEmpty.Visible = true;
					lblError.Visible = false;
                }
            }

            if (type == 2)
            {
                if (Session[Constansts.SESSION_ORDERMODEL_LIST] != null)
                {
                    var orders = (List<OrderModel>)Session[Constansts.SESSION_ORDERMODEL_LIST];

					var o = orders.Where(x => x.OrderTypeId != 1 && x.OrderStatus > 3 && x.OrderStatus != 5);
					dPayAmount = o.Sum(x => (x.TotalPayAmountNormal));
					dTotalAmount = o.Sum(x => (x.SumAmount ?? 0));
					dSumFeeShip = o.Sum(x => (x.SumFeeShip ?? 0));
					dSumAmountCalFeeDelay = o.Sum(x => (x.AmountCalFeeDelay));
					dSumAmountFeeDelay = o.Sum(x => (x.AmountFeeDelay));
					dRemainAmount = o.Sum(x => (x.RemainAmount ?? 0));

                    this.gridCart.DataSource = orders;
                    this.gridCart.DataBind();
                }
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


		private DataTable ReturnDatatable(List<OrderModel> lstToTable)
		{
			DataTable objTable = new DataTable();

			#region Create header
			objTable.Columns.Add("STT");
			objTable.Columns.Add("Loại ĐH");
			objTable.Columns.Add("Mã đơn hàng");
			objTable.Columns.Add("Thời gian tạo ĐH");
			objTable.Columns.Add("Ngày xác nhận ĐH");
			objTable.Columns.Add("Tình trạng");
			objTable.Columns.Add("Tổng tiền");
			objTable.Columns.Add("Đã thanh toán");
			objTable.Columns.Add("Tiền vận chuyển");
			//objTable.Columns.Add("Tổng còn lại tính phí quá hạn");
			//objTable.Columns.Add("Phí trả quá hạn");
			objTable.Columns.Add("Công nợ ĐH");
			#endregion
			int index = 1;

			var statusList = new int[] {OrderInStatus.OrderConfirmed, OrderInStatus.Finished, OrderInStatus.Deliveried };

			foreach (var item in lstToTable)
			{
				var row = objTable.NewRow();
				row["STT"] = index;
				row["Loại ĐH"] = item.OrderNo;
				row["Mã đơn hàng"] = item.OrderNo;
				row["Thời gian tạo ĐH"] = item.CreatedDate == null ? "" : item.CreatedDate.Value.ToString("dd/MM/yyyy");
				row["Ngày xác nhận ĐH"] = item.ConfirmDate == null ? "" : item.ConfirmDate.Value.ToString("dd/MM/yyyy");
				row["Tình trạng"] = item.OrderStatusText;
				row["Tổng tiền"] = Math.Round((statusList.Contains(item.OrderStatus ?? 0) ? item.SumAmount??0 : 0), 0);
				row["Đã thanh toán"] = Math.Round(item.TotalPayAmountNormal,0);
				row["Tiền vận chuyển"] = Math.Round(item.SumFeeShip??0,0);
				//row["Tổng còn lại tính phí quá hạn"] = Math.Round(item.AmountCalFeeDelay,0);
				//row["Phí trả quá hạn"] = Math.Round(item.AmountFeeDelay,0);
				row["Công nợ ĐH"] = Math.Round((statusList.Contains(item.OrderStatus??0)? item.RemainAmount??0 :0),0);

				objTable.Rows.Add(row);
				index += 1;
			}

			//var rowFuter = objTable.NewRow();
			//rowFuter["STT"] = "Tổng";
			////rowFuter["Hoa hồng tạm tính"] = totalT;
			////rowFuter["Hoa hồng thực tính"] = totalNT;
			//objTable.Rows.Add(rowFuter);
			return objTable;
		}

		protected void btnExportExcel_Click(object sender, EventArgs e)
		{
			if (Session[Constansts.SESSION_ORDERMODEL_LIST] == null)
			{
				Response.Redirect("~/admin/security/login.aspx");
			}
			var listResult = (List<OrderModel>)Session[Constansts.SESSION_ORDERMODEL_LIST];
			var table = ReturnDatatable(listResult);
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ClearContent();
			HttpContext.Current.Response.ClearHeaders();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ContentType = "application/ms-excel";
			HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
			HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=orderlist.xls");

			HttpContext.Current.Response.Charset = "utf-8";
			//HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
			//sets font
			HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<BR><BR><BR>");
			HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
			int columnscount = table.Columns.Count;

			HttpContext.Current.Response.Write("<th colspan='10' style='font-size:14.0pt; font-family:Calibri;'>");
			HttpContext.Current.Response.Write("<B>");
			HttpContext.Current.Response.Write("TỔNG HỢP ĐƠN HÀNG");
			HttpContext.Current.Response.Write("</B>");
			HttpContext.Current.Response.Write("</th>");
			HttpContext.Current.Response.Write("</TR>");

			HttpContext.Current.Response.Write("<TR>");
			for (int j = 0; j < columnscount; j++)
			{
				//Makes headers bold
				HttpContext.Current.Response.Write("<Td>");
				HttpContext.Current.Response.Write("<B>");
				HttpContext.Current.Response.Write(table.Columns[j].ColumnName);
				HttpContext.Current.Response.Write("</B>");
				HttpContext.Current.Response.Write("</Td>");
			}
			HttpContext.Current.Response.Write("</TR>");
			foreach (DataRow row in table.Rows)
			{
				HttpContext.Current.Response.Write("<TR>");
				for (int i = 0; i < table.Columns.Count; i++)
				{
					HttpContext.Current.Response.Write("<Td>");
					HttpContext.Current.Response.Write(row[i].ToString());
					HttpContext.Current.Response.Write("</Td>");
				}
				HttpContext.Current.Response.Write("</TR>");
			}
			HttpContext.Current.Response.Write("</Table>");
			HttpContext.Current.Response.Write("</font>");
			HttpContext.Current.Response.Flush();
			HttpContext.Current.Response.End();

		}
        #endregion
    }
}