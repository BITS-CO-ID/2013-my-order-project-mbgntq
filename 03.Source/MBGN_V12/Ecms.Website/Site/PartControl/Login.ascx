<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Ecms.Website.Site.PartControl.Login" %>
<p class="p-note">
    Đăng nhập</p>
<br />
<table style="width: 80%;" align="center" class="tableForm">
    <tr>
        <td class="tdFirst">
            Tên đăng nhập<strong> </strong>
        </td>
        <td>
            <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tdFirst">
            Mật khẩu<strong> </strong>
        </td>
        <td>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="tdFirst" colspan="2">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tdFirst">
            &nbsp;
        </td>
        <td colspan="2">
            <asp:Button ID="btnLogin" runat="server" CssClass="button" OnClick="btnLogin_Click"
                Text="Đăng nhập" />
            &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button cancel" OnClick="btnCancel_Click"
                Text="Hủy bỏ" />
        </td>
    </tr>
    <tr>
        <td class="tdFirst">
            &nbsp;
        </td>
        <td colspan="2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="tdFirst">
            &nbsp;
        </td>
        <td colspan="2">
            <a href='<%= ResolveUrl("~/site/mbgn/ForgotPassword.aspx") %>'>Quên mật khẩu?</a>
        </td>
    </tr>
    <tr>
        <td class="tdFirst">
            &nbsp;
        </td>
        <td colspan="2">
            <asp:LinkButton ID="link1" Text="Đăng ký tài khoản" OnClick="link1_OnClick" runat="server"></asp:LinkButton>
            <%--<a href='<%= ResolveUrl("~/site/mbgn/Register.aspx?ordercart=1") %>'>Đăng ký tài khoản</a>--%>
        </td>
    </tr>
</table>
