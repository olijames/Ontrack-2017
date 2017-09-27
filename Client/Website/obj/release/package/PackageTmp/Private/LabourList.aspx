<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="LabourList.aspx.cs" Inherits="Electracraft.Client.Website.Private.LabourList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    View Labour Items
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h2>
                Labour List</h2>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <asp:GridView ID="gvLabour" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="LabourName" DataField="LabourName" />
                    <asp:BoundField HeaderText="Cost Price" DataField="CostPrice" />
                    <asp:BoundField HeaderText="Sell Price" DataField="SellPrice" />
                    <asp:BoundField HeaderText="Description" DataField="Description" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditLabour" CommandArgument='<%# Eval("LabourID").ToString() %>'
                                OnClick="btnEdit_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <asp:Button ID="btnAddLabour" runat="server" Text="Add New Labour Item" OnClick="btnAddLabour_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    </div>
</asp:Content>
