using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TaskQuote")]
    public class DOTaskQuote : DOBase
    {
        public enum TaskQuoteStatus : int
        {
            Quoted = 0,
            Accepted = 1,
            Declined = 2
        }

        [DatabaseField("TaskQuoteID", IsPrimaryKey = true)]
        public Guid TaskQuoteID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }

        [DatabaseField("Status")]
        public TaskQuoteStatus Status { get; set; }

        [DatabaseField("TermsAndConditions")]
        public string TermsAndConditions { get; set; }

        [DatabaseField("Margin")]
        public decimal Margin { get; set; }
    }
}
