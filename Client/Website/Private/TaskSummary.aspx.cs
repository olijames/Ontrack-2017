using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;
using System.Web.Services;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using System.Configuration;
// Tony added for Xero connectivity
using XeroApi.OAuth;
using XeroApi.Model;
using Button = System.Web.UI.WebControls.Button;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;
using CheckBox = System.Web.UI.WebControls.CheckBox;


namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class TaskSummary : PageBase
    {
        //Tony added for Xero Api Connection
        private XeroApi.OAuth.XeroApiPrivateSession xSession;
        private XeroApi.Repository repository;
        private X509Certificate2 cert;
        private Addresses addresses;
        private LineItems items;
        private string errorMessage = "";
        
        ////Tonys:
        ////Location of cert file
        //private const string CertPath = "C:\\OpenSSL-Win32\\bin\\public_privatekey.pfx";
        ////string of consumer key
        //private const string ConsumerKey = "ZYOUYTKMXIZHXMSX02IEOYN084UZVO";
        ////cert password
        //private const string CertPassword = "xero";


        //Jareds
        //Location of cert file
        private string CertPath = ConfigurationManager.AppSettings["xeroCertPath"];
        //private const string CertPath = "C:\\OpenSSL-Win64\\bin\\public_privatekey.pfx";
        //string of consumer key
        private string ConsumerKey = ConfigurationManager.AppSettings["xeroConsumerKey"];
        //cert password
        private string CertPassword = ConfigurationManager.AppSettings["xeroCertPassword"];
        //Tonys password:
        //private const string CertPassword = "xero";
        

        //Message string for Xero functionality
        //Info message for xero connection is successful
        private const string XeroConSuccess = "Connection to xero successful";
        private const string InvoiceTransferSuccess = "Invoice transferred to Xero successfully";
        private const string InvoiceTransferFail = "There were errors in transferring invoice to Xero";

        //Error message if there is no cert at all
        private const string CertNotExist =
            "Please contact Ontrack for further information about connecting to Xero directly. Importing invoices to Xero can be as quick as a click of a button.";
        //Error message if cert password doesn't match
        private const string CertPasswdNotMatch = "There is an issue with your connection to Xero. Your certificate password is not valid.Please contact Ontrack or your administrator.In the meantime you can copy the text below the invoice section and import this to Xero";
        //Error message if consumer key is invalid
        private const string ConsumerKeyInvalid = "There is an issue with your connection to Xero. Your consumer key is not valid.Please contact Ontrack or your administrator.In the meantime you can copy the text below the invoice section and import this to Xero";
        //Error message if cert file in invalid
        private const string CertPathNotExists =
            "There is an issue with your connection to Xero. The path to the certificate no longer exists. Please contact Ontrack or your administrator. In the meantime you can copy the text below the invoice section and import this to Xero";
        //Error message if another exception occurs
        private const string XeroEtcExeption =
            "There is an issue with your connection to Xero. Please contact Ontrack or your administrator. In the meantime you can copy the text below the invoice section and import this to Xero";

        //Tony added 2.Feb.2017 regarding Vehicle permission begin
        private const string NO_VEHICLE_NO_MATERIAL_FROM_VEHICLE_NO_ADD_VEHICLE =
            "You do not have a vehicle or the permission to move stock from one vehicle to another. " +
            "If you want to add a vehicle or have permission to add materials from another persons vehicle then talk to:\\n";

        private const string NO_VEHICLE_NO_MATERIAL_FROM_VEHICLE_ADD_VEHICLE =
            "You do not have a vehicle or the permission to move stock from one vehicle to another.\\n " +
            "If you want to add a vehicle, see the top banner and\\n" +
            "click here to add vehicle";
        //Tony added 2.Feb.2017 regarding Vehicle permission end

        private bool certExists = false; // Check if cert exists
        private bool certValid = true; // Check if cert is valid
        private Invoice sResults;
        private Button createCSVButton;

        //Tony added for Xero Api Connection
        //Tony added 14.1.2017
        private bool IsForMaterialChecked = false;

        protected string[] arrGeneralInfo = new string[15];
        protected string[,] arrMaterial = new string[20, 99999]; //todo if for maximum (9999)
        protected string[,] arrLabour = new string[20, 99999]; //same as above
        private DOJobContractor dojc;
        protected int intLabourCount = 0;
        protected int intMaterialCount = 0;
        protected DOEmployeeInfo Employee;
        // protected bool HasTemporaryViewOfButtons = false;
        protected bool HasTemporaryViewOfInvoices = false;
        protected DOTask Task;
        protected DOTradeCategory TradeCategory = null;
        protected DOContact contractor = new DOContact();
        protected DOContact customer = new DOContact();
        DOTaskQuote Quote = null;
        Guid? FindNewContractorID = null;
        // DOContact NewContractor = null;
        // bool NewContractorAdded = false;
        string PendingContractorEmail = null;

        bool NewTask = true;
        bool CanQuote = false;

        //Tony added 2.Feb.2017
        private CompanyPageFlag myFlags;
        private string targetURL = "VehicleInput.aspx";

        protected void Page_Init(object sender, EventArgs e)
        {
            dojc = CurrentBRJob.SelectJobContractor(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.CurrentContact.ContactID);
            //Tony added 6.1.2017
            // Tony modified 16.Feb.2017
            //            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            //            Employee = CurrentSessionContext.CurrentEmployee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            Employee = CurrentSessionContext.CurrentEmployee;

            if (Employee != null)
            {
                long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag

                //Tony commented on 2.Feb.2017 to use myFlags in other method as well 
                //                CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
                myFlags = (CompanyPageFlag)storedVal;

                // Make connection to Xero if the user has permission to see invoice
                if ((myFlags & CompanyPageFlag.ViewInvoices) == CompanyPageFlag.ViewInvoices)
                {
                    certExists = File.Exists(CertPath);

                    // if .pfx key exists in the specified path , try Xero connection
                    if (certExists)
                    {
                        connectToXero();
                    }
                    else
                    {
                        ShowMessage(CertNotExist, MessageType.Error);
                    }
                }
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
                Task = CurrentBRJob.CreateTask(CurrentSessionContext.CurrentJob.JobID, CurrentSessionContext.Owner.ContactID);
            }
            if (Task.TradeCategoryID != null)
                TradeCategory = CurrentBRTradeCategory.FindTradeCategoryName(Task.TradeCategoryID);
            if (TradeCategory == null)
                TradeCategory = new DOTradeCategory();
            contractor = CurrentBRContact.SelectContact(Task.ContractorID);
            DOContractorCustomer docc = CurrentBRContact.SelectContractorCustomerByCCID(Task.TaskCustomerID);

            //there are some old db records that need updating. The code below does that.
            if (docc == null)
            { 
                docc = CurrentBRContact.SelectContractorCustomer(CurrentSessionContext.CurrentContact.ContactID, Task.TaskCustomerID);
                Task.TaskCustomerID = docc.ContactCustomerId;
                CurrentBRJob.UpdateTask(Task);
            }
            //end of update
            customer = CurrentBRContact.SelectContact(docc.CustomerID);
            if (contractor.DisplayName == "Default User")
            {
                contractor.FirstName = "Pending";
                contractor.LastName = "User";
            }



            String strZeros = "00";
            if (CurrentSessionContext.CurrentTask.TaskNumber >= 10)
            {
                strZeros = "0";
                if (CurrentSessionContext.CurrentTask.TaskNumber >= 100)
                {
                    strZeros = "";
                }
            }

            string myString = strZeros + CurrentSessionContext.CurrentTask.TaskNumber.ToString();

            pruna.Text = "Task: " + dojc.JobNumberAuto + " " + myString;




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
            FileDisplayer1.FileList = CurrentBRJob.SelectFilesForTask(Task.TaskID);
        }

        //Tony added 6.1.2017
        //Change text of btnSend
        private void changeTextBttn()
        {
            foreach (GridViewRow GVParentR in gvContractorInvoices.Rows)
            {
                string s = gvContractorInvoices.DataKeys[GVParentR.RowIndex].Values["InvoiceID"].ToString();
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(Guid.Parse(s));

                string value = myInvoice.InvoiceStatus.ToString();
                if (value == "InProgress")
                {
                    createCSVButton = GVParentR.FindControl("btnSend") as Button;
                    createCSVButton.Text = "Create CSV";
                }
            }
        }

        private void connectToXero()
        {
            try
            {
                cert = new X509Certificate2(CertPath, CertPassword);
                xSession = new XeroApiPrivateSession("XeroConnection", ConsumerKey, cert);
                repository = new XeroApi.Repository(xSession);
                ShowMessage(XeroConSuccess, MessageType.Info);
            }
            catch (Exception e)
            {
                //If password doesn't match, display following message
                if (e.Message == "The specified network password is not correct.\r\n")
                {
                    ShowMessage(CertPasswdNotMatch, MessageType.Error);
                }
                //If cert is not valid one, display following message
                else if (e.Message == "Cannot find the requested object.\r\n")
                {
                    ShowMessage(CertPathNotExists);
                }
                else
                {
                    ShowMessage(XeroEtcExeption);
                }

                certValid = false;
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
                    if (file.ContentLength < 30000000) // !here! use variable
                    {
                        //DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Job.JobID, fileNew.PostedFile);
                        DOFileUpload File = CurrentBRJob.SaveFile(CurrentSessionContext.Owner.ContactID, Task.TaskID, file, file.ContentLength, CurrentSessionContext.CurrentContact.ContactID);
                        DOTaskFile tf = CurrentBRJob.CreateTaskFile(CurrentSessionContext.Owner.ContactID, Task.TaskID, File.FileID);
                        CurrentBRJob.SaveTaskFile(tf);
                    }
                    else
                    {
                        ShowMessage(file.FileName + " - Upload Size exceeded"); //!here! use variable
                    }
                }

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                ShowMessage(ex.Message, MessageType.Error);
            }
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
                    Response.Redirect("TaskSummary.aspx", false);
                else
                    Response.Redirect(Constants.URL_Home);

                Response.End();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            if (Employee != null)
            {
                long storedVal = Employee.AccessFlags; //get employeeinfo.accessflag
                                                       // Reinstate the mask and re-test

                //                CompanyPageFlag myFlags = (CompanyPageFlag)storedVal;
                // Tony modified 2.Feb.2017
                myFlags = (CompanyPageFlag)storedVal;
                //Jared 30.1.17
                //if ((myFlags & CompanyPageFlag.ViewImportFromButtons) == CompanyPageFlag.ViewImportFromButtons)
                //{
                //    HasTemporaryViewOfButtons = true;

                //}
                if ((myFlags & CompanyPageFlag.ViewInvoices) == CompanyPageFlag.ViewInvoices)
                {
                    HasTemporaryViewOfInvoices = true;

                }
                //if ((myFlags & CompanyPageFlag.DeleteMaterialsFromVehicle) == CompanyPageFlag.DeleteMaterialsFromVehicle) btnExport.Visible = true;
                if (HasTemporaryViewOfInvoices)
                {
                    gvContractorInvoices.Visible = true;
                    gvCustomerInvoices.Visible = true;
                    lnkCreateInvoice.Visible = true;
                    phInvoices.Visible = true;

                }
                //jared 30.1.17
                // if (HasTemporaryViewOfButtons)
                //  {
                //Button A = phAddMaterial.FindControl("btnAddFromVehicle") as Button;
                //Button B = phAddMaterial.FindControl("btnAddFromInvoice") as Button;
                //A.Visible = true;
                //B.Visible = true;
                // }
            }

            //LoadContractors();
            LoadTimeDropdowns();


            CheckTaskQuote();

            List<DOBase> Materials = CurrentBRJob.SelectTaskMaterialsList(Task.TaskID);
            gvMaterials.DataSource = Materials;
            gvMaterials.DataBind();

            List<DOBase> LabourItems = CurrentBRJob.SelectTaskLabours(Task.TaskID);
            gvLabour.DataSource = LabourItems;
            gvLabour.DataBind();

            List<DOBase> ContractorInvoices = CurrentBRJob.SelectContractorTaskInvoices(Task.TaskID, CurrentSessionContext.CurrentContact.ContactID);
            gvContractorInvoices.DataSource = ContractorInvoices;
            gvContractorInvoices.DataBind();

            List<DOBase> CustomerInvoices = CurrentBRJob.SelectCustomerTaskInvoices(Task.TaskID, CurrentSessionContext.CurrentContact.ContactID);
            gvCustomerInvoices.DataSource = CustomerInvoices;
            gvCustomerInvoices.DataBind();

            if (!IsPostBack)
            {
                LoadForm();
                //Tony Testing...
                changeTextBttn();

            }


            LoadFormPostBack();

            phNew.Visible = NewTask;
            //            lnkSave.Visible = NewTask;
            phEdit.Visible = !NewTask;
            //lnkAmendTask.Visible = !NewTask && Task.Status == DOTask.TaskStatusEnum.Incomplete;
            lnkDeleteTask.Enabled = Materials.Count == 0;
            //lnkUncompleteTask.Visible = Task.Status == DOTask.TaskStatusEnum.Complete;
            //&& Task.TaskInvoiceStatus==0;

            // lnkCompleteTask.Enabled = CurrentSessionContext.CurrentJob.JobType != DOJob.JobTypeEnum.ToQuote;

            //lnkCompleteTask.Visible = (Task.TaskType == DOTask.TaskTypeEnum.Standard || Task.TaskType == DOTask.TaskTypeEnum.Reference) && (Task.Status == DOTask.TaskStatusEnum.Incomplete);
            //cbeCompleteJob.Enabled = CurrentSessionContext.CurrentJob.JobStatus == DOJob.JobStatusEnum.Complete;

            if (Task.Status == DOTask.TaskStatusEnum.Amended || Task.Status == DOTask.TaskStatusEnum.Complete)
            {
                //lnkSave.Enabled = false;
                //lnkAmendTask.Enabled = false;
                lnkDeleteTask.Enabled = false;
                // lnkCompleteTask.Enabled = false;
                //btnAdd15.Enabled = false;
                //btnAdd60.Enabled = false;
                //btnAdd300.Enabled = false;
                btnAddLabour.Enabled = false;
                btnAddMaterial.Enabled = false;

            }
            if (Task.Status == DOTask.TaskStatusEnum.Incomplete)
            {
                btnAddLabour.Enabled = true;
                btnAddMaterial.Enabled = true;
                btnAddFromInvoice.Enabled = true;
                btnAddFromVehicle.Enabled = true;

            }
            else
            {
                //Tony commented for testing 2.Feb.2017
                //                btnAddLabour.Enabled = false;
                //                btnAddMaterial.Enabled = false;
                //                btnAddFromInvoice.Enabled = false;
                //                btnAddFromVehicle.Enabled = false;
            }
            DataBind();

            CheckAddLabourAndMaterials();
            //            LoadDropDownList();
            //Tony modified on 2.Feb.2017 
            //call following method only when user has a vehicle
            if (CheckHasVehicle())
                LoadDropDownList();

            //RenameDeleteLabels();

        }

        protected void ColourLabourLines(object sender, EventArgs e)//unused.
        {

            foreach (GridViewRow GVR in gvLabour.Rows)
            {
                Button btnDelete = GVR.FindControl("btnDeleteLabour") as Button;
                Guid TaskLabourID = (Guid)gvLabour.DataKeys[GVR.RowIndex].Values[0];
                string InvoiceStatus = gvLabour.DataKeys[GVR.RowIndex].Values[1].ToString();
                if (InvoiceStatus != "00000000-0000-0000-0000-000000000000")
                {
                    btnDelete.Enabled = false;
                    foreach (TableCell TC in GVR.Cells)
                    {
                        TC.ForeColor = System.Drawing.Color.Fuchsia;
                        TC.Enabled = false;
                    }

                }
                else
                {
                    //Tony added 17.Jan.2017
                    DOTaskLabour myTL = CurrentBRJob.SelectSingleTaskLabour(TaskLabourID);

                    if (myTL != null)
                    {
                        if (!myTL.Active)
                        {
                            foreach (TableCell TC in GVR.Cells)
                            {
                                TC.Enabled = false;
                            }
                        }
                    }

                }
            }
        }


        protected void ColourMaterialLines(object sender, EventArgs e)//unused.
        {
            ////List<DOTaskMaterialInfo> TM = gvMaterials.DataSource as DOTaskMaterialInfo;

            //Button btnDelete = e.Row.FindControl("btnDeleteMaterial") as Button;
            //string TaskMaterialID = gvMaterials.DataKeys[e.Row.RowIndex].Value.ToString();

            //DOSupplierInvoiceMaterial DOsim = CurrentBRJob.SelectSupplierInvoiceMaterialByTM(Guid.Parse(TaskMaterialID));
            //if (DOsim.OldSupplierInvoiceMaterialID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            //{
            //    //Can delete
            //}
            //else
            //{
            //    DOVehicle V = new DOVehicle();
            //    V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
            //    if (V != null) btnDelete.Text = "Move to vehicle";
            //    else btnDelete.Enabled = false;

            //}


            //            CheckBox chkAllBox = gvMaterials.HeaderRow.FindControl("chkAllOrNoneMaterial") as CheckBox;

            foreach (GridViewRow GVR in gvMaterials.Rows)
            {
                Button btnDelete = GVR.FindControl("btnDeleteMaterial") as Button;

                //Tony added 16.Jan.2017

                //                CheckBox chkBox = GVR.FindControl("chkSelectMaterial") as CheckBox;
                //
                //                if (chkAllBox.Checked && chkBox.Enabled)
                //                    chkBox.Checked = true;
                //                CheckBox chkMaterial = GVR.FindControl("chkSelectMaterial") as CheckBox;

                string TaskMaterialID = gvMaterials.DataKeys[GVR.RowIndex].Values[0].ToString();
                string InvoiceStatus = gvMaterials.DataKeys[GVR.RowIndex].Values[1].ToString();
                if (InvoiceStatus != "00000000-0000-0000-0000-000000000000")
                {
                    btnDelete.Enabled = false;
                    foreach (TableCell TC in GVR.Cells)
                    {
                        TC.ForeColor = System.Drawing.Color.Fuchsia;
                        TC.Enabled = false;
                    }

                }
                else
                {
                    //Tony added 17.Jan.2017
                    List<DOBase> TM = CurrentBRJob.SelectTaskMaterialsByTMID((Guid)gvMaterials.DataKeys[GVR.RowIndex].Values[0]);

                    if (TM.Count > 0)
                    {
                        foreach (DOTaskMaterial myTm in TM)
                        {
                            if (!myTm.Active)
                            {
                                foreach (TableCell TC in GVR.Cells)
                                {
                                    TC.Enabled = false;
                                }
                            }
                        }
                    }

                }

                DOSupplierInvoiceMaterial DOsim = CurrentBRJob.SelectSupplierInvoiceMaterialByTM(Guid.Parse(TaskMaterialID));
                if (DOsim != null)
                {
                    if (DOsim.OldSupplierInvoiceMaterialID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        //Can delete
                    }

                    else
                    { //cant delete
                        DOVehicle V = new DOVehicle();
                        V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
                        if (V != null) btnDelete.Text = "Move to vehicle";
                        else btnDelete.Enabled = false;
                    }
                }
                //DOSupplierInvoiceMaterial DOsim = CurrentBRJob.s

                //System.Diagnostics.Debug.WriteLine( GVR.DataItem.ToString());

                //need to identify the sim here


                //(DOTaskMaterialInfo)Container.DataItem).Active



                //DOSupplierInvoiceMaterial DOsim = CurrentBRJob.s

                //System.Diagnostics.Debug.WriteLine( GVR.DataItem.ToString());

                //need to identify the sim here


                //(DOTaskMaterialInfo)Container.DataItem).Active

            }
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

        //private void CheckLabourAndMaterialsVisible()
        //{
        //    //Not visible if task is new.
        //    if (Task.PersistenceStatus != ObjectPersistenceStatus.Existing)
        //    {
        //        pnlLabour.Visible = false;
        //        pnlMaterials.Visible = false;
        //        return;
        //    }
        //    //LAbour and materials are only visible if you are the task contractor or belong to their company.
        //    bool LandMVisible = false;
        //    if (Task.LMVisibility == DOTask.LMVisibilityEnum.All && Task.PersistenceStatus == ObjectPersistenceStatus.Existing)
        //    {
        //        LandMVisible = true;
        //    }
        //    phLMVisibility.Visible = false;

        //    if (CurrentSessionContext.Owner.ContactID == Task.ContractorID)
        //    {
        //        LandMVisible = true;
        //        phLMVisibility.Visible = true;
        //    }
        //    else if (CurrentBRContact.CheckCompanyContact(Task.ContractorID, CurrentSessionContext.Owner.ContactID))
        //    {
        //        LandMVisible = true;
        //        phLMVisibility.Visible = true;
        //    }

        //    pnlLabour.Visible = LandMVisible;
        //    pnlMaterials.Visible = LandMVisible;
        //}

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

            if (CurrentSessionContext.CurrentJob.JobType == DOJob.JobTypeEnum.ChargeUp)
            {
                txtLabourRate.Text = CurrentSessionContext.CurrentContact.DefaultChargeUpRate.ToString();
            }
            else
            {
                txtLabourRate.Text = CurrentSessionContext.CurrentContact.DefaultQuoteRate.ToString();
            }

        }
        protected void LoadFormPostBack()
        {
            LoadMaterials();
            LoadLabourers();

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
            //ddlStartTime.Items.Clear();
            //ddlEndTime.Items.Clear();
            ddlLabourTime.Items.Clear();

            //ddlStartTime.Items.Add(new ListItem("Not Selected", "-1"));
            //ddlEndTime.Items.Add(new ListItem("Not Selected", "-1"));

            for (int min = 0; min < 24 * 60; min += 15)
            {
                string MinText = string.Format("{0}:{1:D2}", min / 60, min % 60);
                //ddlStartTime.Items.Add(new ListItem(MinText, min.ToString()) { Selected = Task.StartMinute == min });
                //ddlEndTime.Items.Add(new ListItem(MinText, min.ToString()) { Selected = Task.EndMinute == min });
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
                //ddlStartTime.SelectedValue = Request.Form[ddlStartTime.UniqueID];
                //ddlEndTime.SelectedValue = Request.Form[ddlEndTime.UniqueID];
            }


        }

        //protected void LoadContractors()
        //{
        //    if (Task.PersistenceStatus == ObjectPersistenceStatus.New)
        //    {
        //        List<DOBase> Contractors = CurrentBRContact.SelectSubscribedContacts();
        //        List<DOBase> NewContractors = new List<DOBase>();

        //        //Get specific unsubscribed contractor details if specified.
        //        if (FindNewContractorID.HasValue && NewContractor == null)
        //        {
        //            if (FindNewContractorID == Constants.Guid_DefaultUser)
        //            {
        //                NewContractor = CurrentBRContact.GetDefaultUser(PendingContractorEmail);
        //            }
        //            else
        //            {
        //                NewContractor = CurrentBRContact.SelectContact(FindNewContractorID.Value);
        //            }
        //        }

        //        if (NewContractor != null)
        //        {
        //            NewContractors.Add(NewContractor);
        //        }

        //        // Add the 'find new contractor' contractor and any unsubscribed companies of theirs to the list.
        //        // Add companies with same email as contractor as well.
        //        if (NewContractor != null && NewContractor.ContactID != Constants.Guid_DefaultUser)
        //        {
        //            List<Guid> AddedGuids = new List<Guid>();
        //            AddedGuids.Add(NewContractor.ContactID);

        //            //Companies linked to contractor
        //            foreach (DOContact NewContractorCheck in CurrentBRContact.SelectContactCompanies(NewContractor.ContactID))
        //            {
        //                bool CanAdd = true;
        //                if (NewContractorCheck.ContactID == NewContractor.ContactID) CanAdd = false;
        //                if (NewContractorCheck.CustomerExclude) CanAdd = false;
        //                if (NewContractorCheck.Subscribed || NewContractorCheck.SubscriptionPending) CanAdd = false;
        //                if (AddedGuids.Contains(NewContractorCheck.ContactID)) CanAdd = false;

        //                if (CanAdd)
        //                {
        //                    NewContractorCheck.LastName += " (" + NewContractor.DisplayName + ")";
        //                    NewContractorCheck.CompanyName += " (" + NewContractor.DisplayName + ")";
        //                    NewContractors.Add(NewContractorCheck);
        //                    AddedGuids.Add(NewContractorCheck.ContactID);
        //                }
        //            }
        //            //Companies with same email.
        //            foreach (DOContact NewContractorCheck in CurrentBRContact.SelectContactsByEmail(NewContractor.Email))
        //            {
        //                bool CanAdd = true;
        //                if (NewContractorCheck.ContactID == NewContractor.ContactID) CanAdd = false;
        //                if (NewContractorCheck.CustomerExclude) CanAdd = false;
        //                if (NewContractorCheck.Subscribed || NewContractorCheck.SubscriptionPending) CanAdd = false;
        //                if (AddedGuids.Contains(NewContractorCheck.ContactID)) CanAdd = false;

        //                if (CanAdd)
        //                {
        //                    NewContractorCheck.LastName += " (" + NewContractor.Email + ")";
        //                    NewContractorCheck.CompanyName += " (" + NewContractor.Email + ")";
        //                    NewContractors.Add(NewContractorCheck);
        //                    AddedGuids.Add(NewContractorCheck.ContactID);
        //                }
        //            }
        //        }

        //        //If the current user is in the list of contractors, remove them.
        //        for (int i = Contractors.Count - 1; i > 0; i--)
        //        {
        //            if ((Contractors[i] as DOContact).ContactID == CurrentSessionContext.CurrentContact.ContactID)
        //            {
        //                Contractors.RemoveAt(i);
        //                break;
        //            }
        //        }

        //        Contractors.Insert(0, CurrentSessionContext.CurrentContact);

        //        //If specific contractor(s) requested by 'find new contractor', put at top of list.
        //        //if (NewContractor != null)
        //        //    Contractors.Insert(0, NewContractor);
        //        foreach (DOContact c in NewContractors)
        //        {
        //            Contractors.Insert(0, c);
        //        }

        //        ddlContractor.DataSource = Contractors;
        //        ddlContractor.DataValueField = "ContactID";
        //        ddlContractor.DataTextField = "DisplayName";
        //        ddlContractor.DataBind();

        //        if (IsPostBack)
        //        {
        //            try
        //            {
        //                if (NewContractorAdded)
        //                {
        //                    ddlContractor.SelectedValue = NewContractor.ContactID.ToString();
        //                }
        //                else
        //                {
        //                    ddlContractor.SelectedValue = GetDDLGuid(ddlContractor).ToString();
        //                }
        //            }
        //            catch { }
        //        }

        //    }
        //    else
        //    {
        //        DOContact TaskContractor = CurrentBRContact.SelectContact(Task.ContractorID);
        //        ddlContractor.Items.Clear();
        //        ddlContractor.Items.Add(new ListItem(TaskContractor.DisplayName, TaskContractor.ContactID.ToString()));
        //        ddlContractor.Enabled = false;

        //    }
        //}
        protected void LoadDropDownList()
        {
            String L = "";
            //todo this works only in your(Jared's) login, so you might need to look upon the db values or sth.
            if (IsPostBack && ddVehicleSelect.Items.Count > 0)
            {
                L = ddVehicleSelect.SelectedItem.Value;
            }
            //Jared 9.3.17 
            //List<DOBase> LineItem = CurrentBRContact.SelectCompanyContactsWithAVehicle(CurrentSessionContext.CurrentContact.ContactID);//electracraft id ECA7B55C-3971-41DA-8E84-A50DA10DD233, 
            List<DOBase> LineItem = CurrentBRContact.SelectContactVehicles(CurrentSessionContext.CurrentContact.ContactID);
            if (LineItem.Count != 0)
            {

                //dsAddress.Tables[0].Columns.Add("StreetAndPlace", typeof(string), "StreetAddress + Place");
                //lstAddressDropdown.DataSource = dsAddress;
                //lstAddressDropdown.DataTextField = "StreetAndPlace";
                //lstAddressDropdown.DataBind();
                ddVehicleSelect.DataSource = LineItem;
                ddVehicleSelect.DataTextField = "EmployeeAndVehicle";//more here
                ddVehicleSelect.DataValueField = "VehicleID";

                ddVehicleSelect.DataBind();


                if (!IsPostBack)
                {
                    //    ddVehicleSelect.SelectedValue = ddVehicleSelect.SelectedItem.Value;
                    // if session owner has no vehicle
                    // ListItem d =  ddVehicleSelect.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString());
                    if (ddVehicleSelect.Items.FindByValue(CurrentSessionContext.Owner.ContactID.ToString()) != null)
                    {
                        ddVehicleSelect.SelectedValue = CurrentSessionContext.Owner.ContactID.ToString();
                    }

                }

                else
                {
                    ddVehicleSelect.SelectedValue = L;

                }
            }

            // CurrentBRContact.SelectCompanyContacts

        }

        protected void btnDeleteMaterial_Click(object sender, EventArgs e)
        {
            Guid TMID = new Guid(((Button)sender).CommandArgument);
            DOTaskMaterial TM = CurrentBRJob.SelectSingleTaskMaterial(TMID);
            if (TM.FromInvoice || TM.FromVehicle || TM.FromCustom)
            {
                // if any deleted that are from a SIM then must be assigned to a vehicle
                DOBase currentSIM = CurrentBRJob.SelectSupplierInvoiceMaterialByTM(TM.TaskMaterialID);
                DOSupplierInvoiceMaterial DOCurrentSIM = currentSIM as DOSupplierInvoiceMaterial;

                //1.create new sim
                //2. populate with deleted details
                //reassign qty remaining.



                DOSupplierInvoiceMaterial DONewSIM = new DOSupplierInvoiceMaterial();
                DONewSIM.QtyRemainingToAssign = DOCurrentSIM.Qty;
                DONewSIM.Qty = DOCurrentSIM.Qty;
                DONewSIM.TaskMaterialID = TMID;
                DOVehicle V = CurrentBRJob.SelectVehicleByVehicleID(Guid.Parse(ddVehicleSelect.SelectedValue));
                DONewSIM.VehicleID = V.VehicleID;
                DONewSIM.SupplierInvoiceID = Guid.Parse("00000000-0000-0000-0000-000000000001"); //"deleted from task" contractorref and supplier reference// = DOCurrentSIM.SupplierInvoiceID; todo later
                DONewSIM.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                DONewSIM.SupplierInvoiceMaterialID = Guid.NewGuid();
                DONewSIM.Assigned = false;
                DONewSIM.CreatedBy = CurrentSessionContext.Owner.ContactID;
                DONewSIM.MaterialID = TM.MaterialID;


                CurrentBRJob.SaveSupplierInvoiceMaterial(DONewSIM);
                CurrentBRJob.DeleteSupplierInvoiceMaterial(DOCurrentSIM);






                //// if any deleted that are from a SIM then must be assigned to a vehicle
                //DOBase currentSIM = CurrentBRJob.SelectSupplierInvoiceMaterialByTM(TM.TaskMaterialID);
                //DOSupplierInvoiceMaterial DOCurrentSIM = currentSIM as DOSupplierInvoiceMaterial;


                ////1.create new sim
                ////2. populate with deleted details



                ////reassign qty remaining.

                //DOBase OriginalSIM = CurrentBRJob.SelectSupplierInvoiceMaterial(DOCurrentSIM.OldSupplierInvoiceMaterialID);
                //DOSupplierInvoiceMaterial DOOriginalSIM = OriginalSIM as DOSupplierInvoiceMaterial;
                //DOOriginalSIM.QtyRemainingToAssign = DOOriginalSIM.QtyRemainingToAssign + DOCurrentSIM.Qty;
                //DOOriginalSIM.Assigned = false;

                //CurrentBRJob.SaveSupplierInvoiceMaterial(DOOriginalSIM);
                //CurrentBRJob.DeleteSupplierInvoiceMaterial(DOCurrentSIM);

            }

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

        // Tony added 2.Feb.2017 to check if user has permissions regarding vehicle begin

        //Check if user has permission of "Move material from other vehicle
        bool CheckMaterialFromVehicle()
        {
            if ((myFlags & CompanyPageFlag.MoveMaterialsFromOtherVehicle) ==
                CompanyPageFlag.MoveMaterialsFromOtherVehicle)
                return true;
            return false;
        }

        string ListOfEmpWithEmployeeDetails()
        {
            List<DOEmployeeInfo> ret = new List<DOEmployeeInfo>();

            DOContact Company = null;
            Company = CurrentSessionContext.CurrentContact;

            List<DOBase> Employees = CurrentBRContact.SelectCompanyEmployees(Company.ContactID, false);

            foreach (DOBase emp in Employees)
            {
                ret.Add(emp as DOEmployeeInfo);
            }

            StringBuilder strEmp = new StringBuilder();

            foreach (DOEmployeeInfo emp in ret)
            {
                if (((CompanyPageFlag)emp.AccessFlags & CompanyPageFlag.EmployeeDetails) == CompanyPageFlag.EmployeeDetails)
                {
                    strEmp.AppendFormat("Name: {0,-20}, Email: {1,-30}\\n", emp.DisplayName, emp.Email);
                }
            }

            return strEmp.ToString();
        }


        bool CheckAddVehicle()
        {
            if ((myFlags & CompanyPageFlag.AddAndEditVehicles) == CompanyPageFlag.AddAndEditVehicles)
                return true;
            return false;
        }

        //Check if use has vehicle
        bool CheckHasVehicle()
        {
            DOVehicle V = new DOVehicle();
            //            V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
            //Tony modified 22.Feb.2017, FR Key changed from Contact.ContactID to EmployeeInfo.employeeID
            V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.CurrentEmployee.EmployeeID);

            if (V != null) return true;
            return false;
        }

        //Display popup
        protected void ShowPopup(string message)
        {
            string script = "<script type=\"text/javascript\">alert('" + message + "');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
        }

//        protected void ShowPopupRedirect(string message, string url)
//        {
//            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "err_msg", "alert('" +
//                   message + "');window.location='" + url + "';", true);
//        }
        // Tony added 2.Feb.2017 to check if user has permissions regarding vehicle end

        //Tony modified btnAddFromVehicle_Click() on 2.Feb.2017
        protected void btnAddFromVehicle_Click(object sender, EventArgs e)
        {
            if (!CheckMaterialFromVehicle() && !CheckAddVehicle() && !CheckHasVehicle())
            {
                ShowPopup(NO_VEHICLE_NO_MATERIAL_FROM_VEHICLE_NO_ADD_VEHICLE + ListOfEmpWithEmployeeDetails());
            }
            else if (!CheckHasVehicle() && !CheckMaterialFromVehicle() && CheckAddVehicle())
            {
                //Tony modified 5.Feb.2017
                ShowPopup(NO_VEHICLE_NO_MATERIAL_FROM_VEHICLE_ADD_VEHICLE);

                string linkPart = "<a href=\"" + targetURL + "\"> here </a>";
                ShowMessage("Click" + linkPart + "to create vehicle");
            }
            else if (!CheckHasVehicle() && CheckMaterialFromVehicle() && CheckAddVehicle())
            {
                string checkValue = "addVehicle";
                Response.Redirect("~/private/MaterialFromVehicle.aspx?checkValue=" + checkValue);
            }
            else
            {
                bool n = CurrentSessionContext.CurrentContact.Active;
                Response.Redirect("~/private/MaterialFromVehicle.aspx");
            }
        }

        protected void btnAddFromInvoice_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/private/MaterialFromInvoice.aspx");
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
                //1. create SIM



                //DOTaskMaterial TaskMaterial = CurrentBRJob.CreateTaskMaterial(Task.TaskID, GetDDLGuid(ddlMaterials), CurrentSessionContext.Owner.ContactID);
                TaskMaterial = CurrentBRJob.CreateTaskMaterial(Task.TaskID, MaterialID, CurrentSessionContext.Owner.ContactID);
                TaskMaterial.Quantity = GetDecimal(txtQuantity, "Material Quantity");
                TaskMaterial.InvoiceQuantity = TaskMaterial.Quantity;
                TaskMaterial.Description = txtDescription.Text;

                //Save name and price in task material entry (in case these change later).
                DOMaterial material = CurrentBRJob.SelectMaterial(MaterialID);
                TaskMaterial.MaterialName = material.MaterialName;
                TaskMaterial.SellPrice = material.SellPrice;
                TaskMaterial.FromCustom = true;
                TaskMaterial.CustomerID = CurrentSessionContext.CurrentTask.TaskCustomerID;
                TaskMaterial.ContractorID = CurrentSessionContext.CurrentContact.ContactID;

                if (TaskMaterial.Description.Length > 512)
                {
                    TaskMaterial.Description = TaskMaterial.Description.Substring(0, 512);
                    Trunc = true;
                }
                TaskMaterial.MaterialType = (TaskMaterialType)int.Parse(Request.Form[ddlMaterialType.UniqueID]);
                CurrentBRJob.SaveTaskMaterial(TaskMaterial);



                DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
                SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();                                     //As is
                SIMaterial.MaterialID = material.MaterialID;                                         //stroriginalmaterialid                    
                SIMaterial.TaskMaterialID = TaskMaterial.TaskMaterialID;

                SIMaterial.VehicleID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                SIMaterial.SupplierInvoiceID = Guid.Parse("00000000-0000-0000-0000-000000000000");      //change??
                SIMaterial.Qty = TaskMaterial.Quantity;
                SIMaterial.QtyRemainingToAssign = TaskMaterial.Quantity;
                SIMaterial.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                SIMaterial.Assigned = true;
                SIMaterial.OldSupplierInvoiceMaterialID = Guid.Parse("00000000-0000-0000-0000-000000000000");                                              //stroriginalsupplierinvoicematerialid

                //DOVehicle V = CurrentBRJob.SelectVehicleByDriverID(CurrentSessionContext.Owner.ContactID);
                //SIMaterial.VehicleID = V.VehicleID;

                CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);




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
                TaskLabour.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                TaskLabour.CustomerID = CurrentSessionContext.CurrentTask.TaskCustomerID;
                TaskLabour.InvoiceDescription = txtLabourDesc.Text;
                decimal TimeQuantity = (TaskLabour.EndMinute - TaskLabour.StartMinute);// / 60);

                TimeQuantity = TimeQuantity / 60;
                // CurrentBRJob.UpdateTaskLabourQuantity(TaskLabour.TaskLabourID, TimeQuantity);

                TaskLabour.InvoiceQuantity = TimeQuantity;
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

            if (CurrentSessionContext.CurrentContractee != null && CurrentSessionContext.CurrentCustomer != null)
            {
                bool Complete = true;
                List<DOBase> activeTasksforContact =
                        CurrentBRJob.SelectActiveTasksforContact(CurrentSessionContext.CurrentContractee.ContactID,
                            CurrentSessionContext.CurrentCustomer.ContactID, Task.JobID);
                if (activeTasksforContact.Count == 0)
                {

                    foreach (var doBase in activeTasksforContact)
                    {
                        var task = (DOTask)doBase;
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
                            DOJobContractor jobContractor = CurrentBRJob.SelectJobContractor(job.JobID,
                                CurrentSessionContext.CurrentContact.ContactID);
                            jobContractor.Status = 1;
                            CurrentBRJob.SaveJobContractor(jobContractor);
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
            Response.Redirect(Constants.URL_JobSummary);
            //Response.Redirect(Constants.URL_JobDetails);
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

        protected void btnTaskDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_TaskDetails);
        }

        //protected DOInvoice CreateInvoice(DOTask myTask, DateTime PaymentDate)
        //{//used
        //    DOInvoice myInvoice = new DOInvoice();
        //    myInvoice.InvoiceID = Guid.NewGuid();
        //    myInvoice.ContractorID = myTask.ContractorID; //todo confirm during creation of invoice
        //    myInvoice.CustomerID = myTask.TaskCustomerID; //todo confirm during creation of invoice
        //    myInvoice.DueDate = PaymentDate;
        //    myInvoice.InvoiceStatus = InvoiceStatusEnum.InProgress;
        //    myInvoice.Terms = "My Terms";
        //    myInvoice.DetailLevel = DetailLevelEnum.TotalsOnly;
        //    myInvoice.Description = "My Description";
        //    myInvoice.CreatedBy = CurrentSessionContext.Owner.ContactID;
        //    myInvoice.CreatedDate = DateTime.Now;
        //    myInvoice.Active = true;
        //    myInvoice.TaskID = myTask.TaskID;

        //    CurrentBRJob.SaveInvoice(myInvoice);

        //    return myInvoice;

        //}



        protected void preRenderContractorChild(object sender, EventArgs e)
        {//used
            foreach (GridViewRow GVParentR in gvContractorInvoices.Rows)
            {
                Label Customer = GVParentR.FindControl("lblCustomer") as Label;
                Label DateCreated = GVParentR.FindControl("lblDateCreated") as Label;
                Label DateSent = GVParentR.FindControl("lblDateSent") as Label;
                Label Status = GVParentR.FindControl("lblStatus") as Label;
                Label Amount = GVParentR.FindControl("lblAmount") as Label;

                //L.Text = InvoiceValue(Guid.Parse(GVParentR.Cells[3].Text)).ToString();
                string s = gvContractorInvoices.DataKeys[GVParentR.RowIndex].Values["InvoiceID"].ToString();
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(Guid.Parse(s));

                Amount.Text = InvoiceValue(myInvoice.InvoiceID).ToString("#.##");
                DOContractorCustomer MyCustomer = CurrentBRContact.SelectContractorCustomerByCCID(myInvoice.CustomerID);
                Guid GuidMyCustomer;
                if (MyCustomer == null)
                {
                    DOContactInfo MyCustomerContact = CurrentBRContact.SelectAContact(myInvoice.CustomerID);
                    GuidMyCustomer = MyCustomerContact.ContactID;
                }
                else
                {
                     GuidMyCustomer = MyCustomer.CustomerID;
                }

                Customer.Text = CurrentBRContact.SelectAContact(GuidMyCustomer).DisplayName;
                DateCreated.Text = myInvoice.CreatedDate.ToString("dd/MM/yy");
                if (myInvoice.SentDate.ToString() != "1900-01-01 00:00:00.000") DateSent.Text = myInvoice.SentDate.ToString("dd/MM/yy");

                string value = myInvoice.InvoiceStatus.ToString();
                if (value == "InProgress")
                {

                    Button myDeleteInvoiceButton = GVParentR.FindControl("btnDeleteInvoice") as Button;
                    myDeleteInvoiceButton.Enabled = true;

                    //Tony Added 6.1.2017
                    if (!File.Exists(CertPath))
                    {
                        createCSVButton = GVParentR.FindControl("btnSend") as Button;
                        createCSVButton.Text = "Create CSV text";
                    }
                }
                //InvoiceStatusEnum enumDisplayStatus = (InvoiceStatusEnum)value;
                //Status.Text = enumDisplayStatus.ToString();
                Status.Text = value;

                LoadgvInvoiceMaterials(myInvoice, GVParentR);
                LoadgvInvoiceLabours(myInvoice, GVParentR);

            }

        }
        protected void LoadgvInvoiceMaterials(DOInvoice myInvoice, GridViewRow myRow)
        {//used
            List<DOBase> MaterialLineItem = CurrentBRJob.SelectTaskMaterialsByInvoiceID(myInvoice.InvoiceID);
            List<DOTaskMaterialInfo> myList = new List<DOTaskMaterialInfo>();
            DOTaskMaterialInfo myTM = new DOTaskMaterialInfo();
            foreach (DOBase item in MaterialLineItem)
            {
                myTM = item as DOTaskMaterialInfo;
                myList.Add(myTM);
            }
            GridView gvContractorChildMaterials = myRow.FindControl("gvContractorInvoiceMaterials") as GridView;
            gvContractorChildMaterials.DataSource = myList;
            gvContractorChildMaterials.DataBind();
        }

        protected void PreRendergvInvoiceMaterials(object sender, EventArgs e)
        {//used
            GridView gvChild = sender as GridView;
            foreach (GridViewRow gvChildR in gvChild.Rows)
            {
                Label costprice = gvChildR.FindControl("lblCostPrice") as Label;
                TextBox quantity = gvChildR.FindControl("txtquantity") as TextBox;
                Label lblquantity = gvChildR.FindControl("lblquantity") as Label;
                Label materialdate = gvChildR.FindControl("lbldate") as Label;
                Label employee = gvChildR.FindControl("lblEmployee") as Label;
                TextBox sellprice = gvChildR.FindControl("txtSellPrice") as TextBox;
                TextBox materialdescription = gvChildR.FindControl("txtDescription") as TextBox;
                TextBox MaterialName = gvChildR.FindControl("txtMaterialName") as TextBox;
                DOTaskMaterial myTaskMaterial = CurrentBRJob.SelectSingleTaskMaterial(Guid.Parse(gvChild.DataKeys[gvChildR.RowIndex].Values[0].ToString()));
                DOMaterial myMaterial = CurrentBRJob.SelectMaterial(myTaskMaterial.MaterialID);
                sellprice.Text = myTaskMaterial.SellPrice.ToString("#.##");
                costprice.Text = myMaterial.CostPrice.ToString("#.##");
                quantity.Text = myTaskMaterial.InvoiceQuantity.ToString();
                lblquantity.Text = myTaskMaterial.Quantity.ToString();
                materialdate.Text = myTaskMaterial.CreatedDate.ToString("dd/MM/yy");
                materialdescription.Text = myTaskMaterial.Description.ToString();
                employee.Text = CurrentBRContact.SelectAContact(Guid.Parse(myTaskMaterial.CreatedBy.ToString())).DisplayName;
                MaterialName.Text = myMaterial.MaterialName;
            }
        }



        protected void LoadgvInvoiceLabours(DOInvoice myInvoice, GridViewRow myRow)
        {//used
            List<DOBase> LabourLineItem = CurrentBRJob.SelectTaskLaboursByInvoiceID(myInvoice.InvoiceID);
            List<DOTaskLabourInfo> myList = new List<DOTaskLabourInfo>();
            DOTaskLabourInfo myLM = new DOTaskLabourInfo();

            foreach (DOBase item in LabourLineItem)
            {
                myLM = item as DOTaskLabourInfo;
                myList.Add(myLM);
            }
            GridView gvContractorChildLabours = myRow.FindControl("gvContractorInvoiceLabours") as GridView;
            gvContractorChildLabours.DataSource = myList;
            gvContractorChildLabours.DataBind();

        }


        protected void PreRendergvInvoiceLabours(object sender, EventArgs e)
        {//used
            GridView gvChild = sender as GridView;
            decimal TimeQuantity;
            foreach (GridViewRow gvChildR in gvChild.Rows)
            {
                // Label costprice = gvChildR.FindControl("lblCostPrice") as Label;
                Label quantity = gvChildR.FindControl("lbllabourquantity") as Label;
                // TextBox InvoiceQuantity = gvChildR.FindControl("txtInvoiceQuantity") as TextBox;
                Label LabourDate = gvChildR.FindControl("lbllabourdate") as Label;
                Label employee = gvChildR.FindControl("lblLabourEmployee") as Label;
                TextBox LabourDescription = gvChildR.FindControl("txtlabourdesc") as TextBox;
                TextBox LabourRate = gvChildR.FindControl("txtlabourcharge") as TextBox;
                TextBox InvoicedLabourQty = gvChildR.FindControl("txtLabourQuantity") as TextBox;



                DOTaskLabour myTaskLabour = CurrentBRJob.SelectSingleTaskLabour(Guid.Parse(gvChild.DataKeys[gvChildR.RowIndex].Values[0].ToString()));
                // DOMaterial myLabour = CurrentBRJob.SelectMaterial(myTaskLabour.LabourID);
                // costprice.Text = myLabour.CostPrice.ToString("#.##");

                TimeQuantity = (myTaskLabour.EndMinute - myTaskLabour.StartMinute);// / 60);
                TimeQuantity = TimeQuantity / 60;

                if (TimeQuantity < 1) quantity.Text = TimeQuantity.ToString("0.##");
                else quantity.Text = TimeQuantity.ToString("##.##");
                InvoicedLabourQty.Text = myTaskLabour.InvoiceQuantity.ToString("##.##");
                LabourRate.Text = myTaskLabour.LabourRate.ToString("##.##");
                LabourDate.Text = myTaskLabour.LabourDate.ToString("dd/MM/yy");
                LabourDescription.Text = myTaskLabour.Description.ToString();
                employee.Text = CurrentBRContact.SelectAContact(Guid.Parse(myTaskLabour.ContactID.ToString())).DisplayName;
                if (TimeQuantity > decimal.Parse(quantity.Text))
                {
                    InvoicedLabourQty.ForeColor = System.Drawing.Color.Green;
                }
                if (TimeQuantity < decimal.Parse(quantity.Text))
                {
                    InvoicedLabourQty.ForeColor = System.Drawing.Color.Red;
                }
            }
        }




        protected decimal InvoiceValue(Guid myInvoiceID) //!
        {//used
            DOInvoice myInvoice = CurrentBRJob.SelectInvoice(myInvoiceID);
            decimal Value = 0;
            List<DOBase> MaterialLineItem = CurrentBRJob.SelectTaskMaterialsByInvoiceID(myInvoice.InvoiceID);
            //List<DOTaskMaterial> myList = new List<DOTaskMaterial>();
            DOTaskMaterialInfo myTM = new DOTaskMaterialInfo();
            foreach (DOBase item in MaterialLineItem)
            {
                myTM = item as DOTaskMaterialInfo;
                //  myList.Add(myTM);
                Value = Value + (myTM.InvoiceQuantity * myTM.SellPrice);
            }

            List<DOBase> LabourLineItem = CurrentBRJob.SelectTaskLaboursByInvoiceID(myInvoice.InvoiceID);
            //List<DOTaskLabour> myList2 = new List<DOTaskLabour>();
            DOTaskLabourInfo myLM = new DOTaskLabourInfo();
            decimal TimeQuanity;
            foreach (DOBase item in LabourLineItem)
            {
                myLM = item as DOTaskLabourInfo;
                //myList2.Add(myLM);

                Value = Value + (myLM.InvoiceQuantity * myLM.LabourRate);
            }


            return Value;
        }

        //protected void Load_gvContractorChild(object sender, GridViewRowEventArgs e)
        //{

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {

        //        string strInvoiceID = gvContractorInvoices.DataKeys[e.Row.RowIndex].Value.ToString();
        //      //  Label InvoiceTitle = e.Row.FindControl("InvoiceTitle") as Label;
        //        //DOInvoice myInvoice = CurrentBRJob.SelectInvoice(Guid.Parse(strInvoiceID));
        //        //InvoiceTitle.Text = "Invoice: " + myInvoice.CreatedDate.ToString() + " " + CurrentBRContact.SelectAContact(myInvoice.CustomerID).DisplayName;
        //        GridView gvContractorChildMaterials = e.Row.FindControl("gvContractorInvoiceMaterials") as GridView;
        //        if (gvContractorChildMaterials != null)
        //        {
        //            List<DOBase> MaterialLineItem = CurrentBRJob.SelectTaskMaterialsByInvoiceID(Guid.Parse(strInvoiceID));
        //            List<DOTaskMaterial> myList = new List<DOTaskMaterial>();
        //            DOTaskMaterial myTM = new DOTaskMaterial();
        //            foreach (DOBase item in MaterialLineItem)
        //            {


        //                myTM = item as DOTaskMaterial;
        //                myList.Add(myTM);
        //            }





        //            gvContractorChildMaterials.DataSource = myList;
        //             gvContractorChildMaterials.DataBind();
        //        }

        //        GridView gvChildLabours = e.Row.FindControl("gvContractorInvoiceLabours") as GridView;
        //        if (gvChildLabours != null)
        //        {

        //            List<DOBase> LabourLineItem = CurrentBRJob.SelectTaskLaboursByInvoiceID(Guid.Parse(strInvoiceID));
        //            List<DOTaskLabour> myList = new List<DOTaskLabour>();
        //            DOTaskLabour myLM = new DOTaskLabour();
        //            foreach (DOBase item in LabourLineItem)
        //            {
        //                myLM = item as DOTaskLabour;
        //                myList.Add(myLM);
        //            }


        //            gvChildLabours.DataSource = myList;
        //            gvChildLabours.DataBind();
        //            //e.Row.Cells[0].Visible = false;
        //        }
        //    }

        //}


        //protected void Load_gvChildLabours(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        GridView gvChild = e.Row.FindControl("gvInvoiceLabours") as GridView;
        //        string strInvoiceID = gvInvoice.DataKeys[e.Row.RowIndex].Value.ToString();
        //        List<DOBase> LabourLineItem = CurrentBRJob.SelectTaskLaboursByInvoiceID(Guid.Parse(strInvoiceID));
        //        gvChild.DataSource = LabourLineItem;
        //        gvChild.DataBind();
        //    }

        //        //}
        //        public async static void SetRequest(string mXml)
        //        {
        //            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.CreateHttp("http://dork.com/service");
        //            webRequest.Method = "POST";
        //            webRequest.Headers["SOURCE"] = "WinApp";
        //
        //            // Decide your encoding here
        //
        //            //webRequest.ContentType = "application/x-www-form-urlencoded";
        //            webRequest.ContentType = "text/xml; charset=utf-8";
        //
        //            // You should setContentLength
        //            byte[] content = System.Text.Encoding.UTF8.GetBytes(mXml);
        //            webRequest.ContentLength = content.Length;
        //
        //            var reqStream = await webRequest.GetRequestStreamAsync();
        //            reqStream.Write(content, 0, content.Length);
        //
        //            var res = await HttpWebRequest(webRequest);
        //        }

        protected void browseDirectory(object sender, EventArgs e)
        {
            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog.SelectedPath;
            }
        }

        protected void ExportInvoiceToXero(object sender, EventArgs e)
        {
            Button myButton = sender as Button;
            GridViewRow row = (GridViewRow)myButton.NamingContainer;
            if (row != null)
            {
                DOTask myTask = CurrentSessionContext.CurrentTask;
                GridView gv = row.NamingContainer as GridView;
                Guid InvoiceID = Guid.Parse(gv.DataKeys[row.RowIndex].Value.ToString());
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(InvoiceID);
                //get invoice number
                string Name = dojc.JobNumberAuto.ToString();
                string tasknumber = myTask.TaskNumber.ToString();
                // if (myTask.TaskNumber < 100) tasknumber = "0" + tasknumber;
                if (myTask.TaskNumber < 10) tasknumber = "0" + tasknumber;
                tasknumber = tasknumber + myTask.TaskNumber;


                MemoryStream ms1 = new MemoryStream();
                StreamWriter writer = new StreamWriter(ms1);
                string strScreenDump1;
                strScreenDump1 = "ContactName,EmailAddress,POAddressLine1,POAddressLine2,POAddressLine3,POAddressLine4,POCity,PORegion,POPostalCode,POCountry,InvoiceNumber,Reference,InvoiceDate,DueDate,PlannedDate,Total,TaxTotal,InvoiceAmountPaid,InvoiceAmountDue,InventoryItemCode,Description,Quantity,UnitAmount,Discount,LineAmount,AccountCode,TaxType,TaxAmount,TrackingName1,TrackingOption1,TrackingName2,TrackingOption2,Currency,Type,Sent,Status";

                strScreenDump1 = strScreenDump1 + Environment.NewLine;



                intMaterialCount = 0;
                intLabourCount = 0;

                //get TLs and TMs
                List<DOBase> MaterialLineItems = CurrentBRJob.SelectTaskMaterialsByInvoiceID(myInvoice.InvoiceID);
                List<DOBase> LabourLineItems = CurrentBRJob.SelectTaskLaboursByInvoiceID(myInvoice.InvoiceID);
                DOContractorCustomer ContractorCustomerInvoiceCustomer =null;
                DOContact ContactInvoiceCustomer = CurrentBRContact.SelectContact(myInvoice.CustomerID);
                //below. I believe we need to check if above is null and redirect to contractorcustomer.
                if (ContactInvoiceCustomer==null)
                {
                    ContractorCustomerInvoiceCustomer = CurrentBRContact.SelectContractorCustomerByCCID(myInvoice.CustomerID);

                }
                GetGeneralInfoForExport(arrGeneralInfo, ContactInvoiceCustomer, ContractorCustomerInvoiceCustomer);

                //make xero string for material
                foreach (DOTaskMaterialInfo myTaskMaterial in MaterialLineItems)
                {
                    GetMaterialsForExport(myTaskMaterial, tasknumber);
                }
                //make xero string for labour
                foreach (DOTaskLabourInfo myTaskLabour in LabourLineItems)
                {
                    GetLabourForExport(myTaskLabour);
                }

                for (int k = 0; k < 15; k++)//write general info before each material array
                {
                    strScreenDump1 = strScreenDump1 + arrGeneralInfo[k] + ", ";
                }
                for (int j = 0; j < 20; j++) //write one line item of materials to file each loop
                {
                    if (j == 5) strScreenDump1 = strScreenDump1 + "Task " + myTask.TaskNumber + ": " + myTask.TaskName + ". ";
                    strScreenDump1 = strScreenDump1 + ", ";
                }
                strScreenDump1 = strScreenDump1 + Environment.NewLine;

                for (int i = 0; i < intMaterialCount; i++)
                {

                    for (int k = 0; k < 15; k++)//write general info before each material array
                    {
                        strScreenDump1 = strScreenDump1 + arrGeneralInfo[k] + ", ";
                    }
                    for (int j = 0; j < 20; j++) //write one line item of materials to file each loop
                    {
                        strScreenDump1 = strScreenDump1 + arrMaterial[j, i] + ", ";
                    }
                    strScreenDump1 = strScreenDump1 + Environment.NewLine;
                }

                for (int i = 0; i < intLabourCount; i++)
                {
                    for (int k = 0; k < 15; k++)//write general info
                    {
                        strScreenDump1 = strScreenDump1 + arrGeneralInfo[k] + ", ";
                    }
                    for (int j = 0; j < 20; j++) //write one line item of materials to file each loop
                    {
                        strScreenDump1 = strScreenDump1 + arrLabour[j, i] + ", ";
                    }
                    strScreenDump1 = strScreenDump1 + Environment.NewLine;
                }
                //                txtScreenDump.Text = strScreenDump1;

                //Tony added : call method to send data to Xero api only when cert exists and valid
                if (certExists && certValid)
                {
                    SendToXero(strScreenDump1); //if cert exists, don't display csv data
                }
                else
                {
                    txtScreenDump.Text = strScreenDump1;
                }


                //                foreach (string eachLine in strScreenDump1.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                //                {
                //                    foreach (string eachItem in eachLine.Split(','))
                //                    {
                //                        
                //                    }
                //                }

                CurrentBRJob.UpdateTaskInvoiceNumber(myTask.TaskID, "+");
            }
        }

        //Tony added fuction for transfer to Xero
        protected void SendToXero(string csvData)
        {
            //Tony added for transferring data to Xero api
            string[] itemLine = csvData.Split(new string[] { "\n", "\r\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Invoice uInvoice = new Invoice();
            Contact uContact = new Contact();
            uInvoice.LineItems = new XeroApi.Model.LineItems();

            for (int lineNum = 1; lineNum < itemLine.Length; lineNum++)
            //            for (int lineNum = 1; lineNum < 3; lineNum++)
            {
                //                Invoice uInvoice = new Invoice();
                //                Contact uContact = new Contact();
                LineItem uLineItem = new LineItem();

                string[] item = itemLine[lineNum].Split(',');
                Address address = new Address();
                uInvoice.LineAmountTypes = LineAmountType.Exclusive; //added jared 2/1/17
                for (int itemNo = 0; itemNo < item.Length; itemNo++)
                {
                    switch (itemNo)
                    {
                        case 0:
                            uContact.Name = item[itemNo];
                            break;
                        case 1:
                            uContact.EmailAddress = item[itemNo];
                            break;
                        case 2:
                            address.AddressLine1 = item[itemNo];
                            break;
                        case 3:
                            address.AddressLine2 = item[itemNo];
                            break;
                        case 4:
                            address.AddressLine3 = item[itemNo];
                            break;
                        case 5:
                            address.AddressLine4 = item[itemNo];
                            break;
                        case 6:
                            address.City = item[itemNo];
                            break;
                        case 7:
                            address.Region = item[itemNo];
                            break;
                        case 8:
                            address.PostalCode = item[itemNo];
                            break;
                        case 9:
                            address.Country = item[itemNo];
                            break;
                        case 10:
                            uInvoice.InvoiceNumber = item[itemNo].Trim();
                            break;
                        case 11:
                            uInvoice.Reference = item[itemNo].Trim();
                            break;
                        //                        case 12:
                        //                            uInvoice.Date = DateTime.Parse(item[itemNo]);
                        //                            break;
                        case 13:
                            uInvoice.DueDate = DateTime.Parse(item[itemNo]);
                            break;
                        //                        case 14:
                        //                            //                            uInvoice.PlannedDate = DateTime.Parse(item[itemNo]);
                        //                            break;
                        //                        case 15:
                        //                            uInvoice.Total = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        //                        case 16:
                        //                            uInvoice.TotalTax = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        //                        case 17:
                        //                            uInvoice.AmountPaid = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        //                        case 18:
                        //                            uInvoice.AmountDue = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        //                        case 19:
                        //                            uItem.ItemCode = item[itemNo];
                        //                            break;
                        case 20:
                            uLineItem.Description = item[itemNo];
                            break;
                        case 21:
                            uLineItem.Quantity = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                            break;
                        case 22:
                            uLineItem.UnitAmount = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                            break;
                        //                        case 23:
                        //                            //                            uInvoice.TotalDiscount = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        //                        case 24:
                        //                            uItem.LineAmount = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        case 25:
                            uLineItem.AccountCode = "200/01"; //this works -jared, but needs to be "200" for test version of xero
                            //commented jared 2/1/17
                            //if (item[itemNo].Trim().Length >= 3)
                            //{
                            //    uLineItem.AccountCode = item[itemNo].Trim().Substring(0, 3);
                            //}
                            //else
                            //{
                            //    uLineItem.AccountCode = item[itemNo].Trim();
                            //}
                            break;
                        case 26:
                            //commented jared 2/1/17
                            //                            uLineItem.TaxType = item[itemNo].Trim();
                            //uLineItem.TaxType = "15% GST on Income";
                            break;
                        //                        case 27:
                        //                            uLineItem.TaxAmount = item[itemNo] == " " ? 0 : decimal.Parse(item[itemNo]);
                        //                            break;
                        //                        case 28:
                        //                            //TrackingName1
                        //                            break;
                        //                        case 29:
                        //                            //TrackingOption1
                        //                            break;
                        //                        case 30:
                        //                            //TrackingName2
                        //                            break;
                        //                        case 31:
                        //                            //TrackingOption2
                        //                            break;
                        //                        //Currency	Type	Sent	Status
                        //                        case 32:
                        //                            uInvoice.CurrencyCode = item[itemNo];
                        //                            break;
                        case 33:
                            if ((item[itemNo].Trim().Length == 0))
                            {
                                uInvoice.Type = "ACCREC";
                            }
                            else
                            {
                                uInvoice.Type = item[itemNo].Trim();
                            }
                            break;
                            //                        case 34:
                            //                            uInvoice.SentToContact = item[itemNo] == " " ? false : true;
                            //                            break;
                            //                        case 35:
                            //                            uInvoice.Status = item[itemNo];
                            //                            break;
                    }
                }
                //set all details of address to uContact
                addresses = new Addresses();
                addresses.Add(address);
                uInvoice.LineItems.Add(uLineItem);
            }//End of For statement

            uContact.Addresses = addresses;
            uInvoice.Contact = uContact;

            //Show error message when conumer key is invalid
            if (repository == null)
            {
                ShowMessage(ConsumerKeyInvalid, MessageType.Error);
                txtScreenDump.Text = csvData;
            }
            else
            {
                try
                {
                    sResults = repository.Create(uInvoice);

                    if (sResults.ValidationErrors.Count > 0)
                    {
                        foreach (var error in sResults.ValidationErrors)
                        {
                            errorMessage += error.Message + Environment.NewLine;
                        }
                        ShowMessage(InvoiceTransferFail + Environment.NewLine +
                            errorMessage, MessageType.Error);
                    }
                    else
                    {
                        ShowMessage(InvoiceTransferSuccess, MessageType.Info);
                    }
                }
                catch (DevDefined.OAuth.Framework.OAuthException oe)
                {
                    ShowMessage(ConsumerKeyInvalid, MessageType.Error);
                    changeTextBttn();
                    // If cosumer key is invalid display csv data
                    txtScreenDump.Text = csvData;
                }
            }
        }

        //        //local code for saving file locally
        //        string Name = CurrentSessionContext.CurrentJob.JobNumberAuto;
        //        DOContact TaskCustomer = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentTask.TaskCustomerID);
        //        FileStream fs1 = new FileStream("d:\\" + Name + ".csv", FileMode.Create, FileAccess.Write);
        //        StreamWriter writer = new StreamWriter(fs1);
        //        //Required xero header
        //        writer.WriteLine("ContactName,EmailAddress,POAddressLine1,POAddressLine2,POAddressLine3,POAddressLine4,POCity,PORegion,POPostalCode,POCountry,InvoiceNumber,Reference,InvoiceDate,DueDate,PlannedDate,Total,TaxTotal,InvoiceAmountPaid,InvoiceAmountDue,InventoryItemCode,Description,Quantity,UnitAmount,Discount,LineAmount,AccountCode,TaxType,TaxAmount,TrackingName1,TrackingOption1,TrackingName2,TrackingOption2,Currency,Type,Sent,Status");
        //        GetGeneralInfoForExport(arrGeneralInfo, TaskCustomer);

        //        //2. todo loop through where checkbox=true tasks and get that info into array(s)
        //        string tasknumber;



        //        //0. check if any data to write
        //        //1. Create invoice in db
        //        //2. populate invoice in db i.e. assign tls and tms to invoice created in 1.
        //        //3. covert invoice to file

        //        //0. Check if any data to write
        //        ////enable next 3 lines when ready to loop through all tasks in job.
        //        //List<DOBase> JobTasks = CurrentBRJob.SelectTasks(CurrentSessionContext.CurrentJob.JobID);
        //        //foreach (DOTask myTask in JobTasks)
        //        //{
        //        DateTime dt = DateTime.Today; //todo allow this to be changed
        //        DateTime PaymentDate = Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
        //        DOTask myTask = CurrentSessionContext.CurrentTask; //delete me when enable foreach task in job code
        //        // tasknumber is for making sure each filename is unique.
        //        tasknumber = myTask.TaskNumber.ToString();
        //        if (myTask.TaskNumber < 100) tasknumber = "0" + tasknumber;
        //        if (myTask.TaskNumber < 10) tasknumber = "0" + tasknumber;
        //        tasknumber = tasknumber + myTask.InvoiceNumber;
        //        List<DOBase> TaskMaterials = CurrentBRJob.SelectTaskMaterialsList(myTask.TaskID);
        //        List<DOBase> TaskLabour = CurrentBRJob.SelectTaskLabours(myTask.TaskID);
        //        if (TaskLabour.Count > 0 || TaskMaterials.Count > 0)
        //        {
        //            //1. Create invoice in db
        //            DOInvoice myInvoice = CreateInvoice(myTask, PaymentDate);
        //            //2. populate invoice in db i.e. assign tls and tms to invoice created in 1.

        //            intMaterialCount = 0;
        //            intLabourCount = 0;

        //            foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
        //            {
        //                if (myTaskMaterial.Active == true && myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                {
        //                    GetMaterialsForExport(myTaskMaterial, tasknumber);
        //                    CurrentBRJob.UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);
        //                }

        //            }
        //            System.Diagnostics.Debug.WriteLine("Material array complete " + DateTime.Now.ToString("ss.fff"));


        //            System.Diagnostics.Debug.WriteLine("Labour list complete    " + DateTime.Now.ToString("ss.fff"));
        //            foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
        //            {
        //                if (myTaskLabour.Active == true && myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                {
        //                    GetLabourForExport(myTaskLabour);
        //                    CurrentBRJob.UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);
        //                }
        //            }
        //            System.Diagnostics.Debug.WriteLine("Labour array complete   " + DateTime.Now.ToString("ss.fff"));





        //            //3. Loop through array and write to file
        //            for (int i = 0; i < intMaterialCount; i++)
        //            {

        //                for (int k = 0; k < 15; k++)//write general info before each material array
        //                {
        //                    //if (k == 10)arrGeneralInfo[k] = arrGeneralInfo[k] +
        //                    writer.Write(arrGeneralInfo[k] + ", ");

        //                    //System.Diagnostics.Debug.Write(arrGeneralInfo[k] + ", ");
        //                }
        //                for (int j = 0; j < 21; j++) //write one line item of materials to file each loop
        //                {
        //                    writer.Write(arrMaterial[j, i] + ", ");
        //                    //System.Diagnostics.Debug.Write(arrMaterial[j, i] + ", ");
        //                }

        //                //add an extra
        //                writer.WriteLine("");
        //                //System.Diagnostics.Debug.WriteLine("");
        //            }
        //            //writer.WriteLine("");
        //            //System.Diagnostics.Debug.WriteLine("");




        //            for (int i = 0; i < intLabourCount; i++)
        //            {
        //                for (int k = 0; k < 15; k++)//write general info
        //                {
        //                    writer.Write(arrGeneralInfo[k] + ", ");
        //                    //System.Diagnostics.Debug.Write(arrGeneralInfo[k] + ", ");
        //                }
        //                for (int j = 0; j < 21; j++) //write one line item of materials to file each loop
        //                {
        //                    writer.Write(arrLabour[j, i] + ", ");
        //                    // System.Diagnostics.Debug.Write(arrLabour[j, i] + ", ");
        //                }
        //                writer.WriteLine("");
        //                // System.Diagnostics.Debug.WriteLine("");
        //            }
        //            //writer.WriteLine("");
        //            //System.Diagnostics.Debug.WriteLine("");


        //            // } todo - reinstate this when ready to loop through all tasks in the job

        //            //writer.WriteLine("Hello Welcome");
        //            writer.Close();
        //            System.Diagnostics.Debug.WriteLine("File Written            " + DateTime.Now);

        //            //this code segment read data from the file.
        //            //FileStream fs2 = new FileStream("D:\\Yourfile.txt", FileMode.OpenOrCreate, FileAccess.Read);
        //            //StreamReader reader = new StreamReader(fs2);
        //            //string s = reader.ReadToEnd();
        //            //System.Diagnostics.Debug.WriteLine(s);
        //            //reader.Close();
        //        }

        //}

        //Tony added 17.Jan.2017
        protected Guid[] ListOfMaterialID()
        {
            List<Guid> listOfGuid = new List<Guid>();

            foreach (GridViewRow GVR in gvMaterials.Rows)
            {
                string taskMaterialID = gvMaterials.DataKeys[GVR.RowIndex].Values[0].ToString();

                CheckBox chkBox = GVR.FindControl("chkSelectMaterial") as CheckBox;

                if (chkBox != null)
                {
                    if (chkBox.Checked)
                    {
                        listOfGuid.Add(new Guid(taskMaterialID));
                    }
                }
            }
            Guid[] returnGuids = new Guid[listOfGuid.Count];

            int i = 0;

            foreach (Guid id in listOfGuid)
            {
                returnGuids[i++] = id;
            }
            return returnGuids;
        }

        protected Guid[] ListOfLabourID()
        {
            List<Guid> listOfGuid = new List<Guid>();

            foreach (GridViewRow GVR in gvLabour.Rows)
            {
                string taskLabourID = gvLabour.DataKeys[GVR.RowIndex].Values[0].ToString();

                CheckBox chkBox = GVR.FindControl("chkSelectLabour") as CheckBox;

                if (chkBox != null)
                {
                    if (chkBox.Checked)
                    {
                        listOfGuid.Add(new Guid(taskLabourID));
                    }
                }
            }
            Guid[] returnGuids = new Guid[listOfGuid.Count];

            int i = 0;

            foreach (Guid id in listOfGuid)
            {
                returnGuids[i++] = id;
            }
            return returnGuids;
        }

        protected void CreateInvoice(object sender, EventArgs e)
        {//used

            DateTime PaymentDate = DateTime.Today;      //Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);
            DOTask myTask = CurrentSessionContext.CurrentTask; //delete me when enable foreach task in job code

            Guid[] materialIDs = ListOfMaterialID();
            Guid[] labourIDs = ListOfLabourID();

            //call CurrentBRJob.CreateInvoice only when the count of materialIDs or labourIDs larger than 0
            if (materialIDs.Length > 0 || labourIDs.Length > 0)
                CurrentBRJob.CreateInvoice(myTask, CurrentSessionContext.Owner, PaymentDate, materialIDs, labourIDs);

            //    //DateTime dt = DateTime.Today; //todo allow this to be changed
            //  //below is not working on the live site. There are two commands on this page that are not working live, but are local

            //    //DateTime PaymentDate = Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1);


            //    List<DOBase> TaskMaterials = CurrentBRJob.SelectTaskMaterialsListNotInvoiced(myTask.TaskID);
            //    List<DOBase> TaskLabour = CurrentBRJob.SelectTaskLaboursNotInvoiced(myTask.TaskID);
            //if (TaskLabour.Count > 0 || TaskMaterials.Count > 0)
            //{
            //    DOInvoice myInvoice = CurrentBRJob.SaveInvoice(myTask, PaymentDate, CurrentSessionContext.Owner);
            //    intMaterialCount = 0;
            //    intLabourCount = 0;
            //    foreach (DOTaskMaterialInfo myTaskMaterial in TaskMaterials)
            //    {
            //        if (myTaskMaterial.Active == true && myTaskMaterial.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            //        {
            //            CurrentBRJob.UpdateTMInvoiceID(myTaskMaterial.TaskMaterialID, myInvoice.InvoiceID);

            //        }
            //    }
            //    foreach (DOTaskLabourInfo myTaskLabour in TaskLabour)
            //    {

            //        if (myTaskLabour.Active == true && myTaskLabour.InvoiceID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            //        {
            //            CurrentBRJob.UpdateTLInvoiceID(myTaskLabour.TaskLabourID, myInvoice.InvoiceID);


            //        }
            //    }
            //}
        }







        public void GetGeneralInfoForExport(string[] arrGetGeneralInfo, DOContact CExportCustomer, DOContractorCustomer CCExportCustomer)
        {//used
            DOTask ExportTask = CurrentSessionContext.CurrentTask;
            DOJob ExportJob = CurrentSessionContext.CurrentJob;
            DOSite ExportSite = CurrentSessionContext.CurrentSite;


            //DOContact ExportCustomer = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentTask.TaskCustomerID);
            string now = DateTime.Today.ToString("dd/MM/yyyy");
            string InvoiceNumber = "";


            string TaskNumber = CurrentSessionContext.CurrentTask.TaskNumber.ToString();
            // if (myTask.TaskNumber < 100) tasknumber = "0" + tasknumber;
            if (CurrentSessionContext.CurrentTask.TaskNumber < 10) TaskNumber = "0" + TaskNumber;
            TaskNumber = TaskNumber + CurrentSessionContext.CurrentTask.TaskNumber;


            //ContactName,EmailAddress,POAddressLine1,POAddressLine2,POAddressLine3,POAddressLine4,POCity,PORegion,POPostalCode,POCountry,InvoiceNumber,Reference,InvoiceDate,
            //DueDate,PlannedDate,
            // if/else statement below is because there has been a db update. exsiting records before update pointed to contact. 
            // new records point to contractorcustomer
            if (CExportCustomer != null)
            {
                arrGetGeneralInfo[0] = CExportCustomer.DisplayName;
                arrGetGeneralInfo[1] = CExportCustomer.Email;
                arrGetGeneralInfo[2] = CExportCustomer.Address1;
                arrGetGeneralInfo[3] = "";
                arrGetGeneralInfo[4] = "";
                arrGetGeneralInfo[5] = CExportCustomer.Address2;
                arrGetGeneralInfo[6] = CExportCustomer.Address3;
                arrGetGeneralInfo[7] = CExportCustomer.Address4;
                arrGetGeneralInfo[8] = "";
                arrGetGeneralInfo[9] = "New Zealand"; //pocountry
                arrGetGeneralInfo[10] = dojc.JobNumberAuto.ToString() + '-' + TaskNumber + ExportTask.InvoiceNumber; //invoicenumber - delete now once running
                arrGetGeneralInfo[11] = ExportSite.Address1 + "-" + ExportSite.Address2; // reference
                arrGetGeneralInfo[12] = now; //invoice date
            }
            else
            {
                if (CCExportCustomer.Email == null) CCExportCustomer.Email = "";
                arrGetGeneralInfo[0] = CCExportCustomer.DisplayName;
                arrGetGeneralInfo[1] = CCExportCustomer.Email;
                arrGetGeneralInfo[2] = CCExportCustomer.Address1;
                arrGetGeneralInfo[3] = "";
                arrGetGeneralInfo[4] = "";
                arrGetGeneralInfo[5] = CCExportCustomer.Address2;
                arrGetGeneralInfo[6] = CCExportCustomer.Address3;
                arrGetGeneralInfo[7] = CCExportCustomer.Address4;
                arrGetGeneralInfo[8] = "";
                arrGetGeneralInfo[9] = "New Zealand"; //pocountry
                arrGetGeneralInfo[10] = dojc.JobNumberAuto.ToString() + '-' + TaskNumber + ExportTask.InvoiceNumber; //invoicenumber - delete now once running
                arrGetGeneralInfo[11] = ExportSite.Address1 + "-" + ExportSite.Address2; // reference
                arrGetGeneralInfo[12] = now; //invoice date
            }

            DateTime dt = DateTime.Today;
            //below is not working on the live site. There are two commands on this page that are not working live, but are local
            DateTime PaymentDate = DateTime.Today;//Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1); 
            //DateTime PaymentDate = Convert.ToDateTime(dt.ToString("20/MM/yyyy")).AddMonths(1); 


            arrGetGeneralInfo[13] = PaymentDate.ToString("dd/MM/yyyy"); //todo 20th of the following/whatever the customer chooses

            arrGetGeneralInfo[14] = ""; //planned date wtf???

            string NoCommas;

            for (int i = 0; i < 15; i++)
            {
                NoCommas = ""; //make sure no commas in the text because the output needs to be comma delimited...
                foreach (char c in arrGetGeneralInfo[i])
                {
                    if (c == ',')
                    {
                        NoCommas = NoCommas + '.';
                    }
                    else
                    {
                        if (c != '\r')//cr
                        {
                            if (c != '\n')//lf
                            {
                                NoCommas = NoCommas + c;
                            }
                        }
                    }
                }
                arrGetGeneralInfo[i] = NoCommas;
            }



        }

        public void btnDeleteInvoice(object sender, EventArgs e)
        {//used

            //Guid TMID = new Guid(((Button)sender).CommandArgument);
            //DOTaskMaterial TM = CurrentBRJob.SelectSingleTaskMaterial(TMID);

            Button myButton = sender as Button;
            GridViewRow row = (GridViewRow)myButton.NamingContainer;
            if (row != null)
            {
                GridView gv = row.NamingContainer as GridView;
                Guid InvoiceID = Guid.Parse(gv.DataKeys[row.RowIndex].Value.ToString());
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(InvoiceID);
                //delete invoice tms and tls
                CurrentBRJob.UpdateTLInvoiceIDToZero(InvoiceID);
                CurrentBRJob.UpdateTMInvoiceIDToZero(InvoiceID);
                CurrentBRJob.DeleteInvoice(myInvoice);
            }

        }


        public void GetMaterialsForExport(DOTaskMaterialInfo myTaskMaterial, string TaskNumber)
        {//used
         //Total,TaxTotal,InvoiceAmountPaid,InvoiceAmountDue,InventoryItemCode,Description,Quantity,UnitAmount,Discount,LineAmount,AccountCode,TaxType,
         //TaxAmount,TrackingName1,TrackingOption1,TrackingName2,TrackingOption2,Currency,Type,Sent,Status
            DOTask myTask = CurrentSessionContext.CurrentTask;

            //if (myTaskMaterial.Active = true && myTaskMaterial.InvoiceStatus==0)
            {
                //CurrentBRJob.UpdateTaskMaterialInvoiceStatus(myTaskMaterial.TaskMaterialID, 1);
                arrMaterial[0, intMaterialCount] = "";
                arrMaterial[1, intMaterialCount] = "";
                arrMaterial[2, intMaterialCount] = "";
                arrMaterial[3, intMaterialCount] = "";
                arrMaterial[4, intMaterialCount] = "";
                arrMaterial[5, intMaterialCount] = myTaskMaterial.CreatedDate.ToString("dd/MM/yyyy") + "-" + myTaskMaterial.MaterialName + ". " + myTaskMaterial.Description; //description. make sure no commas in the text...
                arrMaterial[6, intMaterialCount] = myTaskMaterial.InvoiceQuantity.ToString();
                arrMaterial[7, intMaterialCount] = myTaskMaterial.SellPrice.ToString();
                arrMaterial[8, intMaterialCount] = ""; //discount
                arrMaterial[9, intMaterialCount] = ""; //lineamount
                arrMaterial[10, intMaterialCount] = "200/01"; //accountcode
                arrMaterial[11, intMaterialCount] = "15% GST on Income"; //tax type
                arrMaterial[12, intMaterialCount] = "";
                arrMaterial[13, intMaterialCount] = "";
                arrMaterial[14, intMaterialCount] = "";
                arrMaterial[15, intMaterialCount] = "";
                arrMaterial[16, intMaterialCount] = "";
                arrMaterial[17, intMaterialCount] = "";
                arrMaterial[18, intMaterialCount] = "";
                arrMaterial[19, intMaterialCount] = "";



                string NoCommas;

                for (int i = 0; i < 20; i++)
                {
                    NoCommas = ""; //make sure no commas, LF or CR in the text...
                    foreach (char c in arrMaterial[i, intMaterialCount])
                    {
                        if (c == ',')
                        {
                            NoCommas = NoCommas + '.';
                        }
                        else
                        {
                            if (c != '\r')//cr
                            {
                                if (c != '\n')//lf
                                {
                                    NoCommas = NoCommas + c;
                                }
                            }
                        }
                    }
                    arrMaterial[i, intMaterialCount] = NoCommas;
                }
                intMaterialCount++;
            }


        }
        public void gvDeleteContractorRow(object sender, GridViewDeleteEventArgs e)
        {

            //GridViewRow myRow = (GridViewRow)gvContractorInvoices.Rows[e.RowIndex];
            //Guid InvoiceID = new Guid(((Button)sender).CommandArgument);

            //Label lbldeleteid = (Label)row.FindControl("lblID");
            //conn.Open();
            //SqlCommand cmd = new SqlCommand("delete FROM detail where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", conn);
            //cmd.ExecuteNonQuery();
            //conn.Close();gvContractorInvoiceMaterials_RowEditing
            //gvbind();


        }
        public void gvContractorInvoiceMaterials_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }




        public void GetLabourForExport(DOTaskLabourInfo myTaskLabour)
        {//used
            //Total,TaxTotal,InvoiceAmountPaid,InvoiceAmountDue,InventoryItemCode,Description,Quantity,UnitAmount,Discount,LineAmount,AccountCode,TaxType,
            //TaxAmount,TrackingName1,TrackingOption1,TrackingName2,TrackingOption2,Currency,Type,Sent,Status
            DOTask myTask = CurrentSessionContext.CurrentTask;

            //if (myTaskLabour.Active = true && myTaskLabour.QuoteStatus==0)
            {
                DOContact LabourOwner = CurrentBRContact.SelectContact(myTaskLabour.ContactID);
                //CurrentBRJob.UpdateTaskLabourInvoiceStatus(myTaskLabour.TaskLabourID, 1);

                arrLabour[0, intLabourCount] = "";
                arrLabour[1, intLabourCount] = "";
                arrLabour[2, intLabourCount] = "";
                arrLabour[3, intLabourCount] = "";
                arrLabour[4, intLabourCount] = "";
                arrLabour[5, intLabourCount] = myTaskLabour.LabourDate.ToString("dd/MM/yyyy") + "-" + LabourOwner.DisplayName + ". " + myTaskLabour.Description;
                //decimal Hours = ((decimal.Parse( myTaskLabour.EndMinute.ToString()) - decimal.Parse(myTaskLabour.StartMinute.ToString())) / 60);
                arrLabour[6, intLabourCount] = myTaskLabour.InvoiceQuantity.ToString(); // Hours.ToString();
                arrLabour[7, intLabourCount] = "65";
                arrLabour[8, intLabourCount] = ""; //discount
                arrLabour[9, intLabourCount] = ""; //lineamount
                arrLabour[10, intLabourCount] = "200/01"; //tax type
                arrLabour[11, intLabourCount] = "15% GST on Income";
                arrLabour[12, intLabourCount] = "";
                arrLabour[13, intLabourCount] = "";
                arrLabour[14, intLabourCount] = "";
                arrLabour[15, intLabourCount] = "";
                arrLabour[16, intLabourCount] = "";
                arrLabour[17, intLabourCount] = "";
                arrLabour[18, intLabourCount] = "";
                arrLabour[19, intLabourCount] = "";
                //System.Diagnostics.Debug.WriteLine("Labour array filled     " + DateTime.Now.ToString("ss.fff"));
                string NoCommas;

                for (int i = 0; i < 20; i++)
                {
                    NoCommas = ""; //make sure no commas, LF or CR in the text...
                    foreach (char c in arrLabour[i, intLabourCount])
                    {
                        if (c == ',')
                        {
                            NoCommas = NoCommas + '.';
                        }
                        else
                        {
                            if (c != '\r')//cr
                            {
                                if (c != '\n')//lf
                                {
                                    NoCommas = NoCommas + c;
                                }
                            }
                        }
                    }
                    arrLabour[i, intLabourCount] = NoCommas;
                }


                intLabourCount++;

            }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {//used


            TextBox t = gvContractorInvoices.Rows[0].FindControl("mytb") as TextBox;
            Button myButton = sender as Button;
            GridViewRow row = (GridViewRow)myButton.NamingContainer;
            if (row != null)
            {
                GridView gv = row.NamingContainer as GridView;

                Guid InvoiceID = Guid.Parse(gv.DataKeys[row.RowIndex].Value.ToString());
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(InvoiceID);

                //save materials
                GridView gvMaterials = gv.Rows[row.RowIndex].FindControl("gvContractorInvoiceMaterials") as GridView;
                foreach (GridViewRow gvR in gvMaterials.Rows)
                {
                    //need TaskMaterialID
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {
                        //Retreiving the GridView DataKey Value
                        Guid myTaskMaterialID = Guid.Parse(gvMaterials.DataKeys[gvR.RowIndex].Value.ToString());
                        TextBox Quantity = gvR.FindControl("txtQuantity") as TextBox;
                        TextBox SellPrice = gvR.FindControl("txtsellprice") as TextBox;
                        TextBox MaterialName = gvR.FindControl("txtmaterialname") as TextBox;
                        TextBox Description = gvR.FindControl("txtdescription") as TextBox;
                        DOTaskMaterial myTaskMaterial = CurrentBRJob.SelectSingleTaskMaterial(myTaskMaterialID);
                        CurrentBRJob.UpdateTM(myTaskMaterialID, decimal.Parse(Quantity.Text), decimal.Parse(SellPrice.Text), MaterialName.Text, Description.Text);

                        //here021116
                    }
                }



                //save labours
                GridView gvLabours = gv.Rows[row.RowIndex].FindControl("gvContractorInvoiceLabours") as GridView;
                foreach (GridViewRow gvR in gvLabours.Rows)
                {
                    //need TaskLabourID
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {
                        //Retreiving the GridView DataKey Value
                        Guid myTaskLabourID = Guid.Parse(gvLabours.DataKeys[gvR.RowIndex].Value.ToString());

                        TextBox Quantity = gvR.FindControl("txtlabourQuantity") as TextBox;
                        TextBox LabourCharge = gvR.FindControl("txtLabourCharge") as TextBox;
                        TextBox LabourDescription = gvR.FindControl("txtlabourdesc") as TextBox;
                        DOTaskLabour myTaskLabour = CurrentBRJob.SelectSingleTaskLabour(myTaskLabourID);
                        myTaskLabour.InvoiceDescription = LabourDescription.Text;
                        CurrentBRJob.UpdateInvoiceTL(myTaskLabourID, decimal.Parse(Quantity.Text), decimal.Parse(LabourCharge.Text), LabourDescription.Text);

                    }
                }
                InvoiceValue(InvoiceID);

            }


        }



        protected void btnStatusUp_Click(object sender, EventArgs e)
        {
            Button myButton = sender as Button;
            GridViewRow row = (GridViewRow)myButton.NamingContainer;
            if (row != null)
            {
                GridView gv = row.NamingContainer as GridView;
                Guid InvoiceID = Guid.Parse(gv.DataKeys[row.RowIndex].Value.ToString());
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(InvoiceID);
                CurrentBRJob.UpdateInvoiceStatus(myInvoice.InvoiceID, "+");
            }
        }

        protected void btnStatusDown_Click(object sender, EventArgs e)
        {
            Button myButton = sender as Button;
            GridViewRow row = (GridViewRow)myButton.NamingContainer;
            if (row != null)
            {
                GridView gv = row.NamingContainer as GridView;
                Guid InvoiceID = Guid.Parse(gv.DataKeys[row.RowIndex].Value.ToString());
                DOInvoice myInvoice = CurrentBRJob.SelectInvoice(InvoiceID);
                CurrentBRJob.UpdateInvoiceStatus(myInvoice.InvoiceID, "-");
            }
        }
    }


}