<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="LabourDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.LabourDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Labour Item Details
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h2>
                Labour Item Details</h2>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Labour Item Name
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtLabourName" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Cost Price
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtCostPrice" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Sell Price</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtSellPrice" runat="server"></asp:TextBox>
        </div>
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
