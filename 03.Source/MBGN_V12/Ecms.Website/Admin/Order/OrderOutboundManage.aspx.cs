using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Biz;
using CommonUtils;
using System.Globalization;

namespace Ecms.Website.Admin
{
    public partial class OrderOutboundManage : PageBase
    {
        #region // Declaration

        private readonly OrderService _orderService = new OrderService();
        private readonly WebsiteService _websiteService = new WebsiteService();
        private readonly CommonService _commonService = new CommonService();

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (Session["User"] == null)
                        Response.Redirect("~/admin/security/login.aspx");
                    InitData();
                    this.DoSearch(1,1);
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoSearch(1,0);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // add cart
            var newList = new List<OrderDetailModel>();
            foreach (GridViewRow gr in grdD.Rows)
            {
                CheckBox cb = (CheckBox)gr.Cells[0].FindControl("chkCheck");
                if (cb.Checked)
                {
                    HiddenField hdfield = (HiddenField)gr.Cells[0].FindControl("hdODId");
                    // do ur job here
                    var listNew = (List<OrderDetailModel>)Session[Constansts.SESSION_CURRENT_ORDERCART];
                    if (listNew != null)
                    {
                        var odId = Convert.ToInt32(hdfield.Value);
                        var orderDetail = listNew.Where(p => p.OrderDetailId == odId).SingleOrDefault();

                        newList.Add(orderDetail);
                    }
                }
            }
            if (newList != null && newList.Count > 0)
            {
                var details = (List<OrderDetailModel>)Session[Constansts.SESSION_ORDERCART];

                // Thêm những sản phẩm mới được chọn
                details.Union(newList);
            }
        }

        protected void btnAddAll_Click(object sender, EventArgs e)
        {
            if (Session[Constansts.SESSION_CURRENT_ORDERCART] == null || ((List<OrderDetailModel>)Session[Constansts.SESSION_CURRENT_ORDERCART]).Count == 0)
            {
                lblError.Text = "Không có link hàng nào.";
                lblError.Visible = true;
                return;
            }
            Session[Constansts.SESSION_ORDERCART] = Session[Constansts.SESSION_CURRENT_ORDERCART];
            Response.Redirect("grouplinkcart.aspx");
        }

        protected void btnViewCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("grouplinkcart.aspx");
        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdD.PageIndex = e.NewPageIndex;
                this.DoSearch(2,1);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void ddlWebsiteGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebsiteGroup.SelectedValue.Equals(""))
            {
                ddlWebsite.Items.Clear();
                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn Shop --", ""));
            }
            else
            {
                int websiteParentId = Convert.ToInt32(ddlWebsiteGroup.SelectedValue);
                var websiteList = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == websiteParentId);
                ddlWebsite.DataSource = websiteList;
                ddlWebsite.DataTextField = "WebsiteName";
                ddlWebsite.DataValueField = "WebsiteId";
                ddlWebsite.DataBind();
                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn Shop --", ""));
            }
        }

        protected void grdD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
			try
			{
				if (Session[Constansts.SS_ORDER_OUTBOUND_LIST_ADMIN] == null)
				{
					Response.Redirect("~/admin/order/orderoutboundmanage.aspx");
				}

				int id = Convert.ToInt32(e.CommandArgument);
				var orderOutbound = (List<OrderOutboundModel>)Session[Constansts.SS_ORDER_OUTBOUND_LIST_ADMIN];
				var orderOutboundFirst = orderOutbound.SingleOrDefault(p => p.OrderOutboundId == id);

				if (orderOutboundFirst == null)
				{
					return;
				}
				Session[Constansts.SS_ORDER_OUTBOUND_ADMIN] = orderOutboundFirst;

				switch (e.CommandName)
				{
					case "OrderOutBoundDetail":
						Response.Redirect("~/admin/order/orderoutbounddetail.aspx");
						break;
					case "OrderOutBoundUpdate":
						Response.Redirect("~/admin/order/orderoutboundupdate.aspx");
						break;
					case "OrderOutBoundDelete":
						if (orderOutboundFirst.Status == 1)
						{
							var result = _orderService.OrderOutboundDelete(orderOutboundFirst.OrderOutboundId + "", this);
							if (result)
							{
								mtvMain.ActiveViewIndex = 1;
								lblResult.Text = "Đã xóa đơn hàng thành công!";
							}
						}
						else
						{
							lblError.Text = "Đơn hàng này không được phép xóa";
							lblError.Visible = true;
							return;
						}
						break;
				}
			}
			catch (Exception exc)
			{
				Utils.ShowExceptionBox(exc, this);
			}
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/order/orderoutboundmanage.aspx?returnBack=return");
        }

        #endregion

        #region // Method

        private void InitData()
        {
            var currentDate = DateTime.Now;
            this.txtDateFrom.Text = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0).ToString("dd/MM/yyyy");
            this.txtDateTo.Text = currentDate.ToString("dd/MM/yyyy");

            var parentWebsite = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == null);
            ddlWebsiteGroup.DataSource = parentWebsite;
            ddlWebsiteGroup.DataTextField = "WebsiteName";
            ddlWebsiteGroup.DataValueField = "WebsiteId";
            ddlWebsiteGroup.DataBind();
            ddlWebsiteGroup.Items.Insert(0, new ListItem("-- Chọn website --", ""));
            ddlWebsite.Items.Insert(0, new ListItem("-- Chọn Shop --", ""));
        }

        private void DoSearch(int type, int btnClick)
        {
            if (type == 1)
            {
                lblError.Text = "";
                var fDate = new DateTime();
                var tDate = new DateTime();
                if (ValidData(ref fDate, ref tDate) == false)
                    return;

                var searchParamDict = new Dictionary<string, string>();
                if (Request.QueryString["returnBack"] != null)
                {
                    if (btnClick == 0)
                    {
                        searchParamDict["OOD"] = txtOOD.Text;
                        searchParamDict["trackingNo"] = txtTrackingNo.Text;
                        searchParamDict["orderNumber"] = txtOrderNumber.Text;
                        searchParamDict["fDate"] = fDate.ToString("yyyy-MM-dd 00:00:00");
                        searchParamDict["tDate"] = tDate.ToString("yyyy-MM-dd 23:59:59");
                        searchParamDict["orderStatus"] = ddlStatus.SelectedValue;
                        searchParamDict["parentWebsite"] = ddlWebsiteGroup.SelectedValue;
                        searchParamDict["website"] = ddlWebsite.SelectedValue;
                        searchParamDict["accountWebsiteNo"] = txtAccountWebsiteNo.Text;
                        Session["searchParamOutBoundDict"] = searchParamDict;
                    }
                    else
                    {
                        if (Session["searchParamOutBoundDict"] != null)
                        {
                            searchParamDict = (Dictionary<string, string>)Session["searchParamOutBoundDict"];
                            txtOOD.Text = searchParamDict["OOD"];
                            txtTrackingNo.Text = searchParamDict["trackingNo"];
                            txtOrderNumber.Text = searchParamDict["orderNumber"];
                            txtDateFrom.Text = Convert.ToDateTime(searchParamDict["fDate"]).ToString("dd/MM/yyyy");
                            cldFromDate.SelectedDate = Convert.ToDateTime(searchParamDict["fDate"]);
                            txtDateTo.Text = Convert.ToDateTime(searchParamDict["tDate"]).ToString("dd/MM/yyyy");
                            cldToDate.SelectedDate = Convert.ToDateTime(searchParamDict["tDate"]);
                            ddlStatus.SelectedValue = searchParamDict["orderStatus"];
                            ddlWebsiteGroup.SelectedValue = searchParamDict["parentWebsite"];
                            if (!string.IsNullOrEmpty(searchParamDict["parentWebsite"]))
                            {
                                int websiteParentId = Convert.ToInt32(searchParamDict["parentWebsite"]);
                                var websiteList = _commonService.WebsiteList("", "", "").Where(x => x.ParentId == websiteParentId);
                                ddlWebsite.DataSource = websiteList;
                                ddlWebsite.DataTextField = "WebsiteName";
                                ddlWebsite.DataValueField = "WebsiteId";
                                ddlWebsite.DataBind();
                                ddlWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
                                ddlWebsite.SelectedValue = searchParamDict["website"];
                            }
                            txtAccountWebsiteNo.Text = searchParamDict["accountWebsiteNo"];
                        }
                    }
                }
                else
                {
                    searchParamDict["OOD"] = txtOOD.Text;
                    searchParamDict["trackingNo"] = txtTrackingNo.Text;
                    searchParamDict["orderNumber"] = txtOrderNumber.Text;
                    searchParamDict["fDate"] = fDate.ToString("yyyy-MM-dd 00:00:00");
                    searchParamDict["tDate"] = tDate.ToString("yyyy-MM-dd 23:59:59");
                    searchParamDict["orderStatus"] = ddlStatus.SelectedValue;
                    searchParamDict["parentWebsite"] = ddlWebsiteGroup.SelectedValue;
                    searchParamDict["website"] = ddlWebsite.SelectedValue;
                    searchParamDict["accountWebsiteNo"] = txtAccountWebsiteNo.Text;
                    Session["searchParamOutBoundDict"] = searchParamDict;
                }

				//Session["User"]
				var list = _orderService.OrderOutboundGet(
							""
							, searchParamDict["OOD"]
							, searchParamDict["trackingNo"]
							, searchParamDict["orderNumber"]
							, searchParamDict["fDate"]
							, searchParamDict["tDate"]
							, searchParamDict["orderStatus"]
							, ""
							, searchParamDict["accountWebsiteNo"]
							, searchParamDict["website"]
							, ""
							, Session["User"].ToString().ToLower().Equals("sysadmin") ? "" : Session["User"].ToString()
							, ""
							, ""
							, this);

                if (list.Count > 0)
                {
                    grdD.Visible = true;
                    lblError.Visible = false;
                    grdD.DataSource = list;
                    grdD.DataBind();
                    Session[Constansts.SS_ORDER_OUTBOUND_LIST_ADMIN] = list;
                }
                else
                {
                    grdD.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
                }
            }

            if (type == 2)
            {
                if (Session[Constansts.SS_ORDER_OUTBOUND_LIST_ADMIN] != null)
                {
                    var list = (List<OrderOutboundModel>)Session[Constansts.SS_ORDER_OUTBOUND_LIST_ADMIN];
                    grdD.DataSource = list;
                    grdD.DataBind();
                }
            }
        }

        private bool ValidData(ref DateTime fDate, ref DateTime tDate)
        {
            if (string.IsNullOrEmpty(txtDateFrom.Text))
            {
                lblError.Text = "Từ ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                fDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtDateTo.Text))
            {
                lblError.Text = "Đến ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                tDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Đến ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            return true;
        }
        #endregion
    }
}