using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [DatabaseTable("")]
    public class DOSiteWithContact : DOSite
    {
        [DatabaseField("FirstName")]
        public string FirstName { get; set; }
        [DatabaseField("LastName")]
        public string LastName { get; set; }
        [DatabaseField("CompanyName")]
        public string CompanyName { get; set; }
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(CompanyName))
                    return CompanyName;
                else
                    return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
