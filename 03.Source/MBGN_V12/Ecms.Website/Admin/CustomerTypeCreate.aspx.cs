using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecms.Biz.Entities;
using Ecms.Biz;
using Ecms.Website.DBServices;
using Ecms.Website.Common;

namespace Ecms.Website.Admin.Security
{
    public partial class CustomerTypeCreate : PageBase
    {

        #region // Declaration

        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region // Even

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                    Response.Redirect("~/Admin/Security/Login.aspx");

                try
                {
                    loadEdit();
                }
                catch (Exception exc)
                {
                    Utils.ShowExceptionBox(exc, this);
                }
            }
        }

        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtCustomerTypeName.Text.Trim()))
            //{
            //    lblError.Text = "Tên không được để trống.";
            //    return;
            //}
            try
            {
                if (Session["CustomerTypeCode"] != null)
                {
                    string id = (string)Session["CustomerTypeCode"];
                    var item = new CustomerType();
                    item.CustomerTypeId = Convert.ToInt32(id);
                    item.CustomerTypeName = txtCustomerTypeName.Text;
                    Session["CustomerTypeCode"] = null;
                    var result = this._customerService.CustomerTypeUpdate(item, this);
                    if(result.CustomerTypeId != 0)
                        DisplayAlert("Hệ thống cập nhật thành công", "CustomerTypeList.aspx");
                }
                else
                {
                    var item = new CustomerType();
                    item.CustomerTypeName = txtCustomerTypeName.Text;
                    var result = this._customerService.CustomerTypeCreate(item, this);
                    if (result.CustomerTypeId != 0)
                        DisplayAlert("Hệ thống cập nhật thành công", "CustomerTypeList.aspx");
                }
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerTypeList.aspx");
        }

        #endregion

        #region // Method

        private void loadEdit()
        {
            if (Session["lbtEdit_Click"] != null)
            {
                btnSave.Text = "Lưu";
                lblFunction.Text = "Sửa";
                string id = (string)Session["lbtEdit_Click"];
                Session["CustomerTypeCode"] = id;
                var customerType = this._customerService.CustomerTypeGet(id, "", this);

                txtCustomerTypeName.Text = customerType[0].CustomerTypeName;
                Session["lbtEdit_Click"] = null;
            }
            else
            {
                lblFunction.Text = "Thêm";
                txtCustomerTypeName.Text = "";
            }
        }

        protected virtual void DisplayAlert(string message, string page)
        {
            try
            {
                ClientScript.RegisterStartupScript(
                  this.GetType(),
                  Guid.NewGuid().ToString(),
                  string.Format("alert('{0}');window.location.href = '" + page + "'",
                    message.Replace("'", @"\'").Replace("\n", "\\n").Replace("\r", "\\r")),
                    true);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        #endregion

    }
}