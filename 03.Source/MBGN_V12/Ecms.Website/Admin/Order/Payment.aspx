<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="Payment.aspx.cs" Inherits="Ecms.Website.Admin.Order.Payment" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Lập hóa đơn thanh toán
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
        <asp:View ID="formView" runat="server">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table style="width: 65%;">
                        <tr>
                            <td colspan="2">
                                Thông tin thanh toán
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Loại thanh toán (<span style="color: Red;">*</span>)
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="cbo" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="ddlPaymentType_SelectedIndexChanged">
                                    <%--<asp:ListItem Value="">-- Tất cả --</asp:ListItem>--%>
                                    <asp:ListItem Value="201" Selected="True">Khách hàng gửi xác nhận thanh toán</asp:ListItem>
                                    <%--<asp:ListItem Value="202">MBGN Hoàn tiền cho khách hàng</asp:ListItem>--%>
                                    <%--<asp:ListItem Value="203">MBGN mua hàng từ đối tác</asp:ListItem>--%>
                                    <asp:ListItem Value="209">Phân bổ thanh toán cho đơn hàng từ SD khả dụng</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server" id="trBank" visible="false">
                            <td class="tdFirst">
                                Ngân hàng (<span style="color: Red;">*</span>)
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="cbo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server" id="trAccountNo" visible="false">
                            <td>
                                Số tài khoản chuyển
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr runat="server" id="trOrderNo" visible="false">
                            <td>
                                Mã đơn hàng:
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr runat="server" id="trCreatedUser" visible="false">
                            <td>
                                Người thanh toán (<span style="color: Red;">*</span>)
                            </td>
                            <td>
                                <asp:TextBox ID="txtCreatedUser" runat="server" CssClass="textbox"></asp:TextBox>
                                <asp:Button ID="btnCheckAvaiable" runat="server" CssClass="Button" OnClick="btnCheckAvaiable_Click" Text="Kiểm tra SD khả dụng" Font-Italic="true" Visible="false" Width="180px"/>
                                <asp:Label ID="lblBalanceAvaiable" runat="server" Text="" Font-Bold="true" Font-Italic="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            <asp:Label ID="Label1" runat="server" Text="(Người thanh toán có thể là Mã KH hoặc tên đăng nhập)" Font-Bold="true" Font-Italic="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mã đơn hàng
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
                                <asp:Button ID="btnOrderDebit" runat="server" CssClass="Button" OnClick="btnOrderDebit_Click" Text="Kiểm tra CN ĐH còn lại" Font-Italic="true" Visible="false" Width="180px"/>
                                <asp:Label ID="lblOrderDebit" runat="server" Text="" Font-Bold="true" Font-Italic="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ngày thanh toán (<span style="color: Red;">*</span>)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPaymentDate" CssClass="datepicker" />
                                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtPaymentDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True">
                                </obout:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Số tiền (<span style="color: Red;">*</span>)
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="doubleNumber textbox" Font-Bold="true" Font-Size="Large"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nội dung
                            </td>
                            <td rowspan="3">
                                <asp:TextBox ID="txtContent" runat="server" Height="60px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Xác nhận thanh toán"
                    OnClick="btnAccept_Click" OnClientClick="return confirm('Bạn có chắc muốn xác nhận thanh toán');" />
                &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="button" OnClick="btnBack_Click" Text="Quay lại" Visible="false"/>
                <br />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table width="50%" border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td style="text-align: center;">
                       <strong> Đã xác nhận thông tin thanh toán</strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnOk" runat="server" CssClass="button" OnClick="btnOk_Click" Text="Ok" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
