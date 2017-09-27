<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs"
    Inherits="Electracraft.Client.Website.Private.Notifications" MasterPageFile="~/Private/PrivatePage.Master" %>

<%@ Register Src="~/UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Notifications
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <asp:Repeater runat="server" ID="custDetailsRep">
        <ItemTemplate>
            <asp:Panel runat="server" ID="CustomerNotification">
                <div class="row">
                    <br />
                    <asp:Label runat="server" ID="ContractorName" Font-Bold="True" Text='<%# Eval("ContactContractor.displayname") %>'></asp:Label>
                    has added <strong> <asp:Label runat="server" Text='<%# Eval("ContactCustomer.FirstName") %>'></asp:Label>
                         <asp:Label runat="server" Text='<%# Eval("ContactCustomer.LastName") %>'></asp:Label>
                      <asp:Label runat="server" Text='<%# Eval("ContactCustomer.CompanyName") %>'></asp:Label></strong> 
                    as a customer with these details:- <br/>
   
    
                 

                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="ContractorNotification">
              <%--  You have recieved details from--%>
                <asp:Label runat="server" ID="CustomerDisplayName"></asp:Label>

                <div class="row">
                <div class="small-12 medium-8 large-8 columns">
                        <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.FirstName") %>'></asp:Label>
                         <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.LastName") %>'></asp:Label>
                        
                      <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.CompanyName") %>'></asp:Label>
                        <br />
                         <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.Address1") %>'></asp:Label>
                         <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.Address2") %>'></asp:Label>
                        <br/>
                             <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.Address3") %>'></asp:Label>
                        <br />
                             <asp:Label runat="server" Text='<%# Eval("ContractorCustomer.Address4") %>'></asp:Label>
                     <br/>  <br/> <strong >  Do you want to send them your details? </strong> 
                     <asp:Button runat="server" ID="SendCustomerDetailsToContractor_btn" Text="Send" CssClass="button radius small" 
                         OnClick="SendCustomerDetailsToContractor_btn_OnClick" />
                    <asp:Button runat="server" ID="No" Text="No" CssClass="button radius small" OnClick="No_OnClick" />
                    <asp:Button runat="server" ID="ViewDetail" Text="View my details" CssClass="button radius small" OnClick="No_OnClick" />

                         </div>
                   </div>

            </asp:Panel>
        </ItemTemplate>
    </asp:Repeater>



</asp:Content>
