<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobQuotes.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.JobQuotes" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>
<div class="quotes">
<asp:Repeater ID="rpQuotes" runat="server">
<ItemTemplate>
    <div class="row">
        <div class="small-12 medium-8 columns">
            <%# GetJobQuoter(Container.DataItem) %> - <%# DateAndTime.DisplayShortDate((DateTime)Eval("CreatedDate")) %>
        </div>
        <div class="small-12 medium-4 columns">
            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("QuoteStatus") == 0 %>'>
                <asp:Button ID="btnAccept" runat="server" Text="Accept" OnClick="btnAccept_Click" CommandArgument='<%# Eval("QuoteID").ToString() %>' />
                <asp:Button ID="btnDecline" runat="server" Text="Decline" OnClick="btnDecline_Click" CommandArgument='<%# Eval("QuoteID").ToString() %>' />
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("QuoteStatus") != 0 %>'>
                <%# Eval("QuoteStatus") %>
            </asp:PlaceHolder>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>
</div>

<script runat="server">
    string GetJobQuoter(object DataItem)
    {
        DOJobQuote Quote = DataItem as DOJobQuote;
        DOContact Contact = ParentPage.CurrentBRContact.SelectContact(Quote.ContactID);
        return Contact.DisplayName;
    }
</script>