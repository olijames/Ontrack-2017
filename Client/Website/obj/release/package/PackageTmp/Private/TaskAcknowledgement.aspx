<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="TaskAcknowledgement.aspx.cs" Inherits="Electracraft.Client.Website.Private.TaskAcknowledgement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
Task Acknowledgement
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<div class="row">
    <div class="small-12 columns">
        <h2>Task Acknowledgement</h2>
    </div>
</div>
<div class="row">
    <div class="small-12 columns">
        <h4><asp:Literal ID="litTaskName" runat="server"></asp:Literal></h4>
    </div>
</div>

<div class="row">
    <div class="small-12 columns">
        <p><asp:Literal ID="litTaskDescription" runat="server"></asp:Literal></p>
    </div>
</div>

<div class="row">
    <div class="small-12 columns">
        <asp:Button ID="btnAcknowledge" runat="server" Text="Acknowledge" OnClick="btnAcknowledge_Click" />
    </div>
</div>

</asp:Content>
