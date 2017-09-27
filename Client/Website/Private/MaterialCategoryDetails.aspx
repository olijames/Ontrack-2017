<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="MaterialCategoryDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.MaterialCategoryDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
Material Category Details
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<div class="row">
    <div class="small-12 columns">
        Material Category Details
    </div>
</div>

<div class="row">
    <div class="small-12 columns">
        <p>This material category belongs to <asp:Literal ID="litContactName" runat="server"></asp:Literal>.</p>
    </div>
</div>

<div class="row">
    <div class="small-12 medium-4 columns">
        Category Name
    </div>
    <div class="small-12 medium-8 columns">
        <asp:TextBox ID="txtCategoryName" runat="server"></asp:TextBox>
    </div>
</div>

<div class="row">
    <div class="small-12 medium-4 columns">
        Active
    </div>
    <div class="small-12 medium-8 columns">
        <asp:CheckBox ID="chkActive" runat="server" />
    </div>
</div>

<div class="row">
    <div class="small-12 columns">
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
    </div>
</div>
</asp:Content>
