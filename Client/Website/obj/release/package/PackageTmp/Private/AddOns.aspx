<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="AddOns.aspx.cs" Inherits="Electracraft.Client.Website.Private.AddOns" %>

<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Accounts
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <%--<controls:ContactMenu ID="ContactMenu1" runat="server"></controls:ContactMenu>
    --%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Add-Ons and settings</h3>
        </div>
    </div>
  

        <div class="row">
            <div class="small-12 columns">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
            </div>
        </div>
    <br />
     <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkMyCustomers" runat="server" text="Show my Customers list" />
            </div>
    </div>
    <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkMyJobsWithLabour"  runat="server" Text="Show my tasks with labour and/or materials to assign" />
            </div>
    </div>
    <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkSupplierInvoices"   runat="server" text="Show supplier invoices to assign"/>
            </div>
    </div>
    <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkMyContractors"  runat="server" text="Show my contractors" />
            </div>
    </div>
    <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkMyHealth"  runat="server" visible="false" Text="Show my Fitness/Health"/>
            </div>
    </div>
     <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkMyOnlineVault"  runat="server" visible="false" Text="Show my online vault" />
            </div>
    </div>
      <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkGoals"  runat="server" visible="false" Text="Show my goals"/>
            </div>
    </div>
    <div class="row">
            <div class="small-12 columns">
                <asp:CheckBox ID="chkMyProperties"   runat="server" visible="True" Text="Show my properties"/>
            </div>
    </div>
     <div class="columns small-6" >

             <div class="row" style="height:70px" >
                 <div class="column small-5 push-4">
                     <asp:Label runat="server" ID="lbl1" Text="First level singular label:"/>
                 </div>
                 <div class="column small-5" style="height:30%">
                     <asp:TextBox runat="server" ID="txtFirst" Text="Site" />
                 </div>
             </div>

            <div class="row" style="height:70px" >
                <div class="column small-5 push-4">
                     <asp:Label runat="server" ID="Label1" Text="First level plural label:" />
                </div>
                <div class="column small-5" style="height:30%">
                     <asp:TextBox runat="server" ID="TextBox1" Text="Sites" />
                </div>       
            </div>

          <div class="row" style="height:70px" >
                 <div class="column small-5 push-4">
                     <asp:Label runat="server" ID="Label2" Text="Second level singular label:"/>
                 </div>
                 <div class="column small-5" style="height:30%">
                     <asp:TextBox runat="server" ID="TextBox2" Text="Job" />
                 </div>
             </div>

            <div class="row" style="height:70px" >
                <div class="column small-5 push-4">
                     <asp:Label runat="server" ID="Label3" Text="Second level plural label:" />
                </div>
                <div class="column small-5" style="height:30%">
                     <asp:TextBox runat="server" ID="TextBox3" Text="Jobs" />
                </div>       
            </div>
            
    
     <div class="row" style="height:70px" >
                 <div class="column small-5 push-4">
                     <asp:Label runat="server" ID="Label4" Text="Third level singular label:"/>
                 </div>
                 <div class="column small-5" style="height:30%">
                     <asp:TextBox runat="server" ID="TextBox4" Text="Task" />
                 </div>
             </div>

            <div class="row" style="height:70px" >
                <div class="column small-5 push-4">
                     <asp:Label runat="server" ID="Label5" Text="Third level plural label:" />
                </div>
                <div class="column small-5" style="height:30%">
                     <asp:TextBox runat="server" ID="TextBox5" Text="Tasks" />
                </div>       
            </div>

 </div>
</asp:Content>
