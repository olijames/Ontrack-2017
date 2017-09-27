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
using System.Web.Services;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class TaskDetails : PageBase
    {
        DOTask Task;
        DOTaskQuote Quote = null;
        Guid? FindNewContractorID = null;
        static DOContact NewContractor = null;
        static bool NewContractorAdded = false;
        string PendingContractorEmail = null;

        bool NewTask = true;
        bool CanQuote = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if(!IsPostBack)
            { 
            NewContractorAdded = false;
            NewContractor = null;
            }
            CheckTaskQuerystring();
            if (CurrentSessionContext.CurrentJob == null)
            {
                Response.Redirect(Constants.URL_Home);
            }
            else
            {
                //Make sure job is up to date.
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentJob.JobID);
            }

            if (CurrentSessionContext.CurrentTask != null)
            {
                NewTask = false;
                //Make sure task is up to date.
                CurrentSessionContext.CurrentTask = CurrentBRJob.SelectTask(CurrentSessionContext.CurrentTask.TaskID);

                Task = CurrentSessionContext.CurrentTask;

                Quote = CurrentBRJob.SelectTaskQuoteByTask(Task.TaskID);
            }
            else
            {
                NewTask = true;
                if(CurrentSessionContext.CurrentTask==null)
                {
                    int taskNumber=  CurrentBRJob.GetTaskNumber(CurrentSessionContext.CurrentJob.JobID);             
                    Task = CurrentBRJob.CreateTask(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.Owner.ContactID);
                    Task.TaskNumber = taskNumber;
                }
                else
                    Task = CurrentSessionContext.CurrentTask;
            }
        }

       

        private void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidFindNewContractorID.Value))
            {
                FindNewContractorID = new Guid(hidFindNewContractorID.Value);
            }
            if (!string.IsNullOrEmpty(hidPendingContractorEmail.Value))
            {
                PendingContractorEmail = hidPendingContractorEmail.Value;
            }
            if (!IsPostBack)
            {
                //LoadRegions();
                //LoadDistrict();
                //LoadSuburbs();
                LoadTradeCategories();
                //LoadContractorsBasedOnTradeCatgory();
            }
        }
        public void LoadDistrict()
        {
            List<DOBase> Districts = CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
            //District_CBL.DataSource = Districts;
            //District_CBL.DataTextField = "DistrictName";
            //District_CBL.DataValueField = "DistrictID";
            //District_CBL.DataBind();
            District_DDL.DataSource = Districts;
            District_DDL.DataTextField = "DistrictName";
            District_DDL.DataValueField = "DistrictID";
            District_DDL.DataBind();

        }
        public void LoadRegions()
        {
            List<DOBase> Regions = CurrentBRRegion.SelectRegions();
            RegionDD.DataSource = Regions;
            RegionDD.DataTextField = "RegionName";
            RegionDD.DataValueField = "RegionID";
            RegionDD.DataBind();
        }
        private void CheckTaskQuerystring()
        {
            //Check for task querystring and make the active task.
            if (!string.IsNullOrEmpty(Request.QueryString["taskid"]))
            {
                CurrentSessionContext.CurrentTask = CurrentBRJob.SelectTask(new Guid(Request.QueryString["taskid"]));
                CurrentSessionContext.CurrentJob = CurrentBRJob.SelectJob(CurrentSessionContext.CurrentTask.JobID);
                CurrentSessionContext.CurrentSite = CurrentBRSite.SelectSite(CurrentSessionContext.CurrentJob.SiteID);
                CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentTask.ContractorID);

                //Must be the contractor, task owner, or job owner, or belong to the company of one of these.
                bool HasPermission = false;
                Guid ContactID = CurrentSessionContext.Owner.ContactID;
                if (CurrentSessionContext.CurrentJob.JobOwner == ContactID || CurrentSessionContext.CurrentTask.ContractorID == ContactID
                    || CurrentSessionContext.CurrentTask.TaskOwner == ContactID)
                {
                    HasPermission = true;
                }
                else
                {
                    if (CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentJob.JobOwner, ContactID))
                        HasPermission = true;
                    else if (CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentTask.TaskOwner, ContactID))
                        HasPermission = true;
                    else if (CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentTask.ContractorID, ContactID))
                        HasPermission = true;
                }

                if (HasPermission)
                    Response.Redirect("TaskDetails.aspx",false);
                else
                    Response.Redirect(Constants.URL_Home);

                Response.End();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //      LoadContractors();
            LoadTimeDropdowns();
            CheckTaskQuote();
           if (!IsPostBack)
            LoadCustomers();
            List<DOBase> Materials = CurrentBRJob.SelectTaskMaterials(Task.TaskID);
            gvMaterials.DataSource = Materials;
            gvMaterials.DataBind();

            List<DOBase> LabourItems = CurrentBRJob.SelectTaskLabour(Task.TaskID);
            gvLabour.DataSource = LabourItems;
            gvLabour.DataBind();

            if (!IsPostBack)
                LoadForm();
            LoadFormPostBack();

            phNew.Visible = NewTask;
            lnkSave.Visible = NewTask;
            phEdit.Visible = !NewTask;
            lnkAmendTask.Visible = !NewTask && Task.Status == DOTask.TaskStatusEnum.Incomplete;
            lnkAmendTask.Enabled = true;
            lnkDeleteTask.Enabled = Materials.Count == 0;
            lnkUncompleteTask.Visible = Task.Status == DOTask.TaskStatusEnum.Complete;
            //&& Task.TaskInvoiceStatus==0;

            lnkCompleteTask.Enabled = CurrentSessionContext.CurrentJob.JobType != DOJob.JobTypeEnum.ToQuote;

            lnkCompleteTask.Visible = (Task.TaskType == DOTask.TaskTypeEnum.Standard || Task.TaskType == DOTask.TaskTypeEnum.Reference) && (Task.Status == DOTask.TaskStatusEnum.Incomplete) && !NewTask;
            cbeCompleteJob.Enabled = CurrentSessionContext.CurrentJob.JobStatus == DOJob.JobStatusEnum.Complete;

            if (Task.Status == DOTask.TaskStatusEnum.Amended || Task.Status == DOTask.TaskStatusEnum.Complete)
            {
                lnkSave.Enabled = false;
                lnkAmendTask.Enabled = false;
                lnkDeleteTask.Enabled = false;
                lnkCompleteTask.Enabled = false;
                //btnAdd15.Enabled = false;
                //btnAdd60.Enabled = false;
                //btnAdd300.Enabled = false;
                btnAddLabour.Enabled = false;
                btnAddMaterial.Enabled = false;

            }

            BindInvoiceTo();

            DataBind();

            CheckLabourAndMaterialsVisible();

            CheckAddLabourAndMaterials();

            hidFindNewContractorID.Value = FindNewContractorID.HasValue ? FindNewContractorID.Value.ToString() : string.Empty;
            hidPendingContractorEmail.Value = PendingContractorEmail;
            //if (!IsPostBack)
            //{
            //    LoadTradeCategories();

            //    //txtTaskName.Text = CurrentSessionContext.CurrentJob.Name;
            //    //txtTaskDescription.Text = CurrentSessionContext.CurrentJob.Description;

            //}
            //if (NewTask)
            //{
            //    Contractor_RBL.SelectedValue = "Our Staff";
            //}
            if(!NewTask)
           {
                if (!IsPostBack)
                { 
                    if (CurrentSessionContext.CurrentTask.ContractorID!=CurrentSessionContext.CurrentContact.ContactID)
                {
                    DOContact contact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentTask.ContractorID);
                        if (contact.DisplayName == "Default User")
                        {
                            DOTaskPendingContractor pendContract = CurrentBRJob.SelectTaskPendingContractor(CurrentSessionContext.CurrentTask.TaskID);
                            if(pendContract!=null)
                            Contractor_RBL.Items.Insert(0, "Existing contractor (" + pendContract.ContractorEmail+ "- Pending)");
                        }
                        else
                    Contractor_RBL.Items.Insert(0, "Existing contractor (" + contact.DisplayName+")");
                    Contractor_RBL.SelectedIndex = 0;
                }
            else 
                {
                   
                    Contractor_RBL.SelectedValue = "Our Staff";
                }
                }
                //  DifferentContractor_Pnl.Visible = true;
                // if (Task.TradeCategoryID != null && Task.TradeCategoryID != Guid.Parse("00000000-0000-0000-0000-000000000000")) 
                // TradeCategories_ddl.SelectedValue = Task.TradeCategoryID.ToString();
                // foreach (var item in ddlContractor.Items)
                //    {
                //        var match = ddlContractor.Items.FindByValue(Task.ContractorID.ToString());
                //        if (match != null)
                //            match.Selected = true;

                //    }
                //    //ddlContractor.SelectedValue = Task.ContractorID.ToString();
                //    DataBind();
            }



        }

        private void LoadCustomers()
        {
            //2013/7/23 jared List<DOBase> jobContractors = CurrentBRContact.SelectJobContractorContacts(CurrentSessionContext.CurrentJob.JobID);
            List<DOBase> jobContractors = CurrentBRContact.SelectJobContractorContractorCustomer(CurrentSessionContext.CurrentJob.JobID);
            CustomerDDL.DataSource = jobContractors;
            CustomerDDL.DataTextField = "DisplayName";
            CustomerDDL.DataValueField = "ContactCustomerID";
            CustomerDDL.DataBind();
            
        }

        //gets the trade categories
        private void LoadTradeCategories()
        {
            List<DOBase> tradeCategories = new List<DOBase>();
            tradeCategories = CurrentBRTradeCategory.SelectTradeCategories();
            // List<DOBase> uniqueTradeCatgeories = tradeCategories.Distinct().ToList();
            TradeCategoryAll.DataSource = tradeCategories;
            TradeCategoryAll.DataTextField = "TradeCategoryName";
            TradeCategoryAll.DataValueField = "TradeCategoryID";
            TradeCategoryAll.DataBind();
        }

        private string FindCompanyTradeCat()
        {
            if (NewTask)
            {
                List<DOBase> existingTradecategories = CurrentBRContactTradecategory.SelectCTC(CurrentSessionContext.CurrentContact.ContactID);
                List<DOBase> defTradeCatList = new List<DOBase>();
                foreach (var st in existingTradecategories)
                {
                    DOContactTradeCategory subtrad = st as DOContactTradeCategory;
                    defTradeCatList = CurrentBRTradeCategory.FindTradeCategoryNamebySubTrade(subtrad.SubTradeCategoryID);
                    break;
                }
                //DOSubTradeCategory defTradCat = existingTradecategories[0] as DOSubTradeCategory;
                foreach (var tradCat in defTradeCatList)
                {
                    DOTradeCategory tc = tradCat as DOTradeCategory;
                    return tc.TradeCategoryName;
                }
                //DOTradeCategory tradCat = defTradeCatList[1] as DOTradeCategory;
               
            }
            else 
            {
                if (CurrentSessionContext.CurrentTask.TradeCategoryID != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    DOTradeCategory tradCat = CurrentBRTradeCategory.FindTradeCategoryName(CurrentSessionContext.CurrentTask.TradeCategoryID);
                    return tradCat.TradeCategoryName;
                }
                else
                    return "";
            }
            return "";
        }

        private void BindInvoiceTo()
        {
            DOContactBase SiteCustomer = CurrentBRContact.SelectCustomerByContactID(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentSite.ContactID);
            if (SiteCustomer == null)
            {
                SiteCustomer = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentSite.ContactID);
            }
            DOContact JobOwner = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentJob.JobOwner);
            ddlInvoiceTo.Items.Clear();
            ddlInvoiceTo.Items.Add(new ListItem("Customer (" + SiteCustomer.DisplayName + ")", ((int)InvoiceRecipient.Customer).ToString()));

            string SiteOwnerDisplay = CurrentSessionContext.CurrentSite.OwnerFirstName + " " + CurrentSessionContext.CurrentSite.OwnerLastName;
            ddlInvoiceTo.Items.Add(new ListItem("Site Owner (" + SiteOwnerDisplay + ")", ((int)InvoiceRecipient.SiteOwner).ToString()));

            ddlInvoiceTo.Items.Add(new ListItem("Job Owner (" + JobOwner.DisplayName + ")", ((int)InvoiceRecipient.JobOwner).ToString()));

            //ddlInvoiceTo.Items.Clear();
            //string SiteOwnerName;
            //DOCustomer SiteOwnerContact = CurrentBRContact.SelectSiteCustomer(CurrentSessionContext.CurrentSite.SiteID, CurrentSessionContext.CurrentJob.JobOwner);

            //DOContact JobOwner = null;

            //if (SiteOwnerContact != null)
            //{
            //    SiteOwnerName = SiteOwnerContact.DisplayName;
            //}
            //else
            //{
            //    JobOwner = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentJob.JobOwner);
            //    SiteOwnerName = JobOwner.DisplayName;
            //}
            //ddlInvoiceTo.Items.Add(new ListItem("Site Owner (" + SiteOwnerName + ")", ((int)InvoiceRecipient.SiteOwner).ToString()));
            //if (Task.PersistenceStatus == ObjectPersistenceStatus.New)
            //{
            //    if (JobOwner == null || JobOwner.ContactID != CurrentSessionContext.CurrentContact.ContactID)
            //    {
            //        ddlInvoiceTo.Items.Add(new ListItem("Customer (" + CurrentSessionContext.CurrentContact.DisplayName + ")", ((int)InvoiceRecipient.Customer).ToString()));
            //    }
            //    else
            //    {
            //        Task.InvoiceToType = InvoiceRecipient.SiteOwner;
            //    }
            //}
            //else
            //{
            //    DOContact TaskOwnerContact = CurrentBRContact.SelectContact(Task.TaskOwner);
            //    if (JobOwner == null || JobOwner.ContactID != TaskOwnerContact.ContactID)
            //    {
            //        ddlInvoiceTo.Items.Add(new ListItem("Customer (" + TaskOwnerContact.DisplayName + ")", ((int)InvoiceRecipient.Customer).ToString()));
            //    }
            //    else
            //    {
            //        Task.InvoiceToType = InvoiceRecipient.SiteOwner;
            //    }
            //}
            ddlInvoiceTo.SelectedValue = ((int)Task.InvoiceToType).ToString();
        }

        private void CheckTaskQuote()
        {
            bool ToQuote = CurrentSessionContext.CurrentJob.JobType == DOJob.JobTypeEnum.ToQuote;
            phTaskQuote.Visible = (Quote != null);

            if (Quote != null)
            {
                if (Quote.Status != DOTaskQuote.TaskQuoteStatus.Quoted)
                {
                    btnDeleteQuote.Visible = false;
                    litQuoteStatus.Text = " The quote has been " + Quote.Status.ToString();
                }
            }

            if (Task.PersistenceStatus == ObjectPersistenceStatus.New)
            {
                CanQuote = false;
            }
            else
            {
                if (CurrentSessionContext.Owner.ContactID == Task.ContractorID ||
                    CurrentBRContact.CheckCompanyContact(Task.ContractorID, CurrentSessionContext.Owner.ContactID))
                    CanQuote = true;
            }

            if (CanQuote && ToQuote && Quote == null)
            {
                btnSubmitQuote.Visible = true;
                phSubmitQuote.Visible = true;
            }
            else
            {
                btnSubmitQuote.Visible = false;
                phSubmitQuote.Visible = false;
            }
        }

        private void CheckLabourAndMaterialsVisible()
        {
            //Not visible if task is new.
            if (Task.PersistenceStatus != ObjectPersistenceStatus.Existing)
            {
                pnlLabour.Visible = false;
                pnlMaterials.Visible = false;
                return;
            }
            //LAbour and materials are only visible if you are the task contractor or belong to their company.
            bool LandMVisible = false;
            if (Task.LMVisibility == DOTask.LMVisibilityEnum.All && Task.PersistenceStatus == ObjectPersistenceStatus.Existing)
            {
                LandMVisible = true;
            }
            phLMVisibility.Visible = false;

            if (CurrentSessionContext.Owner.ContactID == Task.ContractorID)
            {
                LandMVisible = true;
                phLMVisibility.Visible = true;
            }
            else if (CurrentBRContact.CheckCompanyContact(Task.ContractorID, CurrentSessionContext.Owner.ContactID))
            {
                LandMVisible = true;
                phLMVisibility.Visible = true;
            }

            pnlLabour.Visible = LandMVisible;
            pnlMaterials.Visible = LandMVisible;
        }

        private void CheckAddLabourAndMaterials()
        {
            ddlMaterialType.Items.Clear();
            ddlLabourType.Items.Clear();
            bool QuotedOnly = CurrentSessionContext.CurrentJob.JobType == DOJob.JobTypeEnum.ToQuote;
            if (Quote != null && Quote.Status == DOTaskQuote.TaskQuoteStatus.Accepted)
                QuotedOnly = false;


            if (QuotedOnly)
            {
                ddlMaterialType.Items.Add(new ListItem(TaskMaterialType.Quoted.ToString(), ((int)TaskMaterialType.Quoted).ToString()));
                ddlLabourType.Items.Add(new ListItem(TaskMaterialType.Quoted.ToString(), ((int)TaskMaterialType.Quoted).ToString()));
            }
            else
            {
                ddlMaterialType.Items.Add(new ListItem(TaskMaterialType.Actual.ToString(), ((int)TaskMaterialType.Actual).ToString()));
                ddlMaterialType.Items.Add(new ListItem(TaskMaterialType.Required.ToString(), ((int)TaskMaterialType.Required).ToString()));

                ddlLabourType.Items.Add(new ListItem(TaskMaterialType.Actual.ToString(), ((int)TaskMaterialType.Actual).ToString()));
                ddlLabourType.Items.Add(new ListItem(TaskMaterialType.Required.ToString(), ((int)TaskMaterialType.Required).ToString()));
            }

            ////If the job is to quote, you can only add materials/labour if you belong to the job owners company.
            //if (CurrentSessionContext.CurrentJob.JobType == DOJob.JobTypeEnum.ToQuote)
            //{
            //    bool AddLandM = false;
            //    if (CurrentSessionContext.CurrentJob.JobOwner == CurrentSessionContext.Owner.ContactID)
            //    {
            //        AddLandM = true;
            //    }
            //    else if (CurrentBRContact.CheckCompanyContact(CurrentSessionContext.CurrentJob.JobOwner, CurrentSessionContext.Owner.ContactID))
            //    {
            //        AddLandM = true;
            //    }
            //    phAddLabour.Visible = AddLandM;
            //    phAddMaterial.Visible = AddLandM;
            //}
        }

        protected void LoadForm()
        {
            dateStartDate.SetDate(Task.StartDate);
            dateEndDate.SetDate(Task.EndDate);

            litAmendedDate.Text = Task.CreatedDate.ToString("dd/MM/yyyy");

            txtTaskName.Text = Task.TaskName;
            txtTaskDescription.Text = Task.Description;

            if (CurrentSessionContext.CurrentJob.JobType == DOJob.JobTypeEnum.ChargeUp)
            {
                txtLabourRate.Text = CurrentSessionContext.CurrentContact.DefaultChargeUpRate.ToString();
            }
            else
            {
                txtLabourRate.Text = CurrentSessionContext.CurrentContact.DefaultQuoteRate.ToString();
            }
            chkAppointment.Checked = Task.Appointment;
            LoadTaskTypes();

        }
        protected void LoadFormPostBack()
        {
            LoadMaterials();
            LoadLabourers();

            ddlLMVisibility.SelectedValue = ((int)Task.LMVisibility).ToString();
        }

        private void LoadTaskTypes()
        {
            ddlTaskType.Items.Clear();
            ddlTaskType.Items.Add(new ListItem("Standard", ((int)DOTask.TaskTypeEnum.Standard).ToString()));
         //   ddlTaskType.Items.Add(new ListItem("Acknowledgement", ((int)DOTask.TaskTypeEnum.Acknowledgement).ToString()));

            //Cannot add new reference tasks. References tasks are only created when the job is created.
            if (Task.TaskType == DOTask.TaskTypeEnum.Reference)
            {
                ddlTaskType.Items.Add(new ListItem("Reference", ((int)DOTask.TaskTypeEnum.Reference).ToString()));
            }
            if (Task.PersistenceStatus == ObjectPersistenceStatus.Existing)
            {
                ddlTaskType.SelectedValue = ((int)Task.TaskType).ToString();
            }
            ddlTaskType.Enabled = NewTask;
        }

        private void LoadLabourers()
        {
            ddlLabourContactID.Items.Clear();
            ddlLabourContactID.Items.Add(new ListItem(CurrentSessionContext.Owner.DisplayName, CurrentSessionContext.Owner.ContactID.ToString()));

            if (!IsPostBack)
            {
                DateTime current = DateAndTime.GetCurrentDateTime();
                dateLabourDate.SetDate(new DateTime(current.Year, current.Month, current.Day));
            }
            DOContact TaskContact = CurrentBRContact.SelectContact(Task.ContractorID);
            if (TaskContact.ContactType == DOContact.ContactTypeEnum.Individual)
            {
                if (TaskContact.ContactID != CurrentSessionContext.CurrentContact.ContactID)
                {
                    ddlLabourContactID.Items.Add(new ListItem(TaskContact.DisplayName, TaskContact.ContactID.ToString()));
                }
            }
            else
            {
                List<DOBase> CompanyContacts = CurrentBRContact.SelectCompanyEmployees(TaskContact.ContactID);
                foreach (DOContactEmployee Contact in CompanyContacts)
                {
                    if (Contact.ContactID != CurrentSessionContext.Owner.ContactID)
                    {
                        ddlLabourContactID.Items.Add(new ListItem(Contact.DisplayName, Contact.ContactID.ToString()));
                    }
                    else
                    {
                        ListItem li = ddlLabourContactID.Items.FindByValue(Contact.ContactID.ToString());
                        if (li != null) li.Text = Contact.DisplayName;
                    }
                }
                ddlLabourContactID.Items.Add(new ListItem("New Employee", Constants.Guid_DefaultUser.ToString()));
            }

            if (IsPostBack)
            {
                if (Task.PersistenceStatus == ObjectPersistenceStatus.Existing)
                {
                    if (!string.IsNullOrEmpty(Request.Form[ddlLabourContactID.UniqueID]))
                    {
                        Guid LabourContactID = GetDDLGuid(ddlLabourContactID);
                        ddlLabourContactID.SelectedValue = LabourContactID.ToString();
                    }
                }
            }


        }
        protected void LoadTimeControl(DropDownList ddl, int SelectedTime)
        {
            ddl.Items.Clear();
            for (int time = 0; time < 60 * 24; time += 15)
            {
                ddl.Items.Add(new ListItem(string.Format("{0:D2}:{1:D2}", time / 60, time % 60), time.ToString()) { Selected = time == SelectedTime });
            }
            ddl.Items.Insert(0, new ListItem("Select...", "-1"));
        }

        private void LoadMaterials()
        {
            List<DOBase> Categories = CurrentBRJob.SelectMaterialCategories(Task.ContractorID);
            ddlMaterialCategory.Items.Clear();
            ddlMaterialCategory.Items.Add(new ListItem("Select Category...", Guid.Empty.ToString()));
            Guid SelectedCategory = Guid.Empty;
            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlMaterialCategory.UniqueID]))
                {
                    if (Task.PersistenceStatus == ObjectPersistenceStatus.Existing)
                    {
                        SelectedCategory = GetDDLGuid(ddlMaterialCategory);
                    }
                }
            }

            foreach (DOMaterialCategory Category in Categories)
            {
                ddlMaterialCategory.Items.Add(new ListItem(Category.CategoryName, Category.MaterialCategoryID.ToString()) { Selected = Category.MaterialCategoryID == SelectedCategory });
            }
            ddlMaterialCategory.Items.Add(new ListItem("New Material", Constants.Guid_DefaultUser.ToString()) { Selected = SelectedCategory == Constants.Guid_DefaultUser });

            //ddlMaterials.Items.Clear();
            //if (SelectedCategory != Guid.Empty)
            //{
            //    ddlMaterials.Items.Clear();
            //    foreach (DOMaterial Material in CurrentBRJob.SelectMaterials(SelectedCategory))
            //    {
            //        ddlMaterials.Items.Add(new ListItem(Material.MaterialName, Material.MaterialID.ToString()));
            //    }
            //}


            //if (SelectedCategory == Constants.Guid_DefaultUser)
            //{
            //Get data entered in new material form and reload the form (if visible).
            DOMaterial NewMaterial = CurrentBRJob.CreateMaterial(CurrentSessionContext.Owner.ContactID);
            MaterialForm.CategoryContactID = Task.ContractorID;
            //if (Request.Form[ddlMaterials.UniqueID] != null && Request.Form[ddlMaterials.UniqueID] == Guid.Empty.ToString())
            //    MaterialForm.SaveForm(NewMaterial);
            MaterialForm.LoadForm(NewMaterial);
            //}
            //else
            //{

            //}

            if (IsPostBack)
            {
                string selectedMaterialType = Request.Form[ddlMaterialType.UniqueID];
                if (!string.IsNullOrEmpty(selectedMaterialType))
                {
                    ListItem liSelected = ddlMaterialType.Items.FindByValue(selectedMaterialType);
                    if (liSelected != null) liSelected.Selected = true;
                }
                //string selectedMaterial = Request.Form["ddlMaterials"];
                //if (!string.IsNullOrEmpty(selectedMaterial))
                //{
                //    ListItem liMaterial = ddlMaterials.Items.FindByValue(selectedMaterial);
                //    if (liMaterial != null) liMaterial.Selected = true;
                //}

                txtQuantity.Text = Request.Form[txtQuantity.UniqueID];
                txtDescription.Text = Request.Form[txtDescription.UniqueID];
            }

            if (!IsPostBack)
            {
                txtQuantity.Text = "1";
            }
        }

        protected void LoadTimeDropdowns()
        {
            ddlStartTime.Items.Clear();
            ddlEndTime.Items.Clear();
            ddlLabourTime.Items.Clear();

            ddlStartTime.Items.Add(new ListItem("Not Selected", "-1"));
            ddlEndTime.Items.Add(new ListItem("Not Selected", "-1"));

            for (int min = 0; min < 24 * 60; min += 15)
            {
                string MinText = string.Format("{0}:{1:D2}", min / 60, min % 60);
                ddlStartTime.Items.Add(new ListItem(MinText, min.ToString()) { Selected = Task.StartMinute == min });
                ddlEndTime.Items.Add(new ListItem(MinText, min.ToString()) { Selected = Task.EndMinute == min });
                if (min > 0 && min < 10 * 60)
                    ddlLabourTime.Items.Add(new ListItem(MinText, min.ToString()));
            }
            if (CurrentSessionContext.CurrentJob.JobType == DOJob.JobTypeEnum.ToQuote)
            {
                for (int hour = 10; hour < 50; hour += 10)
                {
                    ddlLabourTime.Items.Add(new ListItem(hour.ToString() + " hours", (hour * 60).ToString()));
                }
                for (int hour = 50; hour <= 500; hour += 50)
                {
                    ddlLabourTime.Items.Add(new ListItem(hour.ToString() + " hours", (hour * 60).ToString()));
                }
            }

            if (IsPostBack)
            {
                ddlStartTime.SelectedValue = Request.Form[ddlStartTime.UniqueID];
                ddlEndTime.SelectedValue = Request.Form[ddlEndTime.UniqueID];
            }


        }

        protected void LoadContractors()
        {
            if (Task.PersistenceStatus == ObjectPersistenceStatus.New)
            {
                List<DOBase> Contractors = CurrentBRContact.SelectSubscribedContacts();
                List<DOBase> NewContractors = new List<DOBase>();


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

                if (NewContractor != null)
                {
                    NewContractors.Add(NewContractor);
                }

                // Add the 'find new contractor' contractor and any unsubscribed companies of theirs to the list.
                // Add companies with same email as contractor as well.
                if (NewContractor != null && NewContractor.ContactID != Constants.Guid_DefaultUser)
                {
                    List<Guid> AddedGuids = new List<Guid>();
                    AddedGuids.Add(NewContractor.ContactID);

                    //Companies linked to contractor
                    foreach (DOContact NewContractorCheck in CurrentBRContact.SelectContactCompanies(NewContractor.ContactID))
                    {
                        bool CanAdd = true;
                        if (NewContractorCheck.ContactID == NewContractor.ContactID) CanAdd = false;
                        if (NewContractorCheck.CustomerExclude) CanAdd = false;
                        if (NewContractorCheck.Subscribed || NewContractorCheck.SubscriptionPending) CanAdd = false;
                        if (AddedGuids.Contains(NewContractorCheck.ContactID)) CanAdd = false;

                        if (CanAdd)
                        {
                            NewContractorCheck.LastName += " (" + NewContractor.DisplayName + ")";
                            NewContractorCheck.CompanyName += " (" + NewContractor.DisplayName + ")";
                            NewContractors.Add(NewContractorCheck);
                            AddedGuids.Add(NewContractorCheck.ContactID);
                        }
                    }
                    //Companies with same email.
                    foreach (DOContact NewContractorCheck in CurrentBRContact.SelectContactsByEmail(NewContractor.Email))
                    {
                        bool CanAdd = true;
                        if (NewContractorCheck.ContactID == NewContractor.ContactID) CanAdd = false;
                        if (NewContractorCheck.CustomerExclude) CanAdd = false;
                        if (NewContractorCheck.Subscribed || NewContractorCheck.SubscriptionPending) CanAdd = false;
                        if (AddedGuids.Contains(NewContractorCheck.ContactID)) CanAdd = false;

                        if (CanAdd)
                        {
                            NewContractorCheck.LastName += " (" + NewContractor.Email + ")";
                            NewContractorCheck.CompanyName += " (" + NewContractor.Email + ")";
                            NewContractors.Add(NewContractorCheck);
                            AddedGuids.Add(NewContractorCheck.ContactID);
                        }
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

                //If specific contractor(s) requested by 'find new contractor', put at top of list.
                //if (NewContractor != null)
                //    Contractors.Insert(0, NewContractor);
                foreach (DOContact c in NewContractors)
                {
                    Contractors.Insert(0, c);
                }

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

            else
            {
                DOContact TaskContractor = CurrentBRContact.SelectContact(Task.ContractorID);
                ddlContractor.Items.Clear();
                ddlContractor.Items.Add(new ListItem(TaskContractor.DisplayName, TaskContractor.ContactID.ToString()));
                ddlContractor.Enabled = false;

                phFindNew.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();

                Response.Redirect(Constants.URL_JobSummary);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                ShowMessage(ex.Message);
            }
        }

        protected void Save()
        {
            DOTask SaveTask;

            if (NewTask)
            {
                SaveTask = Task;
              SaveTask.ContractorID=  GetTaskContractor();
            }
            else
            {
                //Create a new amended task, and update the old task to reflect that it is amended.
                SaveTask = CurrentBRJob.CreateTask(Task.JobID, CurrentSessionContext.Owner.ContactID);
                SaveTask.TaskNumber = Task.TaskNumber;
                if (Contractor_RBL.SelectedValue == "Our Staff")
                {
                    SaveTask.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                }
                else if (Contractor_RBL.SelectedValue == "Different")
                {
                    //if (ddlContractor.SelectedValue != null)
                    //    //else
                    //SaveTask.ContractorID = Task.ContractorID;

                    if (NewContractor != null)
                    {
                        SaveTask.ContractorID = NewContractor.ContactID;
                    }
                    else if (ContractorsDDl.SelectedValue == "-Select-")
                        throw new FieldValidationException("Please select contractor");
                    else if (ContractorsDDl.SelectedValue.ToString() != "")
                        SaveTask.ContractorID = Guid.Parse(ContractorsDDl.SelectedValue.ToString());
                    if (NewContractorAdded)
                    {

                    }
                }
                else
                    SaveTask.ContractorID = Task.ContractorID;
                if (SaveTask.ContractorID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    if (Contractor_RBL.SelectedValue == "Our Staff")
                    {
                        SaveTask.ContractorID = Task.ContractorID;
                        SaveTask.TradeCategoryID = Task.TradeCategoryID;
                    }
                    else if (Contractor_RBL.SelectedValue == "Different")
                    {
                        //if (ddlContractor.SelectedValue != null)
                        //    //else
                        //SaveTask.ContractorID = Task.ContractorID;
                        if (ContractorsDDl.SelectedValue.ToString() != "")
                            SaveTask.ContractorID = Guid.Parse(ContractorsDDl.SelectedValue.ToString());

                    }
                }
                if (TradeCategoryAll.SelectedValue.ToString() == "-Select-")
                    throw new FieldValidationException("Please select trade category");
                Task.Status = DOTask.TaskStatusEnum.Amended;
                Task.AmendedBy = CurrentSessionContext.Owner.ContactID;
                Task.AmendedDate = DateAndTime.GetCurrentDateTime();
                Task.AmendedTaskID = SaveTask.TaskID;
                CurrentBRJob.SaveTask(Task);
            }
            if (NewTask)
            {
                if (Task.ContractorID == Constants.Guid_DefaultUser && string.IsNullOrEmpty(PendingContractorEmail))
                {
                    throw new FieldValidationException("No email for pending contractor.");
                }
            }
            if (txtTaskName.Text == "")
                throw new FieldValidationException("Please enter task name");
            SaveTask.TaskName = txtTaskName.Text;
            SaveTask.Description = txtTaskDescription.Text;
            SaveTask.TaskOwner = GetTaskOwner();
            SaveTask.InvoiceToType = (InvoiceRecipient) int.Parse(Request.Form[ddlInvoiceTo.UniqueID]);
            SaveTask.Appointment = chkAppointment.Checked;
            if (TradeCategoryAll.SelectedValue != "All")
                SaveTask.TradeCategoryID = Guid.Parse(TradeCategoryAll.SelectedValue.ToString());
            //Guid.Parse(TradeCategories_ddl.SelectedValue.ToString());
            if (Request.Form[ddlTaskType.UniqueID] != null)
                SaveTask.TaskType = (DOTask.TaskTypeEnum) int.Parse(Request.Form[ddlTaskType.UniqueID]);

            SaveTask.StartDate = dateStartDate.GetDate();
            SaveTask.StartMinute = int.Parse(ddlStartTime.SelectedValue);
            SaveTask.EndDate = dateEndDate.GetDate();
            SaveTask.EndMinute = int.Parse(ddlEndTime.SelectedValue);
            SaveTask.TaskCustomerID = Guid.Parse(CustomerDDL.SelectedValue);
            SaveTask.SiteID = CurrentSessionContext.CurrentJob.SiteID;
            CurrentBRJob.SaveTask(SaveTask);
            if (!NewTask && Task.AmendedTaskID != Guid.Parse("00000000-0000-0000-0000-000000000000") &&
                Contractor_RBL.SelectedValue != "Our Staff" && Contractor_RBL.SelectedValue != "Different")
            {
                DOTaskPendingContractor tpc = CurrentBRJob.SelectTaskPendingContractor(Task.TaskID);
                if (tpc != null)
                {
                    tpc.TaskID = SaveTask.TaskID;
                    if (!string.IsNullOrEmpty(PendingContractorEmail))
                        tpc.ContractorEmail = PendingContractorEmail;
                    CurrentBRJob.SaveTaskPendingContractor(tpc);
                }

                else
                {
                    DOTaskPendingContractor TPC =
                        CurrentBRJob.CreateTaskPendingContractor(CurrentSessionContext.Owner.ContactID);
                    if (!string.IsNullOrEmpty(PendingContractorEmail))
                        TPC.ContractorEmail = PendingContractorEmail;
                    TPC.TaskID = SaveTask.TaskID;
                    CurrentBRJob.SaveTaskPendingContractor(TPC);
                }
            }
            //if (SaveTask.ContractorID!= Task.ContractorID)
            //{
            /*
           ******No need to delete the old contractor
                //delete the previously linked contractor
            List<DOBase> oldContractor = CurrentBRJob.FindContractorForAllTaskofJob(SaveTask.JobID, Task.ContractorID);
            if (oldContractor.Count<=1)
            {
                var oldJobContractor = CurrentBRJob.SelectJobContractor(SaveTask.JobID, Task.ContractorID);
                if (oldJobContractor!=null)
                {
                    CurrentBRJob.DeleteJobContractor(oldJobContractor);
                }
                
            } */

            //Update Job Contractor table for Task contractor ID
            var doJcTaskContractor = CurrentBRJob.SelectJobContractor(SaveTask.JobID, SaveTask.ContractorID);
            if (doJcTaskContractor == null)
            {
                doJcTaskContractor = CurrentBRJob.CreateJobContractor(SaveTask.JobID, SaveTask.ContractorID,
                    CurrentSessionContext.Owner.ContactID);
                doJcTaskContractor.Active = true;
                doJcTaskContractor.Status = 0;
                //doJcTaskContractor.Active = true;
                //doJcTaskContractor.Status = 0;
                CurrentBRJob.SaveJobContractor(doJcTaskContractor);

            }
            else
            {
                doJcTaskContractor.Active = true;
                doJcTaskContractor.Status = 0;
                CurrentBRJob.SaveJobContractor(doJcTaskContractor);

            }
            //update job contractor table for customer id
            DOContractorCustomer docc = CurrentBRContact.SelectContractorCustomerByCCID(SaveTask.TaskCustomerID);
            var doJcTaskCustomer = CurrentBRJob.SelectJobContractor(SaveTask.JobID, docc.CustomerID);
            if (doJcTaskCustomer == null)
            {
                doJcTaskCustomer = CurrentBRJob.CreateJobContractor(SaveTask.JobID, SaveTask.ContractorID,
                    CurrentSessionContext.Owner.ContactID);
                doJcTaskCustomer.Active = true;
                doJcTaskContractor.Status = 0;
                CurrentBRJob.SaveJobContractor(doJcTaskCustomer);

            }
            else
            {
                doJcTaskCustomer.Active = true;
                doJcTaskContractor.Status = 0;
                CurrentBRJob.SaveJobContractor(doJcTaskCustomer);

            }
            //}
            //Update the ContractorCustomer table
            DOContractorCustomer contractorCustomer = CurrentBRContact.SelectContractorCustomer(SaveTask.ContractorID,
                docc.CustomerID);
            if (contractorCustomer == null)
            {
                contractorCustomer = CurrentBRContact.CreateContactCustomer(SaveTask.ContractorID, SaveTask.TaskCustomerID, Guid.Empty); //added guid.empty 2017.4.25 needs testing
                contractorCustomer.Active = true;
                CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            }
            else
            {
                contractorCustomer.Active = true;
                CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            }

            if (CurrentSessionContext.CurrentContractee != null)
            {
                contractorCustomer = CurrentBRContact.SelectContactCustomer(SaveTask.TaskCustomerID,
                CurrentSessionContext.CurrentContractee.ContactID);

                if (contractorCustomer != null)
                {
                    contractorCustomer.Active = true;
                    CurrentBRContact.SaveContractorCustomer(contractorCustomer);
                }
                else
                {
                
                    if (SaveTask.TaskCustomerID != CurrentSessionContext.CurrentContractee.ContactID)
                        contractorCustomer = CurrentBRContact.CreateContactCustomer(SaveTask.TaskCustomerID,
                            CurrentSessionContext.CurrentContractee.ContactID, Guid.Empty); //added guid.empty 2017.4.25 needs testing
                    if (contractorCustomer != null)
                    {
                        contractorCustomer.Active = true;
                        CurrentBRContact.SaveContractorCustomer(contractorCustomer);
                    }
                }

            }
                //Add a record to contactsite table for the site added as a contractor
                DOContactSite contactSite = CurrentBRSite.SelectContactSite(CurrentSessionContext.CurrentJob.SiteID,
                    SaveTask.ContractorID);
                if (contactSite == null)
                {
                    contactSite = new DOContactSite
                    {
                        SiteID = CurrentSessionContext.CurrentJob.SiteID,
                        ContactID = SaveTask.ContractorID,
                        ContactSiteID = Guid.NewGuid()
                    };

                }
                else
                {
                    contactSite.Active = true;

                }
                CurrentBRSite.SaveContactSite(contactSite);

                //Add a record to contactsite table for the site added as a customer
                DOContactSite contactSiteCust = CurrentBRSite.SelectContactSite(CurrentSessionContext.CurrentJob.SiteID,
                    SaveTask.TaskCustomerID);
                if (contactSiteCust == null)
                {
                    contactSiteCust = new DOContactSite
                    {
                        SiteID = CurrentSessionContext.CurrentJob.SiteID,
                        ContactID = SaveTask.ContractorID,
                        ContactSiteID = Guid.NewGuid()
                    };

                }
                else
                {
                    contactSiteCust.Active = true;

                }
                CurrentBRSite.SaveContactSite(contactSiteCust);
                if (NewTask)
                {
                    if (Task.ContractorID == Constants.Guid_DefaultUser)
                    {
                        //Pending user. Add them to the task pendign contractor table so they can be linked when they register.
                        //Add entry to contractor pending list so the task can be updated when the contractor registers.
                        DOTaskPendingContractor TPC =
                            CurrentBRJob.CreateTaskPendingContractor(CurrentSessionContext.Owner.ContactID);
                        TPC.ContractorEmail = PendingContractorEmail;
                        TPC.TaskID = Task.TaskID;
                        CurrentBRJob.SaveTaskPendingContractor(TPC);

                    }
                    else
                    {
                        //Notify contractor that they have been added.
                        DOContact Contractor = CurrentBRContact.SelectContact(SaveTask.ContractorID);
                        string Body = "Job Name: " + CurrentSessionContext.CurrentJob.Name + "<br >Task Name: " +
                                      SaveTask.TaskName + "<br />" + SaveTask.Description;
                        Body += "<br /><a href=\"" + CurrentBRGeneral.SelectWebsiteBasePath() +
                                "/private/TaskDetails.aspx?taskid=" + SaveTask.TaskID.ToString() + "\">View Task</a>";
                        if (Constants.EMAIL__TESTMODE)
                        {
                            Body += "<br/><br/>";
                            Body += "Sender: " + CurrentSessionContext.CurrentContact.DisplayName + "<br />";
                            Body += "Sender logged in as: " + CurrentSessionContext.Owner.DisplayName + " (" +
                                    CurrentSessionContext.Owner.UserName + ")<br />";
                            Body += "Recipient: " + Contractor.DisplayName + " (" + Contractor.Email + ")<br />";
                            //Body += "Job ID: " + CurrentSessionContext.CurrentJob.JobNumberAuto;
                        }

                        CurrentBRGeneral.SendEmail("no-reply@ontrack.co.nz", Contractor.Email,
                            CurrentSessionContext.CurrentContact.DisplayName + " has assigned you a task on Ontrack",
                            Body);
                    }
                }

                if (!NewTask)
                {
                    CurrentBRJob.SaveTask(Task);
                    if (CurrentSessionContext.AmendedTaskIDs == null)
                        CurrentSessionContext.AmendedTaskIDs = new List<Guid>();
                    CurrentSessionContext.AmendedTaskIDs.Add(Task.TaskID);

                    //Update the JobContractor Table for Amended task
                    DOJobContractor doJCTaskContractorO = CurrentBRJob.SelectJobContractor(SaveTask.JobID,
                        SaveTask.ContractorID);
                    CurrentBRJob.DeleteJobContractor(doJCTaskContractorO);

                    DOJobContractor doJCTaskContractorA = CurrentBRJob.CreateJobContractor(Task.JobID,
                        SaveTask.ContractorID, CurrentSessionContext.Owner.ContactID);
                    CurrentBRJob.SaveJobContractor(doJCTaskContractorA);

                    //Copy the materials to the amended task.
                    List<DOBase> Materials = CurrentBRJob.SelectTaskMaterials(Task.TaskID);
                    foreach (DOTaskMaterial TaskMaterial in Materials)
                    {
                        TaskMaterial.TaskMaterialID = Guid.NewGuid();
                        TaskMaterial.TaskID = SaveTask.TaskID;
                        TaskMaterial.PersistenceStatus = ObjectPersistenceStatus.New;
                        CurrentBRJob.SaveTaskMaterial(TaskMaterial);
                    }

                    //Copy the labour to the amended task.
                    List<DOBase> Labour = CurrentBRJob.SelectTaskLabour(Task.TaskID);
                    foreach (DOTaskLabour TaskLabour in Labour)
                    {
                        TaskLabour.TaskLabourID = Guid.NewGuid();
                        TaskLabour.TaskID = SaveTask.TaskID;
                        TaskLabour.PersistenceStatus = ObjectPersistenceStatus.New;
                        CurrentBRJob.SaveTaskLabour(TaskLabour);
                    }
                }
                NewTask = false;
                CurrentSessionContext.CurrentTask = Task;

                //If the job was a completed job, uncomplete the job.
                if (CurrentSessionContext.CurrentJob.JobStatus == DOJob.JobStatusEnum.Complete)
                {
                    DOJob Job = CurrentSessionContext.CurrentJob;
                    Job.CompletedBy = Guid.Empty;
                    Job.CompletedDate = DateAndTime.NoValueDate;

                    Job.JobStatus = DOJob.JobStatusEnum.Incomplete;
                    CurrentBRJob.SaveJob(Job);

                    DOJobChange jc = CurrentBRJob.CreateJobChange(Job.JobID, DOJobChange.JobChangeType.JobUncompleted,
                        CurrentSessionContext.Owner.ContactID);
                    CurrentBRJob.SaveJobChange(jc);
                }

            

        }

        private Guid GetTaskContractor()
        {
            if (Contractor_RBL.SelectedValue != "Our Staff")
            {
                if (NewContractorAdded && ContractorsDDl.SelectedValue == "-Select-")
                    return NewContractor.ContactID;
                 if (ContractorsDDl.SelectedValue.ToString() != "" && ContractorsDDl.SelectedValue != "-Select-")
                    return Guid.Parse(ContractorsDDl.SelectedValue.ToString());
                 if (NewContractor != null)
                    return NewContractor.ContactID;
                
               
                    if (ddlContractor.SelectedValue.ToString() != "")
                        return Guid.Parse(ddlContractor.SelectedValue.ToString());
                     if (ContractorsDDl.SelectedValue == "-Select-")
                        throw new FieldValidationException("Please select contractor");
                     if (ContractorsDDl.SelectedValue.ToString() != "")
                        return Guid.Parse(ContractorsDDl.SelectedValue.ToString());
                    
                        return CurrentSessionContext.CurrentContact.ContactID;
                
            }
            else
            {
                return CurrentSessionContext.CurrentContact.ContactID;
            }
           
               
        }

        protected void btnDeleteMaterial_Click(object sender, EventArgs e)
        {
            Guid TMID = new Guid(((Button)sender).CommandArgument);
            DOTaskMaterial TM = CurrentBRJob.SelectSingleTaskMaterial(TMID);
            TM.Active = false;
            CurrentBRJob.SaveTaskMaterial(TM);
        }

        protected void btnDeleteLabour_Click(object sender, EventArgs e)
        {
            Guid TLID = new Guid(((Button)sender).CommandArgument);
            DOTaskLabour TL = CurrentBRJob.SelectSingleTaskLabour(TLID);
            TL.Active = false;
            CurrentBRJob.SaveTaskLabour(TL);
        }

        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            bool Trunc = false;

            try
            {
                //Save();
                DOTaskMaterial TaskMaterial;
                Guid MaterialID;
                if (Request.Form[ddlMaterialCategory.UniqueID] == Constants.Guid_DefaultUser.ToString())
                {
                    //New material, create and save.
                    DOMaterial NewMaterial = CurrentBRJob.CreateMaterial(CurrentSessionContext.Owner.ContactID);
                    MaterialForm.SaveForm(NewMaterial);
                    CurrentBRJob.SaveMaterial(NewMaterial);
                    MaterialID = NewMaterial.MaterialID;
                }
                else
                {
                    if (string.IsNullOrEmpty(Request.Form["ddlMaterials"]))
                    {
                        ShowMessage("A material category must be selected.", MessageType.Error);
                        return;
                    }
                    else
                    {
                        MaterialID = new Guid(Request.Form["ddlMaterials"]);
                    }
                }

                //Must enter quantity.
                if (string.IsNullOrEmpty(txtQuantity.Text))
                {
                    ShowMessage("Quantity must be entered.", MessageType.Error);
                    return;
                }

                //DOTaskMaterial TaskMaterial = CurrentBRJob.CreateTaskMaterial(Task.TaskID, GetDDLGuid(ddlMaterials), CurrentSessionContext.Owner.ContactID);
                TaskMaterial = CurrentBRJob.CreateTaskMaterial(Task.TaskID, MaterialID, CurrentSessionContext.Owner.ContactID);
                TaskMaterial.Quantity = GetDecimal(txtQuantity, "Material Quantity");
                TaskMaterial.Description = txtDescription.Text;

                //Save name and price in task material entry (in case these change later).
                DOMaterial material = CurrentBRJob.SelectMaterial(MaterialID);
                TaskMaterial.MaterialName = material.MaterialName;
                TaskMaterial.SellPrice = material.SellPrice;

                if (TaskMaterial.Description.Length > 512)
                {
                    TaskMaterial.Description = TaskMaterial.Description.Substring(0, 512);
                    Trunc = true;
                }
                TaskMaterial.MaterialType = (TaskMaterialType)int.Parse(Request.Form[ddlMaterialType.UniqueID]);
                CurrentBRJob.SaveTaskMaterial(TaskMaterial);

                //Deduct actual from required (if applicable).
                if (TaskMaterial.MaterialType == TaskMaterialType.Actual)
                {
                    List<DOBase> TaskMaterials = CurrentBRJob.SelectTaskMaterials(Task.TaskID);
                    decimal Remaining = TaskMaterial.Quantity;
                    foreach (DOTaskMaterial tm in TaskMaterials)
                    {
                        if (tm.MaterialType == TaskMaterialType.Required && TaskMaterial.MaterialID == tm.MaterialID)
                        {
                            if (tm.Quantity < Remaining)
                            {
                                Remaining -= tm.Quantity;
                                tm.Quantity = 0;
                            }
                            else
                            {
                                tm.Quantity -= Remaining;
                                Remaining = 0;
                            }
                            CurrentBRJob.SaveTaskMaterial(tm);
                        }
                        if (Remaining <= 0) break;
                    }
                }
                if (Trunc)
                {
                    ShowMessage("The material was added successfully, however the material description was truncated.", MessageType.Warning);
                }
                else
                {
                    ShowMessage("The material was added successfully.", MessageType.Info);
                }

                if (ddlMaterialCategory.Items.Count > 0)
                    ddlMaterialCategory.SelectedIndex = 0;
                MaterialForm.ClearForm();
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnAddLabour_Click(object sender, EventArgs e)
        {
            
            DOContact NewEmployee = null;
            bool EmployeeAdded = false;
            bool Trunc = false;
            int Minutes = int.Parse(Request.Form[ddlLabourTime.UniqueID]);
            //try
            //{
            //    string SenderID = ((Button)sender).ID;
            //    if (SenderID.StartsWith("btnAdd"))
            //        Minutes = int.Parse(SenderID.Substring(6));
            //    else
            //        throw new Exception();
            //}
            //catch
            //{
            //    ShowMessage("Invalid time button for adding labour", MessageType.Error);
            //}

            try
            {
                Guid LabourContactID = GetDDLGuid(ddlLabourContactID);
                if (LabourContactID == Constants.Guid_DefaultUser)
                {
                    //Get the company for the new contact.
                    DOContact TaskCompany = CurrentBRContact.SelectContact(Task.ContractorID);
                    if (TaskCompany.ContactType == DOContact.ContactTypeEnum.Individual)
                    {

                        TaskCompany = null;
                    }

                    //Get form details.
                    NewEmployee = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID, DOContact.ContactTypeEnum.Individual);
                    NewUserDetails.SaveForm(NewEmployee);

                    //Check if user with email already exists.
                    DOContact ExistingContact = CurrentBRContact.SelectContactByUsername(NewEmployee.Email);
                    if (ExistingContact == null)
                    {
                        CurrentBRContact.SaveContact(NewEmployee);
                    }
                    else
                    {
                        NewEmployee = ExistingContact;
                    }

                    DOContact EmployeeContact = ExistingContact != null ? ExistingContact : NewEmployee;

                    //Link to the company if task contractor not individual.
                    if (TaskCompany != null)
                    {
                        DOContactCompany CC = CurrentBRContact.CreateContactCompany(EmployeeContact.ContactID, TaskCompany.ContactID, CurrentSessionContext.Owner.ContactID);
                        CurrentBRContact.SaveContactCompany(CC);

                        //Create new employee record from entered form data.
                        //Use entered data even if there is an existing contact.
                        DOEmployeeInfo Employee = CurrentBRContact.CreateEmployeeInfo(CC.ContactCompanyID, CurrentSessionContext.Owner.ContactID);
                        Employee.FirstName = NewEmployee.FirstName;
                        Employee.LastName = NewEmployee.LastName;
                        Employee.Email = NewEmployee.Email;
                        Employee.Phone = NewEmployee.Phone;
                        Employee.Address1 = NewEmployee.Address1;
                        Employee.Address2 = NewEmployee.Address2;
                        decimal PayRate = 0;
                        decimal LabourRate = 0;
                        decimal.TryParse(txtPayRate.Text, out PayRate);
                        decimal.TryParse(txtLabourRateNew.Text, out LabourRate);
                        Employee.PayRate = PayRate;
                        Employee.LabourRate = LabourRate;
                        CurrentBRContact.SaveEmployeeInfo(Employee);

                        //Send email informing employee that they have been linked.
                        string Subject = TaskCompany.DisplayName + " has added you as an employee on OnTrack";
                        string Body = TaskCompany.DisplayName + " has added you as an employee on OnTrack";
                        if (Constants.EMAIL__TESTMODE)
                        {
                            Body += "<br/><br/>";
                            Body += "Sender: " + CurrentSessionContext.CurrentContact.DisplayName + "<br />";
                            Body += "Sender logged in as: " + CurrentSessionContext.Owner.DisplayName + " (" + CurrentSessionContext.Owner.UserName + ")<br />";
                            Body += "Recipient: " + TaskCompany.DisplayName + " (" + TaskCompany.Email + ")<br />";
                            //Body += "Job ID: " + CurrentSessionContext.CurrentJob.JobNumberAuto;

                        }

                        CurrentBRGeneral.SendEmail("no-reply@ontrack.co.nz", Employee.Email, Subject, Body);
                    }


                    LabourContactID = NewEmployee.ContactID;

                }
                DOTaskLabour TaskLabour = CurrentBRJob.CreateTaskLabour(Task.TaskID, LabourContactID, CurrentSessionContext.Owner.ContactID);
                TaskLabour.LabourDate = dateLabourDate.GetDate();
                TaskLabour.LabourType = (TaskMaterialType)int.Parse(Request.Form[ddlLabourType.UniqueID]);
                //TaskLabour.StartMinute = int.Parse(Request.Form[ddlLabourStartTime.UniqueID]);
                //TaskLabour.EndMinute = int.Parse(Request.Form[ddlLabourEndTime.UniqueID]);
                TaskLabour.StartMinute = 0;
                TaskLabour.EndMinute = Minutes;
                TaskLabour.Description = txtLabourDesc.Text;
                decimal TaskLabourRate;
                if (!decimal.TryParse(txtLabourRate.Text, out TaskLabourRate))
                {
                    ShowMessage("Labour rate must be a valid number.", MessageType.Error);
                    return;
                }
                TaskLabour.LabourRate = TaskLabourRate;
                if (TaskLabour.Description.Length > 512)
                {
                    Trunc = true;
                    TaskLabour.Description = TaskLabour.Description.Substring(0, 512);
                }
                TaskLabour.Chargeable = chkChargeable.Checked;
                CurrentBRJob.SaveTaskLabour(TaskLabour);

                if (TaskLabour.LabourType == TaskMaterialType.Actual)
                {
                    //Deduct from required.
                    List<DOBase> TaskLabours = CurrentBRJob.SelectTaskLabour(Task.TaskID);
                    int Remaining = TaskLabour.EndMinute;
                    foreach (DOTaskLabour tl in TaskLabours)
                    {
                        if (tl.LabourType == TaskMaterialType.Required && TaskLabour.ContactID == tl.ContactID)
                        {
                            if (tl.EndMinute < Remaining)
                            {
                                Remaining -= tl.EndMinute;
                                tl.EndMinute = 0;
                            }
                            else
                            {
                                tl.EndMinute -= Remaining;
                                Remaining = 0;
                            }
                            CurrentBRJob.SaveTaskLabour(tl);
                        }
                        if (Remaining <= 0) break;
                    }
                }
                if (Trunc)
                {
                    ShowMessage("The labour time was added successfully, however the labour description was truncated." + (EmployeeAdded ? "<br />An email has been sent to your new employee." : ""), MessageType.Warning);
                }
                else
                {
                    ShowMessage("The labour time was added successfully." + (EmployeeAdded ? "<br />An email has been sent to your new employee." : ""), MessageType.Info);
                }
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                if (NewEmployee != null && NewEmployee.PersistenceStatus == ObjectPersistenceStatus.Existing)
                    CurrentBRContact.DeleteContactComplete(NewEmployee);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="MaterialID"></param>
        protected string GetMaterialName(Guid MaterialID)
        {
            DOMaterial Material = CurrentBRJob.SelectMaterial(MaterialID);
            return Material.MaterialName;
        }

        protected string GetLabourName(Guid LabourID)
        {
            DOLabour Labour = CurrentBRJob.SelectLabour(LabourID);
            return Labour.LabourName;
        }


        List<DOBase> Employees = null;

        protected string GetEmployeeName(Guid ContactID)
        {
            if (Employees == null)
                Employees = CurrentBRContact.SelectCompanyEmployees(CurrentSessionContext.CurrentContact.ContactID);

            DOContactEmployee ceInfo = (from DOContactEmployee ce in Employees
                                        where ce.ContactID == ContactID
                                        select ce).FirstOrDefault<DOContactEmployee>();

            if (ceInfo != null)
                return ceInfo.DisplayName;
            return string.Empty;
        }


        protected Guid GetTaskOwner()
        {
            //If the current contact has been set as a job contractor, the task will appear as if the job owner added the task.
            DOJobContractor JobContractor = CurrentBRJob.SelectJobContractor(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.CurrentContact.ContactID);
            if (JobContractor == null)
                return CurrentSessionContext.CurrentContact.ContactID;
            else
                return CurrentSessionContext.CurrentJob.JobOwner;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_JobSummary);
        }

        protected void btnDeleteTask_Click(object sender, EventArgs e)
        {
            CurrentBRJob.DeleteTask(Task);
            List<DOBase> TaskList = CurrentBRJob.SelectTasks(Task.JobID);
         
            if (CurrentSessionContext.CurrentCustomer == null)
            {
                CurrentSessionContext.CurrentCustomer = CurrentSessionContext.CurrentContractee;
            }
            if (CurrentSessionContext.CurrentContact != null && CurrentSessionContext.CurrentCustomer != null)
            {
                bool Complete = true;
                List<DOBase> activeTasksforContact =
                        CurrentBRJob.SelectActiveTasksforContact(CurrentSessionContext.CurrentContact.ContactID,
                            CurrentSessionContext.CurrentCustomer.ContactID, Task.JobID);
                if (activeTasksforContact.Count==0)
                {
                   
                    foreach (var doBase in activeTasksforContact)
                    {
                        var task = (DOTask) doBase;
                        if (task.Status != DOTask.TaskStatusEnum.Paid)
                        {
                            Complete = false;

                            break;
                        }
                    }
                    if (Complete)
                    {
                        DOJob job = CurrentBRJob.SelectJob(Task.JobID);
                        if (job != null)
                        {
                            //job.JobStatus = DOJob.JobStatusEnum.Complete;
                            //CurrentBRJob.SaveJob(job);
                            DOJobContractor jobContract = CurrentBRJob.SelectJobContractor(job.JobID,
                              CurrentSessionContext.CurrentContact.ContactID);
                            jobContract.Status = 1;
                            CurrentBRJob.SaveJobContractor(jobContract);
                        }
                    }
                    else
                    {
                        DOJob job = CurrentBRJob.SelectJob(Task.JobID);
                        if (job != null)
                        {
                            job.JobStatus = DOJob.JobStatusEnum.Incomplete;
                            CurrentBRJob.SaveJob(job);
                        }
                    }
                }
                //if tasks count is 0 that means contractor-customer pair can become inactive 
          

            
            if (TaskList.Count == 0)
            {
                if (CurrentSessionContext.CurrentJob != null)
                {
                    CurrentSessionContext.CurrentJob.JobStatus = DOJob.JobStatusEnum.Complete;
                    CurrentBRJob.SaveJob(CurrentSessionContext.CurrentJob);
                }
                CurrentBRJob.UpdateJobContractor(Task.JobID);
            
            //if tasks count is 0 that means contractor-customer pair can become inactive 
            //find the contractor-customer pair
            DOContractorCustomer contractorCustomer =
                CurrentBRContact.SelectContractorCustomer(
                    ContactID: CurrentSessionContext.CurrentContact.ContactID,
                    CustomerID: CurrentSessionContext.CurrentCustomer.ContactID);
            //if a pair record is found- make it inactive
            if (contractorCustomer != null)
            {
                contractorCustomer.Active = !Complete;
                CurrentBRContact.SaveContractorCustomer(contractorCustomer);
            }
            // if task count=0, job can be completed for that contractor
            DOJobContractor jobContractor = CurrentBRJob.SelectJobContractor(Task.JobID,
                CurrentSessionContext.CurrentContact.ContactID);
            //if job is found- make it inactive
            if (jobContractor != null)
            {
                jobContractor.Active = !Complete;
                CurrentBRJob.SaveJobContractor(JobContractor: jobContractor);
            }
                }
                //find the contactsite record for the current contact (entity) and site
                DOContactSite contactSite =
                CurrentBRSite.SelectContactSite(Task.SiteID,
                    contractorId: CurrentSessionContext.CurrentContact.ContactID);
            //if contactsite record is found- make it inactive
            if (contactSite != null)
                contactSite.Active = !Complete;
            CurrentBRSite.SaveContactSite(contactSite);
           
            }
            // Response.Redirect(Constants.URL_JobDetails);
            Response.Redirect(Constants.URL_JobSummary);
        }

        protected void btnCompleteTask_Click(object sender, EventArgs e)
        {
            //Can't complete task if no materials and no labour.
            List<DOBase> TM = CurrentBRJob.SelectTaskMaterials(Task.TaskID);
            List<DOBase> TL = CurrentBRJob.SelectTaskLabour(Task.TaskID);
            bool HasActual = false;
            foreach (DOTaskMaterial myTm in TM)
            {
                if (myTm.MaterialType == TaskMaterialType.Actual && myTm.Active)
                {
                    HasActual = true;
                    break;
                }
            }
            if (!HasActual)
            {
                foreach (DOTaskLabour myTL in TL)
                {
                    if (myTL.LabourType == TaskMaterialType.Actual && myTL.Active)
                    {
                        HasActual = true;
                        break;
                    }
                }
            }


            if (!HasActual)
            {
                ShowMessage("The task cannot be completed because no materials or labour have been assigned.", MessageType.Error);
                return;
            }
           
            DOTaskCompletion TC = CurrentBRJob.CreateTaskCompletion(Task.TaskID, CurrentSessionContext.Owner.ContactID);
            CurrentBRJob.SaveTaskCompletion(TC);
            Task.Status = DOTask.TaskStatusEnum.Complete;
            CurrentBRJob.SaveTask(Task);
           
            Response.Redirect(Constants.URL_JobSummary);

        }

        protected void btnUncompleteTask_Click(object sender, EventArgs e)
        {
            DOTaskCompletion TC = CurrentBRJob.SelectTaskCompletion(Task.TaskID);
            CurrentBRJob.DeleteTaskCompletion(TC);
            Task.Status = DOTask.TaskStatusEnum.Incomplete;
            CurrentBRJob.SaveTask(Task);

        }

        protected void btnLMVisibilityUpdate_Click(object sender, EventArgs e)
        {
            Task.LMVisibility = (DOTask.LMVisibilityEnum)int.Parse(Request.Form[ddlLMVisibility.UniqueID]);
            CurrentBRJob.SaveTask(Task);
        }

        protected void btnSubmitQuote_Click(object sender, EventArgs e)
        {
            if (Quote != null) return;
            if (Task.PersistenceStatus == ObjectPersistenceStatus.New) return;
            if (CurrentSessionContext.CurrentJob.JobType != DOJob.JobTypeEnum.ToQuote) return;

            //Must have either quoted labour or materials.
            List<DOBase> TaskLabour = CurrentBRJob.SelectTaskLabour(Task.TaskID);
            List<DOBase> TaskMaterials = CurrentBRJob.SelectTaskMaterials(Task.TaskID);
            if (TaskLabour.Count == 0 && TaskMaterials.Count == 0)
            {
                ShowMessage("This task has no quoted material or labour", MessageType.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtTermsAndConditions.Text))
            {
                ShowMessage("You must enter terms and conditions.", MessageType.Error);
                return;
            }

            Quote = CurrentBRJob.CreateTaskQuote(Task.TaskID, CurrentSessionContext.Owner.ContactID);
            Quote.TermsAndConditions = txtTermsAndConditions.Text;
            CurrentBRJob.SaveTaskQuote(Quote);


        }

        protected void btnDeleteQuote_Click(object sender, EventArgs e)
        {
            CurrentBRJob.DeleteTaskQuote(Quote);
            Quote = null;
        }

        /// <summary>
        /// Find the contractor if email id is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                List<DOBase> EmailContractors = CurrentBRContact.SelectContactsByEmail(txtFindNewContractor.Text.Trim());

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
                            //Body += "Job ID: " + CurrentSessionContext.CurrentJob.JobNumberAuto;

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
            else
                ShowMessage("Please enter emailID");

            //SetFocus(txtFindNewContractor);
        }

        [WebMethod]
        public static string GetMaterialCategoryItems(string MaterialCategoryID)
        {
            string ret = string.Empty;
            try
            {
                Electracraft.Framework.BusinessRules.BRJob staticBRJob = new Framework.BusinessRules.BRJob();
                List<DOBase> items = staticBRJob.SelectMaterials(new Guid(MaterialCategoryID));

                ret = "{";
                bool first = true;
                foreach (DOMaterial item in items)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        ret += ",";
                    }
                    ret += string.Format("\"{0}\":\"{1}\"", item.MaterialID.ToString(), HttpUtility.HtmlEncode(item.MaterialName.ToString()));
                }
                ret += "}";
            }
            catch
            {
                ret = "{}";
            }
            return ret;
        }
        protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDistrict();
            LoadSuburbs();
            LoadContractorsBasedOnTradeCatgory();
           // TradeCategories_ddl.Focus();

        }
        protected void LoadSuburbs()
        {
            // Guid regionCode;
            //regionCode = ;
            List<DOBase> Suburbs = CurrentBRSuburb.SelectSuburbsSorted(Guid.Parse(District_DDL.SelectedValue));
            // Suburbs.Add(new ListItem("Select All"),0);
            //Suburbs.Add(0, "All");
            Suburb_CBList.DataSource = Suburbs;
            Suburb_CBList.DataTextField = "SuburbName";
            Suburb_CBList.DataValueField = "SuburbID";
            Suburb_CBList.DataBind();
        }

        //protected void TradeCategories_List_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        protected void SuburbSelection_rdbtn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SuburbSelection_rdbtn.SelectedValue == "Select")
            {
                Suburb_cbl_div.Visible = true;
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    item.Selected = false;
                }
               // TradeCategories_ddl.Focus();
            }
            else if (SuburbSelection_rdbtn.SelectedValue == "All")
            {
                Suburb_cbl_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    item.Selected = true;
                }
               // TradeCategories_ddl.Focus();
            }
            LoadContractorsBasedOnTradeCatgory();

        }
        public void LoadContractorsBasedOnTradeCatgory()
        {
            List<DOBase> ContractorsList = new List<DOBase>();
            // IEnumerable<DOBase> list;
            ContractorsList.Clear();
            //If subscribed user, show all contractors
            if (CurrentSessionContext.CurrentContact.Subscribed || CurrentSessionContext.CurrentContact.SubscriptionPending)
            {
                List<DOBase> ContractorsforSubscribedUsers = new List<DOBase>();
                if(Suburb_CBList.Items.Count==0)
               LoadSuburbs();
                if(SuburbSelection_rdbtn.SelectedValue=="All")
                MakeSuburbSelectedAllByDefault();
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
                            Guid suburbId = os.SuburbID;
                            //Guid tradecategoryId = Guid.Parse(TradeCategories_ddl.SelectedValue);
                            if(SubtradeCat_cbl.Items.Count==0)
                            LoadSubTradeCategories();
                            if(SubtradeCategoryRdBtn.SelectedValue=="All")
                            MakeSubTradeCategorySelectedAllByDefault();
                            foreach (ListItem i in SubtradeCat_cbl.Items)
                            {
                                if (i.Selected)
                                {
                                    Guid subTradeCategoryID = Guid.Parse(i.Value);
                                    ContractorsforSubscribedUsers.AddRange(CurrentBRContactTradecategory.SelectContractors(
                                                      suburbId, subTradeCategoryID
                                                          ));
                                }
                            }
                        }
                        finally
                        { }
                    }
                }
                ContractorsList = ContractorsforSubscribedUsers;
            }
            else if (!CurrentSessionContext.CurrentContact.Subscribed)
            {
                List<DOBase> ContractorsforUnSubscribedUsers = new List<DOBase>();
                MakeSuburbSelectedAllByDefault();
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
                            Guid suburbId = os.SuburbID;
                            //Guid tradecategoryId = Guid.Parse(TradeCategories_ddl.SelectedValue);
                            //ContractorsforUnSubscribedUsers.AddRange
                             //   (CurrentBRContactTradecategory.SelectContractors(suburbId, tradecategoryId));
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
                        { }
                    }
                }
                ContractorsList = ContractorsforUnSubscribedUsers;
                //  list = ContractorsforUnSubscribedUsers.Distinct();
            }
            if (Contractor_RBL.SelectedValue == "Our Staff")
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
                    if (testItem.ID == searchItem.ID || testItem.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {

                        ContractorsList.RemoveAt(j);
                        j--;
                    }
                }
            }
           
           ContractorsList.Insert(0,CurrentSessionContext.CurrentContact);
            ddlContractor.DataSource = ContractorsList;
            // ddlContractor.DataSource = list;
            ddlContractor.DataValueField = "ContactID";
            ddlContractor.DataTextField = "DisplayName";
            ddlContractor.DataBind();
        }

        private void MakeSuburbSelectedAllByDefault()
        {
            if (SuburbSelection_rdbtn.SelectedValue == "All")
            {
                Suburb_cbl_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in Suburb_CBList.Items)
                {
                    item.Selected = true;
                }
            }

        }
        private void MakeSubTradeCategorySelectedAllByDefault()
        {
            if (SubtradeCategoryRdBtn.SelectedValue == "All")
            {
                SUbTradeCategory_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in SubtradeCat_cbl.Items)
                {
                    item.Selected = true;
                }
            }

        }

        protected void TradeCategories_ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubTradeCategories();
            LoadContractorsBasedOnTradeCatgory();
           // ddlContractor.Focus();
           
        }
        protected void Contractor_RBL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Contractor_RBL.SelectedValue == "Our Staff")
            {
                //DifferentContractor_Pnl.Visible = false;
                DiffContractor.Visible = false;
                ContractorsListDiv.Visible = true;
            }
            else if (Contractor_RBL.SelectedValue == "Different")
            {
                //DifferentContractor_Pnl.Visible = true;
                // ddlContractor.Focus();
                DiffContractor.Visible = true;
                ContractorsListDiv.Visible = true;
                //LoadContractorsBasedOnTradeCatgory();
                LoadAllSubscribedSearchableContractors();
            }
        }

        private void LoadAllSubscribedSearchableContractors()
        {
            List<DOBase> contractors = CurrentBRContact.SelectSearchableContractors();
            List<DOBase> contractorsFinalList = new List<DOBase>();
            //If subscribed user, show all contractors
            if (CurrentSessionContext.Owner.Subscribed || CurrentSessionContext.Owner.SubscriptionPending)
            {
                contractorsFinalList = contractors;
            }
            else
            {
                for (int i=0; i < contractors.Count;i++)
                {
                    DOContact contact = contractors[i] as DOContact;
                    if (contact.Subscribed == true)
                        contractorsFinalList.Add(contractors[i]);
                }
            }
           
            ContractorsDDl.DataSource = contractorsFinalList;
            ContractorsDDl.DataTextField = "DisplayName";
            ContractorsDDl.DataValueField = "ContactID";
            ContractorsDDl.DataBind();
            ContractorsDDl.Items.Insert(0, "-Select-");
        }

        protected void Suburb_CBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTradeCategories();
            LoadContractorsBasedOnTradeCatgory();
           // TradeCategories_ddl.Focus();
        }

        protected void District_DDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuburbs();
            LoadContractorsBasedOnTradeCatgory();
           // TradeCategories_ddl.Focus();
        }

        protected void SubtradeCategoryRdBtn_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFindNewContractor.Focus();
            LoadSubTradeCategories();
            LoadContractorsBasedOnTradeCatgory();
            if (SubtradeCategoryRdBtn.SelectedValue == "Select")
            {
                SUbTradeCategory_div.Visible = true;
                foreach (ListItem item in SubtradeCat_cbl.Items)
                {
                    item.Selected = false;
                }
              //  ddlContractor.Focus();
            }
            else if (SubtradeCategoryRdBtn.SelectedValue == "All")
            {
                SUbTradeCategory_div.Visible = false;
                // Suburb_CBList.
                foreach (ListItem item in SubtradeCat_cbl.Items)
                {
                    item.Selected = true;
                }
              //  btnFindNewContractor.Focus();
            }
        }

        private void LoadSubTradeCategories()
        {
            //List<DOBase> subTradeCategories = CurrentBRTradeCategory.SelectSubTradeCategories(Guid.Parse(TradeCategories_ddl.SelectedValue));
            //SubtradeCat_cbl.DataSource = subTradeCategories;
            //SubtradeCat_cbl.DataTextField = "SubTradeCategoryName";
            //SubtradeCat_cbl.DataValueField = "SubTradeCategoryID";
            //SubtradeCat_cbl.DataBind();
        }

        protected void Suburb_CBList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            LoadContractorsBasedOnTradeCatgory();
        }

        protected void SubtradeCat_cbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContractorsBasedOnTradeCatgory();
        }

        protected void TradeCategoryAll_PreRender(object sender, EventArgs e)
        {
            if(!IsPostBack)
            { 
            string defaultTradeCat = FindCompanyTradeCat();
                if (defaultTradeCat != "")
                    TradeCategoryAll.SelectedIndex = TradeCategoryAll.Items.IndexOf(TradeCategoryAll.Items.FindByText(defaultTradeCat));
                else
                    TradeCategoryAll.Items.Insert(0, "All");
            }
        }

        protected void ContractorsChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
           try
            {
                if (ContractorsChoice.SelectedValue == "ByTradCat")//Bug here if dropdown "TradeCategoryAll" has nothing selected.
                {
                    if (TradeCategoryAll.SelectedValue != "All")
                    {
                        List<DOBase> contractors = CurrentBRContactTradecategory.SelectContractorsbyTradeCat(Guid.Parse(TradeCategoryAll.SelectedValue.ToString()));
                        if (CurrentSessionContext.Owner.Subscribed || CurrentSessionContext.Owner.SubscriptionPending)
                        {
                            for (int i = 0; i < contractors.Count; i++)
                            {
                                DOContactTradeCatInfo contact = contractors[i] as DOContactTradeCatInfo;
                                if (contact.Searchable != 1)
                                {
                                    contractors.RemoveAt(i);
                                    if (i > 0)
                                        i--;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < contractors.Count; i++)
                            {
                                DOContactTradeCatInfo contact = contractors[i] as DOContactTradeCatInfo;
                                if (contact.Searchable != 1 && contact.Subscribed != true)
                                {
                                    contractors.RemoveAt(i);
                                    if (i > 0)
                                        i--;
                                }
                            }
                        }
                        for (int i = 0; i < contractors.Count; i++)
                        {
                            for (int j = i + 1; j < contractors.Count; j++)
                            {
                                DOContactTradeCatInfo contact1 = contractors[i] as DOContactTradeCatInfo;
                                DOContactTradeCatInfo contact2 = contractors[j] as DOContactTradeCatInfo;
                                if (contact1.ContactID == contact2.ContactID)
                                    contractors.RemoveAt(j);
                            }
                        }
                        //if (CurrentSessionContext.Owner.Subscribed || CurrentSessionContext.Owner.SubscriptionPending)
                        //{

                        //}
                        //if (CurrentSessionContext.Owner.Subscribed || CurrentSessionContext.Owner.SubscriptionPending)
                        //{

                        //}
                        //if (CurrentSessionContext.Owner.Subscribed || CurrentSessionContext.Owner.SubscriptionPending)
                        //{

                        //}  //if (CurrentSessionContext.Owner.Subscribed || CurrentSessionContext.Owner.SubscriptionPending)
                        //{

                        //}
                        //else
                        //{
                        //    for (int i = 0; i < contractors.Count; i++)
                        //    {
                        //        DOContactTradeCatInfo contact = contractors[i] as DOContactTradeCatInfo;
                        //        if (contact.Subscribed != true)
                        //        {
                        //            contractors.RemoveAt(i);
                        //            if (i > 0)
                        //                i--;
                        //        }
                        //    }
                        //}
                        ContractorsDDl.DataSource = contractors;
                        ContractorsDDl.DataTextField = "DisplayName";
                        ContractorsDDl.DataValueField = "ContactID";
                    }
                }
                else if (Contractor_RBL.SelectedValue == "Different")
                    LoadAllSubscribedSearchableContractors();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            SetFocus(ContractorsDDl);
        }

        protected void TradeCategoryAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContractorsChoice_SelectedIndexChanged(sender, e);
        }

        protected void ContractorsDDl_PreRender(object sender, EventArgs e)
        {
            if (Contractor_RBL.SelectedValue == "Different")
            { 
            ListItem item = ContractorsDDl.Items.FindByText("-Select-");
            if (item==null)
            ContractorsDDl.Items.Insert(0, "-Select-");
            }
        }



        //protected void District_RBL_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    btnFindNewContractor.Focus();
        //    if (District_RBL.SelectedValue == "Select")
        //    {
        //        District_div.Visible = true;
        //        foreach (ListItem item in District_CBL.Items)
        //        {
        //            item.Selected = false;
        //        }
        //        TradeCategories_ddl.Focus();
        //    }
        //    else if (District_RBL.SelectedValue == "All")
        //    {
        //        District_div.Visible = false;

        //        foreach (ListItem item in District_CBL.Items)
        //        {
        //            item.Selected = true;
        //        }
        //        TradeCategories_ddl.Focus();
        //    }
        //    LoadDistrict();
        //    LoadContractorsBasedOnTradeCatgory();

        //}
        protected void CustomerDDL_OnPreRender(object sender, EventArgs e)
        {
            if (NewTask)
            {
                if (CurrentSessionContext.CurrentCustomer != null && !IsPostBack)
                    CustomerDDL.SelectedValue = CurrentSessionContext.CurrentCustomer.ContactID.ToString();
            }
            else
            {
                if (CurrentSessionContext.CurrentTask != null)
                    CustomerDDL.SelectedValue = CurrentSessionContext.CurrentTask.TaskCustomerID.ToString();
            }
            
        }
    }


}