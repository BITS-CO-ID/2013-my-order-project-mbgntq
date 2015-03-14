<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderOutboundManage.aspx.cs" Inherits="Ecms.Website.Admin.OrderOutboundManage" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý đơn hàng nước ngoài
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
                <asp:View ID="fromView" runat="server">
                    <table style="width:750px;">
                        <tr>
                            <td>
                                Từ ngày
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="datepicker" />
                                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtDateFrom" Columns="1" DatePickerImageTooltip="Chọn ngày"
                                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png">
                                </obout:Calendar>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Đến ngày
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="datepicker" />
                                <obout:Calendar ID="cldToDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtDateTo" Columns="1" DatePickerImageTooltip="Chọn ngày"
                                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png">
                                </obout:Calendar>
                            </td>
                            <td>
                                
                            </td>
                            <td>                               
                                <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="Textbox" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mã đơn hàng
                            </td>
                            <td>
                                <asp:TextBox ID="txtOOD" runat="server" CssClass="Textbox"></asp:TextBox>
                            </td>
                            <td>
                                <%--Mã Bill   --%>     
                                Tình trạng                        
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="Cbo">
                                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                                    <asp:ListItem Text="Đang gom, đang xử lý" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Đã mua" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Đã hủy" Value="3"></asp:ListItem>
                                    <%--<asp:ListItem Text="Đã đến Mỹ" Value="4"></asp:ListItem>--%>
                                    <asp:ListItem Text="Đã về Việt Nam" Value="5"></asp:ListItem>
                                    <%--<asp:ListItem Text="Đã giao hàng" Value="6"></asp:ListItem>--%>
                                </asp:DropDownList>     
                                <asp:TextBox ID="txtTrackingNo" runat="server" CssClass="Textbox" Visible="false"></asp:TextBox>                         
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Website</td>
                            <td>
                                <asp:DropDownList ID="ddlWebsiteGroup" runat="server" AutoPostBack="true" 
                                    CssClass="cbo" onselectedindexchanged="ddlWebsiteGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Tài khoản đặt hàng
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountWebsiteNo" runat="server" CssClass="Textbox"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr style="display:none;">
                            <td>
                                Shop
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWebsite" runat="server" CssClass="cbo">
                                </asp:DropDownList>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblError" ForeColor="Red" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="btnLine">
                        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" CssClass="Button" />
                        <asp:Button ID="btnAdd" runat="server" Text="Đưa vào giỏ hàng" OnClick="btnAdd_Click" CssClass="Button" Visible="false" />
                        <asp:Button ID="btnAddAll" runat="server" Text="Đặt hàng" OnClick="btnAddAll_Click" CssClass="Button" Visible="false" />
                        <asp:Button ID="btnViewCart" runat="server" Text="Xem giỏ hàng" OnClick="btnViewCart_Click" CssClass="Button" Visible="false" />
                    </div>
                    <div>
                        <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                            Width="100%" AllowPaging="True" PageSize="15" OnPageIndexChanging="grdD_PageIndexChanging"
                            OnRowCommand="grdD_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="STT">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã đơn hàng">
                                    <ItemTemplate>
                                        <asp:LinkButton Text='<%# Eval("OrderOutboundNo")%>' ID="lbtnDetail" CommandArgument='<%# Eval("OrderOutboundId") %>'
                                            CommandName="OrderOutBoundDetail" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="BillNo">
                                    <ItemTemplate>
                                        <%# Eval("TrackingNo")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Order Number">
                                    <ItemTemplate>
                                        <%# Eval("OrderNumber")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Ngày đặt hàng">
                                    <ItemTemplate>
                                        <%# Eval("OrderDate") == null ? "" : Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Người đặt">
                                    <ItemTemplate>
                                        <%# Eval("UserCreate") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Website">
                                    <ItemTemplate>
                                        <%# Eval("WebsiteName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tài khoản đặt hàng">
                                    <ItemTemplate>
                                        <%# Eval("AccountWebsiteNo")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Số TK TT">
                                    <ItemTemplate>
                                        <%# Eval("VisaNo")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tình trạng">
                                    <ItemTemplate>
                                        <%# Eval("StatusText")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="200" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Thành tiền">
                                    <ItemTemplate>
                                        <%# Eval("SumAmount") == null ? "0" : Convert.ToDouble(Eval("SumAmount")).ToString("N0")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnUpdate" runat="server" CommandArgument='<%#Eval("OrderOutboundId") %>'
                                            CommandName="OrderOutBoundUpdate">Cập nhật</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("OrderOutboundId") %>'
                                            Visible ='<%#Eval("Status").ToString().Equals("1")?true:false %>'
                                            CommandName="OrderOutBoundDelete" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa đơn hàng này?')">Xóa</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="50" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
                    </div>
                </asp:View>
                <asp:View ID="resultView" runat="server">
                    <table align="center" style="width: 50%;">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xóa User thành công"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" OnClick="btnOK_Click"/>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
