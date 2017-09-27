using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("ContractorCustomer")]
    public class DOContractorCustomer : DOBase
    {
        public enum LinkedEnum:int
        {
            NotLinked=4,
            AwaitingCust=1,
            AwaitingContractor=2,
            Linked=0
        }
		public enum CustomerTypeEnum : int
		{
			Company = 0,
			Individual = 1,
			Self = 2
		}
		[DatabaseField("ContactCustomerID", IsPrimaryKey = true)]
        public Guid ContactCustomerId { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorId { get; set; }
       
        [DatabaseField("CustomerID")]
        public Guid CustomerID { get; set; }

        [DatabaseField("FirstName")]
        public string FirstName { get; set; }

        [DatabaseField("LastName")]
        public string LastName { get; set; }

        [DatabaseField("Phone")]
        public string Phone { get; set; }

        [DatabaseField("Address1")]
        public string Address1 { get; set; }

        [DatabaseField("Address2")]
        public string Address2 { get; set; }

        [DatabaseField("Address3")]
        public string Address3 { get; set; }

        [DatabaseField("Address4")]
        public string Address4 { get; set; }

        [DatabaseField("CompanyName")]
        public string CompanyName { get; set; }
        public string DisplayName
        {
            get
            {
                if (CustomerType==CustomerTypeEnum.Company)
                    return CompanyName;
                else
                {
					return string.Format("{0} {1}", FirstName, LastName);
				}
                    
            }
        }

             


        [DatabaseField("Linked")]
        public LinkedEnum Linked
        { get; set; }

        [DatabaseField("Deleted")]
        public bool Deleted
        { get; set; }

        [DatabaseField("CustomerType")]
		public CustomerTypeEnum CustomerType
		{ get; set; }

        [DatabaseField("PendingSiteOwner")]
        public bool PendingSiteOwner
        { get; set; }

        [DatabaseField("Email")]
        public string Email
        { get; set; }

        [DatabaseField("CreatorContractorCustomer")]
        public Guid CreatorContractorCustomer
        { get; set; }



    }
}
