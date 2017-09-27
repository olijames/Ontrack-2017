<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="TaskHistory.aspx.cs" Inherits="Electracraft.Client.Website.Private.TaskHistory" %>
<%@ Register Src="~/UserControls/JobTasksHistory.ascx" TagName="JobTasks" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
Task History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<div class="row">
<div class="small-12 columns">
    <h4>Task History</h4>
</div></div>

    <controls:JobTasks ID="TaskList" runat="server" ShowFull="true" />

<div class="row">
<div class="small-12 columns">
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Private/JobSummary.aspx">Back</asp:HyperLink>
</div></div>
</asp:Content>
