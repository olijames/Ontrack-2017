<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="CompanyDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.Admin.CompanyDetails" %>

<%@ Register Src="~/UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Company Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h2 id="heading" runat="server">
            </h2>
        </div>
    </div>
    <controls:UserDetails ID="udCompany" runat="server" />
    <div class="row">
        <div class="small-12 medium-4 columns">
            Company Manager
        </div>
        <div class="small-12 medium-8 columns">
            <asp:Literal ID="litCompanyManager" runat="server"></asp:Literal>
        </div>
    </div>

    <div class="row">
        <div class="small-12 medium-4 columns">
            Assign New Manager
        </div>
        <div class="small-12 medium-8 columns">
            <asp:DropDownList ID="ddlNewManager" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row">
        <div class="small-12 columns">
            <asp:Button ID="btnSaveContact" runat="server" Text="Save" OnClick="btnSaveContact_Click" />
            <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" OnClick="btnDeactivate_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
            <asp:Button ID="btnActivate" runat="server" Text="Activate" OnClick="btnActivate_Click" />
            <asp:HyperLink ID="HyperLink1" runat="server" Text="Cancel" NavigateUrl="~/Private/Admin/ManageUsers.aspx"></asp:HyperLink>
        </div>
    </div>
</asp:Content>
