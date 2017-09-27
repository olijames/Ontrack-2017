using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Electracraft.Framework.DataObjects;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Electracraft.Framework.DataAccess
{
    public abstract class DABase
    {
        public string ConnectionString { get; set; }

        public DABase(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public virtual void Validate(DOBase obj)
        {
        }

        #region Command execution
        /// <summary>
        /// Retrieves a dataset.
        /// </summary>
        /// <param name="cmd">The data command to use.</param>
        /// <returns>The dataset.</returns>
        protected DataSet GetDataSet(SqlCommand cmd)
        {
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            cmd.Connection = sqlConn;

            DataSet dsResult = null;
            try
            {
                dsResult = new DataSet();
                sqlConn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dsResult);
            }
            finally
            {
                sqlConn.Close();
            }

            return dsResult;
        }

        /// <summary>
        /// Retrieves a data table.
        /// </summary>
        /// <param name="cmd">The data command to use.</param>
        /// <returns>The data table.</returns>
        private DataTable GetDataTable(SqlCommand cmd)
        {
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            cmd.Connection = sqlConn;

            DataTable dtResult = null;
            try
            {
                dtResult = new DataTable();
                sqlConn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dtResult);
            }
            catch(SqlException s)
            {
                s.StackTrace.ToString();
            }
            finally
            {
                sqlConn.Close();
            }

            return dtResult;
        }

        /// <summary>
        /// Retrieves a scalar object.
        /// </summary>
        /// <param name="cmd">The data command to use.</param>
        /// <returns>The object.</returns>
        private object GetScalar(SqlCommand cmd)
        {
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            cmd.Connection = sqlConn;

            object objResult = null;
            try
            {
                sqlConn.Open();
                objResult = cmd.ExecuteScalar();
            }finally
            {
                sqlConn.Close();
            }

            return objResult;
        }

        /// <summary>
        /// Retrives a single data row.
        /// </summary>
        /// <param name="cmd">The data command to use.</param>
        /// <returns>The data row.</returns>
        public DataRow GetDataRow(SqlCommand cmd)
        {
            DataTable dtResult = GetDataTable(cmd);
            if (dtResult.Rows.Count == 0) return null;
            return dtResult.Rows[0];
        }

        /// <summary>
        /// Executes a data command that doesn't return a value.
        /// </summary>
        /// <param name="cmd">The data command.</param>
        private void Execute(SqlCommand cmd)
        {
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            cmd.Connection = sqlConn;

            try { 
                sqlConn.Open();
                cmd.ExecuteNonQuery();
            }

            finally
            { 
                sqlConn.Close();
            }
        }

        /// <summary>
        /// Executes a database query.
        /// </summary>
        /// <param name="Query">The query text.</param>
        /// <param name="Parameters">The query parameters (unencoded).</param>
        protected void ExecuteQuery(string Query, params object[] Parameters)
        {
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            SqlCommand cmd = new SqlCommand(string.Format(Query, FormattedParams));
            Execute(cmd);
        }
        //Execute query directly
        //public List<DOBase> ExecuteQueryWithoutParameters(string Query)
        //{
        //    //object[] FormattedParams = new object[Parameters.Length];
        //    //for (int i = 0; i < Parameters.Length; i++)
        //    //    FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
        //    //SqlCommand cmd = new SqlCommand(string.Format(Query, FormattedParams));
        //    SqlCommand cmd = new SqlCommand(string.Format(Query));
        //   // SqlCommand cmd = new SqlCommand(Query.ToString());

        //    DataRow dr = GetDataRow(cmd);
        //    if (dr == null) return null;

        //    DOBase obj = CreateObjectFromDataRow(ObjectType, Schema, dr);
        //    return  ;
        //}
        /// <summary>
        /// Executes a scalar query.
        /// </summary>
        /// <param name="Query">The query text.</param>
        /// <param name="Parameters">The query parameters (unencoded).</param>
        /// <returns>The scalar object.</returns>
        public object ExecuteScalar(string Query, params object[] Parameters)
        {
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            SqlCommand cmd = new SqlCommand(string.Format(Query, FormattedParams));

            return GetScalar(cmd);
        }

        #endregion

        #region Data object population / persistence
        /// <summary>
        /// Selects a single object.
        /// </summary>
        /// <param name="Table">The table to select from.</param>
        /// <param name="WhereClause">The SQL where clause.</param>
        /// <returns></returns>
        /// 
        /// Tony reviewing here
        public DOBase SelectObject(Type ObjectType, string WhereClause, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);

            StringBuilder Query = new StringBuilder("SELECT * FROM " + Schema.TableName + " WHERE ");
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);

            Query.Append(string.Format(WhereClause, FormattedParams));
            SqlCommand cmd = new SqlCommand(Query.ToString());

            DataRow dr = GetDataRow(cmd);
            if (dr == null) return null;

            DOBase obj = CreateObjectFromDataRow(ObjectType, Schema, dr);

            return obj;
        }
      
        /// <summary>
        /// Selects a single object.
        /// </summary>
        /// <param name="Table">The table to select from.</param>
        /// <param name="WhereClause">The SQL where clause.</param>
        /// <returns></returns>
        public DOBase SelectQuery(Type ObjectType, StringBuilder Query,string WhereClause, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);

            //StringBuilder Query = new StringBuilder("SELECT * FROM " + Schema.TableName + " WHERE ");
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);

            Query.Append(string.Format(" WHERE "+WhereClause, FormattedParams));
            SqlCommand cmd = new SqlCommand(Query.ToString());

            DataRow dr = GetDataRow(cmd);
            if (dr == null) return null;

            DOBase obj = CreateObjectFromDataRow(ObjectType, Schema, dr);

            return obj;
        }

        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="Table">The table to select from.</param>
        /// <param name="WhereClause">The SQL where clause.</param>
        /// <returns></returns>
        public List<DOBase> SelectQueryListofObjects(Type ObjectType, StringBuilder Query, string WhereClause, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);

            //StringBuilder Query = new StringBuilder("SELECT * FROM " + Schema.TableName + " WHERE ");
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);

            Query.Append(string.Format(" WHERE " + WhereClause, FormattedParams));
            SqlCommand cmd = new SqlCommand(Query.ToString());

           // DataRow dr = GetDataRow(cmd);
            DataTable dt = GetDataTable(cmd);

            List<DOBase> objList = new List<DOBase>();

            foreach (DataRow dr in dt.Rows)
            {
                DOBase obj = CreateObjectFromDataRow(ObjectType, Schema, dr);
                objList.Add(obj);
            }
            return objList;
           
        }
        /// <summary>
        /// Creates an instance of an object from the underlying data row.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="Schema">The object schema.</param>
        /// <param name="dr">The data row to populate the object with.</param>
        /// <returns>The object instance.</returns>
        private static DOBase CreateObjectFromDataRow(Type ObjectType, DataObjectSchema Schema, DataRow dr)
        {
            DOBase obj = Activator.CreateInstance(ObjectType) as DOBase;
            obj.PersistenceStatus = ObjectPersistenceStatus.Existing;

            for (int FieldIndex = 0; FieldIndex < Schema.FieldsIncludingPrimaryKey.Count; FieldIndex++)
            {
                string FieldName = Schema.FieldsIncludingPrimaryKey[FieldIndex];
                PropertyInfo FieldInfo = Schema.PropertyInfoIncludingPrimaryKey[FieldIndex];


                object FieldValue = dr[FieldName];
                if (FieldInfo.PropertyType.BaseType == typeof(Enum))
                {
                    FieldValue = Enum.ToObject(FieldInfo.PropertyType, FieldValue);
                }
                else if (FieldValue is DBNull)
                {
                    FieldValue = null;
                }

                FieldInfo.SetValue(obj, FieldValue, null);
            }
            return obj;
        }

        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <returns>The object list.</returns>
        public List<DOBase> SelectObjects(Type ObjectType)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            StringBuilder Query = new StringBuilder("SELECT * FROM " + Schema.TableName);
            SqlCommand cmd = new SqlCommand(Query.ToString());

            return SelectObjects(ObjectType, Schema, cmd);
        }

        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="WhereClause">The where clause to filter on.</param>
        /// <param name="Parameters">The query parameters (unencoded).</param>
        /// <returns>The object list.</returns>
        public List<DOBase> SelectObjects(Type ObjectType, string WhereClause, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            StringBuilder Query = new StringBuilder("SELECT * FROM " + Schema.TableName + " WHERE ");
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            Query.Append(string.Format(WhereClause, FormattedParams));
            SqlCommand cmd = new SqlCommand(Query.ToString());

            return SelectObjects(ObjectType, Schema, cmd);
        }
        /*
        Execute query
            */
        public void ExecuteDirectQuery(string query,bool flag, Guid siteID)
        {
            ExecuteQuery(query,flag,siteID);
        }


        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="Query">The full query text to use.</param>
        /// <param name="Parameters">The query parameters (unencoded).</param>
        /// <returns>The object list.</returns>
        public List<DOBase> SelectObjectsCustomQuery(Type ObjectType, string Query, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            SqlCommand cmd = new SqlCommand(string.Format(Query, FormattedParams));

            return SelectObjects(ObjectType, Schema, cmd);
        }
        /// <summary>
        /// Selects an object.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="Query">The full query text to use.</param>
        /// <param name="Parameters">The query parameters (unencoded).</param>
        /// <returns>The object list.</returns>
        public List<DOBase> SelectObjectCustomQuery(Type ObjectType, string Query, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            SqlCommand cmd = new SqlCommand(string.Format(Query, FormattedParams));

            return SelectObjects(ObjectType, Schema, cmd);
        }



        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="OrderBy">The field to order by.</param>
        /// <returns>The list of objects.</returns>
        public List<DOBase> SelectObjectsOrderBy(Type ObjectType,string OrderBy)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            StringBuilder Query = new StringBuilder();
            SqlCommand cmd = new SqlCommand("SELECT * FROM " + Schema.TableName + " ORDER BY " + OrderBy);
            return SelectObjects(ObjectType, Schema, cmd);
        }
        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="OrderBy">The field to order by.</param>
        /// <param name="
        /// <returns>The list of objects.</returns>
        public List<DOBase> SelectObjectsOrderByWhereClause(Type ObjectType, string WhereClause,  string OrderBy, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            StringBuilder Query = new StringBuilder("SELECT * FROM " + Schema.TableName + " WHERE ");
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            Query.Append(string.Format(WhereClause, FormattedParams));
            Query.Append(" ORDER BY ");
            Query.Append(OrderBy);
            //Query.Append(string.Format(WhereClause, FormattedParams));
            SqlCommand cmd = new SqlCommand(Query.ToString());
          //  SqlCommand cmd = new SqlCommand("SELECT * FROM " + Schema.TableName + " ORDER BY " + OrderBy);
            return SelectObjects(ObjectType, Schema, cmd);
        }

        /// <summary>
        /// Selects a list of objects.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="Schema">The schema of the object type.</param>
        /// <param name="cmd">The sql command to execute.</param>
        /// <returns>The object list.</returns>
        private List<DOBase> SelectObjects(Type ObjectType, DataObjectSchema Schema, SqlCommand cmd)
        {
            DataTable dt = GetDataTable(cmd);

            List<DOBase> objList = new List<DOBase>();

            foreach (DataRow dr in dt.Rows)
            {
                DOBase obj = CreateObjectFromDataRow(ObjectType, Schema, dr);
                objList.Add(obj);
            }
            return objList;
        }


        /// <summary>
        /// Selects a list of objects from a stored procedure.
        /// </summary>
        /// <param name="ObjectType">The object type (must inherit from DOBase).</param>
        /// <param name="StoredProcedureName">The stored procedure name.</param>
        /// <param name="Params">The parameters.</param>
        /// <returns>The list of objects.</returns>
        public List<DOBase> SelectObjectsFromStoredProcedure(Type ObjectType, string StoredProcedureName, SortedList<string, object> Params)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(ObjectType);
            SqlCommand cmd = new SqlCommand(StoredProcedureName);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (string Param in Params.Keys)
            {
                cmd.Parameters.AddWithValue(Param, Params[Param]);
            }

            return SelectObjects(ObjectType, Schema, cmd);
        }

        private const string UpdateFieldSeparator = ", ";
        private const string UpdateClauseFormat = "[{0}] = {1}";


        /// <summary>
        /// Saves an object to the database.
        /// </summary>
        /// <param name="obj">The object to save.</param>
        public void SaveObject(DOBase obj)
         {
            Validate(obj);

            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(obj.GetType());
            if (obj.PersistenceStatus == ObjectPersistenceStatus.New)
            {
                InsertObject(obj, Schema);
            }
            else
            {
                UpdateObject(obj, Schema);   
            }
        }


        /// <summary>
        /// Updates the database entry of an existing object.
        /// </summary>
        /// <param name="obj">The object to update.</param>
        /// <param name="Schema">The data object schema.</param>
        private void UpdateObject(DOBase obj, DataObjectSchema Schema)
        {
            int FieldCount = Schema.FieldsExcludingPrimaryKey.Count;
            string[] UpdateClause = new string[FieldCount];
            for (int i = 0; i < FieldCount; i++)
                UpdateClause[i] = string.Format(UpdateClauseFormat, Schema.FieldsExcludingPrimaryKey[i], EncodeValue(obj, Schema.PropertyInfoExcludingPrimaryKey[i]));

            StringBuilder Query = new StringBuilder("UPDATE " +Schema.TableName + " SET ");
            Query.Append(string.Join(UpdateFieldSeparator, UpdateClause));
            Query.Append(" WHERE " + Schema.PrimaryKeyField + " = " + EncodeValue(obj, Schema.PrimaryKeyPropertyInfo));

            SqlCommand cmd = new SqlCommand(Query.ToString());
            Execute(cmd);
        }

        private const string InsertFieldNameSeparator = "],[";
        private const string InsertFieldValueSeparator = ",";

        /// <summary>
        /// Inserts a new object into the database.
        /// </summary>
        /// <param name="obj">The object to insert.</param>
        /// <param name="Schema">The data object schema</param>
        private void InsertObject(DOBase obj, DataObjectSchema Schema)
        {
            int FieldCount = Schema.FieldsIncludingPrimaryKey.Count;
            string[] FieldValueStrings = new string[FieldCount];

            for (int i = 0; i < FieldCount; i++)
                FieldValueStrings[i] = EncodeValue(obj, Schema.PropertyInfoIncludingPrimaryKey[i]);

            StringBuilder Query = new StringBuilder("INSERT " + Schema.TableName + " ([");
            Query.Append(string.Join(InsertFieldNameSeparator, Schema.FieldsIncludingPrimaryKey));
            Query.Append("]) VALUES (");
            Query.Append(string.Join(InsertFieldValueSeparator, FieldValueStrings));
            Query.Append(")");
            SqlCommand cmd = new SqlCommand(Query.ToString());
            Execute(cmd);
            obj.PersistenceStatus = ObjectPersistenceStatus.Existing;
        }

        /// <summary>
        /// Deletes an object from the database.
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        public void DeleteObject(DOBase obj)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(obj.GetType());
            SqlCommand cmd = new SqlCommand("DELETE FROM " + Schema.TableName + " WHERE " + Schema.PrimaryKeyField + " = " + EncodeValue(obj, Schema.PrimaryKeyPropertyInfo));
            Execute(cmd);
        }
        /// <summary>
        /// Deletes an object from the database.
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        public void DeleteObjectWhereParams(Type obj,string WhereClause, params object[] Parameters)
        {
            DataObjectSchema Schema = DataObjectSchema.GetDataObjectSchema(obj.GetType());
            StringBuilder Query = new StringBuilder("DELETE * FROM " + Schema.TableName + " WHERE ");
            object[] FormattedParams = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                FormattedParams[i] = EncodeValue(Parameters[i].GetType(), Parameters[i]);
            Query.Append(string.Format(WhereClause, FormattedParams));
            SqlCommand cmd = new SqlCommand(Query.ToString());
            Execute(cmd);
        }
        /// <summary>
        /// Encodes a value for use in a sql query.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="pi">The property info of the property value to encode.</param>
        /// <returns>The encoded value.</returns>
        private string EncodeValue(DOBase obj, PropertyInfo pi)
        {
            object value = pi.GetValue(obj, null);
            Type objType = pi.PropertyType;

            return EncodeValue(objType, value);
        }

        /// <summary>
        /// Encodes a value for use in a sql query.
        /// </summary>
        /// <param name="objType">The object type.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The encoded value.</returns>
        private string EncodeValue(Type objType, object value)
        {
            string QuoteStr = "'{0}'";
            string NoQuoteStr = "{0}";

            if (objType.Equals(typeof(string)))
            {
                if (value == null) value = string.Empty;
                //string MyString = value.ToString();
                //if (MyString == "+" || MyString == "-" || MyString == ">0" || MyString=="=0") //todo this is not ideal 11/9
                //{
                //    return ((string)value);
                
                    return string.Format(QuoteStr, ((string)value).Replace("'", "''"));
                
            }
            else if (objType.Equals(typeof(Guid)))
            {
                return string.Format(QuoteStr, value);
            }
            else if (objType.Equals(typeof(decimal)) || objType.Equals(typeof(int)) || objType.Equals(typeof(float)) ||
                objType.Equals(typeof(long)) || objType.Equals(typeof(short)))
            {
                return string.Format(NoQuoteStr, value);
            }
            else if (objType.Equals(typeof(DateTime)))
            {
                if(((DateTime)value) == DateTime.MinValue)
                    value = Utility.DateAndTime.NoValueDate;
                return string.Format(QuoteStr, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (objType.Equals(typeof(bool)))
            {
                return string.Format(NoQuoteStr, ((bool)value) ? 1 : 0);
            }
            else if (objType.BaseType.Equals(typeof(Enum)))
            {
                return ((int)value).ToString();
            }
         
            else
                throw new Exception("Invalid object type");

        }
        #endregion
    }
}
