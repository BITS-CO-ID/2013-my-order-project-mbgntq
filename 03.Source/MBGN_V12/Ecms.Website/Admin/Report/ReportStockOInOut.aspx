<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportStockOInOut.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportStockOInOut" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo Tổng hợp nhập xuất tồn
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
                Mã sản phẩm
            </td>
            <td>
                <asp:TextBox ID="txtProductCode" runat="server" CssClass="textbox"></asp:TextBox>
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
                <asp:TemplateField HeaderText="Mã sản phẩn<br/>(1)">
                    <ItemTemplate>
                        <%#Eval("ProductCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên sản phẩm<br/>(2)">
                    <ItemTemplate>
                        <%#Eval("ProductName") %>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="SL đầu kỳ<br/>(3)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("BalanceOpenQuantity").ToString()).ToString("N0") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBalanceOpenQuantity.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Gía trị đầu kỳ<br/>(4)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("BalanceOpenPrice").ToString()).ToString("N0")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBalanceOpenPrice.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SL Nhập<br/>(5)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("StockInQuantity").ToString()).ToString("N0")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dStockInQuantity.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá trị Nhập<br/>(6)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("StockInPrice").ToString()).ToString("N0") %>
                    </ItemTemplate>
                     <FooterTemplate>
                        <strong> <%=dStockInPrice.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SL Xuất<br/>(7)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("StockOutQuantity").ToString()).ToString("N0")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dStockOutQuantity.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá trị xuất<br/>(8)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("StockOutPrice").ToString()).ToString("N0")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dStockOutPrice.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SL Tồn<br/>(9)=(3)+(5)-(7)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("BalanceQuantity").ToString()).ToString("N0") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBalanceQuantity.ToString("N0")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá trị tồn<br/>(10)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("BalancePrice").ToString()).ToString("N0") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong> <%=dBalancePrice.ToString("N0") %></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"/>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
