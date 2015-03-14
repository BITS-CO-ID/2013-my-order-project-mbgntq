<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Feedback.ascx.cs" Inherits="Ecms.Website.Site.PartControl.Feedback" %>
<fieldset>
    <legend>Feedback</legend>        
            <asp:MultiView ActiveViewIndex="0" runat="server" ID="mtvMain">
                <asp:View runat="server" ID="formView">
                    <table style="width: 100%;" class="tableForm">
                        <tr>
                            <td>
                                Họ và tên bạn <span class="required">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="textbox txtFullName"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Địa chỉ Email <span class="required">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox txtEmail"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tiêu đề thông điệp <span class="required">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="textbox txtTitleFeedback"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nội dung thông điệp <span class="required">*</span>
                            </td>
                            <td rowspan="5">
                                <asp:TextBox ID="txtContent" runat="server" CssClass="textbox txtContentFeedback"
                                    Height="100px" TextMode="MultiLine" Width="445px"></asp:TextBox>
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
                        <tr>
                            <td>
                                <asp:Button ID="btnSend" runat="server" CssClass="button" Text="Gửi" OnClick="btnSend_Click" />
                                &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button cancel" Text="Hủy bỏ"
                                    OnClick="btnCancel_Click" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View runat="server" ID="resultView">
                    <table style="width: 80%;" align="center">
                        <tr>
                            <td colspan="2">
                                Cảm ơn quý khách đã gửi thông tin đến <span style="color: #014586;">QuangChau247</span>.
                                Chúng tôi sẽ liên hệ với quý khách trong thời gian sớm nhất.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button Text="Về trang chủ" runat="server" ID="btnOK" OnClick="btnOK_Click" CssClass="button cancel" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
</fieldset>
