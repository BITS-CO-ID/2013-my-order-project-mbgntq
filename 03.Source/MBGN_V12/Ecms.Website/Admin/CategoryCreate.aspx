<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CategoryCreate.aspx.cs" Inherits="Ecms.Website.Admin.Security.CategoryCreate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Label ID="lblFunction" runat="server" Text="Thêm mới"></asp:Label>
    &nbsp;nhóm sản phẩm
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- Start Content Box -->
        <table  style="width: 100%;">
            <tr>
                <td class="small" style="width: 123px">
                    Thông tin chung&nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 123px">
                    &nbsp;Tên nhóm sản phẩm 
                    (<span style="color: #CC0000">*</span>):
                </td>
                <td>
                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="Textbox" 
                        MaxLength="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCategoryName"
                        CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 123px">
                    Loại sản phẩm:
                </td>
                <td>
                    <asp:DropDownList ID="ddtxtParentId" runat="server" CssClass="Cbo">
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
                    <asp:TextBox ID="txtRemark" runat="server" CssClass="Textbox" MaxLength="2000" 
                        TextMode="MultiLine" Height="80px" Width="500px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="small" style="width: 123px">
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
        </table>
    </div>
</asp:Content>
