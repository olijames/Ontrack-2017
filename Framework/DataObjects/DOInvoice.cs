using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    public enum InvoiceStatusEnum : int
    {
        InProgress = 0,
        AwaitingApproval = 1,
        SentToCustomer = 2,
        PaidByCustomer = 3,
        WrittenOff = 4
    }

    public enum DetailLevelEnum : int
    {
        TotalsOnly = 0,
        TaskNamesAndDescription = 1,
        TaskNamesAndDescritionAndCosts = 2,
        LabourAndMaterialsAndCosts = 3
        
    }

    [DatabaseTable("Invoice")]
    public class DOInvoice : DOBase
    {
        [DatabaseField("InvoiceID", IsPrimaryKey = true)]
        public Guid InvoiceID { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

        [DatabaseField("CustomerID")]
        public Guid CustomerID { get; set; }

        [DatabaseField("DueDate")]
        public DateTime DueDate { get; set; }

        [DatabaseField("InvoiceStatus")]
        public InvoiceStatusEnum InvoiceStatus { get; set; }

        [DatabaseField("Terms")]
        public string Terms { get; set; }

        [DatabaseField("SubmissionDetailLevel")]
        public DetailLevelEnum DetailLevel { get; set; }

        [DatabaseField("InvoiceDescription")]
        public string Description { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }

        [DatabaseField("SentDate")]
        public DateTime SentDate { get; set; }
    }
}
