﻿using System;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;

namespace LANChat_Server
{
    /// <summary>
    /// Utility for communicating with SQL database
    /// </summary>
    class Database
    {
        /// <summary>
        /// Executes a query and returns the result
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns>The result</returns>
        public static DataTable ExecuteQuery(String query)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = Properties.Settings.Default.AppDataConnectionString;
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                connection.Close();
            }
            catch(Exception ex) { }

            return dt;
        }

        /// <summary>
        /// Runs SQL that is not a query
        /// </summary>
        /// <param name="sql">The command</param>
        /// <returns>Number of affected rows</returns>
        public static int ExecuteNonQuery(String sql)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Properties.Settings.Default.AppDataConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            int rowsUpdated = command.ExecuteNonQuery();
            connection.Close();

            return rowsUpdated;
        }
    }
}
