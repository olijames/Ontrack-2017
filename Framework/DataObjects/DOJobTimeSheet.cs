using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobTimeSheet")]
    public class DOJobTimeSheet : DOBase
    {
        [DatabaseField("TimeSheetID", IsPrimaryKey=true)]
        public Guid TimeSheetID { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("TimeSheetDate")]
        public DateTime TimeSheetDate { get; set; }

        [DatabaseField("StartMinute")]
        public int StartMinute { get; set; }

        [DatabaseField("EndMinute")]
        public int EndMinute { get; set; }

        [DatabaseField("Comment")]
        public string Comment { get; set; }
    }
}
