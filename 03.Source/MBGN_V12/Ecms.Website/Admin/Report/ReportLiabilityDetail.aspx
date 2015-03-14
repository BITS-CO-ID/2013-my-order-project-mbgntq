<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportLiabilityDetail.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportLiabilityDetail" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo chi tiết tài khoản khách hàng
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
                <asp:Label ID="lblSDDK" runat="server" Visible="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
    </div>
    <div>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15"
            ShowFooter="True">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Mã giao dịch">
                    <ItemTemplate>
                        <%#Eval("InvoiceModel.InvoiceCode")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong>Công nợ:</strong>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã nghiệp vụ">
                    <ItemTemplate>
                        <%#Eval("InvoiceModel.BusinessCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nghiệp vụ thanh toán">
                    <ItemTemplate>
                        <%#Eval("InvoiceModel.BusinessName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày giao dịch">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("InvoiceModel.InvoiceDate").ToString()).ToString("dd/MM/yyyy HH:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TK thanh toán">
                    <ItemTemplate>
                        <%#Eval("InvoiceModel.FromAccount")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số tiền">
                    <ItemTemplate>
                        <%# Eval("Amount") != null ? Convert.ToDouble(Eval("Amount").ToString()).ToString("N0") : "0.00"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong><%= Totals[0] %></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ghi chú">
                    <ItemTemplate>
                        <%#Eval("InvoiceModel.Remark")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
