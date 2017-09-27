<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="Electracraft.Client.Website.Private.Admin.ManageUsers" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Manage Users - Admin
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<script runat="server">
    string GetUserTypeString(DOContact.ContactTypeEnum ContactType)
    {
        if (ContactType == DOContact.ContactTypeEnum.Individual)
            return "Private User";
        else
            return "Company";
        
    }

    string GetNameString(DOContact Contact)
    {
        if (Contact.ContactType == DOContact.ContactTypeEnum.Company)
            return Contact.CompanyName;
        else
            return string.Format("{0} {1}", Contact.FirstName, Contact.LastName);
    }

    string GetEditUserURL(DOContact Contact)
    {
        if (Contact.ContactType == DOContact.ContactTypeEnum.Company)
            return "CompanyDetails.aspx?contactid=" + Contact.ContactID.ToString();
        else
            return "IndividualDetails.aspx?contactid="+Contact.ContactID.ToString();
    }
</script>

<div class="row">
    <div class="small-12 columns">
        <h2>Manage Users</h2>
    </div>
</div>

<div class="row">
<div class="small-12 columns">
    <h4>Search Users</h4>
    Term: <asp:TextBox ID="txtTerm" runat="server"></asp:TextBox><br />
    User status:
    <asp:RadioButtonList ID="rblUserStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Text="Active" runat="server" Value="1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Inactive" runat="server" Value="0"></asp:ListItem>
    </asp:RadioButtonList><br />
    User type: 
    <asp:RadioButtonList ID="rblUserType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Text="Any" Value="-1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Company" Value="0"></asp:ListItem>
        <asp:ListItem Text="Private User" Value="1"></asp:ListItem>
    </asp:RadioButtonList><br />
    Subscription status: 
    <asp:RadioButtonList ID="rblSubType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Text="Any" Value="-1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Subscribed" Value="1"></asp:ListItem>
        <asp:ListItem Text="Not subscribed" Value="0"></asp:ListItem>
    </asp:RadioButtonList>
    <br />

    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />

</div>
</div>
<style>
    .loginas.Company{display:none;}
</style>
<div class="row">
<div class="small-12 columns">
    <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Username" HeaderText="Username" />
            <asp:TemplateField HeaderText="User Type" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                <ItemTemplate><%# GetUserTypeString(((DOContact)Container.DataItem).ContactType) %></ItemTemplate>
            </asp:TemplateField>            
            <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down" />
            <asp:TemplateField HeaderText="Name">   
                <ItemTemplate><%# GetNameString((DOContact)Container.DataItem) %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Subscription Expiry Date" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                <ItemTemplate><%# DateAndTime.DisplayShortDate(((DOContact)Container.DataItem).SubscriptionExpiryDate) %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down"> 
                <ItemTemplate>
                    <a href="<%# GetEditUserURL((DOContact)Container.DataItem) %>">Edit</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class='loginas <%# Eval("ContactType") %>'>
                    <asp:Button ID="btnLoginAs" runat="server" CommandArgument='<%# Eval("ContactID").ToString() %>' Text="Login as this user" OnClick="btnLoginAs_Click" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="HyperLink2" runat="server" Text="New Private User" NavigateUrl="~/Private/Admin/IndividualDetails.aspx"></asp:HyperLink>
</div>
</div>
</asp:Content>
