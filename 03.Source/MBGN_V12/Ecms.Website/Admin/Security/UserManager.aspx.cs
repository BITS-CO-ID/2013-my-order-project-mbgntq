using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Ecms.Website.Admin.Security
{
    public partial class UserManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] forUser = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            if (forUser != null)
            {
                bool flag = false;
                for (int i = 0; i < forUser.Count(); i++)
                {
                    string x = forUser[i];
                    if (x != "Quyền Administrator")
                    {
                        flag = true;
                    }else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    Response.Redirect("~/Admin/Permisstion.aspx");
                }
            }            
            if (!IsPostBack)
            {
                UserGrid.DataSource = Membership.GetAllUsers();
                UserGrid.DataBind();   
            }            
            UserGrid.RowDeleting += new GridViewDeleteEventHandler(UserGrid_RowDeleting);
            UserGrid.RowEditing += new GridViewEditEventHandler(UserGrid_RowEditing);
            UserGrid.RowCancelingEdit += new GridViewCancelEditEventHandler(UserGrid_RowCancelingEdit);
            UserGrid.RowUpdating += new GridViewUpdateEventHandler(UserGrid_RowUpdating);
        }

        void UserGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = UserGrid.Rows[e.RowIndex];                        
            var text = ((TextBox)row.FindControl("userNameTxt")).Text;
            var email = ((TextBox)row.FindControl("emailTxt")).Text;
            var user = Membership.GetUser(text);
            user.Email = email;
            Membership.UpdateUser(user);
            UserGrid.EditIndex = -1;
            UserGrid.DataSource = Membership.GetAllUsers();
            UserGrid.DataBind();           
        }

        void UserGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            UserGrid.EditIndex = -1;
            UserGrid.DataSource = Membership.GetAllUsers();
            UserGrid.DataBind();
        }

        void UserGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            UserGrid.EditIndex = e.NewEditIndex;
            UserGrid.DataSource = Membership.GetAllUsers();
            UserGrid.DataBind();
        }

        void UserGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            GridViewRow row = UserGrid.Rows[e.RowIndex];
            var text = ((Label)row.FindControl("userNameLbl")).Text;

            if (text == "Admin")
            {
                e.Cancel = true;
                MessageLtr.Text = "<div class=\"notification error png_bg\"><a href=\"#\" class=\"close\"><img src=\"/App_Layouts/UIT/images/icons/cross_grey_small.png\" title=\"Đóng thông báo\"alt=\"close\" /></a><div>Bạn không có quyền xóa user Admin!</div></div>";
            }
            else
            {
                Membership.DeleteUser(text);
                UserGrid.DataSource = Membership.GetAllUsers();
                UserGrid.DataBind();
                MessageLtr.Text += "<div class=\"notification success png_bg\"><a href=\"#\" class=\"close\"><img src=\"/App_Layouts/UIT/images/icons/cross_grey_small.png\" title=\"Đóng thông báo\"alt=\"close\" /></a><div>Xóa thành công!</div></div>";
            }
        }

        protected void ReloadGrid_Click(object sender, EventArgs e)
        {
            UserGrid.DataSource = Membership.GetAllUsers();
            UserGrid.DataBind();   
        }
    }
}