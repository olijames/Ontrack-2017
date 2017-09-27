using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("JobTemplate")]
    public class DOJobTemplate : DOBase
    {
        [DatabaseField("JobTemplateID", IsPrimaryKey = true)]
        public Guid JobTemplateID { get; set; }

        [DatabaseField("JobTemplateName")]
        public string JobTemplateName { get; set; }

        [DatabaseField("ContractorID")]
        public Guid ContractorID { get; set; }


    }
}
