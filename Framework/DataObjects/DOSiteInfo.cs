using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
   public class DOSiteInfo:DOBase
    {
        [DatabaseField("SiteID", IsPrimaryKey = true)]
        public Guid SiteId { get; set; }

        [DatabaseField("SiteOwnerID")]
        public Guid SiteOwnerId { get; set; }

        [DatabaseField("Address1")]
        public string Address1 { get; set; }

        [DatabaseField("Address2")]
        public string Address2 { get; set; }

        [DatabaseField("Address3")]
        public string Address3 { get; set; }

        [DatabaseField("Address4")]
        public string Address4 { get; set; }

        [DatabaseField("FirstName")]
        public string OwnerFirstName { get; set; }

        [DatabaseField("LastName")]
        public string OwnerLastName { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactId { get; set; }

        
        public int JobsCount { get; set; }

        // Tony Added 9/11/2016
        public string DisplayAddress
        {
            get
            {
               return string.Format("{0}, {1}", Address1, Address2);
            }
        }
    }
}
