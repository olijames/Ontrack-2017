using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    sealed public class DatabaseFieldAttribute : Attribute
    {
        public DatabaseFieldAttribute(string FieldName)
        {
            this.FieldName = FieldName;
        }

        public string FieldName { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
