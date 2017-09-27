using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOActiveJobsForContractor : DOBase
    {
        [DatabaseField("JobContractorID", IsPrimaryKey = true)]
        public Guid JobContractorID { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("status")]
        public int Status { get; set; }

        [DatabaseField("SiteID")]
        public Guid SiteID { get; set; }

    }
}
