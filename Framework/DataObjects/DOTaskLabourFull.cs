using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOTaskLabourFull : DOTaskLabour
    {
        [DatabaseField("JobName")]
        public string JobName { get; set; }

        [DatabaseField("JobNumber")]
        public string JobNumber { get; set; }

        [DatabaseField("JobNumberAuto")]
        public string JobNumberAuto { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("InvoiceQuantity")]
        public decimal InvoiceQuantity { get; set; }

        [DatabaseField("JobDescription")]
        public string JobDescription { get; set; }

        [DatabaseField("Customer")]
        public string Customer { get; set; }

        [DatabaseField("TaskName")]
        public string TaskName { get; set; }


    }
}
