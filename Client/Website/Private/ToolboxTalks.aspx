<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="ToolboxTalks.aspx.cs" Inherits="Electracraft.Client.Website.Private.ToolboxTalks" %>
<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/FileDisplayerToolBoxFiles.ascx" TagName="FileDisplayer" TagPrefix="controls" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Toolbox Talks
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
<%--<controls:ContactMenu runat="server"></controls:ContactMenu>
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
     <div class="top-header">
        <div class="row">
            <div class="small-8 columns">
                <h2>
                    <asp:LinkButton ID="lnkBack" runat="server" OnClick="btnDone_Click"><i class="fi-arrow-left"></i>&nbsp;&nbsp;</asp:LinkButton>&nbsp;&nbsp;<span style="font-size: 0.9em;">Toolbox Talks</span></h2>
            </div>
        </div>
         </div>
      <asp:PlaceHolder ID="phFiles" runat="server">
           <div class="row section" style="margin-top: 1em;">
             <div class="small-12 large-6 columns">
                <h2>Files</h2>             
            </div>         
               <div class="small-12 large-4 columns">             
                 <asp:FileUpload ID="fileNew" runat="server"  AllowMultiple="true" Multiple="Multiple" CssClass="radius button tiny" />
                </div>
                              <div class="small-12 large-2 columns right"> 
               <asp:Button ID="btnUpload" CssClass="radius button small" Font-Italic="true" runat="server" Text="Upload" OnClick="btnUploadFile_Click"/>
                                            </div>
                                         <controls:FileDisplayer ID="FileDisplayer1" runat="server"></controls:FileDisplayer>
                                    </div>
    </asp:PlaceHolder>
</asp:Content>
