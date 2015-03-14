using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz;

namespace Ecms.Website.Admin.Order
{
	public partial class OrderDetailTrackingUpdate : PageBase
    {
        #region //Declares

        private readonly ProductService _productService = new ProductService();
        private readonly CustomerService _customerService = new CustomerService();
        private readonly OrderService _orderService = new OrderService();

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

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
			{
				#region // validate
				lblError.Text = "";
				lblError.Visible = false;
				if (string.IsNullOrEmpty(txtBillNo.Text))
				{
					lblError.Text = "Bạn chưa nhập Mã Bill";
					lblError.Visible = true;
					return;
				}
				#endregion

				if (Session["gridBillNo"] != null)
                {
					var cartTransportList = (List<OrderDetailModel>)Session["gridBillNo"];
					foreach (var item in cartTransportList)
					{
						item.TrackingNo = txtBillNo.Text.Trim();
					}
					var result = _orderService.OrderDetailDeliveryUpdate(cartTransportList, this);

					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
					}
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/admin/order/orderdetailstatusupdate.aspx");

			//if (Request.QueryString["Odsu"] != null)
			//{
			//    Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			//}else
			//{
			//    Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
			//}
		}

		protected void btnReturn_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/admin/order/orderdetailstatusupdate.aspx");
		}

        protected void gridCartTransport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridCartTransport.PageIndex = e.NewPageIndex;
            LoadData(2);
        }

        protected void gridCartTransport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
					case "OrderDetailDelete":
                        int Id = Convert.ToInt32(e.CommandArgument);
						if (Session["gridBillNo"] != null)
                        {
							var cartTransport = (List<OrderDetailModel>)Session["gridBillNo"];
                            if (cartTransport.Count != 0)
                            {
                                var transport = cartTransport.Where(x => x.OrderDetailId == Id).SingleOrDefault();

                                if (transport != null)
                                {
                                    cartTransport.Remove(transport);
									Session["gridBillNo"] = cartTransport;
                                    LoadData(2);
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

        protected void gridCartTransport_DataBound(object sender, EventArgs e)
        {
            MergeRows(gridCartTransport);
        }

        #endregion

        #region //Private methods

        private void InitData()
        {
            try
            {
				if (Session["User"] == null)
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
			if (type == 1)
			{
				if (string.IsNullOrEmpty(Request.QueryString["orderId"]) || string.IsNullOrEmpty(Request.QueryString["orderId"]))
				{
					return;
				}
				string orderId = Request.QueryString["orderId"].ToString();
				string shop = Request.QueryString["shop"].ToString();
				this.txtShop.Text = shop;
				this.txtShop.Enabled = false;
				var orderReturn = _orderService.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).SingleOrDefault();

				if (orderReturn != null)
				{
					var orderDetailDelivery = orderReturn.lstOrderDetailModel.ToList().Where(x => x.Shop == shop).ToList();
					if (orderDetailDelivery.Count != 0)
					{
						pnCartTransport.Visible = true;

						gridCartTransport.DataSource = orderDetailDelivery;
						gridCartTransport.DataBind();

						Session["gridBillNo"] = orderDetailDelivery;
					}
					else
					{
						pnCartTransport.Visible = true;
					}
				}
			}

            if (type == 2)
            {
				if (Session["gridBillNo"] != null)
                {
					var listTransportModel = (List<OrderDetailModel>)Session["gridBillNo"];
                    if (listTransportModel.Count != 0)
                    {
                        pnCartTransport.Visible = true;
						gridCartTransport.DataSource = listTransportModel;//listTransportModel.ToList().OrderBy(x => x.TrackingNo).ToList();
                        gridCartTransport.DataBind();
                    }
                    else
                    {
                        pnCartTransport.Visible = true;
                    }
                }
            }
        }

        public void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];
                var lit1 = (Literal)row.Cells[1].FindControl("litTrackingNumber");
                var lit2 = (Literal)previousRow.Cells[1].FindControl("litTrackingNumber");

                if (lit1.Text == lit2.Text)
                {
                    //Gộp tracking
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 : previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
					////Gộp Order Number
					//row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 : previousRow.Cells[2].RowSpan + 1;
					//previousRow.Cells[2].Visible = false;
                }
            }
        }

        #endregion
    }
}