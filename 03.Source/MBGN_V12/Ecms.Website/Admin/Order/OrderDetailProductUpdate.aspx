<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderDetailProductUpdate.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderDetailProductUpdate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cập nhật sản phẩm gửi
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" ID="mtvMain" runat="server">                    
        <asp:View ID="step2View" runat="server">
            <table style="width: 500px;" class="tableForm">
                <tr>
                    <td colspan="2">
                        <strong>THÔNG TIN HÀNG GỬI</strong>
                    </td>
                </tr>
               
                <tr>
                    <td class="indend">
                        Tên sản phẩm
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductName" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="indend">
                        Cân nặng (kg)
                    </td>
                    <td>
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="indend">
                        Phí ship mới(VNĐ)</td>
                    <td>
                        <asp:TextBox ID="txtShipModified" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Số lượng
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
                    <td class="indend">
                        Phụ thu
                    </td>
                    <td>
                        <asp:TextBox ID="txtSurcharge" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Giá khai báo</td>
                    <td>
                        <asp:TextBox ID="txtDeclarePrice" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend" colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" />
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" Text="Xác nhận" OnClick="btnAccept_Click" CssClass="button" OnClientClick="return confirm('Bạn có chắc chắn muốn cập nhật thông tin của món hàng này?');"/>
                <asp:Button ID="btnBack" runat="server" CssClass="button cancel" OnClick="btnBack_Click" Text="Quay lại" />
            </div>
        </asp:View>
        <asp:View ID="step4View" runat="server">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <strong>Đã cập nhật thông tin đơn hàng!</strong></td>
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
