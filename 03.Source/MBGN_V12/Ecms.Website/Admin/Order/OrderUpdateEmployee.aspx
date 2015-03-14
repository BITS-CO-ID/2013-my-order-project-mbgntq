<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderUpdateEmployee.aspx.cs" Inherits="Ecms.Website.Admin.Order.OrderUpdateEmployee" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Cập nhật nhân viên bán hàng cho đơn hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <table style="width: 50%; margin-bottom: 0px;">
                <tr>
                    <td>
                        NV bán hàng (<span style="color:Red;">*</span>)</td>
                   <td>
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
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
                <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Hủy bỏ" OnClick="btnCancel_Click" />
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
