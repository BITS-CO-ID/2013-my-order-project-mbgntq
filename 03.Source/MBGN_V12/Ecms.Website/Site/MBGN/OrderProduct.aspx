<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderProduct.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderProduct" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table class="tableForm" style="width: 100%;">
        <tr>
            <td class="td-bg" colspan="4">
                QUẢN LÝ ĐƠN HÀNG
            </td>
        </tr>
        <tr>
            <td style="width:50px;">
                Từ ngày:
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" CssClass="txtDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                 Đến ngày:
            </td>
            <td>
                <asp:TextBox ID="txtToDate" CssClass="txtDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Mã đơn hàng:
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Tình trạng:
            </td>
            <td>
                <asp:DropDownList ID="ddlOrderStatus" runat="server">
                    <asp:ListItem Text="-- Tất cả --" Value="0" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="BG Chưa trả lời" Value="1"></asp:ListItem>
                    <asp:ListItem Text="BG Đã trả lời" Value="2"></asp:ListItem>
                    <asp:ListItem Text="ĐH Chưa trả lời" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Đã xác nhận ĐH" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Hủy" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Hoàn thành" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Đã giao hàng" Value="7"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <p class="p-note"></p>
    <div>
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Tìm kiếm" OnClick="btnSearch_Click" />
        <asp:Button ID="btnSendPayment" runat="server" CssClass="button" Text="Gửi Y/C thanh toán" OnClick="btnSendPayment_Click" />
        <asp:Button ID="btnExportExcel" runat="server" CssClass="button" Text="Xuất file excel" OnClick="btnExportExcel_Click" />
        <br />
    </div>
    <asp:Panel runat="server" ID="pnOrderEmpty">
        <div style="text-align: center; color: Red;">
            Không có đơn hàng nào theo điều kiện tìm kiếm!
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnCartNotEmty">
        <div style="float:left; margin-top:10px; margin-bottom:10px; width:100%;">
            <asp:GridView ID="gridCart" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                OnRowCommand="gridCart_RowCommand" AllowPaging="True" PageSize="15" OnPageIndexChanging="gridCart_PageIndexChanging"
                OnRowDataBound="gridMain_RowDataBound" ShowFooter="true" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <%# Container.DataItemIndex+1 %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>Tổng:</strong>
                        </FooterTemplate>
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Loại ĐH<br/>(1)">
                        <ItemTemplate>
                            <%# Eval("OrderTypeName")%>
                        </ItemTemplate>
                        <ItemStyle Width="50" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mã đơn hàng<br/>(2)">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdOrderId" runat="server" Value='<%# Eval("OrderId") %>' />
                            <asp:HiddenField ID="hdOrderTypeId" runat="server" Value='<%# Eval("OrderTypeId") %>' />
                            <asp:LinkButton Text='<%# Eval("OrderNo")%>' ID="lbtnDetail" CommandArgument='<%# Eval("OrderId") %>'
                                CommandName="OrderDetail" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="60" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ngày tạo ĐH<br/>(3)">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("CreatedDate").ToString()).ToString("dd/MM/yyyy")%>
                        </ItemTemplate>
                        <ItemStyle Width="80" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ngày xác<br/>nhận ĐH<br/>(4)">
                        <ItemTemplate>
                            <%# Eval("ConfirmDate")==null?"": Convert.ToDateTime(Eval("ConfirmDate").ToString()).ToString("dd/MM/yyyy")%>
                        </ItemTemplate>
                        <ItemStyle Width="80" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tình trạng<br/>(5)">
                        <ItemTemplate>
                            <%# Eval("OrderStatusText")%>
                        </ItemTemplate>
                        <ItemStyle Width="150" HorizontalAlign="Left" />
                    </asp:TemplateField>                
                    <asp:TemplateField HeaderText="Tổng tiền ĐH<br/>(6)">
                        <ItemTemplate>
                            <%# (Eval("OrderStatus").ToString().Equals("4") || Eval("OrderStatus").ToString().Equals("6") || Eval("OrderStatus").ToString().Equals("7")) ? (Convert.ToDouble(Eval("SumAmount").ToString())).ToString("N0") : ""%>
                            <%--<%# Eval("SumAmount") == null ? "" : Convert.ToDouble(Eval("SumAmount").ToString()).ToString("N0")%>--%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>
                                <%= dTotalAmount.ToString("N0")%>
                            </strong>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Đã thanh toán<br/>(7)">
                        <ItemTemplate>
                            <%# Convert.ToDouble(Eval("TotalPayAmountNormal").ToString()).ToString("N0")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>
                                <%= dPayAmount.ToString("N0")%>
                            </strong>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle Width="80" HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Tiền vận<br/>chuyển<br/>(8)">
                        <ItemTemplate>
                            <%# Eval("SumFeeShip") == null?"": Convert.ToDouble(Eval("SumFeeShip").ToString()).ToString("N0")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>
                                <%= dSumFeeShip.ToString("N0")%>
                            </strong>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle Width="80" HorizontalAlign="Right"/>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="Tổng còn lại<br />tính phí quá hạn<br/>(9)=(6)-(8)">
                        <ItemTemplate>
                            <%# Eval("AmountCalFeeDelay") == null ? "" : Convert.ToDouble(Eval("AmountCalFeeDelay").ToString()).ToString("N0")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>
                                <%= dSumAmountCalFeeDelay.ToString("N0")%>
                            </strong>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle Width="130" HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phí trả<br/>quá hạn<br/>(10)">
                        <ItemTemplate>
                            <%# Eval("AmountFeeDelay")==null?"": Convert.ToDouble(Eval("AmountFeeDelay").ToString()).ToString("N0")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>
                                <%= dSumAmountFeeDelay.ToString("N0")%>
                            </strong>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle Width="60" HorizontalAlign="Right" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Công nợ ĐH<br/>(8)=(6)-(7)">
                        <ItemTemplate>
                            <%# (Eval("OrderStatus").ToString().Equals("4") || Eval("OrderStatus").ToString().Equals("6") || Eval("OrderStatus").ToString().Equals("7")) ? (Convert.ToDouble(Eval("RemainAmount").ToString())).ToString("N0") : ""%>
                           <%-- <%# Eval("RemainAmount") == null ? "" : Convert.ToDouble(Eval("RemainAmount").ToString()).ToString("N0")%>--%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <strong>
                                <%= dRemainAmount.ToString("N0")%>
                            </strong>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle Width="80" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
    <br />
</asp:Content>
