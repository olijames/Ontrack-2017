using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Electracraft.Framework.BusinessRules
{
    public abstract class BRBase
    {
        internal string ConnectionString
        {
            get
            {
#if DEBUG
                return ConfigurationManager.ConnectionStrings["ConnectionStringDebug"].ConnectionString;
#else
                return ConfigurationManager.ConnectionStrings["ConnectionStringRelease"].ConnectionString;
#endif
            }
        }

    }
}
