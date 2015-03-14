<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftNavigation.ascx.cs"
    Inherits="Ecms.Website.Site.PartControl.LeftNavigation" %>
<div class="widget">
    <h2>
        Website mua hàng TQ</h2>
    <div id="menu-wrapper">
        <ul class="nav">
            <asp:Literal ID="litWebsiteLink" runat="server" />
        </ul>
    </div>
</div>
<div class="widget support" style="display:none;">
    <h2>
        Hỗ trợ trực tuyến</h2>
    <div class="txt">
        <div class="yahoochat">
			<a href="ymsgr:sendim?it_vnpro" >
        		<img src="http://opi.yahoo.com/online?u=congthao_bg&m=g&t=2&l" border="0" />
        	</a>
		</div>
		<script type="text/javascript" src="http://cdn.dev.skype.com/uri/skype-uri.js"></script>
		<div id="SkypeButton_Chat_cong.thao_1">
		  <script type="text/javascript">
		    Skype.ui({
		      "name": "chat",
		      "element": "SkypeButton_Chat_cong.thao_1",
		      "participants": ["cong.thao"],
		      "imageSize": 32
		    });
		  </script>
		</div>
    </div>
</div>
<div class="widget ads1">
    <a href='<%= ResolveUrl("~/site/mbgn/newsdetail.aspx") + "?newsId=42" %>'><img src="../../Content/images/ads1.png" alt=""/></a>
    
</div>