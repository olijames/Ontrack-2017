using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class Settings : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            udContact.Contact = CurrentSessionContext.Owner;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            udContact.LoadForm();
        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                udContact.SaveForm();
                CurrentBRContact.SaveContact(CurrentSessionContext.Owner);
                ShowMessage("Your details were updated successfully.", MessageType.Info);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Private/ChangePassword.aspx");
        }
    }
}