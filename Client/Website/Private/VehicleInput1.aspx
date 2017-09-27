<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="VehicleInput.aspx.cs" Inherits="Electracraft.Client.Website.VehicleInput" %>

<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>

<%--<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>--%>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Vehicle Input
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
<h3>Add a new Vehicle</h3>
<div class = "container">
   <div class = "row" >
   
      <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label1" runat="server" text="Vehicle Make and Model"/> 
        <asp:TextBox ID="TextBox1" runat="server" text=""></asp:TextBox>
      </div>

       <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label2" runat="server" text="Vehicle registration"/> 
        <asp:TextBox ID="TextBox2" runat="server" text=""></asp:TextBox>
      </div>

           <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label3" runat="server" text="Vehicle Driver"/> 
        <asp:DropDownList ID="ddCompanyContacts" DataTextField="" runat="server"></asp:DropDownList>
      </div>
           <div class = "column" style = "background-color: lightyellow">
                <asp:Button ID="btnAdd" runat="server" text="Add Vehicle" OnClick="btnAdd_Click"/> 
           </div>
                


           
      </div>

   
</div>



</asp:Content>