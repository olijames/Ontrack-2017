<%@ Page Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" 
CodeBehind="SiteVisibility.aspx.cs" Inherits="Electracraft.Client.Website.Private.SiteVisibility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
Site Visibility
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
<div class="row">
<div class="small-12 columns">
<h4>Site Visibility</h4>
<p>
</p>
<div>Site visible to: <asp:DropDownList ID="ddlSV" runat="server" ClientIDMode="Static" onchange="CheckShowCustomers(400)">
<asp:ListItem Value="0" Text="None"></asp:ListItem>
<asp:ListItem Value="1" Text="Selected contractors"></asp:ListItem>
<asp:ListItem Value="2" Text="All"></asp:ListItem>
</asp:DropDownList>
<small>Note: Sites are always visible to contractors with active jobs on the site.</small>
</div>
<div id="svCustomers">
<asp:Repeater ID="rpCustomers" runat="server">
<ItemTemplate>
<div>
<input type="checkbox" id='sv<%# Eval("ContactID").ToString() %>' name='sv<%# Eval("ContactID").ToString() %>' <%# VisibleContactIDs.Contains((Guid)Eval("ContactID")) ? "checked=\"checked\"" : "" %> />
<label for='sv<%# Eval("ContactID").ToString() %>'><%#  Eval("DisplayName") %></label>

</div>
</ItemTemplate>
</asp:Repeater>
</div>
<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
<asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />

</div></div>

<script type="text/javascript">
    $(document).ready(function () {
        CheckShowCustomers(1);
    });

    function CheckShowCustomers(speed) {
        if ($('#ddlSV').val() == '1') {
            $('#svCustomers').slideDown(speed);
        }
        else {
            $('#svCustomers').slideUp(speed);
        }
    }
</script>
</asp:Content>
