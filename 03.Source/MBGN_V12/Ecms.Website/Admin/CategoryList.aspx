<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CategoryList.aspx.cs" Inherits="Ecms.Website.Admin.Security.CategoryList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý nhóm sản phẩm
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <table>
                <tr>
                    <td>
                        Tên nhóm sản phẩm:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="Textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
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
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Loại sản phẩm">
                                    <ItemTemplate>
                                        <%#getCategoryParentName(Eval("ParentId") == null ? "" : Eval("ParentId").ToString())%>
                                    </ItemTemplate>
                                    <ControlStyle Width="10px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CategoryName" HeaderText="Tên nhóm sản phẩm" />                        
                        <asp:BoundField DataField="Remark" HeaderText="Mô tả" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("CategoryId") %>'
                                    CommandName="Edit" OnCommand="lbtEdit_Click">Sửa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("CategoryId") %>'
                                    CommandName="Delete" OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa nhóm sản phẩm này không?')) return false;"
                                    OnCommand="lbtDelete_Click">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
                <br />
                <asp:Button ID="btnExportExcell" runat="server" CssClass="button" 
                    Text="Xuất file excel" Visible="False" />
            </div>
            
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa nhóm sản phẩm thành công"></asp:Label>
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
