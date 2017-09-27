<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileDisplayerToolBoxFiles.ascx.cs"
    Inherits="Electracraft.Client.Website.UserControls.FileDisplayerToolBoxFiles" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<asp:Panel ID="pnlFiles" runat="server">
    <%--<asp:PlaceHolder runat="server" Visible="<%# rpFiles.Items.Count <= 0 %>">
        No files selected.
    </asp:PlaceHolder>--%>
    <asp:Repeater ID="rpFiles" runat="server">
        <HeaderTemplate>
            <table style="width: 100%;">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 60%;">
                    <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# ParentPage.CurrentBRJob.GetFilePathRelative((DOFileUpload)Container.DataItem) %>'>
                        <%# Eval ("Filename") %>
                    </asp:HyperLink></td>
                <td style="width: 20%;">
                    <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# ParentPage.CurrentBRJob.GetFilePathRelative((DOFileUpload)Container.DataItem) %>'>
                        <%# Eval ("CreatedDate") %>
                    </asp:HyperLink></td>
                <td style="width: 20%; text-align: right;">
                    <asp:Button ID="btnDelete" runat="server" CommandArgument='<%# Eval("FileID").ToString() %>'
                        OnClick="btnDelete_Click" Text="Delete" />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnDelete" ConfirmText="Are you sure you want to delete this file?"></ajaxToolkit:ConfirmButtonExtender>
                </td>
            </tr>

        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>
</asp:Panel>
<asp:Panel ID="pnlImages" runat="server">
    <asp:Repeater ID="rpImages" runat="server">
        <HeaderTemplate>
            <ul class="small-block-grid-2 medium-block-grid-3 large-block-grid-4">
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <asp:HyperLink runat="server" Target="_blank" NavigateUrl="<%# ParentPage.CurrentBRJob.GetImagePathRelative((DOFileUpload)Container.DataItem, DOFileUpload.ImageType.Standard) %>">
                    <asp:Image runat="server" AlternateText="<%# ((DOFileUpload)Container.DataItem).Filename %>"
                        ImageUrl="<%# ParentPage.CurrentBRJob.GetImagePathRelative((DOFileUpload)Container.DataItem, DOFileUpload.ImageType.Thumb) %>" />
                    <asp:Button Style="display: block" ID="btnDelete" runat="server" CommandArgument='<%# Eval("FileID").ToString() %>'
                        OnClick="btnDelete_Click" Text="Delete" />
                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete" ConfirmText="Are you sure you want to delete this file?"></ajaxToolkit:ConfirmButtonExtender>
                </asp:HyperLink>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</asp:Panel>
