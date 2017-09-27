<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="JobDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.JobDetails" %>

<%@ Register Src="~/UserControls/JobTasks.ascx" TagName="JobTasks" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/FileDisplayer.ascx" TagName="FileDisplayer" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/JobQuoteStatus.ascx" TagName="JobQuoteStatus" TagPrefix="controls" %>
<%--<%@ Register Src="~/UserControls/JobQuotes.ascx" TagName="Quotes" TagPrefix="controls" %>
--%>
<%--<%@ Register Src="~/UserControls/JobTimeSheets.ascx" TagName="TimeSheets" TagPrefix="controls" %>
--%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Job Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">

    <div class="row">
        <div class="small-12 columns">
            <h2>Job Details</h2>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns" style="padding-bottom: 1em;">
            <div class="button-panel">
                <asp:Button ID="btnDone" runat="server" OnClick="btnDone_Click" Text="Back" CssClass="button radius tiny" />
                <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                    <asp:Button ID="btnAddTask" runat="server" OnClick="btnAddTask_Click" Text="Add Task" Visible="false" />
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="button radius tiny" Text="Save" />
                   &nbsp;<asp:Button ID="btnCompleteJob" runat="server" OnClick="btnCompleteJob_Click" CssClass="button radius tiny" Text="Complete Job" Visible="false" />
                    <asp:Button ID="btnUncompleteJob" runat="server" OnClick="btnUncompleteJob_Click" CssClass="button radius tiny"
                        Text="Uncomplete Job" />
                </asp:PlaceHolder>

            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phCompleteJob" runat="server" Visible="false">
        <div class="row">
            <div class="small-12 columns">
                <div class="row warning red">
                    <div class="small-12 columns">
                        This job was completed by
                    <strong>
                        <asp:Literal ID="litCompletedBy" runat="server"></asp:Literal></strong>
                        on
                    <strong>
                        <asp:Literal ID="litCompletedDate" runat="server"></asp:Literal></strong>.
                    <asp:Literal ID="litIncompleteLabel" runat="server"><p style="margin-bottom: 0;">Reason for incomplete tasks: </asp:Literal><asp:Literal
                        ID="litIncompleteReason" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phTasksIncomplete" runat="server" Visible="false">
        <div class="row">
            <div class="small-12 columns">
                <div class="row warning">
                    <div class="small-12 columns">
                        <h3>Incomplete Job</h3>
                        <p>
                            You are attempting to complete a job that still has incomplete tasks.<br />
                            If you still wish to mark this job as complete, you must enter the reason that these tasks have been left incomplete.
                        </p>
                        <asp:TextBox ID="txtIncompleteReason" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        <asp:Button ID="btnIncompleteSubmit" runat="server" OnClick="btnIncompleteSubmit_Click" CssClass="button radius tiny"
                            Text="Complete Job" /> &nbsp;
                        <asp:Button ID="btnIncompleteCancel" runat="server" Text="Cancel" OnClick="btnIncompleteCancel_Click" CssClass="button radius tiny"/>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="small-12 columns">
            <!--quote status here-->
            <controls:JobQuoteStatus ID="QuoteStatus" runat="server" Visible="false"></controls:JobQuoteStatus>

            <div class="row">
                <div class="small-6 columns">
                    Job Name<asp:Label runat="server" ForeColor="Red"> *</asp:Label><br />
                    <asp:TextBox ID="txtJobName" runat="server" Font-Size="Smaller"></asp:TextBox>
                </div>
               <%-- <div class="small-6 medium-6 columns">
                    Job Type<br />
                    <asp:DropDownList ID="ddlJobType" runat="server">
                        <asp:ListItem Text="Charge Up" Value="2"></asp:ListItem>
                        <asp:ListItem Text="To Quote" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Quoted" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </div>--%>
            </div>
            <div class="row">
                <div class="small-12 columns">
                    Job Description<asp:Label runat="server" ForeColor="Red"> *</asp:Label><br />
                    <asp:TextBox ID="txtJobDescription" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                </div>
             

                <div class="small-12 columns">
                    Project Manager<br />
                    <asp:DropDownList ID="ddlProjectManager" runat="server" ClientIDMode="Static" onchange="checkPM($(this).val())">
                    </asp:DropDownList>
                    <div class="row" id="pmOther" style="margin: -0.7em 0 0 0; display: none; font-size: 0.8em; background: #f8f8f8; padding: 8px 15px;">
                        <div class="small-12 medium-6 columns">
                            Name:<br />
                            <asp:TextBox ID="txtProjectManagerText" runat="server" Style="margin-bottom: 0;"></asp:TextBox>
                        </div>
                        <div class="small-12 medium-6 columns">
                            Phone:<br />
                            <asp:TextBox ID="txtProjectManagerPhone" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>



                   <div class="small-12 columns">
                    Start Date:
                    
                        
                       <controls:DateControl ID="dcStartDate"  runat="server" />
                        </div>
                        <div class="small-12 columns">
                    End Date:
                       <controls:DateControl ID="dcEndDate"  runat="server" />
               </div>
                    <%--<asp:DropDownList ID="DropDownList1" runat="server"  ClientIDMode="Static" OnDataBound="ddJobTemplates_DataBound"></asp:DropDownList>
                    <asp:Button ID="Button1" runat="server" Text="njlsl" onclick="ddJobTemplates_OnChange"/>--%>
                    <%-- Gridview to go here to display tasks in this jobtemplate --%>
                



                    <div class="small-12 columns">
                    Select Job Template
                    <asp:DropDownList ID="ddJobTemplates" runat="server"  ClientIDMode="Static" OnDataBound="ddJobTemplates_DataBound"></asp:DropDownList>
                    <%-- Gridview to go here to display tasks in this jobtemplate --%>
                </div>
            </div>
          <%--  <input id="btnViewMore" type="button" value="VIEW MORE DETAILS" class="button radius" style="width: 100%"
                onclick="showJobDetails()" />
            <input id="btnHideMore" type="button" value="HIDE DETAILS" class="button radius" style="width: 100%; display: none;"
                onclick="hideJobDetails()" />--%>
            <%--<asp:Button id="MoreDetailsBtn" runat="server" class="button radius" OnClick="MoreDetailsBtn_Click" style="width: 100%"/>--%>
            <%--   <hr class="row style-four"/>--%>
            <asp:Panel runat="server" Visible="false">
                <div class="row">

                    <h3>Setup first task:</h3>
                </div>
                <div class="row">
                    <div class="small-12 columns">
                        Task Name<asp:Label runat="server" ForeColor="Red"> *</asp:Label><br />
                        <asp:TextBox ID="FirstTaskName_Txt" Font-Size="Smaller" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="large-3 columns">
                        <asp:Label runat="server">Task Description </asp:Label>
                    </div>
                    <div class="large-9 columns">
                        <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" runat="server"
                            ID="TaskDesc_RadBtn" CssClass="radio-list" OnSelectedIndexChanged="TaskDesc_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Same">Same as job description</asp:ListItem>
                            <asp:ListItem Value="Different">Different description</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                </div>
                <div class="row">
                    <div class="small-12 columns">
                        <asp:TextBox ID="TextBox_TaskDescription" Visible="false" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <%--   <div class="small-12 medium-6 columns">--%>

                    <div class="large-3 columns">
                        Contractor for first task
                        <label style="display: inline-block">
                            <i class="has-tip tooltip-info"
                                data-tooltip aria-haspopup="true"
                                title="If you are a subscribed user, you are able to add anyone as a contractor">
                                <img src="../image/ico-question.png" /></i></label>
                        <br />
                    </div>

                    <div class="large-9 columns">
                        <asp:RadioButtonList runat="server" ID="RBL_Contractor" AutoPostBack="true" CssClass="radio-list"
                            RepeatDirection="Horizontal" OnSelectedIndexChanged="Contractor_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="OurStaff">Our Staff</asp:ListItem>
                            <asp:ListItem Value="Another">Another contractor</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <asp:Panel runat="server" ID="Pnl_TradeCatgory" Visible="false" BorderColor="WhiteSmoke">
                        <div class="large-12 columns">
                            <asp:Label runat="server">Select Region</asp:Label>
                            <asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False" AutoPostBack="true"
                                Font-Size="Large" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="large-12 columns">
                            <asp:Label runat="server">Select Suburb</asp:Label>
                            <%--<asp:DropDownList ID="SuburbDD" runat="server" EnableTheming="False" AutoPostBack="true" Font-Size="Large" 
    OnSelectedIndexChanged="SuburbDD_SelectedIndexChanged">
  
</asp:DropDownList>--%>

                            <%--<asp:Label runat="server">Select Suburb</asp:Label>--%>
                        </div>
                        <%--<div class="small-12 medium-8 columns">
                       <asp:ListBox ID="ListBox_Suburb" runat="server" SelectionMode="Multiple">
                       
                    </asp:ListBox>
                 </div>--%>
                        <div class="small-12 columns"
                            style="overflow-y: scroll; height: 250px; margin-top: 0px; margin-bottom: 10px">
                            <%-- <asp:ListBox ID="ListBox_Suburb" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
                            <asp:CheckBoxList ID="Suburb_CBList" RepeatLayout="Flow"
                                TextAlign="Right" runat="server">
                            </asp:CheckBoxList>
                        </div>

                        <div class="large-12 small-12 columns">
                            <asp:Label runat="server">Select Trade Category</asp:Label>
                            <%--<asp:DropDownList ID="DropDownList_TradeCategory" runat="server" EnableTheming="False" AutoPostBack="true"
     Font-Size="Large">
</asp:DropDownList>--%>
                            <asp:ListBox ID="TradeCategories_List" runat="server" OnSelectedIndexChanged="TradeCategories_List_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                        </div>
                        <div class="row">
                            <%--New functionality required is different --%>
                            <asp:Label runat="server">Select Trade Category</asp:Label>
                            <asp:DropDownList ID="ddlContractor" runat="server">
                            </asp:DropDownList>

                            <asp:PlaceHolder ID="phFindNew" runat="server">
                                <div class="small-12 medium-6 columns">
                                    Find new contractor<br />
                                    <asp:TextBox ID="txtFindNewContractor" runat="server" Font-Size="Smaller">
                                    </asp:TextBox>
                                </div>
                                <div class="small-12 medium-1 columns">
                                    <asp:Button ID="btnFindNewContractor" CssClass="button radius small" runat="server" Text="Find" OnClick="btnFindNewContractor_Click" BorderWidth="0" />
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:PlaceHolder ID="phJobNumberAuto" runat="server">
                <%--Only visible for existing jobs--%>
                <div class="row">
                    <div class="small-12 medium-12 columns">
                        Job ID<br />
                        <asp:TextBox ID="txtJobNumberAuto" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </asp:PlaceHolder>
            <div class="row">
                <div id="JobMoreDetail" class="columns " runat="server" clientidmode="Static" style="position: relative;display:block; min-height: 100vh;">
                    <div class="row">
                        <div class="small-12 medium-6 columns">
                            Job Number<br />
                            <asp:TextBox ID="txtJobNumber" runat="server" Font-Size="Small"></asp:TextBox>
                        </div>
                        <div id="trJobOwner" class="small-12 medium-6 columns">
                            Job Owner<br />
                            <asp:DropDownList ID="ddlJobOwner" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="small-12 columns">
                            Invoice To<br />
                            <asp:DropDownList ID="ddlInvoiceTo" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="phNewJobFields" runat="server">
                        <asp:HiddenField ID="hidFindNewContractorID" runat="server" />
                        <asp:HiddenField ID="hidPendingContractorEmail" runat="server" />
                        <%-- <div class="row">
                    <%--<div class="small-12 medium-6 columns">
                        Contractor
                        <label style="display:inline-block"><i class="has-tip tooltip-info" data-tooltip aria-haspopup="true" title="If you are a subscribed user, you are able to add anyone as a contractor">
                            <img src="../image/ico-question.png" /></i></label>
                        <br />
                        <asp:DropDownList ID="ddlContractor" runat="server">
                        </asp:DropDownList>
                    </div>--%>
                        <%-- <asp:PlaceHolder ID="phFindNew" runat="server">
                        <div class="small-12 medium-6 columns">
                            Find new contractor<br />
                            <asp:TextBox ID="txtFindNewContractor" runat="server">
                            </asp:TextBox>
                            <asp:Button ID="btnFindNewContractor" runat="server" Text="Find" OnClick="btnFindNewContractor_Click" />
                        </div>
                    </asp:PlaceHolder>--%>
                        <%--  </div>--%>
                        <div class="row">
                            <div class="small-12 medium-6 columns">
                                Start Date<br />
                                <controls:DateControl ID="dateStartDate" ClientIDMode="Static" onchange="checkAppointmentEnabled()"
                                    runat="server" />
                            </div>
                            <div class="small-12 medium-6 columns">
                                Start Time<br />
                                <asp:DropDownList ID="ddlStartTime" runat="server" ClientIDMode="Static" onchange="checkAppointmentEnabled()">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-10 medium-4 columns">
                                Appointment
                            </div>
                            <div class="small-2 medium-8 columns">
                                <asp:CheckBox ID="chkAppointment" runat="server" ClientIDMode="Static" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-12 medium-6 columns">
                                End Date
                        <controls:DateControl ID="dateEndDate" runat="server" />
                            </div>
                            <div class="small-12 medium-6 columns">
                                End Time
                        <asp:DropDownList ID="ddlEndTime" runat="server">
                        </asp:DropDownList>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <%--<div class="row">
                <div class="small-12 columns">
                    <input id="btnShowMoreDetails" type="button" value="VIEW MORE DETAILS" style="width:100%" onclick="ToggleDetails(true)" />                    
                    <input id="btnHideMoreDetails" type="button" value="HIDE MORE DETAILS" style="width:100%" onclick="ToggleDetails(false)" />                    
                </div>
            </div>--%>
                    <div class="row">
                        <div id="pnlMoreDetails">
                            <div class="row" style="padding-top: 1em;">
                                <div class="small-12 medium-6 columns">
                                    Access Type<br />
                                    <asp:DropDownList ID="ddlAccessType" runat="server">
                                        <asp:ListItem Text="Phone First" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Lock Box" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Key" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Other" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="small-12 medium-6 columns">
                                    Access Details<br />
                                    <asp:TextBox ID="txtAccessTypeCustom" Font-Size="Small" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="small-12 medium-12 columns">
                                    Alarm Code<br />
                                    <asp:TextBox ID="txtAlarmCode" Font-Size="Small" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="small-12 columns">
                                    Items requiring long term power<br />
                                    <em id="divPoweredError" class="hide" style="font-size: 0.7em; padding-bottom: 0.5em;">Enter items requiring long term power in the text box or select no items.</em>
                                    <asp:TextBox ID="txtPoweredItems" onblur="CheckPowered()" ClientIDMode="Static" runat="server"
                                        TextMode="MultiLine" Rows="4">
                                    </asp:TextBox>
                                    <asp:CheckBox ID="chkNoPoweredItems" onchange="CheckPowered()" Style="display: inline-block"
                                        ClientIDMode="Static" runat="server" /><label for="chkNoPoweredItems" style="display: inline-block">&nbsp;
                            No
                                Powered Items</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="small-12 columns">
                                    Site Notes<br />
                                    <asp:TextBox ID="txtSiteNotes" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="small-12 columns">
                                    Stock / Fittings Required<br />
                                    <asp:TextBox ID="txtStockRequired" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:PlaceHolder runat="server" Visible="false">
                        <div class="row" style="margin-top: 3em;">
                            <div class="small-12 columns">
                                <h2>Contractors</h2>
                                <asp:Panel ID="pnlNoContractors" runat="server">
                                    No contractors selected.
                                </asp:Panel>
                                <asp:Repeater ID="rpJobContractors" runat="server">
                                    <HeaderTemplate>
                                        <table style="width: 100%;">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 80%;"><%# Eval("DisplayName") %></td>
                                            <td style="width: 20%; text-align: right;">
                                                <asp:Button ID="btnRemoveContractor" runat="server" CommandName="RemoveContractor"
                                                    CommandArgument='<%# Eval("ContactID").ToString() %>' OnClick="btnRemoveContractor_Click"
                                                    Text="Remove" Visible="<%# CurrentSessionContext.CurrentContact.Active %>" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></table></FooterTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                                    <div style="background: #f8f8f8; padding: 15px;">
                                        <div class="row">
                                            <div class="small-12 columns">
                                                Add contractor<br />
                                                <asp:DropDownList ID="ddlContractors" runat="server">
                                                </asp:DropDownList><br />
                                                <asp:Button ID="btnAddContractor" runat="server" OnClick="btnAddContractor_Click"
                                                    Text="Add" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phFiles" runat="server">
                        <div class="row" style="margin-top: 3em;">
                            <div class="small-12 columns">
                                <h2>Files</h2>
                                <controls:FileDisplayer ID="FileDisplayer1" runat="server"></controls:FileDisplayer>
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
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
            </asp:Panel>
            <%--        <div class="small-12 medium-5 columns" data-equalizer-watch>
            <div class="row">
                <div class="small-12 columns" style="background: #f8f8f8; padding: 15px; font-size: 0.8em; margin-top: 1.75em;">
                    <h2>Tasks</h2>
                    <controls:JobTasks ID="TaskList" runat="server" />
                </div>
            </div>
        </div>
            --%>
        </div>
    </div>

    <script type="text/javascript">
        function showJobDetails() {
            document.getElementById('btnHideMore').style.display = 'block';
            document.getElementById('btnViewMore').style.display = 'none';
            document.getElementById('JobMoreDetail').style.display = 'block';
       }
        function hideJobDetails() {
            document.getElementById('btnViewMore').style.display = 'block';
            document.getElementById('btnHideMore').style.display = 'none';
            document.getElementById('JobMoreDetail').style.display = 'none';
        }
        //function hide() { document.getElementById('TextBox_TaskDescription').style.display = 'none'; }
        // 
        $(document).ready(function () {
            $('#divPoweredError').hide();
            checkPM($('#ddlProjectManager').val());
            checkAppointmentEnabled();
            ToggleDetails(true);
            CheckPowered();
        });

        function ToggleDetails(show) {
            if (show) {
                $('#btnShowMoreDetails').hide();
                $('#btnHideMoreDetails').show();
                $('#pnlMoreDetails').slideDown();
            }
            else {
                $('#btnShowMoreDetails').show();
                $('#btnHideMoreDetails').hide();
                $('#pnlMoreDetails').slideUp();
            }
        }

        function checkPM(val) {
            if (val == 'ffffffff-ffff-ffff-ffff-ffffffffffff') {
                $('#pmOther').slideDown();
            }
            else {
                $('#pmOther').slideUp();
            }
        }

        function isNullOrWhitespace(input) {
            if (input == null) return true;
            return input.replace(/\s/g, '').length < 1;
        }

        function CheckPowered() {
            var NoPowered = $('#chkNoPoweredItems').is(':checked');
            var PoweredText = $('#txtPoweredItems').val();
            $('#txtPoweredItems').prop('disabled', NoPowered);
            if (NoPowered == true) {
                $('#txtPoweredItems').val('');
            }
            if (!NoPowered && isNullOrWhitespace(PoweredText)) {
                $find('cpeMore').expandPanel();
                $('#divPoweredError').css('display', 'block');
                $('#txtPoweredItems').focus();
                return false;
            }
            else {
                $('#divPoweredError').css('display', 'none');
                return true;
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

    </script>

</asp:Content>
