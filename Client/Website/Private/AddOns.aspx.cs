using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class AddOns : PageBase
    {
        //public bool MyCustomers = false;
        //public bool MyJobsWithLabour = false;
        //public bool MySupplierInvoices = false;
        //public bool MyContractors = false;
        //public bool MyVault = false;
        //public bool MyHealth = false;
        //public bool MyProperties = false;
        //public bool MyGoals = false;
        //public bool MyScore = false;
        private DOContactCompany docc;



        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_ContactHome);
            docc = CurrentBRContact.SelectAContactCompany(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
            //Make sure contact is up to date.
            CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
        
           

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chkGoals.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyGoals) > 0;
                chkMyContractors.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyContractors) > 0;
                chkMyCustomers.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyCustomers) > 0;
                chkMyHealth.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyFitness) > 0;
                chkMyJobsWithLabour.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyJobsWithLabour) > 0;
                chkMyOnlineVault.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyOnlineVault) > 0;
                chkMyProperties.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowMyProperties) > 0;
                chkSupplierInvoices.Checked = (docc.Settings & (int)ContactCompanySettingsFlag.ShowSupplierInvoicesToAssign) > 0;
            }
            DataBind();
        }

       

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                int Settings =
                    (chkGoals.Checked ? (int)ContactCompanySettingsFlag.ShowMyGoals: 0) |
                    (chkMyContractors.Checked ? (int)ContactCompanySettingsFlag.ShowMyContractors: 0) |
                    (chkMyCustomers.Checked ? (int)ContactCompanySettingsFlag.ShowMyCustomers : 0) |
                    (chkMyHealth.Checked ? (int)ContactCompanySettingsFlag.ShowMyFitness : 0) |
                    (chkMyJobsWithLabour.Checked ? (int)ContactCompanySettingsFlag.ShowMyJobsWithLabour: 0) |
                    (chkMyOnlineVault.Checked ? (int)ContactCompanySettingsFlag.ShowMyOnlineVault: 0) |
                    (chkMyProperties.Checked ? (int)ContactCompanySettingsFlag.ShowMyProperties: 0) |
                    (chkSupplierInvoices.Checked ? (int)ContactCompanySettingsFlag.ShowSupplierInvoicesToAssign: 0);


                docc.Settings = Settings;

                CurrentBRContact.SaveContactCompany(docc);
                ShowMessage("Saved successfully.", MessageType.Info);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
    }
}