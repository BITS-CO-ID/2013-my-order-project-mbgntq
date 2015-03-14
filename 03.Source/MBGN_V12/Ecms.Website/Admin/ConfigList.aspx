<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ConfigList.aspx.cs" Inherits="Ecms.Website.Admin.ConfigList" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitlePlaceHolder" runat="server">
    Quản lý chính sách phí
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table  style="width: 60%;">
        <tr>
            <td colspan="4">
                Thông tin tìm kiếm: &nbsp; &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width:150px;">
                Từ ngày
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFromDate" CssClass="datepicker" />
                <obout:Calendar ID="cldFromDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtFromDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../Content/Images/icons/Calender-icon.png" MultiSelectedDates="True">
                </obout:Calendar>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Đến ngày
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtToDate" CssClass="datepicker" />
                <obout:Calendar ID="cldToDate" runat="server" DatePickerMode="true" CultureName="vi-VN"
                    YearMonthFormat="dd/MM/yyyy" TextBoxId="txtToDate" Columns="1" DatePickerImageTooltip="Chọn ngày"
                    DatePickerImagePath="../Content/Images/icons/Calender-icon.png">
                </obout:Calendar>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Chính sách phí
            </td>
            <td>
                <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="cbo">
                    <asp:ListItem Value="">------- Tất cả -------</asp:ListItem>
                    <%--<asp:ListItem Value="101"> - Công</asp:ListItem>--%>
                    <%--<asp:ListItem Value="TAXUSA"> - Thuế mỹ</asp:ListItem>
                    <asp:ListItem Value="301"> - Áp tính phí cho toàn bộ hệ thống</asp:ListItem>
                    <asp:ListItem Value="302"> - Số ngày trả chậm cho phép</asp:ListItem>
                    <asp:ListItem Value="303"> - Giá trị trả chậm</asp:ListItem>                    --%>
                    <asp:ListItem Value="ORGRATE"> - Tỉ giá mua hộ</asp:ListItem>
                    <%--<asp:ListItem Value="ORGRATEDE"> - Tỉ giá vận chuyển</asp:ListItem>--%>
                    <%--<asp:ListItem Value="FEE"> - Phí vận chuyển</asp:ListItem>--%>
                    <%--<asp:ListItem Value="FEEMH"> - Phí mua hộ</asp:ListItem>--%>
                    <asp:ListItem Value="401"> - Phí ship mua hộ</asp:ListItem>
                    <asp:ListItem Value="402"> - Phí ship vận chuyển</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <div class="btnLine">
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"
            CssClass="Button" />
        &nbsp;<asp:Button ID="btnAdd" runat="server" Text="Thêm mới" OnClick="btnAdd_Click"
            CssClass="Button" />
    </div>
    <br />
    <div>
        <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdD" CssClass="gridview"
            Width="100%" AllowPaging="True" PageSize="15" OnRowCommand="gridMain_RowCommand"
            onpageindexchanging="grdD_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Chính sách">
                    <ItemTemplate>
                        <%# Eval("BusinessName") %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="150" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày áp dụng">
                    <ItemTemplate>
                        <%# Eval("CreatedDate") == null ? "" : Convert.ToDateTime(Eval("CreatedDate").ToString()).ToString("dd/MM/yyyy HH:mm:ss")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="60" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Xuất xứ">
                    <ItemTemplate>
                        <%# Eval("CountryName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="100" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại khách hàng">
                    <ItemTemplate>
                        <%# Eval("CustomerTypeName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="100" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Chủng loại sản phẩm">
                    <ItemTemplate>
                        <%# Eval("CategoryName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="100" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Website">
                    <ItemTemplate>
                        <%# Eval("WebsiteName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="100" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Đơn vị">
                    <ItemTemplate>
                        <%# Eval("Unit")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="50" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Từ">
                    <ItemTemplate>
                        <%# Eval("fromQuantity")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="50" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Đến">
                    <ItemTemplate>
                        <%# Eval("toQuantity")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="50" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Giá trị">
                    <ItemTemplate>
                        <%--<%# Eval("ConfigValue") == null ? "0" : Convert.ToDouble(Eval("ConfigValue")).ToString("G")%>--%>
                        <%# (Convert.ToInt32(Eval("ConfigValue"))==1 &&Eval("BusinessCode").ToString()=="301")?"Cho phép": (Convert.ToInt32(Eval("ConfigValue"))==0 &&Eval("BusinessCode").ToString()=="301")?"Không cho phép":
                        Convert.ToDouble(Eval("ConfigValue")).ToString("G")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="100" />
                </asp:TemplateField>

                 <%--<asp:TemplateField HeaderText="Thay đổi">
                            <ItemTemplate>
                                <asp:LinkButton Text='Thay đổi' Visible='<%# Eval("BusinessCode").ToString().Equals("301") ? true: false %>'
                                    ID="lblChange" CommandArgument='<%# Eval("ConfigBusinessId")%>'
                                    CommandName="ChangedConfig" runat="server" OnClientClick="return Confirm('Bạn có chắc chắn muốn xác nhận thay đổi cấu hình tính phí trả chậm cho toàn bộ hệ thống?');" />
                            </ItemTemplate>
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField> --%>

            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:Literal runat="server" ID="lblStatus"></asp:Literal>
    </div>
</asp:Content>
