<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderDetailDeliveryUpdate.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderDetailDeliveryUpdate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cập nhật gửi hàng vận chuyển
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" ID="mtvMain" runat="server">
        <asp:View ID="step2View" runat="server">
            <table style="width: 500px" class="tableForm">
                <tr>
                    <td colspan="2">
                       <strong> THÔNG TIN HÀNG GỬI</strong>
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
                        Cân nặng
                    </td>
                    <td>
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Phí Ship mới</td>
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
                        <asp:TextBox ID="txtDeclarePrice" runat="server" 
                            CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend" colspan="2">
                        <asp:Label ID="lblErrorStep2" runat="server" ForeColor="Red" Visible="False" />
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnAddToCartTransport" runat="server" Text="Thêm vào đơn hàng" OnClick="btnAddToCartTransport_Click" CssClass="button" />
                <asp:Button ID="btnPrevious" runat="server" CssClass="button cancel" OnClick="btnPrevious_Click" Text="Quay lại" />
            </div>
            <asp:Panel runat="server" ID="pnCartTransport" Visible="false">
                <p class="p-note">
                    CHI TIẾT ĐƠN HÀNG GỬI
                </p>
                <asp:GridView ID="gridCartTransport" runat="server" AutoGenerateColumns="False" OnRowCommand="gridCartTransport_RowCommand"
                    Width="100%" CssClass="gridview" AllowPaging="True" OnPageIndexChanging="gridCartTransport_PageIndexChanging"
                    PageSize="15" OnDataBound="gridCartTransport_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã MH">
                            <ItemTemplate>
                                <%# Eval("DetailCode") %>
                            </ItemTemplate>
                            <ItemStyle Width="60" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã Bill">
                            <ItemTemplate>
                                <asp:Literal Text='<%# Eval("TrackingNo")%>' ID="litTrackingNumber" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhóm sản phẩm">
                            <ItemTemplate>
                                <%# Eval("CategoryName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N1") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí Ship">
                            <ItemTemplate>
                                <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N2") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cân nặng<br/>(kg)">
                            <ItemTemplate>
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Giá khai báo">
                            <ItemTemplate>
                                <%# Eval("DeclarePrice") != null ? Convert.ToDouble(Eval("DeclarePrice").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bảo hiểm(%)">
                            <ItemTemplate>
                                <%# Eval("InsuaranceConfigId") != null ? Convert.ToDouble(Eval("InsuaranceConfigValue").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <p>
                    <asp:Button Text="Xác nhận" runat="server" CssClass="button" ID="btnAccept" OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận?');"
                        OnClick="btnAccept_Click" />
                </p>
            </asp:Panel>
        </asp:View>
        <asp:View ID="step4View" runat="server">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <strong>Yêu cầu gửi hàng đã được cập nhật thành công </strong>
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
