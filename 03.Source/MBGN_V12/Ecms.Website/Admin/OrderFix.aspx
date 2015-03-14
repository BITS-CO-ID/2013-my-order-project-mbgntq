<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderFix.aspx.cs" Inherits="Ecms.Website.Admin.OrderFix" %>

<%@ Register TagPrefix="cc" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Abount
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            
            <table>
            
            </table>
            <asp:Button ID="btnOrderFix" runat="server" Text="Order Fix" OnClick="btnOrderFix_OnClick"/>
            <asp:Button ID="btnOrderFixInvoice" runat="server" Text="btnOrderFixInvoice" OnClick="btnOrderFixInvoice_OnClick"/>
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
