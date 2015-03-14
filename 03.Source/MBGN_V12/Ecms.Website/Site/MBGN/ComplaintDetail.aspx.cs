using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.Common;
using Ecms.Website.DBServices;
using Ecms.Biz.Class;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Website.Site.MBGN
{
    public partial class ComplaintDetail : System.Web.UI.Page
    {
        #region //Declares

        private readonly ComplaintsService _complaintsService = new ComplaintsService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Customer"] == null)
                    Response.Redirect("~/site/mbgn/loginRequirement.aspx");

                loadGrid();
            }
        }

        private void loadGrid()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                try
                {

                    string id = (string)Session["lbtEdit_Click"];

                    Session["ComplainId"] = id;
                    var tempComplaint = this._complaintsService.ComplaintGet(id, "", "", "", this); //.Where(x => x.Type == 1);
                    var tempCustomer = this._customerService.CustomerList(tempComplaint[0].CreatedUser.ToString(), "", "", "", "", "", "", "", "", "", "", this);
                    var tempComplaintDetail = this._complaintsService.ComplaintDetailGet("", id, "", this);
                    //Begin paging

                    var pageIndex = string.IsNullOrEmpty(Request.QueryString["pageIndex"]) ? 1 : Convert.ToInt32(Request.QueryString["pageIndex"]);
                    PagingModels<ComplaintDetailModel> pagingModel = new PagingModels<ComplaintDetailModel>(tempComplaintDetail.AsQueryable(), pageIndex, 5);
                    var strPager = this.PageLinks(pagingModel.PagingInfo, pageIndex + "", ResolveUrl("~/site/mbgn/ComplaintDetail.aspx"));
                    litPager.Text = strPager;
                    //End Paging

                    lblTitle.Text = tempComplaint[0].Title;
                    List<ComplaintDetailModel> comp = new List<ComplaintDetailModel>();
                    if(pageIndex==1)
                        comp.Add(new ComplaintDetailModel()
                        {
                            ComplaintId = Convert.ToInt32(id)
                            ,Content = tempComplaint[0].ContentComplaints
                            ,UserCreatedId = tempComplaint[0].CreatedUser
                            ,CreatedDate = tempComplaint[0].CreatedDate
                            ,Title = tempComplaint[0].Title
                            ,UserName = tempCustomer[0].UserCode
                        });
                    comp.AddRange(pagingModel.Models.OrderBy(x => x.Id));
                    if(pageIndex==1)
                    {
                        int i = 1;
                        foreach (var item in comp)
                        {
                            item.CountPost = i++;
                        }
                    }
                    else
                    {
                        int postPerPage = 5;
                        int i = (pageIndex - 1) * postPerPage + 2;
                        foreach (var item in comp)
                        {
                            item.CountPost = i++;
                        }
                    }
                    rptSaleHot.DataSource = comp;
                    rptSaleHot.DataBind();
                    

                }
                catch (Exception ex)
                {
                    Utils.ShowExceptionBox(ex, this);
                }
            }
            else
            {
                //Response.Redirect("Complaints.aspx");

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (string.IsNullOrEmpty(txtContent.Text))
            {
                lblError.Text = "Nội dung không được bỏ trống";
            }
            try
            {
                UserCustomerModel user = (UserCustomerModel)Session["Customer"];
                Ecms.Biz.Entities.ComplaintDetail comp = new Ecms.Biz.Entities.ComplaintDetail();
                int complaintId = Convert.ToInt32(Session["ComplainId"]);
                Session["ComplainId"] = null;
                comp.ComplaintId = complaintId;
                comp.CreatedDate = DateTime.Now;
                comp.Content = txtContent.Text;
                comp.UserCreatedId = user.CustomerId;



                var result = this._complaintsService.ComplaintDetailCreate(comp, this);
                if (result.Id != 0)
                {
                    Response.Write("<script type='text/javascript'>alert('Đăng thành công');</script>");
                    txtContent.Text = "";
                    loadGrid();

                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert('Không đăng được');</script>");
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }   // end
    }
}