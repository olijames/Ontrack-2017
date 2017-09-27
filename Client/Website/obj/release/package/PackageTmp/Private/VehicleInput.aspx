<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
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
    <div>
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CausesValidation="False" />
    </div>
    <h3>Add/Edit a Vehicle</h3>

    <div>

        <div id="divGrid" style="padding: 10px; width: auto">

            <asp:UpdatePanel ID="vInputUpdatePanel" runat="server">
                <ContentTemplate>
                    <%--  DataKeyNames="VehicleID,VehicleDriver, DriverName,VehicleName,VehicleRegistration,WOFDueDate,RegoDueDate,InsuranceDueDate">   --%>
                    <asp:GridView ID="gvVehicleInput" runat="server" Width="90%" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="LightGray"
                        AllowPaging="true" ShowFooter="true" PageSize="10" OnRowEditing="gvVehicleInput_RowEditing" OnRowCancelingEdit="gvVehicleInput_RowCancelingEdit" OnRowUpdating="gvVehicleInput_RowUpdating"
                        OnRowDataBound="gvVehicleInput_RowDataBound" DataKeyNames="VehicleID, VehicleDriver, DriverName, VehicleName, VehicleRegistration,
                                                                                WOFDueDate, RegoDueDate, InsuranceDueDate"
                        OnDataBound="gvVehicleInput_DataBound">
                        <Columns>
                            <%-- Vehicle Driver --%>
                            <asp:TemplateField HeaderText="Driver" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDriverName" runat="server" Text='<%# Eval("DriverName") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddDriverName" runat="server" Font-Size="Small">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddDriverName" runat="server" ErrorMessage="Driver Name is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="EditGroup" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddAddDriverName" runat="server" Font-Size="Small" AppendDataBoundItems="True" AutoPostBack="False" ValidationGroup="AddGroup">
                                        <asp:ListItem Value="" Text="Select Driver" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddAddDriverName" runat="server" ErrorMessage="Driver is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Vehicle Name --%>
                            <asp:TemplateField HeaderText="Vehicle Make & Model" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicleName" runat="server" Text='<%# Eval("VehicleName") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtVehicleName" runat="server" Height="20px" Font-Size="Small" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtVehicleName" runat="server" ErrorMessage="Make & Model is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="EditGroup" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAddVehicleName" runat="server" Height="20px" Font-Size="Medium" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtAddVehicleName" runat="server" ErrorMessage="Make & Model is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Vehicle Rego --%>
                            <asp:TemplateField HeaderText="Rego">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicleRego" runat="server" Text='<%# Eval("VehicleRegistration") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtVehicleRego" runat="server" Height="20px" Font-Size="Small" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtVehicleRego" runat="server" ErrorMessage="Rego is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="EditGroup" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAddVehicleRego" runat="server" Height="20px" Font-Size="Small" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtAddVehicleRego" runat="server" ErrorMessage="Rego is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Vehicle WOF Due Date --%>
                            <asp:TemplateField HeaderText="WOF Due Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblWOFDueDate" runat="server" Text='<%# Eval("WOFDueDate", "{0:dd/M/yyyy}") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWOFDueDate" runat="server" TextMode="Date" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtWOFDueDate" runat="server" ErrorMessage="WOF due date is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="EditGroup" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAddWOFDueDate" runat="server" TextMode="Date" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtAddWOFDueDate" runat="server" ErrorMessage="WOF due date is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Vehicle Rego Due Date--%>
                            <asp:TemplateField HeaderText="Rego Due Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegoDueDate" runat="server" Text='<%# Eval("RegoDueDate", "{0:dd/M/yyyy}") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRegoDueDate" runat="server" TextMode="Date" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtRegoDueDate" runat="server" ErrorMessage="Rego due date is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="EditGroup" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAddRegoDueDate" runat="server" TextMode="Date" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtAddRegoDueDate" runat="server" ErrorMessage="Rego due date is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Vehicle Insurance --%>
                            <asp:TemplateField HeaderText="Ins Due Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsDueDate" runat="server" Text='<%# Eval("InsuranceDueDate", "{0:dd/M/yyyy}") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtInsDueDate" runat="server" TextMode="Date" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtInsDueDate" runat="server" ErrorMessage="Insurance due date is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="EditGroup" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAddInsDueDate" runat="server" TextMode="Date" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtAddInsDueDate" runat="server" ErrorMessage="Insurance due date is a required field" Text="*" Font-Bold="true" Font-Size="X-Large" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Delete Button --%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('Do you want to delete Vehicle?')"
                                        CommandArgument='<%# Eval("VehicleID") + "," + Eval("VehicleName") %>' Text="Delete" OnClick="btnDelete_Click" CausesValidation="False"></asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnAddVehicle" runat="server" Text="Add" OnClick="btnAdd_Click" CausesValidation="true" ValidationGroup="AddGroup" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Edit Button --%>
                            <asp:CommandField ShowEditButton="true" ValidationGroup="EditGroup" CausesValidation="true" />

                            <%-- Tony added following button on 21.Feb.2017--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnSetDefault" runat="server" CommandArgument='<%# Eval("VehicleID") + "," + Eval("VehicleDriver") %>'
                                        Text="Set as Default"
                                        Visible='<%# ChkDefaultVehicle((Guid)Eval("VehicleID")) %>'
                                        OnClick="btnSetDefault_Click" />
                                    <asp:Label runat="server" Text="Default" Visible='<%#!ChkDefaultVehicle((Guid)Eval("VehicleID")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <%-- Validation Summary --%>

                    <asp:ValidationSummary runat="server" ValidationGroup="EditGroup"></asp:ValidationSummary>
                    <asp:ValidationSummary runat="server" ValidationGroup="AddGroup"></asp:ValidationSummary>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <asp:UpdatePanel ID="ErrorPanel" runat="server">
            <ContentTemplate>
                <div id="divError" runat="server">
                    <asp:TextBox ID="txtError" Text="" Font-Bold="true" Font-Size="Large" runat="server" />
                    <asp:Button Text="Clear Error" ID="btnClearError" OnClick="btnClearError_Click" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--Transfer materials between vehicles --%>
        <asp:UpdatePanel ID="vTransferUpdatePanel" runat="server">
            <ContentTemplate>
                <div id="divTransfer" runat="server">
                    <h4>Transfer materials to another car before deleting?</h4>
                    <br />
                    <asp:Label Text="From:  " runat="server" />
                    <asp:DropDownList ID="ddTransferFrom" runat="server" DataTextField="" Width="300px" />
                    <asp:Label Text="  To: " runat="server" />
                    <asp:DropDownList ID="ddTransferTo" runat="server" DataTextField="" Width="300px" /><br />
                    <asp:Button ID="btnTransferMaterials" runat="server" Text="Transfer Materials" CausesValidation="False" OnClick="btnTransferMaterials_Click" />
                    <asp:Button ID="btnCancelTransfer" runat="server" Text="Cancel" OnClick="btnCancelTransfer_Click" CausesValidation="False" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <%-- Original Code
    <div class="container">
        <div class="row">

            <div class="column" style="background-color: lightyellow">
                <asp:Label ID="label3" runat="server" Text="Vehicle Driver" /><br />
                <asp:DropDownList ID="ddCompanyContacts" DataTextField="" runat="server" Width="350">                    
                </asp:DropDownList>
            </div>

            <div class="column" style="background-color: lightyellow" >
                <asp:Label ID="label1" runat="server" Text="Vehicle Make and Model" /> <br />                
                <div style="float:left">
                    <asp:TextBox ID="tbVehicleMakeModel" runat="server" Text="" Width="350"></asp:TextBox>                
                </div>
                <div style="float:left">
                    <asp:RequiredFieldValidator ID="MakeModelValidator" runat="server" ControlToValidate="tbVehicleMakeModel" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="column" style="background-color: lightyellow">
                <asp:Label ID="label2" runat="server" Text="Vehicle Registration" AssociatedControlID="tbVehicleMakeModel" />
                <div style="float:left">
                    <asp:TextBox ID="tbVehicleRegistration" runat="server" Text="" Width="350"></asp:TextBox>                    
                </div>
                <div style="float:left">
                    <asp:RequiredFieldValidator ID="RegistrationValidator" runat="server" ControlToValidate="tbVehicleRegistration" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                </div>                
            </div>

            <div class="column" style="background-color: lightyellow">
                <asp:Label ID="label4" runat="server" Text="WOF Due Date" AssociatedControlID="tbWOFDueDate"/>
                <div style="float:left">
                    <asp:TextBox ID="tbWOFDueDate" runat="server" Text="" Width="350" TextMode="Date"></asp:TextBox>
                </div>                
                <div style="float:left">
                    <asp:RequiredFieldValidator ID="WOFValidator" runat="server" ControlToValidate="tbWOFDueDate" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="column" style="background-color: lightyellow">
                <asp:Label ID="label5" runat="server" Text="Rego Due Date"/><br /> 
                <div style="float:left">
                    <asp:TextBox ID="tbRegoDueDate" runat="server" Text="" Width="350" TextMode="Date"></asp:TextBox>
                </div>                
                <div style="float:left">
                    <asp:RequiredFieldValidator ID="RegoDateValidator" runat="server" ControlToValidate="tbRegoDueDate" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="column" style="background-color: lightyellow">
                <asp:Label ID="label6" runat="server" Text="Insurance Due Date" /><br />                 
                <div style="float:left">
                    <asp:TextBox ID="tbInsuranceDueDate" runat="server" Text="" Width="350" TextMode="Date"></asp:TextBox>
                </div>
                <div style="float:left">
                    <asp:RequiredFieldValidator ID="InsuranceValidator" runat="server" ControlToValidate="tbInsuranceDueDate" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="column" style="background-color: lightyellow">
                <asp:Button ID="btnAdd" runat="server" Text="Add Vehicle" OnClick="btnAdd_Click" />
            </div>

            <div class="column" style="background-color: lightyellow">
            </div>
            <br />
        </div>
    </div>
    
    <div class="container" id="GridContainer">
        <h4>Vehicles in Use</h4>
            <asp:GridView ID="VehicleGridView" runat="server" AutoGenerateColumns="False" BorderStyle="None" GridLines="None"
                OnRowCommand="VehicleGridView_RowCommand" DataKeyNames="VehicleID,VehicleDriver, DriverName,VehicleName,VehicleRegistration,WOFDueDate,RegoDueDate,InsuranceDueDate">
                <Columns>    
                    
                    <asp:TemplateField HeaderText ="Driver">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlDriverNameGrid" runat="server"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Make & Model">
                        <ItemTemplate>
                            <asp:TextBox ID="tbMakeModelGrid" Text="" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="WOF Due Date">
                        <ItemTemplate>
                            <asp:TextBox ID="tbWOFDueDateGrid" Text="" TextMode="Date" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    
                    <%--                
                    <asp:BoundField DataField="DriverName" HeaderText="Driver" />
                    <asp:BoundField DataField="VehicleName" HeaderText="Make & Model" />
                    <asp:BoundField DataField="VehicleRegistration" HeaderText="Rego" />
                    <asp:BoundField DataField="WOFDueDate" DataFormatString="{0:d}" HeaderText="WOF Due Date" />
                    <asp:BoundField DataField="RegoDueDate" DataFormatString="{0:d}" HeaderText="Rego Due Date" />
                    <asp:BoundField DataField="InsuranceDueDate" DataFormatString="{0:d}" HeaderText="Insurance Due Date" />
                    <asp:ButtonField Text="Edit" CommandName="EditVehicle" />
                    <asp:ButtonField Text="Delete" />

                     
                </Columns>

            </asp:GridView>
    </div>
    --%>
</asp:Content>
