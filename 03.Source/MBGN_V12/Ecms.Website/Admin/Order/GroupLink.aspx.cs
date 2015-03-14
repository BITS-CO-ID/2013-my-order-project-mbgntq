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
    public partial class GroupLink : PageBase
    {
        #region // Declaration

        private readonly OrderService _orderService = new OrderService();
        private readonly WebsiteService _websiteService = new WebsiteService();

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

                    loadComboBox();
                    //this.DoSearch();
                    this.txtDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                this.DoSearch(1);
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
                this.DoSearch(2);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void grdD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "OrderDetailDelete":
                        int orderDetailId = Convert.ToInt32(e.CommandArgument);

                        if (Session[Constansts.SESSION_CURRENT_ORDERCART] != null)
                        {
                            var list = (List<OrderDetailModel>)Session[Constansts.SESSION_CURRENT_ORDERCART];
                            if (list.Count != 0)
                            {
                                var orderDetail = list.Where(x => x.OrderDetailId == orderDetailId).FirstOrDefault();

                                if (orderDetail != null)
                                {
                                    list.Remove(orderDetail);
                                    Session[Constansts.SESSION_CURRENT_ORDERCART] = list;
                                    var result = _orderService.OrderDetailChangeStatus(orderDetailId + "", "3", this);
                                    if (result)
                                    {
                                        DoSearch(2);
                                    }
                                }
                            }
                        }
                        break;
					case "OrderDetailUpdate":

						var orderId = e.CommandArgument.ToString().Split('|')[0];
						var order = _orderService.OrderGet(orderId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", this).FirstOrDefault();
							if (order != null)
							{
								var detailId =Convert.ToInt32(e.CommandArgument.ToString().Split('|')[1]);
								var orderDetailFirst = order.lstOrderDetailModel.ToList().Where(x => x.OrderDetailId == detailId).FirstOrDefault();
								if (orderDetailFirst != null)
								{
									Session[Constansts.SS_ORDER_DETAIL_ADMIN] = orderDetailFirst;
									Response.Redirect(string.Format("~/admin/order/orderdetailupdate.aspx?orderId={0}&ubl=1", orderId));
								}
								else
								{
									//Response.Redirect("~/admin/order/ordermanage.aspx");
								}
							}

						//Response.Redirect(string.Format("OrderDetailUpdate.aspx?detailId={0}", Convert.ToInt32(e.CommandArgument)));
						break;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, this);
            }
        }

        #endregion

        #region // Method

        private void DoSearch(int type)
        {
            if (type == 1)
            {
                var groupWebsite = ddlGroupWebsite.SelectedValue;
                var fDate = new DateTime();
                var tDate = new DateTime();
                if (ValidData(ref fDate, ref tDate) == false)
                    return;

                lblError.Text = "";

				var employeeCode = ((List<UserModel>)Session["UserModel"]).FirstOrDefault().SupperAdmin == Constansts.FlagActive ? "" : Session["User"].ToString();

                var list = _orderService.OrderGroupLinkGet(
                            ""
                            , ""
                            , ""
                            , ""
                            , fDate.ToString("yyyy-MM-dd 00:00:00")
                            , tDate.ToString("yyyy-MM-dd 23:59:59")
                            , ""
                            , ""
                            , ""
							, groupWebsite //website
							, txtShop.Text.Trim()
							, employeeCode //Session["User"].ToString().ToLower().Equals("sysadmin") ? "" : Session["User"].ToString()
                            , this);

                if (list.Count > 0)
                {
                    grdD.Visible = btnAddAll.Visible = true;
                    grdD.DataSource = list;
                    grdD.DataBind();
                    lblError.Visible = false;
                    Session[Constansts.SESSION_CURRENT_ORDERCART] = list;
					Session["Website"] = groupWebsite;
                }
                else
                {
                    grdD.Visible = btnAddAll.Visible = false;
                    lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm!";
                    lblError.Visible = true;
                }
            }
            if (type == 2)
            {
                var list = (List<OrderDetailModel>)Session[Constansts.SESSION_CURRENT_ORDERCART];
                if (list.Count > 0)
                {
                    grdD.Visible = btnAddAll.Visible = true;
                    lblError.Visible = false;
                    grdD.DataSource = list;
                    grdD.DataBind();
                }
                else
                {
                    grdD.Visible = btnAddAll.Visible = false;
                    lblError.Text = "Không có sản phẩm nào để gom!";
                    lblError.Visible = true;
                }
            }

			if (Session["PolicyUserMH"] != null)
			{
				this.grdD.Columns[13].Visible = false;
				this.grdD.Columns[14].Visible = false;
				this.grdD.Columns[15].Visible = false;
				this.grdD.Columns[16].Visible = false;
			}
			else
			{
				this.grdD.Columns[13].Visible = true;
				this.grdD.Columns[14].Visible = true;
				this.grdD.Columns[15].Visible = true;
				this.grdD.Columns[16].Visible = true;
			}
        }

        private void loadComboBox()
        {
            ddlGroupWebsite.DataSource = _websiteService.WebsiteLinkGet("", "", "", "0", this);
            ddlGroupWebsite.DataTextField = "WebsiteName";
            ddlGroupWebsite.DataValueField = "WebsiteId";
            ddlGroupWebsite.DataBind();
            ddlGroupWebsite.Items.Insert(0, new ListItem("-- Chọn website --", ""));
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