<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactPanelHome.ascx.cs"
Inherits="Electracraft.Client.Website.UserControls.ContactPanelHome" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<script src="//code.jquery.com/jquery-1.10.2.js"></script>
<script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
<asp:UpdatePanel ID="ActiveCustomers_UpdPnl" runat="server">
<ContentTemplate>
	<div class="row collapse <%# GetInactiveText() %>">
		<!-- This is the panel header -->
		<div class="small-12 columns">
			<asp:Panel ID="pnlHeader" runat="server" CssClass="row">
				<div class="small-2 medium-1 columns">
					<asp:LinkButton ID="btnExpandCollapse" runat="server" BackColor="#000" CssClass="btn-2 text-center" Width="100%"><i style="font-size: 28px; color: white;" class="<%# GetExpandIcon() %>"></i></asp:LinkButton>
				</div>
				<div class="small-10 medium-11 columns" style="padding-left: 0; margin-left: 0px; margin-right: 0px; text-align: center; padding-top: 0px; padding-bottom: 5px;">
					<div class="row">
						<div class="smallScreenStyle columns" style="padding: 0 0; margin-left: 0">
							<asp:LinkButton ID="btnContact" Width="100%" runat="server" BackColor="#000" ForeColor="White" Height="48px" CssClass="text-center select" />
						</div>
						<div class="ManageBtn columns" style="margin-left: 0;">
							<asp:LinkButton ID="LinkButton" Width="100%" BackColor="#000" runat="server" OnClick="btnContact_Click" CssClass="text-center select" ForeColor="White" Height="48px">Manage</asp:LinkButton>
						</div>
					</div>
				</div>
			</asp:Panel>
            <%-- Tony added defaultbutton 25.1.17 
			<asp:Panel CssClass="searchbox-scope" ID="pnlMain" runat="server" DefaultButton="JobFinder">
            --%>
            <asp:Panel CssClass="searchbox-scope" ID="pnlMain" runat="server" DefaultButton="JobFinder">
				<asp:PlaceHolder ID="phNewCustomer" runat="server">
					<div class="row" style="padding: 0.2em 0;">
						<%--<div class="small-12 medium-11 medium-offset-1 columns" style="padding-bottom: 1%;">
							<input type="button" onclick="$('.<%= Contact.ContactID.ToString() %>_newcustomer').slideDown();" style="font-size: 1em; align-content: center; width: 100%; padding: 0.5em 0;" value="Add New Customer" />
							<asp:TextBox ID="SearchText" runat="server" Style="display: none; font-size: 1em; float: right; width: 20%;"></asp:TextBox>
						</div>
						<div class="row <%= Contact.ContactID.ToString() %>_newcustomer" style="width: 100%; display: none; padding-top: 0.6em;">
							<div class="medium-offset-1 columns" style="width: 66.5%; font-size: 0.6em;">
								<asp:TextBox ID="txtNewCustomerEmail" runat="server" placeholder="Email Address"></asp:TextBox>
							</div>
							<div>
								<asp:Button ID="btnAddNewCustomer" CssClass="searchButton button radius" runat="server" OnClick="btnAddNewCustomer_Click" Height="43px" Font-Size=".7em" Text="Add" />
							</div>
						</div>--%>
						<div class="row" style="width: 100%;">
							<div class="medium-offset-1 smallScreenStyle columns" style="width: 66.5%; font-size: 0.6em; align-content: center;">
								<input type="text" id="jobSearchBox" name="Searchtext" placeholder="Search..." onkeyup="UpdateVisibleJobs(this)" />
							</div>
							<div>
								<asp:Button ID="JobFinder" CssClass="searchButton  button radius" runat="server" Height="43px" Font-Size="0.7em" Text="Find Job" OnClick="JobFinder_Click" />
							</div>
						</div>
						<div style="align-items: center;" id="ErrorDiv" class="errorText">
							<asp:Label ClientIDMode="Static" ForeColor="Red" ID="Error" runat="server" Text="Record not found" Visible="false"></asp:Label>
						</div>
						<script>
							function UpdateVisibleJobs(context) {
								var searchScope = $(context).parents('.searchbox-scope');
								var searchText = $('#jobSearchBox', searchScope).val().toLowerCase();
								// The filter gets executed for every job
								// It should return true if the element should remain
								// It should return false if the element should be hidden
								function filter(elem) {
									var elementText = $(elem).children('.site-name').text();
									if(elementText.toLowerCase().indexOf(searchText) > -1) {
										return true;
									}
									return false;
								}

								var targets = searchScope.children('.searchfilter-jobs').toArray();
								targets.forEach(function(element) {
									var shouldShow = filter(element);
									$(element).css('display', shouldShow? 'block' : 'none');
								});
								document.getElementById("<%=Error.ClientID%>").style.display = "none";
								 
							}
							function showActiveCustomers() {
								document.getElementById('ac').style.display ='block';
								document.getElementById('btnViewActiveJobs').style.display = 'none';
								document.getElementById('btnHideActiveJobs').style.display = 'block';
							}
						</script>
					</div>
				</asp:PlaceHolder>
				<asp:Button ID="ViewActiveCustomers_btn" Text="My Customers" CssClass="medium button small-12 medium-11 medium-offset-1" BorderWidth="0" runat="server" OnClick="ViewActiveCustomers_btn_Click"></asp:Button>
				<asp:PlaceHolder ID="addnew"        runat="server" Visible="false">
                    <div class="small-12 medium-11 medium-offset-1 columns" style="padding-bottom: 1%;">
					    <input type="button" onclick="$('.<%= Contact.ContactID.ToString() %>_newcustomer').slideDown();" style="font-size: 1em; align-content: center; width: 100%; padding: 0.5em 0;" value="Add New Customer" />
					    <asp:TextBox ID="SearchText" runat="server" Style="display: none; font-size: 1em; float: right; width: 20%;"></asp:TextBox>
				    </div>
				    <div class="row <%= Contact.ContactID.ToString() %>_newcustomer" style="width: 100%; display: none; padding-top: 0.6em;">
		    		    <div class="medium-offset-1 columns" style="width: 66.5%; font-size: 0.6em;">
	    	    		    <asp:TextBox ID="txtNewCustomerEmail" runat="server" placeholder="Email Address"></asp:TextBox>
				        </div>
					    <div>
						    <asp:Button ID="btnAddNewCustomer" CssClass="searchButton button radius" runat="server" OnClick="btnAddNewCustomer_Click" Height="43px" Font-Size=".7em" Text="Add" />
					    </div>
				    </div>
                </asp:PlaceHolder>
                <asp:Repeater ID="RpActiveCustomers" runat="server" Visible="false">
					<ItemTemplate>
						<div class="searchfilter-jobs row site-address active">
							<div class="small-12 medium-6 medium-offset-1 columns site-name" style="padding: 0;">
								<asp:LinkButton Width="100%" ID="btnSelectCustomer" runat="server" height="100%" CommandName='<%# Eval("ContactType")%>' CommandArgument='<%# Eval("ContactID") %>' OnClick="btnSelectCustomer_Click" CssClass='<%# Eval("Active").ToString()=="True"? "btn-3 text-left" : "grey" %>' >
								<i class="fi-play"></i>&nbsp;&nbsp;<%# Eval("DisplayName") %> 
								<%#Eval("ContactID").ToString()==Contact.ContactID.ToString()?" (My active sites)":"" %>
								</asp:LinkButton>
							</div>
							<hr class="show-for-medium-down" />
						</div>
					</ItemTemplate>
				</asp:Repeater>

				<%--<asp:Button ID="ViewInactiveCustomers" Text="My Inactive Customers" CssClass="medium button small-12 medium-11 medium-offset-1 columns" runat="server" OnClick="ViewInactiveCustomers_Click" />
				<asp:Repeater ID="Rp_InactiveCustomers" runat="server" Visible="false">
					<ItemTemplate>
						<div class="searchfilter-jobs row site-address inactive">
							<div class="small-12 medium-6 medium-offset-1 columns site-name" style="padding: 0;">
								<asp:LinkButton Width="100%" ID="btnSelectCustomer" runat="server" CommandName='<%# Eval("ContactType")%>' CommandArgument='<%# Eval("ContactID") %>' OnClick="btnSelectCustomer_Click" CssClass="btn-3 text-left">
							<i class="fi-play"></i>&nbsp;&nbsp;<%# Eval("DisplayName")/* GetDisplayName(Container.DataItem) */%>
								</asp:LinkButton>
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>--%>

              <asp:Button ID="Sites_btn" Text="My Properties" CssClass="medium button small-12 medium-11 medium-push-1" runat="server" OnClick="Sites_btn_Click"></asp:button>
                <asp:PlaceHolder ID="lnkAddSite" runat="server" Visible="false"> 
                    <div class="small-12 medium-11 medium-offset-1" style="padding-bottom: 1%;">
                        <asp:button id="lnk" runat="server" onclick="btnAddSite_Click"          cssclass="button radius expanded" text="add my property..."></asp:button>
                    </div>
                </asp:PlaceHolder>
				<asp:Repeater ID="rpSites" runat="server" Visible="false">

					<ItemTemplate>
						<div class="row">
							<div class="small-12 medium-6 medium-offset-1 columns">
								<asp:Button ID="btnSelectSite" runat="server" CssClass="button radius expanded" CommandName="SelectSite" CommandArgument='<%# Eval("SiteID") %>'
									Text='<%# Eval("Address1") %>' OnClick="btnSelectSite_Click" Width="100%" />
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>
                  <asp:Button ID="btnJobsToInvoice" Text="My Jobs with labour/materials to invoice" CssClass="medium button small-12 medium-11 medium-offset-1" runat="server" OnClick="btnJobsToInvoice_Click" BorderWidth="0"></asp:button>
      		    <asp:Button ID="btnSupplierInvoices" Text="Supplier invoices to assign" CssClass="medium button small-12 medium-11 medium-offset-1" runat="server" OnClick="btnSupplierInvoices_Click" BorderWidth="0">   </asp:button>
				
            </asp:Panel>
			<ajaxToolkit:CollapsiblePanelExtender ID="cpeMain" runat="server" CollapseControlID="btnExpandCollapse"
				ExpandControlID="btnExpandCollapse" TargetControlID="pnlMain" OnPreRender="cpeMain_PreRender" EnableViewState="true">
			</ajaxToolkit:CollapsiblePanelExtender>
		</div>
	</div>
</ContentTemplate>
</asp:UpdatePanel>
<script runat="server">
string GetDisplayName(object DataItem)
{
	DOContactBase cb = DataItem as DOContactBase;
	if (cb.ID == Contact.ContactID)
	{
		return cb.DisplayName + " (My Active Sites)";
	}
	else
	{
		return cb.DisplayName;
	}
}


string GetNextTaskDays(object DataItem)
{
	string strNextTask;
	//string separator = "&nbsp;&nbsp;|&nbsp;&nbsp;";
	string strIncompleteTasks;
	Guid CustomerContactID;
	List<DOBase> Tasks;
	if (DataItem is DOContact)
	{
		CustomerContactID = ((DOContact)DataItem).ContactID;
		Tasks = ParentPage.CurrentBRJob.SelectContracteeTasks(CustomerContactID, Contact.ContactID);
	}
	else if (DataItem is DOCustomer)
	{
		CustomerContactID = ((DOCustomer)DataItem).ContactID;
		Tasks = ParentPage.CurrentBRJob.SelectCustomerTasks(CustomerContactID, Contact.ContactID);
	}
	else
	{
		throw new Exception();
	}

	List<DOTask> IncompleteTasks = (from DOTask t in Tasks where t.Status == DOTask.TaskStatusEnum.Incomplete select t).ToList<DOTask>();
	if (IncompleteTasks.Count == 0 || IncompleteTasks[0].StartDate == Electracraft.Framework.Utility.DateAndTime.NoValueDate)
	{
		strNextTask = "<i class=\"fi-clock\"></i>&nbsp;No pending tasks";
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

	int incompleteCount = IncompleteTasks.Count;
	strIncompleteTasks = string.Format("<span><i class=\"fi-clipboard-notes\"></i>&nbsp;{0} incomplete task{1}</span>", incompleteCount, incompleteCount == 1 ? string.Empty : "s");
	return strNextTask + " (" + incompleteCount.ToString() + ")";
}

</script>
