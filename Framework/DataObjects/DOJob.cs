using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    public enum InvoiceRecipient : int
    {
        Customer = 0,
        SiteOwner = 1,
        JobOwner = 2
    }

    [DatabaseTable("Job")]
    public class DOJob : DOBase
    {
        public enum JobTypeEnum : int
        {
            ToQuote = 0,
            Quoted = 1,
            ChargeUp = 2
        }

        public enum JobStatusEnum : int
        {
            Incomplete = 0,
            Complete = 1
           
        }

        public enum JobAccessType : int
        {
            PhoneFirst = 0,
            LockBox = 1,
            Key = 2,
            Custom = 3
        }
        
        [DatabaseField("JobID", IsPrimaryKey = true)]
        public Guid JobID { get; set; }

        [DatabaseField("SiteID")]
        public Guid SiteID { get; set; }

        [DatabaseField("JobOwner")]
        public Guid JobOwner { get; set; }

        [DatabaseField("Name")]
        public string Name { get; set; }

        [DatabaseField("JobNumber")]
        public string JobNumber { get; set; }

        [DatabaseField("ProjectManagerID")]
        public Guid ProjectManagerID { get; set; }

        [DatabaseField("ProjectManagerText")]
        public string ProjectMangerText { get; set; }

        [DatabaseField("ProjectManagerPhone")]
        public string ProjectManagerPhone { get; set; }

        [DatabaseField("InvoiceTo")]
        public Guid InvoiceTo { get; set; }

        [DatabaseField("JobType")]
        public JobTypeEnum JobType { get; set; }

        [DatabaseField("JobStatus")]
        public JobStatusEnum JobStatus { get; set; }

        [DatabaseField("AccessType")]
        public JobAccessType AccessType { get; set; }

        [DatabaseField("AccessTypeCustom")]
        public string AccessTypeCustom { get; set; }

        [DatabaseField("PoweredItems")]
        public string PoweredItems { get; set; }

        [DatabaseField("NoPoweredItems")]
        public bool NoPoweredItems { get; set; }

        [DatabaseField("AlarmCode")]
        public string AlarmCode { get; set; }

        [DatabaseField("SiteNotes")]
        public string SiteNotes { get; set; }

        [DatabaseField("StockRequired")]
        public string StockRequired { get; set; }

        [DatabaseField("QuotedAmount")]
        public decimal QuotedAmount { get; set; }

        [DatabaseField("CompletedDate")]
        public DateTime CompletedDate { get; set; }

        [DatabaseField("CompletedBy")]
        public Guid CompletedBy { get; set; }

        [DatabaseField("IncompleteTasksReason")]
        public string IncompleTasksReason { get; set; }

        [DatabaseField("InvoiceToType")]
        public InvoiceRecipient InvoiceToType { get; set; }

        //[DatabaseField("JobNumberAuto")]
        //public string JobNumberAuto { get; set; }
        [DatabaseField("Description")]
        public string Description { get; set; }
    }
}
