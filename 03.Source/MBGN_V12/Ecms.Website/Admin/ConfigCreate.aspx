<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ConfigCreate.aspx.cs" Inherits="Ecms.Website.Admin.ConfigCreate" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Thiết lập chính sách phí
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
            <div class="content-box">
                <!-- Start Content Box -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table  style="width: 100%;">
                            <tr>
                                <td class="small" style="width: 150px">
                                    Thông tin chung
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chính sách:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="cbo" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlOrderType_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">-------- Tất cả -------</asp:ListItem>
                                       <%-- <asp:ListItem Value="101"> - Công</asp:ListItem>
                                        <asp:ListItem Value="TAXUSA"> - Thuế mỹ</asp:ListItem>
                                        <asp:ListItem Value="302"> - Số ngày trả chậm cho phép</asp:ListItem>
                                        <asp:ListItem Value="303"> - Giá trị trả chậm</asp:ListItem>--%>
                                        <asp:ListItem Value="ORGRATE"> - Tỉ giá mua hộ</asp:ListItem>
                                        <%--<asp:ListItem Value="ORGRATEDE"> - Tỉ giá vận chuyển</asp:ListItem>--%>
                                        <%--<asp:ListItem Value="FEE"> - Phí</asp:ListItem>--%>
                                        <asp:ListItem Value="401"> - Phí ship mua hộ</asp:ListItem>
                                       <%-- <asp:ListItem Value="INSUARANCE"> - Phí bảo hiểm</asp:ListItem>--%>
                                       <asp:ListItem Value="402"> - Phí ship vận chuyển</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trWebsiteGroup" visible="false">
                                <td>
                                    Nhóm website
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlWebsiteGroup" AutoPostBack="true" runat="server" 
                                        CssClass="cbo" onselectedindexchanged="ddlWebsiteGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trWebsite" visible="false">
                                <td>
                                    Website:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlWebsite" runat="server" CssClass="cbo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trCountry" visible="false">
                                <td>
                                    Xuất xứ:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="cbo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trApplyService" visible="false">
                                <td>
                                    Dịch vụ áp dụng
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlApplyService" runat="server" CssClass="cbo">
                                        <asp:ListItem Value="">-- Chọn dịch vụ --</asp:ListItem>
                                        <asp:ListItem Value="FEEMH">Mua hộ hàng</asp:ListItem>
                                        <asp:ListItem Value="FEE">Vận chuyển</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trCustomerType" visible="false">
                                <td>
                                    Đối tượng khách hàng:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="cbo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trCategory" visible="false">
                                <td>
                                    Chủng loại sản phẩm:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="cbo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trFromQuantity" visible="false">
                                <td>
                                    <asp:Label ID="lblFrom" runat="server" Text="SL từ:"></asp:Label>                                    
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromQuantity" runat="server" CssClass="doubleNumber Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" id="trToQuantity" visible="false">
                                <td>
                                    <asp:Label ID="lblTo" runat="server" Text="SL đến:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToQuantity" runat="server" CssClass="doubleNumber Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" id="trConfigValue" visible="false">
                                <td class="small" style="width: 150px">
                                    Giá trị(<span style="color: #CC0000">*</span>)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfigValue" runat="server" CssClass="doubleNumber Textbox"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblCurrency" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="small" colspan="2">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="btnLine">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Lưu" CssClass="Button" />
                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click"
                        CssClass="Button" CausesValidation="False" />
                </div>
            </div>
        </asp:View>
        <asp:View ID="resultView" runat="server">
            <table align="center" style="width: 50%;">
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
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
