﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataAccess;
using Electracraft.Framework.Utility;
using Electracraft.Framework.DataObjects;
using System.Data.SqlClient; //jared 19/1/17

namespace Electracraft.Framework.BusinessRules
{
    public class BRContact : BRBase
    {
        private SqlConnection connection;
        private DAContact _CurrentDAContact;
        private DAContact CurrentDAContact
        {
            get
            {
                if (_CurrentDAContact == null)
                    _CurrentDAContact = new DAContact(ConnectionString);
                return _CurrentDAContact;
            }
        }

        #region Authentication and Session
        /// <summary>
        /// Authenticates a user's credentials entered through the login form.
        /// </summary>
        /// <param name="Username">The username.</param>
        /// <param name="Password">The password.</param>
        /// <returns>The session context, or null if credentials were not valid.</returns>
        public SessionContext AuthenticateUserFromForm(string Username, string Password)
        {
            //Get the contact for this username. Authentication fails if username not found.
            DOContact Contact = SelectContactByUsername(Username);
            //if (Contact == null || !Contact.Active) 
            //    return null;
            if (Contact == null)
            {
                throw new Exception("Login failed for user " + Username + ".");
            }
            if (!Contact.Active)
            {
                throw new Exception("Your account is inactive.");
            }

            if (PasswordHash.ValidatePassword(Password, Contact.PasswordHash))
            {
                return CreateSessionContext(Contact);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Authenticates a user's credentials from cookie information.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
        /// <param name="PasswordHash">The password hash.</param>
        /// <returns>The session context, or null if credentials were not valid.</returns>
        public SessionContext AuthenticateUserFromCookie(Guid UserID, string PasswordHash)
        {
            DOContact Contact = SelectContact(UserID);
            if (Contact == null || !Contact.Active)
                throw new Exception("Login failed.");

            //If the stored password hash doesn't match the provided password hash, authentication fails.
            if (Contact.PasswordHash != PasswordHash)
                throw new Exception("Login failed.");

            return CreateSessionContext(Contact);
        }

        /// <summary>
        /// Creates a session context for the current contact's session.
        /// </summary>
        /// <param name="Contact">The current contact.</param>
        /// <returns>The session context.</returns>
        public SessionContext CreateSessionContext(DOContact Contact)
        {
            SessionContext CurrentSessionContext = new SessionContext(Contact);
            return CurrentSessionContext;
        }

        /// <summary>
        /// Sets the password for a contact.
        /// </summary>
        /// <param name="Contact">The contact.</param>
        /// <param name="Password">The unencrypted password.</param>
        public void SetPassword(DOContact Contact, string Password)
        {
            Contact.PasswordHash = Utility.PasswordHash.CreateHash(Password);
        }


        #endregion


        #region Roles
        /// <summary>
        /// Checks if a contact is an admin.
        /// </summary>
        /// <param name="Contact">The contact.</param>
        /// <returns>True if the contact is an admin.</returns>
        public bool IsAdmin(DOContact Contact)
        {
            if (Contact == null) return false;
            return (Contact.ContactID == Guid.Empty);
        }

        /// <summary>
        /// Find all the job contractors
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>List of contractors</returns>
        public List<DOBase> GetJobContractors(Guid jobId)
        {
            SortedList<string, object> Params = new SortedList<string, object>();
            Params.Add("@jobid", jobId);
            return CurrentDAContact.SelectObjectsFromStoredProcedure(typeof(DOContactInfo), "FindJobContractors", Params);

        }

        public List<DOBase> GetTradeCategoryJobContractors(Guid jobId, Guid tradeCategoryId)
        {
            SortedList<string, object> parms =
              new SortedList<string, object>();
            parms.Add("@jobid", jobId);
            parms.Add("@tradeCategoryID", tradeCategoryId);
            return CurrentDAContact.SelectObjectsFromStoredProcedure(typeof(DOContactInfo),
                "FindJobContractorsWithTradeCategory", parms);
        }

        #endregion

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="CreatedBy">The user creating the contact.</param>
        /// <param name="ContactType">The contact type.</param>
        /// <returns>The new contact.</returns>
        public DOContact CreateContact(Guid CreatedBy, DOContact.ContactTypeEnum ContactType)
        {

            DOContact Contact = new DOContact();

            Contact.CreatedBy = CreatedBy;
            Contact.CreatedDate = DateAndTime.GetCurrentDateTime();
            Contact.ContactID = Guid.NewGuid();
            Contact.ContactType = ContactType;
            //Contact.PendingUser = true;
            if (ContactType == DOContact.ContactTypeEnum.Company)
                Contact.UserName = Contact.ContactID.ToString();
            return Contact;
        }


        /// <summary>
		/// Creates a new contractorCustomer as a company.
		/// </summary>
		/// <param name="CreatedBy">The user creating the contact.</param>
		/// <param name="ContactType">The contact type.</param>
		/// <returns>The new contact.</returns>
		public DOContractorCustomer CreateContractorCustomer(Guid CreatedBy, Guid CustomerID, Guid ContractorID, string Address1, string Address2, string Address3, string Address4, string CompanyName,
             DOContractorCustomer.LinkedEnum Linked, string Phone, Guid Creator, string FirstName, string LastName, int CustomerType)
        {
            

            DOContractorCustomer ContractorCustomer = new DOContractorCustomer();
            ContractorCustomer.ContactCustomerId = Guid.NewGuid();
            if (Creator.ToString() == "00000000-0000-0000-0000-000000000000") Creator = ContractorCustomer.ContactCustomerId; //jared 2017.4.25
            ContractorCustomer.CreatedBy = CreatedBy;
            ContractorCustomer.ContractorId = ContractorID;
            ContractorCustomer.CustomerID = CustomerID;
            ContractorCustomer.Active = true;
            ContractorCustomer.Address1 = Address1;
            ContractorCustomer.Address2 = Address2;
            ContractorCustomer.Address3 = Address3;
            ContractorCustomer.Address4 = Address4;
            ContractorCustomer.CreatedDate = DateTime.Now;
            //ContractorCustomer.CustomerType = DOContractorCustomer.CustomerTypeEnum.Company;
            ContractorCustomer.Deleted = false;
            ContractorCustomer.Linked = Linked;
            ContractorCustomer.Phone = Phone;
            ContractorCustomer.CreatorContractorCustomer = Creator;
            //When a thirdparty contractorcustomer is added it will always be customertype=1     (individual) 
            //when the site owner details are updated we will give the option for company or not. 2017.4.25
            if (FirstName == "" && CompanyName == "" && LastName == "")
            {
                DOContractorCustomer docc = SelectContractorCustomerByCCID(Creator);
                FirstName = "Pending";
                LastName = "Customer";
            }
            ContractorCustomer.FirstName = FirstName;
            ContractorCustomer.LastName = LastName;
            ContractorCustomer.CompanyName = CompanyName;
            ContractorCustomer.CustomerType = (DOContractorCustomer.CustomerTypeEnum)CustomerType;

            return ContractorCustomer;


        }

        //Tony modifying. 5/11/2016
        // Selects all contact without condition
        public List<DOBase> SelectContacts()
        {
            return CurrentDAContact.SelectObjectsOrderByWhereClause(typeof(DOContact), "LEN(companyName)>0",
                "companyName", "");
            //            return CurrentDAContact.SelectObjectsOrderBy(typeof(DOContact),"companyName");
        }



        /// <summary>
        /// Get all the contractors for logged in entity
        /// </summary>
        /// <param name="loggedInContactId"></param>
        /// <returns></returns>
        public List<DOBase> SelectOtherContractors(Guid loggedInContactId)
        {
            StringBuilder query = new StringBuilder(@"SELECT DISTINCT c.FirstName,
															 c.ContactID,
															 c.LastName,
															 c.Email,
															 c.Phone,
															 c.ContactType,
															 c.Address1,
															 c.Address2,
															 c.Address3,
															 c.Address4,
															 c.CompanyName,
															 c.CreatedBy,
															 c.CreatedDate,
															 c.Active,
                                                             cc.CreatorContractorCustomer 
														FROM ContractorCustomer cc
														INNER JOIN ContactCompany ccom ON ccom.companyID = cc.customerid
														INNER JOIN Contact c ON c.ContactID=cc.ContractorID");
            return CurrentDAContact.SelectQueryListofObjects(typeof(DOContactInfo), query, "ccom.ContactID={0}",
                loggedInContactId);
        }

        /// <summary>
        /// Find all the existing contractors for a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List of contractors for a particular customer</returns>
        public List<DOBase> SelectMyContractors(Guid customerId)
        {
            StringBuilder query = new StringBuilder(@"select c.FirstName,c.ContactID,c.LastName,c.Email,c.Phone,c.ContactType,c.Address1,c.Address2,c.Address3,c.Address4,c.CompanyName,c.CreatedBy,c.CreatedDate,c.Active from contractorcustomer cc
			 inner join Contact c on c.ContactID = cc.ContractorID");
            return CurrentDAContact.SelectQueryListofObjects(typeof(DOContactInfo), query, "cc.customerid={0}",
                 customerId);

        }

        /// <summary>
        /// Deletes a contact. This will not cascade so any other references to this contact must be deleted first.
        /// </summary>
        /// <param name="Contact"></param>
        public void DeleteContact(DOContact Contact)
        {
            if (Contact.ContactID == Guid.Empty || Contact.ContactID == Constants.Guid_DefaultUser)
                throw new Exception("This contact cannot be deleted.");
            CheckManagedCompanies(Contact.ContactID);
            CurrentDAContact.DeleteObject(Contact);
        }
        private void CheckManagedCompanies(Guid ContactID)
        {
            List<DOBase> ManagedCompanies = SelectCompaniesManaged(ContactID);
            if (ManagedCompanies.Count > 0)
            {
                string CompanyNames = string.Empty;
                foreach (DOContact c in ManagedCompanies)
                {
                    if (!string.IsNullOrEmpty(CompanyNames)) CompanyNames += ", ";
                    CompanyNames += c.DisplayName;
                }
                throw new Exception("The contact cannot be deleted as they manage the following companies: " + CompanyNames);
            }
        }
        public void DeleteContactComplete(DOContact Contact)
        {
            if (Contact.ContactID == Guid.Empty || Contact.ContactID == Constants.Guid_DefaultUser)
                throw new Exception("This contact cannot be deleted.");
            CheckManagedCompanies(Contact.ContactID);
            DeleteContactCustomers(Contact);
            DeleteContactCompanyLinks(Contact);
            DeleteContact(Contact);
        }

        /// <summary>
        /// Selects a single contact.
        /// </summary>
        /// <param name="ContactID">The contact ID.</param>
        /// <returns>The contact, or null if no contact with the contact ID.</returns>
        public DOContact SelectContact(Guid ContactID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContact), "ContactID = {0}", ContactID) as DOContact;
        }

        /// <summary>
        /// Selects a single contact.
        /// </summary>
        /// <param name="ContactID">The contact ID.</param>
        /// <returns>The contact, or null if no contact with the contact ID.</returns>
        public DOContactInfo SelectAContact(Guid ContactID)
        {
            StringBuilder query = new StringBuilder("select contactid,FirstName,LastName,Email,Phone,ContactType,Address1,Address2,Address3,Address4,CompanyName,CreatedBy,CreatedDate,Active from Contact");
            return CurrentDAContact.SelectQuery(typeof(DOContactInfo), query, "ContactID = {0}", ContactID) as DOContactInfo;
        }
        /// <summary>
        /// Selects a single contact.
        /// </summary>
        /// <param name="Username">The username of the contact.</param>
        /// <returns>The contact, or null if no contact with the username.</returns>
        public DOContact SelectContactByUsername(string Username)
        {
            return CurrentDAContact.SelectObject(typeof(DOContact), "Username = {0}", Username) as DOContact;
        }

        /// <summary>
        /// Selects all contacts with a specified email address.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public List<DOBase> SelectContactsByEmail(string Email)
        {
            return CurrentDAContact.SelectObjects(typeof(DOContact), "Email = {0} AND Active = 1 ORDER BY CreatedDate DESC", Email);
        }


        /// <summary>
        /// Selects a contact by the company key.
        /// </summary>
        /// <param name="CompanyKey"></param>
        /// <returns></returns>
        public DOContact SelectContactByCompanyKey(string CompanyKey)
        {
            return CurrentDAContact.SelectObject(typeof(DOContact), "CompanyKey = {0} AND Active = 1", CompanyKey) as DOContact;
        }


        /// <summary>
        /// Saves a contact to the database.
        /// </summary>
        /// <param name="Contact">The contact.</param>
        public void SaveContact(DOContact Contact)
        {
            CurrentDAContact.SaveObject(Contact);
        }

        /// <summary>
        /// Searches all contacts.
        /// </summary>
        /// <param name="ContactType">The contact type converted to int, or -1 to include all contacts.</param>
        /// <param name="Subscribed">Subscription status of contact, or null to include all contacts.</param>
        /// <param name="Term">The search term to filter on username, first name, last name, company name, or null for no filter.</param>
        /// <returns>The list of matching contacts.</returns>
        public List<DOBase> SearchContacts(int ContactType, bool? Subscribed, string Term, bool Active)
        {
            object SubscribedValue = null;
            if (Subscribed.HasValue) SubscribedValue = Subscribed.Value;

            SortedList<string, object> Params = new SortedList<string, object>();
            Params.Add("ContactType", ContactType);
            if (Subscribed.HasValue) Params.Add("Subscribed", Subscribed.Value);
            if (!string.IsNullOrEmpty(Term)) Params.Add("Term", Term);
            Params.Add("Active", Active);

            return CurrentDAContact.SelectObjectsFromStoredProcedure(typeof(DOContact), "ContactSearch", Params);
        }

        public List<DOBase> SearchLinkedContacts(Guid customerid)
        {

            SortedList<string, object> Params = new SortedList<string, object>();
            Params.Add("customerid", customerid);
            return CurrentDAContact.SelectObjectsFromStoredProcedure(typeof(DOContractorCustomer), "FindLinkedCustomers", Params);
        }
        /// <summary>
        /// Selects all subscribed contacts.
        /// </summary>
        /// <returns></returns>
        public List<DOBase> SelectSubscribedContacts()
        {
            return CurrentDAContact.SelectObjects(typeof(DOContact), "Active = 1 AND (SubscriptionPending = 1 OR Subscribed = 1) ORDER BY ContactType DESC, LastName, CompanyName");
        }
        //Select all searchable and subscribed contractors
        public List<DOBase> SelectSearchableContractors()
        {
            return CurrentDAContact.SelectObjects(typeof(DOContact), "Searchable=1");
        }
        /// <summary>
        /// Selects the contact details of contractors for a job.
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobContractorContacts(Guid JobID)
        {
            //commented by jared 23/5
            //string Query = "SELECT c.* FROM JobContractor jc INNER JOIN Contact c ON jc.ContactID = c.ContactID WHERE jc.JobID = {0} AND c.Active = 1 ORDER BY ContactType DESC, LastName, CompanyName";
            string Query = @"SELECT distinct c.ContactID,c.FirstName,c.LastName,c.Email,c.Phone,
c.ContactType,c.Address1,c.Address2,c.Address3,c.Address4,c.CompanyName,c.createdby,c.createddate,c.active FROM JobContractor jc 
INNER JOIN Contact c ON jc.ContactID = c.ContactID 
WHERE jc.JobID = {0} AND c.Active = 1 
ORDER BY ContactType DESC, LastName, CompanyName";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), Query, JobID);

            //commented by jared 23/5
            //return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, JobID);
        }

        /// <summary>
        /// Selects the contact details of contractors for a job. 23/7/2017
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobContractorContractorCustomer(Guid JobID)
        {
                        string Query = @"Select distinct cc.ContactCustomerID,cc.ContractorID,cc.customerid,cc.linked,cc.deleted,cc.pendingsiteowner,creatorcontractorcustomer,cc.FirstName,cc.LastName,cc.Email,cc.Phone,
                                        cc.CustomerType,cc.Address1,cc.Address2,cc.Address3,cc.Address4,cc.CompanyName,cc.createdby,cc.createddate,cc.active
                                        from
										(select distinct c.contactid from contact, JobContractor jc INNER JOIN Contact c ON jc.ContactID = C.ContactID 
                                            WHERE jc.JobID = {0} AND c.Active = 1
										) a, ContractorCustomer cc where a.ContactID=cc.ContractorID and a.ContactID=cc.CustomerId
                                        ORDER BY CustomerType DESC, LastName, CompanyName";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContractorCustomer), Query, JobID);

           
        }

        /// <summary>
        /// Gets the path to the specified register control
        /// </summary>
        /// <param name="ContactType">The contact type.</param>
        /// <returns>The path to the control.</returns>
        public string GetRegisterControlPath(DOContact.ContactTypeEnum ContactType)
        {
            if (ContactType == DOContact.ContactTypeEnum.Company)
                return "~/UserControls/RegisterCompany.ascx";
            else
                return "~/UserControls/RegisterIndividual.ascx";
        }

        #region Contact Companies
        /// <summary>
        /// Generates a company key and validates that it doesnt exist already.
        /// </summary>
        /// <returns></returns>
        public string GenerateCompanyKey()
        {
            string CompanyKey;
            DOContact CheckContact = null;
            do
            {
                CompanyKey = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                CheckContact = SelectContactByCompanyKey(CompanyKey);
            } while (CheckContact != null);

            return CompanyKey;
        }

        /// <summary>
        /// Creates a contact company link.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOContactCompany CreateContactCompany(Guid ContactID, Guid CompanyID, Guid CreatedBy)
        {
            DOContactCompany ContactCompany = new DOContactCompany();
            ContactCompany.ContactCompanyID = Guid.NewGuid();
            ContactCompany.CreatedBy = CreatedBy;
            ContactCompany.ContactID = ContactID;
            ContactCompany.CompanyID = CompanyID;

            return ContactCompany;
        }


        /// <summary>
        /// Saves a contact company link.
        /// </summary>
        /// <param name="ContactCompany"></param>
        public void SaveContactCompany(DOContactCompany ContactCompany)
        {
            CurrentDAContact.SaveObject(ContactCompany);
        }

        /// <summary>
        /// Selects a contact company, if there exists a link between the contact as same as company.
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="companyID"></param>
        /// <returns>Contact Company record</returns>
        public Guid SelectContactCompany(Guid contactID, Guid companyID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT ContactCompanyID FROM ContactCompany 
													WHERE ContactID = @contactID AND companyID = @companyID");
                cmd.Parameters.AddWithValue("@contactID", contactID);
                cmd.Parameters.AddWithValue("@companyID", companyID);
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                Guid contactCompanyID = (Guid)cmd.ExecuteScalar();
                return contactCompanyID;
            }
            catch (Exception exception)
            {
                return Guid.Empty;
            }
            finally
            {
                connection?.Close();
            }
        }

        public DOContactCompany SelectContactCompany(Guid ContactCompanyID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContactCompany), "ContactCompanyID = {0}", ContactCompanyID) as DOContactCompany;
        }


        /// <summary>
        /// Selects a contactcompany
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="CompanyID"></param>
        public DOContactCompany SelectAContactCompany(Guid ContactID, Guid CompanyID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContactCompany), "ContactID = {0} and companyid={1}", ContactID, CompanyID) as DOContactCompany;
        }

        /// <summary>
        /// Removes all company links from a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        public void DeleteContactCompanyLinks(DOContact Contact)
        {
            List<DOBase> ccAll = CurrentDAContact.SelectObjects(typeof(DOContactCompany), "ContactID = {0} OR companyID = {0}", Contact.ContactID);
            foreach (DOContactCompany cc in ccAll)
            {
                CurrentDAContact.DeleteObject(cc);
            }
        }



        // Martin Falconer 21/06
        /// <summary>
        /// Selects all Vehicles that belong to a contact
        /// </summary>
        /// <param name="ContactID">Contact's Guid</param>        
        /// <returns>List of Vehicles belonging to a contact</returns>
        public List<DOBase> SelectContactVehicles(Guid ContactID, bool ApprovedOnly = true)
        {
            //Query to select all vehicles related owned by a contact, returning all vehicle data necessary with driver's first name
            //			string Query = @"SELECT v.VehicleID, v.VehicleDriver, v.VehicleRegistration, v.VehicleName, v.WOFDueDate, v.RegoDueDate, v.InsuranceDueDate, c.FirstName , v.CreatedBy, v. CreatedDate, V.Active FROM Vehicle v LEFT JOIN Contact c ON v.VehicleDriver = c.ContactID WHERE v.VehicleOwner = {0}";

            //Tony modified 21.Feb.2017
            string Query = @"SELECT v.VehicleID, v.VehicleDriver, v.VehicleRegistration, v.VehicleName, v.WOFDueDate, v.RegoDueDate, v.InsuranceDueDate, e.FirstName , v.CreatedBy, v. CreatedDate, V.Active FROM Vehicle v LEFT JOIN EmployeeInfo e ON v.VehicleDriver = e.EmployeeID WHERE v.VehicleOwner = {0}";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOForVehicleInput), Query, ContactID, ApprovedOnly);
        }

        /// <summary>
        /// Gets a list of all companies linked to the contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="ApprovedOnly">Only include non-pending entries in results.</param>
        /// <returns></returns>
        public List<DOBase> SelectContactCompanies(Guid ContactID)
        {
            return SelectContactCompanies(ContactID, false, true);
        }

        /// <summary>
        /// Gets a list of all companies linked to the contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="ApprovedOnly">Only include non-pending entries in results.</param>
        /// <returns></returns>
        public List<DOBase> SelectContactCompanies(Guid ContactID, bool IncludeInactive, bool ApprovedOnly)
        {
            string Query = @"SELECT c.ContactID
									, c.Username
									, c.PasswordHash
									, c.FirstName
									, c.LastName
									, c.CompanyName
									, c.Email
									, c.Phone
									, c.Address1
									, c.Address2
									, c.CreatedBy
									, c.CreatedDate
									, c.Active
									, c.ContactType
									, c.BankAccount
									, c.SubscriptionExpiryDate
									, c.SubscriptionPending
									, c.Subscribed
									, c.ManagerID
									, c.CompanyKey
									, c.PendingUser
									, c.CustomerExclude
									, c.DefaultQuoteRate
									, c.DefaultChargeUpRate
									, c.JobNumberAuto
									, c.Address3
									, c.Address4
									, c.iCount
									, c.Searchable
									, c.PendingSiteOwner
									, c.DefaultRegion
                                    , cc.Settings
							FROM ContactCompany cc
							INNER JOIN Contact c ON cc.companyID = c.ContactID
							WHERE cc.ContactID = {0}
									AND(c.Active = 1 OR 1 = 1)
									AND((cc.Pending = 0 AND cc.Active = 1) OR 1 = 0)  
									AND cc.CompanyID != {0}
							ORDER BY c.CompanyName";
            //added cc.settings 2017.4.24 jared
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, ContactID, ApprovedOnly, IncludeInactive);
        }

        /// <summary>
        /// Select all contacts that belong to a company.
        /// </summary>
        /// <param name="dOContact"></param>
        /// <returns></returns>
        public List<DOBase> SelectCompanyContacts(Guid CompanyID, bool ApprovedOnly = true)
        {
            string Query = "SELECT c.* FROM ContactCompany cc INNER JOIN Contact c ON cc.ContactID = c.ContactID WHERE cc.companyID = {0} AND c.Active = 1 AND ((cc.Pending = 0 AND cc.Active = 1) OR {1} = 0) ORDER BY c.LastName";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, CompanyID, ApprovedOnly);
        }

        //Tony added on 19.Feb.2017
        /// <summary>
        /// /// <param name="DOEmployeeInfo"></param>
        /// Select all employees that belong to a company.
        /// </summary>
        public List<DOBase> SelectCompanyEmployee(Guid CompanyID, bool ApprovedOnly = true)
        {
            string Query = "SELECT e.* FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID WHERE cc.companyID = {0} AND e.Active = 1 AND ((cc.Pending = 0 AND cc.Active = 1) OR {1} = 0) ORDER BY e.LastName";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOEmployeeInfo), Query, CompanyID, ApprovedOnly);
        }

        //Tony added on 22.Feb.2017 begin
        public List<DOBase> SelectCompanyEmployeeWithVehicle(Guid CompanyID)
        {
            //            string query = @"SELECT e.EmployeeID, e.FirstName, e.LastName,v.VehicleName, v.VehicleRegistration, v.createdBy, 
            //                            v.createdDate, v.Active, e.ContactCompanyID, " +
            //                           "IsDefault = CASE v.VehicleID WHEN e.DefaultVehicleID THEN 'Default' ELSE 'Secondary' END " +
            //                           "FROM Vehicle v, ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID " +
            //                           "WHERE cc.CompanyID = {0} AND e.Active = 1 AND((cc.Pending = 0 AND cc.Active = 1)) " +
            //                           "AND v.VehicleDriver = e.EmployeeID ORDER BY e.LastName";
            string query =
                 @"SELECT  e.EmployeeID, e.FirstName, e.LastName, v.VehicleName, v.VehicleRegistration, NewKey = CONVERT(NCHAR(36),e.EmployeeID) + CONVERT(NCHAR(36),v.vehicleID),
                           DisplayInfo = e.FirstName + ' ' + e.LastName + ',' + v.VehicleName + ' ' + v.VehicleRegistration
                                        +' ('+ CASE v.VehicleID WHEN e.DefaultVehicleID THEN 'Default' ELSE 'Secondary' END +')',
                           v.CreatedDate , v.Active ,v.CreatedBy, v.VehicleID 
                     FROM  Vehicle v ,ContactCompany cc
                    INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID
                    WHERE   cc.CompanyID = {0} AND e.Active = 1 AND ( (cc.Pending = 0 AND cc.Active = 1))
                      AND v.VehicleDriver = e.EmployeeID
                 ORDER BY e.LastName";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOEmployeeVehicle), query, CompanyID);
        }
        //        public List<DOBase> SelectCompanyEmployeeWithVehicle(Guid CompanyID, bool ApprovedOnly = true)
        //        {
        //            string Query = "SELECT e.EmployeeID, e.FirstName+' '+ e.LastName+','+ v.VehicleName + ' '+ v.VehicleRegistration +' '" +
        //                           "CASE v.VehicleID WHEN e.DefaultVehicleID THEN 'Default' ELSE 'Secondary' END AS TotalInfo" +
        //                           "FROM Vehicle v, ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID " +
        //                           "WHERE cc.CompanyID = {0} AND e.Active = 1 AND((cc.Pending = 0 AND cc.Active = 1) OR {1} = 0) " +
        //                           "AND v.VehicleDriver = e.EmployeeID ORDER BY e.LastName";
        //
        //            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOEmployeeVehicle), Query, CompanyID, ApprovedOnly);
        //        }



        //Tony added on 22.Feb.2017 end


        /// <summary>
        /// Select all contacts that belong to a company that have vehicles
        /// </summary>
        /// <param name="dOContact"></param>
        /// <returns></returns>
        public List<DOBase> SelectCompanyContactsWithAVehicle(Guid CompanyID, bool ApprovedOnly = true)
        {
            string Query = "SELECT c.* FROM Vehicle V, ContactCompany cc INNER JOIN Contact c ON cc.ContactID = c.ContactID WHERE cc.CompanyID = {0} AND c.Active = 1 AND ((cc.Pending = 0 AND cc.Active = 1) OR {1} = 0) and c.contactid = v.vehicledriver ORDER BY c.LastName";
            //string Query = "SELECT e.* FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactID = e.ContactCompanyID LEFT JOIN Contact c ON cc.ContactID = c.ContactID WHERE cc.CompanyID = {0} AND c.Active = 1 ORDER BY c.LastName";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, CompanyID, ApprovedOnly);
        }



        ///// <summary>
        ///// Select all contacts that belong to a company and their vehicles
        ///// </summary>
        ///// <param name="dOContact"></param>
        ///// <returns></returns>
        //public List<DOBase> SelectCompanyContactsAndVehicleName(Guid CompanyID, bool ApprovedOnly = true)
        //{
        //    string Query = "SELECT c.contactid, c.Firstname, c.lastname, vehicle.vehiclename, vehicle.vehicleregistration FROM Vehicle, ContactCompany cc INNER JOIN Contact c ON cc.ContactID = c.ContactID WHERE vehicle.driverid=c.contactid AND cc.CompanyID = {0} AND c.Active = 1 AND ((cc.Pending = 0 AND cc.Active = 1) OR {1} = 0) ORDER BY c.LastName";
        //    //string Query = "SELECT e.* FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactID = e.ContactCompanyID LEFT JOIN Contact c ON cc.ContactID = c.ContactID WHERE cc.CompanyID = {0} AND c.Active = 1 ORDER BY c.LastName";
        //    return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, CompanyID, ApprovedOnly);
        //}




        /// <summary>
        /// Selects a job template by contractorid
        /// </summary>
        /// <param name="ContractorID"></param>
        /// <returns></returns>
        public List<DOBase> SelectJobTemplatesByCompanyID(Guid ContractorID)
        {

            return CurrentDAContact.SelectObjects(typeof(DOJobTemplate), "ContractorID = {0}", ContractorID);
        }





        /// <summary>
        /// Creat contractorcustomer record for the company and contractor
        /// </summary>
        /// <param name="contractorCustomer"></param>
        /// <param name="contactCompany"></param>
        /// <returns></returns>
        public DOContractorCustomer SetContractorCustomer(DOContractorCustomer contractorCustomer, DOContact contactCompany, Guid Creator)
        {
            contractorCustomer.Active = false;
            contractorCustomer.Address1 = contactCompany.Address1;
            contractorCustomer.Address2 = contactCompany.Address2;
            contractorCustomer.Address3 = contactCompany.Address3;
            contractorCustomer.Address4 = contactCompany.Address4;
            contractorCustomer.CompanyName = contactCompany.CompanyName;
            contractorCustomer.Phone = contactCompany.Phone;
            contractorCustomer.Linked = DOContractorCustomer.LinkedEnum.AwaitingCust;
            contractorCustomer.CustomerType = DOContractorCustomer.CustomerTypeEnum.Company;
            contractorCustomer.CreatorContractorCustomer = Creator;
            return contractorCustomer;
        }



        /// <summary>
        /// Send email to customer
        /// </summary>
        public void InformCustomerByEmail(DOContact contact)
        {
            string subject = "You have been invited to Ontrack";
            StringBuilder body = new StringBuilder();
            body.AppendFormat("<strong>{0} </strong>has added you ( {1} ) as a customer of theirs. As you will be aware " +
                              "this gives you many benefits including:-" +
                              "<ul><li>You can view the progress of any job that {0} is working on for you.</li>" +
                              "<li>You can keep track of all the jobs done on your site in the past.</li>" +
                              "<li>You can request other contractors through OnTrack to work on this job or any other jobs " +
                              "and track their progress too.</li>" +
                              "<li>You can create you own projects to work on.</li></ul>",
                              contact.DisplayName, contact.Email);
            body.AppendFormat("<br/><br/>  Please <a href='http://localhost:63323/'> click here </a>to be taken to<a href='http://localhost:63323/'> OnTrack</a> to log in and link to Electracraft Ltd. <br/><br/>");
            BRGeneral brGeneral = new BRGeneral();
            brGeneral.SendConfirmationEmail(Constants.EmailSender, "jared@ecraft.co.nz", subject, body);
        }

        /// <summary>
        /// Select employee info of all contacts that belong to a company.
        /// </summary>
        /// <param name="dOContact"></param>
        /// <returns></returns>
        public List<DOBase> SelectCompanyEmployees(Guid CompanyID, bool ApprovedOnly = true)
        {
          //string Query = "SELECT e.*, cc.ContactID, cc.Active CCActive, cc.Pending CCPending FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID LEFT JOIN Contact c ON cc.ContactID = c.ContactID WHERE cc.CompanyID = {0} AND c.Active = 1 AND ((cc.Pending = 0 AND cc.Active = 1) OR {1} = 0) ORDER BY e.LastName";
            //Tony modified 2016
            string Query = "SELECT e.*, cc.ContactID, cc.Active CCActive, cc.Pending CCPending FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID LEFT JOIN Contact c ON cc.ContactID = c.ContactID WHERE cc.CompanyID = {0} AND c.Active = 1 AND ((cc.Pending = 0) OR {1} = 0) ORDER BY e.LastName";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactEmployee), Query, CompanyID, ApprovedOnly);
        }

        public List<DOBase> SelectCompanyEmployeesWithoutInfo(Guid CompanyID)
        {
            string Query =
@"select c.* from contactcompany cc
left join contact c on cc.contactid = c.contactid   
left join employeeinfo e on cc.contactcompanyid = e.contactcompanyid
where cc.companyid = {0} and e.employeeid is null";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, CompanyID);
        }

        /// <summary>
        /// CheckCompanyContact
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public bool CheckCompanyContact(Guid CompanyID, Guid ContactID)
        {
            DOBase ret = CurrentDAContact.SelectObject(typeof(DOContactCompany), "ContactID = {0} AND companyID = {1}", ContactID, CompanyID);
            return (ret != null);
        }


        /// <summary>
        /// Deletes a contact company link.
        /// </summary>
        /// <param name="ContactCompany"></param>
        public void DeleteContactCompany(DOContactCompany ContactCompany)
        {
            //jared 2017.4.24
            //DOEmployeeInfo ei = SelectEmployeeInfo(ContactCompany.ContactID, ContactCompany.CompanyID);
            //if (ei != null)
            //    DeleteEmployeeeInfo(ei); commented jared 2017.4.24 was effecting contactcompanies.ascx.cs line 156
            CurrentDAContact.DeleteObject(ContactCompany);
        }
        #endregion

        #region Contact Customers
        /// <summary>
        /// Creates a contact customer link entry.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public DOContractorCustomer CreateContactCustomer(Guid ContactID, Guid CustomerID, Guid Creator)
        {
            DOContractorCustomer contractorCustomer = new DOContractorCustomer();
            contractorCustomer.ContactCustomerId = Guid.NewGuid();
            if (Creator == null) Creator = contractorCustomer.ContactCustomerId;
            contractorCustomer.ContractorId = ContactID;
            contractorCustomer.CustomerID = CustomerID;
            contractorCustomer.Active = false;
            contractorCustomer.CreatorContractorCustomer = Creator;
            return contractorCustomer;
        }

        /// <summary>
        /// Saves a contact customer link.
        /// </summary>
        /// <param name="contractorCustomer"></param>
        public void SaveContractorCustomer(DOContractorCustomer contractorCustomer)
        {
            CurrentDAContact.SaveObject(contractorCustomer);
        }


        /// <summary>
        /// Selects a customer contact link.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public DOContractorCustomer SelectContactCustomer(Guid ContactID, Guid CustomerID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContractorCustomer), "ContractorID = {0} AND CustomerID = {1}", ContactID, CustomerID) as DOContractorCustomer;
        }
        /// <summary>
        /// Selects an active customer contractor link. 
        /// To find out if there is already a relationship between contractor and customer
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="CustomerID"></param>
        /// <returns>contractorcustomer object</returns>
        public DOContractorCustomer SelectContractorCustomer(Guid ContactID, Guid CustomerID)
        {
            StringBuilder query = new StringBuilder(@"SELECT ContactCustomerID,
																ContractorID,
																customerid,
																FirstName,
																LastName,
																Phone,
																Address1,
																Address2,
																Address3,
																Address4,
																CompanyName,
																CreatedBy,
																CreatedDate,
																Active,
																linked,
                                                                Deleted,
																CustomerType,
                                                                PendingSiteOwner,
                                                                Email,
                                                                CreatorContractorCustomer 
														FROM ContractorCustomer");
            return (DOContractorCustomer)CurrentDAContact.SelectQuery(typeof(DOContractorCustomer), query, "ContractorID = {0} AND customerid = {1}", ContactID, CustomerID);
        }

        /// <summary>
        /// Deletes a customer from a contact.
        /// </summary>
        /// <param name="cc"></param>
        public void DeleteContactCustomer(DOContractorCustomer cc)
        {
            /*Update the deleted column in the table, instead of deleting the record*/
            cc.Deleted = true;
            SaveContractorCustomer(cc);
        }

        /// <summary>
        /// Deletes all contact customer entries for a contact.
        /// </summary>
        /// <param name="CustomerID"></param>
        public void DeleteContactCustomers(DOContact Contact)
        {
            //Delete contact customers.
            string Query = "SELECT cc.* FROM Customer c LEFT JOIN contractorCustomer cc ON c.CustomerID = cc.CustomerID WHERE c.ContactID = {0}";
            List<DOBase> ccAll = CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContractorCustomer), Query, Contact.ContactID);
            foreach (DOBase cc in ccAll)
            {
                CurrentDAContact.DeleteObject(cc);
            }

            // Delete customers.
            string Query2 = "SELECT * FROM Customer WHERE ContactID = {0}";
            List<DOBase> cAll = CurrentDAContact.SelectObjectsCustomQuery(typeof(DOCustomer), Query2, Contact.ContactID);
            foreach (DOBase c in cAll)
            {
                CurrentDAContact.DeleteObject(c);
            }
        }

        /// <summary>
        /// Selects the companies managed by a user.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectCompaniesManaged(Guid ContactID)
        {
            return CurrentDAContact.SelectObjects(typeof(DOContact), "ManagerID = {0}", ContactID);
        }
        /// <summary>
        /// Selects the customers for a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectContactCustomers(Guid ContactID)
        {
            //            string Query = "SELECT c. * FROM contractorCustomer cc INNER JOIN Contact c ON c.ContactID = cc.CustomerID WHERE cc.ContactID = {0} ORDER BY c.LastName";
            string Query =

@"SELECT DISTINCT c.* FROM Contact c 
LEFT JOIN contractorCustomer cc ON c.ContactID = cc.CustomerID 
LEFT JOIN Site s ON c.ContactID = s.JobOwner 
LEFT JOIN Job j ON s.SiteID = j.SiteID
LEFT JOIN JobContractor jc ON j.JobID = jc.JobID
WHERE c.Active = 1 AND (cc.ContactID = {0} OR jc.ContactID = {0})";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, ContactID);
        }

        /// <summary>
        /// Select the contacts for a customer where the contact has directly added that customer (ie. is not a customer because they
        /// were assigned as contractor or task or job)
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectCustomerContacts(Guid ContactID)
        {
            string Query =
@"SELECT c.* FROM Contact c
INNER JOIN contractorCustomer cc ON c.ContactID = cc.ContactID
INNER JOIN Customer cu ON cc.CustomerID = cu.CustomerID 
WHERE cu.ContactID = {0}
ORDER BY c.LastName, c.CompanyName";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, ContactID);
        }
        #endregion

        #region Customers
        /// <summary>
        /// Creates a customer.
        /// </summary>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOContact CreateCustomer(Guid CreatedBy)
        {
            DOContact Customer = new DOContact();
            //Customer. = Guid.NewGuid();
            Customer.CreatedBy = CreatedBy;

            return Customer;
        }

        //Delect Customer
        public void DeleteCustomer(DOContact contact, DOContact customer)
        {
            DOContractorCustomer CC = SelectContactCustomer(contact.ContactID, customer.ContactID) as DOContractorCustomer;
            if (CC != null)
                DeleteContactCustomer(CC);
            CurrentDAContact.DeleteObject(customer);
            // DOContact cont = SelectContact(customer.ContactID);
            //  DeleteContact(cont);

        }

        /// <summary>
        /// Selects a customer.
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public DOContact SelectCustomer(Guid CustomerID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContact), "contactID = {0}", CustomerID) as DOContact;
        }
        /// <summary>
        /// Selects contractor customer record.
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public List<DOBase> SelectContractorCustomer(Guid contactId)
        {
            StringBuilder query = new StringBuilder("SELECT CreatedBy,CreatedDate,Active,Linked,ContractorID,customerid FROM ContractorCustomer ");
            return CurrentDAContact.SelectQueryListofObjects(typeof(DOContractorCustomerLinkedInfo), query, "(customerid = {0} OR ContractorID = {0}) AND Linked > 0", contactId);
        }


        /// <summary>
        /// Selects a single contractorCustomer.
        /// </summary>
        /// <param name="ContractorCustomerID">The contractorCustomer ID.</param>
        /// <returns>The contact, or null if no contact with the contact ID.</returns>
        public DOContractorCustomer SelectContractorCustomerByCCID(Guid ContactID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContractorCustomer), "ContactCustomerID = {0}", ContactID) as DOContractorCustomer;
        }




        /// <summary>
        /// Selects a customer of a contact by contact id.
        /// </summary>
        /// <param name="CompanyContactID"></param>
        /// <param name="CustomerContactID"></param>
        /// <returns></returns>
        public DOCustomer SelectCustomerByContactID(Guid CompanyContactID, Guid CustomerContactID)
        {
            string Query = @"SELECT cu.*
								FROM contractorCustomer cc
								LEFT JOIN Customer cu ON cc.CustomerID = cu.CustomerID
								WHERE cc.ContactID = {0} AND cu.ContactID = {1}";

            List<DOBase> ret = CurrentDAContact.SelectObjectsCustomQuery(typeof(DOCustomer), Query, CompanyContactID, CustomerContactID);
            if (ret.Count == 0) return null;
            return ret[0] as DOCustomer;
        }

        /// <summary>
        /// Saves a customer.
        /// </summary>
        /// <param name="Customer"></param>
        public void SaveCustomer(DOContact Customer)
        {
            CurrentDAContact.SaveObject(Customer);
        }

        /// <summary>
        /// Selects the customers of a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
		public List<DOBase> SelectActiveCustomers(Guid ContactID)
        {
            string query =
                @"SELECT c.contactid,
							cc.FirstName,
							cc.LastName,
							cc.Address1,
							cc.Address2,
							cc.Address3,
							cc.Address4,
							c.Email,
							cc.Phone,
							cc.CompanyName,
							cc.CustomerType,
							c.CreatedBy,
							c.CreatedDate,
							c.Active,
							c.ContactType,
                            cc.CreatorContractorCustomer
					FROM Contact c 
					JOIN ContractorCustomer cc ON cc.customerid = c.ContactID
					WHERE cc.ContractorID = {0} AND cc.Active = 1 and deleted=0 order by firstname, companyname";
            //jared 13.2.17 was not compatible with live site
            //       @"SELECT c.contactid,
            //		cc.FirstName,
            //		cc.LastName,
            //		cc.Address1,
            //		cc.Address2,
            //		cc.Address3,
            //		cc.Address4,
            //		c.Email,
            //		cc.Phone,
            //		cc.CompanyName,
            //		cc.CustomerType,
            //		c.CreatedBy,
            //		c.CreatedDate,
            //		c.Active,
            //		c.ContactType
            //FROM Contact c 
            //JOIN ContractorCustomer cc ON cc.customerid = c.ContactID
            //WHERE cc.ContractorID = {0} AND cc.Active = 1 and cc.Del = 0";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, ContactID);
        }


        /// <summary>
        /// Selects the customers of a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectAllCustomers(Guid ContactID)
        {
            string query =
                @"SELECT c.contactid,
							cc.FirstName,
							cc.LastName,
							cc.Address1,
							cc.Address2,
							cc.Address3,
							cc.Address4,
							c.Email,
							cc.Phone,
							cc.CompanyName,
							cc.CustomerType,
							c.CreatedBy,
							c.CreatedDate,
							c.Active,
							c.ContactType,
                            cc.CreatorContractorCustomer
                    FROM Contact c 
					JOIN ContractorCustomer cc ON cc.customerid = c.ContactID
					WHERE cc.ContractorID = {0} order by firstname, companyname";
          
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, ContactID);
        }

        /// <summary>
        /// Selects the customers of a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectAllNonDeletedCustomers(Guid ContactID)
        {
            string query =
                @"SELECT c.contactid,
							cc.FirstName,
							cc.LastName,
							cc.Address1,
							cc.Address2,
							cc.Address3,
							cc.Address4,
							c.Email,
							cc.Phone,
							cc.CompanyName,
							cc.CustomerType,
							c.CreatedBy,
							c.CreatedDate,
							cc.Active,
							c.ContactType,
                            cc.CreatorContractorCustomer
					FROM Contact c 
					JOIN ContractorCustomer cc ON cc.customerid = c.ContactID
					WHERE cc.ContractorID = {0} and deleted=0 order by cc.active desc, firstname, companyname";

                       return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, ContactID);
        }


        //jared 2017.2.15
        /// <summary>
        /// Selects the contractors of a contact. Unsure what customertype is for
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectContractors(Guid ContactID)
        {
            string query =
                @"SELECT c.contactid,
							c.FirstName,
							c.LastName,
							c.Address1,
							c.Address2,
							c.Address3,
							c.Address4,
							c.Email,
							c.Phone,
							c.CompanyName,
							c.ContactType as customertype,
							c.CreatedBy,
							c.CreatedDate,
							c.Active,
							c.ContactType,
                             cc.CreatorContractorCustomer
					FROM Contact c 
					JOIN ContractorCustomer cc ON cc.contractorid = c.ContactID
					WHERE cc.CustomerID = {0} AND cc.Active = 1";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, ContactID);
        }


        //Tony Added
        public List<DOBase> SelectSiteCustomers(Guid siteID) //!here!
        {
            string query =
                @"select distinct c.contactid,c.FirstName,c.LastName,c.Address1,c.Address2,c.Address3,
c.Address4,c.Email,c.Phone,c.CompanyName,c.ContactType,c.CreatedBy,c.CreatedDate,c.Active from Contact c 
join ContactSite cs on cs.ContactID=c.ContactID
where cs.siteID={0}";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, siteID);
        }

        //Tony added
        //2017.2.15 Jared modified, added 'order by' and removed where active=1
        public List<DOBase> SiteExcludeCustomers(Guid ContactID, Guid SiteID)
        {
            string query =
             @"select distinct c.contactid,c.FirstName,c.LastName,c.Address1,c.Address2,c.Address3,
c.Address4,c.Email,c.Phone,c.CompanyName,c.ContactType,c.CreatedBy,c.CreatedDate,c.Active,cc.creatorcontractorcustomer from Contact c 
join ContractorCustomer cc
on cc.CustomerID=c.ContactID
where cc.ContractorID={0} and
c.contactid NOT IN ( select contactid FROM contactSite WHERE siteID = {1}) order by c.firstname, c.CompanyName";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, ContactID, SiteID);
        }

        /// <summary>
        /// Selects the inactive customers of a contact.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectInactiveCustomers(Guid ContactID)
        {
            string query = @"select c.contactid,
									cc.FirstName,
									cc.LastName,
									cc.Address1,
									cc.Address2,
									cc.Address3,
									cc.Address4,
									c.Email,
									cc.Phone,
									cc.CompanyName,
									cc.CustomerType,
									c.CreatedBy,
									c.CreatedDate,
									c.Active,
									c.ContactType,
                                    cc.CreatorContractorCustomer
							FROM Contact c 
							JOIN ContractorCustomer cc ON cc.customerid = c.ContactID
							WHERE cc.ContractorID = {0} AND cc.Active=0 and deleted=0 order by firstname, companyname";
       //Jared 2017.2.14
       //     string query = @"select c.contactid,
							//		cc.FirstName,
							//		cc.LastName,
							//		cc.Address1,
							//		cc.Address2,
							//		cc.Address3,
							//		cc.Address4,
							//		c.Email,
							//		cc.Phone,
							//		cc.CompanyName,
							//		cc.CustomerType,
							//		c.CreatedBy,
							//		c.CreatedDate,
							//		c.Active,
							//		c.ContactType
							//FROM Contact c 
							//JOIN ContractorCustomer cc ON cc.customerid = c.ContactID
							//WHERE cc.ContractorID = {0} AND cc.Active=0 AND cc.Del=0";

            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContactInfo), query, ContactID);
        }
        /// <summary>
        /// Selects contacts have have asked the current user to contract for them.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectContractees(Guid ContactID)
        {
            //Select all contacts where:
            //- They are the job owner of a job we are a contractor on.
            //- They are the task owner of a task we are the contractor on.

            string Query =
@"WITH ContactIDs AS (
SELECT DISTINCT c.ContactID FROM Contact c
LEFT JOIN Job j ON c.ContactID = j.JobOwner
LEFT JOIN JobContractor jc ON jc.JobID = j.JobID
LEFT JOIN Task t ON c.ContactID = t.TaskOwner
WHERE jc.ContactID = {0} OR t.ContractorID = {0})
SELECT c.* FROM Contact c INNER JOIN ContactIDs ON c.ContactID = ContactIDs.ContactID
WHERE c.Active = 1";
            //            string Query = 
            //@"SELECT c.* FROM JobContractor jc 
            //LEFT JOIN Job j ON jc.JobID = j.JobID 
            //LEFT JOIN Site s ON j.SiteID = s.SiteID 
            //LEFT JOIN Contact c ON s.JobOwner = c.ContactID 
            //WHERE jc.ContactID = {0}";
            return CurrentDAContact.SelectObjectsCustomQuery(typeof(DOContact), Query, ContactID);
        }


        /// <summary>
        /// Selects the customer of a site for a contact.
        /// </summary>
        /// <param name="SiteID">The site.</param>
        /// <param name="ContactID">The contact who the customer is a customer of.</param>
        /// <returns></returns>
        public DOCustomer SelectSiteCustomer(Guid SiteID, Guid ContactID)
        {
            string Query = @"
SELECT cu.* from Site s
LEFT JOIN Customer cu on s.contactid = cu.contactid
LEFT JOIN contractorCustomer cc on cu.customerid = cc.customerid
WHERE s.SiteID = {0} AND cc.ContactID = {1}";

            List<DOBase> ret = CurrentDAContact.SelectObjectsCustomQuery(typeof(DOCustomer), Query, SiteID, ContactID);
            if (ret == null || ret.Count == 0) return null;
            return ret[0] as DOCustomer;

        }
        #endregion


        #region Employee Info
        /// <summary>
        /// Creates an employee info record. 
        /// </summary>
        /// <param name="ContactCompanyID"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public DOEmployeeInfo CreateEmployeeInfo(Guid ContactCompanyID, Guid CreatedBy)
        {
            DOEmployeeInfo Employee = new DOEmployeeInfo();
            Employee.ContactCompanyID = ContactCompanyID;
            Employee.EmployeeID = Guid.NewGuid();
            Employee.CreatedBy = CreatedBy;
            return Employee;
        }

        /// <summary>
        /// Saves an employee info record.
        /// </summary>
        /// <param name="Employee"></param>
        public void SaveEmployeeInfo(DOEmployeeInfo Employee)
        {
            CurrentDAContact.SaveObject(Employee);
        }

        /// <summary>
        /// Selects employee info.
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public DOEmployeeInfo SelectEmployeeInfo(Guid EmployeeID)
        {
            return CurrentDAContact.SelectObject(typeof(DOEmployeeInfo), "EmployeeID={0}", EmployeeID) as DOEmployeeInfo;
        }


        //Tony added 21.Feb.2017 begin
        public DOEmployeeInfo FindEmployeeByCar(Guid vehicleID)
        {
            return CurrentDAContact.SelectObject(typeof(DOEmployeeInfo), "DefaultVehicleID={0}", vehicleID) as DOEmployeeInfo;
        }

        //Tony added 21.Feb.2017 end



        public DOEmployeeInfo SelectEmployeeInfo(Guid ContactID, Guid CompanyID)
        {
            string Query = "SELECT e.*,cc.ContactID FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID WHERE cc.ContactID = {0} AND cc.companyID = {1}";
            List<DOBase> ret = CurrentDAContact.SelectObjectsCustomQuery(typeof(DOEmployeeInfo), Query, ContactID, CompanyID);
            if (ret.Count == 0)
                return null;
            else
                return ret[0] as DOEmployeeInfo;
        }

        /// <summary>
        /// Deletes an employee info record.
        /// </summary>
        /// <param name="ei"></param>
        public void DeleteEmployeeeInfo(DOEmployeeInfo ei)
        {
            CurrentDAContact.DeleteObject(ei);
        }
        #endregion

        #region Customer Invitation
        /// <summary>
        /// Create customer invitation.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="InviterID"></param>
        public void CreateCustomerInvitation(Guid ContactID, Guid InviterID)
        {
            DOCustomerInvitation CI = new DOCustomerInvitation();
            CI.ContactID = ContactID;
            CI.InviterID = InviterID;
            CI.CIID = Guid.NewGuid();
            CurrentDAContact.SaveObject(CI);
        }

        /// <summary>
        /// Selects all invitations a contact has received.
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public List<DOBase> SelectCustomerInvitations(Guid ContactID)
        {
            return CurrentDAContact.SelectObjects(typeof(DOCustomerInvitation), "ContactID = {0}", ContactID);
        }

        /// <summary>
        /// Deletes a customer invitation.
        /// </summary>
        /// <param name="CI"></param>
        public void DeleteCustomerInvitation(DOCustomerInvitation CI)
        {
            CurrentDAContact.DeleteObject(CI);
        }
        #endregion


        #region Verification
        public DOContactVerification CreateContactVerification(Guid ContactID)
        {
            DOContactVerification cv = new DOContactVerification();
            cv.ID = Guid.NewGuid();
            cv.CreatedBy = ContactID;
            cv.CreatedDate = DateTime.Now;
            cv.Active = true;
            cv.ContactID = ContactID;
            cv.VerificationCode = Guid.NewGuid().ToString().Replace("-", string.Empty);

            SaveContactVerification(cv);

            return cv;
        }

        public DOContactVerification SelectContactVerificationByContactID(Guid ContactID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContactVerification), "ContactID = {0}", ContactID) as DOContactVerification;
        }

        public void SaveContactVerification(DOContactVerification cv)
        {
            CurrentDAContact.SaveObject(cv);
        }

        public void DeleteContactVerification(DOContactVerification cv)
        {
            CurrentDAContact.DeleteObject(cv);
        }

        public DOContactVerification CheckVerification(string VerificationCode)
        {
            return CurrentDAContact.SelectObject(typeof(DOContactVerification), "VerificationCode = {0}", VerificationCode) as DOContactVerification;
        }
        #endregion
        #region Miscellaneous
        /// <summary>
        /// Gets a default user contact.
        /// </summary>
        /// <param name="Caption"></param>
        /// <returns></returns>
        public DOContact GetDefaultUser(string Caption)
        {
            return new DOContact() { ContactType = DOContact.ContactTypeEnum.Individual, ContactID = Constants.Guid_DefaultUser, FirstName = Caption, LastName = string.Empty };
        }
        /// Gets a default user contact.
        /// </summary>
        /// <param name="Caption"></param>
        /// <returns></returns>
        public DOContact GetPendingUser(string Caption)
        {
            return new DOContact() { ContactType = DOContact.ContactTypeEnum.Individual, ContactID = Constants.Guid_DefaultUser, FirstName = Caption, LastName = string.Empty };
        }
        #endregion
        public DOContact SelectCustomerbyContactID(Guid contactID)
        {
            return CurrentDAContact.SelectObject(typeof(DOContact), "ContactID={0}", contactID) as DOContact;
        }

        //To find the contact sites
        public List<DOBase> SelectContactSites(Guid contactID)
        {
            return CurrentDAContact.SelectObjects(typeof(DOContactSite), "ContactID={0}", contactID);
        }
        //Select all contact sites
        public List<DOBase> SelectContactSites()
        {
            return CurrentDAContact.SelectObjects(typeof(DOContactSite));
        }

        public void UpdateContactCustomer(Guid customerid, bool b, Guid contractorID)
        {
            string query = @"UPDATE ContractorCustomer SET Active = {0} WHERE customerid = {1} AND contractorID = {2}";
            CurrentDAContact.ExecuteScalar(query, b, customerid, contractorID);
        }

        //Tony Added 21.Feb.2017
        public void UpdateDefaultVehicle(Guid vehicleID, Guid vehicleDriver)
        {
            string query = @"UPDATE EmployeeInfo SET DefaultVehicleID = {0} WHERE employeeID = {1}";
            CurrentDAContact.ExecuteScalar(query, vehicleID, vehicleDriver);
        }

        public void ValidateName(string firstName, string lastName, string companyName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(companyName))
            {
                throw new Exception("First name/ Last name or Company name is required");
            }

        }
    }


}