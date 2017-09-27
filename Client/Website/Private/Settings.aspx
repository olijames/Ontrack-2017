<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="Settings.aspx.cs" Inherits="Electracraft.Client.Website.Private.Settings" %>

<%@ Register Src="~/UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/ContactCompanies.ascx" TagName="ContactCompanies"
    TagPrefix="controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Options
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">  
            <h2>My Account</h2>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns" style="padding-bottom: 1em;">
            <div class="button-panel">
                <asp:Button ID="btnSaveDetails" runat="server" Text="Save" OnClick="btnSaveContact_Click" />
                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" />
            </div>
        </div>
    </div>
    <div class="row section">
        <div class="small-12 columns">
            <h4 class="underline"><i class="fi-torso"></i>&nbsp;&nbsp;My Details</h4>
            <controls:UserDetails AdminMode="false" ID="udContact" runat="server" />
        </div>
    </div>
    <div class="row section">
        <div class="small-12 columns">
            <h4 class="underline"><i class="fi-link"></i>&nbsp;&nbsp;My Linked Companies</h4>
            <controls:ContactCompanies ID="cc" runat="server" />
        </div>
    </div>
  
</asp:Content>
