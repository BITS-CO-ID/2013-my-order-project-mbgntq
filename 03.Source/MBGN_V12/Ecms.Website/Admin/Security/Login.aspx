<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ecms.Website.Admin.Security.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Đăng nhập hệ thống</title>
    <style type="text/css">
        body
        {
            font-family: Tahoma;
            font-size: 13px;
            background: url('../../Content/Images/mbgntq-bg.png') repeat;
        }
        .LoginWrapper
        {
            background-color: #FFFFFF;
            border-radius: 15px 15px 15px 15px;
            box-shadow: 2px 2px 2px #333333;
            margin: 100px auto;
            padding: 0 20px 5px 20px;
            width: 400px;
        }
        input[type=text], input[type=password]
        {
            width: 250px;
            border: solid 1px #ddd;
            padding: 2px;
            height: 22px;
            line-height: 22px;
        }
        .copyright
        {
            color: #0071BC;
            text-align: right;
        }
        table tr td
        {
            padding-bottom: 10px;
            padding-left: 10px;
        }
        h3
        {
            color: #0071BC;
            text-transform: uppercase;
            font-weight: normal;
            font-size: 20px;
        }
        .Button, .button
        {
            display: inline-block;
            text-decoration: none;
            font: bold 12px/12px HelveticaNeue, Arial;
            padding: 5px 10px;
            border: 1px solid #dedede;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: #f5f5f5;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f9f9f9', endColorstr='#f0f0f0'); /*  IE */
            background: -webkit-gradient(linear, left top, left bottom, from(#f9f9f9), to(#f0f0f0)); /*  WebKit */
            background: -moz-linear-gradient(top,  #f9f9f9, #f0f0f0);
            color: #014586;
            text-shadow: 0 1px 0 #fff;
            -webkit-box-shadow: 0 1px 1px #eaeaea, inset 0 1px 0 #fbfbfb;
            -moz-box-shadow: 0 1px 1px #eaeaea, inset 0 1px 0 #fbfbfb;
            box-shadow: 0 1px 1px #eaeaea, inset 0 1px 0 #fbfbfb;
            border: #7f9db9 1px solid;
            cursor:pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="LoginWrapper">
        <asp:Login ID="Login1" runat="server" TitleText='<h3>Đăng nhập hệ thống</h3>' RememberMeText='Ghi nhớ'
            LoginButtonText='Đăng nhập' UserNameLabelText='Tên đăng nhập' PasswordLabelText='Mật khẩu'
            RememberMeSet="true" FailureText='Đăng nhập thất bại! Vui lòng thử lại.' OnAuthenticate="Login1_Authenticate"
            OnLoginError="Login1_LoginError">
            <LoginButtonStyle CssClass="button" />
        </asp:Login>
        <br />
        <span class="copyright">&copy; 2014 http://quangchau247.vn</span>
    </div>
    </form>
</body>
</html>
