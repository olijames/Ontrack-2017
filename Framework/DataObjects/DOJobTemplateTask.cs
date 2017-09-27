using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobTemplateTask")]
    public class DOJobTemplateTask : DOBase
    {
        [DatabaseField("JobTemplateTaskID", IsPrimaryKey = true)]
        public Guid JobTemplateTaskID { get; set; }

        [DatabaseField("JobTemplateID")]
        public Guid JobTemplateID { get; set; }

        [DatabaseField("TemplateTaskID")]
        public Guid TemplateTaskID { get; set; }
        
        [DatabaseField("StartDelay")]
        public decimal StartDelay { get; set; }

        [DatabaseField("Duration")]
        public decimal Duration { get; set; }


    }
}
