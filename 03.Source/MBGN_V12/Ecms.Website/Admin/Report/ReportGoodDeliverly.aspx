<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportGoodDeliverly.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportGoodDeliverly" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Báo cáo Tình trạng món hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
    <table style="width:700px;" >
        <tr>
            <td style="width: 150px;">
                Từ ngày(Ngày Đặt hàng)
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFromDate" CssClass="datepicker" />
                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtFromDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True" DatePickerSynchronize="true">
                </obout:Calendar>
               
            </td>
            <td>
                Loại đơn hàng
            </td>
            <td>
                <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <%--<asp:ListItem Value="1">BG - Báo giá</asp:ListItem>--%>
                    <asp:ListItem Value="2">MH - Đặt hàng mua hộ</asp:ListItem>
                    <asp:ListItem Value="3">VC - Gửi hàng vận chuyển</asp:ListItem>
                    <%--<asp:ListItem Value="4">CS - Đặt hàng có sẵn</asp:ListItem>--%>
                </asp:DropDownList>
            </td>

            
        </tr>
        <tr>
            <td style="width: 150px;">
                Đến ngày
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtToDate" CssClass="datepicker" />
                <obout:Calendar ID="cldToDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtToDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../../Content/Images/icons/Calender-icon.png" DatePickerSynchronize="true">
                </obout:Calendar>
            </td>
            <td>
                Website
            </td>
            <td>
                <asp:DropDownList ID="ddParrentWebsite" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCategoryIdParent_SelectedIndexChanged" CssClass="Cbo">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Mã đơn hàng
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
            <td> 
                Shop
            </td>
            <td>
                <asp:TextBox ID="txtShop" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                Mã khách hàng
            </td>
            <td>
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
             <td>
                Tình trạng
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="Cbo">
                    <asp:ListItem Value="">-- Tất cả --</asp:ListItem>
                    <asp:ListItem Text="Mới gửi" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Đang gom, đang xử lý" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Đã mua" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Đã hủy" Value="3"></asp:ListItem>
                    <%--<asp:ListItem Text="Đã đến Mỹ" Value="4"></asp:ListItem>--%>
                    <asp:ListItem Text="Đã về Việt Nam" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Đã giao hàng" Value="6"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr> 
        <tr>
            <td style="width: 150px;">
                Mã MH
            </td>
            <td>
                <asp:TextBox ID="txtDetailCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
             <td>
                Mã Bill
            </td>
            <td>
                <asp:TextBox ID="txtBillNo" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>             
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Tìm kiếm" OnClick="btnSearch_Click" />
        <asp:Button ID="btnExcel" runat="server" CssClass="button" Text="Xuất file Excel" OnClick="btnExcel_Click" />
        <asp:Button ID="btnPrintDeliverly" runat="server" CssClass="button" Text="In phiếu giao nhận" OnClick="btnPrintDeliverly_Click" Visible="false"/>
    </div>
    <div>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã MH">
                     <ItemTemplate>
                        <%# Eval("DetailCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại ĐH">
                     <ItemTemplate>
                        <%# Eval("OrderTypeName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                     <ItemTemplate>
                        <%#Eval("OrderNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng NN">
                     <ItemTemplate>
                        <%#Eval("OrderOutboundNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày<br/>đặt hàng">
                    <ItemTemplate>
                        <%# Eval("OrderDate") == null ? "" : Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã Bill">
                    <ItemTemplate>
                        <%#Eval("TrackingNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH">
                    <ItemTemplate>
                        <%#Eval("CustomerCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên KH">
                    <ItemTemplate>
                        <%#Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Nhóm sản phẩm">
                    <ItemTemplate>
                        <%#Eval("CategoryName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Tên sản phẩm">
                    <ItemTemplate>
                        <%#Eval("ProductName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Website">
                    <ItemTemplate>
                        <%#Eval("WebsiteName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shop">
                    <ItemTemplate>
                        <%#Eval("Shop") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày<br/>về VN">
                    <ItemTemplate>
                        <%# Eval("DeliveryVNDate") == null ? "" : Convert.ToDateTime(Eval("DeliveryVNDate")).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày<br/>giao hàng">
                    <ItemTemplate>
                        <%# Eval("DeliveryDate") == null ? "" : Convert.ToDateTime(Eval("DeliveryDate")).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%#Eval("DetailStatusText")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="In<br/>phiếu GN">
                    <ItemTemplate>
                        <%#Eval("CPDetailId")==null?"Chưa In":"Đã In"%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
