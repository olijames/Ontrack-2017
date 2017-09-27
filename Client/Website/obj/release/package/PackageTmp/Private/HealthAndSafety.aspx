<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="HealthAndSafety.aspx.cs" Inherits="Electracraft.Client.Website.Private.HealthAndSafety" %>
<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/FileDisplayer.ascx" TagName="FileDisplayer" TagPrefix="controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Health and Safety
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
<%--<controls:ContactMenu runat="server"></controls:ContactMenu>
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Health and Safety</h3>
        </div>
    </div>
      <asp:PlaceHolder ID="phFiles" runat="server">
        <div class="row section" style="margin-top: 1em;">
            <div class="small-12 columns">
                <h2>Files</h2>
                <controls:FileDisplayer ID="FileDisplayer" runat="server"></controls:FileDisplayer>
            </div>
            <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                                    <div style="background: #f8f8f8; padding: 15px; margin-bottom: 2rem;">
                                        <div class="row">
                                            <div class="small-12 columns">
                                                Add file(s)
                    <asp:FileUpload ID="fileNew" runat="server"  AllowMultiple="true" Multiple="Multiple" />
                                                <asp:Button ID="btnUploadImage" runat="server" Text="Upload" OnClick="btnUploadFile_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
        </div>
    </asp:PlaceHolder>
</asp:Content>
