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
    public partial class OrderDetailDeliveryUpdate : PageBase
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

        protected void btnAddToCartTransport_Click(object sender, EventArgs e)
        {
            try
            {
				//if (string.IsNullOrEmpty(ddlCategory.SelectedValue))
				//{
				//    lblErrorStep2.Text = "Bạn chưa chọn chủng loại sản phẩm";
				//    lblErrorStep2.Visible = true;
				//    return;
				//}

                var cartTransportList = new List<TransportModel>();
                var orderDetailModel = new TransportModel();
                //orderDetailModel.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                //orderDetailModel.CategoryName = ddlCategory.SelectedItem.Text;
                orderDetailModel.ProductName = txtProductName.Text;
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    orderDetailModel.Weight = Convert.ToDouble(txtWeight.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtSurcharge.Text))
                {
                    orderDetailModel.Surcharge = Convert.ToDouble(txtSurcharge.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    orderDetailModel.Quantity = Convert.ToDouble(txtQuantity.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtDeclarePrice.Text))
                {
                    orderDetailModel.DeclarePrice = Convert.ToDouble(txtDeclarePrice.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtShipModified.Text))
                {
                    orderDetailModel.ShipModified = Convert.ToDouble(txtShipModified.Text.Replace(",", ""));
                }

                if (Session["InsuaranceConfig"] != null)
                {
                    var insuaranceDict = (Dictionary<string,string>)Session["InsuaranceConfig"];
                    orderDetailModel.InsuaranceConfigId = Convert.ToInt32(insuaranceDict["InsuaranceConfigId"]);
                    orderDetailModel.InsuaranceConfigValue = Convert.ToDouble(insuaranceDict["InsuaranceConfigValue"]);
                }

                if (Session["dictChangeStatus"] != null)
                {
                    var dictChangeStatus = (Dictionary<string, string>)Session["dictChangeStatus"];
                    string orderId = dictChangeStatus["orderId"];
                    string orderNumber = dictChangeStatus["orderNumber"];
                    string trackingNumber = dictChangeStatus["trackingNumber"];
                    string customerTypeId = dictChangeStatus["customerTypeId"];
					string detailStatus = dictChangeStatus["DetailStatus"];
                    orderDetailModel.OrderId = Convert.ToInt32(orderId);
                    orderDetailModel.TrackingNo = trackingNumber;
                    orderDetailModel.OrderNoDelivery = orderNumber;
                    if (!string.IsNullOrEmpty(detailStatus))
                    {
                        orderDetailModel.DetailStatus = Convert.ToInt32(detailStatus);
                    }
					if (!string.IsNullOrEmpty(customerTypeId))
					{
						orderDetailModel.CustomerTypeId = Convert.ToInt32(customerTypeId);
					}
                }

                if (Session["gridCartTransport"] != null)
                {
                    cartTransportList = (List<TransportModel>)Session["gridCartTransport"];
                    if (cartTransportList.Count != 0)
                    {
                        var Id = cartTransportList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                        orderDetailModel.Id = Id;
                    }
                }
                else
                {
                    orderDetailModel.Id = 1;
                }
                cartTransportList.Add(orderDetailModel);
                Session["gridCartTransport"] = cartTransportList;
                LoadData(2);
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
		{
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}else
			{
				Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
			}
		}

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["gridCartTransport"] != null)
                {
                    var cartTransportList = (List<TransportModel>)Session["gridCartTransport"];
                    var listOrderDetailModel = new List<OrderDetailModel>();
                    foreach (var item in cartTransportList)
                    {
                        var orderDetailModel = new OrderDetailModel();
                        orderDetailModel.OrderId = item.OrderId.Value;
                        orderDetailModel.OrderDetailId = item.OrderDetailId;
                        orderDetailModel.Quantity = item.Quantity;
                        orderDetailModel.Weight = item.Weight;
                        orderDetailModel.ProductName = item.ProductName;
                        orderDetailModel.CategoryId = item.CategoryId;
                        if (item.CustomerTypeId != null)
                        {
                            var configBusiness = _customerService.ConfigBusinessGet("", item.CustomerTypeId + "", "3", "", "FEE", "", item.CategoryId + "", "", "", this).FirstOrDefault();
                            if (configBusiness != null)
                            {
                                orderDetailModel.ShipConfigId = configBusiness.ConfigBusinessId;
                            }
                        }
                        orderDetailModel.DetailStatus = item.DetailStatus;
                        orderDetailModel.DateToUsa = item.DateToUsa;
                        orderDetailModel.Surcharge = item.Surcharge;
                        orderDetailModel.OrderNoDelivery = item.OrderNoDelivery;
                        orderDetailModel.TrackingNo = item.TrackingNo;

                        if (item.DeclarePrice != null)
                        {
                            orderDetailModel.DeclarePrice = item.DeclarePrice;
                        }
                        if (item.ShipModified != null)
                        {
                            orderDetailModel.ShipModified = item.ShipModified;
                        }
                        if (item.InsuaranceConfigId != null)
                        {
                            orderDetailModel.InsuaranceConfigId = item.InsuaranceConfigId;
                        }
                        listOrderDetailModel.Add(orderDetailModel);
                    }
                    var result=_orderService.DeliveryDetailUpdate(listOrderDetailModel, this);
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
			if (Request.QueryString["Odsu"] != null)
			{
				Response.Redirect("~/admin/order/OrderDetailStatusUpdate.aspx");
			}else
			{
				Response.Redirect("~/admin/order/orderdeliverydetail.aspx?orderId=" + Request.QueryString["orderId"].ToString());
			}
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
                    case "deleteCartTransport":
                        int Id = Convert.ToInt32(e.CommandArgument);
                        if (Session["gridCartTransport"] != null)
                        {
                            var cartTransport = (List<TransportModel>)Session["gridCartTransport"];
                            if (cartTransport.Count != 0)
                            {
                                var transport = cartTransport.Where(x => x.Id == Id).FirstOrDefault();

                                if (transport != null)
                                {
                                    cartTransport.Remove(transport);
                                    Session["gridCartTransport"] = cartTransport;
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
                if (Session["dictChangeStatus"] != null)
                {
                    var dictChangeStatus = (Dictionary<string, string>)Session["dictChangeStatus"];
                    string orderId = dictChangeStatus["orderId"];
                    string trackingNumber = dictChangeStatus["trackingNumber"];
					var orderReturn = _orderService.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
                    if (orderReturn != null)
                    {
                        var orderDetailDelivery = orderReturn.lstOrderDetailModel.ToList().Where(x => x.TrackingNo == trackingNumber).ToList();
                        if (orderDetailDelivery.Count != 0)
                        {
                            pnCartTransport.Visible = true;
                            var listTransportModel = ConvertToTransportModel(orderDetailDelivery);
                            gridCartTransport.DataSource = listTransportModel;
                            gridCartTransport.DataBind();
                            Session["gridCartTransport"] = listTransportModel;
                            //Get orderDetailModel firt to get insuarance
                            var firstByTracking = orderDetailDelivery.FirstOrDefault();
                            if (firstByTracking.InsuaranceConfigId != null)
                            {
                                var insuaranceDict = new Dictionary<string, string>();
                                insuaranceDict["InsuaranceConfigId"] = firstByTracking.InsuaranceConfigId+"";
                                insuaranceDict["InsuaranceConfigValue"] = firstByTracking.InsuaranceConfigValue+"";
                                Session["InsuaranceConfig"] = insuaranceDict;
                            }
                        }
                        else
                        {
                            pnCartTransport.Visible = true;
                        }
                    }
                }
            }

            if (type == 2)
            {
                if (Session["gridCartTransport"] != null)
                {
                    var listTransportModel = (List<TransportModel>)Session["gridCartTransport"];
                    if (listTransportModel.Count != 0)
                    {
                        pnCartTransport.Visible = true;
                        gridCartTransport.DataSource = listTransportModel.ToList().OrderBy(x => x.TrackingNo).ToList();
                        gridCartTransport.DataBind();
                    }
                    else
                    {
                        pnCartTransport.Visible = true;
                    }
                }
            }
        }

        private List<TransportModel> ConvertToTransportModel(List<OrderDetailModel> listOrderDetailModel)
        {
            var listTransportModel = new List<TransportModel>();
            int id = 1;
            foreach (var item in listOrderDetailModel)
            {
                var tranModel = new TransportModel();
                tranModel.Id = id++;
                tranModel.OrderId = item.OrderId;
                tranModel.OrderDetailId = item.OrderDetailId;
                tranModel.CategoryId = (item.CategoryId ?? 0);
                tranModel.CategoryName = item.CategoryName;
                tranModel.ProductName = item.ProductName;
                tranModel.Quantity = item.Quantity;
                tranModel.Weight = item.Weight;
                tranModel.Surcharge = item.Surcharge;
                tranModel.TrackingNo = item.TrackingNo;
                tranModel.OrderNoDelivery = item.OrderNoDelivery;
                tranModel.DateToUsa = item.DateToUsa;
				tranModel.DetailStatus = item.DetailStatus;
                tranModel.InsuaranceConfigId = item.InsuaranceConfigId;
                tranModel.InsuaranceConfigValue = item.InsuaranceConfigValue;
                tranModel.DeclarePrice = item.DeclarePrice;
                tranModel.ShipModified = item.ShipModified;
                tranModel.ShipConfigid = item.ShipConfigId;
                tranModel.ShipConfigValue = item.ShipConfigValue;
				tranModel.DetailCode = item.DetailCode;
                listTransportModel.Add(tranModel);
            }
            return listTransportModel;
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