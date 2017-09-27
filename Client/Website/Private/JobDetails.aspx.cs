using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class JobDetails : PageBase
    {
        new DOSite Site;
        DOJob Job;
        DOJobContractor dojc;
       // DOJobContractor dojcCustomer;
        List<DOBase> Tasks;
        private bool ShowIncomplete = false;

        protected DOTask GetReferenceTask()
        {
            if (Tasks == null) return null;
            var Task = from DOTask task in Tasks
                       where task.TaskType == DOTask.TaskTypeEnum.Reference
                       orderby task.CreatedDate
                       select task;
            return Task.SingleOrDefault<DOTask>();
        }

        private void GetTasks()
        {
            if (Job.PersistenceStatus == ObjectPersistenceStatus.Existing)
            {
                Tasks = CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty);

                DOTask AcknowledgeTask = null;
                {
                    foreach (DOTask Task in Tasks)
                    {
                        //if (Task.TaskType == DOTask.TaskTypeEnum.Acknowledgement && Task.Status != DOTask.TaskStatusEnum.Amended)
                        //{
                        //    DOTaskAcknowledgement TA = CurrentBRJob.SelectTaskAcknowledgement(Task.TaskID, CurrentSessionContext.Owner.ContactID);
                        //    if (TA == null)
                        //    {
                        //        AcknowledgeTask = Task;
                        //        break;
                        //    }
                        //}
                    }
                }
                if (AcknowledgeTask != null)
                {
                    CurrentSessionContext.CurrentTask = AcknowledgeTask;
                    Response.Redirect(Constants.URL_TaskAcknowledgement);
                }
                //TaskList.Job = Job;
                //TaskList.TaskList = Tasks;
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
            CheckJobQuerystring();
            //Must have a job or site selected.
            if (CurrentSessionContext.CurrentJob == null && CurrentSessionContext.CurrentSite == null)
                Response.Redirect(Constants.URL_Home);
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);

            if (CurrentSessionContext.CurrentJob != null)
            {
                //Reload job from database (in case other users have changed job)
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentJob.JobID);
                CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
            }
            Site = CurrentSessionContext.CurrentSite;
            Job = CurrentSessionContext.CurrentJob;

            ClearTask();
            //ClearTimeSheets();

            if (Job == null)
            {
                //btnCompleteJob.Visible = false;
                Job = CurrentBRJob.CreateJob(Site.SiteID, CurrentSessionContext.Owner.ContactID);
            }
            if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                btnSave.Enabled = false;

            }
            if (Job == null || Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                btnAddTask.Enabled = false;
                btnUploadImage.Enabled = false;
                QuoteStatus.Visible = false;
            }
            else
            {
                QuoteStatus.Job = Job;
            }
            dojc = CurrentBRJob.SelectJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID);
            GetTasks();

            EnsureEmployees();
            //CheckTimeSheetAdminMode();
        }

        private void CheckJobQuerystring()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["jobid"]))
            {
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(new Guid(Request.QueryString["jobid"]));
                CurrentSessionContext.CurrentSite = CurrentBRSite.SelectSite(CurrentSessionContext.CurrentJob.SiteID);
                CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentJob.JobOwner);

                //Must be job owner or customer to access via link.
                bool HasPermission = false;
                Guid ContactID = CurrentSessionContext.Owner.ContactID;
                if (CurrentSessionContext.CurrentJob.JobOwner == ContactID || CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentJob.JobOwner, ContactID))
                {
                    HasPermission = true;
                }
                else
                {
                    DOCustomer customer = CurrentBRContact.SelectSiteCustomer(CurrentSessionContext.CurrentSite.SiteID, CurrentSessionContext.CurrentJob.JobOwner);
                    if (CurrentSessionContext.Owner.ContactID == customer.ContactID || CurrentBRContact.CheckCompanyContact(customer.ContactID, CurrentSessionContext.Owner.ContactID))
                    {
                        HasPermission = true;
                    }

                }

                if (HasPermission)
                    Response.Redirect(Constants.URL_JobDetails);
                else
                    Response.Redirect(Constants.URL_Home);

                Response.End();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidFindNewContractorID.Value))
            {
                FindNewContractorID = new Guid(hidFindNewContractorID.Value);
            }
            if (!string.IsNullOrEmpty(hidPendingContractorEmail.Value))
            {
                PendingContractorEmail = hidPendingContractorEmail.Value;
            }

            //  RegionDD.AppendDataBoundItems = true;
            if (!IsPostBack)
            {
                JobMoreDetail.Visible = false;
                LoadJTDropDownList();
                //MoreDetailsBtn.Text = "View more details";
                //List<DOBase> Regions = CurrentBRRegion.SelectRegions();
                //RegionDD.DataSource = Regions;
                //RegionDD.DataTextField = "RegionName";
                //RegionDD.DataValueField = "RegionID";
                //RegionDD.DataBind();
                //LoadSuburbs();
                //LoadTradeCategories();
            }

        }

        protected void LoadJTDropDownList()//job templates
        {

            List<DOBase> LineItems = CurrentBRContact.SelectJobTemplatesByCompanyID(Guid.Parse("ECA7B55C-3971-41DA-8E84-A50DA10DD233"));//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
            ddJobTemplates.DataSource = LineItems;
            ddJobTemplates.DataTextField = "JobTemplateName";
            ddJobTemplates.DataValueField = "JobTemplateID";
            ddJobTemplates.DataBind();
           
        }

        protected void ddJobTemplates_DataBound(object sender, EventArgs e)
        {
            DropDownList list = sender as DropDownList;
            ListItem l = list.Items.FindByText("--Select One--");

            if (l == null)
            {



                if (list != null)
                    list.Items.Insert(0, "--Select One--");
            }
        }


        protected void ddJobTemplates_OnChange(object sender, EventArgs e)
        {
            DropDownList dd = ddJobTemplates;
            if (ddJobTemplates.SelectedIndex != 0)
            {


                System.Diagnostics.Debug.WriteLine("                                                  " + dd.SelectedIndex + " " + dd.SelectedItem + " " + dd.SelectedValue);

            }
        }


        //gets the trade categories
        public void LoadTradeCategories()
        {

            List<DOBase> tradeCategories = new List<DOBase>();
            tradeCategories = CurrentBRTradeCategory.SelectTradeCategories();
            List<DOBase> uniqueTradeCatgeories = tradeCategories.Distinct().ToList();
            TradeCategories_List.DataSource = uniqueTradeCatgeories;
            TradeCategories_List.DataTextField = "TradeCategoryName";
            TradeCategories_List.DataValueField = "TradeCategoryID";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadForm();
            if (ShowIncomplete)
            {
                phTasksIncomplete.Visible = true;
            }

            if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                ShowCompletedInfo();
            }

            hidFindNewContractorID.Value = FindNewContractorID.HasValue ? FindNewContractorID.Value.ToString() : string.Empty;
            hidPendingContractorEmail.Value = PendingContractorEmail;
            phNewJobFields.Visible = Job.PersistenceStatus == ObjectPersistenceStatus.New;
            phFiles.Visible = Job.PersistenceStatus != ObjectPersistenceStatus.New;
            if (!IsPostBack)
                LoadContractors();
            LoadTimeDropdowns();

            //if (Job.PersistenceStatus == ObjectPersistenceStatus.New)
            //{
            //    ListItem liQuoted = ddlJobType.Items.FindByValue("1");
            //    ddlJobType.Items.Remove(liQuoted);
            //}

            if (Job.JobType == DOJob.JobTypeEnum.ChargeUp)
            {
                QuoteStatus.Visible = false;
            }
            DataBind();

        }

        private void ShowCompletedInfo()
        {
            List<DOBase> Changes = CurrentBRJob.SelectJobChanges(Job.JobID, DOJobChange.JobChangeType.JobCompleted);
            if (Changes.Count == 0)
                return;
            DOJobChange Change = Changes[0] as DOJobChange;

            phCompleteJob.Visible = true;
            DOContact CompletedBy = CurrentBRContact.SelectContact(Change.CreatedBy);
            litCompletedBy.Text = CompletedBy.DisplayName;
            litCompletedDate.Text = DateAndTime.DisplayShortDate(Change.CreatedDate);

            bool IncompleteReason = !string.IsNullOrEmpty(Job.IncompleTasksReason);
            litIncompleteReason.Visible = IncompleteReason;
            litIncompleteLabel.Visible = IncompleteReason;
            if (IncompleteReason)
                litIncompleteReason.Text = Job.IncompleTasksReason;
        }

        private void LoadForm()
        {

            if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                btnSave.Enabled = false;

            }
            else
            {
                btnSave.Enabled = true;
            }
            if (!IsPostBack)
            {
                DOJobContractor dojc = CurrentBRJob.SelectJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID);
                txtJobName.Text = Job.Name;
                //chkActive.Checked = Job.Active;
                DOTask Reference = GetReferenceTask();
                if (Reference != null)
                {
                    txtJobDescription.Text = Reference.Description;
                    txtJobDescription.Enabled = false;
                }

                //txtJobNumber.Text = Job.JobNumber;

                //if (Job.PersistenceStatus == ObjectPersistenceStatus.New)
                //{
                //    txtJobNumberAuto.Text = CurrentBRJob.SelectJobNumberAuto(CurrentSessionContext.CurrentContact.ContactID);
                //}
                //else
                //{
                //    txtJobNumberAuto.Text = Job.JobNumberAuto;
                //}
                if(dojc!=null)
                    txtJobNumberAuto.Text = dojc.JobNumberAuto.ToString();
                phJobNumberAuto.Visible = Job.PersistenceStatus == ObjectPersistenceStatus.Existing;

                txtProjectManagerText.Text = Job.ProjectMangerText;
                txtProjectManagerPhone.Text = Job.ProjectManagerPhone;

                txtAlarmCode.Text = Job.AlarmCode;
                txtAccessTypeCustom.Text = Job.AccessTypeCustom;
                txtPoweredItems.Text = Job.PoweredItems;
                chkNoPoweredItems.Checked = Job.NoPoweredItems;
                txtSiteNotes.Text = Job.SiteNotes;
                txtStockRequired.Text = Job.StockRequired;
            }

            //  BindSubscribedContacts();
            BindOwnerDropDown();
            BindProjectManagerDropDown();
            BindInvoiceToDropDown();
            //ShowQuotes();
            List<DOBase> jobContractors = CurrentBRContact.SelectJobContractorContacts(Job.JobID);
            rpJobContractors.DataSource = jobContractors;
            rpJobContractors.DataBind();

            FileDisplayer1.FileList = CurrentBRJob.SelectFilesForJob(Job.JobID);
            pnlNoContractors.Visible = jobContractors.Count == 0;

            ddlAccessType.SelectedValue = FromForm(ddlAccessType.UniqueID, ((int)Job.AccessType).ToString());
            //ddlJobType.SelectedValue = FromForm(ddlJobType.UniqueID, ((int)Job.JobType).ToString());
            //if (Job.PersistenceStatus == ObjectPersistenceStatus.Existing)
            //{
            //    ddlJobType.Enabled = false;
            //}

            //btnSubmitQuote.Visible = (Job.PersistenceStatus == ObjectPersistenceStatus.Existing &&
            //    Job.JobType == DOJob.JobTypeEnum.ToQuote && Job.JobStatus != DOJob.JobStatusEnum.Complete);
            //if (btnSubmitQuote.Visible)
            //{
            //    List<DOBase> JobQuotes = CurrentBRJob.SelectJobQuotes(Job.JobID);
            //    //if (JobQuotes.Count > 0)
            //    //{
            //    //    btnSubmitQuote.Visible = false;
            //    //}
            //    //else 
            //    if (CurrentSessionContext.Owner.ContactID != Job.JobOwner && !CurrentBRContact.CheckCompanyContact(Job.JobOwner, CurrentSessionContext.Owner.ContactID))
            //    {
            //        btnSubmitQuote.Visible = false;
            //    }
            //}

            btnCompleteJob.Visible = (Job.JobStatus != DOJob.JobStatusEnum.Complete);
            if (Job.PersistenceStatus == ObjectPersistenceStatus.New)
                btnCompleteJob.Visible = false;
            btnUncompleteJob.Visible = Job.JobStatus == DOJob.JobStatusEnum.Complete;
            phTasksIncomplete.Visible = ShowIncomplete;
        }

        //private void ShowQuotes()
        //{            
        //    if (Job.PersistenceStatus == ObjectPersistenceStatus.New)
        //    {
        //        phQuotes.Visible = false;
        //    }
        //    else
        //    {
        //        phQuotes.Visible = true;
        //        Quotes.Job = Job;
        //    }
        //}

        private string FromForm(string Key, string Default)
        {
            if (Request.Form[Key] == null)
                return Default;
            else
                return Request.Form[Key];
        }

        private void BindSubscribedContacts()
        {
            List<DOBase> Subscribers = CurrentBRContact.SelectSubscribedContacts();
            Guid SelectedSub = Guid.Empty;
            if (!string.IsNullOrEmpty(Request.Form[ddlContractors.UniqueID]))
                SelectedSub = new Guid(Request.Form[ddlContractors.UniqueID]);

            ddlContractors.Items.Clear();
            foreach (DOContact Contact in Subscribers)
            {
                ddlContractors.Items.Add(new ListItem(Contact.DisplayName, Contact.ContactID.ToString()) { Selected = Contact.ContactID == SelectedSub });
            }
        }

        private bool Save()
        {
            //Job.Active = chkActive.Checked;
            bool NewJob = (Job.PersistenceStatus == ObjectPersistenceStatus.New);

            if (txtJobName.Text == "")
                throw new FieldValidationException("Job name is required");
            if (txtJobDescription.Text == "")
                throw new FieldValidationException("Job Description is required");
            Job.Name = txtJobName.Text;
            Job.Description = txtJobDescription.Text;
            Job.JobNumber = txtJobNumber.Text;
            if(Job.InvoiceTo==null)
            Job.InvoiceTo = GetDDLGuid(ddlInvoiceTo);

            if (CurrentSessionContext.CurrentContact.ContactID == Job.JobOwner || NewJob)
            {
                Job.ProjectManagerID = GetDDLGuid(ddlProjectManager);
                Job.ProjectMangerText = txtProjectManagerText.Text;
                Job.ProjectManagerPhone = txtProjectManagerPhone.Text;
            }

            Job.AlarmCode = txtAlarmCode.Text;
            Job.AccessTypeCustom = txtAccessTypeCustom.Text;
            Job.NoPoweredItems = chkNoPoweredItems.Checked;
            if (Job.NoPoweredItems)
                Job.PoweredItems = string.Empty;
            else
                Job.PoweredItems = txtPoweredItems.Text;

            if (string.IsNullOrEmpty(Job.PoweredItems) && !Job.NoPoweredItems)
            {
                throw new FieldValidationException("Enter items requiring long term power in the text box or select no items.");
            }

            Job.SiteNotes = txtSiteNotes.Text;
            Job.StockRequired = txtStockRequired.Text;

            Job.AccessType = (DOJob.JobAccessType)int.Parse(ddlAccessType.SelectedValue);

            txtAlarmCode.Text = Job.AlarmCode;
            txtAccessTypeCustom.Text = Job.AccessTypeCustom;
            txtPoweredItems.Text = Job.PoweredItems;
            txtSiteNotes.Text = Job.SiteNotes;
            txtStockRequired.Text = Job.StockRequired;

            if (NewJob)
            {
                //Job.JobOwner = new Guid(Request.Form[ddlJobOwner.UniqueID]);
                Job.JobOwner = Guid.Parse(ddlJobOwner.SelectedValue);
                // Job.JobType = (DOJob.JobTypeEnum)int.Parse(ddlJobType.SelectedValue);
                if (string.IsNullOrEmpty(txtJobDescription.Text))
                {
                    throw new FieldValidationException("Job description is required.");
                }
            }

            CurrentBRJob.SaveJob(Job);

            CurrentBRJob.UpdateSite(Job.SiteID);
            //DOJobContractor doJCJobOwner= CurrentBRJob.CreateJobContractor(Job.JobID, Job.JobOwner, CurrentSessionContext.Owner.ContactID);
            // CurrentBRJob.SaveJobContractor(doJCJobOwner);
            //2017.4.27 replaced below
            //if (NewJob)
            //{
            //    DOContractorCustomer docc = CurrentBRContact.SelectContractorCustomerByCCID(CurrentSessionContext.CurrentSite.SiteOwnerID);

            //    if (Job.JobOwner != docc.CustomerID)
            //    {
            //        DOJobContractor doJCSiteOwner = CurrentBRJob.SelectJobContractor(Job.JobID, docc.CustomerID);
            //        if (doJCSiteOwner == null)
            //        {

            //            doJCSiteOwner = CurrentBRJob.CreateJobContractor(Job.JobID, docc.CustomerID, CurrentSessionContext.Owner.ContactID);
            //            CurrentBRJob.SaveJobContractor(doJCSiteOwner);
            //        }
            //    }
            //    //if (Job.JobOwner != CurrentSessionContext.CurrentSite.SiteOwnerID && CurrentSessionContext.CurrentSite.SiteOwnerID != CurrentSessionContext.CurrentContractee.ContactID && CurrentSessionContext.CurrentContractee.ContactID != Job.JobOwner)
            //    //{
            //    if (CurrentSessionContext.CurrentContractee != null)
            //        if (docc.CustomerID != CurrentSessionContext.CurrentContractee.ContactID)
            //        {
            //            DOJobContractor doJCContractor = CurrentBRJob.CreateJobContractor(Job.JobID,
            //            CurrentSessionContext.CurrentContractee.ContactID, CurrentSessionContext.Owner.ContactID);
            //            CurrentBRJob.SaveJobContractor(doJCContractor);
            //        }
            //    //}
            //    //if (Job.JobOwner != CurrentSessionContext.CurrentSite.SiteOwnerID 
            //    //    && CurrentSessionContext.CurrentSite.SiteOwnerID != CurrentSessionContext.CurrentContractee.ContactID 
            //    //    && CurrentSessionContext.CurrentContractee.ContactID != Job.JobOwner
            //    //   && CurrentSessionContext.CurrentContractee.ContactID !=CurrentSessionContext.CurrentContact.ContactID)
            //    //{
            //    if (CurrentSessionContext.CurrentContact.ContactID != docc.CustomerID)
            //    {
            //        DOJobContractor doJCContractorCreatingContact = CurrentBRJob.SelectJobContractor(Job.JobID,
            //        CurrentSessionContext.CurrentContact.ContactID);
            //        if (doJCContractorCreatingContact == null)
            //        {
            //            doJCContractorCreatingContact = CurrentBRJob.CreateJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);
            //        }
            //        else
            //        {
            //            doJCContractorCreatingContact.Active = true;
            //        }
            //        CurrentBRJob.SaveJobContractor(doJCContractorCreatingContact);

            //    }
            //}
            //replacement below
            if (NewJob)
            {
                //share the job with the siteowner first
                DOContractorCustomer doccSiteOwner = CurrentBRContact.SelectContractorCustomerByCCID(CurrentSessionContext.CurrentSite.SiteOwnerID);
                //DOContractorCustomer doccSiteOwner = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentSite.SiteOwnerID);
                //above, possibly need to deal with if this null todo
                //below this if statement makes sure that 
                if(doccSiteOwner.ContractorId!=doccSiteOwner.CustomerID)
                {
                    Guid GuidSiteOwner = doccSiteOwner.CustomerID;
                    //doccSiteOwner = CurrentBRContact.SelectContactCustomer(doccSiteOwner.CustomerID, doccSiteOwner.CustomerID);
                    doccSiteOwner = CurrentBRContact.SelectContactCustomer(GuidSiteOwner, GuidSiteOwner);

                    //if above is null then it is due to the old version of the database not having a contractorcustomer record where the siteowner is both 
                    //the contractor and the customer. i.e. they need their own contractorcustomer record. Hence:
                    if (doccSiteOwner == null)
                    {
                        doccSiteOwner = CurrentBRContact.CreateContractorCustomer(CurrentSessionContext.Owner.ContactID, GuidSiteOwner, GuidSiteOwner
                            , CurrentSessionContext.CurrentContractee.Address1, CurrentSessionContext.CurrentContractee.Address2, CurrentSessionContext.CurrentContractee.Address3
                            , CurrentSessionContext.CurrentContractee.Address4, CurrentSessionContext.CurrentContractee.CompanyName, DOContractorCustomer.LinkedEnum.NotLinked
                            , CurrentSessionContext.CurrentContractee.Phone, Guid.Empty, CurrentSessionContext.CurrentContractee.FirstName, CurrentSessionContext.CurrentContractee.LastName
                            , (int)CurrentSessionContext.CurrentContractee.ContactType);
                        CurrentBRContact.SaveContractorCustomer(doccSiteOwner);
                    }
                    CurrentSessionContext.CurrentSite.SiteOwnerID = doccSiteOwner.ContactCustomerId;
                    CurrentBRSite.SaveSite(CurrentSessionContext.CurrentSite);
                }
                DOContact siteowner = CurrentBRContact.SelectContact(doccSiteOwner.CustomerID);
                DOContact currentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
                DOContact currentContractee = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContractee.ContactID);
                DOJobContractor doJCSiteOwner = CurrentBRJob.SelectJobContractor(Job.JobID, siteowner.ContactID);

                if (doJCSiteOwner == null)
                {
                    doJCSiteOwner = CurrentBRJob.CreateJobContractor(Job.JobID, siteowner.ContactID, CurrentSessionContext.Owner.ContactID);
                }
                doJCSiteOwner.JobNumberAuto = siteowner.JobNumberAuto;
                CurrentBRJob.SaveJobContractor(doJCSiteOwner);
                siteowner.JobNumberAuto++;
                CurrentBRContact.SaveContact(siteowner);

                //logged in entity is not the site owner
                if (currentContact.ContactID!=siteowner.ContactID)
                {
                    //issue here jared 6.6.17
                    DOJobContractor doJCCurrentContact; //jared 11.6.17 = CurrentBRJob.SelectJobContractor(Job.JobID, currentContact.ContactID);
                    doJCCurrentContact = CurrentBRJob.CreateJobContractor(Job.JobID, currentContact.ContactID, CurrentSessionContext.Owner.ContactID);
                    doJCCurrentContact.JobNumberAuto = currentContact.JobNumberAuto+1;
                    CurrentBRJob.SaveJobContractor(doJCCurrentContact);
                    currentContact.JobNumberAuto++;
                    CurrentBRContact.SaveContact(currentContact);
                }
                if (currentContractee.ContactID != siteowner.ContactID)
                {
                    DOJobContractor doJCCurrentContractee = CurrentBRJob.SelectJobContractor(Job.JobID, currentContractee.ContactID);
                    doJCCurrentContractee = CurrentBRJob.CreateJobContractor(Job.JobID, currentContractee.ContactID, CurrentSessionContext.Owner.ContactID);
                    doJCCurrentContractee.JobNumberAuto = currentContractee.JobNumberAuto;
                    CurrentBRJob.SaveJobContractor(doJCCurrentContractee);
                    currentContractee.JobNumberAuto++;
                    CurrentBRContact.SaveContact(currentContractee);
                }
            }
            //eob 2017.4.27 jared
            CurrentSessionContext.CurrentJob = Job;
            ShowMessage("Job Details Saved.", MessageType.Info);

            if (NewJob)
            {
                // Assign the auto job number from the creating contact and increment.
                // 11.6.17 jared CurrentSessionContext.CurrentContact.JobNumberAuto++;
                // 11.6.17 jared CurrentBRContact.SaveContact(CurrentSessionContext.CurrentContact);
                phJobNumberAuto.Visible = true;
                DOJobContractor dojc = CurrentBRJob.SelectJobContractor(Job.JobID, CurrentSessionContext.CurrentContact.ContactID);
                //dojc.JobNumberAuto = CurrentSessionContext.CurrentContact.JobNumberAuto;
                //CurrentBRJob.SaveJobContractor(dojc);
                txtJobNumberAuto.Text =  dojc.JobNumberAuto.ToString();



                //Create the reference task.
                DOTask ReferenceTask = CurrentBRJob.CreateTask(Job.JobID, CurrentSessionContext.Owner.ContactID);
                ReferenceTask.Description = txtJobDescription.Text;
                ReferenceTask.TaskOwner = CurrentSessionContext.CurrentContact.ContactID;
                ReferenceTask.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                //ReferenceTask.ContractorID = new Guid(Request.Form[ddlContractor.UniqueID]);
                ReferenceTask.TaskNumber = 1;

                if (ReferenceTask.ContractorID == Constants.Guid_DefaultUser)
                {
                    //Pending user. Add them to the task pendign contractor table so they can be linked when they register.
                    //Add entry to contractor pending list so the task can be updated when the contractor registers.
                    DOTaskPendingContractor TPC = CurrentBRJob.CreateTaskPendingContractor(CurrentSessionContext.Owner.ContactID);
                    TPC.ContractorEmail = PendingContractorEmail;
                    TPC.TaskID = ReferenceTask.TaskID;
                    CurrentBRJob.SaveTaskPendingContractor(TPC);

                }
                else
                {
                    //Notify contractor that they have been added.
                    DOContact Contractor = CurrentBRContact.SelectContact(ReferenceTask.ContractorID);
                    DOJobContractor dojcCustomer = CurrentBRJob.SelectJobContractor(Job.JobID, Contractor.ContactID);
                    string Body = "Job Name: " + Job.Name + "<br >Task Name: " + ReferenceTask.TaskName + "<br />" + ReferenceTask.Description;
                    Body += "<br /><a href=\"" + CurrentBRGeneral.SelectWebsiteBasePath() + "/private/TaskDetails.aspx?taskid=" + ReferenceTask.TaskID.ToString() + "\">View Task</a>";
                    if (Constants.EMAIL__TESTMODE)
                    {
                        Body += "<br/><br/>";
                        Body += "Sender: " + CurrentSessionContext.CurrentContact.DisplayName + "<br />";
                        Body += "Sender logged in as: " + CurrentSessionContext.Owner.DisplayName + " (" + CurrentSessionContext.Owner.UserName + ")<br />";
                        Body += "Recipient: " + Contractor.DisplayName + " (" + Contractor.Email + ")<br />";
                        Body += "Job ID: " + dojcCustomer.JobNumberAuto;
                    }

                    CurrentBRGeneral.SendEmail("no-reply@ontrack.co.nz", Contractor.Email, CurrentSessionContext.CurrentContact.DisplayName + " has assigned you a task on Ontrack", Body);
                }


                ReferenceTask.TaskType = DOTask.TaskTypeEnum.Reference;
                ReferenceTask.TaskName = Job.Name;
                ReferenceTask.Description = Job.Description;

                ReferenceTask.StartDate = dateStartDate.GetDate();
                ReferenceTask.StartMinute = int.Parse(ddlStartTime.SelectedValue);
                ReferenceTask.EndDate = dateEndDate.GetDate();
                ReferenceTask.EndMinute = int.Parse(ddlEndTime.SelectedValue);
                ReferenceTask.Appointment = chkAppointment.Checked;

                if (ReferenceTask.Appointment && (ReferenceTask.StartDate == DateAndTime.NoValueDate || ReferenceTask.StartMinute < 0))
                    throw new Exception("Must enter date for appointment.");
                ReferenceTask.TaskCustomerID = CurrentSessionContext.CurrentSite.SiteOwnerID;//changed by jared 5/12/16 was currentsession.owner(the company - not the user)
                ReferenceTask.SiteID = CurrentSessionContext.CurrentSite.SiteID;
                CurrentBRJob.SaveTask(ReferenceTask);



                //Jareds code below
                    if (ddJobTemplates.SelectedIndex != 0)
                    {

                        //DOBase DOBJTT = CurrentBRJob.SelectVehicleByDriverID(Guid.Parse("53E58C1B-4D58-41F9-9849-FBB5B4F87833"));
                       // DOVehicle V = DOBJTT as DOVehicle;

                        List<DOBase> BJTT = CurrentBRJob.SelectJobTemplateTasks(Guid.Parse(ddJobTemplates.SelectedValue));
                        //DOJobTemplateTask DOJTT = DOBJTT as DOJobTemplateTask;
                         
                        foreach (DOMyJobTemplate JTT in BJTT)
                        {

                            //Create new task
                            DOTask Task = new DOTask();
                            Guid TaskGuid = Guid.NewGuid();
                            Task.TaskID = TaskGuid;
                            Task.Description = JTT.Description;
                            Task.TaskName = JTT.TaskName;
                            Task.Status = DOTask.TaskStatusEnum.Incomplete;
                            Task.JobID = Job.JobID;
                            Task.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                            Task.ParentTaskID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                            Task.TaskType = DOTask.TaskTypeEnum.Standard;
                            Task.CreatedBy = Job.CreatedBy;
                            Task.CreatedDate = DateTime.Now;
                            Task.Active = true;
                            Task.TaskOwner = Guid.Parse("00000000-0000-0000-0000-000000000000");
                            Task.AmendedBy = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
                            Task.AmendedTaskID = Guid.Parse("00000000-0000-0000-0000-000000000000");

                            Task.TaskCustomerID = CurrentSessionContext.CurrentContact.ContactID;// needs to be sorted !fix!
                            Task.SiteID = Job.SiteID;
                            Task.TaskOwner = CurrentSessionContext.CurrentContact.ContactID;

                        //code for start dates and end dates. based on job start date
                            DateTime MyDate = dcStartDate.GetDate();
                            MyDate = MyDate.AddDays(decimal.ToDouble(JTT.StartDelay));
                            Task.StartDate = MyDate;

                           
                            MyDate = MyDate.AddDays(decimal.ToDouble(JTT.StartDelay) + decimal.ToDouble(JTT.Duration));
                            Task.EndDate = MyDate;
                            CurrentBRJob.SaveTask(Task);

                    }
                }


                //Jareds code above




                // SetCustomerActive();

                //Refresh task list.
                GetTasks();

                btnAddTask.Enabled = true;
                btnUploadImage.Enabled = true;
                //Response.Redirect(Constants.URL_TaskDetails);
                Response.Redirect(Constants.URL_JobSummary);
            }

            return true;

        }


        private void SetCustomerActive()
        {
            DOContact customer = CurrentSessionContext.CurrentCustomer;
            if (customer != null)
            {
                DOContractorCustomer contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, customer.ContactID);
                if (contractorCustomer != null)
                {
                    contractorCustomer.Active = true;
                    CurrentBRContact.SaveContractorCustomer(contractorCustomer);
                }

              else  if (CurrentSessionContext.CurrentContact.ContactID == customer.ContactID)
                {
                    contractorCustomer = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, customer.ContactID);
                    contractorCustomer.Active = true;
                    CurrentBRContact.SaveContractorCustomer(contractorCustomer);
                }
            }
        }
        private void BindOwnerDropDown()
        {
            //Job owner can only be selected on a new job. Otherwise, show the current job owner.
            List<DOBase> contacts;
            if (Job.PersistenceStatus == ObjectPersistenceStatus.Existing)
            {
                contacts = new List<DOBase>();

                DOContactBase Customer = CurrentBRContact.SelectCustomerByContactID(CurrentSessionContext.CurrentContact.ContactID, Job.JobOwner);
                if (Customer != null)
                {
                    //If job owner is customer, show customer name.
                    Customer.FirstName = "< Customer >";
                    Customer.LastName = string.Empty;// Customer.LastName;
                    Customer.CompanyName = string.Empty;// Customer.CompanyName;

                    contacts.Add(Customer);
                }
                else
                {
                    contacts.Add(CurrentBRContact.SelectContact(Job.JobOwner));
                }
            }
            else
            {
                contacts = CurrentBRContact.SelectContactCompanies(CurrentSessionContext.Owner.ContactID);
                contacts.Insert(0, CurrentSessionContext.Owner);

                //Add customer as options.
                DOContact CustomerContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentSite.ContactID);
                DOContactBase Customer = CurrentBRContact.SelectCustomerByContactID(CurrentSessionContext.CurrentContact.ContactID, CustomerContact.ContactID);
                if (Customer != null)
                {
                    //Use name of customer instead of name of contact (if available).
                    //CustomerContact.FirstName = Customer.FirstName;
                    //CustomerContact.LastName = Customer.LastName;
                    //CustomerContact.CompanyName = Customer.CompanyName;
                    CustomerContact.FirstName = "< Customer >";
                    CustomerContact.LastName = string.Empty;// Customer.LastName;
                    CustomerContact.CompanyName = string.Empty;// Customer.CompanyName;
                    contacts.Insert(0, CustomerContact);
                    //ddlJobOwner.SelectedValue = CurrentSessionContext.CurrentContact.ContactID.ToString();
                    ddlJobOwner.SelectedValue = CustomerContact.ContactID.ToString();
                }

            }
            ddlJobOwner.DataSource = contacts;
            ddlJobOwner.DataTextField = "DisplayName";
            ddlJobOwner.DataValueField = "ContactID";
            ddlJobOwner.DataBind();

            SetDDLValuePostBack(ddlJobOwner);
        }

        private void BindInvoiceToDropDown()
        {
            ddlInvoiceTo.Items.Clear();
            ddlInvoiceTo.Items.Add(new ListItem() { Text = "Customer", Value = Constants.Guid_DefaultUser.ToString() });
        }

        private void BindProjectManagerDropDown()
        {

            ddlProjectManager.Items.Clear();

            bool CanSelect = (Job.JobOwner == CurrentSessionContext.CurrentContact.ContactID || Job.PersistenceStatus == ObjectPersistenceStatus.New);
            if (CanSelect)
            {
                List<DOBase> Contacts;
                if (CurrentSessionContext.CurrentContact.ContactType == DOContact.ContactTypeEnum.Company)
                {
                    Contacts = CurrentBRContact.SelectCompanyEmployees(CurrentSessionContext.CurrentContact.ContactID);
                }
                else
                {
                    Contacts = new List<DOBase>();
                    Contacts.Add(CurrentSessionContext.CurrentContact);
                }

                Contacts.Insert(Contacts.Count, CurrentBRContact.GetDefaultUser("Other (Specify)"));
                foreach (DOBase c in Contacts)
                {
                    string Name;
                    Guid ContactID;
                    if (c is DOContact)
                    {
                        Name = (c as DOContact).DisplayName;
                        ContactID = (c as DOContact).ContactID;
                    }
                    else
                    {
                        Name = (c as DOContactEmployee).DisplayName;
                        ContactID = (c as DOContactEmployee).ContactID;
                    }

                    ddlProjectManager.Items.Add(new ListItem(Name, ContactID.ToString()));
                }
                if (!IsPostBack)
                {
                    if (Job.PersistenceStatus == ObjectPersistenceStatus.New)
                    {
                        //ddlProjectManager.SelectedValue = CurrentSessionContext.Owner.ContactID.ToString();
                        var item = ddlProjectManager.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString());
                        if (item != null)
                        {
                            ddlProjectManager.SelectedValue = CurrentSessionContext.Owner.ContactID.ToString();
                        }
                        else
                            ddlProjectManager.SelectedIndex = 0;
                    }
                    else
                    {
                        ListItem li = ddlProjectManager.Items.FindByValue(Job.ProjectManagerID.ToString());
                        if (li == null)
                        {
                            DOContact ProjectManager = CurrentBRContact.SelectContact(Job.ProjectManagerID);
                            ddlProjectManager.Items.Add(new ListItem(ProjectManager.DisplayName, ProjectManager.ContactID.ToString()));
                        }
                        ddlProjectManager.SelectedValue = Job.ProjectManagerID.ToString();
                    }
                }
                else
                {
                    SetDDLValuePostBack(ddlProjectManager);
                }
            }

            else
            {
                string Name;
                Guid ContactID;
                //DOContact ProjectManager = CurrentBRContact.SelectContact(Job.ProjectManagerID);
                DOContact JobOwner = CurrentBRContact.SelectContact(Job.JobOwner);
                if (JobOwner.ContactType == DOContact.ContactTypeEnum.Individual)
                {
                    Name = JobOwner.DisplayName;
                    ContactID = JobOwner.ContactID;
                }
                else
                {
                    DOContact contactPM = CurrentBRContact.SelectContact(Job.ProjectManagerID);
                    if (contactPM == null)
                    {
                        Name = "Unknown";
                    }
                    else
                    {
                        Name = contactPM.DisplayName;
                    }
                    ContactID = Job.ProjectManagerID;

                    //DOEmployeeInfo eiPM = CurrentBRContact.SelectEmployeeInfo(Job.ProjectManagerID, JobOwner.ContactID);
                    //if (eiPM == null)
                    //{
                    //    eiPM = new DOEmployeeInfo();
                    //    DOContact ProjectManager = CurrentBRContact.SelectContact(Job.ProjectManagerID);
                    //    DOContactCompany cc = CurrentBRContact.SelectContactCompany(ProjectManager.ContactID, JobOwner.ContactID);
                    //    if (cc == null)
                    //    {
                    //        cc = CurrentBRContact.CreateContactCompany(ProjectManager.ContactID, JobOwner.ContactID, CurrentSessionContext.Owner.ContactID);
                    //        CurrentBRContact.SaveContactCompany(cc);
                    //    }
                    //    eiPM = CurrentBRContact.CreateEmployeeInfo(cc.ContactCompanyID, CurrentSessionContext.Owner.ContactID);
                    //}
                    //Name = eiPM.DisplayName;
                    //ContactID = Job.ProjectManagerID;

                }
                ddlProjectManager.Items.Add(new ListItem(Name, ContactID.ToString()));
                ddlProjectManager.Enabled = false;
                txtProjectManagerPhone.Enabled = false;
                txtProjectManagerText.Enabled = false;
            }
            ddlProjectManager.DataBind();
        }

        private void EnsureEmployees()
        {
            if (CurrentSessionContext.CurrentContact.ContactType != DOContact.ContactTypeEnum.Company) return;
            List<DOBase> NoInfo = CurrentBRContact.SelectCompanyEmployeesWithoutInfo(CurrentSessionContext.CurrentContact.ContactID);
            foreach (DOContact c in NoInfo)
            {
                //Create employee info for this contact.
                Guid contactCompanyID = CurrentBRContact.SelectContactCompany(c.ContactID, CurrentSessionContext.CurrentContact.ContactID);
                if (contactCompanyID == null) continue;

                DOEmployeeInfo ei = CurrentBRContact.CreateEmployeeInfo(contactCompanyID, CurrentSessionContext.Owner.ContactID);
                ei.Address1 = string.Empty;
                ei.Address2 = string.Empty;
                ei.Email = c.Email;
                ei.FirstName = c.FirstName;
                ei.LastName = c.LastName;
                ei.Phone = string.Empty;
                CurrentBRContact.SaveEmployeeInfo(ei);
            }
        }


        protected void LoadTimeDropdowns()
        {
            ddlStartTime.Items.Clear();
            ddlEndTime.Items.Clear();

            ddlStartTime.Items.Add(new ListItem("Not Selected", "-1"));
            ddlEndTime.Items.Add(new ListItem("Not Selected", "-1"));

            for (int min = 0; min < 24 * 60; min += 15)
            {
                string MinText = string.Format("{0}:{1:D2}", min / 60, min % 60);
                ddlStartTime.Items.Add(new ListItem(MinText, min.ToString()));
                ddlEndTime.Items.Add(new ListItem(MinText, min.ToString()));
            }

            if (IsPostBack)
            {
                ddlStartTime.SelectedValue = Request.Form[ddlStartTime.UniqueID];
                ddlEndTime.SelectedValue = Request.Form[ddlEndTime.UniqueID];
            }


        }

        Guid? FindNewContractorID = null;
        DOContact NewContractor = null;
        bool NewContractorAdded = false;
        string PendingContractorEmail = null;

        protected void LoadContractors()
        {
            List<DOBase> Contractors = CurrentBRContact.SelectSubscribedContacts();

            //Get specific unsubscribed contractor details if specified.
            if (FindNewContractorID.HasValue && NewContractor == null)
            {
                if (FindNewContractorID == Constants.Guid_DefaultUser)
                {
                    NewContractor = CurrentBRContact.GetDefaultUser(PendingContractorEmail);
                }
                else
                {
                    NewContractor = CurrentBRContact.SelectContact(FindNewContractorID.Value);
                }
            }

            //If the current user is in the list of contractors, remove them.
            for (int i = Contractors.Count - 1; i > 0; i--)
            {
                if ((Contractors[i] as DOContact).ContactID == CurrentSessionContext.CurrentContact.ContactID)
                {
                    Contractors.RemoveAt(i);
                    break;
                }
            }

            Contractors.Insert(0, CurrentSessionContext.CurrentContact);
            //If specific contractor requested, put at top of list.
            if (NewContractor != null)
                Contractors.Insert(0, NewContractor);

            ddlContractor.DataSource = Contractors;
            ddlContractor.DataValueField = "ContactID";
            ddlContractor.DataTextField = "DisplayName";
            ddlContractor.DataBind();

            if (IsPostBack)
            {
                try
                {
                    if (NewContractorAdded)
                    {
                        ddlContractor.SelectedValue = NewContractor.ContactID.ToString();
                    }
                    else
                    {
                        ddlContractor.SelectedValue = GetDDLGuid(ddlContractor).ToString();
                    }
                }
                catch { }
            }

            //If subscribed user, show add new contractor section.
            if (CurrentSessionContext.CurrentContact.Subscribed || CurrentSessionContext.CurrentContact.SubscriptionPending)
            {
                phFindNew.Visible = true;
            }
            else
            {
                phFindNew.Visible = false;
            }
        }

        protected void btnAddContractor_Click(object sender, EventArgs e)
        {
            try
            {
            if (Save())
            {
                Guid ContractorID = new Guid(Request.Form[ddlContractors.UniqueID]);

                DOJobContractor JobContractor = CurrentBRJob.CreateJobContractor(Job.JobID, ContractorID, CurrentSessionContext.Owner.ContactID);
                CurrentBRJob.SaveJobContractor(JobContractor);
            }
        }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnRemoveContractor_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null && b.CommandName == "RemoveContractor")
            {
                Guid ContractorID = new Guid(b.CommandArgument.ToString());
                DOJobContractor JobContractor = CurrentBRJob.SelectJobContractor(Job.JobID, ContractorID);
                if (JobContractor != null)
                    CurrentBRJob.DeleteJobContractor(JobContractor);
            }
        }

        /*
        Change done by Mandeep
        To redirect screen to Job Summary page after adding New Job
    */
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (FieldValidationException ex)
            {
                bool NewJob = (Job.PersistenceStatus == ObjectPersistenceStatus.New);
                if (NewJob)
                    CurrentBRJob.DeleteJob(Job);
                ShowMessage(ex.Message, MessageType.Error);
                // return false;
                if (ex.Message == "")
                    Response.Redirect(Constants.URL_JobSummary);
            }


        }

        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_TaskDetails);
        }
        protected void btnDone_Click(object sender, EventArgs e)
        {
            //TODO: something with amended tasks
            CurrentSessionContext.CurrentJob = null;
            Response.Redirect(Constants.URL_SiteHome);
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                HttpFileCollection uploadedFiles = Request.Files;
                for(int i=0;i<uploadedFiles.Count;i++)
                {
                    HttpPostedFile file = uploadedFiles[i];
                    if (file.ContentLength< 10000000)
                    {
                        //DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, fileNew.PostedFile);
                        DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, file, file.ContentLength, CurrentSessionContext.Owner.ContactID);
                        DOJobFile jf = CurrentBRJob.CreateJobFile(CurrentSessionContext.Owner.ContactID, Job.JobID, File.FileID);
                        CurrentBRJob.SaveJobFile(jf);
                    }
                    else
                    {
                        ShowMessage(file.FileName+" - Upload Size exceeded");
                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        //protected void btnSubmitQuote_Click(object sender, EventArgs e)
        //{
        //    DOJobQuote Quote = CurrentBRJob.CreateJobQuote(CurrentSessionContext.Owner.ContactID, Job.JobID);
        //    Quote.ContactID = CurrentSessionContext.CurrentContact.ContactID;
        //    Quote.QuoteAccepted = false;
        //    Quote.QuoteStatus = DOJobQuote.JobQuoteStatus.Quoted;
        //    CurrentBRJob.SaveQuote(Quote);
        //}

        protected void btnUncompleteJob_Click(object sender, EventArgs e)
        {
            Job.CompletedBy = Guid.Empty;
            Job.CompletedDate = DateAndTime.NoValueDate;

            Job.JobStatus = DOJob.JobStatusEnum.Incomplete;
            CurrentBRJob.SaveJob(Job);

            LogChange(DOJobChange.JobChangeType.JobUncompleted);
            phCompleteJob.Visible = false;
        }

        protected void btnCompleteJob_Click(object sender, EventArgs e)
        {
            try
            {

            
            if (!Save())
                return;
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message);
              
            }
            //Check if all tasks are complete.
            List<DOBase> JobTasks = CurrentBRJob.SelectTasks(Job.JobID, Guid.Empty);
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
                Job.CompletedBy = CurrentSessionContext.Owner.ContactID;
                Job.CompletedDate = DateAndTime.GetCurrentDateTime();

                CurrentBRJob.SaveJob(Job);
                LogChange(DOJobChange.JobChangeType.JobCompleted);
                ShowIncomplete = false;
            }
            else
            {
                ShowIncomplete = true;
            }
        }

        protected void btnIncompleteSubmit_Click(object sender, EventArgs e)
        {
            try
            {
            if (!Save())
                return;
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, MessageType.Error);
                return;
            }
            ShowIncomplete = true;
            //Make sure a reason was entered.
            if (string.IsNullOrEmpty(txtIncompleteReason.Text))
            {
                ShowMessage("You must enter a reason for incomplete tasks.", MessageType.Error);
            }
            else
            {
                Job.IncompleTasksReason = txtIncompleteReason.Text;
                Job.JobStatus = DOJob.JobStatusEnum.Complete;
                Job.CompletedBy = CurrentSessionContext.Owner.ContactID;
                Job.CompletedDate = DateAndTime.GetCurrentDateTime();

                CurrentBRJob.SaveJob(Job);
                LogChange(DOJobChange.JobChangeType.JobCompleted);

                ShowIncomplete = false;
            }
        }

        protected void btnIncompleteCancel_Click(object sender, EventArgs e)
        {
            ShowIncomplete = false;
        }

        public void LogChange(DOJobChange.JobChangeType ChangeType)
        {
            DOJobChange Change = CurrentBRJob.CreateJobChange(Job.JobID, ChangeType, CurrentSessionContext.Owner.ContactID);
            CurrentBRJob.SaveJobChange(Change);
        }

        protected void btnFindNewContractor_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFindNewContractor.Text))
            {
                string ContractorEmail = txtFindNewContractor.Text.Trim();
                if (!ContractorEmail.Contains("@") || !ContractorEmail.Contains("."))
                {
                    ShowMessage("Please enter a valid email address.", MessageType.Error);
                    return;
                }

                DOContact Contractor = CurrentBRContact.SelectContactByUsername(txtFindNewContractor.Text.Trim());
                if (Contractor == null)
                {
                    try
                    {
                        //Send email to contractor asking them to join.
                        string Subject = CurrentSessionContext.CurrentContact.DisplayName + " has requested you to join OnTrack";
                        string Body = CurrentSessionContext.CurrentContact.DisplayName + " has requested you to join OnTrack.<br />";
                        Body += string.Format("Click <a href=\"{0}\">here</a> to register now", CurrentBRGeneral.SelectWebsiteBasePath() + "/RegisterIndividual.aspx?email=" + ContractorEmail);
                        if (Constants.EMAIL__TESTMODE)
                        {
                            Body += "<br/><br/>";
                            Body += "Sender: " + CurrentSessionContext.CurrentContact.DisplayName + "<br />";
                            Body += "Sender logged in as: " + CurrentSessionContext.Owner.DisplayName + " (" + CurrentSessionContext.Owner.UserName + ")<br />";
                            Body += "Recipient: " + ContractorEmail + "<br />";
                            //Body += "Job ID: " + Job.JobNumberAuto;

                        }

                        CurrentBRGeneral.SendEmail("no-reply@ontrack.co.nz", ContractorEmail, Subject, Body);

                        //Add default user as a placeholder contractor.
                        FindNewContractorID = Constants.Guid_DefaultUser;
                        PendingContractorEmail = ContractorEmail;
                        NewContractor = CurrentBRContact.GetDefaultUser(PendingContractorEmail + " (pending)");
                        NewContractorAdded = true;

                        ShowMessage(ContractorEmail + " can now be selected as the contractor for this job as a pending contractor. An email has been sent requesting them to register.");
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, MessageType.Error);
                    }
                }
                else
                {
                    //Set new contractor ID value so that it will appear in the contractor list.
                    FindNewContractorID = Contractor.ContactID;
                    NewContractor = Contractor;
                    NewContractorAdded = true;

                    ShowMessage(string.Format("{0} ({1}) can now be selected as the contractor for this job.", NewContractor.DisplayName, NewContractor.UserName), MessageType.Info);
                }

            }
        }

        protected void txtJobName_TextChanged(object sender, EventArgs e)
        {
            FirstTaskName_Txt.Text = txtJobName.Text;
        }

        protected void TaskDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TaskDesc_RadBtn.SelectedValue == "Same")
            {
                TextBox_TaskDescription.Visible = false;
            }
            else if (TaskDesc_RadBtn.SelectedValue == "Different")
            {
                TextBox_TaskDescription.Visible = true;
            }
        }
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbs();
        }

        //protected void SuburbDD_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //CurrentBRTradeCategory.SelectTradeCategories();
        //    List<DOBase> tradeCategories = CurrentBRTradeCategory.SelectTradeCategoriesWithSuburb(SuburbDD.SelectedValue);
        //    DropDownList_TradeCategory.DataSource = tradeCategories;
        //    DropDownList_TradeCategory.DataTextField = "TradeCategoryName";
        //    DropDownList_TradeCategory.DataValueField = "TradeCategoryID";
        //    DropDownList_TradeCategory.DataBind();

        //}

        protected void Contractor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBL_Contractor.SelectedValue == "Another")
            {
                Pnl_TradeCatgory.Visible = true;
            }
            else if (RBL_Contractor.SelectedValue == "OurStaff")
            {
                Pnl_TradeCatgory.Visible = false;
            }
        }
        protected void LoadSuburbs()
        {
            Guid regionCode;
            regionCode = Guid.Parse(RegionDD.SelectedValue);
            List<DOBase> Suburbs = CurrentBRSuburb.SelectSuburbsSorted(regionCode);
            Suburb_CBList.DataSource = Suburbs;
            Suburb_CBList.DataTextField = "SuburbName";
            Suburb_CBList.DataValueField = "SuburbID";
            Suburb_CBList.DataBind();

        }
        protected void TradeCategories_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DOBase> ContractorsList = new List<DOBase>();
            //IEnumerable<DOBase> list;
            List<DOBase> ContractorsforSubscribedUsers = new List<DOBase>();
            //If subscribed user, show all contractors
            if (CurrentSessionContext.CurrentContact.Subscribed || CurrentSessionContext.CurrentContact.SubscriptionPending)
            {
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    if (item.Selected)
                    {
                        try
                        {
                            DOOperatingSites os = new DOOperatingSites();
                            os.ContactID = CurrentSessionContext.Owner.ContactID;
                            //  os.OSID = Guid.NewGuid();
                            os.SuburbID = Guid.Parse(item.Value);
                            ContractorsforSubscribedUsers.AddRange(
                                CurrentBRContactTradecategory.SelectContractors(
                                    os.SuburbID, Guid.Parse(
                                        TradeCategories_List.SelectedValue)
                                        ));
                            ContractorsList = ContractorsforSubscribedUsers;
                        }

                        finally
                        {

                        }
                    }

                }

                //ContractorsList = ContractorsforSubscribedUsers;

            }
            else
            {
                List<DOBase> ContractorsforUnSubscribedUsers = new List<DOBase>();
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    if (item.Selected)
                    {
                        try
                        {

                            DOOperatingSites os = new DOOperatingSites();
                            os.ContactID = CurrentSessionContext.Owner.ContactID;
                            //os.OSID = Guid.NewGuid();
                            os.SuburbID = Guid.Parse(item.Value);
                            ContractorsforUnSubscribedUsers = CurrentBRContactTradecategory.SelectContractors(os.SuburbID, Guid.Parse(TradeCategories_List.SelectedValue));
                            // foreach (var i in ContractorsforUnSubscribedUsers)
                            for (int i = ContractorsforUnSubscribedUsers.Count - 1; i >= 0; i--)
                            {
                                var contact = ContractorsforUnSubscribedUsers[i] as DOContact;
                                if (!contact.Subscribed)
                                {
                                    ContractorsforUnSubscribedUsers.RemoveAt(i);
                                }
                            }

                        }

                        finally
                        {

                        }
                    }

                }
                ContractorsList = ContractorsforUnSubscribedUsers;
                //  list = ContractorsforUnSubscribedUsers.Distinct();
            }



            ContractorsList.Insert(0, CurrentSessionContext.CurrentContact);
            // Remove duplicates.
            for (var i = 0; i < ContractorsList.Count; i++)
            {

                var searchItem = ContractorsList[i] as DOContact;
                for (var j = i + 1; j < ContractorsList.Count; j++)
                {

                    var testItem = ContractorsList[j] as DOContact;
                    //compares id of starting with next immediate item, and then decreases j because it next 
                    //item moves up and index remains same otherwise compares to next item by increasing j
                    if (testItem.ID == searchItem.ID)
                    {
                        ContractorsList.RemoveAt(j);
                        j--;
                    }
                }
            }

            ddlContractor.DataSource = ContractorsList;
            // ddlContractor.DataSource = list;
            ddlContractor.DataValueField = "ContactID";
            ddlContractor.DataTextField = "DisplayName";
            ddlContractor.DataBind();

        }

        //protected void MoreDetailsBtn_Click(object sender, EventArgs e)
        //{
        //    if (MoreDetailsBtn.Text == "Hide details")
        //    {
        //        MoreDetailsBtn.Text = "View more details";
        //        JobMoreDetail.Visible = false;
        //    }
        //    else if (MoreDetailsBtn.Text == "View more details")
        //    {
        //        MoreDetailsBtn.Text = "Hide details";
        //        JobMoreDetail.Visible = true;
        //    }
        //}
    }
}