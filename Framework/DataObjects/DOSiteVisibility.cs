using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("SiteVisibility")]
    public class DOSiteVisibility : DOBase
    {
        [DatabaseField("SVID", IsPrimaryKey = true)]
        public Guid SVID { get; set; }

        [DatabaseField("SiteID")]
        public Guid SiteID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }
    }
}
