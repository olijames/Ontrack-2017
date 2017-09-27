<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="CustomerHome.aspx.cs" Inherits="Electracraft.Client.Website.Private.CustomerHome" %>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>
<%@ Register Src="~/UserControls/RegisterCompany.ascx" TagName="RegisterCompany" TagPrefix="controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Customer
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="top-header">
        <div class="row">
            <div class="small-8 columns">
                <h2>
                    <asp:LinkButton ID="lnkBack" runat="server" OnClick="btnBack_Click"><i class="fi-arrow-left"></i>&nbsp;&nbsp;</asp:LinkButton>&nbsp;&nbsp;<span style="font-size: 0.9em;"><asp:Literal ID="litCustomerName" runat="server"></asp:Literal></span></h2>
            </div>
            <asp:PlaceHolder runat="server" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                <div class="small-4 columns text-right">
                    <h2>
                        <asp:Button ID="btn_Add_New_Company" runat="server" Text="Add company" CssClass="button radius tiny" Visible="False" OnClick="btn_Add_New_Company_OnClick" />
                        <asp:LinkButton ID="lnkEdit" runat="server" OnClick="btnEditCustomer_Click" Style="margin-right: 0.5em;"><i class="fi-pencil"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkInviteCustomer" runat="server" OnClick="btnInviteCustomer_Click" Style="margin-right: 0.5em;"><i class="fi-mail"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkRemove" runat="server" OnClick="btnRemoveCustomer_Click" visible="false" ><i class="fi-trash"></i></asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender ConfirmText="Are you sure you want to remove this customer?" TargetControlID="lnkRemove" runat="server"></ajaxToolkit:ConfirmButtonExtender>
                    </h2>
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="row">
            <div class="small-12 columns">
                <asp:Panel ID="pnlContactHomeButtons" runat="server" CssClass="button-panel" Visible="<%# CurrentSessionContext.CurrentContact.Active %>">
                    <div class="hide">
                        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                        <asp:Button ID="btnAddSite" runat="server" Text="Add Site" OnClick="btnAddSite_Click" />
                        <asp:Button ID="btnEditCustomer" runat="server" Text="Edit" OnClick="btnEditCustomer_Click" />
                        <asp:Button ID="btnRemoveCustomer" runat="server" Text="Remove" OnClick="btnRemoveCustomer_Click" />
                        <ajaxToolkit:ConfirmButtonExtender ConfirmText="Are you sure you want to remove this customer?" TargetControlID="btnRemoveCustomer" runat="server"></ajaxToolkit:ConfirmButtonExtender>
                        <asp:Button ID="btnInviteCustomer" runat="server" Text="Invite" OnClick="btnInviteCustomer_Click" Visible="false" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div class="row section">
        <div class="small-12 columns" id="sitesTitleDiv" runat="server">
            <h2>
                <asp:Literal ID="litCustomerName2" runat="server"></asp:Literal>&nbsp;&gt;&nbsp;Sites</h2>
        </div>
    </div>
    <asp:Repeater ID="rpSites" runat="server">
        <HeaderTemplate>
            <div class="row section">
                <div class="small-12 columns">
                    <div class="row" style="border-bottom: inset">
                        <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                            <asp:LinkButton ID="lnkAddSite" runat="server" OnClick="btnAddSite_Click" CssClass="btn-3 text-left  blue" Height="100%" Width="100%">
								 <%# "<i class=\"fi-plus\"></i>&nbsp;&nbsp; Add new site..."  %>
						 
                            </asp:LinkButton>
                        </div>
                    </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row site-address <%# GetSiteClass((DOSiteInfo)Container.DataItem) %>" data-equalizer>
                <div class="small-12 medium-5 columns site-name" data-equalizer-watch>
                    <asp:LinkButton ID="btnSelectSite" runat="server" CommandName="SelectSite"
                        CommandArgument='<%# Eval("SiteID") %>' CssClass='btn-3 text-left'
                        OnClick="btnSelectSite_Click" Width="100%">
						<%# "<i class=\"fi-play\"></i>&nbsp;&nbsp;" + Eval("Address1").ToString() + ", " + Eval("Address2").ToString() %>
                    </asp:LinkButton>
                </div>
                <div class="small-12 medium-7 columns site-details show-for-medium-up" data-equalizer-watch>
                    <span title="Site Owner"><i class="fi-torso"></i>&nbsp;<%# Eval("OwnerFirstName")+" "+Eval("OwnerLastName")%></span>&nbsp;|&nbsp;&nbsp;
					<span title="Site Jobs"><i class="fi-lightbulb"></i>&nbsp;<%# Eval("jobsCount") %><span class="hide-for-small-only">&nbsp; Active Job(s)</span>&nbsp;&nbsp;|&nbsp;&nbsp;<span title="Days Until Next Task"><%# GetNextTaskDays(Container.DataItem) %></span>
                </div>
                <hr class="show-for-small-only" />
            </div>

        </ItemTemplate>
        <FooterTemplate>
            <div class="small-12 columns">
                <asp:PlaceHolder runat="server" Visible='<%# rpSites.Items.Count == 0 %>'>No sites currently listed.
                </asp:PlaceHolder>

            </div>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater runat="server" ID="existingCompanies">
        <HeaderTemplate>
            <div class="small-12 columns">
                <div class="row" style="border-bottom: inset">
                    <div class="columns small-12 medium-5 large-5" style="padding-bottom: 20px">
                        <h2>Existing Companies</h2>
                    </div>
                </div>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <span><%# Eval("CompanyName")%></span>
        </ItemTemplate>
        <FooterTemplate>
            <div class="small-12 columns">
                <div class="row" style="border-bottom: inset">
                    <asp:PlaceHolder runat="server" Visible='<%# existingCompanies.Items.Count == 0 %>'>No Existing companies visible. 
                    </asp:PlaceHolder>
                </div>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <div class="row" id="CC_Register" clientidmode="Static" runat="server">
        <h3 align="center">Add a New Company </h3>
        <controls:RegisterCompany ID="rgCompany" runat="server" />
        <div class="small-12 medium-7 columns" style="float: right">
            <asp:Button ID="btnAddCompany" runat="server" Text="Add & Link Company" OnClick="btnAddCompany_OnClick" CssClass="button radius tiny" />
            <asp:Button runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="button radius tiny" />
        </div>

    </div>
    <asp:Panel runat="server" CssClass="row" ID="IsOwner_pnl" Visible="False">
        How is your customer
        linked to the company?
                 <asp:Button ID="Owner_btn" runat="server" Text="Owner" CssClass="button radius tiny" OnClick="Owner_btn_OnClick" />
        <asp:Button ID="Emp_btn" runat="server" Text="Employee" CssClass="button radius tiny" OnClick="Emp_btn_OnClick" />
        <asp:Button runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="button radius tiny" />
    </asp:Panel>
    <asp:Panel runat="server" ID="companyName_pnl" Visible="False">
        <asp:Label runat="server" Text="Company Name"></asp:Label>
        <asp:TextBox runat="server" ID="companyName_txt"></asp:TextBox>
    </asp:Panel>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#CC_Register").hide();
            $("#<%=btn_Add_New_Company.ClientID%>").click(function () {
                    event.preventDefault();
                    $("#CC_Register").show();
                });

        });
    </script>
</asp:Content>

<script runat="server">
    string GetNextTaskDays(object DataItem)
    {
        string strNextTask;
        string strIncompleteTasks;
        DOSiteInfo Site = DataItem as DOSiteInfo;
        List<DOBase> Tasks = CurrentBRJob.SelectSiteTasksForContractor(Site.SiteId, CurrentSessionContext.CurrentContact.ContactID);
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
        if (incompleteCount != 0)
        {
            strNextTask = "Pending Tasks";
            return strNextTask + " (" + incompleteCount.ToString() + ")";
        }
        else
            return strNextTask;
    }
</script>
