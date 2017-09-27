<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="Accounts.aspx.cs" Inherits="Electracraft.Client.Website.Private.Accounts" %>

<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Accounts
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <%--<controls:ContactMenu ID="ContactMenu1" runat="server"></controls:ContactMenu>
    --%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Accounts</h3>
        </div>
    </div>
    <asp:PlaceHolder ID="phAccounts" runat="server">
        <div class="row">
            <div class="small-12 medium-4 columns">
                Default Quote Rate
            </div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtDefaultQuoteRate" runat="server">
                </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Default Charge Up Rate
            </div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtDefaultChargeUpRate" runat="server">
                </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Next Job ID
            </div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtJobNumberAuto" runat="server">
                </asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="small-12 columns">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
