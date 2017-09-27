<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="CompanyClose.aspx.cs" Inherits="Electracraft.Client.Website.Private.CompanyClose" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
    Close Company
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <h2>Close Company - <%# Company.DisplayName %></h2>
    <asp:PlaceHolder runat="server" ID="phConfirm">
        Click the button below to deactivate the company.
        <asp:Button ID="btnDeactivate" runat="server" OnClick="btnDeactivate_Click" />
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="phOutstanding">
        The company cannot be closed until while there are active jobs and tasks associated with it.<br />

        <asp:PlaceHolder runat="server" ID="phJobs">
            Jobs owned by this company:<br />
            <asp:Repeater ID="rpJobs" runat="server">
                <ItemTemplate>
                    <%# ((DOJob)Container.DataItem).Name %><br />
                </ItemTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="phTasks">
            Tasks that this company is a contractor on:<br />
            <asp:Repeater ID="rpTasks" runat="server">
                <ItemTemplate>
                    <%# ((DOTask)Container.DataItem).TaskName %><br />
                    Job: <%# GetJob((DOTask)Container.DataItem).Name  %>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>
    </asp:PlaceHolder>
</asp:Content>
