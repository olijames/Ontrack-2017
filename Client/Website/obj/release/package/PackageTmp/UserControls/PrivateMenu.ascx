<%@ Control Language="C#" AutoEventWireup="true" Inherits="Electracraft.Framework.Web.UserControlBase" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<script runat="server">
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ParentPage.CurrentSessionContext == null || !ParentPage.CurrentBRContact.IsAdmin(ParentPage.CurrentSessionContext.Owner))
        {
            pnlAdmin.Visible = false;
        }
    }
</script>
        
<ul class="off-canvas-list clearfix">
    <li><label>Main Menu</label></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Private/Home.aspx" Text="Home"></asp:HyperLink></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Private/Settings.aspx" Text="Options"></asp:HyperLink></li>
    <asp:Panel ID="pnlAdmin" runat="server" >
        <li><label>Admin Menu</label></li>
        <li><asp:HyperLink runat="server" Text="Manage Users" NavigateUrl="~/Private/Admin/ManageUsers.aspx"></asp:HyperLink></li>
         <li><asp:HyperLink runat="server" Text="Settings" 
            NavigateUrl="~/Private/Admin/Settings.aspx"></asp:HyperLink>
        </li>
<%--<li>
      <asp:HyperLink runat="server" Text="Trade Categories" 
                        NavigateUrl="~/Private/Admin/TradeCategory.aspx"> </asp:HyperLink>
                
</li>--%>
    </asp:Panel>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Default.aspx?action=logout" Text="Logout"></asp:HyperLink></li>
   <li><asp:HyperLink runat="server" ID="Notifications" Visible="True" NavigateUrl="../Private/Notifications.aspx" ImageUrl="../image/share-512.png" ImageHeight="20px" ImageWidth="30px"></asp:HyperLink></li>

</ul>