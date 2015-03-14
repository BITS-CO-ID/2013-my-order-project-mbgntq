<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="VisaAccountCreate.aspx.cs" Inherits="Ecms.Website.Admin.Security.VisaAccountCreate" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <asp:Label ID="lblFunction" runat="server" Text="Thêm "></asp:Label>
    TK visa
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
        <asp:View ID="fromView" runat="server">
        <!-- Start Content Box -->
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
                <td class="small" style="width: 150px">
                    Số TK Thanh toán(<span style="color: #CC0000">*</span>)</td>
                <td>                   
                    <asp:TextBox ID="txtVisaNo" runat="server" CssClass="Textbox" 
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtVisaNo"
                        CssClass="validate" ErrorMessage="Không được để trống">Không được để trống</asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td class="small" style="width: 150px">
                    Ghi chú:</td>
                <td>                   
                    <asp:TextBox ID="txtRemark" runat="server" CssClass="Textbox" MaxLength="50"></asp:TextBox>
                </td>                
            </tr>

            <tr>
                <td class="small" style="width: 150px">
                    
                </td>
                <td>                    
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
                
            </tr>                   
            
        </table>
        <div class="btnLine">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Lưu" CssClass="Button" />
            <asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click" CssClass="Button" CausesValidation="False" />
            <asp:HiddenField ID="hdnVisaNo" runat="server" />
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
                        <asp:Button ID="btnOK" runat="server" CssClass="Button" Text="OK" OnClick="btnOK_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
        </asp:MultiView>
</asp:Content>
