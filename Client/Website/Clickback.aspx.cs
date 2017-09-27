using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website
{
    public partial class Clickback : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["cv"]))
            {
                VerifyContact(Request.QueryString["cv"]);
            }
        }

        private void VerifyContact(string vcode)
        {
            DOContactVerification cv = CurrentBRContact.CheckVerification(vcode);
            if (cv == null)
            {
                phVerificationFailed.Visible = true;
            }
            else
            {
                //Activate contact.
                DOContact c = CurrentBRContact.SelectContact(cv.ContactID);
                c.Active = true;
                CurrentBRContact.SaveContact(c);

                //Log in contact.
                Login(c.ContactID, c.PasswordHash);

                //Remove verification.
                CurrentBRContact.DeleteContactVerification(cv);

                phVerificationOK.Visible = true;
            }
            
        }
    }
}