<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master"  validateRequest="false" 
 AutoEventWireup="true" CodeBehind="Complaints.aspx.cs" Inherits="Ecms.Website.Site.MBGN.Complaints" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <script src="../../ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../../ckeditor/adapters/jquery.js" type="text/javascript"></script>
      <script type="text/javascript">
          window.onload = function () {
              CKEDITOR.replace('<%= txtContent.ClientID %>', {filebrowserImageUploadUrl: 'Upload.ashx' });
          };
    </script>
    <fieldset style="border: 1px solid #cdcdcd;">
    <table style="width: 100%;" align="center" class="tableForm">
                <tr>
                    <td>
                        <strong>Ý kiến phản hồi:&nbsp;</strong></td>
                </tr>
                <tr>
                    <td>
                         <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
                    Width="100%" AllowPaging="True" PageSize="5" OnPageIndexChanging="grdD_PageIndexChanging"
                    OnRowDeleting="grdD_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="CreatedDate" HeaderText="Ngày tạo">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" HorizontalAlign="Left" />
                        <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Title" HeaderText="Tiêu đề">
                            <ControlStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Tình trạng">
                            <ControlStyle Width="50px" />
                        <HeaderStyle Width="50px" />
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastModifyDate" HeaderText="Cập nhập cuối" >
                        <HeaderStyle Width="60px" />
                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%#Eval("Id") %>'
                                    OnCommand="lbtEdit_Click" CommandName="Edit">Xem chi tiết</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("Id") %>'
                                    CommandName="Delete" OnCommand="lbtDelete_Click" OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa không?')) return false;">Xóa</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                </table> 
                 <table style="width: 100%;" align="center" class="tableForm">
                <tr>
                    <td>
                       
                             <asp:Button ID="btnAddnew" runat="server" CssClass="button" 
                            Text="Thêm phản ánh" onclick="btnAddnew_Click" />
                           
                    </td>
                </tr>
                <tr>
                    <td>
                    <%--////////////////////////--%>
                    <hr />
                        <asp:Panel ID="pnAddNew"  Visible="False" runat="server" BorderStyle="None">
                            Tiêu đề (<span style="color: #CC0000">*</span>):<br />
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtTitle" CssClass="validate" 
                                ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                            <br />
                            <br />
                            Nội dung (<span style="color: #CC0000">*</span>):<br />
                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" 
                                CssClass="cktextbox" ></asp:TextBox>
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtTitle" CssClass="validate" 
                                ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                            <br />
                            <asp:Button ID="btnSave" CssClass="button green"  runat="server" Text="Gửi" 
                                onclick="btnSave_Click" />
                            <asp:Button ID="btnCancel" CssClass="button"  runat="server" Text="Hủy bỏ" 
                                onclick="btnCancel_Click" CausesValidation="False" />
                        </asp:Panel>

                   <%--////////////////////////--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" CssClass="indend" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
</asp:Content>
