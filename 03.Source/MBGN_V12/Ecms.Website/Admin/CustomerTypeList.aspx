<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerTypeList.aspx.cs" Inherits="Ecms.Website.Admin.Security.CustomerList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý nhóm khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            
            <table style="width:600px;">
                <tr>
                    <td>
                        Tên nhóm khách hàng:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="Textbox"></asp:TextBox>
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
                    Width="100%" AllowPaging="True" PageSize="15">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerTypeId" HeaderText="Mã nhóm khách hàng" Visible="False" />
                        <asp:BoundField DataField="CustomerTypeName" HeaderText="Tên nhóm khách hàng" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("CustomerTypeId") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Sửa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("CustomerTypeId") %>'
                                    OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa đối tượng khách hàng này không?')) return false;"
                                    CommandName="Delete" OnCommand="lbtDelete_Click" CssClass="deleteClick">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
            </div>
            <asp:Button ID="btnExportExcel" runat="server" Text="Xuất file excel" 
                OnClick="btnExportExcel_Click" CssClass="button" Visible="False" />
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table  align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa User thành công"></asp:Label>
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
