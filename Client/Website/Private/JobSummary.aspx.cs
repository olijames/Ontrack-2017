using System;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class JobSummary : PageBase
    {
        new DOSite Site;
        //jared 19/1/17
        //protected DOJob Job; previous code
        public DOJob Job;
        public DOJobContractor dojc;
        //jared 19/1/17
        List<DOBase> Tasks;
        public DOContact Contact { get; set; }

        //Tony Added 11.11.2016
        private Guid oldJobID;
        private Guid newJobID;
        private Guid taskID;
        public const string NO_ITEM_LEFT_MESSAGE = "There is no task left to move";
        protected DOEmployeeInfo Employee;
        //Tony Added 11.11.2016
        //jared 21.2.2017 start of block
        private Guid NewJobContactID;
        private Guid toContactID;
        private string selectedContact = "";
        public const string SHARE_SITE_MESSAGE = "You shared this job with ";
        //jared end of block

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
                TaskList.Job = Job;
                TaskList.TaskList = Tasks;

            }


        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //Tony added 16.Feb.2017
           
            CheckJobQuerystring();

            //Must have a site selected.
            if (CurrentSessionContext.CurrentSite == null)
                Response.Redirect(Constants.URL_Home);

            //If no job, go to job details screen instead to create job.
            if (CurrentSessionContext.CurrentJob == null)
            {
                Response.Redirect(Constants.URL_JobDetails, true);
            }

            if (CurrentSessionContext.CurrentJob != null)
            {
                //Reload job from database (in case other users have changed job)
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentJob.JobID);
            }
            Site = CurrentSessionContext.CurrentSite;
            Job = CurrentSessionContext.CurrentJob;

            ClearTask();
            //ClearTimeSheets();

            if (Job == null)
            {
                Job = CurrentBRJob.CreateJob(Site.SiteID, CurrentSessionContext.Owner.ContactID);
            }

            if (Job == null || Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                btnAddTask.Enabled = false;
                QuoteStatus.Visible = false;
            }
            else
            {
                QuoteStatus.Job = Job;
            }
            CurrentSessionContext.CurrentEmployee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            dojc = CurrentBRJob.SelectJobContractor(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.CurrentContact.ContactID);

            GetTasks();
            // btnContact.Text = CurrentSessionContext.Owner.DisplayName;
            //DOJobContractor dojc = CurrentBRJob.SelectJobContractor(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.CurrentContact.ContactID);
            lTitle.Text = dojc.JobNumberAuto + " " + CurrentSessionContext.CurrentJob.Name;

            //Tony Added 11.11.2016

            //Populate task drop down list
            populateTaskDD();

            // Populate job drop down list
            List<DOBase> Jobs = CurrentBRJob.SelectViewableJobs(CurrentSessionContext.CurrentSite.SiteID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.Owner.ContactID);
            JobDD.DataSource = Jobs;
            JobDD.DataTextField = "DisplayName";
            JobDD.DataValueField = "JobId";
            JobDD.DataBind();
            //Tony Added 11.11.2016
            btnMoveAll.Attributes.Add("onclick", "javascript:return confirm('Do you really want to move all data?');");

            //jared 21.2.17 start of block
            if (!IsPostBack)
            {
                LoadContact();
            }
            //jared end of block
        }

        // Tony Added 15.11.2016
        private void populateTaskDD()
        {
            // Populate task drop down list
            List<DOBase> tasks = CurrentBRJob.SelectTasks(CurrentSessionContext.CurrentJob.JobID);

            TaskDD.DataSource = tasks;
            TaskDD.DataTextField = "TaskName";
            TaskDD.DataValueField = "TaskId";
            TaskDD.DataBind();
        }

        private void CheckJobQuerystring()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["jobid"]))
            {
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(new Guid(Request.QueryString["jobid"]));
                CurrentSessionContext.CurrentSite = CurrentBRSite.SelectSite(CurrentSessionContext.CurrentJob.SiteID);
                //CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentJob.JobOwner);

                //Must be job owner or customer to access via link.
                bool HasPermission = false;
                Guid ContactID = CurrentSessionContext.Owner.ContactID;
                if (CurrentSessionContext.CurrentJob.JobOwner == ContactID || CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentJob.JobOwner, ContactID))
                {
                    HasPermission = true;
                }
                else
                {
                    if (CurrentSessionContext.Owner.ContactID == CurrentSessionContext.CurrentSite.ContactID ||
                        CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentSite.ContactID, CurrentSessionContext.Owner.ContactID))
                    {
                        HasPermission = true;
                    }

                }
                if (!HasPermission)
                {
                    //If the contact or their company is a contractor on a task of the job, they have access.
                    List<DOBase> companies = CurrentBRContact.SelectContactCompanies(CurrentSessionContext.Owner.ContactID);
                    List<DOBase> tasks = CurrentBRJob.SelectTasks(CurrentSessionContext.CurrentJob.JobID);
                    foreach (DOTask task in tasks)
                    {
                        if (task.ContractorID == CurrentSessionContext.Owner.ContactID)
                        {
                            HasPermission = true;
                            break;
                        }
                        else
                        {
                            foreach (DOContact company in companies)
                            {
                                if (task.ContractorID == company.ContactID)
                                {
                                    HasPermission = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (HasPermission)
                    Response.Redirect(Constants.URL_JobSummary);
                else
                    Response.Redirect(Constants.URL_Home);

                Response.End();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Tony added on 16.Feb.2017 to avoid null exception error
        protected void PermissionVisible()
        {
            long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag
                                                   // Reinstate the mask and re-test

            //Check if user has a permission to share site with another customer
            CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
            if ((myFlags & CompanyPageFlag.MoveTaskToAnotherJob) == CompanyPageFlag.MoveTaskToAnotherJob)
            {
                phMoveTask.Visible = true; // Display the control of share site function
            }
            else
            {
                phMoveTask.Visible = false;// Hide the control of share site function
            }
            //Tony added end
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Tony added start
            //Tony modified 16.Feb.2017
            //            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            Employee = CurrentSessionContext.CurrentEmployee;

            if(Employee != null)
                PermissionVisible();

            LoadForm();

            if (Job.JobStatus == DOJob.JobStatusEnum.Complete)
            {
                ShowCompletedInfo();
            }


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
            //Tony Testing 11.11.2016
            //            if (!IsPostBack)
            //            {
            //
            //                DOTask Reference = GetReferenceTask();
            //                if (Reference != null)
            //                {
            //                    litJobDescription.Text = Reference.Description;
            //                }
            //
            //            }
            DOContact JobOwner = CurrentBRContact.SelectContact(Job.JobOwner);

            
    
                //string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                //UriBuilder uri = new UriBuilder(codeBase);
                //string path = Uri.UnescapeDataString(uri.Path);
                //string path2 = Path.GetDirectoryName(path);
            


            String rootPath = Server.MapPath("~");
            //jared 19/1/17
            //info.Text = rootPath;
            //jared 19/1/17
            litJobOwner.Text = JobOwner.DisplayName;
            BindProjectManagerDropDown();

            List<DOBase> jobContractors = CurrentBRContact.SelectJobContractorContacts(Job.JobID);
            if (rpJobContractors != null)
            {
                rpJobContractors.DataSource = jobContractors;
                rpJobContractors.DataBind();
            }

            FileDisplayer1.FileList = CurrentBRJob.SelectFilesForJob(Job.JobID);
            pnlNoContractors.Visible = jobContractors.Count == 0;

        }

        private string FromForm(string Key, string Default)
        {
            if (Request.Form[Key] == null)
                return Default;
            else
                return Request.Form[Key];
        }


        private void BindProjectManagerDropDown()
        {

            string Name, Phone;
            Guid ContactID;

            if (Job.ProjectManagerID == Constants.Guid_DefaultUser)
            {
                Name = Job.ProjectMangerText;
                Phone = Job.ProjectManagerPhone;
            }
            else
            {
                DOContact JobOwner = CurrentBRContact.SelectContact(Job.JobOwner);
                if (JobOwner.ContactType == DOContact.ContactTypeEnum.Individual)
                {
                    Name = JobOwner.DisplayName;
                    Phone = JobOwner.Phone;
                    ContactID = JobOwner.ContactID;
                }
                else
                {
                    DOContact contactPM = CurrentBRContact.SelectContact(Job.ProjectManagerID);
                    if (contactPM == null)
                    {
                        Name = "Unknown";
                        Phone = string.Empty;
                    }
                    else
                    {
                        Name = contactPM.DisplayName;
                        Phone = contactPM.Phone;
                    }
                    ContactID = Job.ProjectManagerID;

                }
            }

            litProjectManagerName.Text = Name;
            litProjectManagerPhone.Text = Phone;
        }

        protected void btnAddTask_Click(object sender, EventArgs e)
        {

            Response.Redirect(Constants.URL_TaskDetails);
        }
        protected void btnDone_Click(object sender, EventArgs e)
        {
            //TODO: something with amended tasks
            CurrentSessionContext.CurrentJob = null;
            //To retrive jobs only on basis of jobcontractor table
            //CurrentSessionContext.CurrentContact = null;
            Response.Redirect(Constants.URL_SiteHome);
        }


        protected void btnJobDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_JobDetails);
        }


        protected string GetAccessTypeString()
        {
            switch (Job.AccessType)
            {
                case DOJob.JobAccessType.Key: return "Key";
                case DOJob.JobAccessType.LockBox: return "Lockbox";
                case DOJob.JobAccessType.PhoneFirst: return "Phone first";
                default: return "Other";
            }
        }

        protected string GetJobTypeString()
        {
            switch (Job.JobType)
            {
                case DOJob.JobTypeEnum.ChargeUp: return "Charge Up";
                case DOJob.JobTypeEnum.Quoted: return "Quoted";
                case DOJob.JobTypeEnum.ToQuote: return "To Quote";
                default: throw new Exception("Invalid job type");
            }
        }
        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                HttpFileCollection uploadedFiles = Request.Files;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile file = uploadedFiles[i];
                    if (file.ContentLength < 30000000) //todo !here! set this in settings possibly per customer
                    {
                        //DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, fileNew.PostedFile);

                        DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, file, file.ContentLength, CurrentSessionContext.CurrentContact.ContactID);
                        DOJobFile jf = CurrentBRJob.CreateJobFile(CurrentSessionContext.Owner.ContactID, Job.JobID, File.FileID);
                        CurrentBRJob.SaveJobFile(jf);
                    }
                    else
                    {
                        ShowMessage(file.FileName + " - Upload Size exceeded");
                    }
                }

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        //Tony added 11.11.2016
        private bool getSelectedItem()
        {
            if (TaskDD.Items.Count == 0)
            {
                return false;
            }
            else
            {
                oldJobID = CurrentSessionContext.CurrentJob.JobID;
                newJobID = new Guid(JobDD.SelectedItem.Value);
                taskID = new Guid(TaskDD.SelectedItem.Value);
                return true;

            }
        }

        protected void AddAlertOnMoveAll()
        {
            
        }

        protected void btnMoveOne_Click(object sender, EventArgs e)
        {
            if (getSelectedItem())
            {
                CurrentBRJob.MoveTask(taskID, oldJobID, newJobID);

                //Refresh List of Tasks
                lblMoveTask.Text = TaskDD.SelectedItem.Text + " => " + JobDD.SelectedItem.Text;
                populateTaskDD();
            }
            else
            {
                lblMoveTask.BackColor = Color.Coral;
                lblMoveTask.Text = NO_ITEM_LEFT_MESSAGE;
            }
        }

        protected void btnMoveAll_Click(object sender, EventArgs e)
        {
            if (getSelectedItem())
            {
                CurrentBRJob.MoveAllTask(oldJobID, newJobID);

                lblMoveTask.Text = "You moved all tasks to " + JobDD.SelectedItem.Text;
                populateTaskDD();
            }
            else
            {
                lblMoveTask.BackColor = Color.Coral;
                lblMoveTask.Text = NO_ITEM_LEFT_MESSAGE;
            }
        }
        //Tony end of block 11.11.2016
        //Jared 21/2/2017 insert block
        protected void btnShare_Click(object sender, EventArgs e)
        {
            //????If confirm check box for share site information checked, call shareSite method
            DOJob myjob = CurrentSessionContext.CurrentJob;

            NewJobContactID = new Guid(ContactDD.SelectedItem.Value);
            shareJob(myjob.JobID, NewJobContactID);
            selectedContact = ContactDD.SelectedItem.Text;
            lblShareSite.Text = SHARE_SITE_MESSAGE + selectedContact;

            populateContact();
            LoadContact();
        }
        private void shareJob(Guid JobID, Guid NewJobContactID)
        {
            //if any value is selected from company list, do copy and paste current site info
            if (ContactDD.SelectedIndex >= 0)
            {
                DOJobContractor myJC =  CurrentBRJob.CreateJobContractor(JobID, NewJobContactID, CurrentSessionContext.Owner.ContactID);
                CurrentBRJob.SaveJobContractor(myJC);
                //add this contact to site and maybe customercontact
            }
        }
        private void populateContact()
        {
            List<DOBase> contact = CurrentBRContact.SelectJobContractorContacts(CurrentSessionContext.CurrentJob.JobID);

            if (rpJobContractors != null)
            {
                rpJobContractors.DataSource = contact;
                rpJobContractors.DataBind();
            }

            foreach (RepeaterItem ri in rpJobContractors.Items)
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
        protected void LoadContact()
        {
            List<DOBase> contact =
                CurrentBRContact.SiteExcludeCustomers(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentSite.SiteID);
            ContactDD.DataSource = contact;
            ContactDD.DataTextField = "DisplayName";
            ContactDD.DataValueField = "ContactID";
            ContactDD.DataBind();
        }
        //jared end of block
    }
}