using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("Labour")]
    public class DOLabour : DOBase
    {
        [DatabaseField("LabourID", IsPrimaryKey = true)]
        public Guid LabourID { get; set; }

        [DatabaseField("LabourCategoryID")]
        public Guid LabourCategoryID { get; set; }

        [DatabaseField("LabourName")]
        public string LabourName { get; set; }

        [DatabaseField("Description")]
        public string Description { get; set; }

        [DatabaseField("CostPrice")]
        public decimal CostPrice { get; set; }

        [DatabaseField("SellPrice")]
        public decimal SellPrice { get; set; }

    }
}
