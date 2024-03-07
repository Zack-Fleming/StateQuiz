<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" Title="Login/Register" CodeBehind="~/pages/LoginRegister.aspx.cs" Inherits="StateQuiz.pages.LoginRegister" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <div class = "content left half level2">
        <h1>Register</h1>

        <p>
            <asp:Label ID="lbl_register_error" runat="server" />
        </p>

        <p>
            <asp:Label Text="First Name: " runat="server" />
            <asp:TextBox ID="txt_first" Font-Names="Courier" runat="server" />
        </p>

        <p>
            <asp:Label Text="Last Name : " runat="server" />
            <asp:TextBox ID="txt_last" Font-Names="Courier" runat="server" />
        </p>

        <br /><br />

        <p>
            <asp:Label Text="User Name : " runat="server" />
            <asp:TextBox ID="txt_user" Font-Names="Courier" runat="server" />
        </p>

        <p>
            <asp:Label Text="Password &nbsp;: " runat="server" />
            <asp:TextBox ID="txt_pass" Font-Names="Courier" TextMode="Password" runat="server" />
        </p>
        <p>
            <asp:Label Text="Confirm &nbsp;&nbsp;: " runat="server" />
            <asp:TextBox ID="txt_confirm" Font-Names="Courier" TextMode="Password" runat="server" />
        </p>

        <p>
            password Requirements:
        </p>
        <div style="text-align: center;">
            <ul style="display: inline-block;">
                <li>use 16 or more characters</li>
                <li>one+ lowercase character</li>
                <li>one+ uppercase character</li>
                <li>one+ number</li>
                <li>one+ special character</li>
                <ul>
                    <li>includes: -_@$!%*?&#|</li>
                </ul>
                <li>do not include: </li>
                <ul>
                    <li>;:<>().,+=</li>
                </ul>
            </ul>
        </div>

        <br />

        <p>
            <asp:Button ID="btn_register" Font-Names="Courier" Text="Register Account" OnClick="Add_User" runat="server" />
        </p>


    </div>

    <div class = "content right half level2">
        <h1>Login</h1>

        <p>
            <asp:Label ID="lbl_login_error" runat="server" />
        </p>

        <p>
            <asp:Label Text="User Name: " runat="server" />
            <asp:TextBox ID="txt_user_login" Font-Names="Courier" runat="server" />
        </p>

        <p>
            <asp:Label Text="Password : " runat="server" />
            <asp:TextBox ID="txt_pass_login" Font-Names="Courier" TextMode="Password" runat="server" />
        </p>

        <br />

        <p>
            <asp:Button ID="btn_login" Font-Names="Courier" Text="Login" OnClick="Login_User" runat="server" />
        </p>
    </div>
</asp:Content>
