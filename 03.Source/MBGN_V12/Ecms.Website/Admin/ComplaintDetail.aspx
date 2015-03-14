<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" validateRequest="false" 
 CodeBehind="ComplaintDetail.aspx.cs" Inherits="Ecms.Website.Admin.ComplaintDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
Quản lý ý kiến khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <script src="../../ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../../ckeditor/adapters/jquery.js" type="text/javascript"></script>
      <script type="text/javascript">
          window.onload = function () {
              CKEDITOR.replace('<%= txtContent.ClientID %>', { filebrowserImageUploadUrl: 'UploadComplaint.ashx' });
          };
    </script>
    
    <div class="post">
                <div class="posttop">
                    <div class="username">Tiêu đề:&nbsp;<asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></div>
                    <div class="date"><br /><hr /></div>
                </div>   
    </div>
   
    <asp:Repeater ID="rptSaleHot" runat="server">
        <HeaderTemplate>
            <table width="100%" cellspacing="0"  border="0" >
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
           <%--**************************************************************************--%>
                    
           <div class="post">
                <div class="posttop">
                    <div class="username"><%# DataBinder.Eval(Container, "DataItem.UserName")%></div>
                    <div class="date"><%# DataBinder.Eval(Container, "DataItem.CreatedDate")%></div>
                </div>
                <div class="posttext">
                        <%# DataBinder.Eval(Container, "DataItem.Content")%>
                </div>
                 <div class="postcontrol">
                       <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Id")%>'
                       Visible = '<%# Convert.ToString(DataBinder.Eval(Container, "DataItem.Id"))=="-1"? false : true %>'
                                            CommandName="Delete" OnCommand="lbtDelete_Click"
                                            OnClientClick="if (!confirm('Bạn có chắc chắn muốn xóa nội dung này không?')) return false;">Xóa</asp:LinkButton>
                </div>
            </div>
        <%--**************************************************************************--%>
        </ItemTemplate>
        <FooterTemplate>
            </tbody></table>
        </FooterTemplate>
    </asp:Repeater>
    <div class="clearfix">
    <br />
    </div>
    <div class="pagination">
        <asp:Literal ID="litPager" runat="server" />
    </div>
    <br /><br /><br />
    <%--Đăng bài--%>
    <div class="post">
        <div class="posttop">
            <b>Trả lời:</b>
        </div>
         <div class="posttext">
        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" 
                 CssClass="cktextbox"></asp:TextBox>
        </div>
        <div>
                        <asp:Label ID="lblError" runat="server" CssClass="indend" ForeColor="Red"></asp:Label>
                    <br />
                <asp:Button ID="btnSave" runat="server" CssClass="button green" Text="Đăng" 
                onclick="btnSave_Click" />&nbsp;<asp:Button ID="btnCancel" CssClass="button" runat="server" Text="Quay lại" />
        </div>
    </div>
     <br /> <br /><br />
</asp:Content>
