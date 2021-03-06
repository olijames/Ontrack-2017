﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Task")]
    public class DOTask : DOBase
    {



        public enum TaskTypeEnum : int
        {
            Standard = 0,
            Reference = 1,
            Acknowledgement = 2
            //Quote = 4,
            //QuickTask = 8,
            //Appointment = 16
        }

        public enum TaskStatusEnum : int
        {
            Incomplete = 0,
            Complete = 1,
            Amended = 2,
            Pending = 3,
            Invoiced=4,
            Paid=5,
            WrittenOff=6,
            TemplateTask=7
        }
        public enum LMVisibilityEnum : int
        {
            InternalOnly = 0,
            All = 1
        }

        [DatabaseField("TaskID", IsPrimaryKey = true)]
        public Guid TaskID { get; set; }
        [Required]
        [DatabaseField("JobID")]
        public Guid JobID { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

        [DatabaseField("ParentTaskID")]
        public Guid ParentTaskID { get; set; }

        [DatabaseField("TaskName")]
        public string TaskName { get; set; }

        [DatabaseField("TaskType")]
        public TaskTypeEnum TaskType { get; set; }

        [DatabaseField("Description")]
        public string Description { get; set; }

        [DatabaseField("TaskOwner")]
        public Guid TaskOwner { get; set; }

        [DatabaseField("Status")]
        public TaskStatusEnum Status { get; set; }

        [DatabaseField("StartDate")]
        public DateTime StartDate { get; set; }

        [DatabaseField("StartMinute")]
        public int StartMinute { get; set; }

        [DatabaseField("EndDate")]
        public DateTime EndDate { get; set; }

        [DatabaseField("EndMinute")]
        public int EndMinute { get; set; }

        [DatabaseField("Appointment")]
        public bool Appointment { get; set; }

        [DatabaseField("AmendedTaskID")]
        public Guid AmendedTaskID { get; set; }

        [DatabaseField("LMVisibility")]
        public LMVisibilityEnum LMVisibility { get; set; }

        [DatabaseField("InvoiceToType")]
        public InvoiceRecipient InvoiceToType { get; set; }

        [DatabaseField("AmendedBy")]
        public Guid AmendedBy { get; set; }

        [DatabaseField("AmendedDate")]
        public DateTime AmendedDate { get; set; }

        [DatabaseField("TradeCategoryID")]
        public Guid TradeCategoryID { get; set; }

        [DatabaseField("TaskNumber")]
        public int TaskNumber { get; set; }

        [DatabaseField("TaskCustomerID")]
        public Guid TaskCustomerID { get; set; }

        [DatabaseField("SiteID")]
        public Guid SiteID { get; set; }

        [DatabaseField("QuoteID")]
        public Guid QuoteID { get; set; }

        [DatabaseField("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

    }
}
