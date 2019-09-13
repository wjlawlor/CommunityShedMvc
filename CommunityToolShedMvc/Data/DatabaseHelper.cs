using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace CommunityToolShedMvc.Data
{
    public static class DatabaseHelper
    {
        private const string ConnectionStringName = "CommunityShed";

        public static List<TModelType> Retrieve<TModelType>(string sql, params SqlParameter[] parameters)
        {
            Type modelType = typeof(TModelType);

            // Get the properties for the model type.
            PropertyInfo[] properties = modelType.GetProperties();

            // Create an instance of the generic list collection.
            Type listType = typeof(List<>);
            Type[] typeArgs = { modelType };
            Type constructed = listType.MakeGenericType(typeArgs);
            var list = (List<TModelType>)Activator.CreateInstance(constructed);

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    connection.Open();

                    var dr = command.ExecuteReader();

                    // Get a set of the data reader columns.
                    // We'll use this in a bit to determine which model properties
                    // have associated columns in the data reader.
                    HashSet<string> dataReaderColumnNames = GetDataReaderColumns(dr);

                    while (dr.Read())
                    {
                        TModelType model = Activator.CreateInstance<TModelType>();

                        foreach (var property in properties)
                        {
                            string propertyName = property.Name;

                            if (property.CanWrite && dataReaderColumnNames.Contains(propertyName))
                            {
                                object value = dr[propertyName];
                                if (value is DBNull)
                                {
                                    property.SetValue(model, "");
                                }
                                else
                                {
                                    property.SetValue(model, value);
                                }

                            }
                        }

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public static TModelType RetrieveSingle<TModelType>(string sql, params SqlParameter[] parameters)
        {
            Type modelType = typeof(TModelType);

            // Get the properties for the model type.
            PropertyInfo[] properties = modelType.GetProperties();

            TModelType model = default(TModelType);

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    connection.Open();

                    var dr = command.ExecuteReader();

                    // Get a set of the data reader columns.
                    // We'll use this in a bit to determine which model properties
                    // have associated columns in the data reader.
                    HashSet<string> dataReaderColumnNames = GetDataReaderColumns(dr);

                    while (dr.Read())
                    {
                        model = Activator.CreateInstance<TModelType>();

                        foreach (var property in properties)
                        {
                            string propertyName = property.Name;

                            if (property.CanWrite && dataReaderColumnNames.Contains(propertyName))
                            {
                                object value = dr[propertyName];
                                if (value is DBNull)
                                {
                                    property.SetValue(model, "");
                                }
                                else
                                {
                                    property.SetValue(model, value);
                                }
                            }
                        }
                    }
                }
            }

            return model;
        }

        private static HashSet<string> GetDataReaderColumns(SqlDataReader dr)
        {
            var dataReaderColumnNames = new HashSet<string>();
            for (int columnIndex = 0; columnIndex < dr.FieldCount; columnIndex++)
            {
                dataReaderColumnNames.Add(dr.GetName(columnIndex));
            }

            return dataReaderColumnNames;
        }

        public static DataTable Retrieve(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    using (var dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public static void Execute(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static TValueType ExecuteScalar<TValueType>(string sql, params SqlParameter[] parameters)
        {
            TValueType value = default(TValueType);

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    command.Connection.Open();
                    value = (TValueType)command.ExecuteScalar();
                }
            }

            return value;
        }

        public static int? Insert(string sql, params SqlParameter[] parameters)
        {
            // Append a query to the passed in insert query
            // to get the last inserted ID primary key value.
            sql += @"
                select cast(scope_identity() as int) as 'id';
            ";

            int? id = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    command.Connection.Open();
                    id = (int?)command.ExecuteScalar();
                }
            }

            return id;
        }

        public static void Update(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SqlParameter GetNullableStringSqlParameter(string parameterName, string value)
        {
            return new SqlParameter(parameterName, string.IsNullOrEmpty(value) ? (object)DBNull.Value : value);
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }
    }

}