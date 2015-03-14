<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ChangeDeliveryStatus.aspx.cs" Inherits="Ecms.Website.Admin.Order.ChangeDeliveryStatus" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cập nhật trạng thái vận chuyển
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View runat="server" ID="formView">
            <table style="width: 50%;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Tình trạng <span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="cbo" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                            <asp:ListItem Value="1">Đang xử lý</asp:ListItem>
                            <%--<asp:ListItem Value="4">Đã đến Mỹ</asp:ListItem>                            --%>
                            <asp:ListItem Value="5">Đã về Việt Nam</asp:ListItem>
                            <asp:ListItem Value="6">Đã giao hàng</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" visible="true">
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text="Ngày cập nhật TT"></asp:Label>
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
                <asp:Button Text="Xác nhận" runat="server" ID="btnAccept" CssClass="button" OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận?');" OnClick="btnAccept_Click" />
                <asp:Button ID="btnBack" runat="server" CssClass="button" OnClick="btnBack_Click" Text="Quay lại" />
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table style="width: 80%;" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        Cập nhật tình trạng vận chuyển thành công
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
