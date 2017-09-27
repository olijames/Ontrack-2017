using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Client.Website.Private.Admin
{
    [AdminPage]
    public partial class ManageUsers : PageBase
    {
        string Term;
        int ContactType;
        bool? Subscribed;
        bool Active = true;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ClearParams();
            else
                DisplayParams();                
            
            List<DOBase> Contacts = CurrentBRContact.SearchContacts(ContactType, Subscribed, Term, Active);
            gvContacts.DataSource = Contacts;
            gvContacts.DataBind();
        }

        protected void btnLoginAs_Click(object sender, EventArgs e)
        {
            Guid ContactID = new Guid(((Button)sender).CommandArgument);
            DOContact LoginContact = CurrentBRContact.SelectContact(ContactID);
            if (LoginContact.ContactType == DOContact.ContactTypeEnum.Company)
            {
                ShowMessage("Cannot log in as company.", MessageType.Error);
                return;
            }

            ClearCurrent();
            CurrentSessionContext.Owner = LoginContact;
            Response.Redirect(Constants.URL_Home);
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Active = rblUserStatus.SelectedValue == "0" ? false : true;
            Term = txtTerm.Text;
            ContactType = int.Parse(rblUserType.SelectedValue);
            Subscribed = rblSubType.SelectedValue == "-1" ? (bool?)null : Convert.ToBoolean(int.Parse(rblSubType.SelectedValue));
        }


        protected void DisplayParams()
        {
            txtTerm.Text = Term;
            rblUserType.SelectedValue = ContactType.ToString();
            rblSubType.SelectedValue = Subscribed.HasValue ? (Subscribed.Value ? "1" : "0") : "-1";
        }

        protected void ClearParams()
        {
            Term = string.Empty;
            ContactType = -1;
            Subscribed = null;
            Active = true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearParams();
        }
    }
}