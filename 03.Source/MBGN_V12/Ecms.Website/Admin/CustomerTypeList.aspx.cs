using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Website.Common;
using Ecms.Biz;
using Ecms.Website.DBServices;

namespace Ecms.Website.Admin.Security
{
    public partial class CustomerList : PageBase
    {
        #region // Declaration

        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region // Method

        private List<CustomerType> LoadGrid()
        {
            var list = _customerService.CustomerTypeGet("",txtCustomerName.Text.Trim(), this);
            grdD.DataSource = list;
            grdD.DataBind();
            lblError.Text = "";
            if (list.Count > 0)
            {
                grdD.Visible = true;
            }
            else
            {
                grdD.Visible = false;
                lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
            }
            return list;

        }

        #endregion

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    LoadGrid();
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
                LoadGrid();
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void lbtEdit_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    try
                    {
                        string id = e.CommandArgument.ToString();
                        Session["lbtEdit_Click"] = id;
                        Response.Redirect("CustomerTypeCreate.aspx");
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
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
                        var result = _customerService.CustomerTypeDelete(id, this);
                        if(result)
                            Response.Write("<script type='text/javascript'>alert('Đã xóa dữ liệu thành công.');</script>");
                        LoadGrid();
                        
                    }
                    catch (Exception exc)
                    {
                        Utils.ShowExceptionBox(exc, this);
                    }
                    break;
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerTypeCreate.aspx");
        }


        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        #endregion



    }
}