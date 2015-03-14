<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderByDetail.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderByDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Chi tiết đơn hàng sản phẩm
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 65%;" >
                <tr>
                    <td class="tdFirst">
                        <strong>Mã đơn hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Ngày tạo </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Mã khách hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerCode" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Số điện thoại </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Tên khách hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Địa chỉ </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <p class="p-note">
                Chi tiết đơn hàng</p>
            <div class="data">
                <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <strong>
                                    Tổng:
                                </strong>
                            </FooterTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductCode")%>
                            </ItemTemplate>
                            <ItemStyle Width="200" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sản phẩm">
                            <ItemTemplate>
                                <a href='<%= ResolveUrl("~/site/mbgn/ProductDetail.aspx?proId=") %><%# Eval("ProductId") %>'>
                                    <%# Eval("ProductName") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Đơn giá">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("PriceWeb").ToString()).ToString("N2")%>
                            </ItemTemplate>
                            <ItemStyle Width="100" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("Quantity").ToString()).ToString("N2")%>
                            </ItemTemplate>
                            <ItemStyle Width="100" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thành tiền(VND)">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("TotalMoney").ToString()).ToString("N0")%>
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

                <div style="padding-top:15px;">
                    <asp:Button ID="btnConfirm" runat="server" Text="Xác nhận đơn hàng" CssClass="button" OnClick="btnConfirm_Click" OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận đơn hàng này?');"/>
                    <asp:Button ID="btnCancel" runat="server" Text="Hủy đơn hàng" CssClass="button" OnClick="btnCancel_Click" OnClientClick="return confirm('Bạn c chắc chắn muốn Hủy đơn hàng này?');"/>
                    <asp:Button ID="btnConfirmDelivery" runat="server" Text="Xác nhận Đã giao hàng" CssClass="button" OnClick="btnConfirmDelivery_Click" OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận Giao Hàng cho đơn hàng này?');"/>
                    <asp:Button ID="btnReturn" runat="server" Text="Quay lại" CssClass="button" OnClick="btnReturn_Click" />
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="resultView">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" align="center" style="font-size:14px;">
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding:15px;">
                        <asp:Button ID="btnOK" Text="OK" runat="server" OnClick="btnOK_Click" CssClass="button cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
