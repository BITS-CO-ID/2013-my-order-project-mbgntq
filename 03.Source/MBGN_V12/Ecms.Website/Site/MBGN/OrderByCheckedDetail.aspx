<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderByCheckedDetail.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderByCheckedDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <p class="p-note">Thông tin đơn hàng vận chuyển</p>
    <div>
    <asp:Panel runat="server" ID="pnlOrder">
        <div>
            <table class="tableForm" style="width: 70%;" align="left" cellpadding="5">
                <tr>
                    <td class="tdFirst">
                        <strong>Mã đơn hàng: </strong>
                    </td>
                    <td class="tdFirst">
                        <asp:Label ID="lblOrderNo" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                    <td>
                        <strong>Ngày đặt hàng: </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblOrderDate" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdFirst">
                        <strong>Tình trạng đơn hàng: </strong>
                    </td>
                    <td class="tdFirst">
                        <asp:Label ID="lblOrderStatus" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                    <td>
                        <strong>Ngày xác nhận</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblConfirmDate" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdFirst">
                        <strong>Ngày giao hàng: </strong>
                    </td>
                    <td class="tdFirst">
                        <asp:Label ID="lblDeliverlyDate" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                    <td>
                       <%--<strong>Ngày đến Mỹ</strong> --%>
                    </td>
                    <td>
                        <%--<asp:Label ID="lblDateToUsa" runat="server" ForeColor="#555555"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td class="tdFirst">
                        <strong>Ghi chú: </strong>
                    </td>
                    <td class="tdFirst" colspan="3">
                        <asp:Label ID="lblRemark" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        
        <div style="width: 40%; float: right;">
        <table style="float: right;" class="gridview" cellspacing="0">
            <tr>
                <th>
                    Tổng tiền đơn hàng
                </th>
                <th>
                    Thanh toán
                </th>
                <th>
                    Còn lại
                </th>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblTotalMoneyOrder" runat="server"></asp:Label>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblTotalAmountNormal" runat="server"></asp:Label>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblTotalRemain" runat="server" Font-Bold="true"></asp:Label>
                </td>
            </tr>
        </table>
        </div>
        
    </asp:Panel>
    </div>
    <div style="float:left;">
    <asp:Panel runat="server" ID="pnlOrderDetail">
        <p class="p-note">
            Chi tiết đơn hàng</p>
        <asp:GridView ID="gridCart" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            Width="100%" OnDataBound="gridCart_DataBound" OnRowCommand="gridCart_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã MH">
                    <ItemTemplate>
                        <%# Eval("DetailCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BillNo">
                    <ItemTemplate>
                        <asp:Literal Text='<%# Eval("TrackingNo")%>' ID="litTrackingNumber" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <%--<asp:TemplateField HeaderText="Nhóm sản phẩm">
                    <ItemTemplate>
                        <%# Eval("CategoryName")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Left" />
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="Tên sản phẩm">
                    <ItemTemplate>
                        <%# Eval("ProductName")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Left" />
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                        <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                
                <asp:TemplateField HeaderText="Cân nặng (kg)">
                    <ItemTemplate>
                        <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="60" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phí ship">
                    <ItemTemplate>
                        <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N2") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phụ thu">
                    <ItemTemplate>
                        <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="60" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá khai báo">
                    <ItemTemplate>
                        <%# Eval("DeclarePrice") != null ? Convert.ToDouble(Eval("DeclarePrice").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="80" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bảo hiểm(%)">
                    <ItemTemplate>
                        <%# Eval("InsuaranceConfigId") != null ? Convert.ToDouble(Eval("InsuaranceConfigValue").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="80" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày về VN">
                    <ItemTemplate>
                        <%# Eval("DeliveryVNDate") != null ? Convert.ToDateTime(Eval("DeliveryVNDate").ToString()).ToString("dd/MM/yyyy") : ""%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="80" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Thành tiền (VND)">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("TotalMoney") ?? 0).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%# Eval("DetailStatus") != null ? Convert.ToInt32(Eval("DetailStatus")) != 0 ? Eval("DetailStatusText") : "" : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton Text="Cập nhật" CommandArgument='<%# Eval("OrderId")+ "|" + Eval("TrackingNo")+ "|" + Eval("OrderDetailId") %>'
                            CommandName="updateTrackingOrder" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table class="tableForm" style="width: 25%; margin-top: 20px;" align="right" cellpadding="5">
            <tr>
                <td style="text-align: right; padding-top: 10px;">
                    <asp:Button ID="btnOrder" runat="server" CssClass="button" OnClick="btnOrder_Click"
                        OnClientClick="return confirm('Quý khách có chắc chắn muốn thanh toán tiếp?');"
                        Text="Thanh toán" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click"
                        Text="Quay lại" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </div>
</asp:Content>
