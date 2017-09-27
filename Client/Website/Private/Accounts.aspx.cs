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
    public partial class Accounts : PageBase
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_ContactHome);

            //Make sure contact is up to date.
            CurrentSessionContext.CurrentContact = CurrentBRContact.SelectContact(CurrentSessionContext.CurrentContact.ContactID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        
            phAccounts.Visible = CheckEmployeePageStatus(CompanyPageFlag.None);

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtDefaultChargeUpRate.Text = CurrentSessionContext.CurrentContact.DefaultChargeUpRate.ToString();
            txtDefaultQuoteRate.Text = CurrentSessionContext.CurrentContact.DefaultQuoteRate.ToString();
            txtJobNumberAuto.Text = CurrentSessionContext.CurrentContact.JobNumberAuto.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DOContact Company = CurrentSessionContext.CurrentContact;

            decimal DefaultQuoteRate;
            if (!decimal.TryParse(txtDefaultQuoteRate.Text, out DefaultQuoteRate))
            {
                ShowMessage("Invalid number for default quote rate", MessageType.Error);
                return;
            }

            decimal DefaultChargeUpRate;
            if(!decimal.TryParse(txtDefaultChargeUpRate.Text, out DefaultChargeUpRate))
            {
                ShowMessage("Invalid number for default charge up rate", MessageType.Error);
                return;
            }

            int JobNumberAuto;
            if (!int.TryParse(txtJobNumberAuto.Text, out JobNumberAuto))
            {
                ShowMessage("Invalid number for Job ID", MessageType.Error);
                return;
            }

            if (JobNumberAuto < Company.JobNumberAuto)
            {
                ShowMessage("Job ID cannot be decreased.");
                return;
            }

            Company.DefaultChargeUpRate = DefaultChargeUpRate;
            Company.DefaultQuoteRate = DefaultQuoteRate;
            Company.JobNumberAuto = JobNumberAuto;

            CurrentBRContact.SaveContact(Company);
            ShowMessage("Saved successfully", MessageType.Info);
               
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }
    }
}