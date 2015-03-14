<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="MapGroupUsers.aspx.cs" Inherits="Ecms.Website.Admin.Security.MapGroupUsers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Gán nhóm người dùng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="content-box">
        <div class="content-box-content">
            <div class="" id="tab1">
                <asp:MultiView ID="mtvMain" runat="server" ActiveViewIndex="0">
                    <asp:View ID="formView" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <strong>
                                        <asp:Label ID="lblGroupCode" runat="server"></asp:Label></strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tick chọn(bỏ tick chọn) để gán user(bỏ user) khỏi nhóm.
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" DataKeyNames="UserCode"
                                CssClass="gridview" OnPageIndexChanging="grdD_PageIndexChanging" Width="100%"
                                AllowPaging="true" PageSize="15">
                                <Columns>
                                    <asp:TemplateField HeaderText="STT">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="50" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserCode" HeaderText="Mã người dùng" />
                                    <asp:BoundField DataField="UserName" HeaderText="Tên người dùng" />
                                    <asp:TemplateField HeaderText="Chọn">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="checkBox" Checked='<%# checkIsMapGroupSecurity(Eval("UserCode").ToString()) %>'
                                                EnableViewState="true" />
                                        </ItemTemplate>
                                        <ItemStyle Width="50" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <table>
                            <tr>
                                <asp:Literal runat="server" ID="MessageLtr"></asp:Literal>
                                <caption>
                                    <br />
                                </caption>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnConfirm" runat="server" Text="Xác nhận" CssClass="Button" OnClick="btnConfirm_Click"
                                        OnClientClick='return confirm("Bạn có chắc chắn muốn lưu gán nhóm người dùng?");' />
                                </td>
                                <td>
                                    <asp:Button ID="btnReturn" runat="server" Text="Quay lại" CssClass="Button" OnClick="btnReturn_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="resultView" runat="server">
                        <table align="center" style="width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Lưu gán nhóm người dùng thành công"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" CssClass="Button" OnClick="btnOK_Click" Text="  OK  " />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>
