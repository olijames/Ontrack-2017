using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.Utility;
using System.Web;
using Electracraft.Framework.BusinessRules;
using System.Reflection;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Framework.Web
{
    public class PageBase : System.Web.UI.Page
    {
        public enum MessageType
        {
            None,
            Error,
            Info,
            Warning
        }

        public const string LoginURL = "~/Default.aspx";

        public SessionContext CurrentSessionContext { get; private set; }
        private BRContact _CurrentBRContact;
        public BRContact CurrentBRContact
        {
            get
            {
                if (_CurrentBRContact == null)
                    _CurrentBRContact = new BRContact();
                return _CurrentBRContact;
            }

        }

        private BRSite _CurrentBRSite;
        public BRSite CurrentBRSite
        {
            get
            {
                if (_CurrentBRSite == null)
                    _CurrentBRSite = new BRSite();
                return _CurrentBRSite;
            }
        }

        private BROperatingSites _CurrentBROperatingSites;
        public BROperatingSites CurrentBROperatingSites
        {
            get
            {
                if (_CurrentBROperatingSites == null)
                    _CurrentBROperatingSites = new BROperatingSites();
                return _CurrentBROperatingSites;
            }
        }
        private BRContactTradeCategory _CurrentBRContactTradecategory;
        public BRContactTradeCategory CurrentBRContactTradecategory
        {
            get
            {
                if (_CurrentBRContactTradecategory == null)
                    _CurrentBRContactTradecategory = new BRContactTradeCategory();
                return _CurrentBRContactTradecategory;
            }
        }
        private BRTradeCategory _CurrentBRTradecategory;
        public BRTradeCategory CurrentBRTradeCategory
        {
            get
            {
                if (_CurrentBRTradecategory == null)
                    _CurrentBRTradecategory = new BRTradeCategory();
                return _CurrentBRTradecategory;
            }
        }
        private BRSuburb _CurrentBRSuburb;
        public BRSuburb CurrentBRSuburb
        {
            get
            {
                if (_CurrentBRSuburb == null)
                    _CurrentBRSuburb = new BRSuburb();
                return _CurrentBRSuburb;
            }
        }
        private BRDistrict _CurrentBRDistrict;
        public BRDistrict CurrentBRDistrict
        {
            get
            {
                if (_CurrentBRDistrict == null)
                    _CurrentBRDistrict = new BRDistrict();
                return _CurrentBRDistrict;
            }
        }

        private BRRegion _CurrentBRRegion;
        public BRRegion CurrentBRRegion
        {
            get
            {
                if (_CurrentBRRegion == null)
                    _CurrentBRRegion = new BRRegion();
                return _CurrentBRRegion;
            }
        }

        private BRJob _CurrentBRJob;
        public BRJob CurrentBRJob
        {
            get
            {
                if (_CurrentBRJob == null)
                    _CurrentBRJob = new BRJob();
                return _CurrentBRJob;
            }
        }

        private BRGeneral _CurrentBRGeneral;
        public BRGeneral CurrentBRGeneral
        {
            get
            {
                if (_CurrentBRGeneral == null)
                    _CurrentBRGeneral = new BRGeneral();
                return _CurrentBRGeneral;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            //Check for user session.
            try
            {
                CurrentSessionContext = Session[Constants.SessionCurrentContext] as SessionContext;
                if (CurrentSessionContext == null)
                {
                    CurrentSessionContext = new SessionContext(null);
                    Session[Constants.SessionCurrentContext] = CurrentSessionContext;
                }

                if (CurrentSessionContext.Owner == null)
                {
                    //Check for persistent login cookie.
                    HttpCookie CookieUserID = Request.Cookies[Constants.CookieUserID];
                    HttpCookie CookiePasswordHash = Request.Cookies[Constants.CookiePasswordHash];

                    if (CookieUserID != null && CookieUserID.Value != string.Empty &&
                        CookiePasswordHash != null && CookiePasswordHash.Value != string.Empty)
                    {
                        try
                        {
                            Guid UserID = new Guid(CookieUserID.Value);
                            Login(UserID, CookiePasswordHash.Value);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                    }

                }

                //Check if this page is private.
                object[] PrivatePageAttributeArray = this.GetType().GetCustomAttributes(typeof(PrivatePageAttribute), true);
                if (PrivatePageAttributeArray.Length > 0)
                {
                    if (CurrentSessionContext.Owner == null)
                    {
                        string ThisPageURL = Server.UrlEncode(Request.Url.PathAndQuery);
                        Response.Redirect(string.Format("{0}?returnurl={1}", LoginURL, ThisPageURL));
                    }
                    if (PrivatePageAttributeArray[0] is AdminPageAttribute)
                    {
                        if (CurrentSessionContext.Owner.ContactID != Guid.Empty)
                        {
                            Response.Redirect(Utility.Constants.URL_Home);
                        }
                    }
                }

                base.OnInit(e);
            }
            catch (System.StackOverflowException so)
            {
                so.StackTrace.ToString();
            }
        }


        protected void Login(Guid UserID, string PasswordHash)
        {
            Login(UserID, PasswordHash, true);
        }

        protected void Login(Guid UserID, string PasswordHash, bool RememberMe)
        {
            try
            {
                CurrentSessionContext = CurrentBRContact.AuthenticateUserFromCookie(UserID, PasswordHash);
                PostLogin(RememberMe);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void Login(string Username, string Password, bool RememberMe)
        {
            try
            {
                CurrentSessionContext = CurrentBRContact.AuthenticateUserFromForm(Username, Password);
                PostLogin(RememberMe);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }


        protected void PostLogin(bool RememberMe)
        {

            if (CurrentSessionContext == null)
            {
                CurrentSessionContext = new SessionContext(null);
            }
            if (CurrentSessionContext.Owner == null) return;

            if (CurrentSessionContext.Owner.PendingUser)
            {
                //Send email to inviter of this user.
                List<DOBase> CIs = CurrentBRContact.SelectCustomerInvitations(CurrentSessionContext.Owner.ContactID);
                foreach (DOCustomerInvitation CI in CIs)
                {
                    DOContact Inviter = CurrentBRContact.SelectContact(CI.InviterID);

                    string Subject = "Your customer has accepted your invitation - OnTrack";
                    string Body = string.Format("The customer that you invited to OnTrack - {0} - has accepted your invitation and is now an OnTrack User.", CurrentSessionContext.Owner.DisplayName);
                    CurrentBRGeneral.SendEmail(Constants.EmailSender, Inviter.Email, Subject, Body);

                    CurrentBRContact.DeleteCustomerInvitation(CI);
                }

                CurrentSessionContext.Owner.PendingUser = false;
                CurrentBRContact.SaveContact(CurrentSessionContext.Owner);
            }

            Session[Constants.SessionCurrentContext] = CurrentSessionContext;

            HttpCookie CookieUserID = new HttpCookie(Constants.CookieUserID, CurrentSessionContext.Owner.ContactID.ToString());
            HttpCookie CookiePasswordHash = new HttpCookie(Constants.CookiePasswordHash, CurrentSessionContext.Owner.PasswordHash);

            if (RememberMe)
            {
                CookieUserID.Expires = DateAndTime.GetCurrentDateTime().AddDays(Constants.CookieUsernameExpiryDays);
                CookiePasswordHash.Expires = DateAndTime.GetCurrentDateTime().AddDays(Constants.CookieUsernameExpiryDays);
            }

            Response.Cookies.Add(CookieUserID);
            Response.Cookies.Add(CookiePasswordHash);
        }

        protected void Logout()
        {
            HttpCookie CookieUserID = new HttpCookie(Constants.CookieUserID, string.Empty);
            HttpCookie CookiePasswordHash = new HttpCookie(Constants.CookiePasswordHash, string.Empty);

            CookieUserID.Expires = DateAndTime.GetCurrentDateTime().AddYears(-1);
            CookiePasswordHash.Expires = DateAndTime.GetCurrentDateTime().AddYears(-1);

            Response.Cookies.Add(CookieUserID);
            Response.Cookies.Add(CookiePasswordHash);

            CurrentSessionContext = new SessionContext(null);
            Session[Constants.SessionCurrentContext] = CurrentSessionContext;
        }

        public void ShowMessage(string Message)
        {
            ShowMessage(Message, MessageType.None);
        }

        public void ShowMessage(string Message, MessageType mType)
        {
            MasterPageBase Master = this.Master as MasterPageBase;
            if (Master != null)
                Master.ShowMessage(Message, mType);
        }


        public Guid GetDDLGuid(System.Web.UI.WebControls.DropDownList ddl)
        {
            return new Guid(Request.Form[ddl.UniqueID]);
        }

        public void SetDDLValuePostBack(System.Web.UI.WebControls.DropDownList ddl)
        {
            if (!string.IsNullOrEmpty(Request.Form[ddl.UniqueID]))
                ddl.SelectedValue = Request.Form[ddl.UniqueID];
        }

        public decimal GetDecimal(System.Web.UI.WebControls.TextBox txt, string FieldName)
        {
            if (string.IsNullOrEmpty(txt.Text)) return 0;
            try
            {
                return decimal.Parse(txt.Text);
            }
            catch
            {
                throw new FieldValidationException(FieldName + " must be a valid number.");
            }
        }

        #region Session variable clearing
        protected void ClearTask()
        {
            CurrentSessionContext.CurrentTask = null;
        }

        protected void ClearJob()
        {
            ClearTask();
            //ClearTimeSheets();
            CurrentSessionContext.AmendedTaskIDs = null;
            CurrentSessionContext.CurrentJob = null;
        }

        //protected void ClearTimeSheets()
        //{
        //    CurrentSessionContext.CurrentTimeSheet = null;
        //    CurrentSessionContext.CurrentTimeSheetAdminMode = false;
        //}

        protected void ClearSite()
        {
            ClearJob();
            CurrentSessionContext.CurrentSite = null;
        }

        protected void ClearCustomer()
        {
            ClearSite();
            CurrentSessionContext.CurrentCustomer = null;
            CurrentSessionContext.CurrentContractee = null;
            CurrentSessionContext.LastContactPageType = Electracraft.Framework.Utility.SessionContext.LastContactPageTypeEnum.None;
        }

        protected void ClearContact()
        {
            ClearSite();
            CurrentSessionContext.CurrentContact = null;
            CurrentSessionContext.LastContactPageType = Electracraft.Framework.Utility.SessionContext.LastContactPageTypeEnum.None;
        }

        protected void ClearCurrent()
        {
            ClearContact();
            ClearCustomer();
            CurrentSessionContext.CurrentMaterialCategory = null;
            CurrentSessionContext.LastContactPageType = Electracraft.Framework.Utility.SessionContext.LastContactPageTypeEnum.None;
        }
        #endregion

        public CompanyPageStatus GetPageStatus()
        {
            return GetPageStatus(CompanyPageFlag.None);
        }

        public CompanyPageStatus GetPageStatus(CompanyPageFlag PageFlag)
        {
            if (CurrentSessionContext.CurrentContact == null || CurrentSessionContext.Owner == null)
                return CompanyPageStatus.CompanyNotAuthorised;

            if (CurrentSessionContext.CurrentContact.ContactType == DataObjects.DOContact.ContactTypeEnum.Individual)
                return CompanyPageStatus.Individual;

            if (CurrentSessionContext.CurrentContact.ManagerID == CurrentSessionContext.Owner.ContactID)
            {
                return CompanyPageStatus.CompanyManager;
            }

            DOEmployeeInfo Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);

            if (Employee != null && Employee.AccessFlags > 0)

            //jared 31/1/17 start
            if (Employee != null && ((CompanyPageFlag)Employee.AccessFlags & PageFlag)==PageFlag)
            //if (Employee != null && Employee.AccessFlags > 0)
            //jared 31/1/17 end
            {
                return CompanyPageStatus.CompanyAuthorised;
            }
            return CompanyPageStatus.CompanyNotAuthorised;
        }


        protected bool CheckEmployeePageStatus(CompanyPageFlag PageFlag)
        {
            return CheckEmployeePageStatus(PageFlag, false);
        }

        protected bool CheckEmployeePageStatus(CompanyPageFlag PageFlag, bool AllowCompanyNotAuthorised)
        {
            CompanyPageStatus PageStatus = GetPageStatus(PageFlag);
            if (PageStatus == CompanyPageStatus.CompanyManager || PageStatus == CompanyPageStatus.CompanyAuthorised)
            {
                return true;
            }
            else if (PageStatus == CompanyPageStatus.Individual)
            {
                return true; //jared
                // jared ShowMessage("You must be a company to view this page", MessageType.Warning);
            }
            else if (PageStatus == CompanyPageStatus.CompanyNotAuthorised && !AllowCompanyNotAuthorised)
            {
                ShowMessage("You are not authorised to view this page.", MessageType.Warning);
            }
            return false;
        }

    }

    public enum CompanyPageStatus
    {
        Individual,
        CompanyNotAuthorised,
        CompanyAuthorised,
        CompanyManager
    }

    public enum CompanySubscriptionFlag : long //company subscription rights. Also held in contact.NumberOfUsers(int) and contact.NumberOfGig(int)

    {
        None = 0,
        GanttChart = 1,
        AddMaterialandLabourAndViewTimesheets = 2,
        CalendarView = 4,
        HelpAndSupport = 8,
        HealthAndSafety = 32,
        SiteOverView = 64
        //Fit = 128



    }
    public enum ContactCompanySettingsFlag : long //employee settings DB.ContactCompany.settings
    {
        None = 0,
        MainScreenMaximised = 1,
        AtTheTop = 2,
        ShowMyCustomers = 4,
        ShowSupplierInvoicesToAssign = 8,
        ShowMyFitness = 16,
        ShowMyOnlineVault = 32,
        ShowMyScore = 64,
        ShowMyJobsWithLabour = 128,
        ShowMyContractors =256,
        ShowMyGoals =512,
        ShowMyProperties = 1024        

    }

    public enum CompanyPageFlag : long //employee rights DB.employeeinfo.accessflags
    {
        None = 0,
        ShowEmployeeInfo = 1,
        TimeSheet = 2,
        EmployeeDetails = 4, //possibly not used
        TradeCategories = 8, //possibly not used
        MoveMaterialsFromOtherVehicle = 16,
        DeleteMaterialsFromVehicle = 32, //and delete sites and customers
        ImportInvoices = 64,
        AddMaterialsManuallyToVehicle = 128,

        //Tony Added
        ShareSiteToAnotherCustomer = 256,
        MoveJobToAnotherSite = 512,
        MoveTaskToAnotherJob = 1024,

        ViewInvoices = 2048,
        DeleteInvoices = 4096,
        //jared 30.1.17
        AddAndEditVehicles = 8192, 
        AccountsScreen = 16384,
        CreateJobTemplates = 32768,
        PromoteBusinessScreenAndAddons = 65536
        

        //    EnterPromoteBusiness = 16,
        //    ViewJobSummary = 32,
        //    ViewTaskSummary = 64,
        //    AddLabourMaterials = 128,
        //    AddOrEditSites = 256,
        //    AddOrEditJobs = 512,
        //    AddOrEditTasks = 1024,
        //    DeleteJobs = 2048,
        //    DeleteTasks = 4096,
        //    ViewCostsAndRatesForTasks = 8192,
        //    DeleteCustomer = 16384,
        //    InvoiceTask = 32768,
        //    EditPermissions = 65536,
        //    AcceptQuotes = 131072,
        //    CreateQuotes = 262144
        //    ViewAllTimeSheets = 524288,
        //    ViewMyTeamsTimeSheets = 1048576,
        //    ViewEmployeeInfo = 2097152,
        //    ModifyAndAddEmployeeDetails = 4194304


    }
}
