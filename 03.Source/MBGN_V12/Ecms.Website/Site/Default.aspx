<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Ecms.Website.Site.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Vận chuyển - mua hộ hàng taobao.com - tmall.com - alibaba.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="view-product">
        <div class="head_row" style="display: none;">
            <h4 class="prd">
                Sale Nóng</h4>
            <div class="right_content">
            </div>
        </div>
        <asp:Repeater ID="rptSaleHot" runat="server" Visible="false">
            <HeaderTemplate>
                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="joomlatable">
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <img alt="Link" src='<%= ResolveUrl("~/Content/Images/icons/weblink.png")%>'>&nbsp;&nbsp;
                        <a class="category" href='<%= ResolveUrl("~/site/mbgn/OrderByLink.aspx?WebsiteId=") %><%# DataBinder.Eval(Container, "DataItem.WebsiteId")%>'>
                            <%# DataBinder.Eval(Container, "DataItem.Title")%>
                        </a>
                        <br>
                        <p class="description indend">
                            <%# DataBinder.Eval(Container, "DataItem.NewsContent")%>
                        </p>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="clearfix">
        </div>
        <div class="pagination" style="display: none;">
            <asp:Literal ID="litPager" runat="server" />
        </div>
    </div>
    <div class="clearfix">
    </div>
    <div class="list-product">
        <h2>Hàng nhập khẩu Trung Quốc</h2>
        <div class="product-list-wrapper">
            <ul class="product-list nolist clearfix">
                <asp:Literal ID="litProductBox" runat="server" />
            </ul>
            <div class="pagination">
            <asp:Literal ID="litPagerProduct" runat="server" />
        </div>
        </div>
    </div>
    <div id="mystickytooltip" class="stickytooltip">
        <div style="padding: 5px 10px">
            <asp:Literal ID="litStickyToolTip" runat="server" />
        </div>
    </div>
</asp:Content>
