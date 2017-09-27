using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
    [PrivatePage]
    public partial class ChangePassword : PageBase
    {
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                PerformChangePassword();
                ShowMessage("Your password was changed successfully.", MessageType.Info);
            }
            catch (FieldValidationException ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }

        }

        private void PerformChangePassword()
        {
            //All fields are required.
            string OldPassword = txtOldPassword.Text;
            string NewPassword = txtNewPassword.Text;
            string ConfirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(OldPassword))
                throw new FieldValidationException("Old password is required.");
            if (string.IsNullOrEmpty(NewPassword))
                throw new FieldValidationException("New password is required.");
            if (string.IsNullOrEmpty(ConfirmPassword))
                throw new FieldValidationException("Confirm new password is required.");
            if (NewPassword != ConfirmPassword)
                throw new FieldValidationException("Your new passwords do not match.");

            //Validate old password.
            DOContact Contact = CurrentBRContact.SelectContact(CurrentSessionContext.Owner.ContactID);
 
            SessionContext CheckSessionContext = CurrentBRContact.AuthenticateUserFromForm(Contact.UserName, OldPassword);
            if (CheckSessionContext == null)
                throw new FieldValidationException("Your old password was incorrect.");

            //Update the password.
            Contact.PasswordHash = PasswordHash.CreateHash(NewPassword);
            CurrentBRContact.SaveContact(Contact);
        }
    }
}