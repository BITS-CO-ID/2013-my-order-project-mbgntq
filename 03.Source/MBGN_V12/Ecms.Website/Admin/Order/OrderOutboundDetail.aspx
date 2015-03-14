<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderOutboundDetail.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderOutboundDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Chi tiết đơn hàng nước ngoài
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width:700px;">
        <tr>
            <td>
                <strong>Mã đơn hàng:</strong>
            </td>
            <td>
                <asp:Label ID="lblOrderOutboundCode" runat="server"></asp:Label>
            </td>
            <td>
                <strong>Ngày tạo:</strong>
            </td>
            <td>
                <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Người đặt hàng:</strong>
            </td>
            <td>
                <asp:Label ID="lblCreatedUser" runat="server"></asp:Label>
            </td>
            <td>
                <strong>Tài khoản đặt:</strong>
            </td>
            <td>
                <asp:Label ID="lblAccountCreated" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Mã Bill:</strong>
            </td>
            <td>
                <asp:Label ID="lblTrackingNumber" runat="server"></asp:Label>
            </td>
            <td>
                <strong>Tình trạng:</strong>
            </td>
            <td>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Ghi chú:</strong>
            </td>
            <td colspan="3">
                <asp:Label ID="lblRemark" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnBack" runat="server" CssClass="button" OnClick="btnBack_Click"
            Text="Quay lại" />
    </div>

    <div class="data">
        <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
            Width="100%" AllowPaging="True" PageSize="15" OnPageIndexChanging="grdD_PageIndexChanging"
            OnRowCommand="grdD_RowCommand" ShowFooter="true" OnRowDataBound="gridMain_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                    <FooterTemplate>
                        <strong>Tổng:</strong>
                    </FooterTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Mã MH">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.DetailCode")%>
                    </ItemTemplate>
                    <ControlStyle Width="60" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdODId" runat="server" Value='<%# Eval("OrderDetailId") %>' />
                        <%# Eval("OrderDetail.OrderNo") %>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.CustomerCode")%>
                    </ItemTemplate>
                    <ControlStyle Width="80" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã Bill">
                    <ItemTemplate>
                        <%--<%# Eval("OrderDetail.TrackingNo")%>--%>
                        <asp:LinkButton ID="lbtnTrackingNoUpdate" Text='<%# Eval("OrderDetail.TrackingNo")==null?"Empty":Eval("OrderDetail.TrackingNo").ToString()==""?"Empty": Eval("OrderDetail.TrackingNo")%>' CommandArgument='<%#Eval("OrderDetailId")%>'
                            CommandName="TrackingNoUpdate" runat="server"  OnClick="lbtnTrackingNoUpdate_Click"/>

                        

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shop">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.Shop")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hình ảnh ">
                    <ItemTemplate>
                        <a target="_blank" href='<%# Eval("OrderDetail.ImageUrl") %>'>
                            <img src='<%# Eval("OrderDetail.ImageUrl") %>' width="30" height="30" title="Ảnh sản phẩm" /></a>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Link">
                    <ItemTemplate>
                        <a href='<%# Eval("OrderDetail.ProductLink") %>' title='<%# Eval("OrderDetail.ProductLink") %>'
                            target="_blank">
                            <%# Eval("OrderDetail.ProductLink").ToString().Length < 15 ? Eval("OrderDetail.ProductLink").ToString() : (Eval("OrderDetail.ProductLink").ToString().Substring(0, 15) + "...")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Màu<br/>sắc">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.Color")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Size">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.Size")%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.Quantity")%>
                    </ItemTemplate>
                    <ItemStyle Width="40" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Giá<br/>web">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.PriceWeb") != null ? Convert.ToDouble(Eval("OrderDetail.PriceWeb") ?? 0).ToString("N2") : "0"%>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Right" />
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Giá</br>web off">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.PriceWebOff") != null ? Convert.ToDouble(Eval("OrderDetail.PriceWebOff").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Right" />
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Cân nặng<br/>(kg)">
                            <ItemTemplate>
                                <%# Eval("OrderDetail.Weight") != null ? Convert.ToDouble(Eval("OrderDetail.Weight").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>                            
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phí Ship">
                            <ItemTemplate>
                                <%# Eval("OrderDetail.ShipModified") != null ? Convert.ToDouble(Eval("OrderDetail.ShipModified").ToString()).ToString("N0") : Eval("OrderDetail.ShipConfigValue") != null ? Convert.ToDouble(Eval("OrderDetail.ShipConfigValue").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Phụ</br>thu">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.Surcharge") != null ? Convert.ToDouble(Eval("OrderDetail.Surcharge").ToString()).ToString("N2") : ""%>
                    </ItemTemplate>
                    <ItemStyle Width="50" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thành tiền<br/>(VNĐ)">
                    <ItemTemplate>
                        <%# Eval("OrderDetail.TotalMoney") == null ? "0" : Convert.ToDouble(Eval("OrderDetail.TotalMoney")).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle Width="80" HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= dSumAmount.ToString("N0") %></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("OrderDetail.DetailStatusText") %>' Enabled='<%# Eval("OrderDetail.DetailStatus") != null ? (Eval("OrderDetail.DetailStatus").ToString().Equals("2") || Eval("OrderDetail.DetailStatus").ToString().Equals("3") || Eval("OrderDetail.DetailStatus").ToString().Equals("4")) ? true: false : false %>'
                                    ID="lbtnChangeStatus" CommandArgument='<%# Eval("OrderDetail.OrderDetailId") + "|" + Eval("OrderDetail.DetailStatus")%>'
                                    CommandName="ChangeStatusDetail" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnUpdate" Text="Cập nhật" CommandArgument='<%#Eval("OrderDetail.OrderId") + "|" + Eval("OrderDetailId") + "|" + Eval("OrderOutboundId") %>'
                            CommandName="OODUpdate" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnDelete" Text="Xóa" CommandArgument='<%#  Eval("OrderOutboundDetailId")%>'
                            CommandName="OODDelete" runat="server" OnClientClick="return confirm('Bạn có chắc muốn xóa link sản phẩm này?');" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

        <asp:Button ID="button1" runat="server" Text="Button" style="display:none;"></asp:Button>
    
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="ModalPopupBG" runat="server" CancelControlID="btnCancel" TargetControlID="button1"
                            PopupControlID="Panel1" Drag="true" PopupDragHandleControlID="PopupHeader">
        </ajaxToolkit:ModalPopupExtender>

        <div id="Panel1" style="display: none;" class="popupConfirmation">
            <div class="popup_Container">
                <div class="popup_Titlebar" id="PopupHeader">
                    <div class="TitlebarLeft">Cập nhật Mã Bill</div>
                    <%--<div class="TitlebarRight"></div>--%>
                </div>
                <div class="popup_Body">
                    <p>
                        Mã Bill:
                    </p>
                    <asp:TextBox ID="txtTrackingNo" runat="server" Width="250px" CssClass="Textbox"></asp:TextBox>
                </div>
                <div class="popup_Buttons">
                    <asp:Button ID="btnOkay" runat="server" Text="Xác nhận" OnClick="btnOkay_Click" CssClass="button"/>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button"/>
                    <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click"/>--%>
                </div>
            </div>
        </div>
</asp:Content>
