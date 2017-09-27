using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Framework.Utility
{
    public class SessionContext
    {
        public enum LastContactPageTypeEnum
        {
            None,
            Self,
            Customer
        }

        public SessionContext(DOContact Contact)
        {
            this.Owner = Contact;
        }
        /// <summary>
        /// Session Owner - the account the user is logged in under
        /// </summary>
        public DOContact Owner { get; set; }

        /// <summary>
        /// Current Contact / Company - for displaying sites / adding customers
        /// </summary>
        public DOContact CurrentContact { get; set; }

        /// <summary>
        /// Individual being registered.
        /// </summary>
        public DOContact RegisterContact { get; set; }
        /// <summary>
        /// Company being registered
        /// </summary>
        public DOContact RegisterCompany { get; set; }

        /// <summary>
        /// Current customer - for displaying / adding sites
        /// </summary>
        //public DOContact CurrentCustomer { get; set; }
        //New logic, customer removed and contact is used
        public DOContact CurrentCustomer { get; set; }

        /// <summary>
        /// Current contractee - acting as a customer for when a contact is sub contracting a job.
        /// </summary>
        public DOContact CurrentContractee { get; set; }

        /// <summary>
        /// Current site
        /// </summary>
        public DOSite CurrentSite { get; set; }

        /// <summary>
        /// Current job
        /// </summary>
        public DOJob CurrentJob { get; set; }

        /// <summary>
        /// Current task
        /// </summary>
        public DOTask CurrentTask { get; set; }

        // Tony added 16.Feb.2017
        /// <summary> 
        /// Current Employee
        /// </summary>
        public DOEmployeeInfo CurrentEmployee { get; set; }

        public DOMaterialCategory CurrentMaterialCategory { get; set; }

        public List<Guid> AmendedTaskIDs { get; set; }


        /// <summary>
        /// The site page can be accessed from customer page or contact page. 
        /// These can sometimes be the same so this is used to differentiate so we can go back to the correct page from the site page. 
        /// </summary>
        public LastContactPageTypeEnum LastContactPageType { get; set; }

        //public DOJobTimeSheet CurrentTimeSheet { get; set; }
        //public bool CurrentTimeSheetAdminMode { get; set; }
    }
}
