<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="Connections.aspx.cs" Inherits="Electracraft.Client.Website.Private.Connections" %>
<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Calendar
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">

<%--<controls:ContactMenu runat="server"></controls:ContactMenu>
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Company Connections</h3>
        </div>
    </div>
    All our connected customers
    <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="displayname" />

        </Columns>


    </asp:GridView>

    All our connected contractors
    <asp:GridView ID="gvContractors" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="displayname" />

        </Columns>

    </asp:GridView>

    All our connected employees
    <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="displayname" />

        </Columns>

    </asp:GridView>




</asp:Content>
