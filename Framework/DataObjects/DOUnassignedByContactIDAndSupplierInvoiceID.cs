using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOUnassignedByContactIDAndSupplierInvoiceID : DOBase
    {

        //Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, SupplierInvoice.SupplierReference, SupplierInvoice.SupplierInvoiceID

        [DatabaseField("SupplierInvoiceID", IsPrimaryKey = true)]
        public Guid SupplierInvoiceID { get; set; }

        [DatabaseField("SupplierName")]
        public string SupplierName { get; set; }

        [DatabaseField("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [DatabaseField("ContractorReference")]
        public string ContractorReference { get; set; }

        [DatabaseField("SupplierReference")]
        public string SupplierReference { get; set; }

        [DatabaseField("TotalExGst")]
        public decimal TotalExGst { get; set; }


    }
}
