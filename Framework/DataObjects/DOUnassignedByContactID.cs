using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOUnassignedByContactID : DOBase
    {

        //Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, SupplierInvoice.SupplierReference,
        //SupplierInvoiceMaterial.QTY, Material.Description, Material.CostPrice, material.SellPrice, Material.RRP, Material.UOM, tempID, 
        //, Material.MaterialID, SupplierInvoiceMaterial.SupplierInvoiceMaterialID 

        [DatabaseField("tempID", IsPrimaryKey = true) ]
        public Guid tempID { get; set; }
                    
        [DatabaseField("SupplierName")]
        public string SupplierName { get; set; }

        [DatabaseField("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [DatabaseField("ContractorReference")]
        public string ContractorReference { get; set; }

        [DatabaseField("SupplierReference")]
        public string SupplierReference { get; set; }

        [DatabaseField("Qty")]
        public decimal Qty { get; set; }

        [DatabaseField("Description")]
        public string Description { get; set; }

        [DatabaseField("CostPrice")]
        public decimal CostPrice { get; set; }

        [DatabaseField("SellPrice")]
        public decimal SellPrice { get; set; }

        [DatabaseField("RRP")]
        public decimal RRP { get; set; }

        [DatabaseField("UOM")]
        public String UOM { get; set; }
      //Material.MaterialID, SupplierInvoiceMaterial.SupplierInvoiceMaterialID
        [DatabaseField("MaterialID")]
        public Guid MaterialID { get; set; }

        [DatabaseField("SupplierInvoiceMaterialID")]
        public Guid SupplierInvoiceMaterialID { get; set; }

        [DatabaseField("MaterialName")]
        public String MaterialName { get; set; }

        [DatabaseField("MatchID")]//new
        public Guid MatchID { get; set; }

        [DatabaseField("VehicleID")]//new
        public Guid VehicleID { get; set; }

        [DatabaseField("TaskMaterialID")]//new
        public Guid TaskMaterialID { get; set; }

        [DatabaseField("SupplierInvoiceID")]
        public Guid SupplierInvoiceID { get; set; }

        [DatabaseField("OldSupplierInvoiceMaterialID")]
        public Guid OldSupplierInvoiceMaterialID { get; set; }

        [DatabaseField("QtyRemainingToAssign")]
        public decimal QtyRemainingToAssign { get; set; }


    }
}
