<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlideService.ascx.cs"
    Inherits="Ecms.Website.Site.PartControl.SlideService" %>
<%--<div class="clients-skin-carousel">
    <div class="jcarousel-container jcarousel-container-horizontal">
        <div class="jcarousel-clip jcarousel-clip-horizontal">
            <ul class="clients-carousel jcarousel-list jcarousel-list-horizontal">
                <asp:Literal ID="litService" runat="server" />
            </ul>
        </div>
        <div disabled="disabled" style="display: block;" class="jcarousel-prev jcarousel-prev-horizontal jcarousel-prev-disabled jcarousel-prev-disabled-horizontal">
        </div>
        <div style="display: block;" class="jcarousel-next jcarousel-next-horizontal">
        </div>
    </div>
</div>--%>
<ul class="service-box" style="display:none;">
    <li>
        <div id="service-item-box-left" >
            <div class="item-image">
                <a href="/site/mbgn/NewsDetail.aspx?NewsId=43">
                    <img alt="Vận chuyển hàng hóa" src="/Content/images/vanchuyen12.png" width="100%" /></a>
            </div>
            <div class="item-text">
                <a href="/site/mbgn/NewsDetail.aspx?NewsId=43">Vận chuyển hàng hóa</a>
            </div>
        </div>
    </li>
    <li>
        <div id ="service-item-box-center" >
            <div class="item-image">
                <a href="/site/mbgn/NewsDetail.aspx?NewsId=6">
                    <img alt="Giao nhận hàng hóa" src="/Content/images/giaonhan12.png" width="100%"/></a>
            </div>
            <div class="item-text">
                <a href="/site/mbgn/NewsDetail.aspx?NewsId=6">Giao nhận hàng hóa</a>
            </div>
        </div>
    </li>
    <li>
        <div id="service-item-box-right" >
            <div class="item-image">
                <a href="/site/mbgn/NewsDetail.aspx?NewsId=44">
                    <img alt="Đặt hàng - mua hộ hàng hóa" src="/Content/images/muaho1.png" width="100%"/></a>
            </div>
            <div class="item-text">
                <a href="/site/mbgn/NewsDetail.aspx?NewsId=44">Đặt hàng - mua hộ hàng hóa</a>
            </div>
        </div>
    </li>
</ul>

<%--<div class="svbox">
    <div class="col1">
        <h4>Dịch vụ vận chuyển hàng hóa</h4>
    </div>

    <div class="col2">
        <h4>Dịch vụ giao nhận hàng hóa</h4>
    </div>

    <div class="col3">
        <h4>Dịch vụ Mua hàng hộ</h4>
    </div>
</div>--%>