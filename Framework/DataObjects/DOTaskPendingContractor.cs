using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TaskPendingContractor")]
    public class DOTaskPendingContractor : DOBase
    {
        [DatabaseField("TPCID", IsPrimaryKey = true)]
        public Guid TPCID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }

        [DatabaseField("ContractorEmail")]
        public string ContractorEmail { get; set; }
    }
}
