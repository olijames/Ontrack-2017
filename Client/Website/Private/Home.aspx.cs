using System;
using System.Collections.Generic;
using Electracraft.Framework.Web;
using Electracraft.Client.Website.UserControls;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.Private
{
	[PrivatePage]
	public partial class Home : PageBase
	{
        public DOEmployeeInfo Employee;
        private DOContactCompany docc;
        protected void Page_Init(object sender, EventArgs e)
		{
            
            bool hasOwnContactCompany = false;
			//Get linked companies and display a contact panel for each.
			List<DOBase> Contacts = CurrentBRContact.SelectContactCompanies(CurrentSessionContext.Owner.ContactID, true, true);
            //Display current user at top of list.

            //Contacts.Insert(0, CurrentSessionContext.Owner);

            ContactPanelHome ContactPanel;
            foreach (DOContact Contact in Contacts)
			{
                ContactPanel = (ContactPanelHome)LoadControl("~/UserControls/ContactPanelHome.ascx");
				ContactPanel.Contact = Contact;
				phContactPanels.Controls.Add(ContactPanel);
                //added 2017.4.24 jared start of block
                if (Contact==CurrentSessionContext.Owner)
                {
                    hasOwnContactCompany = true;
                }
                Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, Contact.ContactID);
                if (Employee == null && CurrentSessionContext.Owner.ContactID != Contact.ContactID) //if true then an employee record is missing for yourself
                    AddEmployee(0, Contact);
            }
            docc = CurrentBRContact.SelectAContactCompany(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.Owner.ContactID);
            if (docc != null)
                hasOwnContactCompany = true;

            if (!hasOwnContactCompany)
            {
                docc = CurrentBRContact.CreateContactCompany(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.Owner.ContactID, CurrentSessionContext.Owner.ContactID);
                CurrentBRContact.SaveContactCompany(docc);
            } 

                ContactPanel = (ContactPanelHome)LoadControl("~/UserControls/ContactPanelHome.ascx");
                ContactPanel.Contact = CurrentSessionContext.Owner;
                phContactPanels.Controls.Add(ContactPanel);
            
            Employee = CurrentBRContact.SelectEmployeeInfo(CurrentSessionContext.Owner.ContactID, CurrentSessionContext.Owner.ContactID);
            if (Employee == null) //if true then an employee record is missing for yourself
                AddEmployee(1048576, CurrentSessionContext.Owner);
            //eob 2017.4.24
                        //Reset current contact.
            ClearCurrent();

		}

       

        protected void AddEmployee(int i, DOContact C)
        {
           
                
                //DOContactCompany docc = CurrentBRContact.CreateContactCompany(CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentContact.ContactID, CurrentSessionContext.CurrentContact.ContactID);
                //CurrentBRContact.SaveContactCompany(docc);
                Employee = CurrentBRContact.CreateEmployeeInfo(docc.ContactCompanyID, CurrentSessionContext.Owner.ContactID);
                Employee.AccessFlags = i;
                Employee.Address1 = C.Address1;
                Employee.Address2 = C.Address2;
                Employee.Address3 = C.Address3;
                Employee.Address4 = C.Address4;
                Employee.Email = C.Email;
                Employee.FirstName = C.FirstName;
                Employee.LastName = C.LastName;
                Employee.Phone = C.Phone;
                CurrentBRContact.SaveEmployeeInfo(Employee);
            
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			littext.Text = CurrentSessionContext.Owner.UserName;
		}


	}
}