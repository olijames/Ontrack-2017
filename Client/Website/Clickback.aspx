<%@ Page Title="" Language="C#" MasterPageFile="~/Default2.Master" AutoEventWireup="true" CodeBehind="Clickback.aspx.cs" Inherits="Electracraft.Client.Website.Clickback" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <asp:PlaceHolder ID="phVerificationOK" runat="server" Visible="false">
        <h4>Registration Complete.</h4>
        Thank you for confirming your account. Click <asp:HyperLink runat="server"  ForeColor="White" NavigateUrl="~/Private/Home.aspx">here</asp:HyperLink> to view and manage your jobs.
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phVerificationFailed" Visible="false">
        <h4>Error</h4>
        The verification code is invalid.
    </asp:PlaceHolder>
</asp:Content>
