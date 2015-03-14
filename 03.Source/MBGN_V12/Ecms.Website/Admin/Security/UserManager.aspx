<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="Ecms.Website.Admin.Security.UserManager" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Hồ sơ user
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <!-- Start Content Box -->
        <div class="content-box-header">
            <h3>
                Quản lý thành viên</h3>
            <ul class="content-box-tabs">
                <li><a href="#tab1" class="default-tab">Danh sách thành viên</a></li>
            </ul>
            <div class="clear">
            </div>
        </div>
        <!-- End .content-box-header -->
        <div class="content-box-content">
            <div class="tab-content default-tab" id="tab1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="HeaderFunction">
                            <asp:Button runat="server" CssClass="button" Text="Cập nhật danh sách" 
                                ID="ReloadGrid" onclick="ReloadGrid_Click" /></div>
                        <asp:GridView runat="server" ID="UserGrid" CssClass="FullGrid" AutoGenerateColumns="false" DataKeyNames="UserName">
                            <Columns>
                                <asp:TemplateField HeaderText="Tên đăng nhập">
                                    <ItemTemplate>
                                        <asp:Label ID="userNameLbl" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="userNameTxt" runat="server" Text='<%# Bind("Username") %>' CssClass="text-input small-input"
                                            Enabled="false"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1121" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="emailTxt" runat="server" Text='<%# Bind("Email") %>' CssClass="text-input small-input"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tác vụ">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                            Text="Cập nhật" CssClass="button"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="Hủy" CssClass="button"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                            Text="Sửa nhanh" CssClass="button"></asp:LinkButton>
                                  &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False"
                                                CommandName="Delete" Text="Xóa" CssClass="button" OnClientClick='return confirm("Bạn có chắc chắn muốn xóa?");'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <h3 class="NoData">
                                    Không có thành viên nào!</h3>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:Literal runat="server" ID="MessageLtr"></asp:Literal>
                        <div class="LoginWrapper">
                            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" FinishDestinationPageUrl="~/vi-VN/Home.html"
                                CreateUserButtonText="Tạo tài khoản" DuplicateEmailErrorMessage="Email đã được sử dụng ! Vui lòng nhập Email khác !."
                                DuplicateUserNameErrorMessage="Tên đăng nhập đã tồn tại hoặc không hợp lệ !"
                                InvalidAnswerErrorMessage="Nhập câu trả lời khác !" InvalidEmailErrorMessage="Nhập Email hợp lệ !"
                                InvalidPasswordErrorMessage="Độ dài mật khẩu nhỏ nhất : {0}. Yêu cầu kí tự khác kí tự chữ cái : {1}."
                                InvalidQuestionErrorMessage="Nhập câu hỏi bí mật khác !" UnknownErrorMessage="Tài khoản của bạn không được tạo! Thử lại !"
                                LoginCreatedUser="false" >
                                <WizardSteps>
                                    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                        <ContentTemplate>
                                            <table >
                                                <caption>
                                                    <h3 class="RegisterTitle">
                                                        Thông tin đăng nhập</h3>
                                                </caption>
                                                <tr>
                                                    <td align="right" class="td">
                                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">                                                 
                                                            Tên đăng nhập
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="UserName" runat="server" CssClass="text-input small-input"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                            ErrorMessage="*" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="td">
                                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                                                            Mật khẩu
                                                          
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="text-input small-input"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                            ErrorMessage="*" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="td">
                                                        <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">
                                                            Xác nhận mật khẩu                                                           
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="text-input small-input"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                            ErrorMessage="*" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="td">
                                                        <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">Email</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Email" runat="server" CssClass="text-input small-input"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                                            ErrorMessage="*" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                            ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="*" ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" style="color: Red;">
                                                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <CustomNavigationTemplate>
                                            <div class="Command">
                                                <asp:Button ID="CreateUserButton" runat="server" Text="Tạo tài khoản" ValidationGroup="CreateUserWizard1"
                                                    CssClass="button" CommandName="MoveNext" />
                                            </div>
                                        </CustomNavigationTemplate>
                                    </asp:CreateUserWizardStep>
                                    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                                        <ContentTemplate>
                                            <table cellspacing="10px" cellpadding="10px">
                                                <tr>
                                                    <td align="center">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Tạo tài khoản thành công !
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:CompleteWizardStep>
                                </WizardSteps>
                                <FinishNavigationTemplate>
                                    <div class="Command">
                                        <asp:Button ID="ConfirmButton" runat="server" CausesValidation="true" CommandName="MovePrevious"
                                            CssClass="RegisterButton" Text="Xác nhận" />
                                        <asp:Button ID="CancelButton" runat="server" CommandName="MoveComplete" Text="Bỏ qua"
                                            CausesValidation="false" CssClass="RegisterButton" />
                                    </div>
                                </FinishNavigationTemplate>
                            </asp:CreateUserWizard>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- End #tab1 -->
        </div>
        <!-- End .content-box-content -->
    </div>
</asp:Content>
