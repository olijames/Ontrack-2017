using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobQuote")]
    public class DOJobQuote : DOBase
    {
        public enum JobQuoteStatus : int
        {
            Quoted = 0,
            Accepted = 1,
            Declined = 2
        }
        [DatabaseField("QuoteID", IsPrimaryKey = true)]
        public Guid QuoteID { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("QuotedAmount")]
        public decimal QuotedAmount { get; set; }

        [DatabaseField("QuoteAccepted")]
        public bool QuoteAccepted { get; set; }

        [DatabaseField("Comment")]
        public string Comment { get; set; }

        [DatabaseField("QuoteStatus")]
        public JobQuoteStatus QuoteStatus { get; set; }

        [DatabaseField("Margin")]
        public decimal Margin { get; set; }

        [DatabaseField("QuoteStatusDate")]
        public DateTime QuoteStatusDate { get; set; }

        [DatabaseField("TermsAndConditions")]
        public string TermsAndConditions { get; set; }
    }
}
