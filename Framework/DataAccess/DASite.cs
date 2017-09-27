using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility.Exceptions;

namespace Electracraft.Framework.DataAccess
{
    public class DASite : DABase
    {
        public DASite(string ConnectionString)
            : base(ConnectionString)
        {
        }

        public override void Validate(DOBase obj)
        {
            DOSite Site = obj as DOSite;

            if (Site != null)
            {
                ValidateSite(Site);
            }
        }

        private static void ValidateSite(DOSite Site)
        {
            if (string.IsNullOrEmpty(Site.Address1))
                throw new FieldValidationException("Site Address 1 is required.");
        //if (string.IsNullOrEmpty(Site.Address2))
            //    throw new FieldValidationException("Site Address 2 is required.");
       // if (string.IsNullOrEmpty(Site.Address2))
             //   throw new FieldValidationException("Site Address 2 is required.");
      }
    }
}
