<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="DistributionPaymentManage.aspx.cs" Inherits="Ecms.Website.Admin.DistributionPaymentManage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý thông tin khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <table >
                <tr>
                    <td>
                        Thông tin tìm kiếm:
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Mã khách hàng:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="Textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tên khách hàng:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="Textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Điện thoại:&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="Textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="Textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"
                    CssClass="Button" />
            </div>
            <div>
                <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                    Width="100%" AllowPaging="True" PageSize="15" OnPageIndexChanging="grdD_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                       <%-- <asp:BoundField DataField="CustomerTypeName" HeaderText="Nhóm khách hàng">
                            <ControlStyle Width="50px" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="CustomerId" HeaderText="CustomerId" Visible="False" />
                        <asp:BoundField DataField="CustomerCode" HeaderText="Mã khách hàng" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Tên khách hàng"></asp:BoundField>
                        <asp:BoundField DataField="EmployeeName" HeaderText="Người bán hàng"></asp:BoundField>
                        <asp:BoundField DataField="Mobile" HeaderText="Điện thoại"></asp:BoundField>
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Số dư thực tế">
                            <ItemTemplate>
                                <%# Eval("Balance") != null ? Convert.ToDouble(Eval("Balance").ToString()).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Số dư đóng băng">
                            <ItemTemplate>
                                <%# Eval("Balance") != null ? Convert.ToDouble(Eval("Balance").ToString()).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Số dư khả dụng">
                            <ItemTemplate>
                                <%# Eval("Balance") != null ? Convert.ToDouble(Eval("Balance").ToString()).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDistribution" runat="server" CommandArgument='<%#Eval("CustomerId") %>'
                                    OnCommand="lbtDistribution_Click" CommandName="Distribution">Phân bổ thanh toán</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="120" HorizontalAlign="Center" />
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
