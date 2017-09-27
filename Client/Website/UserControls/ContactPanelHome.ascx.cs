using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using System.Diagnostics;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.UserControls
{
	public partial class ContactPanelHome : UserControlBase
	{
		//static string searchTxt;
		struct ContactDays
		{
			public DOContactBase Contact { get; set; }
			public int Days { get; set; }
		}

		public DOContact Contact { get; set; }
		public object Customers { get; private set; }
        private ContactCompanySettingsFlag myFlags;
        private DOContactCompany docc;
        protected void Page_Init(object sender, EventArgs e)
        {
            
            btnContact.Text = Contact.DisplayName;
            btnContact.CommandArgument = Contact.ContactID.ToString();
            //jared 2017.4.24 start of block
            docc = ParentPage.CurrentBRContact.SelectAContactCompany(ParentPage.CurrentSessionContext.Owner.ContactID, Contact.ContactID);

            if (docc != null) ApplySettings();

        }
        private void ApplySettings()
        {
            bool cpeFlag = false;   
            if (!IsPostBack)
            {
                cpeFlag = (docc.Settings & (int)ContactCompanySettingsFlag.MainScreenMaximised) > 0;
                //chkGoals.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyGoals) > 0;
                //chkMyContractors.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyContractors) > 0;
                ViewActiveCustomers_btn.Visible = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyCustomers) > 0;
                //chkMyHealth.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyFitness) > 0;
                btnJobsToInvoice.Visible = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyJobsWithLabour) > 0;
                //chkMyOnlineVault.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyOnlineVault) > 0;
                Sites_btn.Visible = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyProperties) > 0;
                btnSupplierInvoices.Visible = (docc.Settings & (int)ContactCompanySettingsFlag.ShowSupplierInvoicesToAssign) > 0;
            }
                cpeMain.ClientState = cpeFlag.ToString();
            //DataBind();
        }
        protected void cpeMain_PreRender(object sender, EventArgs e)
        {
            


            if (cpeMain.ClientState != null)
            {
                bool SavedSetting = false;
                bool UISetting = bool.Parse(cpeMain.ClientState);
                long storedVal = docc.Settings;
                myFlags = (ContactCompanySettingsFlag)storedVal;
                if ((myFlags & ContactCompanySettingsFlag.MainScreenMaximised) == ContactCompanySettingsFlag.MainScreenMaximised)
                   SavedSetting=true;
               
                if(SavedSetting && !UISetting)
                {
                    docc.Settings=docc.Settings - (int)ContactCompanySettingsFlag.MainScreenMaximised;
                    ParentPage.CurrentBRContact.SaveContactCompany(docc);
                }
                else if (!SavedSetting && UISetting)
                {
                    docc.Settings = docc.Settings + (int)ContactCompanySettingsFlag.MainScreenMaximised;
                    ParentPage.CurrentBRContact.SaveContactCompany(docc);
                }

                    //int Settings = (int)ContactCompanySettingsFlag.ShowMyContractors;
                    //((myFlags & CompanyPageFlag.MoveTaskToAnotherJob) == CompanyPageFlag.MoveTaskToAnotherJob)

                
            }
        }

        //eob 2017.4.24

        //jared 2017.4.11 start of block
        //private void LoadCustomers()
        //{
        //	Stopwatch st = new Stopwatch();
        //	st.Start();
        //	if (RpActiveCustomers.DataSource == null || Rp_InactiveCustomers.DataSource == null)
        //	{
        //		//Select all the customers for a contact
        //		List<DOBase> customers = ParentPage.CurrentBRContact.SelectActiveCustomers(Contact.ContactID);
        //		List<DOBase> activeCust = customers;
        //		List<DOBase> inactiveCust = ParentPage.CurrentBRContact.SelectInactiveCustomers(Contact.ContactID);
        //		DOContactInfo user = null;
        //		foreach (var doBase in activeCust)
        //		{
        //			var checkUser = (DOContactInfo)doBase;
        //			if (checkUser.ContactID == Contact.ContactID)
        //			{
        //				user = checkUser;//logged in entity
        //			}
        //		}
        //		if (user == null)
        //		{
        //			activeCust.Insert(0, Contact);
        //		}
        //		else
        //		{
        //			activeCust.Remove(user);
        //			activeCust.Insert(0, user);
        //			inactiveCust.Remove(user);
        //		}
        //		RpActiveCustomers.DataSource = activeCust;
        //		RpActiveCustomers.DataBind();
        //		Rp_InactiveCustomers.DataSource = inactiveCust;
        //		Rp_InactiveCustomers.DataBind();
        //		st.Stop();
        //		long ms = st.ElapsedMilliseconds;
        //	}
        //}
        private void LoadCustomers()
        {
            
            if (RpActiveCustomers.DataSource == null)// || Rp_InactiveCustomers.DataSource == null)
            {
                //Select all the customers for a contact
                List<DOBase> customers = ParentPage.CurrentBRContact.SelectAllNonDeletedCustomers(Contact.ContactID);
                //List<DOBase> allCust = customers;
                //List<DOBase> inactiveCust = ParentPage.CurrentBRContact.SelectInactiveCustomers(Contact.ContactID);
                DOContactInfo user = null;
                foreach (var doBase in customers)
                {
                    var checkUser = (DOContactInfo)doBase;
                    if (checkUser.ContactID == Contact.ContactID)
                    {
                        user = checkUser;//logged in entity
                    }
                    
                }
                if (user == null)
                {
                    customers.Insert(0, Contact);
                }
                else
                {
                    
                    customers.Remove(user);
                    customers.Insert(0, user);
                    //inactiveCust.Remove(user);
                }
                RpActiveCustomers.DataSource = customers;
                RpActiveCustomers.DataBind();
                //Rp_InactiveCustomers.DataSource = inactiveCust;
                //Rp_InactiveCustomers.DataBind();
               
            }
        }
        //2017.4.11 eob jared 

        protected string GetInactiveText()
		{
			return Contact.Active ? string.Empty : "inactive";
		}

		protected string GetExpandIcon()
		{
			return "none".Equals(pnlMain.Style["display"]) ? "fi-plus" : "fi-minus";
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			phNewCustomer.Visible = Contact.Active;
			if (IsPostBack)
			{
				//If active customers are not loaded already.
				if (RpActiveCustomers.DataSource == null && RpActiveCustomers.Visible)
					LoadCustomers();

				//If inactive customers are not loaded already.
				//2017.11.4 jared 
                //if (Rp_InactiveCustomers.DataSource == null && Rp_InactiveCustomers.Visible)
					//LoadCustomers();
                    //eob

				//If sites are visible, load my sites.
				if (rpSites.Visible)
				{
					List<DOBase> CustomerSites = ParentPage.CurrentBRSite.SelectContactSites(Contact.ContactID);
					var ActiveSites = from DOSite site in CustomerSites where site.Active select site;
					if (rpSites.DataSource == null)
						rpSites.DataSource = ActiveSites;
					rpSites.DataBind();
					rpSites.Focus();
				}
			}
		}

		protected void btnContact_Click(object sender, EventArgs e)
		{
			if (Contact != null)
			{
				ParentPage.CurrentSessionContext.CurrentContact = Contact;
				Response.Redirect(Constants.URL_ContactHome);
			}
		}
		protected void btnSelectSite_Click(object sender, EventArgs e)
		{
			Button b = sender as Button;
			if (b != null)
			{
				if (b.CommandName == "SelectSite")
				{
					DOSite Site = ParentPage.CurrentBRSite.SelectSite(new Guid(b.CommandArgument.ToString()));
					ParentPage.CurrentSessionContext.CurrentSite = Site;
                    ParentPage.CurrentSessionContext.CurrentContact = Contact;
                    ParentPage.CurrentSessionContext.CurrentContractee = Contact;
                    Response.Redirect(Constants.URL_SiteHome);
				}
			}
		}
		protected void btnSelectCustomer_Click(object sender, EventArgs e)
		{
            //jared 17/3/9 have removed any condition based on contactType as there was an issue when entering a individual. Also unsure why it should be different
            LinkButton b = sender as LinkButton;
            Guid SelectedContactID = new Guid(b.CommandArgument.ToString());
            DOContact SelectedContact = ParentPage.CurrentBRContact.SelectContact(SelectedContactID); 
            ParentPage.CurrentSessionContext.CurrentContact = Contact;
            ParentPage.CurrentSessionContext.CurrentContractee = SelectedContact;
            ParentPage.CurrentSessionContext.CurrentCustomer = SelectedContact;
            Response.Redirect(Constants.URL_CustomerHome);


            //LinkButton b = sender as LinkButton;
            //if (b != null)
            //{
            //	if (b.CommandName == "Individual")
            //	{
            //		Guid contactID = new Guid(b.CommandArgument.ToString());
            //                 //jared 9.3.17
            //                 //DOContact customer = ParentPage.CurrentBRContact.SelectCustomer(contactID);
            //                 DOContact customer = ParentPage.CurrentBRContact.SelectContact(contactID); 
            //                 //eob 9.3.17
            //                 ParentPage.CurrentSessionContext.CurrentContact = Contact;
            //		//ParentPage.CurrentSessionContext.CurrentCustomer = customer;
            //		ParentPage.CurrentSessionContext.CurrentContractee = customer; //jared 9/3/17 changed from null
            //		Response.Redirect(Constants.URL_CustomerHome);

            //	}

            //	if (b.CommandName == "Company")
            //	{
            //		Guid contactId = new Guid(b.CommandArgument.ToString());
            //		DOContact contractee = ParentPage.CurrentBRContact.SelectContact(contactId);
            //		ParentPage.CurrentSessionContext.CurrentContact = Contact;
            //		ParentPage.CurrentSessionContext.CurrentContractee = contractee;
            //		Response.Redirect(Constants.URL_CustomerHome);
            //	}
            //}
            //eob 2017/3/9
        }

        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
		{
            //TextBox txtNewCustomerEmail = RpActiveCustomers.FindControl("txtNewCustomerEmail") as TextBox;
            if (string.IsNullOrEmpty(txtNewCustomerEmail.Text))
			{
				ParentPage.ShowMessage("Please enter the customer email address.");
			}
			else
			{
				ParentPage.CurrentSessionContext.CurrentContact = Contact;
				Response.Redirect(Constants.URL_CustomerDetails + "?new=" + Server.UrlEncode(txtNewCustomerEmail.Text));
			}
		}

		Dictionary<Guid, int> OutstandingJobsCount = new Dictionary<Guid, int>();
		protected int GetOutstandingJobsCount(object customer)
		{
			Guid CustomerID;
			int count = 0;
			if (customer is DOCustomer)
			{
				CustomerID = ((DOCustomer)customer).CustomerID;
				if (!OutstandingJobsCount.ContainsKey(CustomerID))
				{

					List<DOBase> Sites = ParentPage.CurrentBRSite.SelectCustomerSites(CustomerID, Contact.ContactID);
					foreach (DOSite site in Sites)
					{
						count += GetOutstandingJobsCountSite(site);
					}
					OutstandingJobsCount.Add(CustomerID, count);
				}
			}
			else if (customer is DOContact)
			{
				CustomerID = ((DOContact)customer).ContactID;
				if (!OutstandingJobsCount.ContainsKey(CustomerID))
				{
					List<DOBase> Sites = ParentPage.CurrentBRSite.SelectContracteeSites(CustomerID, Contact.ContactID);
					foreach (DOSite site in Sites)
					{
						count += GetOutstandingJobsCountSite(site);
					}
					OutstandingJobsCount.Add(CustomerID, count);
				}

			}
			else
			{
				throw new Exception();
			}

			return OutstandingJobsCount[CustomerID];
		}

		protected DOTask GetEarliestSiteTask(DOSite Site, DOContact Contact)
		{
			List<DOBase> Tasks = new List<DOBase>();
			foreach (DOJob Job in GetSiteJobs(Site))
			{
				Tasks.AddRange(ParentPage.CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty, false));
			}
			DOTask earliest = (from DOTask t in Tasks
							   where (t.StartDate != DateAndTime.NoValueDate && t.ContractorID == Contact.ContactID)
							   orderby t.StartDate
							   select t).FirstOrDefault<DOTask>();
			return earliest;
		}

		Dictionary<Guid, List<DOBase>> SiteJobs = new Dictionary<Guid, List<DOBase>>();
		protected List<DOBase> GetSiteJobs(DOSite Site)
		{
			if (!SiteJobs.ContainsKey(Site.SiteID))
			{
				List<DOBase> Jobs = ParentPage.CurrentBRJob.SelectViewableJobs(Site, Contact.ContactID);
				SiteJobs.Add(Site.SiteID, Jobs);
			}
			return SiteJobs[Site.SiteID];
		}

		Dictionary<Guid, int> OutstandingJobsCountSite = new Dictionary<Guid, int>();
		protected int GetOutstandingJobsCountSite(DOSite Site)
		{
			if (!OutstandingJobsCountSite.ContainsKey(Site.SiteID))
			{
				List<DOBase> Jobs = GetSiteJobs(Site);
				var OutstandingJobs = from DOJob Job in Jobs
									  where Job.JobStatus == DOJob.JobStatusEnum.Incomplete
									  select Job;
				OutstandingJobsCountSite.Add(Site.SiteID, OutstandingJobs.Count());
			}

			return OutstandingJobsCountSite[Site.SiteID];
		}

		protected string GetSiteClass(object data, int flag)
		{
			int oj = GetOutstandingJobsCount(data);
			if (oj > 0)
			{
				if (flag == 1)
					return "active";
				return "remove";
			}
			if (flag == 1)
				return "remove";
			return "inactive";
		}


		//This is a copy of the code in the ascx file - should merge these at some stage
		protected int GetNextTaskDays(object DataItem)
		{
			Guid CustomerContactID;
			DOTask IncompleteTask = null;
			if (DataItem is DOContact)
			{
				CustomerContactID = ((DOContact)DataItem).ContactID;
				IncompleteTask = ParentPage.CurrentBRJob.SelectContracteeTaskNextIncomplete(CustomerContactID, Contact.ContactID);
			}
			else if (DataItem is DOCustomer)
			{
				CustomerContactID = ((DOCustomer)DataItem).ContactID;
				IncompleteTask = ParentPage.CurrentBRJob.SelectCustomerTaskNextIncomplete(CustomerContactID, Contact.ContactID);
			}
			else
			{
				throw new Exception();
			}

			if (IncompleteTask != null && IncompleteTask.StartDate == DateAndTime.NoValueDate)
			{
				return int.MaxValue - 1;
			}
			else if (IncompleteTask == null)
			{
				return int.MaxValue;
			}
			else
			{
				DOTask task = IncompleteTask;
				DateTime current = DateAndTime.GetCurrentDateTime();
				DateTime currentDate = new DateTime(current.Year, current.Month, current.Day);
				int Days = (task.StartDate - currentDate).Days;
				return Days;
			}
		}

        protected void btnAddSite_Click(object sender, EventArgs e)
        {
            //context stuff here??
            ParentPage.CurrentSessionContext.CurrentContact = this.Contact;
            ParentPage.CurrentSessionContext.CurrentContractee = this.Contact;
            Response.Redirect(Constants.URL_SiteDetails);
        }

        protected void JobFinder_Click(object sender, EventArgs e)
		{
			var jobNumberAuto1 = Request["Searchtext"].TrimStart(',');
			var jobNumberAuto = jobNumberAuto1.TrimEnd(',');
			int result;
			ParentPage.CurrentSessionContext.CurrentContact = this.Contact;
			if (int.TryParse(jobNumberAuto, out result))
			{
                //6.6.17 jared
                DOJobContractor dojc = ParentPage.CurrentBRJob.FindJob(int.Parse(jobNumberAuto), ParentPage.CurrentSessionContext.CurrentContact.ContactID);
                DOJob Job = ParentPage.CurrentBRJob.SelectJob(dojc.JobID);
                //DOJob Job = ParentPage.CurrentBRJob.FindJob(int.Parse(jobNumberAuto));
                //eob
                if (Job != null)
				{
					DOSite site = ParentPage.CurrentBRSite.SelectSite(Job.SiteID);
					if (site != null)
					{
						if (Job != null && site.Active == true)
						{
							ParentPage.CurrentSessionContext.CurrentJob = Job;
							Response.Redirect(Constants.URL_JobSummary + "?jobid=" + Job.JobID);
							HttpContext.Current.ApplicationInstance.CompleteRequest();
						}
					}
					else
					{
						Error.Visible = true;
					}
				}
			}
			else
			{
				Error.Visible = true;
			}
		}

		/// <summary>
		/// View your active customers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ViewActiveCustomers_btn_Click(object sender, EventArgs e)
		{
			if (RpActiveCustomers.Visible == false)
			{
                addnew.Visible = true;
                RpActiveCustomers.Visible = true;
				if (RpActiveCustomers.DataSource == null)
					LoadCustomers();
			}
			else
			{
				RpActiveCustomers.Visible = false;
                addnew.Visible = false;
            }
		}
        //2017.4.11 jared start of block
		///// <summary>
		///// View your inactive customers
		///// </summary>
		///// <param name="sender"></param>
		///// <param name="e"></param>
		//protected void ViewInactiveCustomers_Click(object sender, EventArgs e)
		//{
		//	if (Rp_InactiveCustomers.Visible == false)
		//	{
		//		Rp_InactiveCustomers.Visible = true;
		//		if (Rp_InactiveCustomers.DataSource == null)
		//			LoadCustomers();
		//	}
		//	else
		//		Rp_InactiveCustomers.Visible = false;
		//}
        //eob 2017.4.11

		/// <summary>
		/// View the site
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Sites_btn_Click(object sender, EventArgs e)
		{
            List<DOBase> CustomerSites;
            if (rpSites.Visible == false)
            {
                
                lnkAddSite.Visible = true;
                rpSites.Visible = true;
                DOContractorCustomer docc = ParentPage.CurrentBRContact.SelectContractorCustomer(Contact.ContactID, Contact.ContactID);
                if (docc != null)
                {
                    CustomerSites = ParentPage.CurrentBRSite.SelectContactSites(docc.ContactCustomerId);
                    var ActiveSites = from DOSite site in CustomerSites where site.Active select site;
                    rpSites.DataSource = ActiveSites;
                    rpSites.Focus();
                }
            }
            else
            {
                rpSites.Visible = false;
                lnkAddSite.Visible = false;
            }
        }
		protected void btnJobsToInvoice_Click(object sender, EventArgs e)
        {
            ParentPage.CurrentSessionContext.CurrentContact = Contact;
            Response.Redirect(Constants.URL_JobsToInvoice);
        }

        protected void btnSupplierInvoices_Click(object sender, EventArgs e)
        {
            ParentPage.CurrentSessionContext.CurrentContact = Contact;
            Response.Redirect(Constants.URL_MaterialFromInvoice);
        }

    }
}