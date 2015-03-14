<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="GroupLinkCartUpdate.aspx.cs" Inherits="Ecms.Website.Admin.GroupLinkCartUpdate" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Đơn đặt hàng NN
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
                <asp:View ID="fromView" runat="server">
                    <table>
                        <tr>
                            <td>
                                Thông tin đơn hàng:
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                               Nhóm Website
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlGroupWebsite" runat="server" CssClass="Cbo" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupWebsite_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Website
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWebsite" runat="server" CssClass="Cbo" AutoPostBack="true" OnSelectedIndexChanged="ddlWebsite_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tài khoản đặt hàng<span class="required"><strong>(*)</strong></span>
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlWebsiteAccount" runat="server" CssClass="Cbo">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                BillNo<span class="required"><strong>(*)</strong></span>
                            </td>
                            <td>
                                 <asp:TextBox ID="txtTrackingNo" runat="server" CssClass="TextBox"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdFirst indend">
                                Ngày đặt hàng<span class="required"><strong>(*)</strong></span>
                            </td>
                             <td>
                                <asp:TextBox ID="txtOrderDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                                YearMonthFormat="dd/MM/yyyy" TextBoxId="txtOrderDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                                DatePickerImagePath="../../Content/Images/icons/Calender-icon.png">
                                </obout:Calendar>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ghi chú
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="Textbox" 
                                    TextMode="MultiLine" Height="80px" Width="500px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>

                    <div>
                        <br />
                        <table>
                        <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnSave" runat="server" Text="Lưu đơn hàng" OnClick="btnSave_Click" CssClass="Button" OnClientClick="return confirm('Bạn có chắc chắn muốn tạo đơn hàng này không?')" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>

                                <td>
                                    <asp:Button ID="btnCancel" runat="server" Text="Quay lại" OnClick="btnCancel_Click" CssClass="Button" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div>
                        <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
                    </div>
                </asp:View>
                <asp:View ID="resultView" runat="server">
                    <table align="center" style="width: 50%;">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã lưu đơn hàng thành công"></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
