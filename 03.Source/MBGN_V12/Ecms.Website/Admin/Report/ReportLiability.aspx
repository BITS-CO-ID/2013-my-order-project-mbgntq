<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportLiability.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportLiability" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo công nợ khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width: 50%;" >
        <tr>
            <td style="width: 150px;">
                Từ ngày
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFromDate" CssClass="datepicker" />
                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtFromDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True" DatePickerSynchronize="true">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Đến ngày
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtToDate" CssClass="datepicker" />
                <obout:Calendar ID="cldToDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtToDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" DatePickerSynchronize="true">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Mã khách hàng
            </td>
            <td>
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"></asp:TextBox>
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
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview" ShowFooter="true"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnRowCommand="gridMain_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <strong>Tổng:</strong>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Người bán hàng<br/>(1)">
                    <ItemTemplate>
                        <%#Eval("EmployeeName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH<br/>(2)">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("CustomerCode")%>' ID="lbtnDetail" CommandArgument='<%# Eval("CustomerId") %>'
                            CommandName="RptDetail" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên Đăng nhập<br/>(4)">
                    <ItemTemplate>
                        <%#Eval("UserCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên KH<br/>(3)">
                    <ItemTemplate>
                        <%#Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Tồn đầu kỳ<br/>(5)">
                    <ItemTemplate>
                        <%# Eval("BeforeBalance") != null ? Convert.ToDouble(Eval("BeforeBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBeforeBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nộp trong kỳ<br/>(6)">
                    <ItemTemplate>
                        <%# Eval("TotalCharge") != null ? Convert.ToDouble(Eval("TotalCharge").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dTotalCharge.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng cuối kỳ<br/>(7)=(5)+(6)">
                    <ItemTemplate>
                        <%# Eval("AfterBalance") != null ? Convert.ToDouble(Eval("AfterBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dAfterBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số dư hiện tại<br/>(8)">
                    <ItemTemplate>
                        <%# Eval("Balance") != null ? Convert.ToDouble(Eval("Balance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                     <FooterTemplate>
                        <strong> <%=dBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số dư đóng băng<br/>(9)">
                    <ItemTemplate>
                        <%# Eval("BalanceFreeze") != null ? Convert.ToDouble(Eval("BalanceFreeze").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBalanceFreeze.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số dư khả dụng<br/>(10)=(8) - (9)">
                    <ItemTemplate>
                        <%# Eval("BalanceAvaiable") != null ? Convert.ToDouble(Eval("BalanceAvaiable").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBalanceAvaiable.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng CN ĐH<br/>(11)">
                    <ItemTemplate>
                        <%# Eval("RemainBalance") != null ? Convert.ToDouble(Eval("RemainBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dRemainBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CN phải thu<br/>(12)=(11) - (10)">
                    <ItemTemplate>
                        <%# Eval("RemainBalanceReceivable") != null ? Convert.ToDouble(Eval("RemainBalanceReceivable").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dRemainBalanceReceivable.ToString("N0") %></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
