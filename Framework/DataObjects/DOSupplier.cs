namespace Electracraft.Framework.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class DOSupplier:DOBase
    {
        public Guid SupplierID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

    }
}
