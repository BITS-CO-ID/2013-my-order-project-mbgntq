<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ConfirmPayment.aspx.cs" Inherits="Ecms.Website.Admin.Order.ConfirmPayment" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Literal ID="litTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
        <asp:View ID="formView" runat="server">
            <table style="width: 700px;">
                <tr runat="server" id="trCreatedUser">
                    <td>
                        Người gửi thanh toán
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedUser" runat="server" Font-Bold="true"></asp:Label>
                    </td> 
                </tr>
                <tr runat="server" id="trFromAccount">
                    <td>
                        Số tài khoản
                    </td>
                    <td>
                        <asp:Label ID="lblFromAccount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="tr1">
                    <td>
                        Thanh toán cho đơn hàng
                    </td>
                    <td>
                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ngày thanh toán
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Loại thanh toán
                    </td>
                    <td>
                        <asp:Label ID="lblTypePayment" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số tiền
                    </td>
                    <td>
                        <asp:Label ID="lblTotalMoney" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>(VND)
                    </td>
                </tr>
                <tr>
                    <td>
                        Nội dung
                    </td>
                    <td rowspan="3">
                        <asp:TextBox ID="txtContent" runat="server" Height="60px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr runat="server" id="trRemark">
                    <td>
                        Nội dung trả lời
                    </td>
                    <td rowspan="3">
                        <asp:TextBox ID="txtReplyContent" runat="server" Height="60px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Xác nhận thanh toán" OnClick="btnAccept_Click" OnClientClick="return confirm('Bạn có chắc muốn xác nhận thanh toán');" />
                <asp:Button ID="btnReject" runat="server" CssClass="button" Text="Không khớp thanh toán" OnClick="btnReject_Click" OnClientClick="return confirm('Bạn có chắc muốn xác nhận Không khớp thanh toán này');" />
                <asp:Button ID="btnRevertPayment" runat="server" CssClass="button" Text="Hoàn lại thanh toán" OnClick="btnRevertPayment_Click" OnClientClick="return confirm('Bạn có chắc muốn Hoàn lại thanh toán này');" />
                <br />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table width="50%" border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#0066FF" Text=" Đã xác nhận thông tin thanh toán"></asp:Label>
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
