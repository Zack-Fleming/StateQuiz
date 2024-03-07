<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="StateQuiz.pages.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="lit_greeting" runat="server" />

    <div class="content level2">
        <h1>User Info</h1>

        <asp:Literal ID="lit_user" runat="server"/>
    </div>

    <div class="content level2">
        <h1>New Quiz</h1>

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
            When the quiz is graded, you will notice a few thins will happen:
            <div style="text-align: center;">
                <ol style="display: inline-block;">
                    <li>Your answer will be bolded</li>
                    <li>Wrong answers will be striked through</li>
                    <li>Correct answer will not be striked through</li>
                    <li>If your answer is right, it will be bolded</li>
                    <li>If your answer is wrong, it will be bold and striked</li>
                    <li>Your score will be displayed at the bottom of the quiz</li>
                    <li>The 'Quiz Submit' button will change to say 'Finish Quiz'</li>
                    <li>After finishing the quiz, you will be brought to your dashboard</li>
                </ol>
            </div>
        </p>

        <br />

        <p>
            Number of Questions:
            <asp:DropDownList ID="ddl_quiz_len" Font-Names="Courier" runat="server">
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="25" Value="25" />
                <asp:ListItem Text="50" Value="50" />
            </asp:DropDownList>
        </p>

        <p>
            Quiz Mode:
            <br />
            <asp:RadioButton ID="mode_capital" Text="Capitals from State" GroupName="r_quiz_mode" Value="c" runat="server" />
            <br />
            <asp:RadioButton ID="mode_state" Text="States from Capital" GroupName="r_quiz_mode" Value="s" runat="server" />
        </p>
        
        <p>
            <asp:Button ID="btn_start" Text="Start New Quiz" Font-Names="Courier" OnClick="Start_Quiz" runat="server" />
        </p>
    </div>

    <div class="content level2">
        <h1>Past Quizes</h1>

        <asp:Literal ID="lit_past_quiz" runat="server"/>
    </div>
</asp:Content>