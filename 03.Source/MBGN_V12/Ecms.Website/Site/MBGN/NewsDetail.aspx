<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="NewsDetail.aspx.cs" Inherits="Ecms.Website.Site.MBGN.NewsDetail" %>
<%@ Register src="../PartControl/Feedback.ascx" tagname="Feedback" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <div class="product">
        <div class="product-detail">
            <div class="span9">
                <div class="detail-description">
                    <h4 class="title-page">
                        <asp:Label ID="lblNewsTitle" runat="server" />
                    </h4>
                    <p>
                        <asp:Label ID="lblNewsContent" runat="server" />
                    </p>
                </div>
                <div class="feedback">                    
                    <uc1:Feedback ID="fbMain" runat="server" Visible="false" />                   
                </div>
            </div>
        </div>
    </div>
</asp:Content>
