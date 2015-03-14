<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderByLink.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderByLink" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<asp:MultiView ActiveViewIndex="0" ID="mtvMain" runat="server">
    <asp:View ID="step1View" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table class="tableForm" style="width: 60%">
                <tr>
                    <td colspan="4">
                        THÔNG TIN ĐẶT HÀNG
                    </td>
                </tr>
                
                <tr>
                    <td class="tdFirst indend">
                        Website <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWebsiteGroup" runat="server" CssClass="cbo">
                        </asp:DropDownList>
                    </td>
                    <td class="tdFirst indend">
                        Số lượng <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="doubleNumber textbox txtQuantity"></asp:TextBox>
                    </td>
                </tr
                <tr>
                    <td class="indend">
                        Shop:<span style="color: Red;">*</span>
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtShop" runat="server" CssClass="textbox"></asp:TextBox>
                        
                    </td>
                    <td class="indend">
                        Giá web (NDT) <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceWeb" runat="server" CssClass="doubleNumber textbox txtPriceWeb"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Link sản phẩm <span style="color: Red;">*</span>
                    </td>
                    <td>
                       <asp:TextBox ID="txtLinkProduct" runat="server" CssClass="textbox txtLinkProduct"></asp:TextBox>
                    </td>
                    <td class="indend">
                        Màu sắc
                    </td>
                    <td>
                        <asp:TextBox ID="txtColor" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Link ảnh sản phẩm
                    </td>
                    <td>
                        <asp:TextBox ID="txtLinkProductImage" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="indend">
                        Size
                    </td>
                    <td>
                        <asp:TextBox ID="txtSize" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="btnLine">
        <asp:Button ID="btnAddToCartLink" runat="server" CssClass="button" Text="Thêm link sản phẩm" OnClick="btnAddToCartLink_Click" />
    </div>
    <asp:Panel runat="server" ID="pnCartLink" Visible="false">
        <asp:GridView ID="gridCartByLink" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            OnRowCommand="gridCartByLink_RowCommand" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Website">
                    <ItemTemplate>
                        <%# Eval("WebsiteName")%>
                    </ItemTemplate>
                    <ItemStyle Width="80" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shop">
                    <ItemTemplate>
                        <%# Eval("Shop")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hình ảnh ">
                    <ItemTemplate>
                        <a target="_blank" href='<%# Eval("ImageUrl") %>'>
                            <img src='<%# Eval("ImageUrl") %>' width="30" height="30" title="Ảnh sản phẩm" /></a>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Link">
                    <ItemTemplate>
                        <a href='<%# Eval("ProductLink") %>' title='<%# Eval("ProductLink") %>' target="_blank">
                            <%# Eval("ProductLink").ToString().Length < 30 ? Eval("ProductLink").ToString() : (Eval("ProductLink").ToString().Substring(0,30)+"...") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá web">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("PriceWeb").ToString()).ToString("N2")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số lượng">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("Quantity").ToString()).ToString("N2")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Màu sắc">
                    <ItemTemplate>
                        <%# Eval("Color") %>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Size">
                        <ItemTemplate>
                            <%# Eval("Size")%>
                        </ItemTemplate>
                        <ItemStyle Width="100" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" Text="Xóa" CommandName="deleteProduct" CommandArgument='<%# Eval("OrderDetailId") %>'
                            OnClientClick="return confirm('Quý khách có chắc chắn muốn xóa?');" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table style="width: 200px; padding-top:15px">
            <tr>
                <td>
                    <asp:Button Text=" Đặt hàng " ID="btnOrder" CssClass="button cancel" runat="server" OnClick="btnOrderTemp_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="button cancel" Text=" Hủy bỏ " OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </asp:View>
     <asp:View ID="step2View" runat="server">
            <table style="width: 50%;" class="tableForm">
                <tr>
                    <td colspan="2" class="td-bg">
                        <strong>THÔNG TIN MUA HỘ HÀNG</strong>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Tài khoản yêu cầu (<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerUserCode" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Ghi chú
                    </td>
                    <td rowspan="3">
                        <asp:TextBox ID="txtNote" runat="server" CssClass="textbox txtNote" TextMode="MultiLine" Height="60px" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label1" Visible="false" ForeColor="Red" runat="server" />
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="Button1" runat="server" CssClass="button cancel" OnClick="btnOrder_Click" Text="Xác nhận" />
                <asp:Button ID="btnBack" runat="server" CssClass="button cancel" OnClick="btnBack_Click" Text="Quay lại" />
            </div>
        </asp:View>
        <asp:View ID="stepResult" runat="server">
            <div style="font-weight:bold; padding: 30px 0px; text-align:center;">
                    <asp:Label ID="Label2" Visible="true" runat="server" Text="Đã tạo đơn hàng mua hộ"/>
            </div>
            <div style="padding: 15px 0 px 15px 0px; text-align:center">
                <asp:Button ID="btnOK" runat="server" CssClass="button" OnClick="btnOK_Click" Text="   OK   " />
            </div>
        </asp:View>
    </asp:MultiView>
    <div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>           
    </div>
</asp:Content>
