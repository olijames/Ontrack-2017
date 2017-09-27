using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobContractor")]
    public class DOJobContractor : DOBase
    {
        [DatabaseField("JobContractorID", IsPrimaryKey = true)]
        public Guid JobContractorID { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        /// <summary>
        /// 0 - Job is active for this jobcontractor
        /// 1 - Job is inactive for this jobcontractor
        /// </summary>
        [DatabaseField("Status")]
        public int Status { get; set; }

        [DatabaseField("JobNumberAuto")]
        public long JobNumberAuto { get; set; }
    }
}
