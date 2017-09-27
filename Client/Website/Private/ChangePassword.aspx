<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Electracraft.Client.Website.Private.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Change Password
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h2>My Account</h2>
        </div>
    </div>

    <div class="row section">
        <div class="small-12 columns">
            <h4 class="underline"><i class="fi-lock"></i>&nbsp;&nbsp;Change Password</h4>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Enter old password
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Enter new password
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Confirm new password
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns" style="padding-top: 1em;">
            <div class="button-panel">
                <asp:Button ID="btnChangePassword" runat="server" Text="Save" OnClick="btnChangePassword_Click" />
                <asp:Button ID="Button1" runat="server" Text="Cancel" OnClientClick="window.location.href='settings.aspx'; return false;"></asp:Button>
            </div>
        </div>
    </div>
</asp:Content>
