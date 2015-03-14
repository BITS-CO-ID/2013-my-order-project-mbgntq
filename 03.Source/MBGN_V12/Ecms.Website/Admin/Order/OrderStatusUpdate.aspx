<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderStatusUpdate.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderStatusUpdate" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cập nhật TT giao hàng cho món hàng MH
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <b>Cập nhật tình trạng cho món hàng mua hộ</b>
            <table style="width: 50%; margin-bottom: 0px;">
                <tr runat="server">
                    <td>
                        Tình trạng đơn hàng
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="Cbo" >
                            <asp:ListItem Value="3" Text="Hủy"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Đã về Việt Nam"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Đã giao hàng"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" >
                    <td>
                        Ngày cập nhật TT(<span style="color: Red;">*</span>)
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="datepicker" />
                        <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                            YearMonthFormat="dd/MM/yyyy" TextBoxId="txtFromDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                            DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True">
                        </obout:Calendar>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnAccept" runat="server" CssClass="button" Text="Xác nhận" OnClick="btnAccept_Click" />
                &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Quay lại" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" OnClick="btnOK_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
