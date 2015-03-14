using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Website.DBServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ecms.Website.Common
{
	public class Utils
	{
		private const string TITLE_ERR_BOX = "Lỗi";

		public static void ShowExceptionBox(Exception ex,Page page)
		{
			ShowExceptionBox(TITLE_ERR_BOX, ex, page);
		}

		public static void ShowExceptionBox(string title, Exception ex, Page page)
		{
			if (ex is ServiceException)
			{
				var sex = (ServiceException)ex;
				//FrmErrorMessage frm = new FrmErrorMessage();
				//frm.Text = title;
				//string msgCode = sex.ErrorMessage;
				//string msgValue = ErrorMsgHandler.Instance.GetMessage(msgCode);
				//frm.SetErrorMessage(msgValue, sex.ErrorDetail);
				//frm.ShowDialog();

				//new HttpResponse().Write("<script type=\"text/javascript\">window.showModalDialog('ForgotPassword.aspx',null,'status:no;center:yes;dialogWidth:500px;dialogHeight:300px;help:no;scroll:no');</script>");
				
                var litError = (Literal)page.Master.FindControl("litError");
				if (litError != null)
				{
					litError.Text = string.Format("<div class=\"notification error png_bg\"><a href=\"#\" class=\"close\"><img src=\"/App_Layouts/UIT/images/icons/cross_grey_small.png\" title=\"Đóng thông báo\"alt=\"close\" /></a><div> {0}\r\n{1}!</div></div>", "", sex.ErrorDetail);
				}
			}
			else
			{				
                var litError = (Literal)page.Master.FindControl("litError");
				if (litError != null)
				{
					litError.Text = string.Format("<div class=\"notification error png_bg\"><a href=\"#\" class=\"close\"><img src=\"/App_Layouts/UIT/images/icons/cross_grey_small.png\" title=\"Đóng thông báo\"alt=\"close\" /></a><div> {0}\r\n{1}!</div></div>", ex.Message, ex.StackTrace);
					//MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

					//new HttpResponse().Write("<script type=\"text/javascript\">window.showModalDialog('ForgotPassword.aspx',null,'status:no;center:yes;dialogWidth:500px;dialogHeight:300px;help:no;scroll:no');</script>");
				}
			}
		}		
	}
}