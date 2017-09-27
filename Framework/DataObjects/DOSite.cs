using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    public enum SiteVisibility : int
    {
        None = 0,
        Selected = 1,
        All = 2
    }

    [DatabaseTable("Site")]
    public class DOSite : DOBase
    {
        [DatabaseField("SiteID", IsPrimaryKey = true)]
        public Guid SiteID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("SiteOwnerID")]
        public Guid SiteOwnerID { get; set; }

        [DatabaseField("Address1")]
        public string Address1 { get; set; }

        [DatabaseField("Address2")]
        public string Address2 { get; set; }

        [DatabaseField("Address3")]
        public string Address3 { get; set; }

        [DatabaseField("Address4")]
        public string Address4 { get; set; }

        [DatabaseField("CustomerFirstName")]
        public string OwnerFirstName { get; set; }

        [DatabaseField("CustomerLastName")]
        public string OwnerLastName { get; set; }

        [DatabaseField("CustomerAddress1")]
        public string OwnerAddress1 { get; set; }

        [DatabaseField("CustomerAddress2")]
        public string OwnerAddress2 { get; set; }

        [DatabaseField("CustomerPhone")]
        public string OwnerPhone { get; set; }

        [DatabaseField("CustomerEmail")]
        public string OwnerEmail { get; set; }

        //[DatabaseField("JobOwner")]
        //public Guid JobOwner { get; set; }
        [DatabaseField("VisibilityStatus")]
        public SiteVisibility VisibilityStatus { get; set; }
    }
}
