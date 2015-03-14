<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ComplaintList.aspx.cs" Inherits="Ecms.Website.Admin.ComplaintList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
Quản lý ý kiến khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
            <table align="center" class="tableForm">
                <tr>
                    <td>
                        <a style="font-size:medium; font-weight:bold;" >Tình trạng phản hồi của khách hàng</a>
                        <%--<strong>Ý kiến phản hồi:</strong>--%>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                    <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                    Width="100%" AllowPaging="True" PageSize="5" OnPageIndexChanging="grdD_PageIndexChanging"
                    OnRowDeleting="grdD_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="CreatedDate" HeaderText="TG gửi phản hồi">
                            <ItemStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Title" HeaderText="Tiêu đề">
                            <ControlStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Tình trạng">
                            <ControlStyle Width="50px" />
                        <HeaderStyle Width="50px" />
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="LastModifyDate" HeaderText="Cập nhập cuối" >
                        <HeaderStyle Width="60px" />
                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                        </asp:BoundField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("Id") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Xem chi tiết</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("Id") %>'
                                    CommandName="Delete" OnCommand="lbtDelete_Click" OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa không?')) return false;">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table  style="width: 100%;" align="center" class="tableForm">
                <tr>
                    <td>
                       
                             &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" CssClass="indend" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
</asp:Content>
