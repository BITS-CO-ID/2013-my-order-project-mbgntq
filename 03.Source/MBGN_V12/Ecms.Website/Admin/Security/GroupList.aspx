<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="GroupList.aspx.cs" Inherits="Ecms.Website.Admin.Security.GroupList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý nhóm người dùng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- End .content-box-header -->
        <div class="content-box-content">
            <div class="" id="tab1">
                <asp:MultiView ID="mtvMain" runat="server" ActiveViewIndex="0">
                    <asp:View ID="formView" runat="server">
                        <table  width="30%">
                            <tr>
                                <td>
                                    Mã nhóm:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGroupCode" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tên nhóm:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="btnLine">
                            <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="Button" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnAddNew" runat="server" Text="Thêm mới" CssClass="Button" OnClick="btnAddNew_Click" />
                        </div>
                        <br />
                        <div>
                            <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                                Width="100%" AllowPaging="true" PageSize="15">
                                <Columns>
                                    <asp:TemplateField HeaderText="STT">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="50" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GroupCode" HeaderText="Mã nhóm" />
                                    <asp:BoundField DataField="GroupName" HeaderText="Tên nhóm" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtEdit" runat="server" CommandName="EditGroup" CommandArgument='<%#Eval("GroupCode") %>'
                                                OnCommand="lbtEdit_Click">Sửa</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtDelete" runat="server" CommandName="DeleteGroup" CommandArgument='<%#Eval("GroupCode") %>'
                                                OnCommand="lbtDelete_Click" CssClass="deleteClick" OnClientClick="return confirm('Bạn có chắc muốn xóa nhóm?');">Xóa</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50" HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Gán nhóm người dùng">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtGroupUser" runat="server" CommandName="GroupUser" CommandArgument='<%#Eval("GroupCode") %>'
                                                OnCommand="lbtGroupUser_Click">Gán nhóm người dùng</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="150" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gán nhóm chức năng">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtGroupObj" runat="server" CommandName="GroupObj" CommandArgument='<%#Eval("GroupCode") %>'
                                                OnCommand="lbtGroupObj_Click">Gán nhóm chức năng</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="150" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:View>
                    <asp:View ID="resultView" runat="server">
                        <table align="center" style="width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa nhóm thành công"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" CssClass="Button" OnClick="btnOK_Click" Text="OK" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>
