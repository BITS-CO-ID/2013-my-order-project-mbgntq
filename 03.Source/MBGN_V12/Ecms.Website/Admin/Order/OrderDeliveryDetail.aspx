<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderDeliveryDetail.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderDeliveryDetail" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Chi tiết đơn hàng vận chuyển
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
                        <strong>Thời gian tạo ĐH</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Mã KH/Số hiệu KH </strong>
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
                        <strong>Ghi chú </strong>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblRemark" runat="server"></asp:Label>
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <strong>Ngày đến Mỹ</strong>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDateToUsa" CssClass="datepicker" />
                        <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                            YearMonthFormat="dd/MM/yyyy" TextBoxId="txtDateToUsa" Columns="1" DatePickerImageTooltip="Chọn ngày"
                            DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True">
                        </obout:Calendar>
                    </td>
                   
                </tr>
            </table>
            <br />
            <table style="width: 25%;" class="gridview" cellspacing="0" align="right">
                <tr>
                    <th>
                        Tổng tiền đơn hàng<br />(1)
                    </th>
                    <th>
                        Đã thanh toán<br />(2)
                    </th>
                    <th>
                         CN còn lại <br />(3)=(1)-(2)
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
                        <asp:Label ID="lblTotalRemain" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div class="data">
                <p class="p-note">
                    <b>Chi tiết đơn hàng </b></p>
                <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" Width="100%"
                    CssClass="gridview" OnDataBound="gridMain_DataBound" OnRowCommand="gridMain_RowCommand"
                    OnPageIndexChanging="gridMain_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="20" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã MH">
                            <ItemTemplate>
                                <%# Eval("DetailCode") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã Bill">
                            <ItemTemplate>
                                <asp:LinkButton CausesValidation="false" Text='<%# Eval("TrackingNo")%>' Enabled='<%# Eval("DetailStatus") != null ? Eval("DetailStatus").ToString().Equals("6") ? false: true : true %>'
                                    ID="lbtnTrackingNumber" CommandArgument='<%# Eval("TrackingNo") + "|" + Eval("OrderNoDelivery") + "|" + Eval("DetailStatus") + "|" + Eval("OrderDetailId")+ "|" + Eval("DateToUsa")+ "|" + Eval("DeliveryVNDate")+ "|" + Eval("DeliveryDate")%>'
                                    CommandName="UpdateProduct" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Cân nặng<br/>(kg)">
                            <ItemTemplate>
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>                            
                            <ItemStyle Width="40" HorizontalAlign="Center" />
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Phí Ship">
                            <ItemTemplate>
                                <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N2") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("G") : "" %>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá<br/>khai báo">
                            <ItemTemplate>
                                <%# Eval("DeclarePrice") != null ? Convert.ToDouble(Eval("DeclarePrice").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40" />
                        </asp:TemplateField>                
                        <asp:TemplateField HeaderText="Bảo<br/>hiểm(%)">
                            <ItemTemplate>
                                <%# Eval("InsuaranceConfigId") != null ? Convert.ToDouble(Eval("InsuaranceConfigValue").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá trị<br/>lô hàng">
                            <ItemTemplate>
                                <%# Eval("LotPrice") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Ngày giao<br/>hàng">
                            <ItemTemplate>
                                <%# Eval("DeliveryDate") != null ? Convert.ToDateTime(Eval("DeliveryDate").ToString()).ToString("dd/MM/yyyy") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="70" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thành tiền<br/>(VND)">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("TotalMoney") ?? 0).ToString("N0")%>
                            </ItemTemplate>
                            <ItemStyle Width="80" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnUpdate" Text="Cập nhật" runat="server" CommandArgument='<%# Eval("TrackingNo") + "|" + Eval("OrderNoDelivery") + "|" + Eval("DetailStatus") + "|" + Eval("OrderDetailId")+ "|" + Eval("DateToUsa")+ "|" + Eval("DeliveryVNDate")+ "|" + Eval("DeliveryDate") %>'
                                    CommandName="UpdateOrderDetail" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tình trạng">
                            <ItemTemplate>
                                <asp:LinkButton Text='<%# Eval("DetailStatusText") %>' Enabled='<%# Eval("DetailStatus") != null ? Eval("DetailStatus").ToString().Equals("6") ? false: true : true %>'
                                    ID="lbtnChangeStatus" CommandArgument='<%# Eval("TrackingNo") + "|" + Eval("OrderNoDelivery") + "|" + Eval("DetailStatus") + "|" + Eval("OrderDetailId")+ "|" + Eval("DateToUsa")+ "|" + Eval("DeliveryVNDate")+ "|" + Eval("DeliveryDate")%>'
                                    CommandName="ChangeStatus" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </asp:TemplateField>                        
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red" />
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="button cancel" OnClick="btnSave_Click" Text="Xác nhận đơn hàng" />
                <asp:Button ID="btnUpdate" runat="server" CssClass="button cancel" OnClick="btnUpdate_Click" OnClientClick="return  Confirm('Bạn có chắc chắn muốn hoàn tất đơn hàng?');" Text="Hoàn tất đơn hàng" />
                <asp:Button ID="btnComplete" runat="server" CssClass="button" OnClick="btnComplete_Click" Text="Xác nhận giao hàng" OnClientClick="return Confirm('Bạn có chắc chắn muốn xác nhận giao hàng này?');" />
                <asp:Button ID="btnCancel" runat="server" Text="Hủy đơn hàng" CssClass="button" CausesValidation="false" OnClick="btnCancel_Click" />
                <asp:Button ID="btnReverFirst" runat="server" Text="Hoàn lại TT Chưa xác nhận" CssClass="button" CausesValidation="false" OnClick="btnReverFirst_Click" OnClientClick="return Confirm('Bạn có chắc chắn muốn Hoàn lại tình trạng chưa xác nhận cho cho đơn hàng này?');"/>
                <asp:Button ID="btnRevert" runat="server" Text="Hoàn lại TT Hoàn thành" CssClass="button" CausesValidation="false" OnClick="btnRevert_Click" OnClientClick="return Confirm('Bạn có chắc chắn muốn Hoàn lại tình trạng cho đơn hàng này?');"/>
                <asp:Button ID="btnBack" runat="server" CssClass="button cancel" CausesValidation="false" OnClick="btnBack_Click" Text="Quay lại" />
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
                        <asp:Button ID="btnConfirm" Text="Xác nhận" runat="server" OnClick="btnConfirm_Click" CssClass="button"
                            CausesValidation="false" OnClientClick="return Confirm('Bạn có chắc chắn muốn xác nhận đơn hàng này?');"/>
                        <asp:Button ID="btnConfirmCancel" Text="Xác nhận Hủy" runat="server" OnClick="btnConfirmCancel_Click" CssClass="button"
                            CausesValidation="false" OnClientClick="return Confirm('Bạn có chắc chắn muốn xác nhận Hủy đơn hàng này?');"/>
                        <asp:Button ID="btnOK" Text=" OK " runat="server" OnClick="btnOK_Click" CssClass="button cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
