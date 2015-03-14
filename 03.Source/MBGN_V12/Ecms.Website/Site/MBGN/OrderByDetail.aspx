<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderByDetail.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderByDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        Chi tiết đơn hàng</h4>
    <asp:Panel runat="server" ID="pnlOrder">
    <div>
    <table class="tableForm" style="width: 100%;" align="center" cellpadding="5">
        <tr>
            <td class="tdFirst">
                Mã đơn hàng:
            </td>
            <td>
                <asp:Label ID="lblOrderNo" runat="server" ForeColor="#555555" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Ngày đặt hàng:
            </td>
            <td>
                <asp:Label ID="lblOrderDate" runat="server" ForeColor="#555555" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Tình trạng:
            </td>
            <td>
                <asp:Label ID="lblOrderStatus" runat="server" ForeColor="#555555" ></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlOrderDetail">
        <asp:GridView ID="gridCart" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            Width="100%" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Center" />
                    <FooterTemplate>
                        <strong>
                            Tổng:
                        </strong>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã MH">
                        <ItemTemplate>
                            <%# Eval("DetailCode")%>
                        </ItemTemplate>
                        <ItemStyle Width="60" HorizontalAlign="Left" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Mã sản phẩm">
                    <ItemTemplate>
                        <%# Eval("ProductCode")%>
                    </ItemTemplate>
                    <ItemStyle Width="200" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên sản phẩm">
                    <ItemTemplate>
                        <%# Eval("ProductName")%>
                    </ItemTemplate>                    
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Đơn giá">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("PriceWeb") ?? 0).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Right" />
                </asp:TemplateField>
               <asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                        <%# Eval("Quantity")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thành tiền(VND)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("TotalMoney") ?? 0).ToString("N0")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong>
                            <%= dTotalMoney.ToString("N0") %>
                        </strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <ItemStyle Width="100" HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table style="width: 300px; margin-top: 20px;" align="right" cellpadding="5" class="tableForm">
            <tr>
                <td style="text-align: right; padding-top: 10px;" colspan="2">
                    <asp:Button ID="btnOrder" runat="server" Text="Thanh toán" CssClass="button" onclick="btnOrder_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Quay lại" CssClass="button" onclick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
