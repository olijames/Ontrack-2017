<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="MySites.aspx.cs" Inherits="Electracraft.Client.Website.Private.MySites" %>
<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    My Sites
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
<%--<controls:ContactMenu ID="ContactMenu1" runat="server"></controls:ContactMenu>
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                My Sites</h3>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <asp:Panel ID="pnlContactHomeButtons" runat="server">
                <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                    <asp:Button ID="btnAddSite" runat="server" Text="Add New Site" OnClick="btnAddSite_Click" />
                </asp:PlaceHolder>
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
            </asp:Panel>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <hr />
        </div>
    </div>

    <asp:Repeater ID="rpSites" runat="server">
        <ItemTemplate>
            <div class="row">
                <div class="small-12 medium-6 columns">
                    <asp:Button ID="btnSelectSite" runat="server" CommandName="SelectSite" CommandArgument='<%# Eval("SiteID") %>'
                        Text='<%# Eval("Address1").ToString() + "\r\n" + Eval("Address2").ToString() %>' OnClick="btnSelectSite_Click" Width="100%" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>
