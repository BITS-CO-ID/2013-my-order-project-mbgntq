<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ImportStockManage.aspx.cs" Inherits="Ecms.Website.Admin.ImportStockManage" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Danh sách nhập kho
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width: 50%;"  cellspacing="0">
        <tr>
            <td>
                Từ ngày:
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker" />
                <obout:Calendar id="cldFromDate" runat="server" columns="1" culturename="vi-VN" datepickerimagepath="~/Content/Images/icons/Calender-icon.png"
                    datepickerimagetooltip="Chọn ngày" datepickermode="true" multiselecteddates="True"
                    textboxid="txtFromDate" yearmonthformat="dd/MM/yyyy">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td>
                Đến ngày
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtToDate" CssClass="datepicker" />
                <obout:Calendar ID="cldToDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtToDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="~/Content/Images/icons/Calender-icon.png">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td>
                Mã phiếu
            </td>
            <td>
                <asp:TextBox ID="txtInOutCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="button" 
            onclick="btnSearch_Click" />
        &nbsp;<asp:Button ID="btnCreate" runat="server" Text="Nhập kho" 
            CssClass="button" onclick="btnCreate_Click" />
    </div>
    <div class="data">
        &nbsp;<asp:GridView ID="gridMain" runat="server" CssClass="gridview" 
            OnPageIndexChanging="gridMain_PageIndexChanging" 
            AutoGenerateColumns="False" Visible="False"
            AllowPaging="True" PageSize="15" onrowcommand="gridMain_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã phiếu nhập">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("StockOutNo")%>' CommandArgument='<%# Eval("StockInOutId") %>' CommandName="StockInOutDetail" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày tạo">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("CreatedDate").ToString()).ToString("dd/MM/yyyy hh:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày nhập">
                    <ItemTemplate>
                        <%# Eval("InOutDate") != null ? Convert.ToDateTime(Eval("InOutDate").ToString()).ToString("dd/MM/yyyy") : ""%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Người nhập">
                    <ItemTemplate>
                        <%# Eval("UserCode") %>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%# Eval("StatusText") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền (VND)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("SumAmount").ToString()).ToString("N0") %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right"/>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
</asp:Content>
