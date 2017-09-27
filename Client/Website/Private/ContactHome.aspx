<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="ContactHome.aspx.cs" Inherits="Electracraft.Client.Website.Private.ContactHome" %>
<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
<asp:Literal ID="litContactNameTitle" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">

<%--<controls:ContactMenu runat="server"></controls:ContactMenu>
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
    <h2><asp:Literal ID="litContactName" runat="server"></asp:Literal></h2>
        </div>
    </div>
    <br/>
    <div class="row">
        <div class="small-12 columns">
            <asp:Panel ID="pnlContactHomeButtons" runat="server">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button radius blue tiny" OnClick="btnBack_Click" />
            </asp:Panel>
        </div>
    </div>

    <div class="row">
        <div class="small-12 columns">
    <hr />
        </div>
    </div>

    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnAccounts" runat="server" OnClick="btnAccounts_Click" Font-Size="small" Text="Accounts" Width="100%" Style="text-align: left" />
        </div>
    </div>
    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button id="btnAddVehicles" runat="server" Text="Add Vehicles" OnClick="btnVehicleInput_Click" Width="100%" Style="text-align: left"/>
        </div>
    </div>

    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnConnections" runat="server" OnClick="btnConnections_Click" Text="Company connections" Width="100%" Style="text-align: left" />
        </div>
    </div>
    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button id="btnMyTemplates" runat="server" Text="Create/edit job templates" OnClick="btnMyTemplatesInput_Click" Width="100%" Style="text-align: left"/>
        </div>
    </div>

    <%--<div class="row">
        <div class="small-9 medium-5 small-offset-1 end columns">
            <asp:Button ID="btnPAYE" runat="server" OnClick="btnPAYE_Click" Text="PAYE constants" Width="100%" />
        </div>
    </div>

    <div class="row">
        <div class="small-8 medium-4 small-offset-2 end columns">
            <asp:Button ID="btnPAYELowTax" runat="server" OnClick="btnPAYELowTax_Click" Text="Low Tax Bracket" Width="100%" />
        </div>
    </div>
    <div class="row">
        <div class="small-8 medium-4 small-offset-2 end columns">
            <asp:Button ID="btnPAYEHighTax" runat="server" OnClick="btnPAYEHighTax_Click" Text="High Tax Bracket" Width="100%" />
        </div>
    </div>
    <div class="row">
        <div class="small-8 medium-4 small-offset-2 end columns">
            <asp:Button ID="btnPAYELowPercent" runat="server" OnClick="btnPAYELowPercent_Click" Text="Low Tax Percent" Width="100%" />
        </div>
    </div>
    <div class="row">
        <div class="small-8 medium-4 small-offset-2 end columns">
            <asp:Button ID="btnPAYEHighPercent" runat="server" OnClick="btnPAYEHighPercent_Click" Text="High Tax Percent" Width="100%" />
        </div>
    </div>

    <div class="row">
       <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnMaterials" runat="server" OnClick="btnMaterials_Click" Text="Materials" Width="100%" />
        </div>
    </div>


    <%--<div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnCalendar" runat="server" OnClick="btnCalendar_Click" Text="Calendar" Width="100%" />
        </div>
    </div>--%>

    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnEmpInfo" runat="server" OnClick="btnEmpInfo_Click" Text="Employee Info" Width="100%" Style="text-align: left" />
        </div>
    </div>
     <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="AddOns" runat="server" OnClick="btn_AddOns_Click" Text="Entity add-ons and settings" Width="100%" Style="text-align: left"/>
        </div>
    </div>
    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btn_ImportInvoices" runat="server" OnClick="btnImport_Click" Text="Import Supplier Invoices" Width="100%"  Style="text-align: left"/>
        </div>
    </div>
   


<%--    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnHS" runat="server" OnClick="btnHS_Click" Text="Health and Safety" Width="100%" />
        </div>
    </div>--%>


    <asp:Panel ID="pnlMySites" runat="server" Visible="false">
    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnMySites" runat="server" OnClick="btnMySites_Click" Text="My Sites" Width="100%" />
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <asp:Repeater ID="rpSites" runat="server">
                <ItemTemplate>
                    <div class="row">
                        <div class="small-9 medium-5 small-offset-1 end columns">

                            <asp:Button ID="btnSelectSite" runat="server" CommandName="SelectSite" CommandArgument='<%# Eval("SiteID") %>' 
                            Text='<%# Eval("Address1") %>' OnClick="btnSelectSite_Click" Width="100%" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>
        </asp:Panel>

   <%-- <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnOffice" runat="server" OnClick="btnOffice_Click" Text="Office" Width="100%" />
        </div>
    </div>
    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnPlantRegister" runat="server" OnClick="btnPlant_Click" Text="Plant and equipment Register" Width="100%" />
        </div>
    </div>
    <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnVehicles" runat="server" OnClick="btnVehicles_Click" Text="Vehicles" Width="100%" />
        </div>
    </div>--%>
     <div class="row">
        <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btn_PB" runat="server" OnClick="btn_PB_Click" Text="Promote Business settings" Width="100%" Style="text-align: left" />
        </div>
    </div>
    
    
    
     
     <div class="row">
       <div class="small-10 medium-6 end medium-left columns">
            <asp:Button ID="btnTimeSheets" runat="server" OnClick="btnTimeSheets_Click" Text="Time Sheets" Width="100%" Style="text-align: left" />
        </div>
    </div>

</asp:Content>













 