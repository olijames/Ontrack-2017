using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobFile")]
    public class DOJobFile : DOBase
    {
        [DatabaseField("JobFileID", IsPrimaryKey = true)]
        public Guid JobFileID { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("FileID")]
        public Guid FileID { get; set; }
    }
}
