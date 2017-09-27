using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOVehicleMaterialQty : DOBase
    {

        //@"select distinct vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername,uom
        //materialid, supplierinvoice.qty
        [DatabaseField("SupplierInvoiceMaterialID", IsPrimaryKey = true)]
        public Guid SupplierInvoiceMaterial { get; set; }

        [DatabaseField("VehicleID")]
        public Guid VehicleID { get; set; }

        [DatabaseField("QtyRemainingToAssign")]
        public decimal QtyRemainingToAssign { get; set; }

        [DatabaseField("MaterialID")]
        public Guid MaterialID { get; set; }
    }
}
