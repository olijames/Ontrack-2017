<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="TimeSheetDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.TimeSheetDetails" %>
<%@ Register Src="~/UserControls/DateControl.ascx" TagPrefix="controls" TagName="DateControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
Time Sheet Details
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<div class="row">
    <div class="small-12 columns">
        <h2>Time Sheet Details</h2>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        Date:
    </div>
    <div class="small-12 medium-8 columns">
        <asp:PlaceHolder ID="phAdminDate" runat="server">
            <controls:DateControl ID="dateDate" runat="server" />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phDate" runat="server">
            <asp:DropDownList ID="ddlDate" runat="server"></asp:DropDownList>
        </asp:PlaceHolder>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        Start Time
    </div>
    <div class="small-12 medium-8 columns">
        <asp:DropDownList ID="ddlStartTime" runat="server"></asp:DropDownList>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        End Time
    </div>
    <div class="small-12 medium-8 columns">
        <asp:DropDownList ID="ddlEndTime" runat="server">
        </asp:DropDownList>
    </div>
</div>
<div class="row">
    <div class="small-12 medium-4 columns">
        Comment
    </div>
    <div class="small-12 medium-8 columns">
        <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="small-12 columns">
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
    </div>
</div>
</asp:Content>
