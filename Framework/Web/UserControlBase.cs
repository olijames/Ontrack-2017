using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Electracraft.Framework.Web
{
    public class UserControlBase : UserControl
    {
        public PageBase ParentPage
        {
            get
            {
                return Page as PageBase;
            }
        }
    }
}
