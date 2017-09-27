using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Material")]
    public class DOMaterial : DOBase
    {
        [DatabaseField("MaterialID", IsPrimaryKey = true)]
        public Guid MaterialID { get; set; }

        [DatabaseField("MaterialCategoryID")]
        public Guid MaterialCategoryID { get; set; }

        [DatabaseField("MaterialName")]
        public string MaterialName { get; set; }

        [DatabaseField("Description")]
        public string Description { get; set; }

        [DatabaseField("CostPrice")]
        public decimal CostPrice { get; set; }

        [DatabaseField("SellPrice")]
        public decimal SellPrice { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }

        [DatabaseField("SupplierID")]
        public Guid SupplierID { get; set; }

        [DatabaseField("SupplierProductCode")]
        public string SupplierProductCode { get; set; }

        [DatabaseField("UOM")]
        public string UOM { get; set; }

        [DatabaseField("RRP")]
        public decimal RRP { get; set; }


    }
}
