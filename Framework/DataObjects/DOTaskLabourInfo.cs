﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
	[DatabaseTable("")]
	public class DOTaskLabourInfo : DOBase
	{
		[DatabaseField("TaskLabourID", IsPrimaryKey = true)]
		public Guid TaskLabourID { get; set; }

		[DatabaseField("TaskID")]
		public Guid TaskID { get; set; }


		[DatabaseField("Description")]
		public string Description { get; set; }

		[DatabaseField("InvoiceDescription")]
		public string InvoiceDescription { get; set; }

		[DatabaseField("ContactID")]
		public Guid ContactID { get; set; }

		[DatabaseField("LabourDate")]
		public DateTime LabourDate { get; set; }

		[DatabaseField("StartMinute")]
		public int StartMinute { get; set; }

		[DatabaseField("EndMinute")]
		public int EndMinute { get; set; }


		[DatabaseField("LabourType")]
		public TaskMaterialType LabourType { get; set; }

		[DatabaseField("LabourRate")]
		public decimal LabourRate { get; set; }


		[DatabaseField("QuoteNumber")]
		public int QuoteNumber { get; set; }

		[DatabaseField("InvoiceID")]
		public Guid InvoiceID { get; set; }

		[DatabaseField("InvoiceQuantity")]
		public decimal InvoiceQuantity { get; set; }


	}
}
