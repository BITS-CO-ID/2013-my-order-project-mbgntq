using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ecms.Biz;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using CommonUtils;


namespace Ecms.Website.Admin
{
    public class PageBase : Page
    {

        protected override void OnInit(EventArgs e)
        {
            this.DoCheckPermision();
            base.OnInit(e);
        }

        #region // DoCheckPermision

        private void DoCheckPermision()
        {
            if (Session["User"] == null)
            {
                Response.Redirect("/admin/security/login.aspx");
            }

            string userCode = Session["User"].ToString();
            if (userCode.Equals(Constansts.sysadmin)) return;

            var cService = new UserBiz();

            var lstCurrentUser = cService.GetUserForMap(userCode
                                , ""
                                , ""
                                , ""
                                , ""
                                , "1"
                                );

            var resultPermis = lstCurrentUser.Where(p => p.ObjectTemplate != null && p.ObjectTemplate.ToLower().Contains(this.Request.Path.ToLower()));
            if (resultPermis.Count() <= 0)
            {
                Response.Redirect("/Admin/NotPermision.aspx");
            }

			if (lstCurrentUser.Where(p => p.ObjectCode == "Func_MH").Count()>0)
			{
				Session["PolicyUserMH"] = "1";
			}
        }
        #endregion

        #region // Set Button Auth
        protected void SetButtonAuth()
        {
            if (Session["User"] == null)
            {
                Response.Redirect("/Admin/Admin.aspx");
            }
            string userCode = Session["User"].ToString();
            if (userCode.Equals(Constansts.sysadmin)) return;

			if (!Constansts.IsAuthenticateOn)
			{
				return;
			}
            // get list function
            var cService = new UserBiz();
            var lstCurrentUser = cService.GetUserForMap(userCode
                                , ""
                                , ""
                                , ""
                                , "FUN"
                                , "1"
                                );

            ArrayList controlList = new ArrayList();
            AddControls(Page.Controls, controlList);

            foreach (Control control in controlList)
            {
                if (control is Button)
                {
                    Button button = (Button)control;
                    object tag = button.CommandName;
                    if (tag != null && !tag.ToString().Equals(""))
                    {
                        string functionCode = Convert.ToString(tag);
                        if (lstCurrentUser.Where(p => p.ObjectTemplate != null && p.ObjectTemplate.Contains(functionCode)).Count() <= 0)
                        {
                            button.Enabled = false;
                            button.CssClass = "buttonDisable";
                        }
                    }
                }
            }
        }

        public void AddControls(ControlCollection page, ArrayList controlList)
        {
            foreach (Control c in page)
            {
                if (c.ID != null)
                {
                    controlList.Add(c);
                }

                if (c.HasControls())
                {
                    AddControls(c.Controls, controlList);
                }
            }
        }
        #endregion //Set Button Auth
    }
}
