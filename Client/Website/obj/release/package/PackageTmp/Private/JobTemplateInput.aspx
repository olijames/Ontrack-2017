<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="JobTemplateInput.aspx.cs" Inherits="Electracraft.Client.Website.JobTemplateInput" %>

<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>

<%--<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>--%>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
     Job Template
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
<h3>Create a new Job Template</h3>
<div class = "container">
    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
   <div class = "row" >
   
        <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label1" runat="server" text="Template name"/> 
        <asp:TextBox ID="tbJobTemplateName" runat="server" text=""></asp:TextBox>
        </div>

       
        <div class = "column" style = "background-color: lightyellow">
            <asp:Button ID="btnAddJobTemplate" runat="server" text="Add Template" OnClick="btnAddJobTemplate_Click"/> 
        </div>
                


           
      </div>

   
</div>


    

    <h3>Create a new Template task</h3>
<div class = "container">
   <div class = "row" >
   
                    

<div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label2" runat="server" text="Task name"/> 
        <asp:TextBox ID="tbTaskName" runat="server" text=""></asp:TextBox>
        </div>
       <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label4" runat="server" text="Task Description"/> 
        <asp:TextBox ID="tbTaskDescription" runat="server" text="" TextMode="MultiLine"></asp:TextBox>
        </div>
      

        <div class = "column" style = "background-color: lightyellow">
                     <asp:Label ID="label3" runat="server" text="Select Trade Category"/> 
                     <asp:DropDownList ID="ddTradeCategory" runat="server" DataTextField=""> <%--OnPreRender="TradeCategoryAll_PreRender" AutoPostBack="true">--%></asp:DropDownList>
        </div>

        <div class = "column" style = "background-color: lightyellow">
                    <asp:Label ID="label7" runat="server" text="Select Template files"/> <br />
                    <asp:Label ID="label8" runat="server" text="File function here" Font-Bold="true" Font-Size="Large"/> 

        </div>




        <div class = "column" style = "background-color: lightyellow">
            <asp:Button ID="btnAddTemplateTask" runat="server" text="Add Template Task" OnClick="btnAddTemplateTask_Click"/> 
        </div>
                


           
      </div>

   
</div>




<h3>Assign a Task to a Job Template</h3>
<div class = "container">
   <div class = "row" >
   
             
      

       <div class="row">
                    <div class="small-12 medium-4 columns">
                        Select Job Template
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="ddJobTemplates" runat="server" OnPreRender="JobTemplates_PreRender"
                             AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="ddTemplateTasks" runat="server" DataTextField=""> <%--OnPreRender="TemplateTasks_PreRender" AutoPostBack="true">--%></asp:DropDownList>
                    </div>
            <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label5" runat="server" text="Start delay(days) from start of job"/> 
        <asp:TextBox ID="tbStartDelay" runat="server" text="0"></asp:TextBox>
        </div>

       <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label6" runat="server" text="Task duration(days)"/> 
        <asp:TextBox ID="tbDuration" runat="server" text="7"></asp:TextBox>
        </div>

        </div>
             <div class = "column" style = "background-color: lightyellow">
            <asp:Button ID="btnAddTaskToJobTemplate" runat="server" text="Add Task to Job Template" OnClick="btnAddTaskToJobTemplate_Click"/> 
        </div>
         

           
      </div>

   
</div>






</asp:Content>