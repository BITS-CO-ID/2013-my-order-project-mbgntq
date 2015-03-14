<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="UserCreate.aspx.cs" Inherits="Ecms.Website.Admin.Security.UserCreate" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Literal Text="Thêm mới" ID="litFunction" runat="server" />
    người dùng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function Count(text, long) {
            var maxlength = new Number(long);
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                alert(" Chỉ được nhập " + long + " kí tự");
            }
        }

        var isShift = false;
        function keyUP(keyCode) {
            if (keyCode == 16)
                isShift = false;
        }

        function isAlphaNumeric(keyCode) {
            if (keyCode == 16)
                isShift = true;

            return (((keyCode >= 48 && keyCode <= 57) && isShift == false) ||
               (keyCode >= 65 && keyCode <= 90) || keyCode == 8 ||
               (keyCode >= 96 && keyCode <= 105))
        }                                                                               
    </script>
    <div class="content-box">
        <!-- End .content-box-header -->
        <div class="content-box-content">
            <div class="" id="tab1">
                <asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
                    <asp:View ID="formView" runat="server">
                        <table width="50%" >
                            <tr>
                                <td style="width: 150px;">
                                    Tên đăng nhập (<span style="color:Red;">*</span>)</td>
                                <td>
                                    <asp:TextBox ID="txtUserCode" runat="server" CssClass="Textbox" onkeyup="keyUP(event.keyCode)"
                                        onkeydown="return isAlphaNumeric(event.keyCode);" onpaste="return false;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Họ và tên</td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trPassword" runat="server">
                                <td>
                                    Mật khẩu (<span style="color:Red;">*</span>)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trConfirmPassword" runat="server">
                                <td>
                                    Xác nhận mật khẩu (<span style="color:Red;">*</span>)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirm" runat="server" CssClass="Textbox" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email  (<span style="color:Red;">*</span>)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mô tả
                                </td>
                                <td rowspan="1">
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" onKeyUp="Count(this,160)"
                                        onChange="Count(this,160)" Height="89px" Width="328px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Trạng thái kích hoạt:
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkStatus" runat="server" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Supper admin:
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSupperAdmin" runat="server" Checked="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="btnLine">
                            <asp:Button ID="btnSave" runat="server" Text="Xác nhận" CssClass="Button" OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Hủy bỏ" CssClass="Button" OnClick="btnCancel_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="resultView" runat="server">
                        <table align="center" style="width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" CssClass="Button" OnClick="btnOK_Click" Text="OK" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>
