<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="NewsCreate.aspx.cs" Inherits="Ecms.Website.Admin.NewsCreate" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <script src="../ckeditor/adapters/jquery.js" type="text/javascript"></script>
    <script src="../ckeditor/ckeditor.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {
            CKEDITOR.replace('<%= txtShortContentCK.ClientID %>', { filebrowserImageUploadUrl: 'Upload.ashx' });
            CKEDITOR.replace('<%= txtNewsContentCK.ClientID %>', { filebrowserImageUploadUrl: 'Upload.ashx' });
        };
    </script>
    <asp:Label ID="lblFunction" runat="server" Text="Thêm mới"></asp:Label>
    &nbsp;nội dung
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- Start Content Box -->
        <div class="content-box-content">
            <table  style="width: 100%;">
                <tr>
                    <td class="small" style="width: 151px">
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
                    <td class="small" style="width: 151px">
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
                    <td class="small" style="width: 151px">
                        Tiêu đề (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="400px" MaxLength="500"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtTitle"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table  style="width: 100%;">
                        <tr>
                            <td class="small" style="width: 151px">
                                Loại bài viết (<span style="color: #CC0000">*</span>):
                            </td>
                            <td>
                                <asp:DropDownList ID="ddType" runat="server" CssClass="Cbo" AutoPostBack="True" OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">-- Chọn loại bài viết --</asp:ListItem>
                                    <asp:ListItem Value="0">Các dịch vụ</asp:ListItem>
                                    <asp:ListItem Value="1">Sale nóng</asp:ListItem>
                                    <asp:ListItem Value="2">Feedback</asp:ListItem>
                                    <asp:ListItem Value="3">Bài viết</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="small" style="width: 151px">
                                Website
                            </td>
                            <td>
                                <asp:DropDownList ID="ddWebsiteId" runat="server" CssClass="Cbo" Enabled="False">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table  style="width: 100%;">
                <tr>
                    <td class="small" style="width: 151px">
                        Hiển thị:
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbPublished" runat="server" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 151px">
                        Link ảnh đại diện:
                    </td>
                    <td>
                        <%--Hiển thị ảnh sản phẩm--%>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtImage" runat="server" Width="400px" AutoPostBack="True" CssClass="textbox"
                                    OnTextChanged="txtImage_TextChanged" MaxLength="200"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 151px">
                        Đăng ảnh đại diện:
                    </td>
                    <td>
                        <asp:FileUpload ID="fupImage" runat="server" />
                        &nbsp;<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Đăng ảnh"
                            CssClass="Button" CausesValidation="False" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 151px">
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
                    <td class="small" style="width: 151px">
                        &nbsp;
                    </td>
                    <td>
                        <%--Hiển thị ảnh sản phẩm--%>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Image ID="imgView" runat="server" Height="200px" Width="200px" ToolTip="ảnh sản phẩm" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txtImageTempOld" runat="server" Width="35px" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 151px">
                        Nội dung ngắn (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtShortContentCK" runat="server" TextMode="MultiLine" 
                            CssClass="cktextbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtTitle"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                        <br />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 151px">
                        Nội dung (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewsContentCK" runat="server" TextMode="MultiLine" 
                            CssClass="cktextbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtTitle"
                            CssClass="validate" ErrorMessage="Không được để trống"></asp:RequiredFieldValidator>
                        <br />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="small" style="width: 151px">
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
                    <td class="small" style="width: 151px">
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
                    <td class="small" style="width: 151px">
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
                    <td class="small" style="width: 151px">
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
    </div>
</asp:Content>
