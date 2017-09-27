using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    public enum TaskMaterialType : int
    {
        Quoted = 0,
        Required = 1,
        Actual = 2
    }

    [DatabaseTable("TaskMaterial")]
    public class DOTaskMaterial : DOBase
    {
        
        [DatabaseField("TaskMaterialID", IsPrimaryKey = true)]
        public Guid TaskMaterialID { get; set; }

        [DatabaseField("TaskID")]
        public Guid TaskID { get; set; }

        [DatabaseField("MaterialID")]
        public Guid MaterialID { get; set; }

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

        [DatabaseField("FromInvoice")]
        public bool FromInvoice { get; set; }

        [DatabaseField("FromVehicle")]
        public bool FromVehicle { get; set; }

        [DatabaseField("FromCustom")]
        public bool FromCustom { get; set; }

        //added 10/10/16
        [DatabaseField("QuoteNumber")]
        public int QuoteNumber { get; set; }

        [DatabaseField("QuoteID")]
        public Guid QuoteID { get; set; }

        [DatabaseField("InvoiceID")]
        public Guid InvoiceID { get; set; }

        [DatabaseField("InvoiceQuantity")]
        public decimal InvoiceQuantity { get; set; }


        [DatabaseField("CustomerID")]
        public Guid CustomerID { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

    }
}
