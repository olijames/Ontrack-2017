<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobTimeSheets.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.JobTimeSheets" %>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<asp:GridView ID="gvTimeSheets" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Date">
            <ItemTemplate>
                <%# ((DOJobTimeSheet)Container.DataItem).TimeSheetDate.ToString("ddd dd/MM/yyyy") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Start Time">
            <ItemTemplate>
                <%# FormatTimeString(((DOJobTimeSheet)Container.DataItem).StartMinute) %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="End Time">
            <ItemTemplate>
                <%# FormatTimeString(((DOJobTimeSheet)Container.DataItem).EndMinute) %>
           </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Comment" DataField="Comment" />
        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <%# GetContactName(((DOJobTimeSheet)Container.DataItem).ContactID) %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="edittimesheet" CommandArgument='<%# Eval("TimeSheetID").ToString() %>' OnClick="btnEdit_Click" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:Button ID="btnAdd" runat="server" Text="Add time sheet" OnClick="btnAdd_Click" />
<script runat="server">
    string FormatTimeString(int Minute)
    {
        return string.Format("{0:D2}:{1:D2}", Minute / 60, Minute % 60);
    }
</script>