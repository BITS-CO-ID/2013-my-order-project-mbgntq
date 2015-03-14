<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderDeliveryCreate.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderDeliveryCreate" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Tạo đơn hàng vận chuyển
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" ID="mtvMain" runat="server">
        <asp:View ID="step1View" runat="server">
            <table style="width: 50%;">
                <tr>
                    <td colspan="2">
                        <strong>THÔNG TIN GỬI HÀNG</strong>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Mã Bill <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrackingNumber" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Mua bảo hiểm hàng hóa</td>
                    <td>
                        <asp:CheckBox ID="chkInsuarance" runat="server" AutoPostBack="true" OnCheckedChanged="chkInsuarance_OnCheckedChanged"/>
                    </td>
                </tr>
                <tr id="trLotPrice" runat="server" visible="false">
                    <td class="indend">
                        Gía trị lô hàng</td>
                    <td>
                        <asp:TextBox ID="txtLotPrice" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend" colspan="2">
                        <asp:Label ID="lblErrorStep1" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button Text="Tiếp tục" runat="server" ID="btnStep2" CssClass="button" OnClick="btnStep2_Click" />
                <asp:Button ID="btnStep3" runat="server" CssClass="button" Text="Xác nhận gửi hàng" Visible="False" OnClick="btnStep3_Click" />
            </div>
        </asp:View>
        <asp:View ID="step2View" runat="server">
            <table style="width: 900px;" class="tableForm">
                <tr>
                    <td colspan="2">
                        <strong> THÔNG TIN HÀNG GỬI</strong>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Mã Bill
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrackingCurrent" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Tên sản phẩm
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductName" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Cân nặng(kg)
                    </td>
                    <td>
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Phí ship mới(VNĐ)</td>
                    <td>
                        <asp:TextBox ID="txtShipModified" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                        <asp:Label ID="lblShip" runat="server" Font-Bold="true" Font-Italic="true" Text="(Nếu muốn thay đổi phí ship đã thiết lập sẵn thì nhập thông tin Ship mới vào đây)"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td class="indend">
                        Số lượng
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="indend">
                        Phụ thu(VNĐ)
                    </td>
                    <td>
                        <asp:TextBox ID="txtSurcharge" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Giá khai báo</td>
                    <td>
                        <asp:TextBox ID="txtDeclarePrice" runat="server" CssClass="doubleNumber textbox"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="indend" colspan="2">
                        <asp:Label ID="lblErrorStep2" runat="server" ForeColor="Red" Visible="False" />
                    </td>
                </tr>
            </table>
            <p class="btnLine">
                <asp:Button ID="btnAddToCartTransport" runat="server" Text="Thêm vào đơn hàng" OnClick="btnAddToCartTransport_Click" CssClass="button" />
                <asp:Button ID="btnPrevious" runat="server" CssClass="button cancel" OnClick="btnPrevious_Click" Text="Quay lại" />
            </p>
            <asp:Panel runat="server" ID="pnCartTransport" Visible="false">
                <p class="p-note">
                    CHI TIẾT ĐƠN HÀNG GỬI</p>
                <asp:GridView ID="gridCartTransport" runat="server" AutoGenerateColumns="False" OnRowCommand="gridCartTransport_RowCommand"
                    Width="100%" CssClass="gridview" AllowPaging="True" OnPageIndexChanging="gridCartTransport_PageIndexChanging"
                    PageSize="15" ondatabound="gridCartTransport_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã Bill">
                            <ItemTemplate>
                                <asp:Literal Text='<%# Eval("TrackingNo")%>' ID="litTrackingNumber" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Tên sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Cân nặng<br/>(kg)">
                            <ItemTemplate>
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí ship">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : "0.0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : "0.0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bảo hiểm(%)">
                            <ItemTemplate>
                                <%# Eval("InsuaranceConfigId") != null ? Convert.ToDouble(Eval("InsuaranceConfigValue").ToString()).ToString("N2") : "0.0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton Text="Xóa" CommandArgument='<%# Eval("Id") %>' CommandName="deleteCartTransport"
                                    OnClientClick="return confirm('Quý khách có chắc chắn muốn xóa?');" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table style="width: 100%;" align="right">
                    <tr>
                        <td style="text-align: left;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnNext" runat="server" CssClass="button cancel" OnClick="btnNext_Click" Text="Tiếp tục" />
                            <asp:Button ID="btnBackToStep1" runat="server" CssClass="button cancel" OnClick="btnBackToStep1_Click" Text="Nhập tiếp" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="button cancel" OnClick="btnCancel_Click" Text="Huỷ bỏ" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="step3View" runat="server">
            <table style="width: 100%;" class="tableForm">
                <tr>
                    <td colspan="3" class="td-bg">
                        THÔNG TIN GỬI HÀNG
                    </td>
                </tr>
                <tr>
                    <td class="tdFirst indend">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Tài khoản yêu cầu (<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerUserCode" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Ghi chú
                    </td>
                    <td rowspan="3">
                        <asp:TextBox ID="txtNote" runat="server" CssClass="textbox txtNote" TextMode="MultiLine"
                            Height="60px" Width="400px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblError" Visible="false" ForeColor="Red" runat="server" />
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnOrder" runat="server" CssClass="button cancel" OnClick="btnOrder_Click" Text="Xác nhận" />
                <asp:Button ID="btnBack" runat="server" CssClass="button cancel" OnClick="btnBack_Click" Text="Quay lại" />
            </div>
            <br />
        </asp:View>
        <asp:View ID="step4View" runat="server">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <strong>Yêu cầu gửi hàng đã được tạo thành công </strong>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" Text="OK" runat="server" OnClick="btnOK_Click" CssClass="button cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
