<%@ Page Title="" Language="C#" MasterPageFile="~/Site/CMSFrontend1.Master" AutoEventWireup="true"
    CodeBehind="OrderByLinkUpload.aspx.cs" Inherits="Ecms.Website.Site.MBGN.OrderByLinkUpload" %>


<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <h4 class="title-page">
        Quý khách vui lòng upload file excel để đặt hàng:
    </h4>

    <%--<asp:UpdatePanel runat="server">
        <ContentTemplate>--%>

        <div style="padding: 15px 0px;">
            <b style="float:left; width: 120px; padding: 4px 0px 0px 0px;">Chọn file upload: </b>
            <asp:FileUpload ID="fileuploadExcel" runat="server" style="width: 300px; border: 1px solid #C0C0C0; margin: 0px 15px;"/>
            <asp:Button ID="btnImport" runat="server" Text=" Upload " OnClick="btnImport_Click" CssClass="button" Width="80px"/>
            <a href="../../Download/import_template.xls" style='float: right; text-decoration: underline; color: #014586'>File excel mẫu upload</a>
            <div style="padding: 15px 0px;">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </div>
        </div>

        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
   
    <p class="p-note"></p>
    <asp:Panel runat="server" ID="pnCartLink" Visible="false">
        <asp:GridView ID="gridCartByLink" runat="server" AutoGenerateColumns="False" CssClass="gridview" Width="100%" AllowPaging="true" PageSize="15" OnPageIndexChanging="gridCartByLink_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shop">
                    <ItemTemplate>
                        <%# Eval("Shop")%>
                    </ItemTemplate>
                    <ItemStyle Width="80" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Link">
                    <ItemTemplate>
                        <a href='<%# Eval("Link") %>' title='<%# Eval("Link") %>' target="_blank">
                            <%# Eval("Link").ToString().Length < 30 ? Eval("Link").ToString() : (Eval("Link").ToString().Substring(0, 30) + "...")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Size">
                        <ItemTemplate>
                            <%# Eval("Size")%>
                        </ItemTemplate>                        
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Màu sắc">
                    <ItemTemplate>
                        <%# Eval("Color") %>
                    </ItemTemplate>                    
                </asp:TemplateField>   
                <asp:TemplateField HeaderText="Số lượng">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("Quantity").ToString()).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle Width="40" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("Price").ToString()).ToString("N2")%>
                    </ItemTemplate>
                    <ItemStyle Width="40" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mô tả">
                    <ItemTemplate>
                        <%# Eval("Remark")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div style="padding: 15px 0px 0px 0px;">
            
            <asp:Button Text=" Đặt hàng " ID="btnOrder" CssClass="button cancel" runat="server" OnClick="btnOrder_Click" />
            <asp:Button ID="btnCancel" runat="server" CssClass="button cancel" Text=" Hủy bỏ " OnClick="btnCancel_Click" />
        </div>

    </asp:Panel>
</asp:Content>

