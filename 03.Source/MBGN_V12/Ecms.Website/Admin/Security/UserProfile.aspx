<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="UserProfile.aspx.cs" Inherits="Ecms.Website.Admin.Security.UserProfile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Thông tin chi tiết người dùng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table width="50%" >
        <tr>
            <td style="width: 150px;">
                <strong>Tên đăng nhập </strong>
            </td>
            <td>
                <asp:Label ID="lblUserCode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Họ và tên </strong>
            </td>
            <td>
                <asp:Label ID="lblUserName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Email </strong>
            </td>
            <td>
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <fieldset>
        <legend>Các nhóm của người dùng </legend>
        <ul>
            <asp:Repeater ID="rptGroup" runat="server">
                <ItemTemplate>
                    <li>
                        <%# Eval("GroupName")%>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </fieldset>
    <fieldset>
        <legend>Các quyền của người dùng</legend>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="treeListMain">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="treeListMain"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadTreeList ID="treeListMain" runat="server" DataKeyNames="ObjectCode" ParentDataKeyNames="ParentObjectCode"
            AutoGenerateColumns="false" OnNeedDataSource="RadTreeView1_NeedDataSource" AllowMultiItemSelection="true">
            <SelectedItemStyle Font-Bold="true" ForeColor="White" BackColor="Black" />
            <Columns>
                <telerik:TreeListBoundColumn DataField="ObjectName" UniqueName="ObjectName" HeaderText="Tên chức năng">
                </telerik:TreeListBoundColumn>
                <telerik:TreeListBoundColumn DataField="ObjectCode" UniqueName="ObjectCode" HeaderText="Mã chức năng">
                </telerik:TreeListBoundColumn>
            </Columns>
            <ClientSettings>
                <ClientEvents OnItemSelected="OnClientNodeClicked" OnItemDeselected="OnClientNodeClicked" />
                <Selecting AllowItemSelection="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="400" />
            </ClientSettings>
        </telerik:RadTreeList>
    </fieldset>
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
