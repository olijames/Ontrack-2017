<%@ Page Title="" Language="C#" MasterPageFile="~/Default2.Master" AutoEventWireup="true" CodeBehind="RegisterIndividual.aspx.cs" Inherits="Electracraft.Client.Website.RegisterIndividual" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="RegisterIndividual" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterCompany.ascx" TagName="RegisterCompany" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Register 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
  <div class="row">
      <br />
      <br />
      <div class="small-12 large-text-center columns" style="color:white;">
      <h2>Individual Registration</h2>
     
    <p>Enter your personal details below. (You can add a company later)</p>
     </div>
     
       <controls:RegisterIndividual ID="rgIndividual" runat="server"></controls:RegisterIndividual>
     <%-- <div class="row">
                    <div class="medium-6 columns">
<asp:Button runat="server" Text="Back to login screen" Font-Bold="true" CssClass="button radius" ForeColor="#007acc" BackColor="White"/>
                    </div>--%>
                <div class="medium-4 columns" style="float:right">
                <asp:Button ID="btnRegister"  runat="server" Font-Bold="true" CssClass="button radius" ForeColor="#007acc" BackColor="White"
                 Text="Continue" OnClick="btnRegister_Click"  />
                    </div>
      <br />
      <br />
      

      <div class="row">
          <div class="medium-3 columns" style="float:right">
          <asp:Label ID="Label1" runat="server" Text="Have an account?" ForeColor="Black"></asp:Label> <asp:HyperLink NavigateUrl="http://portal.ecraft.co.nz/" ID="HyperLink1" ForeColor="White" runat="server">Login</asp:HyperLink>
     </div>
               </div>
                    </div>
   
   <%-- <table>--%>
       <%-- <tr>
            <td><h4>Contact Details</h4></td>
        </tr>--%>
      <%--  <tr>
            <td>
                
                    </td>
        </tr>
    </table>
       <div class="small-12 large-text-center columns" style="color:white;">
          </div>
       </div>--%>
</asp:Content>
