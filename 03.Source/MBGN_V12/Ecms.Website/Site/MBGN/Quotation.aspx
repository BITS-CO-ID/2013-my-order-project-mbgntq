<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="Quotation.aspx.cs" Inherits="Ecms.Website.Site.MBGN.Quotation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        Quý khách vui lòng copy thông tin link sản phẩm vào mẫu sau để được QuangChau247 báo giá
        trong thời gian sớm nhất.</h4>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table style="width: 100%;" class="tableForm">
                <tr>
                    <td colspan="3" class="td-bg">
                        THÔNG TIN BÁO GIÁ
                    </td>
                </tr>
                <tr>
                    <td > 
                        Link sản phẩm 1
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlWebsiteGroup1" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td >
                        <asp:TextBox ID="txtLink1" runat="server" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td>
                        Link sản phẩm 2
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWebsiteGroup2" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLink2" runat="server"  Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td>
                        Link sản phẩm 3
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWebsiteGroup3" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLink3" runat="server"  Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td>
                        Link sản phẩm 4
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWebsiteGroup4" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLink4" runat="server"  Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td>
                        Link sản phẩm 5
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWebsiteGroup5" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLink5" runat="server"  Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td colspan="3" class="td-bg">
                        THÔNG TIN THÊM
                    </td>
                    
                </tr>
                <tr >
                    <td>Ghi chú</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemark" runat="server" Width="100%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Button ID="btnAccept" runat="server" CssClass="button" OnClick="btnAccept_Click" Text="Gửi báo giá" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="button cancel" OnClick="btnCancel_Click" Text="Hủy bỏ" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
