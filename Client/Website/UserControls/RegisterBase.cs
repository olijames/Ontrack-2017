using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Electracraft.Framework.Web;
using Electracraft.Framework.DataObjects;

namespace Electracraft.Client.Website.UserControls
{
    public abstract class RegisterBase : UserControlBase
    {
        public virtual bool HasData()
        {
            return false;
        }
        public virtual void LoadForm(DOContact contact) { }

        public abstract void SaveForm(DOContact contact);
       
    }
}