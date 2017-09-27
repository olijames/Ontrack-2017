using Electracraft.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class Notifications : PageBase
    {
      public  struct CustDetails
        {
          public  DOContact ContactContractor { get; set; }
            public DOContact ContactCustomer { get; set; }
            public  DOContractorCustomer ContractorCustomer { get; set; }

        }
      public  List<CustDetails> _custDetails = new List<CustDetails>();
        public DOContact ContactContractor;
        public DOContact ContactCustomer;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<DOBase> contractorCustomerPending = IfLinkingPending();
          
            GetAllCustomerDetails(contractorCustomerPending);
            FillRepeaterCustomerDetails();
            IfPendingWithCustomer();
          
            LoadRegions();

        }

        private void IfPendingWithCustomer()
        {
            foreach (var VARIABLE in _custDetails)
            {
               DOContractorCustomer contractorCustomer=VARIABLE.ContractorCustomer as DOContractorCustomer;
                ContactContractor = VARIABLE.ContactContractor;
                ContactCustomer = VARIABLE.ContactCustomer;
                if (contractorCustomer.Linked == DOContractorCustomer.LinkedEnum.AwaitingCust)
                {
                    
                   //todo ContractorName.Text = ContactContractor.DisplayName;
                    // todo custDetailsRep.Visible = false;
                  
                    // todo ContractorNotification.Visible = false;
                    StringBuilder SendCustomerDetailsToContractor =new StringBuilder("Send my details to ");
                    SendCustomerDetailsToContractor.Append(ContactContractor.DisplayName);
                    // todo SendCustomerDetailsToContractor_btn.Text = SendCustomerDetailsToContractor.ToString();
                }

                if (contractorCustomer.Linked == DOContractorCustomer.LinkedEnum.AwaitingContractor)
                {
                    //   todo   CustomerNotification.Visible = false;
                    // todo   ContractorNotification.Visible = true;
                    SetCustomerDetails();
                }
               
            }
        }

        private void SetCustomerDetails()
        {
            //CustomerDisplayName.Text = ContactCustomer.DisplayName;
        }

        private void LoadRegions()
        {
            List<DOBase> Regions = CurrentBRRegion.SelectRegions();
            //DropDownList address4 = (DropDownList) (custDetailsRep.FindControl("Address4Region"));
            //address4.DataSource = Regions;
            //address4.DataTextField= "RegionName";
            //address4.DataValueField= "RegionID";
            //address4.DataBind();
        }

        /// <summary>
        /// To display both the details simultanesouly for all the pending linking contacts/companies
        /// </summary>
        private void FillRepeaterCustomerDetails()
        {
            custDetailsRep.DataSource = _custDetails;
            foreach (RepeaterItem repItem in custDetailsRep.Items)
            {

                var customerNotification = repItem.FindControl("CustomerNotification") as Panel;
                var contractorNotification= repItem.FindControl("ContractorNotification") as Panel;
                foreach (var VARIABLE in _custDetails)
                {
                    DOContractorCustomer contractorCustomer = VARIABLE.ContractorCustomer as DOContractorCustomer;

                    if (contractorCustomer.Linked == DOContractorCustomer.LinkedEnum.AwaitingCust)
                    {
                        if (customerNotification != null) customerNotification.Visible = true;
                        if (contractorNotification != null) contractorNotification.Visible = false;
                    }
                }
            }
            custDetailsRep.DataBind();
        }

        /// <summary>
        /// To get the list of customers with both details- Customer's version as well as Contractor's version
        /// </summary>
        /// <param name="contractorCustomerPendingList"></param>
        /// <returns>List of customers with their different details</returns>
        private void GetAllCustomerDetails(List<DOBase> contractorCustomerPendingList)
        {
           
            foreach (var dobase in contractorCustomerPendingList)
            {
                var contractorCust = dobase as DOContractorCustomer;
                if (contractorCust != null)
                {
                    var customerDetails = CurrentBRContact.SelectContact(contractorCust.CustomerID);
                    var contractorDetails= CurrentBRContact.SelectContact(contractorCust.ContractorId);
                    _custDetails.Add(new CustDetails() { ContractorCustomer = contractorCust, ContactCustomer = customerDetails,ContactContractor = contractorDetails});
                }
            }
          
        }

        /// <summary>
        /// Find out if there is any pending request for Customer or Contractor
        /// </summary>
        private List<DOBase> IfLinkingPending()
        {
            List<DOBase> contractorCustomer = CurrentBRContact.SearchLinkedContacts(CurrentSessionContext.Owner.ContactID);
            return contractorCustomer;
        }

        protected void SendCustomerDetailsToContractor_btn_OnClick(object sender, EventArgs e)
        {
          
        }

        protected void No_OnClick(object sender, EventArgs e)
        {
          Response.Redirect(Constants.URL_Home);
        }
    }
}