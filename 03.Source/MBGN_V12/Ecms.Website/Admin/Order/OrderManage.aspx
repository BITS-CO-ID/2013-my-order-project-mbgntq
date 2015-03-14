<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderManage.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderManage" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Theo dõi Đơn hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width:750px" >
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
            <td class="tdFirst">
                <%--Tên đăng nhập--%>
            </td>
            <td>
                <%--<asp:TextBox runat="server" ID="txtUserCode" CssClass="textbox" />--%>
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
                <%--Số hiệu khách hàng--%>
            </td>
            <td>
                <asp:TextBox ID="txtCustomerNoDelivery" CssClass="textbox" runat="server" Visible="false"></asp:TextBox>
            </td>
           
        </tr>
        <tr>
            <td>
                Mã đơn hàng
            </td>
            <td>
                <asp:TextBox ID="txtOrderCode" CssClass="textbox" runat="server"></asp:TextBox>
            </td>            
             <td>
                Loại đơn hàng
            </td>
            <td>
                <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <asp:ListItem Value="1">BG - Báo giá</asp:ListItem>
                    <asp:ListItem Value="2">MH - Đặt hàng mua hộ</asp:ListItem>
                    <asp:ListItem Value="3">VC - Gửi hàng vận chuyển</asp:ListItem>
                    <asp:ListItem Value="4">CS - Đặt hàng có sẵn</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Mã khách hàng
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCustomerCode" CssClass="textbox" />
            </td>            
            <td>
                Tình trạng</td>
            <td>
                <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <asp:ListItem Value="1">Báo giá chưa trả lời</asp:ListItem>
                    <asp:ListItem Value="2">Báo giá đã trả lời</asp:ListItem>
                    <asp:ListItem Value="3">ĐH chưa trả lời</asp:ListItem>
                    <asp:ListItem Value="4">ĐH đã xác nhận</asp:ListItem>
                    <asp:ListItem Value="5">Hủy</asp:ListItem>
                    <asp:ListItem Value="6">Hoàn thành</asp:ListItem>
                    <asp:ListItem Value="7">Đã giao hàng</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>            
            <td style="display:none;">
                &nbsp;BillNo
            </td>
            <td style="display:none;">
                <asp:TextBox ID="txtTrackingNumber" CssClass="textbox" runat="server"></asp:TextBox>
            </td>
            <td style="display:none;">
                Order Number
            </td>
            <td style="display:none;">
                <asp:TextBox ID="txtOrderNumber" CssClass="textbox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="button" OnClick="btnSearch_Click" />
        <asp:Button ID="btnCreate" runat="server" Text="Lập đơn hàng vận chuyển" CssClass="button" OnClick="btnCreate_Click" />
        <asp:Button ID="btnOrderByLink" runat="server" Text="Lập đơn hàng MH" CssClass="button" OnClick="btnOrderByLink_Click" />
    </div>
    <div class="data">
        <asp:GridView ID="gridMain" runat="server" CssClass="gridview" OnPageIndexChanging="gridMain_PageIndexChanging"
            OnRowCommand="gridMain_RowCommand" AutoGenerateColumns="False" Visible="False" OnRowDataBound="gridMain_RowDataBound"
            AllowPaging="True" PageSize="15">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại ĐH">
                    <ItemTemplate>
                        <asp:HiddenField id ="hdnOrderTypeId" runat="server" Value='<%#Eval("OrderTypeId")%>'/>
                        <%# Eval("OrderTypeName")%>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("OrderNo")%>' ID="lbtnDetail" CommandArgument='<%# Eval("OrderId") + "|" + Eval("OrderTypeId") %>'
                            CommandName="OrderDetail" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thời gian tạo ĐH">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("CreatedDate").ToString()).ToString("dd/MM/yyyy HH:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH">
                    <ItemTemplate>
                        <asp:HiddenField id ="hdnUserCode" runat="server" Value='<%#Eval("UserCode")%>'/>
                        <%# Eval("CustomerCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên KH">
                    <ItemTemplate>
                        <%# Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Người bán hàng">
                    <ItemTemplate>
                        <%# Eval("EmployeeName") %>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Ngày xác<br/>nhận ĐH">
                    <ItemTemplate>
                        <%# Eval("ConfirmDate") == null ? "" : Convert.ToDateTime(Eval("ConfirmDate").ToString()).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền">
                    <ItemTemplate>
                        <%# (Eval("OrderStatus").ToString().Equals("2") || Eval("OrderStatus").ToString().Equals("4") || Eval("OrderStatus").ToString().Equals("6") || Eval("OrderStatus").ToString().Equals("7")) ? (Convert.ToDouble(Eval("SumAmount").ToString()) + Convert.ToDouble(Eval("AmountFeeDelay").ToString())).ToString("N0") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="80" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tình trạng<br/>ĐH">
                    <ItemTemplate>
                        <%# Eval("OrderStatusText")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nhân viên</br>Bán hàng">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%#Eval("EmployeeCode")==null?Eval("CreateUser"):Eval("EmployeeCode")%>' ID="lbtnEmployee" CommandArgument='<%# Eval("OrderId") +"|"+ (Eval("EmployeeCode")==null?Eval("CreateUser"):Eval("EmployeeCode"))%>'
                            CommandName="Employee" runat="server" OnCommand="lbtEmployee_Click" />
                    </ItemTemplate>                    
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Xóa ĐH">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnDeleteOrder" runat="server" CommandArgument='<%#Eval("OrderId") +"|"+ Eval("OrderStatus")%>' 
                            CommandName="DeleteOrder" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa đơn hàng này?')">Xóa ĐH</asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="60" HorizontalAlign="Center" />
                </asp:TemplateField>                
                <%--
                <asp:TemplateField HeaderText="Thiết lập<br/>phí trả chậm">
                    <ItemTemplate>
                        <asp:LinkButton Text='Thiết lập' ID="lbtnFeeDelay" 
                            CommandArgument='<%# Eval("OrderId") +"|"+ Eval("CalFeeDelay")+"|"+ Eval("DayAllowedDelay")+"|"+ Eval("FeeDelay") +"|"+ Eval("AmountFeeDelay")+"|"+ Eval("OrderStatus")+"|"+ Eval("IsCalFeeDelay")%>'
                            Visible='<%# Eval("OrderTypeId").ToString().Equals("2") ? true: false %>'
                            CommandName="FeeDelay" runat="server" OnCommand="lbtFeeDelay_Click" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        <br />
    </div>
</asp:Content>
