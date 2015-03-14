<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportOrderDebit.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportOrderDebit" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo công nợ Đơn hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width: 700px;" >
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
                    <td>
                        NV bán hàng (<span style="color:Red;">*</span>)</td>
                   <td>
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
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
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnRowCommand="gridMain_RowCommand" ShowFooter="true">
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
                <asp:TemplateField HeaderText="Mã khách hàng<br>(1)">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("CustomerCode")%>' ID="lbtnDetail" CommandArgument='<%# Eval("CustomerId") %>'
                            CommandName="RptDetail" runat="server" />
                    </ItemTemplate>                    
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Tên đăng nhập<br>(2)">
                    <ItemTemplate>
                        <%#Eval("UserCode") %>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Tên khách hàng<br>(2)">
                    <ItemTemplate>
                        <%#Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="CN đầu kỳ<br>(3)">
                    <ItemTemplate>
                        <%# Eval("OpenBalance") != null ? Convert.ToDouble(Eval("OpenBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= dOpenBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phát sinh trong kỳ<br>(4)">
                    <ItemTemplate>
                        <%# Eval("IncreaseBalance") != null ? Convert.ToDouble(Eval("IncreaseBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= dIncreaseBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CN Cuối kỳ<br>(5)=(3)+(4)">
                    <ItemTemplate>
                        <%# Eval("AfterBalance") != null ? Convert.ToDouble(Eval("AfterBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= (dOpenBalance + dIncreaseBalance).ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Đã TT Cuối kỳ<br>(6)">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Convert.ToDouble(Eval("PaidPayBalance").ToString()).ToString("N0")%>' ID="lbtnPaidPay" CommandArgument='<%# Eval("CustomerId") %>' 
                        Enabled= '<%# Convert.ToDouble(Eval("PaidPayBalance").ToString())!=0?true:false %>' 
                        CommandName="RptPaidPay" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>                        
                        <strong><%= dPaidPayBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền<br/>vận chuyển cuối kỳ<br>(7)">
                    <ItemTemplate>
                        <%# Eval("SumFeeShip") != null ? Convert.ToDouble(Eval("SumFeeShip").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= dSumFeeShipBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Tổng tiền tính phí<br/>trả chậm cuối kỳ<br>(8)">
                    <ItemTemplate>
                        <%# Eval("AmountCalFeeDelay") != null ? Convert.ToDouble(Eval("AmountCalFeeDelay").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= dAmountCalFeeDelayBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phí trả chậm Cuối kỳ<br>(10)">
                    <ItemTemplate>
                        <%# Eval("AmountFeeDelay") != null ? Convert.ToDouble(Eval("AmountFeeDelay").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= dAmountFeeDelayBalance.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="CN Còn lại<br>(8)=(6)-(7)">
                    <ItemTemplate>
                        <%# Eval("RemainBalance") != null ? Convert.ToDouble(Eval("RemainBalance").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= (dRemainBalance).ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
