<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="PaymentForward.aspx.cs" Inherits="Ecms.Website.Admin.Order.PaymentForward" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Literal ID="litTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
        <asp:View ID="formView" runat="server">
            <table style="width: 15%;">
                <tr runat="server" id="trCreatedUser">
                    <td>
                        Mã KH
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerCode" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trFromAccount">
                    <td>
                        Tên KH
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerName" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số dư hiện tại
                    </td>
                    <td>
                        <asp:Label ID="lblCurrentBalance" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số dư đóng băng
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceFreeze" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số dư khả dụng
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceAvaiabilyty" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                </table>
                <div class="btnLine">
                </div>
                <table style="width: 20%;">
                <tr>
                    <td>
                        Mã đơn hàng
                    </td>
                    <td >
                        <asp:Label ID="lblOrderNo" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tổng tiền
                    </td>
                    <td >
                        <asp:Label ID="lblSumAmount" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>
                        Đã thanh toán
                    </td>
                    <td >
                        <asp:Label ID="lblPaidAmount" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>
                        Còn lại
                    </td>
                    <td >
                        <asp:Label ID="lblRemainAmount" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                
               
                <%--<tr runat="server" id="trRemark">
                    <td>
                        Nội dung trả lời
                    </td>
                    <td rowspan="3">
                        <asp:TextBox ID="txtRemark" runat="server" Height="60px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                    </td>
                </tr>--%>
               
            </table>
            <div class="btnLine">
                <table>
                    <tr>
                        <td>
                            <strong>Nhập số tiền chuyển thanh toán cho đơn hàng trên, số tiền thanh toán  <= số dư khả dụng:</strong>
                        </td>
                        <td colspan="2">
                           <asp:TextBox ID="txtAmount" runat="server" CssClass="textbox" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" Font-Bold="true"></asp:Label>
            </div>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Xác nhận chuyển TT" OnClick="btnAccept_Click" OnClientClick="return confirm('Bạn có chắc muốn xác nhận chuyển thanh toán cho đơn hàng này');" />
                <asp:Button ID="btnBack" runat="server" CssClass="button" OnClick="btnBack_Click" Text="Quay lại" />
                
                <asp:HiddenField ID="hdOrderId" runat="server" />
                <asp:HiddenField ID="hdCustomerId" runat="server" />
                <asp:HiddenField ID="hdBalanceAvaibility" runat="server" />
                <br />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table width="50%" border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#0066FF" Text=" Đã xác nhận chuyển thông tin thanh toán cho đơn hàng"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOk" runat="server" CssClass="button" OnClick="btnOk_Click" Text="Ok" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
