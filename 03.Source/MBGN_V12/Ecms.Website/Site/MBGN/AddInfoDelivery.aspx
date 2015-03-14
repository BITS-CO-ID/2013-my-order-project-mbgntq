<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="AddInfoDelivery.aspx.cs" Inherits="Ecms.Website.Site.MBGN.AddInfoDelivery" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="formView" runat="server">
            <h4 class="title-page">
                Thêm thông tin giao hàng</h4>
            <table style="width: 80%;" align="center">
                <tr>
                    <td class="tdFirst">
                        Họ và tên <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số điện thoại nhận hàng <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Địa chỉ <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thông tin thêm
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="textbox" Height="58px" 
                            Width="286px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnAccept" runat="server" Text="Xác nhận" CssClass="button" OnClick="btnAccept_Click"
                            OnClientClick="return confirm('Quý khách có chắc chắn muốn xác nhận đặt hàng?');" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" CssClass="button cancel"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table class="tableForm" style="width: 80%;" align="center">
                <tr>
                    <td colspan="2">
                        <b>Yêu cầu của quý khách đã được gửi đến QuangChau247.vn. Quý khách vui lòng
                            kiểm tra email để nhận thông tin chi tiết về đơn hàng. Xin cảm ơn!</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button Text="Quay lại" runat="server" ID="btnOK" CssClass="button" OnClick="btnOK_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
