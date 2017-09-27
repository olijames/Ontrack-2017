<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="TimeSheets.aspx.cs" Inherits="Electracraft.Client.Website.Private.TimeSheets" %>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Time Sheets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Time Sheets</h3>
        </div>
    </div>
    <asp:PlaceHolder runat="server" Visible="<%# Authorised %>">
        <div class="row">
            <div class="small-12 columns">
                Viewing time sheets for week starting
                <asp:Literal ID="litStartDate" runat="server"></asp:Literal><br />
                <a href="TimeSheets.aspx?weekstarting=<%# StartDate.AddDays(-7).ToString("yyyyMMdd") %>">
                    Previous week</a> <a href="TimeSheets.aspx?weekstarting=<%# StartDate.AddDays(7).ToString("yyyyMMdd") %>">
                        Next week</a>
            </div>
        </div>
        <div class="row">
            <div class="small-12 columns">
                <asp:GridView ID="gvTimeSheets" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <%# Eval("FirstName") %>
                                <%# Eval("LastName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Hours">
                            <ItemTemplate>
                                <%# DateAndTime.DisplayShortTimeString((int)Eval("TotalMinutes")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chargeable Hours">
                            <ItemTemplate>
                                <%# DateAndTime.DisplayShortTimeString((int)Eval("ChargeableMinutes")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CommandArgument='<%# Eval("ContactID").ToString() %>'
                                    Text="View" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                        <ItemTemplate>
                        </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:PlaceHolder>

        <div class="row">
        <div class="small-12 columns">
            <a href="<%# ResolveClientUrl("~/private/contacthome.aspx") %>">Back</a>
        </div>
        </div>

</asp:Content>
