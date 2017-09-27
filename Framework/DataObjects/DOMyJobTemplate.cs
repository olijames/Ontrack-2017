using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("MyJobTemplate")]
    public class DOMyJobTemplate : DOBase
    {
        
        [DatabaseField("TemplateTaskID", IsPrimaryKey = true)]
        public Guid TemplateTaskID { get; set; }

        [DatabaseField("StartDelay")]
        public decimal StartDelay { get; set; }

        [DatabaseField("Duration")]
        public decimal Duration { get; set; }

        [DatabaseField("TaskName")]
        public string TaskName { get; set; }

        [DatabaseField("Description")]
        public string Description { get; set; }

        [DatabaseField("TradeCategoryID")]
        public Guid TradeCategoryID { get; set; }

       


    }
}
