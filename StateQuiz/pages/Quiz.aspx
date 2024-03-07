<%@ Page Title="Quiz" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Quiz.aspx.cs" Inherits="StateQuiz.pages.Quiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content level2">
        <h1>State Capital Quiz</h1>
        
        <asp:Panel ID="pane_quiz" runat="server" />

        <p>
            <asp:Button ID="btn_submit_quiz" Text="Submit Quiz" Font-Names="Courier" OnClick="Grade_Quiz" runat="server" />
        </p>

        <asp:Literal ID="lit_out" runat="server" />
    </div>

</asp:Content>
