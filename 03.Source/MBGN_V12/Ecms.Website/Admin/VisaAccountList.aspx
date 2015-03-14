<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="VisaAccountList.aspx.cs" Inherits="Ecms.Website.Admin.Security.VisaAccountList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý danh sách tài khoản Visa
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            
            <table >
                <tr>
                    <td>
                        Số TK Thanh toán:
                    </td>
                    <td>
                        <asp:TextBox ID="txtVisaNo" runat="server" CssClass="Textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
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
            <br />
            <div>
                <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                    AllowPaging="True" PageSize="15" Width="40%">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="VisaId" HeaderText="VisaId" Visible="False" />
                        <asp:BoundField DataField="VisaNo" HeaderText="Số TK Thanh toán" />
                        <asp:BoundField DataField="Remark" HeaderText="Ghi chú" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("VisaId") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Sửa</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("VisaId") %>'
                                    OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa số visa này không?')) return false;"
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
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table  align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa số visa thành công"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" OnClick="btnOK_Click"/>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
