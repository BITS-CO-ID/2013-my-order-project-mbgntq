<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportEmployeeDetail.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportEmployeeDetail" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
     Báo cáo chi tiết tài khoản nhân viên
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width: 50%;" >
        <tr>
            <td colspan="2">
                <asp:Label ID="lblDetail" runat="server" Visible="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblDateDetail" runat="server" Visible="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Tìm kiếm" OnClick="btnSearch_Click" />
        <asp:Button ID="btnExcel" runat="server" CssClass="button" Text="Xuất file Excel" OnClick="btnExcel_Click" />
    </div>
    <div>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview" ShowFooter="True"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnRowCommand="gridMain_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã khách hàng">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("CustomerCode")%>' ID="lbtnDetail" CommandArgument='<%# Eval("CustomerId") %>'
                            CommandName="RptDetail" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong>Công nợ:</strong>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên khách hàng">
                    <ItemTemplate>
                        <%#Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dư đầu kỳ">
                    <ItemTemplate>
                        <%# Eval("BeforeBalance") != null ? Convert.ToDouble(Eval("BeforeBalance").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[0] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phát sinh tăng">
                    <ItemTemplate>
                        <%# Eval("TotalCharge") != null ? Convert.ToDouble(Eval("TotalCharge").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[1] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phát sinh giảm">
                    <ItemTemplate>
                        <%# Eval("TotalConfirmedPayment") != null ? Convert.ToDouble(Eval("TotalConfirmedPayment").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[2] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dư cuối kỳ">
                    <ItemTemplate>
                        <%# Eval("AfterBalance") != null ? Convert.ToDouble(Eval("AfterBalance").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[3] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Số dư thực tế">
                    <ItemTemplate>
                        <%# Eval("FactBalance") != null ? Convert.ToDouble(Eval("FactBalance").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
