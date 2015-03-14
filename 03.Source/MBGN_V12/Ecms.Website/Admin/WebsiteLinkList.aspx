<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="WebsiteLinkList.aspx.cs" Inherits="Ecms.Website.Admin.WebsiteLinkList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý website link
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
                        <table >
                            <tr>
                                <td>
                                    Website
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddWebsiteParentId" runat="server" CssClass="Cbo">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tên website/shop
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWebsiteName" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Website Link
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWebsiteLink" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
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
                        <asp:BoundField DataField="WebsiteId" HeaderText="Mã website" Visible="False" />
                        <asp:BoundField DataField="ParentName" HeaderText="Website" />
                        <asp:BoundField DataField="WebsiteName" HeaderText="Tên website/shop"></asp:BoundField>
                        <asp:BoundField DataField="WebLink" HeaderText="Website link"></asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Mô tả">
                            <ControlStyle Width="50px" />
                            <ControlStyle Width="50px" />
                            <ControlStyle Width="50px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("WebsiteId") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Sửa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("WebsiteId") %>'
                                    CommandName="Delete" OnCommand="lbtDelete_Click" OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa website link này không?')) return false;">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
                <br />
                <%--<asp:Button ID="btnExportExcell" runat="server" CssClass="button" 
                            Text="Xuất file excel" />--%>
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table  align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa Website thành công"></asp:Label>
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
