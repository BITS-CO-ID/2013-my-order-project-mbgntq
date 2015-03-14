<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftNavigation2.ascx.cs"
    Inherits="Ecms.Website.Site.PartControl.LeftNavigation2" %>
<div class="widget">
    <div id="menu-wrapper">
        <h2>
            Tiện ích Online</h2>
        <ul class="nav">
            <%--<li><a href='<%= ResolveUrl("~/site/mbgn/Quotation.aspx") %>'>Yêu cầu báo giá</a></li>--%>
            <li><a href='<%= ResolveUrl("~/site/mbgn/OrderByLink.aspx") %>'>Đặt hàng mua hộ</a></li>
            <li><a href='<%= ResolveUrl("~/site/mbgn/orderbylinkupload.aspx") %>'>Upload đơn hàng mua hộ</a></li>
            <li><a href='<%= ResolveUrl("~/site/mbgn/OrderTransport.aspx") %>'>Gửi hàng vận chuyển</a></li>
            <li><a href='<%= ResolveUrl("~/site/mbgn/OrderProduct.aspx")%>' title="">Quản lý đơn hàng</a></li>
            <li><a href='<%= ResolveUrl("~/site/mbgn/InvoiceManage.aspx")%>' title="">Quản lý thanh toán</a></li> 
        </ul>
    </div>	
</div> 
<div class="widget support">
		<asp:Literal ID="litCustomerLoginFunction" runat="server" />
</div>	 