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
	public partial class VisaAccountCreate : PageBase
	{
		#region // Declaration

		private readonly CommonService _commonService = new CommonService();

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
			lblError.Text = "";
			lblError.Visible = true;
			if (string.IsNullOrEmpty(txtVisaNo.Text.Trim()))
			{
				lblError.Text = "Số TK TT đã có trong hệ thống";
				lblError.Visible = true;
				return;
			}
			try
			{
				if (Request.QueryString["id"] != null)
				{
					// validate
					var visaNoOld = hdnVisaNo.Value;
					var list = _commonService.VisaAccountGet("", "", this).Where(p => p.VisaNo != visaNoOld && p.VisaNo == txtVisaNo.Text.Trim()).ToList();
					if (list.Count > 0)
					{
						lblError.Text = "Số TK TT đã có trong hệ thống";
						lblError.Visible = true;
						return;
					}

					var objVisa = new Mst_VisaAccount
					{
						VisaId = Convert.ToInt32(Request.QueryString["id"])
						,
						VisaNo = txtVisaNo.Text
						,
						Remark = txtRemark.Text
					};

					var result = this._commonService.UpdateVisaAccount(objVisa, this);
					if (result)
					{
						mtvMain.ActiveViewIndex = 1;
						lblResult.Text = "Đã cập nhật số TK TT thành công";
					}
				}
				else
				{
					// validate
					var list = _commonService.VisaAccountGet("", txtVisaNo.Text.Trim(), this);
					if (list.Count > 0)
					{
						lblError.Text = "Số TK TT đã có trong hệ thống";
						lblError.Visible = true;
						return;
					}

					var objVisa = new Mst_VisaAccount
					{
						VisaNo = txtVisaNo.Text
						,
						Remark = txtRemark.Text
					};

					var result = this._commonService.AddVisaAccount(objVisa, this);
					if (result != null)
					{
						mtvMain.ActiveViewIndex = 1;
						lblResult.Text = "Đã thêm mới số TK TT thành công";
					}
				}
			}
			catch (Exception exc)
			{
				Utils.ShowExceptionBox(exc, this);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("VisaAccountList.aspx");
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			Response.Redirect("VisaAccountList.aspx");
		}
		#endregion

		#region // Method

		private void loadEdit()
		{
			if (Request.QueryString["id"] != null)
			{
				lblFunction.Text = "Sửa";
				var result = this._commonService.VisaAccountGet(Request.QueryString["id"], "", this).SingleOrDefault();

				txtVisaNo.Text = result == null ? "" : result.VisaNo;
				hdnVisaNo.Value = result == null ? "" : result.VisaNo;
				txtRemark.Text = result == null ? "" : result.Remark;
			}
			else
			{
				lblFunction.Text = "Thêm";
				txtVisaNo.Text = "";
			}
		}

		#endregion
	}
}