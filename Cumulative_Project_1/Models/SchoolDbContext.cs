using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Cumulative_Project_1.Models
{
    public class SchoolDbContext
    {
        // matching local database credentials
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "schooldb"; } }
        private static string Server {  get { return "localhost"; } }
        private static string Port { get { return "3306"; } }


        // connect to the database
        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password;
            }
        }
        /// <summary>
        /// Returns a connection to the school database.
        /// </summary>
        /// <example>
        /// private SchoolDbContext SchoolDb = new SchoolDbContext();
        /// MySqlConnection Conn = SchoolDb.AccessDatabase();
        /// </example>
        /// <returns>A MySqlConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            // instantianting the MySqlConnection Class to create an object
            // object is a specific conncection to schooldb database on port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }

    }

}