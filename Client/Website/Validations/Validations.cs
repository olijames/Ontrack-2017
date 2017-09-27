using System;
using System.Collections.Generic;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;

namespace Electracraft.Client.Website.Validations
{
    public class Validations : PageBase
    {

        public bool ValidateMaterialsLabour(Guid taskId)
        {
            
            List<DOBase> TM = CurrentBRJob.SelectTaskMaterialsList(taskId);
            List<DOBase> TL = CurrentBRJob.SelectTaskLabours(taskId);
            bool HasActual = false;
            foreach (DOTaskMaterialInfo myTm in TM)
            {
                if (myTm.MaterialType == TaskMaterialType.Actual && myTm.Active)
                {
                    HasActual = true;
                    break;
                }
            }
            if (!HasActual)
            {
                foreach (DOTaskLabourInfo myTL in TL)
                {
                    if (myTL.LabourType == TaskMaterialType.Actual && myTL.Active)
                    {
                        HasActual = true;
                        break;
                    }
                }
            }
            return HasActual;
        }
    }
    }
