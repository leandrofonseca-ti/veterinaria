using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace PortalVet.Data.Service
{
    public class ProviderMySql
    {

        public MySqlConnection db;

        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ProviderMySql()
        {
            db = new MySqlConnection(_connectionString);
        }


        public MySqlCommand _DBGetCommand(string sql, MySqlConnection connection, CommandType cmdType = CommandType.Text)
        {
            var cmd = new MySqlCommand(sql, connection);
            cmd.CommandType = cmdType;
            return cmd;
        }

        public MySqlConnection _DBGetConnection()
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            return db;
        }

        public MySqlParameter _DBBuildParameter(MySqlCommand cmd, string key, MySqlDbType type, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = key;
            parameter.MySqlDbType = type;
            parameter.Value = value;
            return parameter;
        }
    }
}
