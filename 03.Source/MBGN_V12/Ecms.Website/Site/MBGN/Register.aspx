<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="Ecms.Website.Site.MBGN.Register" %>

<%@ Register src="../PartControl/Register.ascx" tagname="Register" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">    
    <uc1:Register ID="Register1" runat="server" />    
</asp:Content>
