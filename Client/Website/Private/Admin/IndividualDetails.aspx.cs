using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private.Admin
{
    [AdminPage]
    public partial class IndividualDetails : PageBase
    {
        const string NewUser = "New Private User";
        const string EditUser = "Edit Private User - {0}";

        public DOContact Contact;
        public DOContact.ContactTypeEnum ContactType = DOContact.ContactTypeEnum.Individual;

        protected void Page_Init(object sender, EventArgs e)
        {
            GetContact();
            udIndividual.AdminMode = CurrentBRContact.IsAdmin(CurrentSessionContext.Owner);
            if (Contact.PersistenceStatus == ObjectPersistenceStatus.New)
            {
                phCompanies.Visible = false;
            }
            else
            {
                phCompanies.Visible = true;
                cContactCompanies.Contact = Contact;
            }
        }

        private void GetContact()
        {
            string strContactID = Request.QueryString["contactid"];
            if (string.IsNullOrEmpty(strContactID))
            {
                heading.InnerText = NewUser;
                Contact = CurrentBRContact.CreateContact(CurrentSessionContext.Owner.ContactID, ContactType);
            }
            else
            {
                Contact = CurrentBRContact.SelectContact(new Guid(strContactID));
                heading.InnerText = string.Format(EditUser, Contact.UserName);
            }

            udIndividual.Contact = Contact;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            udIndividual.LoadForm();
            btnActivate.Visible = !Contact.Active;
            btnDeactivate.Visible = Contact.Active;
        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                bool NewContact = Contact.PersistenceStatus == ObjectPersistenceStatus.New;
                udIndividual.SaveForm();
                CheckNewPassword(NewContact);

                CurrentBRContact.SaveContact(Contact);

                if (NewContact)
                {
                    Response.Redirect("IndividualDetails.aspx?contactid=" + Contact.ContactID.ToString());
                }
                else
                {
                    ShowMessage("Saved successfully.", MessageType.Info);
                }
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void CheckNewPassword(bool NewContact)
        {
            string NewPassword = txtNewPassword.Text;
            string ConfirmPassword = txtConfirmPassword.Text;

            if (NewContact)
            {
                if (string.IsNullOrEmpty(NewPassword))
                    throw new FieldValidationException("Password is required.");
                if (string.IsNullOrEmpty(ConfirmPassword))
                    throw new FieldValidationException("Please confirm the password.");
            }
            else
            {
                if (string.IsNullOrEmpty(NewPassword) && string.IsNullOrEmpty(ConfirmPassword))
                    return;
            }

            if (NewPassword != ConfirmPassword)
                throw new FieldValidationException("The passwords you have entered do not match.");

            Contact.PasswordHash = PasswordHash.CreateHash(NewPassword);
        }

        protected void btnActivate_Click(object sender, EventArgs e)
        {
            Contact.Active = true;
            CurrentBRContact.SaveContact(Contact);
            ShowMessage("The user has been activated", MessageType.Info);
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            Contact.Active = false;
            CurrentBRContact.SaveContact(Contact);
            ShowMessage("The user has been deactivated", MessageType.Info);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentBRContact.DeleteContactComplete(Contact);
                Response.Redirect("~/Private/Admin/ManageUsers.aspx",false);
            }
            catch (Exception ex)
            {
                ShowMessage("The contact could not be deleted.<br />" + ex.Message, MessageType.Error);
            }
        }
    }
}