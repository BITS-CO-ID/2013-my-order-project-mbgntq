<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="PaymentManage.aspx.cs" Inherits="Ecms.Website.Admin.Order.PaymentManage" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý thanh toán
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width: 700px;">
        <tr>
            <td>
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
                Loại thanh toán
            </td>
            <td>
                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <asp:ListItem Value="201">Khách hàng gửi xác nhận thanh toán</asp:ListItem>
                    <asp:ListItem Value="202">QC247 Hoàn tiền cho khách hàng</asp:ListItem>
                    <asp:ListItem Value="203">QC247 mua hàng từ đối tác</asp:ListItem>
                    <asp:ListItem Value="204">QC247 nạp tiền vào tài khoản</asp:ListItem>
                    <asp:ListItem Value="205">Khách hàng nạp tiền vào tài khoản</asp:ListItem>
                    <asp:ListItem Value="208">Phân bổ thanh toán cho đơn hàng</asp:ListItem>
                </asp:DropDownList>
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
                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" DatePickerSynchronize="true">
                </obout:Calendar>
            </td>
            <td>
                Tình trạng
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <asp:ListItem Value="1">Mới gửi thanh toán</asp:ListItem>
                    <asp:ListItem Value="2">Đã khớp thanh toán</asp:ListItem>
                    <asp:ListItem Value="3">Không khớp thanh toán</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Mã TT/Mã ĐH
            </td>
            <td>
                <asp:TextBox ID="txtInvoiceCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
            <td>
                Mã khách hàng
            </td>
            <td>
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>       
        <tr>
            <td colspan="4">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Tìm kiếm" 
            onclick="btnSearch_Click" />
        &nbsp;<asp:Button ID="btnCreateInvoice" runat="server" CssClass="button" 
            Text="Lập hóa đơn thanh toán" onclick="btnCreateInvoice_Click" />
        <br />
    </div>
    <div class="data">
        <asp:GridView ID="gridMain" runat="server" CssClass="gridview" OnPageIndexChanging="gridMain_PageIndexChanging"
            OnRowCommand="gridMain_RowCommand" AutoGenerateColumns="False" Visible="False"
            AllowPaging="True" PageSize="15">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã thanh toán">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("InvoiceCode")%>' CommandArgument='<%# Eval("InvoiceId")%>' CommandName="ConfirmPayment" runat="server" />                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                    <ItemTemplate>
                        <%# Eval("OrderNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã TT tham chiếu">
                    <ItemTemplate>
                        <%# Eval("InvoiceRefCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại TT">
                    <ItemTemplate>
                        <%# Eval("BusinessName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH">
                    <ItemTemplate>
                        <%# Eval("CustomerCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên KH">
                    <ItemTemplate>
                        <%# Eval("CustomerName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày TT">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("InvoiceDate").ToString()).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngân hàng">
                    <ItemTemplate>
                        <%# Eval("BankName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Nội dung">
                    <ItemTemplate>
                        <span title='<%# Eval("Remark") %>'><%# Eval("Remark") != null ? Eval("Remark").ToString() : "" %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("SumAmount").ToString()).ToString("N2")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%# Eval("StatusText")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
</asp:Content>
