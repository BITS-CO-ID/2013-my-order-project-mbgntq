<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftNavigation.ascx.cs"
    Inherits="Ecms.Website.Admin.PartControl.LeftNavigation" %>
<div class="accordion">
    <h3 class="accordion-first">
        Quản lý đơn hàng</h3>
    <div>
        <ul>
            <li><a href="/admin/order/ordermanage.aspx">Theo dõi đơn hàng</a></li>
            <li><a href="/admin/order/grouplink.aspx">Gom link</a></li>
            <li><a href="/admin/order/orderoutboundmanage.aspx">Theo dõi đơn hàng nước ngoài</a></li>
            <li><a href="/admin/order/orderdetailstatusupdate.aspx">Cập nhật thông tin món hàng</a></li>
        </ul>
    </div>
    <h3>
        Quản lý thiết lập chính sách</h3>
    <div>
        <ul>
            <li><a href="/admin/ConfigList.aspx">Quản lý thiết lập chính sách</a></li>
        </ul>
    </div>
    <h3>
        Quản lý danh mục</h3>
    <div>
        <ul>
            <li><a href="/admin/security/grouplist.aspx">Quản lý nhóm người dùng</a></li>
            <li><a href="/admin/security/userlist.aspx">Quản lý người dùng</a></li>
            <li><a href="/admin/websitelinklist.aspx">Quản lý TT website/Shop</a></li>
            <li><a href="/admin/websiteaccountList.aspx">Quản lý tài khoản website</a></li>
            <li><a href="/admin/visaaccountlist.aspx">Quản lý tài khoản TT</a></li>
            <%--<li><a href="/admin/newslist.aspx">Quản trị nội dung</a></li>--%>
        </ul>
    </div>
    <h3>
        Quản lý khách hàng</h3>
    <div>
        <ul>
            <li><a href="/admin/customertypelist.aspx">Quản lý nhóm KH</a></li>
            <li><a href="/admin/customerlist.aspx">Quản lý thông tin KH</a></li>
            <%--<li><a href="/admin/complaintlist.aspx">Quản lý ý kiến KH</a></li>--%>
        </ul>
    </div>
    <%--<h3>
        Quản lý thông tin sản phẩm</h3>
    <div>
        <ul>
            <li><a href="/admin/categorylist.aspx">Quản lý nhóm sản phẩm</a></li>
            <li><a href="/admin/productlist.aspx">Quản lý sản phẩm</a></li>
        </ul>
    </div>--%>
    <%--<h3>
        Quản lý kho</h3>
    <div>
        <ul>
            <li><a href="/admin/importstockmanage.aspx">Quản lý nhập kho</a></li>
        </ul>
    </div>--%>
    <h3>
        Quản lý Thanh toán</h3>
    <div>
        <ul>
            <li><a href="/admin/order/paymentmanage.aspx">Quản lý thanh toán</a></li>
            <li><a href="/admin/order/payment.aspx">Lập hóa đơn thanh toán</a></li>
        </ul>
    </div>
    <h3>
        Báo cáo</h3>
    <div>
        <ul>
            <li><a href="/admin/report/ReportGoodDeliverly.aspx">[1] - BC tình trạng món hàng</a></li>
            <%--<li><a href="/admin/report/ReportGoodDeliverlyCPrintMng.aspx">[2] - Quản lý xác nhận In phiếu GN</a></li>--%>
            <%--<li><a href="/admin/report/ReportTransport.aspx">[3] - BC TH đơn hàng vận chuyển</a></li>
            <li><a href="/admin/report/ReportOrderBuy.aspx">[4] - BC TH đơn hàng mua hộ</a></li>--%>
            <%--<li><a href="/admin/report/ReportByProduct.aspx">[5] - BC TH đơn hàng sản phẩm</a></li>--%>
            <%--<li><a href="/admin/report/ReportOutbound.aspx">[6] - BC TH đơn hàng nước ngoài</a></li> --%>           
            <%--<li><a href="/admin/report/reportemployee.aspx">[7] - BC TH tài khoản Nhân viên</a></li>--%>
            <li><a href="/admin/report/reportliability.aspx">[2] - BC công nợ Khách hàng</a></li>
            <li><a href="/admin/report/ReportOrderDebit.aspx">[3] - BC công nợ Đơn hàng</a></li>
            <%--<li><a href="/admin/report/ReportStockOInOut.aspx">[10] - BC TH Nhập xuất tồn kho</a></li>--%>
        </ul>
    </div>
    <h3>
        Thông tin khác</h3>
    <div>
        <ul>
            <li><a href="/admin/about.aspx">About</a></li>
        </ul>
    </div>
</div>
