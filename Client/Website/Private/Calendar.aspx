<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="Calendar.aspx.cs" Inherits="Electracraft.Client.Website.Private.Calendar" %>
<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Calendar
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">

<%--<controls:ContactMenu runat="server"></controls:ContactMenu>
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Calendar</h3>
        </div>
    </div>
</asp:Content>
