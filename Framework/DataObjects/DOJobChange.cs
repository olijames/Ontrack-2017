using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobChange")]
    public class DOJobChange : DOBase
    {
        public enum JobChangeType : int
        {
            QuoteAccepted = 0,
            JobCompleted = 1,
            JobUncompleted = 2
        }

        [DatabaseField("JobChangeID", IsPrimaryKey = true)]
        public Guid JobChangeID { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("ChangeType")]
        public JobChangeType ChangeType { get; set; }
    }
}
