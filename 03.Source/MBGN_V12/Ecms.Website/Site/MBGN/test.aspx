<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Ecms.Website.Site.MBGN.test" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<style type= "text/css">  
        div.cbbNew .rcbInputCell INPUT.rcbInputCell  
        {  
            height: 30px;
            line-height: 30px;
        }  
</style> 

    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManageMain" runat="server">
    </asp:ScriptManager>
    <div>
        <telerik:RadComboBox ID="cbbCombobox" runat="server" style="height:30px;" CssClass="cbbNew"
        CausesValidation="false"
        EmptyMessage="chọn Name"
        AllowCustomText="true"
        MarkFirstMatch="true">        
        </telerik:RadComboBox>
        
        <%--<div>
        <cc:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc:ToolkitScriptManager>
        <cc:ComboBox ID="cbb" runat="server" AutoCompleteMode="SuggestAppend" 
            DropDownStyle="Simple">
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>222</asp:ListItem>
            <asp:ListItem>3333</asp:ListItem>
        
        </cc:ComboBox>
        </div>--%>
    </div>
    </form>
</body>
</html>
