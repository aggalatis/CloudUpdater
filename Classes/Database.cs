using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CloudUpdater.Classes
{
    class Database
    {
        private string host = String.Empty;
        private string port = String.Empty;
        private string name = String.Empty;
        private string user = String.Empty;
        private string pass = String.Empty;

        private MySqlConnection conn;

        public Database(string dbhost, string dbport, string dbname, string dbuser, string dbpass)
        {
            this.host = dbhost;
            this.port = dbport;
            this.name = dbname;
            this.user = dbuser;
            this.pass = dbpass;
            constructConnection();
        }

        private void constructConnection()
        {
            string connstring = $"Server={this.host}; database={this.name}; Port={this.port}; UID={this.user}; password={this.pass};charset=utf8;default command timeout=3600";
            MySqlConnection MySqlConn = new MySqlConnection(connstring);
            this.conn = MySqlConn;

        }

        public int getLastIndex(Helpers Helpers)
        {
            return 0;
        }
    }
}
