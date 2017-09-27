using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
 
    [DatabaseTable("")]
    public class DOTaskMaterialInfo : DOBase
    {
        
        [DatabaseField("TaskMaterialID", IsPrimaryKey = true)]
        public Guid TaskMaterialID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }

        [DatabaseField("Description")]
        public string Description { get; set; }

        [DatabaseField("Quantity")]
        public decimal Quantity { get; set; }

        [DatabaseField("MaterialType")]
        public TaskMaterialType MaterialType { get; set; }

        [DatabaseField("MaterialName")]
        public string MaterialName { get; set; }

        [DatabaseField("SellPrice")]
        public decimal SellPrice { get; set; }

        //added 10/10/16
        [DatabaseField("QuoteNumber")]
        public int QuoteNumber { get; set; }

        [DatabaseField("QuoteID")]
        public Guid QuoteID { get; set; }

        [DatabaseField("InvoiceID")]
        public Guid InvoiceID { get; set; }

        [DatabaseField("InvoiceQuantity")]
        public decimal InvoiceQuantity { get; set; }

        //public decimal TotalSell()
        //{
        //    decimal i = 0;
        //    i = Quantity * SellPrice;
        //    return i;
        //}

        //public decimal TotalCost()
        //{
        //    decimal i = 0;
        //    i = Quantity * CostPrice;
        //    return i;
        //}

    }
}
