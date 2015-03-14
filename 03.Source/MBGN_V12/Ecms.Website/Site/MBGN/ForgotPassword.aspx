<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs" Inherits="Ecms.Website.Site.MBGN.ForgotPassword" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        QUÊN MẬT KHẨU</h4>
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" class="td-bg">
                        Vui lòng nhập email mà bạn đã dùng để đăng ký để nhận hướng dẫn thay đổi mật khẩu.
                    </td>
                </tr>
                <tr>
                    <td class="tdFirst">
                        &nbsp;</td>
                    <td class="indend">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdFirst">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox txtEmail"></asp:TextBox>
                    </td>
                    <td class="indend">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSendMail" CssClass="button" runat="server" OnClick="btnSendMail_Click" Text="Gửi" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View runat="server" ID="resultView">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2">
                        Một email đã được đến email bạn vừa dùng để lấy lại mật khẩu. Hãy kiểm tra email
                        để nhận lại mật khẩu mới.
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button Text="Về trang chủ" runat="server" ID="btnOK"  CssClass="button cancel"
                            onclick="btnOK_Click"/>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
