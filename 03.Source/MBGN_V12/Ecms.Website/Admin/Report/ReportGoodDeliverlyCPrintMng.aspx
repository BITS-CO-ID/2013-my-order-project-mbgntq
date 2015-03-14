<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportGoodDeliverlyCPrintMng.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportGoodDeliverlyCPrintMng" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý xác nhận In phiếu giao nhận
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width:700px;" >
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
            <td>
                Mã khách hàng
            </td>
            <td>
                 <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"></asp:TextBox>
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
            <td>
                Người giao hàng
            </td>
            <td>
                <asp:TextBox ID="txtDeliverFullName" runat="server" CssClass="textbox"></asp:TextBox>
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
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnRowDeleting="gridMain_OnRowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày xác nhận">
                    <ItemTemplate>
                        <%# Eval("CreatedDate") == null ? "" : Convert.ToDateTime(Eval("CreatedDate")).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH">
                    <ItemTemplate>
                        <%#Eval("CustomerCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên KH">
                    <ItemTemplate>
                        <%#Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Điện thoại KH">
                    <ItemTemplate>
                        <%#Eval("Mobile") %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Người giao hàng">
                    <ItemTemplate>
                        <%#Eval("DeliverlyFullName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Chức vụ">
                    <ItemTemplate>
                        <%#Eval("DeliverlyPosition")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Điện thoại">
                    <ItemTemplate>
                        <%#Eval("DeliverlyMobile")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Ghi chú">
                    <ItemTemplate>
                        <%#Eval("Remark")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton Text='Hủy xác nhận' ID="lbtnDelete" CommandArgument='<%# Eval("RptDeliverlyId") %>'
                        CommandName="Delete" runat="server" OnCommand="lbtnDelete_Click" OnClientClick="return confirm('Bạn có chắc chắn muốn Hủy xác nhận đã IN phiếu giao nhận này?');" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"/>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton Text='Xem chi tiết' ID="lbtnDetail" CommandArgument='<%# Eval("RptDeliverlyId") %>'
                        CommandName="Detail" runat="server" OnCommand="lbtnDetail_Click" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"/>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
