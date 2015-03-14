<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="ProductDetail.aspx.cs" Inherits="Ecms.Website.Site.MBGN.ProductDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="product">
        <div class="breacrumbs">
            <div>
                <a href="../../site/default.aspx">QuangChau247</a> <i class="icon-angle-right">
                </i>Chi tiết sản phẩm
            </div>
        </div>
        <div class="product-detail">
            <div class="row">
                <div class="span4">
                    <div class="product-image">
                        <asp:Literal ID="litImage" runat="server" />
                    </div>
                </div>
                <div class="span5">
                    <h1>
                        <asp:Label ID="lblProductName" runat="server" /></h1>
                    <p>
                        <asp:Label ID="lblShortDescription" runat="server" />
                    </p>
                    <p class="price">
                        Trong kho:
                        <asp:Label ID="lblProductType" runat="server" Font-Bold="true"/>
                    </p>
                    <p class="price">
                        Tình trạng:
                        <asp:Label ID="lblProductStatus" runat="server" Font-Bold="true"/>
                    </p>
                    <p class="price">
                        Giá:
                        <asp:Label ID="lblPrice" runat="server" Font-Bold="true" ForeColor="Red"/>
                    </p>
                    <asp:Button Text="Thêm vào giỏ" ID="btnAddToCard" CssClass="button" runat="server" OnClick="btnAddToCard_Click" />
                </div>
                <div class="clearfix">
                </div>
                <div class="span9">
                    <div class="detail-description">
                        <h3>
                            Mô tả sản phẩm</h3>
                        <p>
                            <asp:Label ID="lblDescription" runat="server" />
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
