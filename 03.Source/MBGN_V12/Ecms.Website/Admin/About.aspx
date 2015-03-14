<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="Ecms.Website.Admin.About" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Abount
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            
            <table>
            <tr>
                <td>
                <strong>Thông tin phần mềm</strong>        
                </td>
                <td>
                <h2>Hệ thống quản lý mua bán giao nhận hàng Trung Quốc</h2>
                </td>
            </tr>
            <tr>
                <td>
                <strong>Thời gian cập nhât gần nhất:</strong>        
                </td>
                <td>
                <h4> <%= "16/03/2014" %></h4>
                </td>
            </tr>
            <tr>
                <td>
                <a href="../Download/MBGN_UM_v1.0.doc" style='font-size:medium'>Dowload hướng dẫn sử dụng</a>
                
                </td>
            </tr>
            </table>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <%--<table align="center" style="width: 50%;">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Thiết lập chính sách thành công."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" OnClick="btnOK_Click" />
                    </td>
                </tr>
            </table>--%>
        </asp:View>
    </asp:MultiView>
</asp:Content>
