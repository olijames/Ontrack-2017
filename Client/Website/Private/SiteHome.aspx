<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="SiteHome.aspx.cs" Inherits="Electracraft.Client.Website.Private.SiteHome" %>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    <asp:Literal ID="litSiteAddress" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">

    <div class="top-header">
        <div class="row">
            <div class="small-10 medium-10 large-10 columns">
                <span style="position: relative; top: 5px;">
                    <asp:LinkButton ID="lnkBack" Font-Size="XX-Large" runat="server" OnClick="btnBack_Click"><i class="fi-arrow-left"></i>&nbsp;&nbsp;</asp:LinkButton>
                </span>
                <asp:Literal ID="litSiteDetails" runat="server"></asp:Literal>

            </div>
            <div class="small-2 medium-2 large-2 columns">
                <span style="position: relative; top: 8px; right;">
                    <asp:LinkButton Font-Size="XX-Large" ID="LinkButton2" runat="server" OnClick="btnEditSite_Click" Style="margin-right: 0.5em;"><i class="fi-pencil"></i></asp:LinkButton>


                    <span class="hidden-for-small-down">
                        <asp:LinkButton ID="lnkRemove" Font-Size="XX-Large" runat="server" OnClick="btnDeleteSite_Click" Style="margin-right: 0.5em;"><i class="fi-trash"></i></asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="lnkRemove" ConfirmText="Are you sure you want to delete this site?"></ajaxToolkit:ConfirmButtonExtender>
                    </span>
                </span>
            </div>
            <%--<div class="small-12 medium-12 large-12 columns">
                        <asp:LinkButton  Font-Size="X-Large" ID="LinkButton1" runat="server" OnClick="btnAddJob_Click"><i class="fi-plus"></i></asp:LinkButton>
                             </div>--%>
        </div>
    </div>
     <%-- Tony added 9.11.2016
    <div class="row">
                        Job
                            <asp:DropDownList ID="JobDD" runat="server" EnableTheming="False"
                                AutoPostBack="true" Font-Size="Small" Height="35px" Width="300px">
                            </asp:DropDownList>
                        Site
                            <asp:DropDownList ID="SiteDD" runat="server" EnableTheming="False"
                                AutoPostBack="true" Font-Size="Small" Height="35px" Width="300px">
                            </asp:DropDownList>
                        <asp:Button ID="btnMoveOne" runat="server" Text="Move One" Width="112px" OnClick="btnMoveOne_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnMoveAll" runat="server" Font ForeColor="red" Text="Move All" Width="112px" OnClick="btnMoveAll_Click" />
                    </div>
            <%-- Tony added 9.11.2016 --%>
    <div class="row">
        <div class="small-12 columns">
            <asp:Panel ID="Panel1" runat="server" CssClass="button-panel" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                <div>
                    <%-- <div class="small-12 columns">
                            <asp:LinkButton ID="lnkAddJob" runat="server" OnClick="btnAddJob_Click" 
                                CssClass="mob-btn"><i class="fi-plus show-for-small-only"></i>
                                <span class="hide-for-small-only">Add Job</span></asp:LinkButton>
                        </div>--%>
                </div>
                <div class="hide">
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                    <asp:Button ID="btnAddJob" runat="server" Text="Add New Job" OnClick="btnAddJob_Click" />
                    <asp:Button ID="btnRemoveSite" runat="server" Text="Delete Site" OnClick="btnDeleteSite_Click" />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnRemoveSite"
                        ConfirmText="Are you sure you want to delete this site?">
                    </ajaxToolkit:ConfirmButtonExtender>

                    <asp:Button ID="btnSiteVisibility" runat="server" Text="Site Visibility..." OnClick="btnSiteVisibility_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
    <%--</div>--%>

    <asp:PlaceHolder runat="server" ID="phNoSiteRequirements"></asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phSiteRequirements">
        <asp:Repeater ID="rpSiteRequirements" runat="server">
            <HeaderTemplate>
                <div class="row section" style="background: #FFEAEA; border: 1px solid #C00402; color: #C00402; margin: 1rem auto;">
                    <div class="small-12 columns">
                        <div style="">
                            <h4 style="color: #C00402;"><i class="fi-alert"></i>&nbsp;Site Warnings</h4>
            </HeaderTemplate>
            <%---------------------
                SITE WARNINGS ETC 
                -------------------%>
            <ItemTemplate>
                <%--               <asp:PlaceHolder runat="server" Visible="<%# !((DOJob)Container.DataItem).NoPoweredItems %>">
                    <h5 style="font-size: 15px; color: #C00402;"><%# ((DOJob)Container.DataItem).Name%> - Powered Items</h5>
                    <p style="font-size: 12px"><%# ((DOJob)Container.DataItem).PoweredItems %></p>
                </asp:PlaceHolder>--%>
                <asp:PlaceHolder runat="server" Visible="<%# !((DOJobInfo)Container.DataItem).NoPoweredItems %>">
                    <h5 style="font-size: 15px; color: #C00402;"><%# ((DOJobInfo)Container.DataItem).Name%> - Powered Items</h5>
                    <p style="font-size: 12px"><%# ((DOJobInfo)Container.DataItem).PoweredItems %></p>
                </asp:PlaceHolder>

                <%--                Not sure if these are required? The notes said 'Site Requirements / Power Requirements'--%>
                <%-- <asp:PlaceHolder runat="server" Visible="<%# !string.IsNullOrEmpty(((DOJob)Container.DataItem).SiteNotes) %>">
                    <h5 style="font-size: 15px; color: #C00402;"><%# ((DOJob)Container.DataItem).Name %> - Site Notes</h5>
                    <p style="font-size: 12px"><%# ((DOJob)Container.DataItem).SiteNotes %></p>
                </asp:PlaceHolder>

                <asp:PlaceHolder runat="server" Visible="<%# !string.IsNullOrEmpty(((DOJob)Container.DataItem).StockRequired) %>">
                    <h5 style="font-size: 15px; color: #C00402;"><%# ((DOJob)Container.DataItem).Name %> - Stock Required</h5>
                    <p style="font-size: 12px"><%# ((DOJob)Container.DataItem).StockRequired %></p>
                </asp:PlaceHolder>--%>
                <asp:PlaceHolder runat="server" Visible="<%# !string.IsNullOrEmpty(((DOJobInfo)Container.DataItem).SiteNotes) %>">
                    <h5 style="font-size: 15px; color: #C00402;"><%# ((DOJobInfo)Container.DataItem).Name %> - Site Notes</h5>
                    <p style="font-size: 12px"><%# ((DOJobInfo)Container.DataItem).SiteNotes %></p>
                </asp:PlaceHolder>

                <asp:PlaceHolder runat="server" Visible="<%# !string.IsNullOrEmpty(((DOJobInfo)Container.DataItem).StockRequired) %>">
                    <h5 style="font-size: 15px; color: #C00402;"><%# ((DOJobInfo)Container.DataItem).Name %> - Stock Required</h5>
                    <p style="font-size: 12px"><%# ((DOJobInfo)Container.DataItem).StockRequired %></p>
                </asp:PlaceHolder>
            </ItemTemplate>
            <FooterTemplate>
                </div>
                    </div>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </asp:PlaceHolder>
    <%--<div class="row section">--%>
    <%-- <div class="small-12 columns">
                                <h2>Jobs</h2>
        </div>--%>

    <%-- </div>
            </asp:PlaceHolder>--%>
    <%--</div>--%>



    <%-- For job Completion status --%>
    <asp:PlaceHolder ID="phCompleteJob" runat="server" Visible="false">
        <div class="row">
            <div class="small-12 columns">
                <div class="row warning red">
                    <div class="small-12 columns">
                        The job 
                        (
                        <strong>
                            <asp:Literal ID="JobID" runat="server"></asp:Literal></strong>
                        ) was completed by
                    <strong>
                        <asp:Literal ID="litCompletedBy" runat="server"></asp:Literal></strong>
                        on
                    <strong>
                        <asp:Literal ID="litCompletedDate" runat="server"></asp:Literal></strong>.
                    <asp:Literal ID="litIncompleteLabel" runat="server"><p style="margin-bottom: 0;">Reason for incomplete tasks: </p></asp:Literal><asp:Literal
                        ID="litIncompleteReason" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <%-- For showing incomplete jobs --%>
    <asp:PlaceHolder ID="phTasksIncomplete" runat="server" Visible="false">
        <div class="row">
            <div class="small-12 columns">
                <div class="row warning">
                    <div class="small-12 columns">
                        <h3>Incomplete Job
                            <asp:Literal ID="JobNumber" runat="server"></asp:Literal>
                            ( 
                            <asp:Literal ID="JobIDDisplay" runat="server"></asp:Literal>
                            )</h3>
                        <asp:Literal ID="JobIDtoIncompletePage" runat="server" Visible="false"></asp:Literal>
                        <p>
                            You are attempting to complete a job that still has incomplete tasks.<br />
                            If you still wish to mark this job as complete, you must enter the reason that these tasks have been left incomplete.
                        </p>
                        <asp:TextBox ID="txtIncompleteReason" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        <asp:Button ID="btnIncompleteSubmit" runat="server" OnClick="btnIncompleteSubmit_Click"
                            Text="Complete Job" CssClass="radius tiny button" />
                        <%-- <asp:LinkButton ID="LinkButton1" CommandName="SelectedJob" CommandArgument='<%# Eval("JobIDtoIncompletePage").ToString() %>'  runat="server" OnClick="btnIncompleteSubmit_Click"
                            >Complete Job</asp:LinkButton> --%>&nbsp;
                        <asp:Button ID="btnIncompleteCancel" runat="server" CssClass="radius tiny button" Text="Cancel" OnClick="btnIncompleteCancel_Click" />

                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <%-- --------------
        SITE JOBS TABLE
        --------------%>
    <asp:Repeater ID="rpJobs" runat="server">
        <HeaderTemplate>
            <div class="row section">
                <div class="small-12 columns">
                    <h2>Jobs</h2>
                    <div class="row" style="border-bottom: inset">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="lnkAddJob" runat="server" OnClick="btnAddJob_Click" CssClass="btn-3 text-left  blue" Height="100%" Width="100%">
                                 <%# "<i class=\"fi-plus\"></i>&nbsp;&nbsp; Add new job..."  %>
                         
                            </asp:LinkButton>
                        </div>
                    </div>
                 </div>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <%--          <div class="row site-address <%# GetJobClass((DOJob)Container.DataItem) %>" data-equalizer>--%>
            <div class="row site-address <%# (Eval("JobStatus").ToString())=="Complete"?"inactive":"active"%>" data-equalizer>
                <div class="small-12 columns large-5 medium-5">
                    <asp:LinkButton ID="btnSelectJob" runat="server" CommandName="SelectJob" CommandArgument='<%# Eval("JobID").ToString() %>'
                        CssClass='<%# GetJobColor(Eval("JobStatus").ToString(),Guid.Parse(Eval("JobID").ToString())) %>'
                        OnClick="btnSelectJob_Click" Width="100%">
                        
                        <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp;"+"#"+Eval("JobNumberAuto") +"- " +Eval("Name")%>
                         

                    </asp:LinkButton>
                    <%--                           CssClass='<%# GetClass(Guid.Parse(Eval("JobID").ToString())) %>' --%>
                    <%--                    /*GetJobNameText(Container.DataItem) */--%>
                    <%--                              <asp:LinkButton ID="btnSelectJob" runat="server" Height="100%" CommandName="SelectJob" CommandArgument='<%# Eval("JobID").ToString() %>' CssClass="btn-3 text-left" OnClick="btnSelectJob_Click" Width="100%"><%# "<i class=\"fi-play\"></i>&nbsp;&nbsp;" + GetJobNameText(Container.DataItem)  %></asp:LinkButton>--%>
                </div>
                <div class="small-12 columns site-details show-for-medium-up" style="border-style: none; width: 40%;" data-equalizer-watch>
                
                <%-- Complete Job button- Added by Mandeep --%>
                <%--        <div class="small-12 columns" style="padding-top: 20px; width: 20%; height: 50%;" data-equalizer-watch>         --%>
                <div class="show-for-medium-up" style="float: right">
                    <asp:Button ID="btnCompleteJob" runat="server"
                        CssClass="completeJobButton button radius" OnClick="btnCompleteJob_Click"
                        CommandName="SelectJob"
                        CommandArgument='<%# Eval("JobID").ToString() %>' Text="Complete Job"
                        Visible='<%# (Eval("JobStatus").ToString())=="Complete"? false:true %>' />
                    <%--                        Visible='<%#!GetJobStatus(Container.DataItem) %>' --%>

                    <asp:Button ID="btnUncompleteJob"
                        runat="server" CommandName="SelectJob"
                        CommandArgument='<%# Eval("JobID").ToString() %>' OnClick="btnUncompleteJob_Click" CssClass="uncompleteButton button radius small secondary"
                        Text="Uncomplete Job"
                        Visible='<%# (Eval("JobStatus").ToString())=="Complete"? true:false %>' />
                    <%--                                            Visible='<%#GetJobStatus(Container.DataItem)%>' --%>
                </div>
                <%--            
     </div>--%>

                <hr />
            </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <asp:PlaceHolder runat="server" Visible='<%# rpJobs.Items.Count == 0 %>'>No jobs currently listed.
            </asp:PlaceHolder>
            </div>
            </div>

        </FooterTemplate>
    </asp:Repeater>

    <%-- Inserted below by Jared --%>
    <%--<asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">--%>






    <div class="row section">
        <div class="small-12 columns">
            <h2>Health and Safety>Health and Safety</h2>
            <%--<asp:Literal ID="litToolboxDetails" runat="server"></asp:Literal></h2>--%>
            <div class="row" style="border-bottom: inset">
                <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px; display: none">
                    <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                        <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Report hazard / Near miss"  %>
                        <asp:Literal ID="Literal4" runat="server"></asp:Literal>
                    </asp:LinkButton>
                </div>
                <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                    <div class="row">
                        <div class="small-12 columns large-5 medium-5" style="margin-left: 2%">
                            <asp:LinkButton ID="lbToolBoxTalks" runat="server" Text="ToolBox talks" OnClick="lbToolBoxTalks_Click" CssClass="button radius small"></asp:LinkButton>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:Label ID="lblToolBoxTalks" runat="server" Text="Next Toolbox talk:     8AM Monday 16th March 2016" ForeColor="red" Visible="true" Width="70%"></asp:Label>
                <div class="row">

                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton6" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Hazardous substances Register "  %>
                                <asp:Literal ID="Literal5" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="Label1" runat="server" Width="70%"></asp:Label>
                        </div>
                    </div>

                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton8" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Emergency Response Plan"  %>
                                <asp:Literal ID="Literal7" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="Label5" runat="server" Width="70%"></asp:Label>
                        </div>
                    </div>
                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton9" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Safety inspection checklist"  %>
                                <asp:Literal ID="Literal8" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="Label6" runat="server" Width="70%"></asp:Label>
                        </div>
                    </div>


                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton7" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Training register"  %>
                                <asp:Literal ID="Literal6" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="Label2" runat="server" Width="70%"></asp:Label>
                        </div>
                    </div>


                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Report injury "  %>
                                <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="Label3" runat="server" Text="" ForeColor="red" Visible="true" Width="70%"></asp:Label>
                        </div>
                    </div>
                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Task analysis"  %>
                                <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="lblHazards" runat="server" Text="" ForeColor="red" Width="70%"></asp:Label>
                        </div>
                    </div>
                    <div class="row" style="border-bottom: inset">
                        <%-- <div class="columns small-12 medium-5 large-5" style="padding-bottom:20px">
                    <asp:LinkButton ID="lbToolBoxTalks" runat="server" OnClick="lbToolBoxTalks_Click" CssClass="btn-3 text-left  blue" Width="100%">
                        <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Toolbox talks"  %>
                        
                    </asp:LinkButton>
                </div>--%>
                        <%-- <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="lblToolBoxTalksLabel" runat="server" Text="Next Toolbox talk:     8AM Monday 16th March 2016" Visible="true" Width="70%"></asp:Label>
                        </div>--%>
                    </div>


                    <div class="row" style="border-bottom: inset; display: none">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn-3 text-left  blue" Width="100%">
                                <%# "<i class=\"fi-play\"></i>&nbsp;&nbsp; Site specific safety plan "  %>
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            </asp:LinkButton>
                        </div>
                        <div class="small-12 columns large-7 medium-7">
                            <asp:Label ID="lblSSSP" runat="server" Text="You have read the latest SSSP" Width="70%"></asp:Label>
                        </div>
                    </div>














                    <%--<div class="small-10 medium-6 end medium-left columns">
                 <asp:LinkButton ID="lbHazards" runat="server" Height="100%" CommandName="SelectJob"  text="Hazards" OnClick="btnSelectJob_Click" Width="70%" CssClass="completeJobButton button radius"></asp:LinkButton>
                    <asp:Label ID="lblHazards" runat="server"   text="There are 3 hazards on site currently" ForeColor="red" Width="70%"></asp:Label>
                    </div>
                     </div>
                 <div class="row">
                <div class="small-10 medium-6 end medium-left columns">
                 <asp:LinkButton ID="lbSSSP" runat="server" Height="100%" CommandName="SelectJob"  text="Site specific safety plan" OnClick="btnSelectJob_Click" Width="70%" CssClass="completeJobButton button radius"></asp:LinkButton>
                    <asp:Label ID="lblSSSP" runat="server" text="You have read the latest SSSP"  Width="70%" ></asp:Label>
                    </div>
                     </div>
                 <div class="row">
                <div class="small-10 medium-6 end medium-left columns">
                 <asp:LinkButton ID="lbTA" runat="server" Height="100%" CommandName="SelectJob"  text="Task analysis" OnClick="btnSelectJob_Click" Width="70%" CssClass="completeJobButton button radius"></asp:LinkButton>
          
                    </div>
                     </div>
                 <div class="row">
                <div class="small-10 medium-6 end medium-left columns">
                 <asp:LinkButton ID="lbReportInjury" runat="server" Height="100%" CommandName="SelectJob"  text="Report injury/near miss" OnClick="btnSelectJob_Click" Width="70%" CssClass="completeJobButton button radius"></asp:LinkButton>
                    </div>--%>
                </div>
            </div>

            <%-- </asp:PlaceHolder>--%>
            <%-- Below was copied here and commented out by jared 7/4 --%>
            <%--<asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                <div class="small-4 columns text-right">
                    <h2>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnEditSite_Click" Style="margin-right: 0.5em;"><i class="fi-pencil"></i></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnDeleteSite_Click"><i class="fi-trash"></i></asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="lnkRemove" ConfirmText="Are you sure you want to delete this site?"></ajaxToolkit:ConfirmButtonExtender>
                    </h2>
                </div>
            </asp:PlaceHolder>--%>
        </div>
    </div>

    <%-- Inserted above by jared --%>
    <div class="row section" style="background: #f6f6f6; font-size: 0.9em; padding: 0.5em 0;">
        <div class="small-12 columns">
            <div class="row">
                <div class="small-12 medium-2 columns"><strong>Site Owner:</strong></div>
                <div class="small-12 medium-10 columns">

                    <%# CurrentSiteOwner.FirstName + " " + CurrentSiteOwner.LastName %><br />
                    <%# CurrentSiteOwner.Address1 + ", " + CurrentSiteOwner.Address2 %><br />
                    Ph: <a href="tel:<%# CurrentSiteOwner.Phone %>"><%# CurrentSiteOwner.Phone %></a> | Email: <a href="mailto:<%# CurrentSiteOwner.Email %>"><%# CurrentSiteOwner.Email %></a>
                    <%-- <asp:Button ID="btnEditSiteOwner" OnClick="btnEditeSiteOwner_Click" runat="server" text="Edit site owner..."/>--%>
                </div>
            </div>
        </div>
    </div>
    <%-- Tony added 9.11.2016 --%>
    <%-- Share site function start--%>
    <asp:PlaceHolder runat="server" ID="phShareSite">
        <div class="row section">
            <div class="small-12 columns">
                <h2>Contacts On this site</h2>
                <%--<asp:Literal ID="litToolboxDetails" runat="server"></asp:Literal></h2>--%>
            </div>
            <asp:Repeater ID="rpSiteContact" runat="server">
                <HeaderTemplate>
                    <table style="width: 100%;">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <%--                    <td style="width: 80%;"><%# Eval("DisplayName") %></td>--%>
                        <td>
                            <asp:Label runat="server" ID="lblName" Text='<%# Eval("DisplayName")%>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
            <asp:DropDownList ID="ContactDD" runat="server" EnableTheming="False"
                AutoPostBack="false" Font-Size="Small" Height="35px" Width="575px">
            </asp:DropDownList>&nbsp;<asp:Button ID="btnShare" runat="server" OnClick="btnShare_Click" Text="Share"
                Width="112px" CausesValidation="False" UseSubmitBehavior="False" />
            <br />
            &nbsp;&nbsp;<asp:Label ID="lblShareSite" runat="server" BackColor="#66CCFF" Width="800px"></asp:Label>
        </div>
    </asp:PlaceHolder>
    <%-- Share site function end--%>

    <%-- Move job function start--%>
    <asp:PlaceHolder runat="server" ID="phMoveJob">
        <div class="row section">
            <div class="small-12 columns">
                <h2>Move job to other site</h2>
                <%--<asp:Literal ID="litToolboxDetails" runat="server"></asp:Literal></h2>--%>
            </div>
            <div class="row">
                &nbsp;&nbsp;&nbsp;Job
                                              <asp:DropDownList ID="JobDD" runat="server" EnableTheming="False"
                                                  AutoPostBack="false" Font-Size="Small" Height="35px" Width="300px">
                                              </asp:DropDownList>
                Site
                            <asp:DropDownList ID="SiteDD" runat="server" EnableTheming="False"
                                AutoPostBack="false" Font-Size="Small" Height="35px" Width="300px">
                            </asp:DropDownList>&nbsp;
                <asp:Button ID="btnMoveOne" runat="server" Text="Move One" Width="112px" OnClick="btnMoveOne_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnMoveAll" runat="server" Font ForeColor="red" Text="Move All" Width="112px" OnClick="btnMoveAll_Click" />
                <br />
                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMoveJob" runat="server" BackColor="#66CCFF" Width="800px"></asp:Label>
            </div>
        </div>
    </asp:PlaceHolder>
    <%-- Tony added 9.11.2016 --%>
    <%-- Move job function end--%>
</asp:Content>

<script runat="server">
    //To check the status of job then display Complete/Uncomplete Job button
    bool GetJobStatus(object dataItem)
    {
        DOJob Job = dataItem as DOJob;
        //if (Job.JobStatus==DOJob.JobStatusEnum.Complete)
        //Find the job status according to contractor now
        DOJobContractor jobContractor = CurrentBRJob.SelectJobContractor(Job.JobID,
            CurrentSessionContext.CurrentContact.ContactID);
        if (jobContractor.Status == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    string GetJobNameText(object oJob)
    {
        DOJob Job = oJob as DOJob;
        string brackets;
        if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            brackets = "Completed";

        else
            switch (Job.JobType)
            {
                case DOJob.JobTypeEnum.ToQuote: brackets = "To Quote"; break;
                case DOJob.JobTypeEnum.Quoted: brackets = "Quoted"; break;
                default: brackets = "Charge Up"; break;

            }
        DOJobContractor dojc = CurrentBRJob.SelectJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID);
        return string.Format("#{0}- {1} <span style='display: inline-block; float:right; font-size: 0.7em; opacity: 0.9; padding: 0.4em 0.6em 0 0;'>{2}</span>", dojc.JobNumberAuto, Job.Name, brackets);
    }

    string GetPMDetails(object oJob)
    {
        //DOJob Job = oJob as DOJob;
        DOJobInfo jobInfo = oJob as DOJobInfo;
        if (jobInfo.ProjectManagerID == Constants.Guid_DefaultUser)
        {
            //TODO : store email address for 'other' project manager
            //            return Job.ProjectMangerText + "&nbsp;&nbsp;|&nbsp;&nbsp;<a href='tel:" + Job.ProjectManagerPhone + "'>" + Job.ProjectManagerPhone + "</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href='mailto:#'>PM email goes here</a>";
            return jobInfo.ProjectMangerText + " | <a href='tel:" + jobInfo.ProjectManagerPhone + "'>" + jobInfo.ProjectManagerPhone + "</a>";
        }
        else
        {
            DOContact PM = CurrentBRContact.SelectContact(jobInfo.ProjectManagerID);
            if (PM != null)
            {
                return PM.DisplayName + " | <a href='tel:" + PM.Phone + "'>" + PM.Phone + "</a> | <a href='mailto:" + PM.Email + "'>" + PM.Email + "</a>";
                //return PM.DisplayName + " (<a href='tel:" + PM.Phone + "'>" + PM.Phone + "</a>)";
            }
            return string.Empty;
        }
    }

    string GetNextTaskDays(object DataItem)
    {
        string strNextTask;
        //string separator = "&nbsp;&nbsp;|&nbsp;&nbsp;";
        string strIncompleteTasks;

        //DOJob job = DataItem as DOJob;
        DOJobInfo job = DataItem as DOJobInfo;
        List<DOBase> Tasks = new List<DOBase>();
        if (CurrentSessionContext.CurrentContact != null)
        {
            if (job != null) Tasks = CurrentBRJob.SelectJobTasksForContractor(job.JobID, CurrentSessionContext.CurrentContact.ContactID);
        }
        List<DOTask> IncompleteTasks = (from DOTask t in Tasks where t.Status == DOTask.TaskStatusEnum.Incomplete select t).ToList<DOTask>();
        if (IncompleteTasks.Count == 0 || IncompleteTasks[0].StartDate == Electracraft.Framework.Utility.DateAndTime.NoValueDate)
        {
            strNextTask = "No pending tasks";
        }
        else
        {
            DOTask task = IncompleteTasks[0] as DOTask;
            DateTime current = DateAndTime.GetCurrentDateTime();
            DateTime currentDate = new DateTime(current.Year, current.Month, current.Day);
            int Days = (task.StartDate - currentDate).Days;
            string Warning = string.Empty;
            if (Days <= 1)
            {
                Warning = "style=\"color:red\"";
            }
            strNextTask = string.Format("<span {1}><i class=\"fi-clock\"></i>&nbsp;{0} day{2}</span>", Days, Warning, Days == 1 ? string.Empty : "s");
        }
        int incompleteCount = IncompleteTasks == null ? 0 : IncompleteTasks.Count;
        strIncompleteTasks = string.Format("<span><i class=\"fi-clipboard-notes\"></i>&nbsp;{0} incomplete task{1}</span>", incompleteCount, incompleteCount == 1 ? string.Empty : "s");

        //  return strNextTask + separator + strIncompleteTasks;
        return strNextTask + " (" + incompleteCount.ToString() + ")";
    }


</script>
