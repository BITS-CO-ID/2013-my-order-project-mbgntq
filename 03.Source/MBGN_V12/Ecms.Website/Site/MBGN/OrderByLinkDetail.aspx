<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderByLinkDetail.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderByLinkDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <p class="p-note">Thông tin đơn hàng</p>
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
                        <strong>Ngày đặt hàng:</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblOrderDate" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Ngày xác nhận: </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblConfirmDate" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                    <td>
                        <strong>Ngày giao hàng: </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryDate" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Tình trạng đơn hàng: </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblOrderStatus" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Ghi chú:</strong>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblRemark" runat="server" ForeColor="#555555"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    </div>
    <div style="float: right">
    <table style="width: 80%; float:right;" class="gridview" cellspacing="0">
        <tr>
            <th>
                Tổng tiền đơn hàng<br/>(1)
            </th>
            <th>
                Đã thanh toán<br />(2)
            </th>
            <th>
                Tiền vận chuyển<br />(3)
            </th>
            <%--<th>
                Tổng còn lại<br />tính phí trả chậm<br/>(4)=(1)-(3)
            </th>
            <th>
                Phí trả quá hạn<br />(5)
            </th>--%>
            <th>
                Công nợ ĐH<br/>(4)=(1)-(2)
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
                <asp:Label ID="lblSumFeeShip" runat="server"></asp:Label>
            </td>
            <%--<td style="text-align: right;">
                <asp:Label ID="lblAmountCalFeeDelay" runat="server"></asp:Label>
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblAmountFeeDelay" runat="server"></asp:Label>
            </td>--%>
            <td style="text-align: right;">
                <asp:Label ID="lblTotalRemainAmount" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    <div style="float: left; width:100%">
    <p class="p-note">Chi tiết đơn hàng</p>
    <asp:Panel runat="server" ID="pnlOrderDetail">
        <div>
            <asp:GridView ID="gridCart" runat="server" AutoGenerateColumns="False" CssClass="gridview" Width="100%" AllowPaging="true" PageSize="15" OnPageIndexChanging="gridCartByLink_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Mã MH">
                        <ItemTemplate>
                            <%--<%# Eval("DetailCode") %>--%>
                            <div class="tag">
                                <a class="gridViewToolTip"><%# Eval("DetailCode")%></a>
                                <div id="tooltip" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Xuất xứ:</b>&nbsp;</td>
                                            <td><%# Eval("CountryName") %></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Chủng loại:</b>&nbsp;</td>
                                            <td><%# Eval("CategoryName") %></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Màu sắc:</b>&nbsp;</td>
                                            <td><%# Eval("Color") %></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Kích cỡ:</b>&nbsp;</td>
                                            <td><%# Eval("Size") %></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="60" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Website">
                    <ItemTemplate>
                        <%# Eval("WebsiteName")%>
                    </ItemTemplate>
                    <ItemStyle Width="120" />
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Shop">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdOrderDetailId" runat="server" Value='<%# Eval("OrderDetailId") %>' />
                            <%# Eval("Shop") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Hình ảnh ">
                        <ItemTemplate>
                            <a target="_blank" href='<%# Eval("ImageUrl") %>'>
                                <img src='<%# Eval("ImageUrl") %>' width="30" height="30" title="Ảnh sản phẩm" /></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Link">
                        <ItemTemplate>
                            <a href='<%# Eval("ProductLink") %>' title='<%# Eval("ProductLink") %>' target="_blank">
                                <%# Eval("ProductLink").ToString().Length < 15 ? Eval("ProductLink").ToString() : (Eval("ProductLink").ToString().Substring(0, 15) + "...")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cân nặng<br/>(kg)">
                        <ItemTemplate>
                            <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight") ?? 0).ToString("N2") : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="40" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Màu sắc">
                        <ItemTemplate>
                            <%# Eval("Color") %>
                        </ItemTemplate>
                        <ItemStyle Width="100" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Size">
                        <ItemTemplate>
                            <%# Eval("Size")%>
                        </ItemTemplate>
                        <ItemStyle Width="80" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Giá web">
                        <ItemTemplate>
                            <%# Eval("PriceWeb") != null ? Convert.ToDouble(Eval("PriceWeb") ?? 0).ToString("N2") : "0"%>
                        </ItemTemplate>
                        <ItemStyle Width="40" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Giá web off">
                        <ItemTemplate>
                            <%# Eval("PriceWebOff") != null ? Convert.ToDouble(Eval("PriceWebOff").ToString()).ToString("N2") : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="60" HorizontalAlign="Right" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="SL">
                        <ItemTemplate>
                            <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N0") : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="40" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phí ship">
                        <ItemTemplate>
                            <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N0") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N0") : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="40" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phụ thu">
                        <ItemTemplate>
                            <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : "0"%>
                        </ItemTemplate>
                        <ItemStyle Width="40" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="ĐVT">
                        <ItemTemplate>
                            <%# Eval("CurrencyCode")%>
                        </ItemTemplate>
                        <ItemStyle Width="80" HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Thành tiền (VND)">
                        <ItemTemplate>
                            <%# Convert.ToDouble(Eval("TotalMoney") ?? 0).ToString("N0")%>
                        </ItemTemplate>
                        <ItemStyle Width="60" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tình trạng">
                        <ItemTemplate>
                            <%# Eval("DetailStatusText")%>
                        </ItemTemplate>
                        <ItemStyle Width="80" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    </Columns>
            </asp:GridView>
        </div>
        <table style="width: 35%; margin-top: 20px;" align="right" cellpadding="5" class="tableForm">
            <tr>
                <td style="text-align: right; padding-top: 10px;" colspan="2">
                    <asp:Button ID="btnOrder" runat="server" Text="Thanh toán" CssClass="button"
                        OnClientClick="return confirm('Quý khách có chắc chắn muốn thanh toán tiếp?');"
                        OnClick="btnOrder_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Quay lại" CssClass="button" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </div>

<script type="text/javascript">
        function InitializeToolTip() {
            $(".gridViewToolTip").tooltip({
                track: true,
                delay: 0,
                showURL: false,
                fade: 100,
                bodyHandler: function () {
                    return $($(this).next().html());
                },
                showURL: false
            });
        }
</script>

<script type="text/javascript">
    $(function () {
        InitializeToolTip();
    })
</script>
</asp:Content>
