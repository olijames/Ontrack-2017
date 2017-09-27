using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using Microsoft.Ajax.Utilities;
using Button = System.Web.UI.WebControls.Button;

namespace Electracraft.Client.Website.Private
{
    struct job
    {
        public DOJob doJob;
        public string status;

    }
    [PrivatePage]
    public partial class SiteHome : PageBase
    {
        protected DOJob Job;
        new DOSite Site;
        protected DOSite CurrentSite;
        //jared 2017.4.6 block
        protected DOContractorCustomer CurrentSiteOwner; //jared
        //protected DOContact CurrentSiteOwner; //jared
        //eob
        private bool ShowIncomplete;
        //Panel taskIncompletePanel;
        private DOJobContractor jobContractor;
        private DOJobContractor dojc;

        //Tony Added 11.11.2016
        private Guid selectedJobID;
        private Guid newSiteID;
        private Guid oldSiteID;
        private Guid siteID;
        private Guid fromContactID;
        private Guid toContactID;
        private string selectedContact = "";
        public const string NO_ITEM_LEFT_MESSAGE = "There is no job left to move";
        public const string SHARE_SITE_MESSAGE = "You shared this site with ";
        protected DOEmployeeInfo Employee;

        protected void Page_Init(object sender, EventArgs e)
        {
            
            if (CurrentSessionContext.CurrentSite == null)
                Response.Redirect(Constants.URL_Home);
            DOSite Site = CurrentSessionContext.CurrentSite;
            CurrentSite = Site;
            //jared 2017.4.6 block
            //CurrentSiteOwner = CurrentBRContact.SelectContact(Site.SiteOwnerID); 
            CurrentSiteOwner = CurrentBRContact.SelectContractorCustomerByCCID(Site.SiteOwnerID); //jared
            //2017.4.6 EOB
            if (CurrentSiteOwner.PendingSiteOwner)
            {
                CurrentSiteOwner.FirstName = "Please add this siteowners details";
                CurrentSiteOwner.LastName = "";
                CurrentSiteOwner.Address1 = "";
                CurrentSiteOwner.Address2 = "";
                CurrentSiteOwner.Email = "N/A";
                CurrentSiteOwner.Phone = "N/A";



            }
            string customerDisplayName = string.Empty;
            //If contact is null, find the contact from the site.
            if (CurrentSessionContext.CurrentContact == null)
            {
                CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSite.ContactID);
                //CurrentSessionContext.CurrentContact = new DOContact();
            }
            //DOContactBase Customer = CurrentBRContact.SelectCustomerByContactID(CurrentSessionContext.CurrentContact.ContactID, Site.ContactID);
            //if (Customer != null)
            //{
            //    customerDisplayName = Customer.DisplayName;
            //}
            if (CurrentSessionContext.CurrentCustomer != null)
            {
                customerDisplayName = CurrentSessionContext.CurrentCustomer.DisplayName;
            }
            else if (CurrentSessionContext.CurrentContractee != null)
            {
                customerDisplayName = CurrentSessionContext.CurrentContractee.DisplayName;

            }

            litSiteAddress.Text = Site.Address1 + ": " + customerDisplayName;
            litSiteDetails.Text = Site.Address1;
            //litToolboxDetails.Text = Site.Address1;

            //litSiteAddress.Text = Site.Address1 + "&nbsp(" + CurrentSessionContext.CurrentCustomer.DisplayName + ")";
            //litSiteDetails.Text = Site.Address1 + "<span style=\"font-size: 0.7em; font-weight: normal;\">&nbsp&nbsp;(" + CurrentSessionContext.CurrentCustomer.DisplayName + ")</span>";
            //litSiteDetails.Text = string.Format("{0} {1}<br />{2}<br />{3}", Site.CustomerFirstName, Site.CustomerLastName, Site.CustomerPhone, Site.CustomerEmail);

            ClearJob();
            CheckSiteVisibilityVisible();

            //btnCompleteJob.Visible = (Job.JobStatus != DOJob.JobStatusEnum.Complete);

            //btnUncompleteJob.Visible = Job.JobStatus == DOJob.JobStatusEnum.Complete;
            //rpJobs.FindControl["phTasksIncomplete"];
            //taskIncompletePanel = (Panel) rpJobs.FindControl("phTasksIncomplete");
            // taskIncompletePanel.Visible = ShowIncomplete;
            //
            if (!IsPostBack)
            {
                LoadContact();
                populateContact();
                populateJobs();
            }

            btnMoveAll.Attributes.Add("onclick", "javascript:return confirm('Do you really want to move all jobs?');");
            
        }

        //Tony added
        private void populateContact()
        {
            List<DOBase> contact = CurrentBRContact.SelectSiteCustomers(CurrentSessionContext.CurrentSite.SiteID);

            if (rpSiteContact != null)
            {
                rpSiteContact.DataSource = contact;
                rpSiteContact.DataBind();
            }

            foreach (RepeaterItem ri in rpSiteContact.Items)
            {
                Label displayName = (Label)ri.FindControl("lblName");
                String ss = displayName.Text;

                if (selectedContact == displayName.Text)
                {
                    displayName.BackColor = System.Drawing.Color.Red;
                    //                   displayName.Attributes.Add("style", "background-color:Green;");
                }
            }
        }

        private void CheckSiteVisibilityVisible()
        {
            bool Ret = false;
            //Must be site owner or in site owners company.
            if (CurrentSessionContext.CurrentContact.ContactID == CurrentSessionContext.CurrentSite.ContactID)
                Ret = true;
            else if (CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentSite.ContactID, CurrentSessionContext.Owner.ContactID))
                Ret = true;

            btnSiteVisibility.Visible = Ret;
        }

        //Tony added 16.Feb.2017 to avoid null exception
        protected void PermissionVisible()
        {
            

                //2017.2.15 Jared create employee for this individual if they have none.
            if (Employee == null)
            {
                DOContact Contact = CurrentSessionContext.Owner;
                DOContactCompany ContactCompany = CurrentBRContact.CreateContactCompany(Contact.ContactID, Contact.ContactID, Contact.ContactID);
                CurrentBRContact.SaveContactCompany(ContactCompany);
                DOEmployeeInfo Employee = CurrentBRContact.CreateEmployeeInfo(ContactCompany.ContactCompanyID, Contact.ContactID);
                Employee.Address1 = Contact.Address1;
                Employee.Address2 = Contact.Address2;
                Employee.Address3 = Contact.Address3;
                Employee.Address4 = Contact.Address4;
                Employee.FirstName = Contact.FirstName;
                Employee.LastName = Contact.LastName;
                Employee.Email = Contact.Email;
                Employee.Phone = Contact.Phone;
                Employee.AccessFlags = 0;
                CurrentBRContact.SaveEmployeeInfo(Employee);
                Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            }
            //jared end of above

            long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag
                                                   // Reinstate the mask and re-test

            //Check if user has a permission to share site with another customer
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.ShareSiteToAnotherCustomer) == CompanyPageFlag.ShareSiteToAnotherCustomer)
            {
                phShareSite.Visible = true; // Display the control of share site function
            }
            else
            {
                phShareSite.Visible = false;// Hide the control of share site function
            }

            //Check if user has a permission to move job to anothter site
            if ((myFlags & CompanyPageFlag.MoveJobToAnotherSite) == CompanyPageFlag.MoveJobToAnotherSite)
            {
                phMoveJob.Visible = true; // Display the control of moving job function
            }
            else
            {
                phMoveJob.Visible = false;// Hide the control of moving job function
            }
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

            //Commmented on 16.Feb.2017
//            long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag
//                                                   // Reinstate the mask and re-test
//
//            //Check if user has a permission to share site with another customer
//            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
//            if ((myFlags & CompanyPageFlag.ShareSiteToAnotherCustomer) == CompanyPageFlag.ShareSiteToAnotherCustomer)
//            {
//                phShareSite.Visible = true; // Display the control of share site function
//            }
//            else
//            {
//                phShareSite.Visible = false;// Hide the control of share site function
//            }
//
//            //Check if user has a permission to move job to anothter site
//            if ((myFlags & CompanyPageFlag.MoveJobToAnotherSite) == CompanyPageFlag.MoveJobToAnotherSite)
//            {
//                phMoveJob.Visible = true; // Display the control of moving job function
//            }
//            else
//            {
//                phMoveJob.Visible = false;// Hide the control of moving job function
//            }



            if (!IsPostBack)
            {
                LoadSites();
            }
            //Tony added end

            if (ShowIncomplete)
            {
                phTasksIncomplete.Visible = true;
            }
            List<DOBase> Jobs = CurrentBRJob.SelectViewableJobs(CurrentSessionContext.CurrentSite.SiteID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);
            DOJobContractor JobContractor;
            foreach (DOJobInfo job in Jobs)
            {
                JobContractor = CurrentBRJob.SelectJobContractor(job.JobID, CurrentSessionContext.CurrentContact.ContactID);
                job.JobNumberAuto = JobContractor.JobNumberAuto.ToString();

            }
            rpJobs.DataSource = Jobs;
            rpJobs.DataBind();

            List<DOBase> contact = CurrentBRContact.SelectSiteCustomers(CurrentSessionContext.CurrentSite.SiteID);
            if (rpSiteContact != null)
            {
                rpSiteContact.DataSource = contact;
                rpSiteContact.DataBind();

            }

            bool ShowRequirements = false;
            //foreach (DOJob job in Jobs)
            foreach (DOJobInfo job in Jobs)
            {
                if (!job.NoPoweredItems || !string.IsNullOrEmpty(job.SiteNotes) || !string.IsNullOrEmpty(job.StockRequired))
                {
                    ShowRequirements = true;
                }

            }

            if (!ShowRequirements)
            {
                phNoSiteRequirements.Visible = true;
                phSiteRequirements.Visible = false;
            }
            else
            {
                phSiteRequirements.Visible = true;
                phNoSiteRequirements.Visible = false;
                rpSiteRequirements.DataSource = Jobs;
                rpSiteRequirements.DataBind();
            }
            DataBind();
            
        }

        protected void btnAddJob_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_JobSummary);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        protected void btnEditeSiteOwner_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_JobSummary);
        }

        private void GoBack()
        {

            if (CurrentSessionContext.CurrentContractee == null &&
                CurrentSessionContext.CurrentCustomer == null &&
                CurrentSessionContext.LastContactPageType != SessionContext.LastContactPageTypeEnum.Customer)
            {
                //Response.Redirect("~/private/mysites.aspx");
            }
            else
            {
                CurrentSessionContext.CurrentSite = CurrentSite;
                Response.Redirect(Constants.URL_CustomerHome);
            }

            Response.Redirect(Constants.URL_CustomerHome);
        }

        protected void btnSelectJob_Click(object sender, EventArgs e)
        {
            LinkButton b = sender as LinkButton;
            if (b != null)
            {
                if (b.CommandName == "SelectJob")
                {
                    Guid JobID = new Guid(b.CommandArgument.ToString());
                    DOJob Job = CurrentBRJob.SelectJob(JobID);
                    CurrentSessionContext.CurrentJob = Job;
                    //todo ollys redirect here
                    Response.Redirect(Constants.URL_JobSummary);
                }
            }
        }

        //Tony Test
        protected void btnTest_Click(object sender, EventArgs e)
        {
            if (phShareSite.Visible == true)
            {
                phShareSite.Visible = false;
            }
            else
            {
                phShareSite.Visible = true;
            }
        }

        protected void btnEditSite_Click(object sender, EventArgs e)
        {
            Response.Redirect("SiteDetails.aspx", false);
        }
        protected void btnDeleteSite_Click(object sender, EventArgs e)
        {
            CurrentBRSite.DeleteSite(CurrentSessionContext.CurrentSite, CurrentSessionContext.CurrentContact.ContactID);
            // CurrentBRSite.DeleteSite(CurrentSessionContext.CurrentSite, CurrentSessionContext.CurrentContact.ContactID);
            DOContractorCustomer docc = CurrentBRContact.SelectContractorCustomerByCCID(CurrentSite.SiteOwnerID);
            DOContact customer = CurrentBRContact.SelectContact(docc.CustomerID);
            CurrentSessionContext.CurrentCustomer = customer;
            List<DOBase> contactSites = CurrentBRSite.SelectSitesforCustomer(CurrentSessionContext.CurrentContact.ContactID, customer.ContactID);
            bool activeSite = false;
            foreach (DOSiteInfo site in contactSites)
            {
                if (site.Active)
                {
                    activeSite = true;
                    break;
                }
            }
            if (activeSite)
            {
                MakeCustomerActive(true);
            }
            else
            {
                MakeCustomerActive(false);
            }


            CurrentSessionContext.CurrentSite = null;
            CurrentSessionContext.CurrentCustomer = null;
            GoBack();

            //CurrentBRSite.DeleteSite(CurrentSessionContext.CurrentSite);
            //CurrentSessionContext.CurrentSite = null;
            //GoBack();
        }
        //[System.Web.Services.WebMethod]
        //public static void testMethod(object sender, EventArgs e)
        //{
        //    SiteHome siteHome = new SiteHome();
        //    siteHome.btnIncompleteSubmit_Click(sender,e);

        //}
        protected void btnSiteVisibility_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_SiteVisibility);
        }
        protected void btnCompleteJob_Click(object sender, EventArgs e)
        {

            txtIncompleteReason.Text = string.Empty;
            phCompleteJob.Visible = false;
            Button b = sender as Button;
            if (b != null)
            {
                //if (b.CommandName == "SelectJob")
                //{
                //    Guid JobID = new Guid(b.CommandArgument.ToString());
                //    Job = CurrentBRJob.SelectJob(JobID);
                //    CurrentSessionContext.CurrentJob = Job;
                //    //  Response.Redirect(Constants.URL_JobSummary);
                //}
                //use jobcontractor table now
                if (b.CommandName == "SelectJob")
                {
                    Guid JobID = new Guid(b.CommandArgument.ToString());
                    Job = CurrentBRJob.SelectJob(JobID);

                    CurrentSessionContext.CurrentJob = Job;
                    //  Response.Redirect(Constants.URL_JobSummary);
                }
            }
            //if (CurrentSessionContext.CurrentJob == null && CurrentSessionContext.CurrentSite == null)
            //    Response.Redirect(Constants.URL_Home);
            //if (CurrentSessionContext.CurrentContact == null)
            //    Response.Redirect(Constants.URL_Home);

            //if (CurrentSessionContext.CurrentJob != null)
            //{
            //    //Reload job from database (in case other users have changed job)
            //    CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentJob.JobID);
            //    //CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
            //}
            Site = CurrentSessionContext.CurrentSite;
            Job = CurrentSessionContext.CurrentJob;

            DOJobContractor jobContractor = CurrentBRJob.SelectJobContractor(Job.JobID,
              CurrentSessionContext.CurrentContact.ContactID);
            //if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            //Check status of job for current job contractor
            if (jobContractor.Status == 1)
            {
                ShowCompletedInfo();
                // MakeSiteInactiveIfJobsComplete();
                phTasksIncomplete.Visible = false;
            }
            else
            {
                //List<DOBase> JobTasks = CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty);
                List<DOBase> JobTasks = CurrentBRJob.SelectTasks(Job.JobID);
                bool Complete = true;
                foreach (DOTask Task in JobTasks)
                {
                    if (Task.Status == DOTask.TaskStatusEnum.Incomplete && (Task.TaskType == DOTask.TaskTypeEnum.Standard || Task.TaskType == DOTask.TaskTypeEnum.Reference))
                    {
                        Complete = false;
                        break;
                    }
                }

                if (Complete)
                {
                    Job.JobStatus = DOJob.JobStatusEnum.Complete;
                    CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
                    Job.CompletedBy = CurrentSessionContext.Owner.ContactID;
                    Job.CompletedDate = DateAndTime.GetCurrentDateTime();
                    jobContractor.Status = 1;
                    // CurrentBRJob.SaveJob(Job);
                    CurrentBRJob.SaveJobContractor(jobContractor);
                    //Check if this job is complete for all contractors
                    bool jobFlag = CheckJobStatusForContractors(Job.JobID);
                    //If yes, complete the job's status on job table
                    if (jobFlag)
                    {
                        Job.JobStatus = DOJob.JobStatusEnum.Complete;
                        CurrentBRJob.SaveJob(Job);
                    }
                    //Find if there are any active jobs for the same contractor in same site
                    bool activeJobsForSite = FindActiveJobsForContractorSite(Job.SiteID,
                        CurrentSessionContext.CurrentContact.ContactID);
                    if (activeJobsForSite)
                    {
                        SetSiteFlag(Job.SiteID, Job.JobID, false);
                    }

                    //  LogChange(DOJobChange.JobChangeType.JobCompleted);
                    ShowIncomplete = false;
                }
                else
                {
                    ShowIncomplete = true;
                    if (ShowIncomplete)
                    {
                        var jobId = Job.JobID;
                        // ClientScript.RegisterClientScriptBlock(this.GetType(), "modalIncompleteJob", "$(function() { ShowIncompleteJob()});", true);
                        //taskIncompletePanel = (Panel)e.Item.FindControl("phTasksIncomplete");
                        //taskIncompletePanel = (Panel)rpJobs.FindControl("phTasksIncomplete");
                        //taskIncompletePanel.Visible = true;
                        phTasksIncomplete.Visible = true;
                        JobIDtoIncompletePage.Text = jobId.ToString();
                        DOJobContractor dojc = CurrentBRJob.SelectJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID);
                        JobIDDisplay.Text = dojc.JobNumberAuto.ToString();
                        //  ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('Hello');", true);
                        //    JobNumber.Text = Job.JobNumberAuto.ToString();
                        //    JobIDtoIncompletePage.Text = Job.JobID.ToString();
                    }
                }
            }
            ////Check if all tasks are complete.

            if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                //ShowCompletedInfo();
                phTasksIncomplete.Visible = false;
            }

            //JobDetails jobdetails = new JobDetails();

        }

        private bool FindActiveJobsForContractorSite(Guid siteID, Guid contactID)
        {
            List<DOBase> activeJobs = CurrentBRJob.SelectActiveJobsForContractorSite(siteID, contactID);
            if (activeJobs.Count == 0)
            {
                return true;
            }
            return false;

        }

        private bool CheckJobStatusForContractors(Guid jobID)
        {
            List<DOBase> jobContractor = CurrentBRJob.SelectActiveJobContractors(jobID);
            if (jobContractor.Count == 0)
            {
                return true;
            }
            return false;
        }

        public void SetSiteFlag(Guid siteID, Guid jobId, bool active)
        {
            //List<DOBase> activeJobs = CurrentBRJob.SelectActiveJobsForSite(siteID);
            //Select job for this contractor
            //DOJobContractor contractorJob = CurrentBRJob.SelectJobContractor(jobId,
            //     CurrentSessionContext.CurrentContact.ContactID);
            //// if (activeJobs.Count == 0)
            // contractorJob.Active = active;
            // CurrentBRJob.SaveJobContractor(contractorJob);
            DOContactSite contactSite = CurrentBRSite.SelectContactSite(siteID,
                CurrentSessionContext.CurrentContact.ContactID);
            if (contactSite != null)
            {
                // CurrentBRSite.SaveContactSite(siteID, active);
                //Make the contactsite active for siteid and current contractor
                contactSite.Active = active;
                CurrentBRSite.SaveContactSite(contactSite);
            }
            else
            {
                CurrentBRSite.SaveContactSite( true, siteID, CurrentSessionContext.CurrentContact.ContactID);
            }
            if (CurrentSessionContext.CurrentCustomer != null)
            {
                List<DOBase> ContactSites = CurrentBRSite.SelectSitesforCustomer(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentCustomer.ContactID);
                bool activeSite = false;
                foreach (DOSiteInfo site in ContactSites)
                {
                    if (site.Active)
                    {
                        activeSite = true;
                        break;
                    }
                }
                if (activeSite)
                {
                    MakeCustomerActive(true);
                }
                else
                {
                    MakeCustomerActive(false);
                }
            }
        }

        private void MakeCustomerActive(bool active)
        {
            DOContact currentCustomer = new DOContact();
            currentCustomer = CurrentSessionContext.CurrentCustomer;
            DOContact currentContractor = CurrentSessionContext.CurrentContact;
            //DOContactSite contactSite = new DOContactSite();
            //List<DOBase> activeSites=  CurrentBRSite.SelectSitesforCustomer(currentContractor.ContactID,currentCustomer.ContactID);

            //Select active sites for current contractor customer pair and update the customer as active or inactive
            // List<DOBase> activeSites = CurrentBRSite.SelectActiveSitesforContractorCustomer(currentContractor.ContactID, currentCustomer.ContactID);
            //var activeCount = 0;
            //foreach (var variableCS in activeSites)
            //{
            //  DOContactSite cs= variableCS as DOContactSite;
            //    if (cs.Active)
            //    {
            //        activeCount++;
            //    }
            //}
            // if (activeSites.Count > 0)
            // {


            if (currentCustomer != null)
                CurrentBRContact.UpdateContactCustomer(currentCustomer.ContactID, active, currentContractor.ContactID);
            //}
            // else
            //    CurrentBRContact.UpdateContactCustomer(currentCustomer.ContactID, false, currentContractor.ContactID);
            // List<DOBase> activeTasks=CurrentBRJob.SelectActiveTasks()
        }

        protected void btnUncompleteJob_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                if (b.CommandName == "SelectJob")
                {
                    Guid JobID = new Guid(b.CommandArgument.ToString());
                    Job = CurrentBRJob.SelectJob(JobID);
                    CurrentSessionContext.CurrentJob = Job;
                    //  Response.Redirect(Constants.URL_JobSummary);
                }
            }

            //            Job.CompletedBy =;
            Job.CompletedBy = Guid.Empty;
            Job.CompletedDate = DateAndTime.NoValueDate;

            Job.JobStatus = DOJob.JobStatusEnum.Incomplete;
            CurrentBRJob.SaveJob(Job);
            DOJobContractor jobContractor = CurrentBRJob.SelectJobContractor(Job.JobID,
             CurrentSessionContext.CurrentContact.ContactID);
            jobContractor.Status = 0;
            jobContractor.Active = true;
            CurrentBRJob.SaveJobContractor(jobContractor);
            SetSiteFlag(Job.SiteID, Job.JobID, true);
            LogChange(DOJobChange.JobChangeType.JobUncompleted);
            phCompleteJob.Visible = false;
        }
        protected void ShowCompletedInfo()
        {
            //CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentJob.JobID);

            List<DOBase> Changes = CurrentBRJob.SelectJobChanges(Job.JobID, DOJobChange.JobChangeType.JobCompleted);
            if (Changes.Count == 0)
                return;
            DOJobChange Change = Changes[0] as DOJobChange;

            phCompleteJob.Visible = true;
            DOContact CompletedBy = CurrentBRContact.SelectContact(Change.CreatedBy);
            DOJobContractor dojc = CurrentBRJob.SelectJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID);
            JobID.Text = dojc.JobNumberAuto.ToString();
            litCompletedBy.Text = CompletedBy.DisplayName;
            litCompletedDate.Text = DateAndTime.DisplayShortDate(Change.CreatedDate);

            bool IncompleteReason = !string.IsNullOrEmpty(Job.IncompleTasksReason);
            litIncompleteReason.Visible = IncompleteReason;
            litIncompleteLabel.Visible = IncompleteReason;
            if (IncompleteReason)
            {
                litIncompleteReason.Text = Job.IncompleTasksReason;
                phTasksIncomplete.Visible = false;
            }

        }


        protected void btnIncompleteSubmit_Click(object sender, EventArgs e)
        {


            ShowIncomplete = true;
            //Make sure a reason was entered.
            // taskIncompletePanel = (Panel)rpJobs.FindControl("phTasksIncomplete");

            // TextBox panelText = (TextBox) taskIncompletePanel.FindControl("txtIncompleteReason");
            if (string.IsNullOrEmpty(txtIncompleteReason.Text))
            {
                ShowMessage("You must enter a reason for incomplete tasks.", MessageType.Error);
            }
            else
            {

                //Button b = sender as Button;
                //if (b != null)
                //{
                //    if (b.CommandName == "SelectedJob")
                //    {
                //        //Guid JobID = new Guid(b.CommandArgument.ToString());
                //        //DOJob Job = CurrentBRJob.SelectJob(JobID);
                //        //// CurrentSessionContext.CurrentJob = Job;
                //        //  Response.Redirect(Constants.URL_JobSummary);
                //    }
                //}
                //Guid JobID1 = new Guid(b.CommandArgument.ToString());
                // Job = CurrentBRJob.SelectJob(JobID1);
                //  Job.JobNumberAuto = JobNumber.Text;
                //
                Guid JobID = new Guid(JobIDtoIncompletePage.Text);

                Job = CurrentBRJob.SelectJob(JobID);
                Job.IncompleTasksReason = txtIncompleteReason.Text;

                CurrentSessionContext.CurrentJob = Job;
                if (CurrentSessionContext.CurrentJob != null)
                {
                    //Reload job from database (in case other users have changed job)
                    CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentJob.JobID);
                    CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
                }
                Job.JobStatus = DOJob.JobStatusEnum.Complete;
                //CurrentSessionContext.Owner = Cu
                Job.CompletedBy = CurrentSessionContext.CurrentContact.ContactID;
                Job.CompletedDate = DateAndTime.GetCurrentDateTime();
                //Console.Write(Job.JobID + "  ");


                // CurrentBRJob.SaveJob(Job);
                jobContractor = CurrentBRJob.SelectJobContractor(Job.JobID,
    CurrentSessionContext.CurrentContact.ContactID);
                jobContractor.Status = 1;
                CurrentBRJob.SaveJobContractor(jobContractor);
                //Find if there are any active jobs for the same contractor in same site
                bool activeJobsForSite = FindActiveJobsForContractorSite(Job.SiteID,
                    CurrentSessionContext.CurrentContact.ContactID);
                if (activeJobsForSite)
                {
                    SetSiteFlag(Job.SiteID, Job.JobID, false);
                }
                //  Console.Write(Job.JobID + "  ");

                LogChange(Job, DOJobChange.JobChangeType.JobCompleted);

                ShowIncomplete = false;
                if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
                {
                    ShowCompletedInfo();
                }
            }
        }

        protected void btnIncompleteCancel_Click(object sender, EventArgs e)
        {
            ShowIncomplete = false;
            phTasksIncomplete.Visible = false;
        }
        public void LogChange(DOJob Job, DOJobChange.JobChangeType ChangeType)
        {
            //  Console.Write(Job.JobID + "  " );
            //  Console.Write(ChangeType + "  ");
            //  Console.Write(CurrentSessionContext.Owner.ContactID + "    ");
            DOJobChange Change = CurrentBRJob.CreateJobChange(Job.JobID, ChangeType, CurrentSessionContext.Owner.ContactID);
            CurrentBRJob.SaveJobChange(Change);

        }
        public void LogChange(DOJobChange.JobChangeType ChangeType)
        {
            DOJobChange Change = CurrentBRJob.CreateJobChange(Job.JobID, ChangeType, CurrentSessionContext.Owner.ContactID);
            CurrentBRJob.SaveJobChange(Change);
        }
        //To get the class for styling

        protected string GetJobClass(DOJob Job)
        {
            var status = Job.JobStatus;
            DOJobContractor contractor = CurrentBRJob.SelectJobContractor(Job.JobID,
                CurrentSessionContext.CurrentContact.ContactID);
            if (status == DOJob.JobStatusEnum.Incomplete)
            {
                if (contractor.Status == 0)
                {
                    return "active";
                }
                return "inactive";
            }
            else
            {
                //return "remove";
                return "inactive";
            }
        }
        protected string GetClass(Guid jobID)
        {
            DOJob Job = CurrentBRJob.SelectJob(jobID);
            var status = Job.JobStatus;
            if (status == DOJob.JobStatusEnum.Incomplete)
            {

                List<DOBase> JobTasks = CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty);
                foreach (DOTask Task in JobTasks)
                {
                    if (Task.Status == DOTask.TaskStatusEnum.Complete)
                    {
                        return "btn-3 text-left";
                    }
                }
                //foreach (DOTask Task in JobTasks)
                //{
                //    if (Task.Status == DOTask.TaskStatusEnum.Invoiced)
                //    {
                //        return "redTask";
                //    }
                //}
                    }
            else
            {
                return "btn-3 text-left";

            }
            return "grey";
        }

        protected void lbToolBoxTalks_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Private/ToolboxTalks.aspx", false);
        }

        protected string GetJobColor(string status, Guid jobid)
        {
            if (status == DOJob.JobStatusEnum.Incomplete.ToString())
            {

                List<DOBase> JobTasks = CurrentBRJob.SelectTasks(jobid);
                foreach (DOTask Task in JobTasks)
                {
                    if (Task.Status == DOTask.TaskStatusEnum.Incomplete)
                    {
                        return "btn-3 text-left";
                    }
                }
                //foreach (DOTask Task in JobTasks)
                //{
                //    if (Task.Status == DOTask.TaskStatusEnum.Invoiced)
                //    {
                //        return "redTask";
                //    }
                //}
                    }
            else
            {
                return "grey";

            }
            return "grey";
        }

        //Tony Added 9.11.2016
        private void LoadSites()
        {
            List<DOBase> Sites = new List<DOBase>();

            DOContact contact;

            if (CurrentSessionContext.CurrentContractee != null)
                contact = CurrentSessionContext.CurrentContractee;
            else
                contact = CurrentSessionContext.CurrentCustomer;

            if (contact != null)
            {
                Guid contact_ContactID = contact.ContactID;
                Guid currentContact_ContactID = CurrentSessionContext.CurrentContact.ContactID;

                Sites = SelectVisibleSites(contact.ContactID, CurrentSessionContext.CurrentContact.ContactID);

                SiteDD.DataSource = Sites;
                SiteDD.DataTextField = "DisplayAddress";
                SiteDD.DataValueField = "SiteId";
                SiteDD.DataBind();
            }
        }
        //Tony Added 9.11.2016

        private List<DOBase> SelectVisibleSites(Guid currentContracteeID, Guid currentCustomerID)
        {
            List<DOBase> ContactSites = CurrentBRSite.SelectSitesforCustomer(currentContracteeID, currentCustomerID);
            List<DOBase> visibleSites = new List<DOBase>();
            visibleSites = ContactSites;

            return visibleSites;
        }

        //Tony Added 15.11.2016
        protected void populateJobs()
        {
            List<DOBase> Jobs = CurrentBRJob.SelectViewableJobs(CurrentSessionContext.CurrentSite.SiteID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);

            JobDD.DataSource = Jobs;
            JobDD.DataTextField = "DisplayName";
            JobDD.DataValueField = "JobId";
            JobDD.DataBind();
        }

        //Tony added 11.11.2016
        private bool getSelectedItem()
        {
            if (JobDD.Items.Count == 0)
            {
                return false;
            }
            else
            {
                selectedJobID = new Guid(JobDD.SelectedItem.Value);
                newSiteID = new Guid(SiteDD.SelectedItem.Value);
                oldSiteID = CurrentSessionContext.CurrentSite.SiteID;
                return true;
            }
        }


        protected void btnMoveOne_Click(object sender, EventArgs e)
        {
            if (getSelectedItem())
            {
                CurrentBRJob.MoveJob(selectedJobID, oldSiteID, newSiteID);
                lblMoveJob.Text = JobDD.SelectedItem.Text + " => " + SiteDD.SelectedItem.Text;
                populateJobs();
            }
            else
            {
                lblMoveJob.BackColor = Color.Coral;
                lblMoveJob.Text = NO_ITEM_LEFT_MESSAGE;
            }
        }

        protected void btnMoveAll_Click(object sender, EventArgs e)
        {
            if (getSelectedItem())
            {
                CurrentBRJob.MoveAllJob(oldSiteID, newSiteID);
                lblMoveJob.Text = "All jobs Moved to " + SiteDD.SelectedItem.Text;
                populateJobs();
            }
            else
            {
                lblMoveJob.BackColor = Color.Coral;
                lblMoveJob.Text = NO_ITEM_LEFT_MESSAGE;
            }
        }

        //Tony added 5.11.2016
        private void shareSite(Guid siteID, Guid fromContactID, Guid toContactID)
        {
            //if any value is selected from company list, do copy and paste current site info
            if (ContactDD.SelectedIndex >= 0)
            {
                CurrentBRSite.ShareContactSite(siteID, fromContactID, toContactID);

                //3.7.17 Jared. Believe we need to add a contractorcustomer if not existing.
                DOContractorCustomer docc = CurrentBRContact.SelectContractorCustomer(toContactID, fromContactID);
                if(docc==null)
                {
                    int intCompanyType = 0;
                    string s = (CurrentSessionContext.CurrentContact.ContactType = DOContact.ContactTypeEnum.Company).ToString();
                    if (s != "Company") intCompanyType = 1;

                    docc = CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, fromContactID,
                          toContactID, CurrentSessionContext.CurrentContact.Address1, CurrentSessionContext.CurrentContact.Address2, CurrentSessionContext.CurrentContact.Address3, CurrentSessionContext.CurrentContact.Address4, CurrentSessionContext.CurrentContact.CompanyName,
                          DOContractorCustomer.LinkedEnum.NotLinked, "", fromContactID, CurrentSessionContext.CurrentContact.FirstName, CurrentSessionContext.CurrentContact.LastName, intCompanyType);
                    CurrentBRContact.SaveContractorCustomer(docc);

                }

            }
        }
        //Tony added 5.11.2016

        protected void LoadContact()
        {
            List<DOBase> contact =
                CurrentBRContact.SiteExcludeCustomers(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentSite.SiteID);
            ContactDD.DataSource = contact;
            ContactDD.DataTextField = "DisplayName";
            ContactDD.DataValueField = "ContactID";
            ContactDD.DataBind();
        }
        //
        protected void btnShare_Click(object sender, EventArgs e)
        {
            //Tony added 5.11.2016
            //If confirm check box for share site information checked, call shareSite method
            DOSite mysite = CurrentSessionContext.CurrentSite;

            siteID = mysite.SiteID;
            fromContactID = mysite.ContactID;
            toContactID = new Guid(ContactDD.SelectedItem.Value);

            shareSite(siteID, fromContactID, toContactID);

            selectedContact = ContactDD.SelectedItem.Text;
            //update the list

            lblShareSite.Text = SHARE_SITE_MESSAGE + selectedContact;

            populateContact();
            LoadContact();
        }

    }
}
