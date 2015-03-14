<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReportOrderDebitDetail.aspx.cs" Inherits="Ecms.Website.Admin.Report.ReportOrderDebitDetail" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
     Báo cáo chi tiết công nợ đơn hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table style="width: 50%;">
        <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDetail" runat="server" Visible="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDateDetail" runat="server" Visible="true"></asp:Label>
                    </td>
                </tr>
        </table>
        <table style="width: 23%;">
                
                <tr>
                    <td>
                        (1) - Số dư hiện tại
                    </td>
                    <td>
                        <asp:Label ID="lblCurrentBalance" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        (2) - Số dư đóng băng
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceFreeze" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        (3) - Số dư khả dụng[(3)=(1) - (2)]
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceAvaiabilyty" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                </table>

      <div class="btnLine">
      </div>
        <table style="width: 25%;">
        <tr> 
            <td colspan="2">(4) - Đầu kỳ:</td>
            <td >
                <asp:Label ID="lblOpenBalance" runat="server" Visible="true" Font-Bold="true"></asp:Label>
            </td>
        </tr>
         <tr>
            <td colspan="2">(5) - Phát sinh trong kỳ:</td>
            <td colspan="2">
                <asp:Label ID="lblIncreaseBalance" runat="server" Visible="true" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">(6) - CN Cuối kỳ:</td>
            <td colspan="2">
                <asp:Label ID="lblFinishBlance" runat="server" Visible="true" Font-Bold="true"></asp:Label>
            </td>
        </tr>
         <tr>
            <td colspan="2">(7) - Đã TT Cuối kỳ:</td>
            <td colspan="2">
                <asp:Label ID="lblPaiPayBalance" runat="server" Visible="true" Font-Bold="true"></asp:Label>
            </td>
        </tr>
         <tr>
            <td colspan="2">(8) - CN ĐH Còn lại:</td>
            <td colspan="2">
                <asp:Label ID="lblRemainBalance" runat="server" Visible="true" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">(9) - CN Khách hàng(CN phải thu)[(9) =(8) - (3)]<br/></td>
            <td colspan="2">
                <asp:Label ID="lblCusDebit" runat="server" Visible="true" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <%--<asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Tìm kiếm" OnClick="btnSearch_Click" />--%>
        <asp:Button ID="btnExcel" runat="server" CssClass="button" Text="Xuất file Excel" OnClick="btnExcel_Click" />
    </div>
    <div>
        <b>Chi tiết công nợ đơn hàng trong kỳ báo cáo:</b>
        <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview" ShowFooter="True"
            AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15" OnRowCommand="gridMain_RowCommand">
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
                <asp:TemplateField HeaderText="Ngày đặt hàng<br/>(1)">
                    <ItemTemplate>
                        <%#Eval("OrderDate") == null ? "" : Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng<br/>(2)">
                    <ItemTemplate>
                        <%#Eval("OrderNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tình trạng<br/>(3)">
                    <ItemTemplate>
                        <%#Eval("OrderStatusText") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền - CN trong kỳ<br/>(4)">
                    <ItemTemplate>
                        <%# Eval("SumAmount") != null ? Convert.ToDouble(Eval("SumAmount").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[0] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Đã TT trong kỳ<br/>(5)">
                    <ItemTemplate>
                        <%--<%# Convert.ToDouble(Eval("TotalPayAmountNormal").ToString()).ToString("N2")%>--%>
                         <asp:LinkButton Text='<%# Convert.ToDouble(Eval("TotalPayAmountNormal").ToString()).ToString("N0")%>' ID="lbtnPaidPay" CommandArgument='<%# Eval("CustomerId")+"|"+ Eval("OrderNo")%>' 
                        Enabled= '<%# Convert.ToDouble(Eval("TotalPayAmountNormal").ToString())!=0?true:false %>' 
                        CommandName="RptPaidPay" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[1] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tổng tiền<br/>vận chuyển cuối kỳ<br>(6)">
                    <ItemTemplate>
                        <%# Eval("SumFeeShip") != null ? Convert.ToDouble(Eval("SumFeeShip").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[2]%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Tổng tiền tính phí<br/>trả chậm cuối kỳ<br>(7)=(4)-(6)">
                    <ItemTemplate>
                        <%# Eval("AmountCalFeeDelay") != null ? Convert.ToDouble(Eval("AmountCalFeeDelay").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[3]%></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phí trả chậm trong kỳ<br/>(8)">
                    <ItemTemplate>
                        <%# Eval("AmountFeeDelay") != null ? Convert.ToDouble(Eval("AmountFeeDelay").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[4] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="CN còn lại trong kỳ<br/>(7)=(4)-(5)">
                    <ItemTemplate>
                        <%# Eval("RemainAmount") != null ? Convert.ToDouble(Eval("RemainAmount").ToString()).ToString("N0") : "0"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong><%= Totals[5] %></strong>
                    </FooterTemplate>
                    <FooterStyle  HorizontalAlign="Right"/>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
