<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" validateRequest="false" 
    CodeBehind="ProductCreate.aspx.cs" Inherits="Ecms.Website.Admin.ProductCreate" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    <script src="../ckeditor/adapters/jquery.js" type="text/javascript"></script>
    <script src="../ckeditor/ckeditor.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {
           
            CKEDITOR.replace('<%= txtDescription.ClientID %>', { filebrowserImageUploadUrl: 'Upload.ashx' });
        };
    </script>

    <asp:Label ID="lblFunction" runat="server" Text="Thêm mới"></asp:Label>
    &nbsp;sản phẩm
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- Start Content Box -->
        <div class="content-box-content">
                <table style="width:990px;">
                <tr>
                    <td  style="width:120px;">
                        Mã sản phẩm (<span style="color: #CC0000">*</span>):
                    </td>
                    <td style="width:300px;">
                        <asp:TextBox ID="txtProductCode" runat="server" CssClass="Textbox" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width:180px;">
                        Loại sản phẩm (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <asp:DropDownList ID="ddCategoryIdParent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCategoryIdParent_SelectedIndexChanged" CssClass="Cbo">
                            </asp:DropDownList>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tên sản phẩm (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductName" runat="server" CssClass="Textbox" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>
                        Nhóm sản phẩm (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:DropDownList ID="ddCategoryId" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nhà cung cấp:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddVendorId" runat="server" CssClass="Cbo">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Tag:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTag" runat="server" CssClass="Textbox" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Xuất xứ:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOrginal" runat="server" CssClass="Cbo">
                            <asp:ListItem Value="">--Chọn xuất xứ--</asp:ListItem>
                            <asp:ListItem Value="0">Mỹ</asp:ListItem>
                            <asp:ListItem Value="1">Anh</asp:ListItem>
                            <asp:ListItem Value="2">Khác</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Size:
                    </td>
                    <td>
                        <asp:TextBox ID="txtSize" runat="server" CssClass="Textbox" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Cho phép hiển thị:
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbPublished" runat="server" />
                    </td>
                    <td>
                        Sản phẩm đang "Best sale":
                    </td>
                    <td>
                        <asp:CheckBox ID="chkBestSale" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Giá SaleOff:
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductSaleValue" runat="server" CssClass="doubleNumber textbox" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                    </td>
                    <td>
                        Thương hiệu:
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductLable" runat="server" CssClass="Textbox" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Giá sản phẩm (<span style="color: #CC0000">*</span>):
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductValue" runat="server" CssClass="doubleNumber textbox" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                    </td>
                    <td>
                        Tình trạng tồn kho:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStockStatus" runat="server" CssClass="Cbo">
                            <%--<asp:ListItem Value="">--Chọn tình trạng hàng--</asp:ListItem>--%>
                            <asp:ListItem Value="1" Selected="True">Còn hàng</asp:ListItem>
                            <asp:ListItem Value="0">Hết hàng</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        Loại mặt hàng:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProductType" runat="server" CssClass="Cbo">
                            <%--<asp:ListItem Value="">--Chọn tình trạng hàng--</asp:ListItem>--%>
                            <asp:ListItem Value="InStock" Selected="True">Hàng có sẵn trong kho</asp:ListItem>
                            <asp:ListItem Value="Order">Hàng chờ đặt</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Tình trạng hàng:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProductStatus" runat="server" CssClass="Cbo">
                            <%--<asp:ListItem Value="">--Chọn tình trạng hàng--</asp:ListItem>--%>
                            <asp:ListItem Value="1" Selected="True">Hàng mới</asp:ListItem>
                            <asp:ListItem Value="2">Hàng đã qua sử dụng</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                                 
                <tr>
                    <td>
                        Link ảnh online:
                    </td>
                    <td colspan="3">
                        <%--Link ảnh sản phẩm_--%>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtImage" runat="server" Width="450px" AutoPostBack="True" CssClass="textbox"
                                    OnTextChanged="txtImage_TextChanged" MaxLength="200"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        Upload:
                    </td>
                    <td colspan="3">
                        <asp:FileUpload ID="fupImage" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Đăng ảnh" CssClass="Button" CausesValidation="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="3">
                        <%--Hiển thị ảnh sản phẩm--%>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Image ID="imgView" runat="server" Height="200px" Width="200px" ToolTip="ảnh sản phẩm" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txtImageTempOld" runat="server" Width="35px" Visible="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mô tả ngắn gọn:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtShortDescription" runat="server" TextMode="MultiLine" Height="60px" Width="450px"
                            CssClass="cktextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mô tả chi tiết:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                            CssClass="cktextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="  Lưu  " CssClass="Button" OnClientClick="return confirm('Bạn có chắc chắn muốn lưu lại thông tin sản phẩm!');" />
                <asp:Button ID="btnCancel" runat="server" Text="Bỏ qua" OnClick="btnCancel_Click" CssClass="Button" CausesValidation="False" />
            </div>
        </div>
    </div>
</asp:Content>
