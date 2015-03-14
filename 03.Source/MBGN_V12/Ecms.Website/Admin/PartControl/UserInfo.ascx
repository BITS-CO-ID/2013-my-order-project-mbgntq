<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.ascx.cs" Inherits="Ecms.Website.Admin.PartControl.UserInfo" %>
<asp:Label ID="lblAccountInfo" runat="server" Font-Bold="True" />
<a href='<%= ResolveUrl("~/admin/security/changepassword.aspx") %>'>Đổi mật khẩu</a> |
&nbsp;<asp:LinkButton ID="lbtnLogout" Visible="true" runat="server" 
    OnClick="lbtnLogout_Click" 
    OnClientClick="return confirm('Bạn có chắc chắn muốn thoát?');" 
    Font-Bold="True" ForeColor="White">Thoát</asp:LinkButton>
