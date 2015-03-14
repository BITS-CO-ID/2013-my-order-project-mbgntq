<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="WebsiteAccountList.aspx.cs" Inherits="Ecms.Website.Admin.WebsiteAccountList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý tài khoản website
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    
                        <table >
                            <tr>
                                <td>
                                    Wwebsite
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddParrentWebsite" runat="server" CssClass="Cbo">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tài khoản mua hàng
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWebsiteAccountNo" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="btnLine">
                <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
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
                        <asp:BoundField DataField="WebsiteAccountId" HeaderText="WebsiteAccountId" Visible="False" />
                        <asp:BoundField DataField="WebsiteName" HeaderText="Tên website">
                            <ControlStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AccountWebsiteNo" HeaderText="Tài khoản mua hàng">
                            <ControlStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Website" HeaderText="Địa chỉ Website" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("WebsiteAccountId") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Sửa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("WebsiteAccountId") %>'
                                    CommandName="Delete" OnCommand="lbtDelete_Click" OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa tài khoản này không?')) return false;">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table  align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa account thành công"></asp:Label>
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
