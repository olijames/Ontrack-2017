using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("TaskJob")]
    public class DOTaskJob : DOBase
    {

        [DatabaseField("TaskID", IsPrimaryKey = true)]
        public Guid TaskID { get; set; }
        [Required]

        [DatabaseField("TotalMaterial")]
        public decimal TotalMaterial { get; set; }

        [DatabaseField("TotalLabour")]
        public decimal TotalLabour { get; set; }

        [DatabaseField("JobID")]
        public Guid JobID { get; set; }



        //[DatabaseField("ContractorID")]
        //public Guid ContractorID { get; set; }

        //[DatabaseField("ParentTaskID")]
        //public Guid ParentTaskID { get; set; }

        [DatabaseField("TaskName")]
        public string TaskName { get; set; }

        [DatabaseField("Name")]
        public string JobName { get; set; }

        [DatabaseField("jobnumberauto")]
        public string jobnumberauto { get; set; }

        //[DatabaseField("Description")]
        //public string Description { get; set; }

        //[DatabaseField("TaskOwner")]
        //public Guid TaskOwner { get; set; }

        //[DatabaseField("StartDate")]
        //public DateTime StartDate { get; set; }

        //[DatabaseField("StartMinute")]
        //public int StartMinute { get; set; }

        //[DatabaseField("EndDate")]
        //public DateTime EndDate { get; set; }

        //[DatabaseField("EndMinute")]
        //public int EndMinute { get; set; }

        //[DatabaseField("Appointment")]
        //public bool Appointment { get; set; }

        //[DatabaseField("AmendedTaskID")]
        //public Guid AmendedTaskID { get; set; }

        //[DatabaseField("InvoiceToType")]
        //public InvoiceRecipient InvoiceToType { get; set; }

        //[DatabaseField("AmendedBy")]
        //public Guid AmendedBy { get; set; }

        //[DatabaseField("AmendedDate")]
        //public DateTime AmendedDate { get; set; }

        //[DatabaseField("TradeCategoryID")]
        //public Guid TradeCategoryID { get; set; }

        [DatabaseField("TaskNumber")]
        public int TaskNumber { get; set; }

        [DatabaseField("TaskCustomerID")]
        public Guid TaskCustomerID { get; set; }

        //[DatabaseField("SiteID")]
        //public Guid SiteID { get; set; }

        //[DatabaseField("QuoteID")]
        //public Guid QuoteID { get; set; }

        //[DatabaseField("InvoiceNumber")]
        //public int InvoiceNumber { get; set; }

        [DatabaseField("Status")]
        public int Status { get; set; }

        //[DatabaseField("Total")]
        //public decimal Total { get; set; }

        ////[DatabaseField("LabourTotal")]
        ////public decimal LabourTotal { get; set; }



    }
}
