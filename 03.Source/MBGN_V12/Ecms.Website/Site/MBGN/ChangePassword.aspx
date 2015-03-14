<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="Ecms.Website.Site.MBGN.ChangePassword" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        Để thay đổi mật khẩu, vui lòng nhập đầy đủ thông tin có dấu (<span class="required">*</span>)</h4>
        <br />
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 100%;" align="center" class="tableForm">
                <tr>
                    <td class="tdFirst indend">
                        Mật khẩu cũ <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="textbox txtCurrentPassword"
                            TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Mật khẩu mới <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="txtNewPassword textbox"
                            TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Xác nhận mật khẩu mới <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="txtConfirmNewPassword textbox"
                            TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" CssClass="indend" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnAccept" runat="server" CssClass="button" OnClick="btnAccept_Click"
                            Text="Xác nhận" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button cancel" OnClick="btnCancel_Click"
                            Text="Hủy bỏ" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View runat="server" ID="resultView">
           <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2">
                        Quý khách đã thay đổi mật khẩu thành công.</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnOK" Text="Về trang chủ" runat="server" 
                            onclick="btnOK_Click" CssClass="button cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
