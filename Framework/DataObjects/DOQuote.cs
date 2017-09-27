using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    public enum QuoteStatusEnum : int
    {
        InProgress = 0,
        AwaitingApproval = 1,
        SentToCustomer = 2,
        AcceptedByCustomer = 3,
        AcceptedByContractor = 4,
        DeclinedByCustomer = 5,
        DeclinedByContractor = 6

    }
   
    [DatabaseTable("Quote")]
    public class DOQuote : DOBase
    {
        [DatabaseField("QuoteID", IsPrimaryKey = true)]
        public Guid QuoteID { get; set; }

        [DatabaseField("QuoteStatus")]
        public QuoteStatusEnum QuoteStatus { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

        [DatabaseField("CustomerID")]
        public Guid CustomerID { get; set; }

        [DatabaseField("Terms")]
        public string Terms { get; set; }

        [DatabaseField("QuoteAcceptorID")]
        public Guid QuoteAcceptorID  { get; set; }

        [DatabaseField("SubmissionDetailLevel")]
        public DetailLevelEnum DetailLevel { get; set; }

        [DatabaseField("QuoteDescription")]
        public string Description { get; set; }

        [DatabaseField("ExpiryDate")]
        public DateTime ExpiryDate { get; set; }

    }
}
