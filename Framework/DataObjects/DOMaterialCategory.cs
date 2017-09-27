using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("MaterialCategory")]
    public class DOMaterialCategory : DOBase    
    {
        [DatabaseField("MaterialCategoryID", IsPrimaryKey = true)]
        public Guid MaterialCategoryID { get; set; }

        [DatabaseField("CategoryName")]
        public string CategoryName { get; set; }

        [DatabaseField("ContactID")]
        public Guid ContactID { get; set; }
    }
}
