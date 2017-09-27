<%@ Page Title="" Language="C#" MasterPageFile="~/Default2.Master" AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" Inherits="Electracraft.Client.Website.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
      
    Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">

<div class="login-screen">
        <div class="row">
            <div class="small-10 small-centered small-text-center medium-8 columns">
            <%--<div class="small-12 push-4">--%>
                <%--  --%>
                <p style="color: #e8e8e8; margin-top: 2em;">  <embed type="image/svg+xml" src="Scripts/foundation/font/svgs/ontrack logo.svg" /></p>
            
            </div>
        </div>
    <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">
        <div>
            <div class="row">
                <div class="small-10 small-centered medium-8 columns">
                    <asp:TextBox ID="txtUsername" runat="server" placeholder="Email Address"></asp:TextBox></div>
            </div>

            <div class="row">
                <div class="small-10 small-centered medium-8 columns">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox></div>
            </div>
            <div class="row">
                <div class="small-10 small-centered small-text-center medium-8 columns">
                    <asp:CheckBox ID="chkRememberMe" runat="server" Checked="true" Text="Remember Me" /></div>
            </div>
            <div class="row">
                <div class="small-10 small-centered medium-8 columns">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CssClass="button black" Width="100%" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="row">
        <div class="small-10 small-centered small-text-center medium-8 columns">
            <a href="RegisterIndividual.aspx" style="color: #e8e8e8;">Register</a><br />
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            <%--<asp:Button id="btnJared1" runat="server" Text="M.I." OnClick="btnMaterialInput_Click"/>--%>
        </div>
        
      </div>
</div>
</asp:Content>
