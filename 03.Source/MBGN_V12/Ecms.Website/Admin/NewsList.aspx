<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="Ecms.Website.Admin.NewsList" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
Quản trị nội dung
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
               
                    <table >
                        <tr>
                            <td style="width:150px;">
                                Thông tin tìm kiếm:
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Từ ngày</td>
                            <td>
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="datepicker"></asp:TextBox>
                                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtDateFrom" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../Content/Images/icons/Calender-icon.png">
                </obout:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Đến ngày</td>
                            <td>
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="datepicker"></asp:TextBox>
                                <obout:Calendar ID="cldToDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtDateTo" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../Content/Images/icons/Calender-icon.png">
                </obout:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Loại bài viết</td>
                            <td>
                                <asp:DropDownList ID="ddType" runat="server"
                                    CssClass="Cbo">
                                    <asp:ListItem Value="-1">-- Tất cả --</asp:ListItem>
                                    <asp:ListItem Value="0">Các dịch vụ</asp:ListItem>
                                    <asp:ListItem Value="1">Sale nóng</asp:ListItem>
                                    <asp:ListItem Value="2">Feedback</asp:ListItem>
                                    <asp:ListItem Value="3">Bài viết</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tiêu đề</td>
                            <td>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="Textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="btnLine">
                         <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"
                                    CssClass="Button" />
                                &nbsp;<asp:Button ID="btnAdd" runat="server" Text="Thêm mới" OnClick="btnAdd_Click"
                                    CssClass="Button" />
                    </div>
                    <br />
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
                                <asp:BoundField DataField="NewsId" HeaderText="NewsId" Visible="False" />
                                <asp:BoundField DataField="gType" HeaderText="Loại bài viết" />
                                <asp:BoundField DataField="WebsiteName" HeaderText="Tên website" />
                                <asp:BoundField DataField="Title" HeaderText="Tiêu đề">
                                    <ControlStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="gPublished" HeaderText="Hiển thị" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Ngày tạo"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("NewsId") %>'
                                            OnCommand="lbtEdit_Click" CommandName="Edit">Sửa</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="50" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("NewsId") %>'
                                            CommandName="Delete" OnCommand="lbtDelete_Click" Visible = '<%# Eval("AllowDelete")==null? true : Convert.ToBoolean(Eval("AllowDelete")) %>'
                                            OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa nội dung này không?')) return false;">Xóa</asp:LinkButton>
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
</asp:Content>
