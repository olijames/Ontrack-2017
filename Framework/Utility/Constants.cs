using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.Utility
{
    public class Constants
    {
        public static bool DISABLE__EMAIL = false;
        public static bool EMAIL__TESTMODE = true;

        //public static string WebsiteBasePath = "http://portal.ecraft.co.nz/";
        public static string EmailSender = "no-reply@ecraft.co.nz";

        public static string CookieUserID = "4A208A2959B3";
        public static string CookiePasswordHash = "FE9CFA23EFB2";
        public static int CookieUsernameExpiryDays = 180;

        public static Guid Guid_DefaultUser = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
        public static Guid Guid_PendingUser = new Guid("11111111-1111-1111-1111-111111111111");
        public static string SessionCurrentContext = "SESSION_CURRENTCONTEXT";

        public static string URL_ImportSupplierInvoice = "~/private/ImportSupplierInvoices.aspx";
        public static string URL_Home = "~/Private/Home.aspx";
        public static string URL_ContactHome = "~/Private/ContactHome.aspx";
        public static string URL_SiteDetails = "~/Private/SiteDetails.aspx";
        public static string URL_CustomerDetails = "~/Private/CustomerDetails.aspx";
     //   public static string URL_CustomerCompanyDetails = "~/Private/CustomerCompanyDetails.aspx";
        public static string URL_CustomerHome = "~/Private/CustomerHome.aspx";
        public static string URL_SiteHome = "~/Private/SiteHome.aspx";
        public static string URL_SiteVisibility = "~/Private/SiteVisibility.aspx";
        public static string URL_JobDetails = "~/Private/JobDetails.aspx";
        public static string URL_JobSummary = "~/Private/JobSummary.aspx";
        public static string URL_JobsToInvoice = "~/Private/JobsToInvoice.aspx";
        public static string URL_MaterialFromInvoice = "~/Private/MaterialFromInvoice.aspx";
        public static string URL_CompanyDetails = "~/Private/CompanyDetails.aspx";
        public static string URL_TaskDetails = "~/Private/TaskDetails.aspx";
        public static string URL_TaskSummary = "~/Private/TaskSummary.aspx";
        public static string URL_TaskHistory = "~/Private/TaskHistory.aspx";
        public static string URL_TaskAcknowledgement = "~/Private/TaskAcknowledgement.aspx";
        public static string URL_RegisterContact = "~/RegisterIndividual.aspx";
        public static string URL_RegisterCompany = "~/RegisterCompany.aspx";
        public static string URL_RegisterSubscription = "~/RegisterSubscription.aspx";
        public static string URL_TimeSheetDetails = "~/Private/TimeSheetDetails.aspx";
        public static string URL_Connections = "~/Private/Connections.aspx";


        public static string URL_MaterialList = "~/Private/MaterialList.aspx";
        public static string URL_PromoteBusiness = "~/Private/PromoteBusiness.aspx";

        public static string URL_MaterialCategory = "~/Private/MaterialCategory.aspx";
        public static string URL_MaterialCategoryDetails = "~/Private/MaterialCategoryDetails.aspx";

        public static string FileUploadBasePath = "~/FileUpload";
        public static int Image_ThumbWidth = 150;
        public static int Image_StandardWidth = 640;


        //Maximum number of days that can pass for non-admin to add time sheet entry
        public static int TimeSheetEntryDays = 1;


        public static string Setting_TestEmailRecipient = "TestEmailRecipient";
        public static string Setting_WebsiteBasePath = "WebsiteBasePath";
    }
}
