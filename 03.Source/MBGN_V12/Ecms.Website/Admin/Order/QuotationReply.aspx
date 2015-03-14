<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="QuotationReply.aspx.cs" Inherits="Ecms.Website.Admin.Order.QuotationReply" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Trả lời báo giá
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 65%;" >
                <tr>
                    <td class="tdFirst">
                        <strong>Mã đơn hàng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                    </td>
                    <td class="tdFirst">
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
                        <strong>Ghi chú </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblRemark" runat="server"></asp:Label>
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                </tr>
                
            </table>
            <table></table>
            <p class="p-note">
                <b>Chi tiết báo giá</b>
            </p>
            <div class="data">
                <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    PageSize="15" Width="100%" OnRowCommand="gridMain_RowCommand" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã MH">
                            <ItemTemplate>
                                <%# Eval("DetailCode") %>
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Left" />
                            <FooterTemplate>
                                <strong>Tổng:</strong>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Website">
                            <ItemTemplate>
                                <%# Eval("WebsiteName") %>
                            </ItemTemplate>
                            <ItemStyle Width="140" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shop">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdOrderDetailId0" runat="server" 
                                    Value='<%# Eval("OrderDetailId") %>' />
                                <%# Eval("Shop") %>
                            </ItemTemplate>
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hình ảnh ">
                            <ItemTemplate>
                                <a href='<%# Eval("ImageUrl") %>' target="_blank">
                                    <img src='<%# Eval("ImageUrl") %>' width="30" height="30" title="Ảnh sản phẩm" />
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link">
                            <ItemTemplate>
                                <a href='<%# Eval("ProductLink") %>' target="_blank" 
                                    title='<%# Eval("ProductLink") %>'>
                                <%# Eval("ProductLink").ToString().Length < 30 ? Eval("ProductLink").ToString() : (Eval("ProductLink").ToString().Substring(0,30)+"...") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chủng loại">
                            <ItemTemplate>
                                <%# Eval("CategoryName") %>
                            </ItemTemplate>
                            <ItemStyle Width="100" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Màu sắc">
                            <ItemTemplate>
                                <%# Eval("Color") %>
                            </ItemTemplate>
                            <ItemStyle Width="80" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size">
                            <ItemTemplate>
                                <%# Eval("Size")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá web">
                            <ItemTemplate>
                                <%# Eval("PriceWeb") != null ? Convert.ToDouble(Eval("PriceWeb") ?? 0).ToString("N2") : "0"%>                                
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Right" />
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Giá<br/>web off">
                            <ItemTemplate>
                                <%# Eval("PriceWebOff") != null ? Convert.ToDouble(Eval("PriceWebOff").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Right" />
                        </asp:TemplateField>              
                        <asp:TemplateField HeaderText="Số<br/>lượng">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="40" HorizontalAlign="Right" />
                        </asp:TemplateField>     
                       <asp:TemplateField HeaderText="Cân nặng<br/>(kg)">
                            <ItemTemplate>
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight") ?? 0).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí ship">
                            <ItemTemplate>
                                <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N2") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="40" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phụ thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="40" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ĐVTT">
                            <ItemTemplate>
                                <%# Eval("CurrencyCode")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thành tiền<br/>(VND)">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("TotalMoney")??0).ToString("N0") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80" />
                            <FooterTemplate>
                                <strong><%= dTotalMoneyOrder.ToString("N0")%></strong>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnUpdate" runat="server" 
                                    CommandArgument='<%# Eval("OrderDetailId") %>' CommandName="updateOrderDetail" 
                                    Text="Cập nhật" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="70" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red" />
                <br />
                <asp:Button ID="btnConfirm" runat="server" Text="Xác nhân trả lời báo giá" CssClass="button" OnClick="btnConfirm_Click"  />
                <asp:Button ID="btnCancel" runat="server" Text="Hủy báo giá" CausesValidation="false" CssClass="button cancel" OnClick="btnCancel_Click"/>
                <asp:Button ID="btnBack" runat="server" CausesValidation="false" CssClass="button cancel" OnClick="btnBack_Click" Text="Quay lại" />
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
                        <asp:Button ID="btnReply" Text="Xác nhận" runat="server" OnClick="btnReply_Click" CssClass="button cancel" CausesValidation="false" OnClientClick="return Confirm('Bạn có chắc chắn muốn trả lời báo giá này?');"/>
                        <asp:Button ID="btnReplyCancel" Text="Xác nhận Hủy" runat="server" OnClick="btnReplyCancel_Click" CssClass="button cancel" CausesValidation="false" OnClientClick="return Confirm('Bạn có chắc chắn muốn HỦY báo giá này?');"/>
                        <asp:Button ID="btnOK" Text=" OK " runat="server" OnClick="btnOK_Click" CssClass="button cancel" CausesValidation="false" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
