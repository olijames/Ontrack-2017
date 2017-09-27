using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using System.Diagnostics;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.Private
{
	struct SiteCount
	{
		public DOSite site;
		public int count;
		public SiteCount(DOSite site, int count) { this.site = site; this.count = count; }
	}

	[PrivatePage]
	public partial class CustomerHome : PageBase
	{
       static DOContact newCompany = new DOContact();
       protected DOEmployeeInfo Employee;


        public void Page_Init(object sender, EventArgs e)
		{
            //Tony added 16.Feb.2017 begin
            CurrentSessionContext.CurrentEmployee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            //Tony added 16.Feb.2017 end

            if (CurrentSessionContext.CurrentContact == null)
				Response.Redirect(Constants.URL_Home);
			if (CurrentSessionContext.CurrentCustomer == null && CurrentSessionContext.CurrentContractee == null)
				Response.Redirect(Constants.URL_Home);
			ClearSite();
			try
			{
				CurrentSessionContext.LastContactPageType = SessionContext.LastContactPageTypeEnum.Customer;
			}
			catch (Exception eex)
			{
				eex.StackTrace.ToString();
			}
		}

		protected DOContact GetInvitationContact()
		{
			DOContact CustomerContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentCustomer.ContactID);
			//If the customer is a company, check the company manager instead.
			if (CustomerContact.ContactType == DOContact.ContactTypeEnum.Company)
				CustomerContact = CurrentBRContact.SelectContact(CustomerContact.ManagerID);
			return CustomerContact;
		}
        protected void Page_PreRender(object sender, EventArgs e)
        {

            //taskIncompletePanel = (Panel)rpJobs.FindControl("phTasksIncomplete");
            //rpJobs.DataSource = CurrentBRJob.SelectJobs(CurrentSessionContext.CurrentSite.SiteID);
            //if (IsPostBack && ShowIncomplete!=true)
            //{

            //   // JobIDtoIncompletePage.Text = Job.JobID.ToString();

            //}
            //else
            //{

            //Tony added start
            //Tony modified 16.Feb.2017
            CurrentSessionContext.CurrentEmployee =
            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            if (Employee != null)
            {
                PermissionVisible();
            }
            //make sure this contractee has their own contractorcustomer entry
            DOContractorCustomer docc = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContractee.ContactID, CurrentSessionContext.CurrentContractee.ContactID);
            if(docc==null)
            {
                docc = CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContractee.ContactID, CurrentSessionContext.CurrentContractee.ContactID
                    , CurrentSessionContext.CurrentContractee.Address1, CurrentSessionContext.CurrentContractee.Address2, CurrentSessionContext.CurrentContractee.Address3
                    , CurrentSessionContext.CurrentContractee.Address4, CurrentSessionContext.CurrentContractee.CompanyName, DOContractorCustomer.LinkedEnum.NotLinked
                    , CurrentSessionContext.CurrentContractee.Phone, Guid.Empty, CurrentSessionContext.CurrentContractee.FirstName, CurrentSessionContext.CurrentContractee.LastName
                    , (int)CurrentSessionContext.CurrentContractee.ContactType);
                CurrentBRContact.SaveContractorCustomer(docc);
            }
            



        }
        protected void PermissionVisible()
        {
            long storedVal = Employee.AccessFlags;
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.DeleteMaterialsFromVehicle) == CompanyPageFlag.DeleteMaterialsFromVehicle)
            {
                lnkRemove.Visible = true; // Display the control of delete site
            }
            

            
        }
        protected void Page_Load()
		{
			//jared 2017/3/9 believe this is not useful for anything. Everything is set from one of two places: 1. the home page 2. clicking back after entering a job number. I dont believe currentcontractee is useful anymore
            //if (CurrentSessionContext.CurrentCustomer != null && (CurrentSessionContext.CurrentContact != null && CurrentSessionContext.CurrentContact.ContactID == CurrentSessionContext.CurrentCustomer.ContactID))
			//{
			//	CurrentSessionContext.CurrentContractee = CurrentSessionContext.CurrentCustomer;
			//}
			//if (CurrentSessionContext.CurrentContact != null && (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentCustomer == null && CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentContractee.ContactID))
			//	CurrentSessionContext.CurrentCustomer = CurrentSessionContext.CurrentContractee;
			//if (CurrentSessionContext.CurrentContact != null && (CurrentSessionContext.CurrentContractee == null && CurrentSessionContext.CurrentCustomer != null &&
			//                                                     CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentCustomer.ContactID))
			//{
			//	CurrentSessionContext.CurrentContractee = CurrentSessionContext.CurrentCustomer;
			//}




			if (!IsPostBack)
			{
				SetCurrentCustomerDetails(CurrentSessionContext.CurrentCustomer);
				lnkInviteCustomer.Visible = false;
				if (CurrentSessionContext.CurrentCustomer != null && CurrentSessionContext.CurrentCustomer.ContactType == DOContact.ContactTypeEnum.Individual)
					btn_Add_New_Company.Visible = true;
				if (CurrentSessionContext.CurrentCustomer != null)
				{
					DOContact invitationContact = GetInvitationContact();
					if (invitationContact.PendingUser)
					{
						List<DOBase> invitations = CurrentBRContact.SelectCustomerInvitations(invitationContact.ContactID);
						if (invitations.Count == 0)
							lnkInviteCustomer.Visible = true;
					}
				}
				else
				{
					lnkInviteCustomer.Visible = false;
					btnRemoveCustomer.Visible = false;
				}
			}
			if (!IsPostBack)
			{
				List<DOBase> sites=new List<DOBase>();
				if (CurrentSessionContext.CurrentContact != null && (CurrentSessionContext.CurrentCustomer != null && CurrentSessionContext.CurrentContact.ContactID != CurrentSessionContext.CurrentCustomer.ContactID))
				{
					DOContact contact;
					if (CurrentSessionContext.CurrentContractee != null)
						contact = CurrentSessionContext.CurrentContractee;
					else
						contact = CurrentSessionContext.CurrentCustomer;
					DOContractorCustomer contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
					sites = SelectVisibleSites(CurrentSessionContext.CurrentCustomer.ContactID, CurrentSessionContext.CurrentContact.ContactID);
					if (contact.DisplayName != "")
					{
						if (contractorCustomer != null)
						{
							litCustomerName.Text = contractorCustomer.DisplayName;
							litCustomerName2.Text = contractorCustomer.DisplayName;
						}
						else
						{
							litCustomerName.Text = contact.DisplayName;
							litCustomerName2.Text = contact.DisplayName;
						}
					}
					else
					{
						litCustomerName.Text = contact.FirstName + " " + contact.LastName;
						litCustomerName2.Text = contact.FirstName + " " + contact.LastName;
					}
					btnEditCustomer.Visible = true;
				}
				else
				{
					if (CurrentSessionContext.CurrentCustomer != null)
						if (CurrentSessionContext.CurrentContact != null && CurrentSessionContext.CurrentContact.ContactID == CurrentSessionContext.CurrentCustomer.ContactID)
						{
							CurrentSessionContext.CurrentContractee = CurrentSessionContext.CurrentCustomer;
						}
					if (CurrentSessionContext.CurrentContractee != null)
					{
						if (CurrentSessionContext.CurrentContact != null)
						{
							var sitesOriginal = CurrentBRSite.SelectContracteeSites(CurrentSessionContext.CurrentContractee.ContactID, CurrentSessionContext.CurrentContact.ContactID);
							sites = sitesOriginal;
						}
					}
					if (CurrentSessionContext.CurrentContractee != null)
					{
						litCustomerName.Text = CurrentSessionContext.CurrentContractee.DisplayName;
						litCustomerName2.Text = CurrentSessionContext.CurrentContractee.DisplayName;
					}
					btnEditCustomer.Visible = false;
				}

				//Get outstanding job count for each site.
				List<DOSiteInfo> siteWithCount = new List<DOSiteInfo>();

				foreach (var doBase in sites)
				{
					var site = (DOSiteInfo) doBase;
					site.JobsCount = GetOutstandingJobsCount(site.SiteId);
					siteWithCount.Add(site);
				}
				//Sites with no jobs for company, deleted
				List<DOSiteInfo> scListRefined = new List<DOSiteInfo>();
				if (CurrentSessionContext.CurrentCustomer != null)
				{
					scListRefined = siteWithCount;
				}
				else
				{
					foreach (DOSiteInfo t in siteWithCount)
					{
						if (t.JobsCount != 0)
						{
							scListRefined.Add(t);
						}
					}
				}

				//Sites with no jobs at end of list.
				var sorted2 = from DOSiteInfo sc in scListRefined
							  orderby (sc.JobsCount > 0 ? 1 : 0) descending, sc.Address1
							  select sc;

				List<DOSiteInfo> ss = new List<DOSiteInfo>();
				foreach (DOSiteInfo si in sorted2.ToList<DOSiteInfo>())
				{
					ss.Add(si);
				}
				rpSites.DataSource = ss;
				rpSites.DataBind();
			}
		}

		/// <summary>
		/// Current customer gets the details from the contractor
		/// </summary>
		/// <param name="currentCustomer"></param>
		private void SetCurrentCustomerDetails(DOContact currentCustomer)
		{
			if (CurrentSessionContext.CurrentCustomer != null)
			{
				DOContractorCustomer contractorCustomer =
					CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID,
						CurrentSessionContext.CurrentCustomer.ContactID);
				if (contractorCustomer != null) MapCustomerDetailsFromContractor(contractorCustomer);
			}
		}

		/// <summary>
		/// Set the properties of customer 
		/// </summary>
		/// <param name="contractorCustomer"></param>
		private void MapCustomerDetailsFromContractor(DOContractorCustomer contractorCustomer)
		{
			CurrentSessionContext.CurrentCustomer.FirstName = contractorCustomer.FirstName;
			CurrentSessionContext.CurrentCustomer.LastName = contractorCustomer.LastName;
			CurrentSessionContext.CurrentCustomer.CompanyName = contractorCustomer.CompanyName;
			CurrentSessionContext.CurrentCustomer.Phone = contractorCustomer.Phone;
			CurrentSessionContext.CurrentCustomer.Address1 = contractorCustomer.Address1;
			CurrentSessionContext.CurrentCustomer.Address2 = contractorCustomer.Address2;
			CurrentSessionContext.CurrentCustomer.Address3 = contractorCustomer.Address3;
			CurrentSessionContext.CurrentCustomer.Address4 = contractorCustomer.Address4;
		}

		private List<DOBase> SelectVisibleSites(Guid currentContracteeID, Guid currentCustomerID)
		{
			List<DOBase> ContactSites = CurrentBRSite.SelectSitesforCustomer(currentContracteeID, currentCustomerID);
			List<DOBase> visibleSites = new List<DOBase>();
			visibleSites = ContactSites;
			return visibleSites;
		}

		protected void btnAddSite_Click(object sender, EventArgs e)
		{
			Response.Redirect(Constants.URL_SiteDetails);
		}

		protected void btnSelectSite_Click(object sender, EventArgs e)
		{
			LinkButton b = sender as LinkButton;
			if (b != null)
			{
				if (b.CommandName == "SelectSite")
				{
					DOSite Site = CurrentBRSite.SelectASite(new Guid(b.CommandArgument.ToString()));
					CurrentSessionContext.CurrentSite = Site;
					Response.Redirect(Constants.URL_SiteHome);
				}
			}
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
			Response.Redirect(Constants.URL_Home);
		}

		protected void btnEditCustomer_Click(object sender, EventArgs e)
		{
			Response.Redirect(Constants.URL_CustomerDetails);
		}

		protected void btnRemoveCustomer_Click(object sender, EventArgs e)
		{
			if (CurrentSessionContext.CurrentCustomer != null)
			{
				DOContractorCustomer cc = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
				if (cc != null)
					CurrentBRContact.DeleteContactCustomer(cc);
				Response.Redirect(Constants.URL_Home);

			}
		}

		protected void btnInviteCustomer_Click(object sender, EventArgs e)
		{
			DOContact invitationContact = GetInvitationContact();
			//Update password.
			string tempPassword = CurrentBRGeneral.GenerateTempPassword();
			string hash = Electracraft.Framework.Utility.PasswordHash.CreateHash(tempPassword);
			invitationContact.PasswordHash = hash;
			CurrentBRContact.SaveContact(invitationContact);

			CurrentBRContact.CreateCustomerInvitation(invitationContact.ContactID, CurrentSessionContext.CurrentContact.ContactID);

			//Send email.
			const string Subject = "You have been invited to Ontrack";
			string body = string.Format("{0} has added you as a customer and wants you to join OnTrack.", CurrentSessionContext.CurrentContact.DisplayName);
			body += string.Format("<br />To login, <a href=\"{0}\">click here</a> and enter the following details:<br />", CurrentBRGeneral.SelectWebsiteBasePath() + "/default.aspx");
			body += string.Format("Email address: {0}<br />Password: {1}", invitationContact.Email, tempPassword);
			body += string.Format("<br /><br />Please remember to change your password once you log in.");

			if (Constants.EMAIL__TESTMODE)
			{
				body += "<br/><br/>";
				body += "Sent By: " + CurrentSessionContext.CurrentContact.DisplayName + "<br />";
				body += "Sent To: " + invitationContact.DisplayName + " (" + invitationContact.Email + ")";
			}
			CurrentBRGeneral.SendEmail(Constants.EmailSender, invitationContact.Email, Subject, body);

			ShowMessage("An invitation has been sent to " + CurrentSessionContext.CurrentCustomer.DisplayName);
			btnInviteCustomer.Visible = false;
		}


		protected string GetCustomerName(Guid siteId)
		{
			DOCustomer customer = CurrentBRContact.SelectSiteCustomer(siteId, CurrentSessionContext.CurrentContact.ContactID);
			if (customer == null)
				return string.Empty;
			else
				return customer.DisplayName;
		}
		protected string GetSiteOwnerName(Guid siteId)
		{
			DOSite site = CurrentBRSite.SelectSite(siteId);
			if (site == null)
				return string.Empty;
			else
				return site.OwnerFirstName + " " + site.OwnerLastName;
		}

		readonly Dictionary<Guid, int> _outstandingJobsCount = new Dictionary<Guid, int>();

		protected int GetOutstandingJobsCount(DOSite site)
		{
			if (!_outstandingJobsCount.ContainsKey(site.SiteID))
			{
				List<DOBase> jobs = CurrentBRJob.SelectViewableJobs(site, CurrentSessionContext.CurrentContact.ContactID);

				var outstandingJobs = from DOJob job in jobs
									  where job.JobStatus == DOJob.JobStatusEnum.Incomplete
									  select job;

				_outstandingJobsCount.Add(site.SiteID, outstandingJobs.Count());
			}
			return _outstandingJobsCount[site.SiteID];
		}
		protected int GetOutstandingJobsCount(Guid siteID)
		{
			if (!_outstandingJobsCount.ContainsKey(siteID))
			{
				List<DOBase> jobs = CurrentBRJob.SelectViewableJobs(siteID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);

				var outstandingJobs = from DOJobInfo job in jobs
									  where job.JobStatus == 0
									  select job;
				_outstandingJobsCount.Add(siteID, outstandingJobs.Count());
			}
			return _outstandingJobsCount[siteID];
		}
		protected string GetSiteClass(DOSite site)
		{

			int oj = GetOutstandingJobsCount(site);
			if (oj > 0)
			{
				return "active";
			}
			else
			{
				return "inactive";
			}

		}
		protected string GetSiteClass(DOSiteInfo site)
		{
			DOContactSite contactSite = CurrentBRSite.SelectContactSite(site.SiteId,
				CurrentSessionContext.CurrentContact.ContactID);
			if (contactSite != null)
			{
				if (contactSite.Active == false)
				{
					return "inactive";
				}
				else
				{
					return "active";
				}
			}
			else
			{
				return "";
			}

		}

		/// <summary>
		/// Find all the companies linked to an individual
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_Add_New_Company_OnClick(object sender, EventArgs e)
		{
		    sitesTitleDiv.Visible = false;
            List<DOBase> contacts=CurrentBRContact.SelectContactCompanies(CurrentSessionContext.CurrentCustomer.ContactID);
		    if (contacts.Count > 0)
		    {
                List<DOContact> visibleCompanies=new List<DOContact>();
		        foreach (DOContact contact in contacts)
		        {
		            if (contact.Searchable ==1)
		            {
		                 visibleCompanies.Add(contact);
		            }
		        }
		        existingCompanies.DataSource = visibleCompanies;
                existingCompanies.DataBind();
		    }
		}

	    protected void btnAddCompany_OnClick(object sender, EventArgs e)
	    {
            newCompany = CurrentBRContact.CreateContact(CurrentSessionContext.CurrentCustomer.ContactID, DOContact.ContactTypeEnum.Company);
            try
            {
                rgCompany.SaveForm(newCompany);
                newCompany.UserName = Guid.NewGuid().ToString();
                newCompany.ManagerID = CurrentSessionContext.CurrentCustomer.ContactID;
                newCompany.CompanyKey = CurrentBRContact.GenerateCompanyKey();
                CurrentBRContact.SaveContact(newCompany);
                //for the user that you created the company for
                DOContactCompany contactCompany = CurrentBRContact.CreateContactCompany(CurrentSessionContext.CurrentCustomer.ContactID, newCompany.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
                CurrentBRContact.SaveContactCompany(contactCompany);
                //they need to become employee

                //for the creator
                //DOContactCustomer contactCustomer = new DOContactCustomer(); // = CurrentBRContact. (CurrentSessionContext.CurrentContact.ContactID, newCompany.ContactID);
                //contactCustomer.Active = true;
               // contactCustomer.ContactCustomerID

                CurrentBRContact.SaveContactCompany(contactCompany);
                IsOwner_pnl.Visible = true;

            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, PageBase.MessageType.Error);
                CC_Register.Visible = true;
            }
        }
        /// <summary>
		/// If the customer is owner of the company
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Owner_btn_OnClick(object sender, EventArgs e)
        {
            newCompany.ManagerID = CurrentSessionContext.CurrentCustomer.ContactID;
            CurrentBRContact.SaveContact(newCompany);
            CurrentSessionContext.CurrentCustomer = newCompany;
            var contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, newCompany.ContactID) ??
                                     CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContact.ContactID, newCompany.ContactID,Guid.Empty); //added creator 2017.4.25 needs testing
            contractorCustomer = CurrentBRContact.SetContractorCustomer(contractorCustomer, newCompany, Guid.Empty);//added guid.empty 2017.4.25 needs testing
            CurrentBRContact.SaveContractorCustomer(contractorCustomer);
           CurrentBRContact.InformCustomerByEmail(CurrentSessionContext.CurrentContact);
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// If the customer is employee of the company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Emp_btn_OnClick(object sender, EventArgs e)
        {
            IsOwner_pnl.Visible = false;
            companyName_pnl.Visible = false;
            CurrentSessionContext.CurrentCustomer = newCompany;
            var contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, newCompany.ContactID) ??
                                     CurrentBRContact.CreateContactCustomer(CurrentSessionContext.CurrentContact.ContactID, newCompany.ContactID, Guid.Empty); //added guid.empty 2017.4.25 needs testing
            contractorCustomer = CurrentBRContact.SetContractorCustomer(contractorCustomer, newCompany, Guid.Empty); //added guid.empty 2017.4.25 needs testing
            CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            CurrentBRContact.InformCustomerByEmail(CurrentSessionContext.CurrentContact);
            Response.Redirect(Constants.URL_CustomerHome);
        }

        /// <summary>
        /// Go back to home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_CustomerHome);
        }
    }
}