using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOVehicleMaterials : DOBase
    {

        //@"select distinct vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername,uom
        //materialid, supplierinvoice.qty
        [DatabaseField("TempID", IsPrimaryKey = true)]
        public Guid TempID { get; set; }

        [DatabaseField("Vehicle")]
        public Guid Vehicle { get; set; }
                
        [DatabaseField("Qty")]
        public decimal Qty { get; set; }

        [DatabaseField("QtyRemainingToAssign")]
        public decimal QtyRemainingToAssign { get; set; }

        [DatabaseField("CostPrice")]
        public decimal CostPrice { get; set; }

        [DatabaseField("SellPrice")]
        public decimal SellPrice { get; set; }

        [DatabaseField("RRP")]
        public decimal RRP { get; set; }

        [DatabaseField("UOM")]
        public String UOM { get; set; }

        [DatabaseField("MaterialID")]
        public Guid MaterialID { get; set; }

        [DatabaseField("MaterialName")]
        public String MaterialName { get; set; }

        [DatabaseField("ContractorReference")]
        public String ContractorReference { get; set; }

        [DatabaseField("SupplierName")]
        public String SupplierName { get; set; }

        [DatabaseField("SupplierID")]
        public Guid SupplierID { get; set; }

        [DatabaseField("SupplierInvoiceMaterialID")]
        public Guid SupplierInvoiceMaterialID { get; set; }

        [DatabaseField("OldSupplierInvoiceMaterialID")]
        public Guid OldSupplierInvoiceMaterialID { get; set; }

        [DatabaseField("SupplierInvoiceID")]
        public Guid SupplierInvoiceID { get; set; }

        [DatabaseField("Creator")]
        public Guid Creator { get; set; }


    }
}
