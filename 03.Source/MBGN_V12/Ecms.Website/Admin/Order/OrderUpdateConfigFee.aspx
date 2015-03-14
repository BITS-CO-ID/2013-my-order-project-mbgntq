<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderUpdateConfigFee.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderUpdateConfigFee" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cấu hình phí trả chậm
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
        <div>
            <asp:Label ID="Label1" Text="Thiết lập tính phí cho đơn hàng" Font-Bold="true" Font-Italic="true" runat="server" Font-Size="Medium"></asp:Label>
            <br />
        </div>
            <table style="width: 30%; margin-bottom: 0px;">
                <tr>
                    <td>
                        Có tính phí của đơn hàng này không?
                    </td>
                   <td>
                       <asp:CheckBox ID="chkCalFeeDelay" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số ngày trả chậm cho phép:
                    </td>
                   <td>
                       <asp:TextBox ID="txtDayAloowed" runat="server" CssClass="Textbox" Font-Bold="true" style='text-align:right;' ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mức áp phí(%):
                    </td>
                   <td>
                       <asp:TextBox ID="txtFee" runat="server" CssClass="Textbox" Font-Bold="true" style='text-align:right'></asp:TextBox>
                    </td>
                </tr>
            </table>
             <div class="btnLine">
                <asp:Button ID="btnConfirmConfigFee" runat="server" CssClass="button" Text="Xác nhận cấu hình phí" OnClick="btnConfirmConfigFee_Click"
                OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận thay đổi cấu hình phí cho đơn hàng này');" />
             </div>
             <div class="btnLine">
             </div>
             <div>
                <asp:Label ID="Label4" Text="Thay đổi giá trị đã tính" Font-Bold="true" Font-Italic="true" runat="server" Font-Size="Medium"></asp:Label>
            </div>
            <table style="width: 80%; margin-bottom: 0px;">
                <tr>
                    <td>
                        Phí trả chậm (Hệ thống đã tính) VNĐ:
                    </td>
                   <td>
                       <asp:TextBox ID="txtAmountFeeDelay" runat="server" CssClass="doubleNumber textbox" Font-Bold="true" Font-Size="Medium" style='text-align:right' Enabled="false"></asp:TextBox>
                       <asp:Label ID="Label2" Text="Đây là mức phí trả chậm hệ thống đã tính cho đơn hàng" Font-Bold="true" Font-Italic="true" runat="server"></asp:Label>
                       
                    </td>
                </tr>
                <tr>
                    <td>
                        Phí trả chậm mới VNĐ:
                    </td>
                   <td>
                       <asp:TextBox ID="txtAmountFeeDelayNew" runat="server" CssClass="doubleNumber textbox" Font-Bold="true" Font-Size="Medium" style='text-align:right'></asp:TextBox>
                       <asp:Label ID="Label3" Text="Mức phí này sau khi hệ thống tính toán, nếu người quản trị muốn thay đổi thì có thể nhập thông tin phí mới ở đây" Font-Bold="true" Font-Italic="true" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Xác nhận thay đổi " OnClick="btnAccept_Click"
                OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận thay đổi giá trị phí trả chậm cho đơn hàng này');" />
                <%--<asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Hủy bỏ" OnClick="btnCancel_Click" />--%>
            </div>
            <div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
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
