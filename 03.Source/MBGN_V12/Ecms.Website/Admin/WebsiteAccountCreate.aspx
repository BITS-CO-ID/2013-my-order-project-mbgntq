<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="WebsiteAccountCreate.aspx.cs" Inherits="Ecms.Website.Admin.WebsiteAccountCreate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Label ID="lblFunction" runat="server" Text="Thêm mới"></asp:Label>
    &nbsp;website account

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <asp:MultiView ID="mtvView" runat="server" ActiveViewIndex="0">
        <asp:View ID="fromView" runat="server">       
                    <table  style="width: 100%;">
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                        <tr>
                            <td style="width: 150px">
                                Website (<span style="color: #CC0000">*</span>):
                            </td>
                            <td>
                                <asp:DropDownList ID="ddParrentWebsite" runat="server"
                                    CssClass="Cbo">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                       
                       <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <tr>
                            <td style="width: 123px">
                                Tài khoản mua hàng (<span style="color: #CC0000">*</span>):
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountWebsiteNo" runat="server" CssClass="Textbox" MaxLength="50"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAccountWebsiteNo"
                                    CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Link Website:
                            </td>
                            <td>
                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="Textbox" MaxLength="200"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ghi chú:
                            </td>
                            <td >
                                <asp:TextBox ID="txtRemark" runat="server" Height="83px" MaxLength="200" TextMode="MultiLine"
                                    Width="350px"></asp:TextBox>
                            </td>
                            <td >
                                &nbsp;
                            </td>
                        </tr>                                           
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>
                            </td>
                        </tr>
                    </table>
                
            <div class="btnLine">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Lưu" CssClass="Button" />
                <asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click" CssClass="Button" CausesValidation="False" />
            </div>
        </asp:View>
        <asp:View ID="visaView" runat="server">
           
        </asp:View>
        </asp:MultiView>
</asp:Content>


