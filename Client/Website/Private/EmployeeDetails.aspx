<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="EmployeeDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.EmployeeDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Employee Info
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <asp:PlaceHolder runat="server" Visible="<%# ShowEmployee %>">
        <div class="row">
            <div class="small-12 columns">
                <h3>
                    Employee Info</h3>
                <h4>
                    <%# Employee.DisplayName %></h4>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                First Name</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Last Name</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Email</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Phone</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Address 1</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtAddress1" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Address 2</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Pay Rate</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtPayRate" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Labour Rate</div>
            <div class="small-12 medium-8 columns">
                <asp:TextBox ID="txtLabourRate" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 medium-4 columns">
                Employee Section Access</div>
            <div class="small-12 medium-8 columns">
                <asp:CheckBox ID="chkTimeSheet" runat="server" Text="Time Sheets" /><br />
                <asp:CheckBox ID="chkEmployeeInfo" runat="server" Text="View Employee Info" /><br />
                <asp:CheckBox ID="chkEmployeeDetails" runat="server" Text="Edit Employee Details" /><br />
                <asp:CheckBox ID="chkTradeCategories" runat="server" Text="Add Trade Categories" /><br />
                <asp:CheckBox ID="chkDeleteMaterials" runat="server" Text="Delete materials from vehicle" /><br />
                <asp:CheckBox ID="chkViewImportButtons" runat="server" Text="Import a wholesaler invoice" /><br />
                <asp:CheckBox ID="chkMaterialsManually" runat="server" Text="Add manual materials to vehicles" /><br />
                <asp:CheckBox ID="chkShowInvoices" runat="server" Text="Show task invoices" /><br />
                <asp:CheckBox ID="chkCanDeleteInvoices" runat="server" Text="Delete supplier invoices" /><br />
                <asp:CheckBox ID="chkMoveMaterials" runat="server" Text="Move materials from vehicle to vehicle" /><br />
                <asp:CheckBox ID="chkShareSite" runat="server" Text="Share site with another customer" /><br />
                <asp:CheckBox ID="chkMoveJob" runat="server" Text="Move job from site to site" /><br />
                <asp:CheckBox ID="chkMoveTask" runat="server" Text="Move task from job to job" /><br />
                <asp:CheckBox ID="chkVehicles" runat="server" Text="Add and modify vehicles" /><br />
                <asp:CheckBox ID="chkAccounts" runat="server" Text="Enter accounts screen" /><br />
                <asp:CheckBox ID="chkJobTemplates" runat="server" Text="Enter job template screen" /><br />
                <asp:CheckBox ID="chkPromoteBusiness" runat="server" Text="Enter promote business screen" /><br />


            </div>
        </div>
        <div class="row">
            <div class="small-12 columns">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
