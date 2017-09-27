using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.BusinessRules
{
   public class BROperatingSites:BRBase
    {
        private DAOperatingSites _CurrentDAOperatingSites;
        private DAOperatingSites CurrentDAOperatingSites
        {
            get
            {
                if (_CurrentDAOperatingSites == null)
                    _CurrentDAOperatingSites = new DAOperatingSites(ConnectionString);
                return _CurrentDAOperatingSites;
            }
        }
        //Save Operating Sites
        public void SaveOS(DOOperatingSites os)
        {
            CurrentDAOperatingSites.SaveObject(os);
        }
    }
}
