<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs"
    Inherits="Electracraft.Client.Website.UserControls.UserDetails" %>
<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<div class="row">
    <div class="small-12 medium-4 columns">
        Email:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
    </div>
</div>
<asp:PlaceHolder ID="phName" runat="server">
    <div class="row">
        <div class="small-12 medium-4 columns">
            First Name:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Last Name:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
        </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phCompany" runat="server">
    <div class="row">
        <div class="small-12 medium-4 columns">
            Company Name:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Company Key:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtCompanyKey" runat="server" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Exclude other users from selecting this company as a customer
        </div>
        <div class="small-12 medium-8 columns">
            <asp:CheckBox ID="chkCustomerExclude" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Bank Account Number:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtBankAccount" runat="server"></asp:TextBox>
        </div>
    </div>
</asp:PlaceHolder>
<div class="row">
    <div class="small-12 medium-4 columns">
        Phone:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        Address 1:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:TextBox ID="txtAddress1" runat="server"></asp:TextBox>
    </div>
</div>
<%-- Tony modified on 12.Apr.2017 --%>
<div class="row">
    <div class="small-12 medium-4 columns">
        Address 2:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False"
            AutoPostBack="true" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        Address 3:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:DropDownList ID="District_DDL" runat="server" EnableTheming="False"
            AutoPostBack="true" OnSelectedIndexChanged="District_DDL_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        Address 4:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:DropDownList ID="SuburbDD" runat="server" EnableTheming="False"
            AutoPostBack="true" OnPreRender="SuburbDD_PreRender" CausesValidation="True" AppendDataBoundItems="False" Visible="True">
        </asp:DropDownList>
    </div>
</div>
<div class="row section" style="background: #f6f6f6;">
    <div class="small-12 columns ">
        Subscription Status:
        <asp:Literal ID="litSubscriptionStatus" runat="server"></asp:Literal>
    </div>
</div>
<asp:PlaceHolder ID="phAdmin" runat="server">
    <div class="row">
        <div class="small-12 medium-4 columns">
            Subscription Pending:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:CheckBox ID="chkSubscriptionPending" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Subscribed:
        </div>
        <div class="small-12 medium-8 columns">
            <asp:CheckBox ID="chkSubscribed" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            Subscription Expiry Date
        </div>
        <div class="small-12 medium-8 columns">
            <controls:DateControl ID="dateSubscription" runat="server"></controls:DateControl>
        </div>
    </div>
</asp:PlaceHolder>
