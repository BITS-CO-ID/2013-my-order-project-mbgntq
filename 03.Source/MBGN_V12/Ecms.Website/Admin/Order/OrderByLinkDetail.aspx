<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderByLinkDetail.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderByLinkDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Chi tiết đơn hàng mua hộ
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 750px;" >
                <tr>
                    <td class="tdFirst">
                        <strong>Mã đơn hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Ngày tạo </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Mã khách hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerCode" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Số điện thoại </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Tên khách hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Địa chỉ </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Mã Bill</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblTrackingNumber" runat="server"></asp:Label>
                    </td>
                   <td>
                        <strong>Ngày xác nhận</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblConfirmDate" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                   <td>
                        <strong>Ghi chú </strong>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblRemark" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table style="width:350px;" class="gridview" cellspacing="0" align="right">
                <tr>
                    <th>
                        Tổng tiền đơn hàng<br />(1)
                    </th>
                    <th>
                        Đã thanh toán<br/>(2)
                    </th>
                    <th>
                        Tiền vận chuyển<br/>(3)                        
                    </th>
                    <%--<th>
                        Tổng còn lại<br/>tính phí trả chậm<br/>(4)=(1)-(3)
                    </th>
                    <th>
                        Phí trả chậm<br/>(5)=(4)*(%Áp phí)
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
                    <td style="text-align: right;">
                        <asp:Label ID="lblTotalAmount" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
            <b>Chi tiết đơn hàng</b>
            <div class="data" style="padding-top:50px;">
                &nbsp;<asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    PageSize="15" Width="100%" OnRowCommand="gridMain_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="20" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã MH">
                            <ItemTemplate>
                                <%# Eval("DetailCode") %>
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Website">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdOrderDetailId" runat="server" Value='<%# Eval("OrderDetailId") %>' />
                                <%# Eval("WebsiteName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shop">
                            <ItemTemplate>
                                <%# Eval("Shop") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hình ảnh ">
                            <ItemTemplate>
                                <a target="_blank" href='<%# Eval("ImageUrl") %>'>
                                    <img src='<%# Eval("ImageUrl") %>' width="40" height="40" title="Ảnh sản phẩm" /></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link">
                            <ItemTemplate>
                                <a href='<%# Eval("ProductLink") %>' title='<%# Eval("ProductLink") %>' target="_blank">
                                    <%# Eval("ProductLink").ToString().Length < 15 ? Eval("ProductLink").ToString() : (Eval("ProductLink").ToString().Substring(0, 15) + "...")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Size">
                            <ItemTemplate>
                                <%# Eval("Size") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Màu sắc">
                            <ItemTemplate>
                                <%# Eval("Color") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Giá<br/>web">
                            <ItemTemplate>
                                <%# Eval("PriceWeb") != null ? Convert.ToDouble(Eval("PriceWeb") ?? 0).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cân<br/>nặng<br/)(kg)">
                            <ItemTemplate>
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight") ?? 0).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí ship">
                            <ItemTemplate>
                                <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N0") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N0") : "0"%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="ĐVTT">
                            <ItemTemplate>
                                <%# Eval("CurrencyCode")%>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Mô tả">
                            <ItemTemplate>
                                <%# Eval("Remark")%>
                            </ItemTemplate>
                            <ItemStyle Width="120" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thành tiền<br/>(VND)">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("TotalMoney")??0).ToString("N0") %>
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tình trạng">
                            <ItemTemplate>
                                <asp:LinkButton Text='<%# Eval("DetailStatusText") %>' Enabled='<%# Eval("DetailStatus") != null ? Eval("DetailStatus").ToString().Equals("5") ? true: false : true %>'
                                    ID="lbtnChangeStatus" CommandArgument='<%# Eval("DetailStatus") + "|" + Eval("OrderDetailId")%>'
                                    CommandName="ChangeStatusDelivery" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="100" HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton Text="Cập nhật" runat="server" ID="lbtnUpdate" CommandArgument='<%# Eval("OrderDetailId") %>'
                                    CommandName="updateOrderDetail" />
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red" />
                <br />
                <asp:Button ID="btnSave" runat="server" Text="Xác nhận đơn hàng" CssClass="button" OnClick="btnSave_Click" />
                <asp:Button ID="btnUpdate" runat="server" CssClass="button cancel" OnClick="btnUpdate_Click" Text="Hoàn tất đơn hàng" OnClientClick="return  Confirm('Bạn có chắc chắn muốn hoàn tất đơn hàng?');" />
                <asp:Button ID="btnComplete" runat="server" CssClass="button" OnClick="btnComplete_Click" Text="Xác nhận giao hàng" OnClientClick="return  Confirm('Bạn có chắc chắn muốn xác nhận giao hàng?');"/>
                <asp:Button ID="btnCancel" runat="server" Text="Hủy đơn hàng" CssClass="button cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                <asp:Button ID="btnReverFirst" runat="server" Text="Hoàn lại TT Chưa xác nhận" CssClass="button" CausesValidation="false" OnClick="btnReverFirst_Click" OnClientClick="return Confirm('Bạn có chắc chắn muốn Hoàn lại tình trạng chưa xác nhận cho cho đơn hàng này?');"/>
                <asp:Button ID="btnRevert" runat="server" Text="Hoàn lại TT Hoàn thành" CssClass="button" CausesValidation="false" OnClick="btnRevert_Click" OnClientClick="return Confirm('Bạn có chắc chắn muốn Hoàn lại tình trạng cho đơn hàng này?');"/>
                <asp:Button ID="btnBack" runat="server" CssClass="button cancel" OnClick="btnBack_Click" Text="Quay lại" CausesValidation="False" />
                <br />
            </div>
        </asp:View>
        <asp:View runat="server" ID="resultView">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="310px" 
                            Height="79px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnConfirm" Text="Xác nhận" runat="server" OnClick="btnConfirm_Click" CssClass="button" CausesValidation="false" OnClientClick="return Confirm('Bạn có chắc chắn muốn xác nhận đơn hàng này?');"/>
                        <asp:Button ID="btnConfirmCancel" Text="Xác nhận Hủy" runat="server" OnClick="btnConfirmCancel_Click" CssClass="button" CausesValidation="false" OnClientClick="return Confirm('Bạn có chắc chắn muốn xác nhận Hủy đơn hàng này?');"/>
                        <asp:Button ID="btnOK" Text=" OK " runat="server" OnClick="btnOK_Click" CssClass="button cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
