using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("LabourCategory")]
    public class DOLabourCategory : DOBase
    {
        [DatabaseField("LabourCategoryID", IsPrimaryKey = true)]
        public Guid LabourCategoryID { get; set; }

        [DatabaseField("CategoryName")]
        public string CategoryName { get; set; }

    }
}
