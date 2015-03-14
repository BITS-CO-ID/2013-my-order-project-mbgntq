using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using Ecms.Biz;
using Ecms.Website.Common;
using Ecms.Website.DBServices;

namespace Ecms.Website.Admin.Order
{
    public partial class OrderOutboundDetail : PageBase
    {
        #region //Declares
        private readonly OrderService _orderService = new OrderService();
		protected double dSumAmount = 0;
        #endregion

        #region //Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				if (Session["User"] == null)
				{
					Response.Redirect("~/admin/security/login.aspx");
				}
                LoadData();

				

				//modalPopup.OpenerElementID = lbtnTrackingNoUpdate.ClientID;
            }

        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdD.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/orderoutboundmanage.aspx?returnBack=return");
        }

		protected void gridMain_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			//if (e.Row.RowType == DataControlRowType.DataRow)
			//{
			//	//Label lbl = (Label)e.Row.FindControl("Label1");

			//	LinkButton editLink = (LinkButton)e.Row.FindControl("lbtnTrackingNoUpdate");

			//	modalPopup.OpenerElementID = editLink.ClientID;
			//}
		}

        protected void grdD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "OODDelete":
                        int orderDetailId = Convert.ToInt32(e.CommandArgument);
                        if (Session[Constansts.SS_ORDER_OUTBOUND_LIST_ADMIN] != null)
                        {
                            var orderOutbound = (OrderOutboundModel)Session[Constansts.SS_ORDER_OUTBOUND_ADMIN];
							
                            var ood = orderOutbound.lstOrderOutboundDetailModel.ToList().Where(x => x.OrderOutboundDetailId == orderDetailId).FirstOrDefault();
							
                            if (ood != null)
                            {
								if ((ood.OrderDetail.DetailStatus == null || ood.OrderDetail.DetailStatus == OrderOutboundStatus.InProcess || ood.OrderDetail.DetailStatus == OrderOutboundStatus.Cancel))
								{
									var dt = orderOutbound.lstOrderOutboundDetailModel.ToList();
									dt.Remove(ood);
									orderOutbound.lstOrderOutboundDetailModel = dt.AsQueryable();
									Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] = orderOutbound;
									var result = _orderService.OrderOutboundDetailDelete(orderDetailId + "", this);
									if (result)
									{
										LoadData();
									}
								}
								else
								{
									lblError.Text = "Món hàng này Đã Mua hoặc đã được chuyển về VN, không được phép xóa";
									lblError.Visible = true;
									return;
								}                                
							}
                        }
                        break;
                    case "OODUpdate":
                        var param = e.CommandArgument.ToString().Split('|');
                        Response.Redirect(string.Format("~/admin/order/orderdetailupdate.aspx?orderId={0}&orderDetailId={1}&orderOutboundId={2}", param[0], param[1],param[2]));
                        break;
					case "ChangeStatusDetail":
						var paramDetail = e.CommandArgument.ToString().Split('|');
						Response.Redirect(string.Format("~/admin/order/orderoutbounddetailupdate.aspx?orderDetailId={0}&cur_status={1}", paramDetail[0], paramDetail[1]));
						break;
					case "TrackingNoUpdate":
						var detailId = e.CommandArgument.ToString().Split('|');
						var iOrderDetailId = Convert.ToInt32(detailId[0]);
						Session["iOrderDetailId"] = iOrderDetailId;

						
						break;
                }

            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion

        #region //Private methods

        private void LoadData()
        {
			try
			{
				if (Session["User"] == null)
					Response.Redirect("~/admin/security/login.aspx");

				OrderOutboundModel orderOutbound = new OrderOutboundModel();
				if (Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] != null)
				{
					orderOutbound = (OrderOutboundModel)Session[Constansts.SS_ORDER_OUTBOUND_ADMIN];
					if (!string.IsNullOrEmpty(Request.QueryString["isReUp"]))
					{
						orderOutbound = _orderService.OrderOutboundGet(
									Convert.ToString(orderOutbound.OrderOutboundId)
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
									, this).SingleOrDefault();
					}					
				}
				

				if (orderOutbound != null)
				{
					lblOrderOutboundCode.Text = orderOutbound.OrderOutboundNo;
					lblCreatedDate.Text = orderOutbound.CreatedDate.Value.ToString("dd/MM/yyyy");
					lblCreatedUser.Text = orderOutbound.UserCreate;
					lblTrackingNumber.Text = orderOutbound.TrackingNo;
					lblAccountCreated.Text = orderOutbound.AccountWebsiteNo;
					lblStatus.Text = orderOutbound.StatusText;
					lblRemark.Text = orderOutbound.Remark;
					dSumAmount = (orderOutbound.SumAmount ?? 0);

					//Bind lên grid view

					var listDetail = orderOutbound.lstOrderOutboundDetailModel.ToList();
					Session["listDetail"] = listDetail;
					grdD.DataSource = listDetail;
					grdD.DataBind();


					
				}

			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
        }

        #endregion

		protected void btnOkay_Click(object sender, EventArgs e)
		{
			try
			{
				// update billNo

				if (Session["iOrderDetailId"] != null && Session["OODBillNo"] !=null 
					&& !Session["OODBillNo"].ToString().Equals(txtTrackingNo.Text.Trim()))
				{
					var iOrderDetailId = Convert.ToInt32(Session["iOrderDetailId"]);

					var result = _orderService.OrderDetailTrackingNoUpdate(
								Convert.ToString(iOrderDetailId)
								, this.txtTrackingNo.Text
								, this.Page);

					if (result)
					{
						// bind data
						var lst = (List<OrderOutboundDetailModel>)Session["listDetail"];
						foreach (var item in lst)
						{
							if (item.OrderDetailId == iOrderDetailId)
							{
								item.OrderDetail.TrackingNo = this.txtTrackingNo.Text;
							}
						}

						grdD.DataSource = lst;
						grdD.DataBind();
					}
					Session.Remove("iOrderDetailId");
					Session.Remove("OODBillNo");
				}
			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

		protected void lbtnTrackingNoUpdate_Click(object sender, EventArgs e)
		{
			//LinkButton lnkCustomerID = sender as LinkButton;

			LinkButton lbn = (LinkButton)sender;			
			
			var billNo = lbn.Text.Trim();
			if(billNo.ToLower().Equals(Constansts.Const_Empty))
			{
				this.txtTrackingNo.Text = "";
			}else
			{
				this.txtTrackingNo.Text = billNo;
			}
			Session["OODBillNo"] = billNo;

			ModalPopupExtender1.Show();
		}
    }
}