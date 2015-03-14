<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="EditProfile.aspx.cs" Inherits="Ecms.Website.Site.MBGN.EditProfile" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <h4 class="title-page">
                Vui lòng nhập đầy đủ các thông tin có dấu (<span class="required">*</span>) để 
                thay đổi thông tin</h4>
            <table style="width: 100%;" align="center" class="tableForm">
                <tr>
                    <td colspan="3" class="td-bg">
                        THÔNG TIN CÁ NHÂN
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Họ và tên <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="textbox txtFullName"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Điện thoại <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="textbox txtMobile"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Email <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox txtEmail"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Địa chỉ <span class="required">*</span></td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox txtAddress"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Tỉnh/ Thành phố <span class="required">*</span></td>
                    <td>
                        <asp:DropDownList ID="ddlProvince" runat="server" CssClass="ddlProvince ddl large">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Tên người nhận hàng</td>
                    <td>
                        <asp:TextBox ID="txtDeliveryName" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="indend">
                        Số điện thoại nhận hàng</td>
                    <td>
                        <asp:TextBox ID="txtDeliveryMobile" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="indend">
                        Email</td>
                    <td>
                        <asp:TextBox ID="txtDeliveryEmail" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="indend">
                        Địa chỉ nhận hàng</td>
                    <td>
                        <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Xác nhận" OnClick="btnAccept_Click" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button cancel" Text="Hủy bỏ"
                            OnClick="btnCancel_Click" />
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
                        Bạn đã thay đổi thành công thông tin cá nhân trên QuangChau247.vn
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button Text="Về trang chủ" runat="server" ID="btnOK0" 
                            OnClick="btnOK_Click" CssClass="button" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
