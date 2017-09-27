<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="CompanyDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.CompanyDetails" %>
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
            <h2>Company Details - <asp:Literal ID="litCompanyName" runat="server"></asp:Literal></h2>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns" style="padding-bottom: 1em;">
            <div class="button-panel">
                <asp:Button ID="btnSaveContact" runat="server" Text="Save" OnClick="btnSaveContact_Click" />
                <asp:HyperLink ID="HyperLink1" runat="server" Text="Cancel" NavigateUrl="~/Private/Settings.aspx"></asp:HyperLink>
            </div>
        </div>
    </div>
    <controls:UserDetails ID="udCompany" runat="server" />


<table class="">
    

    <tr id="trManager1" runat="server">
        <td colspan="2"><h4>Assign New Manager</h4></td>
    </tr>
    <tr id="trManager2" runat="server">
        <td>Enter email address of new manager</td>
        <td><asp:TextBox ID="txtNewManager" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2">
            
        </td>
    </tr>
</table>

</asp:Content>
