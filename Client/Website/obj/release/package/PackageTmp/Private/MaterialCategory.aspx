<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="MaterialCategory.aspx.cs" Inherits="Electracraft.Client.Website.Private.MaterialCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
Material Categories
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<div class="row">
    <div class="small-12 columns">
        <h2>Material Categories</h2>
        <h4><asp:Literal ID="litContactName" runat="server"></asp:Literal></h4>
    </div>
</div>
<div class="row">
    <div class="small-12 columns">
        <asp:Gridview ID="gvCategories" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="Category Name" DataField="CategoryName" />
                <asp:TemplateField HeaderText="Active">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("Active")) ? "Yes" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" CommandName="EditCategory" CommandArgument='<%# Eval("MaterialCategoryID").ToString() %>' Text="Edit" OnClick="btnEditCategory_Click" />
                        <asp:Button ID="btnDelete" runat="server" CommandName="DeleteCategory" CommandArgument='<%# Eval("MaterialCategoryID").ToString() %>' Text="Delete" OnClick="btnDeleteCategory_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:Gridview>
        <asp:Button ID="btnAddCategory" runat="server" Text="Add new category" OnClick="btnAddCategory_Click" />
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
    </div>
</div>
</asp:Content>
