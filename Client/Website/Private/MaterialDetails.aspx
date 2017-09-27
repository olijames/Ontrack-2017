<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="MaterialDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.MaterialDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Material Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h2>
                Material Details</h2>
                <h4><asp:Literal ID="litContactName" runat="server"></asp:Literal></h4>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Material Name
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtMaterialName" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Category
        </div>
        <div class="small-12 medium-8 columns">
            <asp:DropDownList ID="ddlMaterialCategory" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Cost Price</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtCostPrice" runat="server"></asp:TextBox></div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Sell Price</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtSellPrice" runat="server"></asp:TextBox></div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Description</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    </div>
</asp:Content>
