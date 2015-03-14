<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderDetailTrackingUpdate.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderDetailTrackingUpdate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cập nhật Mã Bill
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" ID="mtvMain" runat="server">
        <asp:View ID="step2View" runat="server">
            <table style="width: 500px" class="tableForm">
                <tr>
                    <td class="indend">
                        Shop: 
                    </td>
                    <td>
                        <asp:TextBox ID="txtShop" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="indend">
                        Mã Bill<span style="color:Red;">(*)</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNo" runat="server" CssClass="textbox"></asp:TextBox>
                        <asp:Label ID="Label1" runat="server" style="font-weight:bold; font-style:italic;" Text="Nhập Mã Bill mới nếu muốn thay đổi" />
                    </td>
                </tr>
                <tr>
                    <td class="indend" colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" />
                    </td>
                </tr>
            </table>
            <div class="btnLine">
            </div>
            <asp:Panel runat="server" ID="pnCartTransport" Visible="false">
                <p class="p-note">
                    Chi tiết món hàng
                </p>
                <asp:GridView ID="gridCartTransport" runat="server" AutoGenerateColumns="False" OnRowCommand="gridCartTransport_RowCommand"
                    Width="100%" CssClass="gridview" AllowPaging="True" OnPageIndexChanging="gridCartTransport_PageIndexChanging"
                    PageSize="15" OnDataBound="gridCartTransport_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã MH">
                            <ItemTemplate>
                                <%# Eval("DetailCode") %>
                            </ItemTemplate>
                            <ItemStyle Width="60" />
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
                                <%# Eval("Weight") != null ? Convert.ToDouble(Eval("Weight").ToString()).ToString("N2") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí Ship">
                            <ItemTemplate>
                                <%# Eval("ShipModified") != null ? Convert.ToDouble(Eval("ShipModified").ToString()).ToString("N0") : Eval("ShipConfigValue") != null ? Convert.ToDouble(Eval("ShipConfigValue").ToString()).ToString("N0") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="SL">
                            <ItemTemplate>
                                <%# Eval("Quantity") != null ? Convert.ToDouble(Eval("Quantity").ToString()).ToString("N1") : ""%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ thu">
                            <ItemTemplate>
                                <%# Eval("Surcharge") != null ? Convert.ToDouble(Eval("Surcharge").ToString()).ToString("N2") : "0"%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("OrderDetailId") %>' CommandName="OrderDetailDelete" Text="Xóa" runat="server" OnClientClick="return confirm('Bạn có chắc muốn loại bỏ link sản phẩm này?');"  />
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <p>
                    <asp:Button Text="Xác nhận" runat="server" CssClass="button" ID="btnAccept" OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận thay đổi thông tin?');" OnClick="btnAccept_Click" />
                    <asp:Button Text="Quay lại" runat="server" CssClass="button" ID="btnReturn" OnClick="btnReturn_Click" />
                </p>
            </asp:Panel>
        </asp:View>
        <asp:View ID="step4View" runat="server">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <strong>Đã cập nhật Mã Bill thành công </strong>
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
