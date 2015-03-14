using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Website.DBServices;
using Ecms.Website.Common;
using Ecms.Biz.Class;
using Ecms.Biz;

namespace Ecms.Website.Admin
{
    public partial class ComplaintDetail : PageBase
    {
        #region //Declares

        private readonly ComplaintsService _complaintsService = new ComplaintsService();
        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

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
                    var strPager = this.PageLinks(pagingModel.PagingInfo, pageIndex + "", ResolveUrl("~/Admin/ComplaintDetail.aspx"));
                    litPager.Text = strPager;
                    //End Paging
                    lblTitle.Text = tempComplaint[0].Title;
                    List<ComplaintDetailModel> comp = new List<ComplaintDetailModel>();
                    if (pageIndex == 1)
                        comp.Add(new ComplaintDetailModel()
                        {
                            Id = -1,
                            ComplaintId = Convert.ToInt32(id)
                            ,
                            Content = tempComplaint[0].ContentComplaints
                            ,
                            UserCreatedId = tempComplaint[0].CreatedUser
                            ,
                            CreatedDate = tempComplaint[0].CreatedDate
                            ,
                            Title = tempComplaint[0].Title
                            ,
                            UserName = tempCustomer[0].UserCode
                        });
                    comp.AddRange(pagingModel.Models.OrderBy(x => x.Id));
                    if (pageIndex == 1)
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

        protected void lbtDelete_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        var result = this._complaintsService.ComplaintDetailDelete(id, this);
                        if (result)
                        {
                            Response.Write("<script type='text/javascript'>alert('Đã xóa dữ liệu thành công.');</script>");
                            loadGrid();
                        }
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
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
                List<UserModel> user = (List<UserModel>)Session["UserModel"];
                if (user == null)
                {
                    Response.Redirect("./Security/Login.aspx");
                }

                var customer = this._customerService.CustomerList("","","","","","","", user[0].UserCode,"","","", this);
                Ecms.Biz.Entities.ComplaintDetail comp = new Ecms.Biz.Entities.ComplaintDetail();
                int complaintId = Convert.ToInt32(Session["ComplainId"]);
                Session["ComplainId"] = null;
                comp.ComplaintId = complaintId;
                comp.CreatedDate = DateTime.Now;
                comp.Content = txtContent.Text;
                comp.UserCreatedId = customer[0].CustomerId;



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
        }
    }
}