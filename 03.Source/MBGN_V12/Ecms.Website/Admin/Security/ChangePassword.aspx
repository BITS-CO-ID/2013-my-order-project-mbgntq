<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="Ecms.Website.Admin.Security.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Đổi mật khẩu
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Đổi mật khẩu
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ID="mtvMain" runat="server" ActiveViewIndex="0">
        <asp:View ID="formView" runat="server">
            <table width="50%" >
                <tr>
                    <td style="width: 150px;">
                        Mã người dùng
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserCode" runat="server" CssClass="Textbox" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mật khẩu hiện tại(<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtOlderPassword" runat="server" CssClass="Textbox" TextMode="Password"
                            AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mật khẩu mới(<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewPass" runat="server" CssClass="Textbox" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Xác nhận mật khẩu(<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirm" runat="server" CssClass="Textbox" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnSave" runat="server" Text="Xác nhận" CssClass="Button" OnClick="btnSave_Click"
                    OnClientClick="return confirm('Bạn có chắc muốn đổi mật khẩu?');" />
                <asp:Button ID="btnCancel" runat="server" Text="Hủy bỏ" CssClass="Button" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" OnClick="btnOK_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
