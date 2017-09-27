namespace Electracraft.Framework.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [DatabaseTable("SupplierInvoiceMaterial")]
    public partial class DOSupplierInvoiceMaterial : DOBase
    {
        [DatabaseField("SupplierInvoiceMaterialID", IsPrimaryKey = true)]
        public Guid SupplierInvoiceMaterialID { get; set; }


        [DatabaseField("MaterialID")]
        public Guid MaterialID { get; set; }

        [DatabaseField("SupplierInvoiceID")]
        public Guid SupplierInvoiceID { get; set; }

        [DatabaseField("Qty")]
        public decimal Qty { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }

        [DatabaseField("Assigned")]
        public bool Assigned { get; set; }

        [DatabaseField("OldSupplierInvoiceMaterialID")]
        public Guid OldSupplierInvoiceMaterialID { get; set; }

        [DatabaseField("TaskMaterialID")]
        public Guid TaskMaterialID { get; set; }

        [DatabaseField("MatchID")]
        public Guid MatchID { get; set; }

        [DatabaseField("VehicleID")]
        public Guid VehicleID{ get; set; }

        [DatabaseField("QtyRemainingToAssign")]
        public decimal QtyRemainingToAssign { get; set; }

        //[DatabaseField("CreatedDate")]
        //public DateTime CreadtedDate { get; set; }




    }
}
