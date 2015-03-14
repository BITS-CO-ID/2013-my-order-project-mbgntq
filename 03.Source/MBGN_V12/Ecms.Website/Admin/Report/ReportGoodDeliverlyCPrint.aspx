<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportGoodDeliverlyCPrint.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportGoodDeliverlyCPrint" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Xác nhận in phiếu giao nhận
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
        <asp:View ID="formView" runat="server">
    <table style="width: 70%;" cellspacing="0" class="gridview">
        <tr >
            <td colspan="4" align="center" style="padding:10px 0 20px 0;">
                <asp:Label ID="Label1" runat="server" Text="PHIẾU GIAO NHẬN(Tổng hợp giao hàng)" Font-Bold="true" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="font-weight:bold; font-size:medium" colspan="2" align="center" >
                Thông tin khách hàng:
            </td>
            <td colspan="2" align="center" style="font-weight:bold; font-size:medium">
                Thông tin người giao hàng
            </td>            
        </tr>
        <tr>
            <td style="width:10%;">
                Mã khách hàng:
            </td>
            <td style="width:40%;">
               <asp:Label ID="lblCusCode" runat="server" Text=""></asp:Label>
            </td>
            <td style="width:10%;">
                Họ và tên:
            </td>
            <td style="width:40%;">
                <asp:TextBox ID="txtDeliverlyFullName" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>            
        </tr>
        <tr>
            <td>
                Tên khách hàng:
            </td>
            <td>
                <asp:Label ID="lblCusName" runat="server" Text=""></asp:Label>
            </td>
            <td>
                Chức vụ:
            </td>
            <td>
                <asp:TextBox ID="txtDeliverlyPosition" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Đ/C nhận hàng:
            </td>
            <td>
                <asp:Label ID="lblCusDeliverAddress" runat="server" Text=""></asp:Label>
            </td>
            <td> 
                Điện thoại:
            </td>
            <td>
                <asp:TextBox ID="txtDeliverlyMobile" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Điện thoại:
            </td>
            <td>
                <asp:Label ID="lblCusMobile" runat="server" Text=""></asp:Label>
            </td>
            <td>
                Ghi chú:
            </td>
            <td> 
                <asp:TextBox ID="txtRemark" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>               
            </td>
        </tr>
    </table>
    <div>
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    </div>
    <div class="btnLine">
        <asp:Button ID="btnConfirmPrintDeliverly" runat="server" CssClass="button" Text="Xác nhận In phiếu giao nhận" OnClick="btnConfirmPrintDeliverly_Click" 
        OnClientClick="return confirm('Bạn có chắc chắn muốn xác nhận lưu lại tình trạng đã in phiếu giao hàng này???');"/>
         <asp:Button ID="btnReturn" runat="server" CssClass="button" Text="Quay lại" OnClick="btnReturn_Click" />
    </div>
    <div>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" ShowFooter="true" OnRowDeleting="gridMain_OnRowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                    <strong>Tổng:</strong>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã MH">
                     <ItemTemplate>
                        <%# Eval("DetailCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                     <ItemTemplate>
                        <%#Eval("OrderNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày đặt hàng">
                    <ItemTemplate>
                        <%# Eval("OrderDate") == null ? "" : Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BillNo">
                    <ItemTemplate>
                        <%#Eval("TrackingNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Nhóm sản phẩm">
                    <ItemTemplate>
                        <%#Eval("CategoryName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Website">
                    <ItemTemplate>
                        <%#Eval("WebsiteName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                        <%#Eval("Quantity") %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trọng lượng">
                    <ItemTemplate>
                        <%#Eval("Weight") %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thành tiền">
                    <ItemTemplate>
                        <%#Eval("TotalMoney") == null ? "0" : Convert.ToDouble(Eval("TotalMoney").ToString()).ToString("N2")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%=dSumTotalAmount.ToString("N2")%></strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%#Eval("DetailStatusText")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ghi chú">
                    <ItemTemplate>                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton Text='Xóa' ID="lbtnDelete" CommandArgument='<%# Eval("OrderDetailId") %>'
                        CommandName="Delete" runat="server" OnCommand="lbtnDelete_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </asp:View>
        <asp:View ID="resultView" runat="server">
            <table width="50%" border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#0066FF" Text="Đã xác nhận lưu thông tin In phiếu giao nhận thành công."></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td align="center">
                        <br />
                        <br />
                        <asp:Button ID="btnExcel" runat="server" CssClass="button" Text="Xuất file Excel In phiếu giao nhận" OnClick="btnExcel_Click" /> 
                        <asp:Button ID="btnOk" runat="server" CssClass="button" OnClick="btnOk_Click" Text="    OK    " />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
