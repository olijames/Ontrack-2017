using Electracraft.Framework.BusinessRules;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;
using Electracraft.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Electracraft.Client.Website.Controllers
{
    public class HomeScreenController : ApiController
    {
        readonly BRContact _currentBrContact = new BRContact();
        readonly BRSite _currentBrSite = new BRSite();
        readonly BRJob _currentBrJob = new BRJob();

        PageBase pb = new PageBase();
        private SessionContext _currentSessionContext;
        public SessionContext CurrentSessionContext
        {
            get
            {
                var session = HttpContext.Current.Session;
                _currentSessionContext = session[Constants.SessionCurrentContext] as SessionContext;
                return _currentSessionContext;
            }
        }


        /// <summary>
        /// Get all companies that the contact belongs to except themselves
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("CompaniesAContactBelongsTo")]
        public List<DOBase> GetCompaniesAContactBelongsTo(Guid id)
        {
            List<DOBase> MyContacts = _currentBrContact.SelectContactCompanies(id, true, true);
            return MyContacts;
        }
        /// <summary>
        /// Get all customers of a company
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("CustomersOfAContactByContactID")]
        public List<DOBase> GetCustomersOfAContact(Guid id)
        {
            List<DOBase> MyContacts = _currentBrContact.SelectAllCustomers(id);
            return MyContacts;
        }
        /// <summary>
        /// Get all sites of a contact that you have access too (the contact may have not allowed access to you)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SitesOfACustomer")]
        public List<DOBase> GetSitesOfACustomer(Guid MyID, Guid CustomerID)
        {
            List<DOBase> MySites = _currentBrSite.SelectCustomerSitesFromContactSites(CustomerID, MyID);
            return MySites;
        }
        /// <summary>
        /// Get all jobs of a contact that you have access to (the contact may have not allowed access to you)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("JobsOfACustomersSite")]
        public List<DOBase> GetJobsOfACustomersSite(Guid SiteID, Guid MyID)
        {
            //List<DOBase> MyJobs = _currentBrJob.SelectActiveJobsForContractorSite(SiteID, MyID);
            List<DOBase> MyJobs = _currentBrJob.SelectSitesJobAsContractor(SiteID, MyID);
            return MyJobs;
        }
    }
}
