<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="DistributionPaymentManageCreate.aspx.cs" Inherits="Ecms.Website.Admin.Order.DistributionPaymentManageCreate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Literal ID="litTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
        <asp:View ID="formView" runat="server">
            <table style="width: 20%;">
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
                <b>Danh sách đơn hàng ĐÃ XÁC NHẬN đã hoặc đang chờ phân bổ thanh toán:</b>
                <br/>
                <br/>
                <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview" Width="100%" >
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đơn hàng">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdOrderId" runat="server" Value='<%# Eval("OrderId") %>' />
                                <%# Eval("OrderNo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tổng tiền">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("SumAmount")).ToString("N2") %>
                            </ItemTemplate>
                            <ItemStyle Width="200" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Đã phân bổ TT">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("PaidAmount")).ToString("N2")%>
                            </ItemTemplate>
                            <ItemStyle Width="200" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Còn lại">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("RemainAmount")).ToString("N2")%>
                            </ItemTemplate>
                            <ItemStyle Width="200" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phân bổ mới">
                            <ItemTemplate>
                                <%--<%# Convert.ToDouble(Eval("PaidNewAmount")).ToString("N2")%>--%>
                                <asp:TextBox ID="txtPaidNew" runat="server" CssClass="Textbox"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Width="200" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" Font-Bold="true"></asp:Label>
            </div>
            <br />
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
