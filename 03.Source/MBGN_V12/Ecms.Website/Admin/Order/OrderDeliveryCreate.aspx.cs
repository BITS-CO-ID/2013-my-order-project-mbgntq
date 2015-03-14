using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Entities;
using Ecms.Biz;
using System.Globalization;
using CommonUtils;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderDeliveryCreate : PageBase
    {
        #region // Declares

        private readonly ProductService _productService = new ProductService();
        private readonly CustomerService _customerService = new CustomerService();
        private readonly OrderService _orderService = new OrderService();

        #endregion

        #region // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                Session.Remove("CartTransportAdmin");
            }
        }

        //Bước 1: nhập thông tin đơn hàng như Order Number, BillNo, Ngày đến mỹ
        protected void btnStep2_Click(object sender, EventArgs e)
        {
            try
            {
                //string orderNumber = txtOrderNumber.Text;
				string orderNumber = "";
                string trackingNumber = txtTrackingNumber.Text;
                DateTime dateToUsa = new DateTime();
                if (ValidDataStep1(trackingNumber, ref dateToUsa) == false)
                    return;

                var step1ParamDict = new Dictionary<string, string>();
                step1ParamDict.Add("orderNumber", orderNumber);
                step1ParamDict.Add("trackingNumber", trackingNumber);

				if (chkInsuarance.Checked)
				{
					var configBusiness = _customerService.ConfigBusinessGet("", "", "", "", "INSUARANCE", "", "", "", "", this).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
					step1ParamDict.Add("insuarance", configBusiness == null ? "" : Convert.ToString(configBusiness.ConfigBusinessId));
					step1ParamDict.Add("insuaranceValue", configBusiness == null ? "" : Convert.ToString(configBusiness.ConfigValue));
				}
				else
				{
					step1ParamDict.Add("insuarance", "");
					step1ParamDict.Add("insuaranceValue", "");
				}
				step1ParamDict.Add("lotPrice", txtLotPrice.Text);

                Session["step1ParamDict"] = step1ParamDict;
				txtTrackingCurrent.Text = txtTrackingNumber.Text;
                mtvMain.ActiveViewIndex = 1;
				
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        //Nhảy từ bước 1 sáng bước 3
        protected void btnStep3_Click(object sender, EventArgs e)
        {
            mtvMain.ActiveViewIndex = 2;
			
        }

        //Bước 2: thêm thông tin sản phầm vào đơn hàng
        protected void btnAddToCartTransport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["step1ParamDict"] == null)
                    Response.Redirect("");

				//if (string.IsNullOrEmpty(ddlCategory.SelectedValue))
				//{
				//    lblErrorStep2.Text = "Bạn chưa chọn chủng loại sản phẩm";
				//    lblErrorStep2.Visible = true;
				//    return;
				//}

                var cartTransportList = new List<TransportModel>();
                var transportModel = new TransportModel();
				//transportModel.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
				//transportModel.CategoryName = ddlCategory.SelectedItem.Text;
                transportModel.ProductName = txtProductName.Text;
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    transportModel.Weight = Convert.ToDouble(txtWeight.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtSurcharge.Text))
                {
                    transportModel.Surcharge = Convert.ToDouble(txtSurcharge.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    transportModel.Quantity = Convert.ToDouble(txtQuantity.Text.Replace(",", ""));
                }



                if (!string.IsNullOrEmpty(txtDeclarePrice.Text))
                {
                    transportModel.DeclarePrice = Convert.ToDouble(txtDeclarePrice.Text.Replace(",", ""));
                }

                if (!string.IsNullOrEmpty(txtShipModified.Text))
                {
                    transportModel.ShipModified = Convert.ToDouble(txtShipModified.Text.Replace(",", ""));
                }
                //Thêm thông tin ở bước 1
                var step1ParamDict = (Dictionary<string, string>)Session["step1ParamDict"];
                transportModel.OrderNoDelivery = step1ParamDict["orderNumber"];
                transportModel.TrackingNo = step1ParamDict["trackingNumber"];
				transportModel.LotPrice = step1ParamDict["lotPrice"];
				
                if (!string.IsNullOrEmpty(step1ParamDict["insuarance"]))
                {
                    transportModel.InsuaranceConfigId = Convert.ToInt32(step1ParamDict["insuarance"]);
                    transportModel.InsuaranceConfigValue = Convert.ToDouble(step1ParamDict["insuaranceValue"]);
                }

                if (Session["CartTransportAdmin"] != null)
                {
                    cartTransportList = (List<TransportModel>)Session["CartTransportAdmin"];
                    if (cartTransportList.Count != 0)
                    {
                        var Id = cartTransportList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                        transportModel.Id = Id;
                    }
                }
                else
                {
                    transportModel.Id = 1;
                }
                cartTransportList.Add(transportModel);
                Session["CartTransportAdmin"] = cartTransportList;
                LoadData();
                txtProductName.Text = txtWeight.Text = txtQuantity.Text = txtSurcharge.Text = "";
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            mtvMain.ActiveViewIndex = 0;
            if (Session["CartTransportAdmin"] != null)
            {
                btnStep3.Visible = true;
            }
            else
            {
                btnStep3.Visible = false;
            }
            lblErrorStep1.Visible = false;
            Session.Remove("step1ParamDict");
        }

        //Quay lại bước 1: Nhập thêm kiện hàng khác
        protected void btnBackToStep1_Click(object sender, EventArgs e)
        {
            mtvMain.ActiveViewIndex = 0;
            if (Session["CartTransportAdmin"] != null)
            {
                btnStep3.Visible = true;
            }
            else
            {
                btnStep3.Visible = false;
            }
            lblErrorStep1.Visible = false;
            Session.Remove("step1ParamDict");
        }

        //Bước 3: tạo đơn hàng
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["CartTransportAdmin"] != null)
                {
					var valid3=ValidDataStep3();
					if (valid3 == null) return;

                    var cartTransport = (List<TransportModel>)Session["CartTransportAdmin"];

                    Ecms.Biz.Entities.Order order = new Ecms.Biz.Entities.Order();
                    order.OrderDate = order.CreatedDate = DateTime.Now;
                    order.OrderTypeId = 3;
                    order.OrderStatus = 3;
                    order.Remark = txtNote.Text;
					order.CustomerId = valid3.CustomerId;
					if (Session["User"] != null)
					{
						order.CreateUser = Session["User"].ToString();
					}

                    foreach (var item in cartTransport)
                    {
                        var orderDetailTemp = new OrderDetail();
                        orderDetailTemp.CategoryId = item.CategoryId;
                        orderDetailTemp.ProductName = item.ProductName;
                        orderDetailTemp.Quantity = item.Quantity;
                        orderDetailTemp.Surcharge = item.Surcharge;
                        orderDetailTemp.Weight = item.Weight;

                        orderDetailTemp.OrderNoDelivery = item.OrderNoDelivery;
                        orderDetailTemp.TrackingNo = item.TrackingNo;
                        if (item.DateToUsa != null)
                        {
                            orderDetailTemp.DateToUsa = item.DateToUsa;
                        }
                        if (item.InsuaranceConfigId != null)
                        {
                            orderDetailTemp.InsuaranceConfigId = item.InsuaranceConfigId;
                        }
                        if (item.DeclarePrice != null)
                        {
                            orderDetailTemp.DeclarePrice = item.DeclarePrice;
                        }
                        if (item.ShipModified != null)
                        {
                            orderDetailTemp.ShipModified = item.ShipModified;
                        }
						orderDetailTemp.LotPrice = item.LotPrice;

						#region // get ShipConfigId
						var configBusiness = _customerService.ConfigBusinessGet(
											""
											, ""
											, ""
											, ""
											, Const_BusinessCode.Business_402
											, ""
											, ""
											, ""
											, ""
											, this).Where(p => p.fromQuantity <= item.Weight && p.toQuantity >= item.Weight).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

						if (configBusiness != null)
						{
							orderDetailTemp.ShipConfigId = configBusiness.ConfigBusinessId;
						}
						#endregion

						order.OrderDetails.Add(orderDetailTemp);
                    }
					#region // configBusiness for ConfigRateId in Order table
					var configBusinessRateOrder = _customerService.ConfigBusinessGet(
									""
									, ""
									, ""
									, ""//Convert.ToString(item.CountryId)
									, Const_BusinessCode.Business_ORGRATEDE
									, ""
									, ""
									, ""
									, ""
									, this).OrderByDescending(p => p.CreatedDate).FirstOrDefault();

					if (configBusinessRateOrder != null)
					{
						order.ConfigRateId = configBusinessRateOrder.ConfigBusinessId;
					}
					#endregion

                    var orderResturn = _orderService.OrderCreate(order, this);
                    if (orderResturn != null)
                    {
                        mtvMain.ActiveViewIndex = 3;
                        Session.Remove("CartTransportAdmin");
                        Session.Remove("step1ParamDict");
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/admin.aspx");
        }

        protected void gridCartTransport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "deleteCartTransport":
                        int Id = Convert.ToInt32(e.CommandArgument);
                        if (Session["CartTransportAdmin"] != null)
                        {
                            var cartTransport = (List<TransportModel>)Session["CartTransportAdmin"];
                            if (cartTransport.Count != 0)
                            {
                                var transport = cartTransport.Where(x => x.Id == Id).FirstOrDefault();

                                if (transport != null)
                                {
                                    cartTransport.Remove(transport);
                                    Session["CartTransportAdmin"] = cartTransport;
                                    LoadData();
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

        protected void gridCartTransport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridCartTransport.PageIndex = e.NewPageIndex;
                LoadData();
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

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (Session["CartTransportAdmin"] != null)
            {
                var cartTransport = (List<TransportModel>)Session["CartTransportAdmin"];
                if (cartTransport.Count == 0)
                {
                    lblErrorStep2.Text = "Bạn phải nhập ít nhất một sản phẩm để vận chuyển!";
                    lblErrorStep2.Visible = true;
                    return;
                }
            }
            else
            {
                lblErrorStep2.Text = "Bạn phải nhập ít nhất một sản phẩm để vận chuyển!";
                lblErrorStep2.Visible = true;
                return;
            }
            mtvMain.ActiveViewIndex = 2;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mtvMain.ActiveViewIndex = 1;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/ordermanage.aspx");
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

            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        private void LoadData()
        {
            if (Session["CartTransportAdmin"] != null)
            {
                var transportModel = (List<TransportModel>)Session["CartTransportAdmin"];
                if (transportModel.Count != 0)
                {
                    gridCartTransport.DataSource = transportModel;
                    gridCartTransport.DataBind();
                    pnCartTransport.Visible = true;
                }
                else
                {
                    pnCartTransport.Visible = false;
                }
            }
        }

        private bool ValidDataStep1(string trackingNumber, ref DateTime dateToUsa)
        {
            if (string.IsNullOrEmpty(trackingNumber))
            {
                lblErrorStep1.Text = "Bạn chưa nhập BillNo!";
                lblErrorStep1.Visible = true;
                return false;
            }

			//var trackingNoCheck = _orderService.OrderDetailTrackingNoCheck(trackingNumber, this);
			//if (trackingNoCheck)
			//{
			//    lblErrorStep1.Text = string.Format("Số TrackingNo=<strong>{0}</strong> đã có trong hệ thống",trackingNumber);
			//    lblErrorStep1.Visible = true;
			//    return false;
			//}
            return true;
        }

        private UserCustomerModel ValidDataStep3()
        {
            if (string.IsNullOrEmpty(txtCustomerUserCode.Text))
            {
                lblError.Text = "Bạn chưa nhập tài khoản yêu cầu!";
                lblError.Visible = true;
                return null;
            }

			// Check theo customerCode
            var customer = _customerService.CustomerList(""
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
				, this).Where(x=>x.CustomerCode==txtCustomerUserCode.Text || x.UserCode==txtCustomerUserCode.Text).SingleOrDefault();

			if (customer == null)
			{
					lblError.Text = "Tài khoản không tồn tại!";
					lblError.Visible = true;
					return null;
			}
			
			return	customer;			
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
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 : previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                    //Gộp Order Number
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 : previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }

		protected void chkInsuarance_OnCheckedChanged(object sender, EventArgs e)
		{
			if (chkInsuarance.Checked)
			{
				trLotPrice.Visible = true;
			}
			else if (!chkInsuarance.Checked)
			{
				trLotPrice.Visible = false;
			}
		}

		#endregion
	}

    #region //TransportModel

    public class TransportModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public double? Quantity { get; set; }
        public double? Weight { get; set; }
        public double? Surcharge { get; set; }
        public int? ShipConfigid { get; set; }
        public double? ShipConfigValue { get; set; }

        public string TrackingNo { set; get; }
        public string OrderNoDelivery { get; set; }
        public DateTime? DateToUsa { get; set; }
        public int? DetailStatus { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? InsuaranceConfigId { get; set; }
        public double? InsuaranceConfigValue { get; set; }
        public double? DeclarePrice { get; set; }
        public double? ShipModified { get; set; }
		public string LotPrice { set; get; }
		public string DetailCode { set; get; }
    }

    #endregion
}