using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website
{
    public partial class RegisterSubscription : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //Must have a current contact.
            if (CurrentSessionContext.Owner == null)
                Response.Redirect(Constants.URL_RegisterContact);

            if (CurrentSessionContext.RegisterCompany == null)
                trCompany.Visible = false;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (chkContactSubscribed.Checked)
            {
                CurrentSessionContext.Owner.SubscriptionPending = true;
                CurrentBRContact.SaveContact(CurrentSessionContext.Owner);
            }

            if (chkCompanySubscribed.Checked)
            {
                CurrentSessionContext.RegisterCompany.SubscriptionPending = true;
                CurrentBRContact.SaveContact(CurrentSessionContext.RegisterCompany);
            }

            CurrentSessionContext.RegisterCompany = null;

            Response.Redirect("~/RegistrationComplete.aspx",false);
        }
    }
}