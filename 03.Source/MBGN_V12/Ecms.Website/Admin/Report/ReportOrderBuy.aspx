<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportOrderBuy.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportOrderBuy" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo tổng hợp đơn hàng mua hộ
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
                Mã đơn hàng
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="textbox"></asp:TextBox>
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
                            <td>
                                Trạng thái
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="cbo">
                                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                                    <%--<asp:ListItem Value="1">Báo giá chưa trả lời</asp:ListItem>
                                    <asp:ListItem Value="2">Báo giá đã trả lời</asp:ListItem>--%>
                                    <asp:ListItem Value="3">ĐH chưa trả lời</asp:ListItem>
                                    <asp:ListItem Value="4">ĐH đã xác nhận</asp:ListItem>
                                    <asp:ListItem Value="5">Hủy</asp:ListItem>
                                    <asp:ListItem Value="6">Hoàn thành</asp:ListItem>
                                    <asp:ListItem Value="7">Đã giao hàng</asp:ListItem>
                                </asp:DropDownList>
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
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                     <ItemTemplate>
                        <%#Eval("OrderNo") %>
                    </ItemTemplate>
                    <FooterTemplate>
                    <strong>
                        Tổng:
                    </strong>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BillNo">
                    <ItemTemplate>
                        <%#Eval("TrackingNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày đặt hàng">
                    <ItemTemplate>
                        <%# Eval("OrderDate") == null ? "" : Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy")%>
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
                <asp:TemplateField HeaderText="Địa chỉ nhận hàng">
                    <ItemTemplate>
                        <%#Eval("DeliveryAddress")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền">
                    <ItemTemplate>
                        <%# (Eval("OrderStatus").ToString().Equals("2") || Eval("OrderStatus").ToString().Equals("4") || Eval("OrderStatus").ToString().Equals("6") || Eval("OrderStatus").ToString().Equals("7")) ? (Convert.ToDouble(Eval("SumAmount").ToString()) + Convert.ToDouble(Eval("AmountFeeDelay").ToString())).ToString("N0") : ""%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                    <strong>
                        <%#rptSumAmout.ToString("N0")%>
                    </strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Đã thanh toán">
                    <ItemTemplate>
                        <%#Convert.ToDouble((Eval("PayAmount") ?? 0)).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                    <strong>
                        <%#rptPayAmount.ToString("N0")%>
                    </strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thanh toán chậm">
                    <ItemTemplate>
                        <%#Convert.ToDouble((Eval("AmountFeeDelay") ?? 0)).ToString("N0")%>
                    </ItemTemplate>
                    <FooterTemplate>
                    <strong>
                        <%#rptAmountFeeDelay.ToString("N0")%>
                    </strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                   
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Công nợ đơn hàng">
                    <ItemTemplate>
                        <%#Convert.ToDouble((Eval("RemainAmount") ?? 0)).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                    <strong>
                        <%#rptRemainAmount.ToString("N0")%>
                    </strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%#Eval("StatusText")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ghi chú">
                    <ItemTemplate>
                        <%#Eval("Remark") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
