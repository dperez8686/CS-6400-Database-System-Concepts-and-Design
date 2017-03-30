using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ASACS5.Services
{
    public static class SqlHelper
    {
        const string connStr = "server=localhost;user=root;database=cs6400_sp17_team090;port=3306;password=admin;";

        /// <summary>
        /// Use this method when you expect a single row back from the db.
        /// </summary>
        /// <param name="sql">The SQL SELECT statement to execute</param>
        /// <param name="columns">The number of columns in the SELECT statement</param>
        /// <returns>An object[] with the query result, or null.</returns>
        public static object[] ExecuteSingleSelect(string sql, int columns)
        {
            // set up the response object
            object[] response = null;

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    // we should not get more than one result with this single select. if it happens, log a debug message
                    // and we will return the last row in the results
                    if (response != null)
                    {
                        Debug.WriteLine("SqlHelper.ExecuteSingleSelect got more than one rows. With SQL: " + sql);
                    }

                    // set up a new response obejct[] and populate the column values
                    response = new object[columns];
                    for (int i = 0; i < columns; i++)
                    {
                        response[i] = rdr[i];
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SqlHelper.RunSingleSelect: " + ex.ToString());
                throw ex;
            }

            conn.Close();

            return response;
        }

        /// <summary>
        /// Use this method when you expect one or more rows back from the db.
        /// </summary>
        /// <param name="sql">The SQL SELECT statement to execute</param>
        /// <param name="columns">The number of columns in the SELECT statement</param>
        /// <returns>A List of object[]s with the query results. Returns an empty list if no results.</returns>
        public static List<object[]> ExecuteMultiSelect(string sql, int columns)
        {
            // set up the response object
            List<object[]> response = new List<object[]>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    // set up a new response obejct[] and populate the column values
                    object[] row = new object[columns];
                    for (int i = 0; i < columns; i++)
                    {
                        row[i] = rdr[i];
                    }

                    // add it to the response List
                    response.Add(row);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SqlHelper.RunMultiSelect: " + ex.ToString());
                throw ex;
            }

            conn.Close();

            return response;
        }

        /// <summary>
        /// Run a non-query SQL statement, like INSERT, UPDATE, or DELETE
        /// </summary>
        /// <param name="sql">The SQL statement to execute (INSERT, UPDATE, or DELETE)</param>
        public static void ExecuteNonQuery(string sql)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SqlHelper.ExecuteNonQuery: " + ex.ToString());
                throw ex;
            }

            conn.Close();
        }

        /// <summary>
        /// Run a query that returns a single value
        /// </summary>
        /// <param name="sql">The SQL statement to execute</param>
        public static object ExecuteScalar(string sql)
        {
            object result = null;

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SqlHelper.ExecuteScalar: " + ex.ToString());
                throw ex;
            }

            conn.Close();

            return result;
        }

    }
}