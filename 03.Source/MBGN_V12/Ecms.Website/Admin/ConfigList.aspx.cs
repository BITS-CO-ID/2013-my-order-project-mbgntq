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
using System.Globalization;

namespace Ecms.Website.Admin
{
    public partial class ConfigList : PageBase
    {
        #region // Declaration

        private readonly CustomerService _customerService = new CustomerService();

        #endregion

        #region // Method

        private void DoSearch(int type)
        {
            if (type == 1)
            {


                lblError.Text = "";
                var fDate = new DateTime();
                var tDate = new DateTime();
                if (ValidData(ref fDate, ref tDate) == false) return;

                var list = _customerService.ConfigBusinessGet(""
                                                            , ""
                                                            , ""
                                                            , ""
                                                            , this.ddlOrderType.SelectedValue
                                                            , ""
                                                            , ""
                                                            , fDate.ToString("yyyy-MM-dd 00:00:00")
                                                            , tDate.ToString("yyyy-MM-dd 23:59:59")
                                                            , this
                                                            );

                if (list.Count > 0)
                {
                    grdD.Visible = true;
                    grdD.DataSource = list;
                    grdD.DataBind();
                    lblError.Text = "";
                    Session["ConfigList"] = list;
                }
                else
                {
                    grdD.Visible = false;
                    lblError.Text = "Không tìm thấy kết quả theo điều kiện tìm kiếm.";
                }
            }

            if (type == 2)
            {
                if (Session["ConfigList"] != null)
                {
                    var list = (List<ConfigBusinessModel>)Session["ConfigList"];
                    grdD.DataSource = list;
                    grdD.DataBind();
                }
            }
        }

        private bool ValidData(ref DateTime fDate, ref DateTime tDate)
        {
            if (string.IsNullOrEmpty(txtFromDate.Text))
            {
                lblError.Text = "Từ ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                fDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", viVN);
            }
            catch
            {
                lblError.Text = "Từ ngày không đúng định dạng!";
                lblError.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(txtToDate.Text))
            {
                lblError.Text = "Đến ngày không được để trống!";
                lblError.Visible = true;
                return false;
            }

            try
            {
                CultureInfo viVN = new CultureInfo("vi-VN");
                tDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", viVN);
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

        #region // Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Admin/Security/Login.aspx");
                }

                try
                {
                    this.txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DoSearch(1);
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
                DoSearch(1);
            }
            catch (Exception exc)
            {
                Utils.ShowExceptionBox(exc, this);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfigCreate.aspx");
        }

        protected void grdD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdD.PageIndex = e.NewPageIndex;
            DoSearch(2);
        }

		protected void gridMain_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{

				var configId = e.CommandArgument.ToString();

				switch (e.CommandName)
				{
					case "ChangedConfig":
						// Update lại ConfigBusiness
						var config = _customerService.ConfigBusinessGet(configId, "", "", "", "", "", "", "", "", this).OrderByDescending(p=>p.CreatedDate).FirstOrDefault();

						var newConfigValue=0;
						if(config!=null && config.ConfigValue==0)
						{
							newConfigValue=1;
						}else if(config!=null && config.ConfigValue==1)
						{
							newConfigValue=0;
						}else
						{
							return;
						}
						var result = _customerService.ConfigBusinessValueUpdate(configId, Convert.ToString(newConfigValue), this);
						if (result)
						{
							this.DoSearch(1);
						}
						break;
				}

			}
			catch (Exception ex)
			{
				Utils.ShowExceptionBox(ex, this);
			}
		}

        #endregion
    }
}