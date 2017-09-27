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
using System.Threading;

namespace Electracraft.Client.Website
{
    public partial class RegisterCompany : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //If no user, return to register individual page.
            if (CurrentSessionContext.RegisterContact == null)
            {
                Response.Redirect(Constants.URL_RegisterContact);
            }
            else
            {
                if (!IsPostBack)
                    rgCompany.LoadForm(CurrentSessionContext.RegisterContact);
            }
        }


        public void Page_PreRender(object sender, EventArgs e)
        {
        }


        protected void btnRegister_Click(object sender, EventArgs e)
        {
            DOContact ContactCompanyCompany = null;
            DOContactCompany ContactCompanyExisting = null;
            DOContactVerification ContactVerification = null;

            CurrentSessionContext.RegisterCompany = null;

            try
            {
                string CompanyKey = txtCompanyKey.Text;
                if (chkCompany.Checked)
                {
                    //Existing company?
                    if (rbLinkCompany.Checked)
                    {
                        if (!string.IsNullOrEmpty(CompanyKey))
                        {
                            ContactCompanyExisting = LinkToExistingCompany(CompanyKey, out ContactCompanyCompany);
                        }
                    }

                    if (rbNewCompany.Checked)
                    {
                        //Save the new company.
                        if (rgCompany.HasData())
                        {
                            RegisterNewCompany();
                        }
                    }

                 



                    if (string.IsNullOrEmpty(CompanyKey) && rbLinkCompany.Checked)
                        throw new FieldValidationException("Please enter a company key.");
                }


                //Check if the registering user is a pending contractor.
                List<DOBase> PendingTasks = CurrentBRJob.SelectPendingContractorTasks(CurrentSessionContext.RegisterContact.Email);
                if (PendingTasks.Count > 0)
                {
                    DOContact ContractorContact = CurrentSessionContext.RegisterContact;
                    if (ContactCompanyCompany != null) ContractorContact = ContactCompanyCompany;
                    if (CurrentSessionContext.RegisterCompany != null) ContractorContact = CurrentSessionContext.RegisterCompany;

                    foreach (DOTaskPendingContractor TPC in PendingTasks)
                    {
                        DOTask Task = CurrentBRJob.SelectTask(TPC.TaskID);
                        DOContact TaskOwner = CurrentBRContact.SelectContact(Task.TaskOwner);
                        DOContact EmailRecipient;

                        //Update the task contractor.
                        Task.ContractorID = ContractorContact.ContactID;
                        CurrentBRJob.SaveTask(Task);

                        //Delete the pending contractor entry.
                        CurrentBRJob.DeleteTaskPendingContractor(TPC);

                        //Send the email. !here!
                        if (TaskOwner.ContactType == DOContact.ContactTypeEnum.Individual)
                            EmailRecipient = TaskOwner;
                        else
                            EmailRecipient = CurrentBRContact.SelectContact(TaskOwner.ManagerID);

                        if (EmailRecipient == null) continue;
                        //Notify task owner of each task that the pending contact has registered.
                        string EmailSubject = "Pending contractor " + ContractorContact.Email + " on task " + Task.TaskName + " has registered - OnTrack";
                        string Body = "Pending contractor " + ContractorContact.Email + " on task " + Task.TaskName + " has registered.<br />";
                        Body += "<br /><a href=\"" + CurrentBRGeneral.SelectWebsiteBasePath() + "/private/TaskDetails.aspx?taskid=" + Task.TaskID.ToString() + "\">View Task</a>";
                        if (Constants.EMAIL__TESTMODE)
                        {
                            Body += "<br/><br/>";
                            Body += "Sent By: " + ContractorContact.DisplayName + "<br />";
                            Body += "Sent To: " + EmailRecipient.DisplayName + " (" + EmailRecipient.Email + ")";
                        }

                        CurrentBRGeneral.SendEmail(Constants.EmailSender, EmailRecipient.Email, EmailSubject, Body);

                    }
                }

                //Make sure there is not already a pending entry for this contact.
                DOContactVerification cvCheck = CurrentBRContact.SelectContactVerificationByContactID(CurrentSessionContext.RegisterContact.ContactID);
                if (cvCheck == null)
                {
                    //Log to pending table.
                    ContactVerification = CurrentBRContact.CreateContactVerification(CurrentSessionContext.RegisterContact.ContactID);
                    SendVerificationEmail(ContactVerification);
                }


                ////Log user in.
                //Login(CurrentSessionContext.RegisterContact.ContactID, CurrentSessionContext.RegisterContact.PasswordHash);
                //Go to final register step.
                Response.Redirect("~/RegistrationComplete.aspx",false);
            }
            catch (ThreadAbortException ex)
            {
                //Do nothing for this exception
                ex.StackTrace.ToString();
            }
            catch (FieldValidationException ex)
            {
                if (ContactCompanyExisting != null)
                {
                    CurrentBRContact.DeleteContactCompany(ContactCompanyExisting);
                }
                if (ContactVerification != null)
                {
                    CurrentBRContact.DeleteContactVerification(ContactVerification);
                }
                ShowMessage(ex.Message, MessageType.Error);
            }
            catch (Exception ex)
            {
                if (ContactCompanyExisting != null)
                {
                    CurrentBRContact.DeleteContactCompany(ContactCompanyExisting);
                }
                if (ContactVerification != null)
                {
                    CurrentBRContact.DeleteContactVerification(ContactVerification);
                }
                ShowMessage(ex.Message, MessageType.Error);

            }
        }

        private void SendVerificationEmail(DOContactVerification cv)
        {
            //Notify task owner of each task that the pending contact has registered.
            string EmailSubject = "Please verify your email for OnTrack";
            string Body = "Please activate your OnTrack account by clicking the link below.";
            Body += "<br /><a href=\"" + CurrentBRGeneral.SelectWebsiteBasePath() + "/Clickback.aspx?cv=" + cv.VerificationCode + "\">Verify Email</a>";
            if (Constants.EMAIL__TESTMODE)
            {
                Body += "<br/><br/>";
                Body += "Sent By: " + CurrentSessionContext.RegisterContact.DisplayName + "<br />";
                Body += "Sent To: " + CurrentSessionContext.RegisterContact.DisplayName;
            }

            CurrentBRGeneral.SendEmail(Constants.EmailSender, CurrentSessionContext.RegisterContact.Email, EmailSubject, Body);
        }

        private void RegisterNewCompany()
      {
            //
            DOContact Company = CurrentBRContact.CreateContact(Guid.Empty, DOContact.ContactTypeEnum.Company);
            rgCompany.SaveForm(Company);
            Company.ManagerID = CurrentSessionContext.RegisterContact.ContactID; // !here! need rules here depending on user role
            Company.CompanyKey = CurrentBRContact.GenerateCompanyKey();

            CurrentBRContact.SaveContact(Company);
            CurrentSessionContext.RegisterCompany = Company;
            DOContactCompany ContactCompanyNew = CurrentBRContact.CreateContactCompany(CurrentSessionContext.RegisterContact.ContactID, Company.ContactID, CurrentSessionContext.RegisterContact.ContactID);
            CurrentBRContact.SaveContactCompany(ContactCompanyNew);
        }

        private DOContactCompany LinkToExistingCompany(string CompanyKey, out DOContact ContactCompanyCompany)
        {
            DOContact Contact = CurrentBRContact.SelectContactByCompanyKey(CompanyKey);
            if (Contact == null)
                throw new FieldValidationException("The company key " + CompanyKey + " does not belong to any company.");
            //Link the company and contact.
            DOContactCompany ContactCompany = CurrentBRContact.CreateContactCompany(CurrentSessionContext.RegisterContact.ContactID, Contact.ContactID, CurrentSessionContext.RegisterContact.ContactID);
            ContactCompany.Pending = true;
            CurrentBRContact.SaveContactCompany(ContactCompany);
            CurrentBRGeneral.SendContactCompanyPendingEmail(Contact, CurrentSessionContext.RegisterContact);
            

            ContactCompanyCompany = Contact;

            return ContactCompany;
        }

    }
}