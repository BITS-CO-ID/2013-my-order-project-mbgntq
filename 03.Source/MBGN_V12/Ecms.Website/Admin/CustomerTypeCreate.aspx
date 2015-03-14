<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerTypeCreate.aspx.cs" Inherits="Ecms.Website.Admin.Security.CustomerTypeCreate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Label ID="lblFunction" runat="server" Text="Thêm "></asp:Label>
    nhóm khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- Start Content Box -->
        <table  style="width: 100%;">
            <tr>
                <td class="small" style="width: 150px">
                        Thông tin chung
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 150px">
                    Tên nhóm khách hàng(<span style="color: #CC0000">*</span>)</td>
                <td>
                    &nbsp;
                    <asp:TextBox ID="txtCustomerTypeName" runat="server" CssClass="Textbox" 
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCustomerTypeName"
                        CssClass="validate" ErrorMessage="Không được để trống">Không được để trống</asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 150px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" colspan="2">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Lưu" CssClass="Button" />
                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click"
                        CssClass="Button" CausesValidation="False" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 150px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 150px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
