<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="PaymentCustomerDetail.aspx.cs" Inherits="Ecms.Website.Admin.Report.PaymentCustomerDetail" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Chi tiết thanh toán khách hàng
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table style="width: 40%;">
        <%--<tr>
            <td>
                Mã TT/Mã ĐH
            </td>
            <td>
                <asp:TextBox ID="txtInvoiceCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
            <td>
                Mã khách hàng
            </td>
            <td>
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr> --%>      
        <tr>
            <td colspan="4">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div>
    <b>Chi tiết giao dịch thanh toán phân bổ cho đơn hàng:</b>
    <br />
    <br />
    </div>
    <div class="data">
        <asp:GridView ID="gridMain" runat="server" CssClass="gridview" OnPageIndexChanging="gridMain_PageIndexChanging"
            AutoGenerateColumns="False" AllowPaging="True" PageSize="15" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <strong>Tổng:</strong>
                    </FooterTemplate>
                    <ItemStyle Width="20" HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã thanh toán">
                    <ItemTemplate>
                        <%# Eval("InvoiceCode")%>
                        <%--<asp:LinkButton Text='<%# Eval("InvoiceCode")%>' CommandArgument='<%# Eval("InvoiceId")%>' CommandName="ConfirmPayment" runat="server" />                        --%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã đơn hàng">
                    <ItemTemplate>
                        <%# Eval("OrderNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã TT tham chiếu">
                    <ItemTemplate>
                        <%# Eval("InvoiceRefCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại TT">
                    <ItemTemplate>
                        <%# Eval("BusinessName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mã KH">
                    <ItemTemplate>
                        <%# Eval("CustomerCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên KH">
                    <ItemTemplate>
                        <%# Eval("CustomerName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày TT">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("InvoiceDate").ToString()).ToString("dd/MM/yyyy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngân hàng">
                    <ItemTemplate>
                        <%# Eval("BankName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Nội dung">
                    <ItemTemplate>
                        <span title='<%# Eval("Remark") %>'><%# Eval("Remark") != null ? Eval("Remark").ToString() : "" %></span>
                    </ItemTemplate>
                </asp:TemplateField>
               <%-- <asp:TemplateField HeaderText="Tổng tiền">
                    <ItemTemplate>
                        <%# Convert.ToDouble(Eval("SumAmount").ToString()).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="Tổng tiền phân bổ">
                    <ItemTemplate>
                        <%# (Convert.ToDouble(Eval("SumAmount").ToString())).ToString("N0")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <strong> <%=dSumAmount.ToString("N0") %> </strong>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>

                <%--<asp:TemplateField HeaderText="Tình trạng">
                    <ItemTemplate>
                        <%# Eval("StatusText")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        <br />
    </div>
</asp:Content>
