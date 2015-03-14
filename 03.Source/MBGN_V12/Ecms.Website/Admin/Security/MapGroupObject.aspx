<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="MapGroupObject.aspx.cs" Inherits="Ecms.Website.Admin.Security.MapGroupObject" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Gán nhóm chức năng
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
                                    Tick chọn(bỏ tick chọn) để gán chức năng(bỏ chức năng) khỏi nhóm.
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
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="RadTreeList">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="RadTreeList"></telerik:AjaxUpdatedControl>
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadTreeList ID="RadTreeList" runat="server" DataKeyNames="ObjectCode" ParentDataKeyNames="ParentObjectCode"
                                AutoGenerateColumns="false" OnNeedDataSource="RadTreeView1_NeedDataSource" AllowMultiItemSelection="true"
                                OnDataBound="RadTreeList_DataBound" >
                                <SelectedItemStyle Font-Bold="true" ForeColor="White" BackColor="Black" />
                                <Columns>
                                    <telerik:TreeListBoundColumn DataField="ObjectName" UniqueName="ObjectName" HeaderText="Tên chức năng">
                                    </telerik:TreeListBoundColumn>
                                    <telerik:TreeListBoundColumn DataField="ObjectCode" UniqueName="ObjectCode" HeaderText="Mã chức năng">
                                    </telerik:TreeListBoundColumn>
                                    <telerik:TreeListSelectColumn ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="50" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:TreeListSelectColumn>
                                </Columns>
                                <ClientSettings>
                                    <ClientEvents OnItemSelected="OnClientNodeClicked" OnItemDeselected="OnClientNodeClicked" />
                                    <Selecting AllowItemSelection="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="400" />
                                </ClientSettings>
                            </telerik:RadTreeList>
                        </div>
                        <table>
                            <tr>
                                <asp:Literal runat="server" ID="MessageLtr"></asp:Literal>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnConfirm" runat="server" Text="Xác nhận" CssClass="Button" OnClick="btnConfirm_Click"
                                        OnClientClick='return confirm("Bạn có chắc chắn muốn lưu gán nhóm chức năng?");' />
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
                                    <asp:Label ID="lblResult" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Lưu gán nhóm chức năng thành công"></asp:Label>
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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var parenItemSelected = false;
            function OnClientNodeClicked(sender, args) {
                var currNode = args.get_item();
                var childNodes = currNode.get_childItems();
                var nodeCount = currNode.get_childItems().length;
                var parentItem = currNode.get_parentItem();
                if (parentItem) {
                    parenItemSelected = true;
                    parentItem.set_selected(true);
                }
                if (currNode.get_selected()) {
                    CheckAllChildren(childNodes, nodeCount);
                }
                else {
                    UnCheckAllChildren(currNode, childNodes, nodeCount);
                }
                parenItemSelected = false;
            }

            function UnCheckAllChildren(currNode, nodes, nodecount) {
                var i;
                for (i = 0; i < nodecount; i++) {
                    nodes[i].set_selected(false);
                }
                currNode.set_selected(false);
            }

            function CheckAllChildren(nodes, nodecount) {
                var i;
                if (!parenItemSelected) {
                    for (i = 0; i < nodecount; i++) {
                        nodes[i].set_selected(true);
                    }
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
