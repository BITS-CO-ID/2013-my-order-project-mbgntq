<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="Payment.aspx.cs" Inherits="Ecms.Website.Site.MBGN.Payment" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        Thông tin thanh toán</h4>
    <p>
        Quý khách thực hiện chuyển khoản vào tài khoản của QuangChau247.vn theo thông
        tin sau: <br /><br />
        <span class="indend">• Số tài khoản: 0491000019253 – Ngân hàng Ngoại thương Việt Nam (Vietcombank), chi nhánh </span>
        Thăng Long. <br />
        <span class="indend">• Chủ tài khoản: Trần Thị Nhung </span><br />
        <span class="indend">• Xem thêm tài khoản tại các ngân hàng <a href="http://quangchau247.vn/site/mbgn/NewsDetail.aspx?NewsId=59"><b>Tại đây</b></a></span><br />
        <span class="indend">• Thanh toán trực tiếp tại: Ngõ 445, Lạc Long Quân, Tây Hồ, Hà Nội. </span>
        <br /><br />Sau khi chuyển khoản, Quý khách vui lòng
        gửi thông báo thanh toán tới chúng tôi theo form dưới đây:
    </p>
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="formView" runat="server">
            <table style="width: 100%;" align="center">
                <tr id ="trTotal" runat="server">
                    <td class="tdFirst" colspan="2">
                        <b>Tổng số tiền đơn hàng hiện tại là:</b>&nbsp;
                        <asp:Label ID="lblTotalMoney" runat="server"></asp:Label>
                        &nbsp;VND
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdFirst">
                        Ngân hàng (<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="ddlBank">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Số tài khoản chuyển (<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="textbox txtAccountNo"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        Mã đơn hàng:
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderNo" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        Số tiền (<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="textbox txtAmount doubleNumber" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nội dung thanh toán
                    </td>
                    <td rowspan="3">
                        <asp:TextBox ID="txtContentPay" runat="server" Height="60px" TextMode="MultiLine"
                            Width="300px"></asp:TextBox>
                        &nbsp; &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Gửi thông báo" OnClick="btnAccept_Click" OnClientClick="return confirm('Quý khách có chắc chắn muốn gửi thông báo?')" />
                        <asp:Button ID="btnPayAffter" runat="server" Text="Thanh toán sau" OnClick="btnPayAffter_Click" CssClass="cancel button" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table style="width: 80%;" align="center">
                <tr>
                    <td>
                        <p>
                            Thông tin thanh toán đã được gửi đến QuangChau247. Chúng tôi sẽ xác nhận và
                            trả lời đến quý khách trong thời gian sớm nhất.
                        </p>
                        <p>
                            Xin cảm ơn!</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnOK" CssClass="cancel button" Text="Về trang chủ" runat="server" OnClick="btnOK_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
