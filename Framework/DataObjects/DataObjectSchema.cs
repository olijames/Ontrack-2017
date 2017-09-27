using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Electracraft.Framework.DataObjects
{
    public class DataObjectSchema
    {
        #region Properties
        
        public string TableName { get; set; }
        public string PrimaryKeyField { get; set; }
        public PropertyInfo PrimaryKeyPropertyInfo { get; set; }

        public List<string> FieldsIncludingPrimaryKey { get; internal set; }
        public List<Type> TypesIncludingPrimaryKey { get; internal set; }
        public List<PropertyInfo> PropertyInfoIncludingPrimaryKey { get; internal set; }

        public List<string> FieldsExcludingPrimaryKey { get; internal set; }
        public List<Type> TypesExcludingPrimaryKey { get; internal set; }
        public List<PropertyInfo> PropertyInfoExcludingPrimaryKey { get; internal set; }

        #endregion

        private static SortedList<string, DataObjectSchema> CachedSchemaList = new SortedList<string, DataObjectSchema>();

        private DataObjectSchema()
        {
            PrimaryKeyField = null;

            FieldsIncludingPrimaryKey = new List<string>();
            TypesIncludingPrimaryKey = new List<Type>();
            PropertyInfoIncludingPrimaryKey = new List<PropertyInfo>();

            FieldsExcludingPrimaryKey = new List<string>();
            TypesExcludingPrimaryKey = new List<Type>();
            PropertyInfoExcludingPrimaryKey = new List<PropertyInfo>();
        }

        /// <summary>
        /// Gets the schema for the specified object.
        /// </summary>
        /// <param name="DataObjectType">The object type.</param>
        /// <returns>The schema.</returns>
        public static DataObjectSchema GetDataObjectSchema(Type DataObjectType)
        {
            DataObjectSchema Schema;
            if (CachedSchemaList.ContainsKey(DataObjectType.FullName))
            {
                Schema = CachedSchemaList[DataObjectType.FullName];
            }
            else
            {
                Schema = ReflectDataObjectSchema(DataObjectType);
                CachedSchemaList[DataObjectType.FullName] = Schema;
            }

            return Schema;
        }

        /// <summary>
        /// Gets the schema for a non cached object using reflection.
        /// </summary>
        /// <param name="DataObjectType">The object type.</param>
        /// <returns>The schema.</returns>
        private static DataObjectSchema ReflectDataObjectSchema(Type DataObjectType)
        {
            DataObjectSchema Schema = new DataObjectSchema();

            //Get the table name.
            object[] DatabaseTableAttributeArray = DataObjectType.GetCustomAttributes(typeof(DatabaseTableAttribute), false);
            
            if (DatabaseTableAttributeArray.Length == 0)
                throw new Exception("Database Table Attribute not defined for " + DataObjectType.FullName);

            DatabaseTableAttribute TableAttribute = DatabaseTableAttributeArray[0] as DatabaseTableAttribute;
            Schema.TableName = TableAttribute.TableName;

            PropertyInfo[] piDataObject = DataObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo piField in piDataObject)
            {
                //Make sure this field is marked as a database field.
                object[] DatabaseFieldAttributeArray = piField.GetCustomAttributes(typeof(DatabaseFieldAttribute), false);
                if (DatabaseFieldAttributeArray.Length == 1)
                {
                    DatabaseFieldAttribute FieldAttribute = DatabaseFieldAttributeArray[0] as DatabaseFieldAttribute;
                    if (FieldAttribute.IsPrimaryKey)
                    {
                        Schema.PrimaryKeyField = FieldAttribute.FieldName;
                        Schema.PrimaryKeyPropertyInfo = piField;
                    }
                    else
                    {
                        Schema.FieldsExcludingPrimaryKey.Add(FieldAttribute.FieldName);
                        Schema.TypesExcludingPrimaryKey.Add(FieldAttribute.GetType());
                        Schema.PropertyInfoExcludingPrimaryKey.Add(piField);
                    }

                    Schema.FieldsIncludingPrimaryKey.Add(FieldAttribute.FieldName);
                    Schema.TypesIncludingPrimaryKey.Add(FieldAttribute.GetType());
                    Schema.PropertyInfoIncludingPrimaryKey.Add(piField);

                }
            }

            if (Schema.PrimaryKeyField == null)
                throw new Exception("Primary key field not defined for " + DataObjectType.FullName);

            return Schema;
        }
    }
}
