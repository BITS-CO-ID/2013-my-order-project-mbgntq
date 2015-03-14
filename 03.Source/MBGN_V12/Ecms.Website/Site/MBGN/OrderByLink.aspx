<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderByLink.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderByLink" %>


    <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">


    <h4 class="title-page">
        Quý khách vui lòng điền đầy đủ thông tin vào các ô (<span class="required">*</span>)
        để đặt hàng</h4>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table class="tableForm" style="width: 100%;" align="center">
                <tr>
                    <td colspan="4" class="td-bg">
                        THÔNG TIN ĐẶT HÀNG
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tdFirst indend">
                        Website<strong> </strong><span class="required">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWebsiteGroup" runat="server" CssClass="ddlWebsiteGroup">
                        </asp:DropDownList>
                    </td>
                    <td class="tdFirst indend">
                        Số lượng<strong> </strong><span class="required">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="doubleNumber textbox txtQuantity"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Shop
                    </td>
                    <td>
                        <asp:TextBox ID="txtShop" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="indend">
                        Giá web (NDT) <asp:Label ID="lblCurencyPriceWeb" runat="server" Visible="False"></asp:Label> <span class="required"><strong>*</strong></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceWeb" runat="server" CssClass="doubleNumber textbox" Font-Size="Medium" Font-Bold="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Link sản phẩm<strong> </strong><span class="required"><strong>*</strong></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLinkProduct" runat="server" CssClass="textbox txtLinkProduct"></asp:TextBox>
                    </td>
                    <td class="indend">
                        Màu sắc
                    </td>
                    <td>
                        <asp:TextBox ID="txtColor" runat="server"></asp:TextBox>
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
                        <asp:TextBox ID="txtSize" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
   
    <p class="p-note"></p>
    <asp:Button ID="btnAddToCartLink" runat="server" CssClass="button" Text="Thêm link sản phẩm" OnClick="btnAddToCartLink_Click" />
    <asp:Panel runat="server" ID="pnCartLink" Visible="false">
        <br />        
        <asp:GridView ID="gridCartByLink" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            OnRowCommand="gridCartByLink_RowCommand" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Center" />
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Website">
                    <ItemTemplate>
                        <%# Eval("ParentWebsiteName")%>
                    </ItemTemplate>
                    <ItemStyle Width="140" />
                </asp:TemplateField>--%>
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
                    <ItemStyle Width="80" />
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
                    <ItemStyle Width="60" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("Quantity").ToString()).ToString("N2")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Size">
                    <ItemTemplate>
                        <%# Eval("Size") %>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Màu sắc">
                    <ItemTemplate>
                        <%# Eval("Color") %>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Left" />
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
        <br />
        <table style="width: 200px;" align="right">
            <tr>
                <td style="text-align: right;">
                    <asp:Button Text=" Đặt hàng " ID="btnOrder" CssClass="button cancel" runat="server" OnClick="btnOrder_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="button cancel" Text=" Hủy bỏ " OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>


</asp:Content>

