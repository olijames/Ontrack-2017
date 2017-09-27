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
    public partial class ContactHome : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentSessionContext.CurrentContact == null)
                Response.Redirect(Constants.URL_Home);

            //Clear currently selected customer.
            ClearCustomer();
            CurrentSessionContext.LastContactPageType = SessionContext.LastContactPageTypeEnum.Self;

            //Tony added 16.Feb.2017
            CurrentSessionContext.CurrentEmployee =
                CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.CurrentContact.ContactID);
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            litContactName.Text = CurrentSessionContext.CurrentContact.DisplayName;
            litContactNameTitle.Text = CurrentSessionContext.CurrentContact.DisplayName;
            
            List<DOBase> CustomerSites = CurrentBRSite.SelectContactSites(CurrentSessionContext.CurrentContact.ContactID);
            var ActiveSites = from DOSite site in CustomerSites where site.Active select site;
            rpSites.DataSource = ActiveSites;
            rpSites.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_Home);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            
            if( CheckEmployeePageStatus(CompanyPageFlag.ImportInvoices))
            {
                Response.Redirect(Constants.URL_ImportSupplierInvoice);
            }
            
        }

        protected void btnConnections_Click(object sender, EventArgs e)
        {

            if (CheckEmployeePageStatus(CompanyPageFlag.AccountsScreen))
            {
                Response.Redirect(Constants.URL_Connections);
            }

        }

        protected void btnVehicleInput_Click(object sender, EventArgs e)
        {
            //jared 30.1.17
            //// CurrentSessionContext.CurrentContact.ManagerID
            // //if (CurrentSessionContext.Owner.ContactID == Guid.Parse("53e58c1b-4d58-41f9-9849-fbb5b4f87833")) !here! Jared's id. Sort permissions out later here
            // if (CurrentSessionContext.Owner.ContactID == CurrentSessionContext.CurrentContact.ManagerID || CurrentSessionContext.CurrentContact.ManagerID == Guid.Parse("00000000-0000-0000-0000-000000000000")) //!here! Jared's id. Sort permissions out later here
            // {
            if (CheckEmployeePageStatus(CompanyPageFlag.AddAndEditVehicles))
            {

                Response.Redirect("~/private/VehicleInput.aspx", false);
            }
        }

        protected void btnAddSite_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_SiteDetails);
        }

        protected void btnSelectSite_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                if (b.CommandName == "SelectSite")
                {
                    DOSite Site = CurrentBRSite.SelectSite(new Guid(b.CommandArgument.ToString()));
                    CurrentSessionContext.CurrentSite = Site;
                    Response.Redirect(Constants.URL_SiteHome);
                }
            }
        }

        protected void btnAccounts_Click(object sender, EventArgs e)
        {
            //jared 30.1.17
            if (CheckEmployeePageStatus(CompanyPageFlag.AccountsScreen))
            {
                Response.Redirect("~/private/accounts.aspx", false);
            }
        }

        protected void btnCalendar_Click(object sender, EventArgs e)
        {
        }

        protected void btnEmpInfo_Click(object sender, EventArgs e)
        {
            //jared 30.1.17
            if (CheckEmployeePageStatus(CompanyPageFlag.ShowEmployeeInfo))
            {
                Response.Redirect("~/private/employeeinfo.aspx", false);
            }
        }
        protected void btnTimeSheets_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/private/timesheets.aspx",false);
        }
        protected void btnHS_Click(object sender, EventArgs e)
        {
           // Response.Redirect("~/Private/HealthAndSafety.aspx", false);
        }

        protected void btnMySites_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/private/mysites.aspx",false);
        }

        protected void btnPAYE_Click(object sender, EventArgs e)
        {
        }

        protected void btnPAYELowTax_Click(object sender, EventArgs e)
        {
        }

        protected void btnPAYEHighTax_Click(object sender, EventArgs e)
        {
        }

        protected void btnPAYELowPercent_Click(object sender, EventArgs e)
        {
        }

        protected void btnPAYEHighPercent_Click(object sender, EventArgs e)
        {
        }

        protected void btnMaterials_Click(object sender, EventArgs e)
        {
            //jared 30.1.17
            if (CheckEmployeePageStatus(CompanyPageFlag.AddMaterialsManuallyToVehicle))
            {
                Response.Redirect(Constants.URL_MaterialList);
            }
        }

        protected void btnOffice_Click(object sender, EventArgs e)
        {
        }

        protected void btnPlant_Click(object sender, EventArgs e)
        {
        }

        protected void btnVehicles_Click(object sender, EventArgs e)
        {
        }

        protected void btnMyTemplatesInput_Click(object sender, EventArgs e)
        {
            //jared 30.1.17
            //if (CurrentSessionContext.Owner.ContactID == Guid.Parse("53e58c1b-4d58-41f9-9849-fbb5b4f87833")) //!here! Jared's id. Sort permissions out later here
            //{
            //    Response.Redirect("~/Private/JobTemplateInput.aspx", false);
            //}
            if (CheckEmployeePageStatus(CompanyPageFlag.CreateJobTemplates))
            {
                   Response.Redirect("~/Private/JobTemplateInput.aspx", false);
            }
        }


        protected void btn_PB_Click(object sender, EventArgs e)
        {
            //jared 30.1.17
            if (CheckEmployeePageStatus(CompanyPageFlag.PromoteBusinessScreenAndAddons))
            {

                Response.Redirect("~/Private/PromoteBusiness.aspx", false);
            }
        }
        protected void btn_AddOns_Click(object sender, EventArgs e)
        {
            //jared 20174.29
            if (CheckEmployeePageStatus(CompanyPageFlag.PromoteBusinessScreenAndAddons))
            {

                Response.Redirect("~/Private/AddOns.aspx", false);
            }
        }
    }
}