using System;
using System.Data.SQLite;
using System.Data;

namespace LANChat_Server
{
    /// <summary>
    /// Utility for communicating with SQLite database
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
                SQLiteConnection connection = new SQLiteConnection("Data Source=data.db");
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = query;
                SQLiteDataReader reader = command.ExecuteReader();
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
            SQLiteConnection connection = new SQLiteConnection("Data Source=data.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = sql;
            int rowsUpdated = command.ExecuteNonQuery();
            connection.Close();

            return rowsUpdated;
        }
    }
}
