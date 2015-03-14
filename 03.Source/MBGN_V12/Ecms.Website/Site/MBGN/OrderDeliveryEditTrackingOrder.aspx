<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderDeliveryEditTrackingOrder.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderDeliveryEditTrackingOrder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <p class="p-note">Cập nhật thông tin Tracking và Order Number</p>
    
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 50%;" cellpadding="5">
                <tr>
                    <td  style="width:150px;">
                       <strong> BillNo</strong> (<span style="color:Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrackingNumber" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <%--<tr>
                    <td class="tdFirst">
                        <strong>Order Number</strong>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>--%>
                <%--<tr>
                    <td>
                        Ngày đễn Mỹ</td>
                    <td>
                        <asp:TextBox ID="txtDateToUsa" runat="server" CssClass="txtDate"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <strong>Mua bảo hiểm hàng hóa</strong>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkInsuarance" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" CssClass="button" OnClick="btnAccept_Click"
                    Text="Xác nhận" />
                &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="button" OnClick="btnBack_Click"
                    Text="Quay lại" />
            </div>
        </asp:View>
        <asp:View runat="server" ID="resultView">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <b>Cập nhật thành công</b>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" Text="OK" runat="server" OnClick="btnOK_Click" CssClass="button cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
