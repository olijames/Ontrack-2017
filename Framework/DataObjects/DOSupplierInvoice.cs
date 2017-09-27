namespace Electracraft.Framework.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [DatabaseTable("SupplierInvoice")]
    public partial class DOSupplierInvoice : DOBase
    {
        [DatabaseField("SupplierInvoiceID", IsPrimaryKey = true)]
        public Guid SupplierInvoiceID { get; set; }


        [DatabaseField("SupplierID")]
        public Guid SupplierID { get; set; }

        [DatabaseField("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [DatabaseField("ContractorReference")]
        public string ContractorReference { get; set; }

        [DatabaseField("SupplierReference")]
        public string SupplierReference { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

        [DatabaseField("TotalExGst")]
        public decimal TotalExGst { get; set; }


    }
}
