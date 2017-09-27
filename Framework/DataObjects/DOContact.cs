using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Contact")]
    public class DOContact : DOContactBase
    {
        public enum ContactTypeEnum : int
        {
            Company = 0,
            Individual = 1
        }

        [DatabaseField("ContactID", IsPrimaryKey = true)]
        public Guid ContactID { get; set; }

        public override Guid ID
        {
            get { return ContactID; }
        }
        public override string ContactBaseType
        {
            get { return "Contact"; }
        }
        [DatabaseField("Username")]
        public string UserName { get; set; }

        //[DatabaseField("Settings")]
        //public ContactSettingsEnum Settings { get; set; }

        [DatabaseField("PasswordHash")]
        public string PasswordHash { get; set; }


        [DatabaseField("ContactType")]
        public ContactTypeEnum ContactType { get; set; }

        [DatabaseField("BankAccount")]
        public string BankAccount { get; set; }

        [DatabaseField("SubscriptionExpiryDate")]
        public DateTime SubscriptionExpiryDate { get; set; }

        [DatabaseField("SubscriptionPending")]
        public bool SubscriptionPending { get; set; }

        [DatabaseField("Subscribed")]
        public bool Subscribed { get; set; }

        [DatabaseField("ManagerID")]
        public Guid ManagerID { get; set; }

        [DatabaseField("CompanyKey")]
        public string CompanyKey { get; set; }

        [DatabaseField("PendingUser")]
        public bool PendingUser { get; set; }

        public override string DisplayName
        {
            get
            {
                if (ContactType == ContactTypeEnum.Company)
                    return CompanyName;
                else
                    return string.Format("{0} {1}", FirstName, LastName);
            }
          
        }

        [DatabaseField("CustomerExclude")]
        public bool CustomerExclude { get; set; }

        [DatabaseField("DefaultQuoteRate")]
        public decimal DefaultQuoteRate { get; set; }

        [DatabaseField("DefaultChargeUpRate")]
        public decimal DefaultChargeUpRate { get; set; }

        [DatabaseField("JobNumberAuto")]
        public int JobNumberAuto { get; set; }

        [DatabaseField("Searchable")]
        public int Searchable { get; set; }

        [DatabaseField("PendingSiteOwner")]
        public bool PendingSiteOwner { get; set; }

        [DatabaseField("DefaultRegion")]
        public Guid DefaultRegion { get; set; }
    }
}
