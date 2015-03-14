<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ImportStockDetail.aspx.cs" Inherits="Ecms.Website.Admin.ImportStockDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Chi tiết phiếu nhập kho
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <asp:MultiView ID="mtvMain" ActiveViewIndex="0" runat="server">
        <asp:View runat="server" ID="formView">
            <table style="width: 80%;">
                <tr>
                    <td style="width: 150px;">
                        <strong>Mã phiếu nhập </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCode" runat="server"></asp:Label>
                    </td>
                    <td style="width: 150px;">
                        <strong>Người tạo </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedUser" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Ngày tạo </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                    </td>
                    <td>
                        <strong>Ngày nhập </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblImportDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Tình trạng </strong>
                    </td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <div class="btnLine">
                <asp:Button Text="Nhập kho" runat="server" OnClientClick="return confirm('Bạn có chắc chắn muốn nhập kho?');"
                    ID="btnImport" CssClass="button" OnClick="btnImport_Click" />

                &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="button" 
                    onclick="btnBack_Click" Text="Quay lại" />

            </div>
            <div>
                <asp:GridView ID="gridMain" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    AllowPaging="True" OnPageIndexChanging="gridMain_PageIndexChanging" PageSize="15">
                    <Columns>
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="50" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductCode") %>
                            </ItemTemplate>
                            <ItemStyle Width="200" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductName") %>
                            </ItemTemplate>
                            <ItemStyle Width="200" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Loại sản phẩm">
                            <ItemTemplate>
                                <%# Eval("CategoryName") %>
                            </ItemTemplate>
                            <ItemStyle Width="200" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Serial">
                            <ItemTemplate>
                                <%# Eval("Serial") %>
                            </ItemTemplate>
                            <ItemStyle Width="200" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá(VND)">
                            <ItemTemplate>
                                <%# Convert.ToInt32(Eval("Price").ToString()).ToString("N0") %>
                            </ItemTemplate>
                            <ItemStyle Width="100" HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="float:right; text-align:right;">
                    <br />
                    <strong>Tổng tiền:</strong> <asp:Label ID="lblSumAmount" runat="server" /> VND
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="resultView">
            <table width="50%" border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td align="center">
                        <strong>Nhập kho thành công! </strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnOk" runat="server" CssClass="button" OnClick="btnOk_Click" Text="Ok" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
