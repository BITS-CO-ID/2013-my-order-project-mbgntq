<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderDetailStatusUpdate.aspx.cs" Inherits="Ecms.Website.Admin.Report.OrderDetailStatusUpdate" %>


<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
<% Response.CacheControl = "Public"; %>
    Cập nhật tình trạng món hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

    <table style="width:750px;" >
        <tr>
            <td style="width: 150px;">
                Từ ngày(Ngày Đặt hàng)
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFromDate" CssClass="datepicker" />
                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtFromDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True" DatePickerSynchronize="true">
                </obout:Calendar>
               
            </td>
            <td>
               Loại đơn hàng
            </td>
            <td>
                <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                   <%-- <asp:ListItem Value="1">BG - Báo giá</asp:ListItem>--%>
                    <asp:ListItem Value="2">MH - Đặt hàng mua hộ</asp:ListItem>
                    <asp:ListItem Value="3">VC - Gửi hàng vận chuyển</asp:ListItem>
                    <%--<asp:ListItem Value="4">CS - Đặt hàng có sẵn</asp:ListItem>--%>
                </asp:DropDownList>
            </td>

            
        </tr>
        <tr>
            <td style="width: 150px;">
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
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="Cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <asp:ListItem Text="Mới gửi" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Đang gom, đang xử lý" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Đã mua" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Đã hủy" Value="3"></asp:ListItem>
                    <%--<asp:ListItem Text="Đã đến Mỹ" Value="4"></asp:ListItem>--%>
                    <asp:ListItem Text="Đã về Việt Nam" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Đã giao hàng" Value="6"></asp:ListItem>
                </asp:DropDownList>    
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Mã đơn hàng
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
            <td> 
                Mã MH
            </td>
            <td>
                <asp:TextBox ID="txtDetailCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Mã khách hàng
            </td>
            <td>
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
             <td>
                Mã Bill
            </td>
            <td>
                <asp:TextBox ID="txtTrackingNo" runat="server" CssClass="textbox" ToolTip="Tìm kiếm tương đối"></asp:TextBox>
            </td>
        </tr>    
        <tr>
            <td style="width: 150px;">
                Shop
            </td>
            <td>
                <asp:TextBox ID="txtShop" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
             <td>
                
            </td>
            <td>
                
            </td>
        </tr>        
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Tìm kiếm" OnClick="btnSearch_Click" />
    </div>
    <div>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnDataBound="gridMain_DataBound" 
            OnRowCommand="gridMain_RowCommand" OnRowDataBound="gridMain_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"  Width="30"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã MH">
                     <ItemTemplate>
                        <div class="tag">
                                <a class="gridViewToolTip"><%# Eval("DetailCode")%></a>
                                <div id="tooltip" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="white-space: nowrap; font-size:medium;" align="center" colspan="2"><b>Thông tin chi tiết món hàng: <%# Eval("DetailCode") %></b>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Loại đơn hàng:</b>&nbsp;</td>
                                            <td><%# Eval("OrderTypeId").ToString().Equals("3") ? "VC - Đơn hàng vận chuyển" : Eval("OrderTypeId").ToString().Equals("2")?"MH - Đơn hàng mua hộ":""%></td>
                                        </tr
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Nhóm sản phẩm:</b>&nbsp;</td>
                                            <td><%# Eval("CategoryName")%></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Tên sản phẩm:</b>&nbsp;</td>
                                            <td><%# Eval("ProductName")%></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Website:</b>&nbsp;</td>
                                            <td><%# Eval("WebsiteName")%></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Hình ảnh:</b>&nbsp;</td>
                                            <td><a target="_blank" href='<%# Eval("ImageUrl") %>'>
                                                <img src='<%# Eval("ImageUrl") %>' width="45" height="45" title="Ảnh sản phẩm" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Link sản phẩm:</b>&nbsp;</td>
                                            <td>
                                                <a href='<%# Eval("ProductLink") %>' title='<%# Eval("ProductLink") %>' target="_blank">
                                                    <%# Eval("ProductLink")!=null?(Eval("ProductLink").ToString().Length < 30 ? Eval("ProductLink").ToString() : (Eval("ProductLink").ToString().Substring(0, 30) + "...")):""%></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Ngày về VN:</b>&nbsp;</td>
                                            <td><%# Eval("DeliveryVNDate") == null ? "" : Convert.ToDateTime(Eval("DeliveryVNDate")).ToString("dd/MM/yyyy")%></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Ngày giao hàng:</b>&nbsp;</td>
                                            <td><%# Eval("DeliveryDate") == null ? "" : Convert.ToDateTime(Eval("DeliveryDate")).ToString("dd/MM/yyyy")%></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                    </ItemTemplate>
                    <ItemStyle Width="60"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại<br/>ĐH">
                     <ItemTemplate>
                     <asp:HiddenField id ="hdnOrderTypeId" runat="server" Value='<%#Eval("OrderTypeId")%>'/>
                        <%# Eval("OrderTypeName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"  Width="30"/>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Mã đơn hàng">
                     <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("OrderNo")%>' ID="lbtnDetail" CommandArgument='<%# Eval("OrderId") + "|" + Eval("OrderTypeId") %>'
                            CommandName="OrderDetail" runat="server" OnCommand="lbtnDetail_Click"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã Bill">
                    <ItemTemplate>
                        <asp:LinkButton CausesValidation="false" 
                            Text='<%# Eval("TrackingNo") %>' 
                            Enabled='<%# Eval("OrderTypeId").ToString().Equals("3")? Eval("DetailStatus").ToString().Equals("6") ? false: true : false %>'
                            ID="lbtnTrackingNumber" CommandArgument='<%# Eval("TrackingNo") + "|" + Eval("OrderNoDelivery") + "|" + Eval("DetailStatus") + "|" + Eval("OrderDetailId")+ "|" + Eval("DateToUsa")+ "|" + Eval("DeliveryVNDate")+ "|" + Eval("DeliveryDate")+ "|" + Eval("OrderId")+ "|" + Eval("CustomerTypeId")+ "|" + Eval("OrderTypeId")%>'
                            CommandName="UpdateProduct" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng NN">
                     <ItemTemplate>
                        <asp:LinkButton Text='<%# Eval("OrderOutboundNo")%>' ID="lbtnOrderOutBoundDetail" CommandArgument='<%# Eval("OrderOutboundId") %>'
                                CommandName="OrderOutBoundDetail" runat="server" OnCommand="lbtnOrderOutBoundDetail_Click"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày<br/>đặt hàng">
                    <ItemTemplate>
                        <div class="tag">
                                <a class="gridViewToolTip"><%# Eval("OrderDate") == null ? "" : Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy")%></a>
                                <div id="tooltip" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Ngày về VN:</b>&nbsp;</td>
                                            <td><%# Eval("DeliveryVNDate") == null ? "" : Convert.ToDateTime(Eval("DeliveryVNDate")).ToString("dd/MM/yyyy")%></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Ngày giao hàng:</b>&nbsp;</td>
                                            <td><%# Eval("DeliveryDate") == null ? "" : Convert.ToDateTime(Eval("DeliveryDate")).ToString("dd/MM/yyyy")%></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Mã KH" >
                    <ItemTemplate>
                        <div class="tag">
                                <a class="gridViewToolTip"><%# Eval("CustomerCode")%></a>
                                <div id="tooltip" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Tên đăng nhập:</b>&nbsp;</td>
                                            <td><%# Eval("UserCode")%></td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;"><b>Tên khách hàng:</b>&nbsp;</td>
                                            <td><%# Eval("CustomerName")%></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                    </ItemTemplate>
                    <ItemStyle Width="60" HorizontalAlign="Left" />
                </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Website">
                            <ItemTemplate>
                                <%# Eval("WebsiteName") %>
                            </ItemTemplate>
                            <ItemStyle Width="140" HorizontalAlign="Left" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Shop">
                            <ItemTemplate>
                                <asp:LinkButton Text='<%# Eval("Shop")%>' ID="lbtnShopDetail" CommandArgument='<%# Eval("OrderId") + "|" + Eval("Shop") %>'
                                CommandName="ShopDetail" runat="server" OnCommand="lbtnShopDetail_Click"/>
                            </ItemTemplate>
                            <ItemStyle Width="140" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link">
                            <ItemTemplate>
                                <a href='<%# Eval("ProductLink") %>' title='<%# Eval("ProductLink") %>' target="_blank">
                                    <%# Eval("ProductLink") != null ? (Eval("ProductLink").ToString().Length < 15 ? Eval("ProductLink").ToString() : (Eval("ProductLink").ToString().Substring(0, 15) + "...")) : ""%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("G") : "" %>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá<br/>web">
                            <ItemTemplate>
                                <%# Eval("PriceWeb") != null ? Convert.ToDouble(Eval("PriceWeb").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí<br/>Ship">
                            <ItemTemplate>
                                <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N0") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cân<br/>nặng<br/>(kg)">
                            <ItemTemplate>
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>                            
                            <ItemStyle Width="30" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ<br/>thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="30" HorizontalAlign="Right" />
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Thành tiền<br/>(VNĐ)">
                            <ItemTemplate>
                                <%# Convert.ToDouble(Eval("TotalMoney") ?? 0).ToString("N0")%>
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Right" />
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Cập nhật<br/>TT MH">
                   <ItemTemplate>
                        <asp:LinkButton ID="lbtnUpdate" Text="Cập nhật" runat="server" CommandArgument='<%# Eval("TrackingNo") + "|" + Eval("OrderNoDelivery") + "|" + Eval("DetailStatus") + "|" + Eval("OrderDetailId")+ "|" + Eval("DateToUsa")+ "|" + Eval("DeliveryVNDate")+ "|" + Eval("DeliveryDate") + "|" + Eval("OrderId")+ "|" + Eval("CustomerTypeId")+ "|" + Eval("OrderTypeId")%>'
                            CommandName="UpdateOrderDetail" />
                   </ItemTemplate>
                   <ItemStyle HorizontalAlign="Center" Width="60" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Tình trạng<br/>MH">
                            <ItemTemplate>
                                <asp:LinkButton Text='<%# Eval("DetailStatusText") %>' 
                                    Enabled='<%# Eval("OrderTypeId").ToString().Equals("3")? Eval("DetailStatus").ToString().Equals("6") ? false: true: (Eval("DetailStatus").ToString().Equals("2") || Eval("DetailStatus").ToString().Equals("3") || Eval("DetailStatus").ToString().Equals("4") || Eval("DetailStatus").ToString().Equals("5")) ? true: false %>'
                                    ID="lbtnChangeStatus" CommandArgument='<%# Eval("TrackingNo") + "|" + Eval("OrderNoDelivery") + "|" + Eval("DetailStatus") + "|" + Eval("OrderDetailId")+ "|" + Eval("DateToUsa")+ "|" + Eval("DeliveryVNDate")+ "|" + Eval("DeliveryDate")+ "|" + Eval("OrderId")+ "|" + Eval("CustomerTypeId")+ "|" + Eval("OrderTypeId")%>'
                                    CommandName="ChangeStatus" runat="server" />
                            </ItemTemplate>
                            
                 </asp:TemplateField>  
            </Columns>
        </asp:GridView>
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

