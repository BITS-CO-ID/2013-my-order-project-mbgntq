<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerEdit.aspx.cs" Inherits="Ecms.Website.Admin.CustomerEdit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Label ID="lblFunction" runat="server" Text="Thêm mới"></asp:Label>
    &nbsp;thông tin khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <!-- Start Content Box -->
        <div class="content-box-content">
            <table style="width: 100%;">
                <tr>
                    <td class="small" style="width: 150px">
                        Thông tin chung
                    </td>
                    <td>
                        &nbsp;
                    </td>                    
                </tr>
                <tr>
                    <td class="small">
                        Mã khách hàng:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 123px">
                        Tên khách hàng(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="textbox" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCustomerName"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 123px">
                        Địa chỉ(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 123px">
                        Email(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 123px">
                        Số điện thoại(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMobile"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 123px">
                        Tỉnh/Thành phố(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:DropDownList ID="ddCityId" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddCityId"
                            CssClass="validate" ErrorMessage="Bạn chưa chọn Tỉnh/Thành phố"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 123px">
                        Nhóm khách hàng(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:DropDownList ID="ddCustomerTypeId" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddCustomerTypeId"
                            CssClass="validate" ErrorMessage="Bạn chưa chọn nhóm khách hàng"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <%--<tr>
                    <td class="small" style="width: 123px">
                        Số hiệu khách hàng(<span style="color: #CC0000">*</span>)</td>
                    <td>
                        <asp:TextBox ID="txtCustomerCodeDelivery" runat="server" CssClass="textbox" 
                            MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtCustomerCodeDelivery"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                    </td>
                </tr>--%>
                <tr>
                    <td class="small" style="width: 123px">
                        Người bán hàng(<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:DropDownList ID="dllEmployee" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="small" colspan="2">
                        &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>
                    </td>
                </tr>
                </table>
                 <div class="btnLine">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Lưu" CssClass="Button" />
                    <asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click" CssClass="Button" CausesValidation="False" />
                </div>
        </div>
       
</asp:Content>
