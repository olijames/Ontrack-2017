using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    sealed public class DatabaseTableAttribute : Attribute
    {
        public DatabaseTableAttribute(string TableName)
        {
            this.TableName = TableName;
        }

        public string TableName { get; set; }
    }
}
