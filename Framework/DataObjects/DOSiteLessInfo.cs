using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOSiteLessInfo : DOBase
    {
        [DatabaseField("SiteID", IsPrimaryKey = true)]
        public Guid SiteId { get; set; }

        [DatabaseField("Address1")]
        public string Address1 { get; set; }

        [DatabaseField("Address2")]
        public string Address2 { get; set; }

        [DatabaseField("Address3")]
        public string Address3 { get; set; }

        [DatabaseField("Address4")]
        public string Address4 { get; set; }

        [DatabaseField("SiteOwnerID")]
        public Guid SiteOwnerId { get; set; }
    }
}
