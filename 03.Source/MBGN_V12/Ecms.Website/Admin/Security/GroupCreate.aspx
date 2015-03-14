<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="GroupCreate.aspx.cs" Inherits="Ecms.Website.Admin.Security.GroupCreate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    <asp:Literal Text="Thêm" runat="server" ID="litPageTitle" />
    nhóm người dùng
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Literal Text="Thêm" runat="server" ID="litContentTitle" />
    nhóm người dùng
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function Count(text, long) {
            var maxlength = new Number(long);
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                alert(" Chỉ được nhập " + long + " kí tự");
            }
        }
    </script>
    <div class="content-box">
        <!-- End .content-box-header -->
        <div class="content-box-content">
            <div class="" id="tab1">
                <asp:MultiView ID="mtvMain" runat="server" ActiveViewIndex="0">
                    <asp:View ID="formView" runat="server">
                        <table style="width:30%;" >
                            <tr>
                                <td>
                                    Mã nhóm (<span style="color:Red;">*</span>)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGroupCode" runat="server" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tên nhóm
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="Textbox"></asp:TextBox>
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
