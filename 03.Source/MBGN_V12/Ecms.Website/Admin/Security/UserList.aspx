<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="UserList.aspx.cs" Inherits="Ecms.Website.Admin.Security.UserList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Danh sách người dùng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <style type="text/css">
        .deleteClick
        {
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".deleteClick").click(function (event) {
                var result = confirm("Bạn có chắc chắn muốn xóa người dùng này không?");
                if (result == false) {
                    event.preventDefault();
                }
            });
        });
    </script>
    <div class="content-box">
        <div class="content-box-content">
            <div class="" id="tab1">
                <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
                    <asp:View ID="fromView" runat="server">
                        <table >
                            <tr>
                                <td style="width: 150px;">
                                    Tên đăng nhập:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserCode" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mã người dùng:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Trạng thái:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboStatus" runat="server" CssClass="Cbo">
                                        <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                                        <asp:ListItem Value="1">Hoạt động</asp:ListItem>
                                        <asp:ListItem Value="0">Khóa</asp:ListItem>
                                    </asp:DropDownList>
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
                            &nbsp;<asp:Button ID="btnAddNew" runat="server" CssClass="Button" OnClick="btnAddNew_Click"
                                Text="Thêm mới" />
                        </div>
                        <div>
                            <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                                Width="100%" AllowPaging="true" PageSize="15" OnPageIndexChanging="grdD_PageIndexChanging"
                                OnRowDataBound="grdD_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="STT">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserCode" HeaderText="Tên đăng nhập" />
                                    <asp:BoundField DataField="UserName" HeaderText="Tên người dùng" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:TemplateField HeaderText="Trạng thái">
                                        <ItemTemplate>
                                            <%# Eval("FlagActive").ToString() == "1" ? "Hoạt động" :"Khóa" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="admin">
                                        <ItemTemplate>
                                            <%# Eval("SupperAdmin")==null?"":Eval("SupperAdmin").ToString() == "1" ? "admin" :"" %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtEdit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("UserCode") %>'
                                                OnCommand="lbtEdit_Click">Sửa</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("UserCode") %>'
                                                OnCommand="lbtDelete_Click" CssClass="deleteClick">Xóa</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnLinkDetail" runat="server" CommandName="Detail" CommandArgument='<%#Eval("UserCode") %>'
                                                OnCommand="btnLinkDetail_Click">Xem chi tiết</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reset mật khẩu">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnLinkReset" runat="server" CommandName="Reset" CommandArgument='<%#Eval("UserCode") %>'
                                                OnCommand="btnLinkReset_Click"> Reset </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
                        </div>
                    </asp:View>
                    <asp:View ID="resultView" runat="server">
                        <table align="center" style="width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa User thành công"></asp:Label>
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
