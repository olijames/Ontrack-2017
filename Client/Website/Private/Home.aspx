<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Electracraft.Client.Website.Private.Home" %>
<%@ Register Src="~/UserControls/ContactPanelHome.ascx" TagName="ContactPanel" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row section">
        <div class="small-12 columns">
            <h2>My Companies</h2>
        </div>
    </div>
    <asp:PlaceHolder ID="phContactPanels" runat="server">
    </asp:PlaceHolder>
    <div class="row section" style="background: #f6f6f6;">
        <div class="small-12 columns ">
            Logged in as : <asp:Literal ID="littext" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
