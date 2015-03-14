<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="WebsiteLinkCreate.aspx.cs" Inherits="Ecms.Website.Admin.WebsiteLinkCreate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Label ID="lblFunction" runat="server" Text="Thêm mới"></asp:Label>
    &nbsp; website link
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- Start Content Box -->
        <div class="content-box-content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table  style="width: 100%;">
                        <tr>
                            <td class="small" style="width: 153px">
                                Tên website/Tên Shop (<span style="color: #CC0000">*</span>):
                            </td>
                            <td>
                                <asp:TextBox ID="txtWebsiteName" runat="server" CssClass="Textbox" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtWebsiteName"
                                    CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="small" style="width: 123px">
                                Website link (<span style="color: #CC0000">*</span>):
                            </td>
                            <td>
                                <asp:TextBox ID="txtWebsiteLink" runat="server" CssClass="Textbox" MaxLength="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtWebsiteLink"
                                    CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="small" style="width: 123px">
                                Thuộc Website:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddParentId" runat="server" CssClass="Cbo" OnTextChanged="ddParentId_TextChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="small" style="width: 123px">
                                Mô tả:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="Textbox" Height="103px"
                                    TextMode="MultiLine" Width="316px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>

                    <div class="btnLine">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Lưu" CssClass="Button" />
                    <asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click" CssClass="Button" CausesValidation="False" />
                    </div>
            </div>
                   
        </div>
    </div>
</asp:Content>
