<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="TaskSummary.aspx.cs" Inherits="Electracraft.Client.Website.Private.TaskSummary" %>

<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/FileDisplayer.ascx" TagName="FileDisplayer" TagPrefix="controls" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    <asp:Literal ID="pruna" runat="server" Text="ds"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <asp:HiddenField ID="hidFindNewContractorID" runat="server" />
    <asp:HiddenField ID="hidPendingContractorEmail" runat="server" />
    <div class="top-header">
        <div class="row">
            <div class="small-8 columns">
                <h2>
                    <asp:LinkButton ID="lnkBack" runat="server" OnClick="btnCancel_Click"><i class="fi-arrow-left"></i>&nbsp;&nbsp;</asp:LinkButton>&nbsp;&nbsp;<span style="font-size: 0.9em;">Edit Task</span></h2>
            </div>
            <div class="small-4 columns text-right">
                <h2>
                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="btnTaskDetails_Click" Style="margin-right: 0.5em;"><i class="fi-pencil"></i></asp:LinkButton>
                    <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                        <asp:LinkButton ID="lnkDeleteTask" runat="server" OnClick="btnDeleteTask_Click"><i class="fi-trash"></i></asp:LinkButton>


                        <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="lnkDeleteTask"
                            ConfirmText="Are you sure you want to delete this task?">
                        </ajaxToolkit:ConfirmButtonExtender>
                    </asp:PlaceHolder>
                </h2>
            </div>
        </div>
        <div class="visible-for-medium-up">
            <asp:LinkButton ID="lnkCreateInvoice" runat="server" OnClick="CreateInvoice" Text="Invoice..." Visible="false"></asp:LinkButton>
        </div>
        <div class="row">
            <div class="small-12 columns">
                <asp:Panel ID="pnlContactHomeButtons" runat="server" CssClass="button-panel" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                    <div>
                        <div class="small-12 columns">
                            <%--                            <asp:LinkButton ID="lnkSave" runat="server" OnClick="btnSave_Click" CssClass="mob-btn"><i class="fi-save show-for-small-only"></i><span class="hide-for-small-only">Save</span></asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="cbeCompleteJob" runat="server" TargetControlID="lnkSave"
                                ConfirmText="The current status of this job is completed. Saving this task will change the job status to incomplete. Do you wish to proceed?">
                            </ajaxToolkit:ConfirmButtonExtender>--%>
                            <%--                            <asp:LinkButton ID="lnkAmendTask" runat="server" OnClick="btnSave_Click" CssClass="mob-btn"><i class="fi-save show-for-small-only"></i><span class="hide-for-small-only">Amend</span></asp:LinkButton>--%>
                            <%--   <asp:LinkButton ID="lnkCompleteTask" runat="server" OnClick="btnCompleteTask_Click" CssClass="mob-btn"><i class="fi-check show-for-small-only"></i><span class="hide-for-small-only">Complete</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkUncompleteTask" runat="server" OnClick="btnUncompleteTask_Click" CssClass="mob-btn"><i class="fi-refresh show-for-small-only"></i><span class="hide-for-small-only">Uncomplete</span></asp:LinkButton>--%>
                            <asp:PlaceHolder ID="phSubmitQuote" runat="server">
                                <asp:LinkButton ID="lnkSubmitQuote" runat="server" OnClientClick="$('#pnlTaskQuoteDetails').slideDown()" CssClass="mob-btn"><i class="fi-download show-for-small-only"></i><span class="hide-for-small-only">Submit Quote</span></asp:LinkButton>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                    <div class="hide">
                        <%--                        <asp:Button ID="btnSave" runat="server" Text="Save Task" OnClick="btnSave_Click" />
                        <ajaxToolkit:ConfirmButtonExtender ID="cbeCompleteJob1" runat="server" TargetControlID="btnSave"
                            ConfirmText="The current status of this job is completed. Saving this task will change the job status to incomplete. Do you wish to proceed?">
                        </ajaxToolkit:ConfirmButtonExtender>--%>
                        <%--                        <asp:Button ID="btnAmendTask" runat="server" Text="Amend Task" OnClick="btnSave_Click" />--%>
                        <asp:Button ID="btnCompleteTask" runat="server" Text="Complete Task" OnClick="btnCompleteTask_Click" />
                        <asp:Button ID="btnUncompleteTask" runat="server" Text="Uncomplete Task" OnClick="btnUncompleteTask_Click" />
                        <asp:Button ID="btnDeleteTask" runat="server" OnClick="btnDeleteTask_Click" Text="Delete Task" />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnDeleteTask"
                            ConfirmText="Are you sure you want to delete this task?">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <input type="button" value="Submit Quote..." onclick="$('#pnlTaskQuoteDetails').slideDown()" />

                        <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <div class="row hide">
        <div class="small-12 columns">
            <asp:PlaceHolder ID="phNew" runat="server">
                <h2>Add New Task</h2>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phEdit" runat="server">
                <h2>Edit Task</h2>
            </asp:PlaceHolder>
        </div>
    </div>
    <div class="row section">
        <div class="small-12 columns">
            <%-- <h4 class="underline"><i class="fi-torso"></i>&nbsp;&nbsp;Task Details  </h4>--%>
            <h4 class="underline"><%# TradeCategory.TradeCategoryName %> - 
                <%# contractor.DisplayName %> for 
                <%# customer.DisplayName %></h4>
            <asp:PlaceHolder ID="phTaskQuote" runat="server">
                <span style="color: Red;">This task has been submitted as a quote.</span>
                <asp:Literal ID="litQuoteStatus" runat="server"></asp:Literal>
                <asp:Button ID="btnDeleteQuote" runat="server" Text="Delete Quote" OnClick="btnDeleteQuote_Click" Visible="<%# CurrentSessionContext.CurrentContact.Active %>" />
            </asp:PlaceHolder>

            <div class="row">
                <div class="small-12 medium-4 columns">
                    Task Name
                </div>
                <div class="small-12 medium-8 columns">
                    <%--<asp:TextBox ID="txtTaskName" runat="server"></asp:TextBox>--%>
                    <%# Task.TaskName %>
                </div>
            </div>
            <%--            <div class="row">
                <div class="small-12 medium-4 columns">
                    Amended Date
                </div>
                <div class="small-12 medium-8 columns">
                    <p>
                        <asp:Literal ID="litAmendedDate" runat="server"></asp:Literal>
                    </p>
                </div>
            </div>--%>
            <%--            <div class="row">
                <div class="small-12 medium-4 columns">
                    Task Type
                </div>
                <div class="small-12 medium-8 columns">--%>
            <%--            <asp:DropDownList ID="ddlTaskType" runat="server">
                <asp:ListItem Text="Standard" Value="0"></asp:ListItem>
                <asp:ListItem Text="Reference" Value="1"></asp:ListItem>
                <asp:ListItem Text="Acknowledgement" Value="2"></asp:ListItem>
            </asp:DropDownList>
            --%>
            <%--                    <asp:DropDownList ID="ddlTaskType" runat="server">
                    </asp:DropDownList>
                </div>
            </div>--%>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Task Description


                </div>
                <div class="small-12 medium-8 columns">
                    <%--<asp:TextBox ID="txtTaskDescription" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>--%>
                    <%# Task.Description %>
                </div>
            </div>
            <%-- <div class="row">
                <div class="small-12 medium-4 columns">
                    TradeCategory


                </div>
                <div class="small-12 medium-8 columns">
                   <%# TradeCategory.TradeCategoryName %>
                </div>
            </div>--%>
            <%--<div class="row">
                <div class="small-12 medium-4 columns">
                   Contractor


                </div>
                <div class="small-12 medium-8 columns">
                    <%# contractor.DisplayName %>
                </div>
            </div>--%>
            <%--            <div class="row">
                <div class="small-12 medium-4 columns">
                    Contractor
                        <label style="display:inline-block"><i class="has-tip tooltip-info" data-tooltip aria-haspopup="true" title="If you are a subscribed user, you are able to add anyone as a contractor">
                            <img src="../image/ico-question.png" /></i></label>

                </div>
                <div class="small-12 medium-8 columns">
                    <asp:DropDownList ID="ddlContractor" runat="server">
                    </asp:DropDownList>
                </div>
            </div>--%>
            <%--            <asp:PlaceHolder ID="phFindNew" runat="server">
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Find new contractor
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtFindNewContractor" runat="server">
                        </asp:TextBox>
                        <asp:Button ID="btnFindNewContractor" runat="server" Text="Find" OnClick="btnFindNewContractor_Click" />
                    </div>
                </div>

            </asp:PlaceHolder>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Invoice To
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:DropDownList ID="ddlInvoiceTo" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Start Date / Time
                </div>
                <div class="small-12 medium-4 columns">
                    <controls:DateControl ID="dateStartDate" ClientIDMode="Static" onchange="checkAppointmentEnabled()"
                        runat="server" />
                </div>
                <div class="small-12 medium-4 columns">
                    <asp:DropDownList ID="ddlStartTime" runat="server" ClientIDMode="Static" onchange="checkAppointmentEnabled()">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 hide-for-small-only columns">
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:CheckBox ID="chkAppointment" runat="server" ClientIDMode="Static" />
                    Appointment
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    End Date / Time
                </div>
                <div class="small-12 medium-4 columns">
                    <controls:DateControl ID="dateEndDate" runat="server" />
                </div>
                <div class="small-12 medium-4 columns">
                    <asp:DropDownList ID="ddlEndTime" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <asp:PlaceHolder ID="phLMVisibility" runat="server">
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Labour and Materials
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="ddlLMVisibility" runat="server">
                            <asp:ListItem Text="My company only" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Visible to all" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnLMVisibilityUpdate" runat="server" Text="Update Task Visibility"
                            OnClick="btnLMVisibilityUpdate_Click" />
                    </div>
                </div>
            </asp:PlaceHolder>--%>
        </div>
    </div>

    <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
        <div class="row section">
            <div class="small-12 columns">
                <h4 class="underline"><i class="fi-clipboard-pencil"></i>&nbsp;&nbsp;Materials</h4>
                Delete to vehicle:<asp:DropDownList ID="ddVehicleSelect" runat="server" AutoPostBack="true"></asp:DropDownList>
                <%--  OnSelectedIndexChanged="ddVehicleSelect_SelectedIndexChanged" --%>
                <asp:Panel ID="pnlMaterials" runat="server">
                    <div class="row">
                        <div class="small-12 columns">
                            <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="TaskMaterialID, InvoiceID" OnPreRender="ColourMaterialLines">
                                <%--OnPreRender="RenameDeleteLabels"--%>
                                <Columns>
                                    <%-- Tony added 14.1.2017 begin--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input type="checkbox" id="chkAllOrNoneMaterial" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectMaterial" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Tony added 14.1.2017 end--%>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("MaterialName") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty.">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("Quantity").ToString() %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sell Price">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# ((decimal)Eval("SellPrice")).ToString("C2") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("Description").ToString() %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("MaterialType").ToString() %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entered By" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <%# GetEmployeeName(((DOTaskMaterialInfo)Container.DataItem).CreatedBy) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Entered On" DataField="CreatedDate" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down" DataFormatString="{0:dd-MM-yy}" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteMaterial" runat="server" CommandArgument='<%# Eval("TaskMaterialID").ToString() %>'
                                                Text="Delete" OnClick="btnDeleteMaterial_Click" Enabled="<%# ((DOTaskMaterialInfo
                                                    )Container.DataItem).Active %>" />
                                        </ItemTemplate>



                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Quoted" DataField="QuoteStatus" runat="server"/>--%>
                                    <%-- <asp:BoundField HeaderText="Invoiced" DataField="InvoiceStatus" runat="server"/>--%>
                                    <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox  HeaderText="Select"   runat="server" id="chkMaterialSelect"  Checked="true" />
                                    </ItemTemplate> 
                                </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="phAddMaterial" runat="server">
                        <div class="row section">
                            <div class="small-12">
                                <div style="background: #666; color: #fff; padding: 0.5em; margin-bottom: 1em;"><span class="fi-plus"></span>&nbsp;&nbsp;Add Material</div>

                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Category / Name:
                                    </div>
                                    <div class="small-12 medium-4 columns">
                                        <%--                                    <asp:DropDownList ID="ddlMaterialCategory" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                        --%>
                                        <asp:DropDownList ID="ddlMaterialCategory" runat="server" onchange="GetMaterials($(this).val()); CheckShowNewMaterial()">
                                        </asp:DropDownList>


                                    </div>
                                    <div class="small-12 medium-4 columns">
                                        <select name="ddlMaterials" id="ddlMaterials" class="ddl-materials">
                                        </select>
                                        <%--                                    <asp:DropDownList ID="ddlMaterials" class="ddl-materials" runat="server">
                                    </asp:DropDownList>
                                        --%>
                                    </div>

                                </div>
                                <div id="new-material-form">
                                    <hr />
                                    <controls:MaterialForm ID="MaterialForm" runat="server"></controls:MaterialForm>
                                    <hr />

                                </div>
                                <%--                            <asp:PlaceHolder ID="phNewMaterial" runat="server">
                                <hr />
                                <controls:MaterialForm ID="MaterialForm" runat="server"></controls:MaterialForm>
                                <hr />
                            </asp:PlaceHolder>
                                --%>
                                <div class="row">
                                    <div class="small-4 columns">
                                        Material Type:
                                    </div>
                                    <div class="small-8 columns">
                                        <asp:DropDownList ID="ddlMaterialType" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-4 columns">
                                        Qty:
                                    </div>
                                    <div class="small-8 columns">
                                        <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-4 columns">
                                        Desc.:
                                    </div>
                                    <div class="small-8 columns">
                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Height="200"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-4 hide-for-small-only columns">
                                    </div>
                                    <div class="small-8 columns">
                                        <asp:Button ID="btnAddMaterial" runat="server" Text="Add Material" OnClick="btnAddMaterial_Click" />
                                        &nbsp;
                                    <asp:Button ID="btnAddFromInvoice" runat="server" Text="Add Material From Invoice..." OnClick="btnAddFromInvoice_Click"/>
                                        &nbsp;
                                    <asp:Button ID="btnAddFromVehicle" runat="server" Text="Add Material From Vehicle..." OnClick="btnAddFromVehicle_Click"/>
                                    
                                        
                                        <%--Jared 30.1.17
                                        <asp:Button ID="Button2" runat="server" Text="Add Material From Invoice..." OnClick="btnAddFromInvoice_Click" visible="false"/>
                                        &nbsp;
                                    <asp:Button ID="btnAddFromVehicle" runat="server" Text="Add Material From Vehicle..." OnClick="btnAddFromVehicle_Click" visible="false" />--%>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </asp:PlaceHolder>

                </asp:Panel>

            </div>
        </div>

        <div class="row section">
            <div class="small-12 columns">
                <h4 class="underline"><i class="fi-torso-business"></i>&nbsp;&nbsp;Labour</h4>
                <asp:Panel ID="pnlLabour" runat="server">
                    <div class="row">
                        <div class="small-12 columns">
                            <asp:GridView ID="gvLabour" runat="server" AutoGenerateColumns="false" Width="100%" OnPreRender="ColourLabourLines" DataKeyNames="TaskLabourID, InvoiceID">
                                <Columns>
                                    <%-- Tony added 14.1.2017 begin--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input type="checkbox" id="chkAllOrNoneLabour" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectLabour" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Tony added 14.1.2017 end--%>
                                    <asp:TemplateField HeaderText="Date" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# ((DOTaskLabourInfo)Container.DataItem).LabourDate.ToString("dd-MM-yy") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# DateAndTime.DisplayShortTimeString(
                                    ((DOTaskLabourInfo)Container.DataItem).EndMinute -
                                    ((DOTaskLabourInfo)Container.DataItem).StartMinute ) %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("LabourType").ToString() %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contractor / Employee">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# GetEmployeeName(((DOTaskLabourInfo)Container.DataItem).ContactID) %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("LabourRate") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# Eval("Description") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entered By" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down">
                                        <ItemTemplate>
                                            <%# GetEmployeeName(((DOTaskLabourInfo)Container.DataItem).CreatedBy) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Entered On" DataField="CreatedDate" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down" DataFormatString="{0:dd-MM-yy}" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteLabour" runat="server" CommandArgument='<%# Eval("TaskLabourID").ToString() %>'
                                                Text="Delete" OnClick="btnDeleteLabour_Click" Enabled="<%# ((DOTaskLabourInfo)Container.DataItem).Active %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Quoted" DataField="QuoteStatus" runat="server"/>--%>
                                    <%-- <asp:BoundField HeaderText="Invoiced" DataField="InvoiceStatus" runat="server"/>--%>
                                    <%-- <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox  HeaderText="Select"   runat="server" id="chkLabourSelect"  Checked="true" />
                                    </ItemTemplate> 
                                </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row section">
                        <div class="small-12">
                            <div style="background: #666; color: #fff; padding: 0.5em; margin-bottom: 1em;"><span class="fi-plus"></span>&nbsp;&nbsp;Add Labour</div>
                            <asp:PlaceHolder ID="phAddLabour" runat="server">
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Date:
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <controls:DateControl ID="dateLabourDate" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Type:
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <asp:DropDownList ID="ddlLabourType" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Employee
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <asp:DropDownList ID="ddlLabourContactID" runat="server" ClientIDMode="Static" onchange="CheckShowNewEmployee()">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="NewEmployee" style="display: none">
                                    <hr />
                                    <div class="row">
                                        <div class="small-12 columns">
                                            <controls:UserDetails ID="NewUserDetails" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="small-12 medium-4 columns">
                                            Pay Rate
                                        </div>
                                        <div class="small-12 medium-8 columns">
                                            <asp:TextBox ID="txtPayRate" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="small-12 medium-4 columns">
                                            Labour Rate
                                        </div>
                                        <div class="small-12 medium-8 columns">
                                            <asp:TextBox ID="txtLabourRateNew" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Description
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <asp:TextBox ID="txtLabourDesc" runat="server" TextMode="MultiLine" Height="200"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Time
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <asp:DropDownList ID="ddlLabourTime" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Chargeable
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <p>
                                            <asp:CheckBox ID="chkChargeable" runat="server" Checked="true" />
                                        </p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns">
                                        Labour Rate
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <asp:TextBox ID="txtLabourRate" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="small-12 medium-4 columns hide-for-small-only">
                                    </div>
                                    <div class="small-12 medium-8 columns">
                                        <asp:Button ID="btnAddLabour" runat="server" Text="Add Labour" OnClick="btnAddLabour_Click" />
                                    </div>
                                </div>
                                <%--            <div class="row">
                <div class="small-12 medium-4 columns">
                    &nbsp;
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:Button ID="btnAdd15" runat="server" OnClick="btnAddLabour_Click" Text="Add 15min" />
                    <asp:Button ID="btnAdd60" runat="server" OnClick="btnAddLabour_Click" Text="Add 1hr" />
                    <asp:Button ID="btnAdd300" runat="server" OnClick="btnAddLabour_Click" Text="Add 5hrs" />
                    <asp:CheckBox ID="chkChargeable" runat="server" Checked="true" />
                    Chargeable
                </div>
            </div>
                                --%>
                            </asp:PlaceHolder>
                        </div>
                    </div>

                </asp:Panel>

            </div>

        </div>

    </asp:PlaceHolder>


    <asp:PlaceHolder ID="phInvoices" runat="server" Visible="false">
        <div class="row section">
            <div class="small-12 columns">
                <h4 class="underline"><i class="fi-torso-business"></i>&nbsp;&nbsp;Invoices</h4>
                <asp:Panel ID="pnlInvoices" runat="server">
                    <div class="row">
                        <div class="small-12 columns">
                            <!-- CONTRACTOR -->
                            <asp:GridView ID="gvContractorInvoices" runat="server" AutoGenerateColumns="false" Width="100%" Caption="My Outgoing invoices" DataKeyNames="InvoiceID" OnPreRender="preRenderContractorChild" OnRowDeleting="gvDeleteContractorRow" Visible="false">
                                <Columns>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <img class="panal-hider" alt="" style="cursor: pointer" src="../image/enter.png" width="20" />
                                            <asp:Panel ID="Panel1" runat="server" class="hide toggle-hide">


                                                <%-- <img alt = "" style="cursor: pointer" src="images/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                --%>

                                                <asp:Label ID="l1" runat="server" Text="Materials"></asp:Label>

                                                <%--CONTRACTOR MATERIALS--%>
                                                <asp:GridView ID="gvContractorInvoiceMaterials" runat="server" AutoGenerateColumns="false" DataKeyNames="TaskMaterialID" OnPreRender="PreRendergvInvoiceMaterials" OnRowEditing="gvContractorInvoiceMaterials_RowEditing">
                                                    <Columns>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblQtyByStaffHeader" Text="Original quantity" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblQtyHeader" Text="Quantity for invoice" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQuantity" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblCostPriceHeader" Text="Cost Price ($)" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCostPrice" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblSellPriceHeader" Text="Sell Price ($)" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSellPrice" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>

                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblMNameHeader" Text="Material" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMaterialName" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblEmployeeHeader" Text="Employee" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmployee" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblDateHeader" Text="Date" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblDescriptionHeader" Text="Description" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDescription" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                                <asp:Label ID="l2" runat="server" Text="Labour items"></asp:Label>

                                                <!--CONTRACTOR LABOUR-->
                                                <asp:GridView ID="gvContractorInvoiceLabours" runat="server" AutoGenerateColumns="false" DataKeyNames="TaskLabourID" OnPreRender="PreRendergvInvoiceLabours">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLabourQuantityHeader" Text="Quantity worked" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLabourQuantity" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLabourQuantityHeader" Text="Quantity Invoiced" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLabourQuantity" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>

                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLabourChargeHeader" Text="Rate ($)" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLabourCharge" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>

                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLabourEmployeeHeader" Text="Employee" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLabourEmployee" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>

                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLabourDateHeader" Text="Labour Date" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLabourDate" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>

                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLabourDescHeader" Text="Description" runat="server" />
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLabourDesc" Text="" runat="server" ForeColor="Blue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomerHeader" Text="Customer" runat="server" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" Text="" runat="server" ForeColor="Blue" />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDCHeader" Text="Date created" runat="server" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblDateCreated" Text="" runat="server" ForeColor="Blue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDSHeader" Text="Date sent" runat="server" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblDateSent" Text="" runat="server" ForeColor="Blue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStatusHeader" Text="Status" runat="server" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Text="" runat="server" ForeColor="Blue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>

                                        <HeaderTemplate>
                                            <asp:Label ID="lblAmountHeader" Text="Amount (gst ex)" runat="server" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" Text="" runat="server" ForeColor="Blue" />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSend" Text="Options" runat="server" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <%-- Tony testing --%>
                                            <asp:Button ID="btnSend" Text="Send To Xero" runat="server" OnClick="ExportInvoiceToXero" />
                                            <asp:Button ID="Button1" Text="Create Xero CSV text" runat="server" OnClick="ExportInvoiceToXero" />
                                            <%--                                            <asp:Button ID="btnBrowse" Text="Location of keys" runat="server" OnClick="browseDirectory" />--%>
                                            <%--                                            <asp:Button ID="btnXeroTest" Text="XeroTest" runat="server" OnClick="XeroTest" />--%>
                                            <asp:Button ID="btnDeleteInvoice" Text="Delete" runat="server" OnClick="btnDeleteInvoice" Enabled="false" />
                                            <asp:Button ID="btnSave" Text="Update invoice" runat="server" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnStatusUp" Text="Status up" runat="server" OnClick="btnStatusUp_Click" />
                                            <asp:Button ID="btnStatusDown" Text="Status down" runat="server" OnClick="btnStatusDown_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                            <asp:GridView ID="gvCustomerInvoices" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="InvoiceID, CustomerID, ContractorID, DueDate, InvoiceStatus" Caption="My Incoming invoices" Visible="false">
                                <Columns>
                                    <asp:BoundField DataField="InvoiceID" runat="server" HeaderText="Invoice ID" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <%-- <asp:literal enableviewstate='false' runat="server" ID="litScreenDump"/>--%>
                <asp:TextBox EnableViewState='false' runat="server" ID="txtScreenDump" TextMode="MultiLine" MaxLength="100000" />

            </div>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phFiles" runat="server" Visible="true">
        <div class="row section" style="margin-top: 1em;">
            <div class="small-12 columns">
                <h2>Files</h2>
                <controls:FileDisplayer ID="FileDisplayer1" runat="server"></controls:FileDisplayer>
            </div>
            <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                <div style="background: #f8f8f8; padding: 15px; margin-bottom: 2rem;">
                    <div class="row">
                        <div class="small-12 columns">
                            Add file(s)
                    <asp:FileUpload ID="fileNew" runat="server" AllowMultiple="true" Multiple="Multiple" />
                            <asp:Button ID="btnUploadImage" runat="server" Text="Upload" OnClick="btnUploadFile_Click" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
        </div>
    </asp:PlaceHolder>

    <%-- Tony test --%>
    <%-- Tony test --%>

    <div id="pnlTaskQuoteDetails" style="display: none">
        <div class="row">
            <div class="small-12 columns">
                Terms and Conditions
            </div>
            <div class="small-12 columns">
                <asp:TextBox ID="txtTermsAndConditions" runat="server" TextMode="MultiLine">
                </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="small-12 columns">
                <asp:Button ID="btnSubmitQuote" runat="server" Text="Submit Quote" OnClick="btnSubmitQuote_Click" />
            </div>
        </div>
    </div>

    <script runat="server">
        string FormatTimeString(int Minute)
        {
            return string.Format("{0:D2}:{1:D2}", Minute / 60, Minute % 60);
        }
    </script>
    <script type="text/javascript">

        //Tony added 15.Jan.2017 for check all boxes function : begin
        //        $('#chkAllOrNoneMaterial, #chkAllOrNoneLabour').change(function () {
        //            //            var checkboxes = $(this).closest('form').find(':checkbox');
        //            var checkboxes = $(this).closest('div.small-12').find(':checkbox');
        //
        //            if ($(this).is(':checked')) {
        //                checkboxes.prop('checked', true);
        //            } else {
        //                checkboxes.prop('checked', false);
        //            }
        //        });

        //        $('#chkAllOrNoneMaterial, #chkAllOrNoneLabour').change(function () {
        //            //            var checkboxes = $(this).closest('form').find(':checkbox');
        //            var checkboxes = $(this).closest('div.small-12').find(':checkbox');
        //
        //            if ($(this).is(':checked')) {
        //                checkboxes.prop('checked', true);
        //            } else {
        //                checkboxes.prop('checked', false);
        //            }
        //        });

        //tony testing
        $('#chkAllOrNoneMaterial, #chkAllOrNoneLabour').change(function () {
            //            var checkboxes = $(this).closest('form').find(':checkbox');
            var checkboxes = $(this).closest('div.small-12').find(':checkbox');

            if ($(this).is(':checked')) {
                checkboxes.not(':disabled').prop('checked', true);
            } else {
                checkboxes.prop('checked', false);
            }
        });

        //Tony added 15.Jan.2017 for check all boxes function : begin

        $(document).ready(function () {

            $('#dateDateControlInput').on('change', function (e) { checkAppointmentEnabled(); });
            $('#txtDateControlDate').on('change', function (e) { checkAppointmentEnabled(); });

            checkAppointmentEnabled();
            GetMaterials($('[id$=ddlMaterialCategory]').val());

            CheckShowNewMaterial();
        });

        function CheckShowNewEmployee() {
            var Show = $('#ddlLabourContactID').val() == '<%# Constants.Guid_DefaultUser.ToString() %>';
            if (Show) {
                $('#NewEmployee').slideDown();
            }
            else {
                $('#NewEmployee').slideUp();
            }
        }
        function checkAppointmentEnabled() {

            var date = $('#dateDateControlInput').val();
            if (typeof date === 'undefined')
                date = $('#txtDateControlDate').val();
            var time = $('#ddlStartTime').val();

            var check = $('#chkAppointment');
            var disabled = (date == '' || time == '-1');
            check.prop('disabled', disabled);
            if (disabled) {
                check.prop('checked', false);
            }
            //        alert('Date: ' + date + '\r\nTime: ' + time + '\r\nAppointment: ' + check.is(':checked'));
        }

        function GetMaterials(cat) {
            if (cat != '') {
                $.ajax({
                    type: "POST",
                    url: "TaskDetails.aspx/GetMaterialCategoryItems",
                    data: "{'MaterialCategoryID':'" + cat + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var res = jQuery.parseJSON(msg.d);
                        var catDDL = $('.ddl-materials');
                        catDDL.empty();
                        $.each(res, function (key, val) {
                            catDDL.append($('<option></option>').val(key).html(val));
                        });
                    }
                });
            }
        }

        function CheckShowNewMaterial() {
            var cat = $('[id$=ddlMaterialCategory]').val();
            if (cat == 'ffffffff-ffff-ffff-ffff-ffffffffffff' || cat == 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF') {
                $('#new-material-form').slideDown();
            }
            else {
                $('#new-material-form').slideUp();
            }
        }
    </script>
    <%--  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "images/plus.png");
        $(this).closest("tr").next().remove();
    });

    
</script>--%>



    <script type="text/javascript">

        $(".panal-hider").click(function () {
            var panal = $(this).parent().find(".toggle-hide");
            if (panal.hasClass("hide")) {
                var row = $(this).parent().parent();
                panal.removeClass("hide");
                var tableRow = '<tr><td></td><td class="insert" colspan="999"></td></tr>';

                $(tableRow).insertAfter(row);

                $(this).parent().parent().next().find(".insert").append(panal);


            } else {
                var panal = $(this).parent().parent().next().find(".toggle-hide");

                panal.addClass("hide");
                var row = $(this);
                panal.insertAfter(row);

                $(this).parent().parent().next().remove();
            }
        });

    </script>


    <%--<script type="text/javascript">

        $(".panal-hider").click(function () {
            var panal = $(this).next();
            if(panal.hasClass("hide")) {
                var row = $(this).parent().parent();
                panal.removeClass("hide");
                panal.addClass("open");
                var tableRow = '<tr><td></td><td class="insert" colspan="999">' + panal.html() + '</td></tr>';
                panal.remove();
                $(tableRow).insertAfter(row);
                
            } else {
                var panal = $(this).parent().parent().next().find(".open");
                    panal.addClass("hide");
                    panal.removeClass("open");
                    var row = $(this);
                    panal.insertAfter(row);

                $(this).parent().parent().next().remove();
            }
        });

    </script>--%>
</asp:Content>


