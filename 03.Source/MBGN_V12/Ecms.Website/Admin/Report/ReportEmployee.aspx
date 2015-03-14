<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportEmployee.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportEmployee" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo tổng hợp tài khoản nhân viên
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
                <%--<asp:DropDownList ID="ddlFromHour" runat="server">
                </asp:DropDownList>
                &nbsp;g&nbsp;
                <asp:DropDownList ID="ddlFromMinute" runat="server">
                </asp:DropDownList>
                &nbsp;p--%>
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
              <%--  <asp:DropDownList ID="ddlToHour" runat="server">
                </asp:DropDownList>
                &nbsp;g&nbsp;
                <asp:DropDownList ID="ddlToMinute" runat="server">
                </asp:DropDownList>
                &nbsp;p--%>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Mã nhân viên
            </td>
            <td>
                <asp:TextBox ID="txtEmpCode" runat="server" CssClass="textbox"></asp:TextBox>
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
    </div>
    <div>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnRowCommand="gridMain_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã nhân viên">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("EmployeeCode")%>' ID="lbtnDetail" CommandArgument='<%# Eval("EmployeeCode") %>'
                            CommandName="RptDetail" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên nhân viên">
                    <ItemTemplate>
                        <%#Eval("EmployeeName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dầu kỳ">
                    <ItemTemplate>
                        <%# Eval("BeforeBalance") != null ? Convert.ToDouble(Eval("BeforeBalance").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phát sinh tăng">
                    <ItemTemplate>
                        <%# Eval("TotalCharge") != null ? Convert.ToDouble(Eval("TotalCharge").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phát sinh giảm">
                    <ItemTemplate>
                        <%# Eval("TotalConfirmedPayment") != null ? Convert.ToDouble(Eval("TotalConfirmedPayment").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dư cuối kỳ">
                    <ItemTemplate>
                        <%# Eval("AfterBalance") != null ? Convert.ToDouble(Eval("AfterBalance").ToString()).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
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
