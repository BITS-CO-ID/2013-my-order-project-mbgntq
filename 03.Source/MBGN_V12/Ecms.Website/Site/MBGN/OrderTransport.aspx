<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderTransport.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderTransport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        Quý khách vui lòng điền đầy đủ thông tin vào các ô (<span class="required">*</span>)
        để yêu cầu vận chuyển hàng hóa</h4>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <asp:MultiView ActiveViewIndex="0" ID="mtvMain" runat="server">
        <asp:View ID="step2View" runat="server">

           
            <table style="width: 100%;" class="tableForm">
                <tr>
                    <td colspan="3" class="td-bg">
                        THÔNG TIN GỬI HÀNG
                    </td>
                </tr>
                <tr>
                    <td class="indend">
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
                        Mã Bill
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrackingNumber" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                    <tr>
                        <td class="indend">
                            Mua bảo hiểm hàng hóa
                        </td>
                        <td>
                            <asp:CheckBox ID="chkInsuarance" runat="server" AutoPostBack="true" 
                                OnCheckedChanged="chkInsuarance_OnCheckedChanged" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr ID="trLot" runat="server" visible="false">
                        <td class="indend">
                            Giá trị lô hàng
                        </td>
                        <td>
                            <asp:TextBox ID="txtLotPrice" runat="server" CssClass="Textbox"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="td-bg">
                        </td>
                    </tr>
            </table>            
            <div style = "padding:15px 0px;">
                <asp:Button ID="btnAddToCart" runat="server" CssClass="button" OnClick="btnAddToCart_Click" Text="Thêm BillNo" />
            </div>
            <asp:Panel ID="pnCartTransport" runat="server">
                <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" Width="100%"
                    CssClass="gridview">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Mã MH">
                            <ItemTemplate>
                                <%# Eval("DetailCode")%>
                            </ItemTemplate>
                            <ItemStyle Width="60" HorizontalAlign="Left" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="BillNo">
                            <ItemTemplate>
                                <%# Eval("TrackingNo")%>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Order Number">
                            <ItemTemplate>
                                <%# Eval("OrderNoDelivery")%>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Mua bảo hiểm">
                            <ItemTemplate>
                                <%# Eval("InsuaranceConfigValue") != null ? Convert.ToDouble(Eval("InsuaranceConfigValue")).ToString("N")+"%" : "0"%>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá trị lô hàng">
                            <ItemTemplate>
                                <%# Eval("LotPrice")%>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <table style="width: 30%;" align="right">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSendOrder" runat="server" CssClass="button cancel" OnClick="btnSendOrder_Click" Text="Gửi đơn hàng" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
        </asp:View>
        <asp:View runat="server" ID="resultView">
                <table style="width: 50%;" align="center">
                    <tr>
                        <td>
                        <b>Nhập thông tin thêm</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="Textbox" Height="51px" Width="450px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnOrder" runat="server" CssClass="button cancel" OnClick="btnOrder_Click" Text="Xác nhận gửi đơn hàng" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br />
        </asp:View>
    </asp:MultiView>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
