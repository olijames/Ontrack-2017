using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;
using Electracraft.Framework.Utility;

namespace Electracraft.Framework.DataAccess
{
	public class DAContact : DABase
	{
		public DAContact(string ConnectionString)
			: base(ConnectionString)
		{ }


		public override void Validate(DOBase obj)
		{
			DOContact Contact = obj as DOContact;
			DOCustomer Customer = obj as DOCustomer;

			if (Contact != null)
				ValidateContact(Contact);

			if (Customer != null)
				ValidateCustomer(Customer);
		}

		/// <summary>
		/// Validates that a customer is valid.
		/// </summary>
		/// <param name="Customer"></param>
		private void ValidateCustomer(DOCustomer Customer)
		{
			if (string.IsNullOrEmpty(Customer.FirstName))
				throw new FieldValidationException("Customer First Name is required.");
			if (string.IsNullOrEmpty(Customer.LastName))
				throw new FieldValidationException("Customer Last Name is required.");
			if (string.IsNullOrEmpty(Customer.Email))
				throw new FieldValidationException("Customer Email is required.");
			ValidateEmail(Customer.Email);
		}

		private void ValidateEmail(string Email)
		{
			if (!Email.Contains("@") || !Email.Contains("."))
				throw new FieldValidationException(Email + " is not a valid email address.");
		}
		/// <summary>
		/// Validates that all the fields of the contact are valid.
		/// </summary>
		/// <param name="Contact"></param>
		private void ValidateContact(DOContact Contact)
		{
			if (Contact.ContactID == Guid.Empty)
			{
				if (Contact.PersistenceStatus == ObjectPersistenceStatus.Existing)
					throw new FieldValidationException("Site Admin account cannot be updated.");
			}
			if (Contact.ContactID == Constants.Guid_DefaultUser)
			{
				throw new FieldValidationException("Default user account cannot be edited.");
			}

			if (Contact.ContactID == Constants.Guid_DefaultUser)
			{
				throw new FieldValidationException("Default User account cannot be updated.");
			}

			bool IsCompany = Contact.ContactType == DOContact.ContactTypeEnum.Company;
			if (string.IsNullOrEmpty(Contact.Email))
				throw new FieldValidationException(string.Format("{0}Email address is required.", IsCompany ? "Company " : string.Empty));
			ValidateEmail(Contact.Email);

			if (string.IsNullOrEmpty(Contact.Phone))
				throw new FieldValidationException(string.Format("{0}Phone is required.", IsCompany ? "Company " : string.Empty));
			if (string.IsNullOrEmpty(Contact.Address1))
				throw new FieldValidationException(string.Format("{0}Address is required.", IsCompany ? "Company " : string.Empty));

			if (Contact.ContactType == DOContact.ContactTypeEnum.Company)
			{
				if (string.IsNullOrEmpty(Contact.CompanyName))
					throw new FieldValidationException("Company Name is required.");
			}

			if (Contact.ContactType == DOContact.ContactTypeEnum.Individual)
			{
				Contact.UserName = Contact.Email;

				DOContact CheckUsernameContact = SelectContactByUsername(Contact.UserName);
				if (CheckUsernameContact != null && CheckUsernameContact.ContactID != Contact.ContactID)
					throw new FieldValidationException("The email address " + Contact.UserName + " is already in use.");
			}
		}

		/// <summary>
		/// Selects a contact by username.
		/// </summary>
		/// <param name="Username">The username.</param>
		/// <returns>The contact, or null if no matching contact.</returns>
		public DOContact SelectContactByUsername(string Username)
		{
			return SelectObject(typeof(DOContact), "Username = {0}", Username) as DOContact;
		}


	}
}
