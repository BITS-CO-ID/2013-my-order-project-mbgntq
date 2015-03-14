<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductList.aspx.cs" Inherits="Ecms.Website.Admin.ProductList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <script type="text/javascript">
    </script>
    Quản lý sản phẩm
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                        <table style="width:650px">
                            <tr>
                                <td>
                                    Loại sản phẩm
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddCategoryIdParent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCategoryIdParent_SelectedIndexChanged"
                                        CssClass="Cbo">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Nhóm sản phẩm
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddCategoryId" runat="server" AutoPostBack="True" CssClass="Cbo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tên sản phẩm
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                                <td>
                                    Mã sản phẩm
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProductCode" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nhà cung cấp
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddVendorId" runat="server" CssClass="Cbo">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Xuất sứ
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddOrginal" runat="server" CssClass="Cbo">
                                        <asp:ListItem Value="-1">-- Tất cả --</asp:ListItem>
                                        <asp:ListItem Value="0">Mỹ</asp:ListItem>
                                        <asp:ListItem Value="1">Anh</asp:ListItem>
                                        <asp:ListItem Value="2">Khác</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Sản phẩm BestSale
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBestSale" runat="server" CssClass="Cbo">
                                        <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                                        <asp:ListItem Value="1">Best Sale</asp:ListItem>
                                        <asp:ListItem Value="0">Off</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Hiện thị sản phẩm
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPublished" runat="server" CssClass="Cbo">
                                        <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                                        <asp:ListItem Value="1">Có</asp:ListItem>
                                        <asp:ListItem Value="0">Không</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="btnLine">                
                        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" CssClass="Button" />
                        <asp:Button ID="btnAdd" runat="server" Text="Thêm mới" OnClick="btnAdd_Click" CssClass="Button" />
            </div>
            <div>
                <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                    Width="100%" AllowPaging="True" PageSize="15" OnPageIndexChanging="grdD_PageIndexChanging"
                    OnRowDeleting="grdD_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CategoryName" HeaderText="Nhóm sản phẩm">
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductCode" HeaderText="Mã sản phẩm">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderText="Tên sản phẩm">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="gVendorId" HeaderText="Nhà cung cấp" />
                        <asp:BoundField DataField="gOrginal" HeaderText="Xuất xứ" />
                        <asp:BoundField DataField="ProductLable" HeaderText="Thương hiệu" />
                        <asp:BoundField DataField="ProductTypeText" HeaderText="Loại mặt hàng" />
                        <asp:TemplateField HeaderText="Giá">
                            <ItemTemplate>
                                <%# Eval("ProductValue") == null ? "0" : Convert.ToDouble(Eval("ProductValue").ToString()).ToString("N0")%>
                            </ItemTemplate>
                            <ItemStyle Width="80px" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá SaleOff">
                            <ItemTemplate>
                                <%# Eval("ProductSaleValue") == null ? "0" : Convert.ToDouble(Eval("ProductSaleValue").ToString()).ToString("N0")%>
                            </ItemTemplate>
                            <ItemStyle Width="80px" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mô tả chung sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ShortDescription") == null ? "" : (Convert.ToString(Eval("ShortDescription")).Length >30 ?Convert.ToString(Eval("ShortDescription")).Substring(0,30):Eval("ShortDescription"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hiện thị<br/>sản phẩm">
                            <ItemTemplate>
                                <%# Eval("Published") == null ? "Không" : (Convert.ToBoolean(Eval("Published"))==true ? "Có" : "Không")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Thay đổi hiện thị<br/>SP BestSale">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtBestSale" runat="server" 
                                    Text='<%#Eval("BestSale")!=null?Eval("BestSale").ToString().Equals("1")?"Best Sale":"Off":"Off"%>'
                                    CommandArgument='<%#Eval("ProductId")+"|"+ Eval("BestSale")%>'
                                    OnCommand="lblBestSale_Click" 
                                    OnClientClick="return confirm('Bạn có chắc chắn muốn thay đổi hiện thị BestSale cho sản phẩm này không?');"
                                    CommandName="BestSale"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("ProductId") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Sửa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("ProductId") %>'
                                    CommandName="Delete" OnCommand="lbtDelete_Click" OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa sản phẩm này không?')) return false;">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
                <br />
                <asp:Button ID="btnExportExcell" runat="server" CssClass="button" Text="Xuất file excel"
                    Visible="False" />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table  align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa sản phẩm thành công"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
