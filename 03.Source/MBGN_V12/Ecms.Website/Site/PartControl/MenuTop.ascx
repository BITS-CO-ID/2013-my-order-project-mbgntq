<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuTop.ascx.cs" Inherits="Ecms.Website.Site.PartControl.MenuTop" %>
<div class="span8">
    <!-- Main Navigation -->
    <nav class="main-nav">
        <ul class="sf-menu" id="nav">
            <%--<li class="current"><a href='<%= ResolveUrl("~/site/default.aspx") %>'>Trang chủ</a></li>--%>
            <asp:literal ID="litMenu" runat="server" />
        </ul>              
    </nav>
    <!-- END Main Navigation -->
</div>
<div class="span4">
    <div id="search">
        <input class="txt-search" type="text" placeholder="Tìm kiếm sản phẩm" />
        <input class="button" type="button" value="Tìm kiếm" />
    </div>
</div>
