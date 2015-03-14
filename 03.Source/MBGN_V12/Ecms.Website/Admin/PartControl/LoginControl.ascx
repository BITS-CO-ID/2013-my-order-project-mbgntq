<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="Ecms.Website.Admin.PartControl.LoginControl" %>
<div class="ContentForm">
    <table style="width: 100%;" cellspacing="5">
        <tr>
            <td align="center" colspan="3">
                <span class="validateMessage" style="color: Red;"></span>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Tên đăng nhập</strong>
            </td>
            <td>
                <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Mật khẩu</strong>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdError" colspan="3">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td>
                <asp:Button ID="btnLogin" runat="server" CssClass="button" Text="Đăng nhập" OnClick="btnLogin_Click" />
            </td>
            <td>&nbsp;
            </td>
        </tr>
    </table>
</div>
