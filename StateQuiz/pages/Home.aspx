<%@ Page Title="Home | States Quiz" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="StateQuiz.pages.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>State Quiz</h1>

    <div class="content level2">
        <h2>About The Quiz</h2>

        <p>
            Are you a geography nerd? Test your knowledge in the capitals of the 
            US states. Pick from a random set of states, ranging in overall quiz 
            length. Quiz lengths range from five, ten, twenty-five, and fifty 
            questions. No matter the length of the quiz, the questions will always 
            be randomly generated, each time. The quiz can be run in two differnet 
            modes: guessing the state capital from the state name,  or guessing 
            the state name from its capital.
        </p>

        <p>
            <asp:Button ID="btn_quiz" Text="Start A New Quiz" Font-Names="Courier" OnClick="Quiz_Redirect" runat="server" />
        </p>
    </div>

    <div class="content level2">
        <h2>User Metrics</h2>

        <p>
            Logging into the site, allows the history of the user's past quizes. 
            These quizes show the number of questions the user got correct, the 
            total number of quiz questions, the calculated score percentage, and 
            the date the quiz was taken. Also, the user will see that the total 
            number of quizes the user has taken, and their average score of all 
            of their quizes. Note: This site does not collect any personally 
            identifiable information, because a person's name alone does not 
            uniquly identify any one person. 
        </p>

        <p>
            <asp:Button ID="btn_user" Text="Register/Login" Font-Names="Courier" OnClick="Login_Redirect" runat="server" />
        </p>

    </div>
</asp:Content>