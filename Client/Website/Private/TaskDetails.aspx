<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="TaskDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.TaskDetails" %>

<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Task Details
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
                    <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                        <asp:LinkButton ID="lnkDeleteTask" runat="server" OnClick="btnDeleteTask_Click"><i class="fi-trash"></i></asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="lnkDeleteTask"
                            ConfirmText="Are you sure you want to delete this task?">
                        </ajaxToolkit:ConfirmButtonExtender>
                    </asp:PlaceHolder>
                </h2>
            </div>
        </div>
        <div class="row">
            <div class="small-12 columns">
                <asp:Panel ID="pnlContactHomeButtons" runat="server" CssClass="button-panel" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                    <div>
                        <div class="small-12 columns">
                            <asp:LinkButton ID="lnkSave" runat="server" OnClick="btnSave_Click" CssClass="mob-btn"><i class="fi-save show-for-small-only"></i><span class="hide-for-small-only">Save</span></asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="cbeCompleteJob" runat="server" TargetControlID="lnkSave"
                                ConfirmText="The current status of this job is completed. Adding this task will change the job status to incomplete. Do you wish to proceed?">
                            </ajaxToolkit:ConfirmButtonExtender>
                            <asp:LinkButton ID="lnkAmendTask" runat="server" OnClick="btnSave_Click" CssClass="mob-btn"><i class="fi-save show-for-small-only"></i><span class="hide-for-small-only">Amend</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkCompleteTask" runat="server" OnClick="btnCompleteTask_Click" CssClass="mob-btn"><i class="fi-check show-for-small-only"></i><span class="hide-for-small-only">Complete</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkUncompleteTask" runat="server" OnClick="btnUncompleteTask_Click" CssClass="mob-btn"><i class="fi-refresh show-for-small-only"></i><span class="hide-for-small-only">Uncomplete</span></asp:LinkButton>
                            <asp:PlaceHolder ID="phSubmitQuote" runat="server">
                                <asp:LinkButton ID="lnkSubmitQuote" runat="server" OnClientClick="$('#pnlTaskQuoteDetails').slideDown()" CssClass="mob-btn"><i class="fi-download show-for-small-only"></i><span class="hide-for-small-only">Submit Quote</span></asp:LinkButton>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                    <div class="hide">
                        <asp:Button ID="btnSave" runat="server" Text="Save Task" OnClick="btnSave_Click" />
                        <ajaxToolkit:ConfirmButtonExtender ID="cbeCompleteJob1" runat="server" TargetControlID="btnSave"
                            ConfirmText="The current status of this job is completed. Saving this task will change the job status to incomplete. Do you wish to proceed?">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <asp:Button ID="btnAmendTask" runat="server" Text="Amend Task" OnClick="btnSave_Click" />
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
            <h4 class="underline"><i class="fi-torso"></i>&nbsp;&nbsp;Task Details</h4>
            <asp:PlaceHolder ID="phTaskQuote" runat="server">
                <span style="color: Red;">This task has been submitted as a quote.</span>
                <asp:Literal ID="litQuoteStatus" runat="server"></asp:Literal>
                <asp:Button ID="btnDeleteQuote" runat="server" Text="Delete Quote" OnClick="btnDeleteQuote_Click" Visible="<%# CurrentSessionContext.CurrentContact.Active %>" />
            </asp:PlaceHolder>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Task Name<asp:Label runat="server" ForeColor="Red"> *</asp:Label>
                </div>

                <div class="small-12 medium-8 columns">
                    <asp:TextBox ID="txtTaskName" Font-Size="Small" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Task Description<asp:Label runat="server" ForeColor="Red"> *</asp:Label>
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:TextBox ID="txtTaskDescription" Font-Size="Small" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                </div>
            </div>
              <div class="row">
                <div class="small-12 medium-4 columns">
                    Start Date / Time
                </div>
                <div class="small-12 medium-4 columns small">
                    <controls:DateControl ID="dateStartDate"  Font-Size="Small" ClientIDMode="Static" onchange="checkAppointmentEnabled()"
                        runat="server" />
                </div>
                <div class="small-12 medium-4 columns">
                    <asp:DropDownList ID="ddlStartTime" runat="server" ClientIDMode="Static" onchange="checkAppointmentEnabled()">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    End Date / Time
                </div>
                <div class="small-12 medium-4 columns">
                    <controls:DateControl ID="dateEndDate"  Font-Size="Small" runat="server" />
                </div>
                <div class="small-12 medium-4 columns">
                    <asp:DropDownList ID="ddlEndTime" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
              
              <div class="row">
                    <div class="small-12 medium-4 columns">
                        Select Trade Category
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="TradeCategoryAll" runat="server" OnPreRender="TradeCategoryAll_PreRender"
                            OnSelectedIndexChanged="TradeCategoryAll_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
             <div class="row">
                <div class="small-12 medium-4 columns">
                    Customer for the Task
                </div>
                <div class="small-12 medium-8 columns">
                     <asp:DropDownList ID="CustomerDDL" runat="server" OnPreRender="CustomerDDL_OnPreRender">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Contractor for the Task
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal"
                        runat="server"
                        ID="Contractor_RBL" CssClass="radio-list" OnSelectedIndexChanged="Contractor_RBL_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Our Staff">Our Company</asp:ListItem>
                        <asp:ListItem Value="Different">Different Contractor</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <asp:Panel ID="DiffContractor" runat="server" Visible="false">
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        <asp:Label runat="server" Font-Size="Medium" >Search for contractor </asp:Label>
                    </div>
                    <div class="small-12 medium-8 columns right">
                    <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="ContractorsChoice_SelectedIndexChanged"
                        runat="server" 
                        ID="ContractorsChoice" CssClass="radio-list">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="ByTradCat">By Trade Category</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                     </div>
                <div class="row" id="ContractorsListDiv" runat="server" visible="false">
                   <div class="small-12 medium-4 columns">
                        Contractor
                        <label style="display: inline-block">
                            <i class="has-tip tooltip-info" data-tooltip aria-haspopup="true" title="If you are a subscribed user, you are able to add anyone as a contractor.">
                                <img src="../image/ico-info.jpg" /></i></label>
                    </div>
                     <div class="small-12 medium-8 columns">
                          <asp:DropDownList ID="ContractorsDDl" runat="server" OnPreRender="ContractorsDDl_PreRender">
                        </asp:DropDownList>
                        </div>
                    </div>
          
               <%--21/02/2017 jared. We will do this function on the main screen 
                   <div class="row">
                                     <div class="small-12 medium-8 columns right">
                        <asp:Label runat="server" Font-Size="Medium" Font-Bold="true">OR </asp:Label>
                    </div>
                      <br />
                    <div class="small-12 medium-8 columns right">
                        <asp:Label runat="server" Font-Size="Medium" Font-Bold="true">Search email </asp:Label>
                    </div>
                    <br />
                    <br />--%>
                    <asp:PlaceHolder runat="server" Visible="false">
                        <%--<asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>"> 21/2/2017 jared--%>
                        <asp:PlaceHolder ID="phFindNew" runat="server">
                            <div class="row">
                                <div class="small-12 medium-4 columns">
                                    Find new contractor
                                </div>
                                <div class="small-12 medium-8 columns">
                                    <asp:TextBox ID="txtFindNewContractor" runat="server">
                                    </asp:TextBox>
                                    <asp:Button ID="btnFindNewContractor" runat="server" CssClass="button radius tiny" Text="Find" OnClick="btnFindNewContractor_Click" />
                                </div>
                            </div>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                    </div>
                    </asp:Panel>

            <asp:Panel ID="DifferentContractor_Pnl" runat="server" Visible="false">
                <div class="row">
                    <div class="small-12 medium-8 columns right">
                        <asp:Label runat="server" Font-Size="Medium" Font-Bold="true">Search OnTrack </asp:Label>
                    </div>
                    <br />
                    <br />
                </div>
                 
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        <asp:Label runat="server">Select Region</asp:Label>
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False" AutoPostBack="true"
                            Font-Size="Large" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <%-- <div class="row">
                    <div class="small-12 medium-4 columns">

                        <asp:Label runat="server">Select District</asp:Label>
                    </div>
                    <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" runat="server"
                        ID="District_RBL" CssClass="radio-list" OnSelectedIndexChanged="District_RBL_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                    </asp:RadioButtonList>
                    <div id="District_div" runat="server" visible="false" class="small-12 medium-3 large-3 large-centered medium-centered columns"
                        style="overflow-y: scroll; height: 250px; margin-top: 0px; margin-bottom: 10px;">
                        <asp:CheckBoxList ID="District_CBL" RepeatLayout="Flow"
                            TextAlign="Right" runat="server">
                        </asp:CheckBoxList>
                    </div>
                </div>--%>
                <div class="row">
                 <div class="small-12 medium-4 columns">

                        <asp:Label runat="server">Select District</asp:Label>
                    </div>
                 <div class="small-12 medium-8 columns">
                <asp:DropDownList runat="server" ID="District_DDL" OnSelectedIndexChanged="District_DDL_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                     </div>
                    </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">

                        <asp:Label runat="server">Select Suburb</asp:Label>
                    </div>
                      <div class="small-12 medium-8 columns">
                    <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" runat="server"
                        ID="SuburbSelection_rdbtn" CssClass="radio-list" OnSelectedIndexChanged="SuburbSelection_rdbtn_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                    </asp:RadioButtonList>
                    <div id="Suburb_cbl_div" runat="server" visible="false" class="small-12 medium-5 large-12 columns"
                        style="overflow-y: scroll; height: 250px; margin-top: 0px; margin-bottom: 10px;">
                        <%-- <asp:ListBox ID="ListBox_Suburb" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
                        <asp:CheckBoxList ID="Suburb_CBList" RepeatLayout="Flow"
                            TextAlign="Right" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Suburb_CBList_SelectedIndexChanged1">
                        </asp:CheckBoxList>
                    </div>
                          </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Select Trade Category
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="TradeCategories_ddl" runat="server" OnSelectedIndexChanged="TradeCategories_ddl_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <%-- <asp:ListBox ID="TradeCategories_List" runat="server" OnSelectedIndexChanged="TradeCategories_List_SelectedIndexChanged" AutoPostBack="true">
                       <asp:ListItem>Select all</asp:ListItem>
                    </asp:ListBox>--%>
                    </div>
                </div>
                 <div class="row">
                    <div class="small-12 medium-4 columns">
                        Select Sub-Trade Category
                    </div>
                    <div class="small-12 medium-8 columns">
                   <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" runat="server"
                        ID="SubtradeCategoryRdBtn" CssClass="radio-list" OnSelectedIndexChanged="SubtradeCategoryRdBtn_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                    </asp:RadioButtonList>
                    <div id="SUbTradeCategory_div" runat="server" visible="false" class="small-12 medium-5 large-12 columns"
                        style="overflow-y: scroll; height: 250px; margin-top: 0px; margin-bottom: 10px;">
                        <asp:CheckBoxList ID="SubtradeCat_cbl" RepeatLayout="Flow"
                            TextAlign="Right" AutoPostBack="true" runat="server" OnSelectedIndexChanged="SubtradeCat_cbl_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Contractor
                        <label style="display: inline-block">
                            <i class="has-tip tooltip-info" data-tooltip aria-haspopup="true" title="If you are a subscribed user, you are able to add anyone as a contractor. Otherwise you can only view subscribed users.">
                                <img src="../image/ico-question.png" /></i></label>

                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:DropDownList ID="ddlContractor" runat="server">
                        </asp:DropDownList>
                    </div>


                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <div class="row">
                   <%--<div class="small-12 medium-8 columns right">
                        <asp:Label runat="server" Font-Size="Medium" Font-Bold="true">OR </asp:Label>
                    </div>
                      <br />
                    <div class="small-12 medium-8 columns right">
                        <asp:Label runat="server" Font-Size="Medium" Font-Bold="true">Search email </asp:Label>
                    </div>
                    <br />
                    <br />
                    <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                        <asp:PlaceHolder ID="phFindNew" runat="server">
                            <div class="row">
                                <div class="small-12 medium-4 columns">
                                    Find new contractor
                                </div>
                                <div class="small-12 medium-8 columns">
                                    <asp:TextBox ID="txtFindNewContractor" runat="server">
                                    </asp:TextBox>
                                    <asp:Button ID="btnFindNewContractor" runat="server" CssClass="button radius" Text="Find" OnClick="btnFindNewContractor_Click" />
                                </div>
                            </div>
                           
                        </asp:PlaceHolder>
                    </asp:PlaceHolder>--%>
                    </div>
            </asp:Panel>
            
          
            <div class="row">
                <div class="small-12 medium-4 columns">
                    Amended Date
                </div>
                <div class="small-12 medium-8 columns">
                    <p>
                        <asp:Literal ID="litAmendedDate" runat="server"></asp:Literal>
                    </p>
                </div>
            </div>
            <div class="row" style="display:none;">
                <div class="small-12 medium-4 columns">
                    Task Type
                </div>
                <div class="small-12 medium-8 columns">
                    <%--            <asp:DropDownList ID="ddlTaskType" runat="server">
                <asp:ListItem Text="Standard" Value="0"></asp:ListItem>
                <asp:ListItem Text="Reference" Value="1"></asp:ListItem>
                <asp:ListItem Text="Acknowledgement" Value="2"></asp:ListItem>
            </asp:DropDownList>
                    --%>
                    <asp:DropDownList ID="ddlTaskType" runat="server">
                    </asp:DropDownList>
                </div>
            </div>


            <div class="row" style="display: none">
                <div class="small-12 medium-4 columns">
                    Invoice To
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:DropDownList ID="ddlInvoiceTo" runat="server">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row" style="display:none;">
                <div class="small-12 medium-4 hide-for-small-only columns">
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:CheckBox ID="chkAppointment" runat="server" ClientIDMode="Static" />
                    Appointment
                </div>
            </div>

            <asp:PlaceHolder runat="server" Visible="false">
                <%--Visible="<%# CurrentSessionContext.CurrentContact.Active %>"--%>
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
                </asp:PlaceHolder>
            </asp:PlaceHolder>
        </div>
    </div>

    <asp:PlaceHolder runat="server" Visible="false">

        <div class="row section">
            <div class="small-12 columns">
                <h4 class="underline"><i class="fi-clipboard-pencil"></i>&nbsp;&nbsp;Materials</h4>
                <asp:Panel ID="pnlMaterials" runat="server">
                    <div class="row">
                        <div class="small-12 columns">
                            <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false" Width="100%">
                                <Columns>
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
                                            <%# GetEmployeeName(((DOTaskMaterial)Container.DataItem).CreatedBy) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Entered On" DataField="CreatedDate" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteMaterial" runat="server" CommandArgument='<%# Eval("TaskMaterialID").ToString() %>'
                                                Text="Delete" OnClick="btnDeleteMaterial_Click" Enabled="<%# ((DOTaskMaterial)Container.DataItem).Active %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
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
                                            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="small-4 hide-for-small-only columns">
                                        </div>
                                        <div class="small-8 columns">
                                            <asp:Button ID="btnAddMaterial" runat="server" Text="Add Material" OnClick="btnAddMaterial_Click" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </asp:PlaceHolder>
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
                            <asp:GridView ID="gvLabour" runat="server" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# ((DOTaskLabour)Container.DataItem).LabourDate.ToString("ddd dd/MM/yyyy") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time">
                                        <ItemTemplate>
                                            <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                                <%# DateAndTime.DisplayShortTimeString(
                                    ((DOTaskLabour)Container.DataItem).EndMinute - 
                                    ((DOTaskLabour)Container.DataItem).StartMinute ) %>
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
                                                <%# GetEmployeeName(((DOTaskLabour)Container.DataItem).ContactID) %>
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
                                            <%# GetEmployeeName(((DOTaskLabour)Container.DataItem).CreatedBy) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Entered On" DataField="CreatedDate" HeaderStyle-CssClass="hide-for-medium-down" ItemStyle-CssClass="hide-for-medium-down" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteLabour" runat="server" CommandArgument='<%# Eval("TaskLabourID").ToString() %>'
                                                Text="Delete" OnClick="btnDeleteLabour_Click" Enabled="<%# ((DOTaskLabour)Container.DataItem).Active %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row section">
                        <div class="small-12">
                            <div style="background: #666; color: #fff; padding: 0.5em; margin-bottom: 1em;"><span class="fi-plus"></span>&nbsp;&nbsp;Add Labour</div>
                            <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
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
                                            Contractor / Employee
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
                                            <asp:TextBox ID="txtLabourDesc" runat="server"></asp:TextBox>
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
                                                <asp:CheckBox ID="chkChargeable" runat="server" Checked="true" /></p>
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
                            </asp:PlaceHolder>
                        </div>
                    </div>

                </asp:Panel>

            </div>

        </div>

    </asp:PlaceHolder>

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
</asp:Content>
