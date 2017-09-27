<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="RegisterSubscription.aspx.cs" Inherits="Electracraft.Client.Website.RegisterSubscription" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Register - Final Step
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
<h2>Register - Final Step</h2>
<div>
    <table>
        <tr>
            <td>Are you joining as a subscribed user?</td>
            <td><asp:CheckBox ID="chkContactSubscribed" runat="server" /></td>
        </tr>
        <tr id="trCompany" runat="server">
            <td>Is your new company a subscribed company?</td>
            <td><asp:CheckBox ID="chkCompanySubscribed" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnCompleteRegistration" runat="server" Text="Complete Registration" OnClick="btnRegister_Click" /></td>
        </tr>
    </table>
</div>
</asp:Content>
