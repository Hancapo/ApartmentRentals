using System;
using System.Data;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;

namespace SkyrentConnect
{
    public class OracleSkyCon
    {

        private readonly string user = "C##VICENTE";
        private readonly string pwd = "123456";
        private readonly string db = "23.251.159.66:1521/xe";

        public(bool, OracleCommand, OracleConnection) CheckOracleConnection()
        {
            string conStringUser = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";";

            bool isConnected;
            OracleConnection con = new(conStringUser);
            OracleCommand cmd = con.CreateCommand();
            try
            {
                con.Open();
                isConnected = true;
            }
            catch (Exception)
            {
                isConnected = false;
            }
            return (isConnected, cmd, con);
        }

        public DataTable OracleToDataTable(string sqlcommand)
        {
            DataTable tb = new();
            if (CheckOracleConnection().Item1)
            {
                OracleDataReader reader = RunOracleExecuteReader(sqlcommand);
                tb.Load(reader);
                CheckOracleConnection().Item3.Close();
                return tb;

            }

            return tb;
        }

        public object RunOracleExecuteScalar(string sqlcommand)
        {
            object newobj = null;
            if (CheckOracleConnection().Item1)
            {
                OracleCommand cmd = CheckOracleConnection().Item2;
                try
                {
                    cmd.CommandText = sqlcommand;

                    newobj = cmd.ExecuteScalar();
                    cmd.Dispose();
                }
                catch (InvalidOperationException)
                {

                    return newobj;

                }

            }

            return newobj;
        }

        public OracleDataReader RunOracleExecuteReader(string sqlcommand)
        {
            if (CheckOracleConnection().Item1)
            {
                OracleCommand cmd = CheckOracleConnection().Item2;
                cmd.CommandText = sqlcommand;
                OracleDataReader odr = cmd.ExecuteReader();
                return odr;
            }

            return null;
        }

        public void RunOracleNonQuery(string sqlcommand)
        {
            if (CheckOracleConnection().Item1)
            {
                OracleCommand cmd = CheckOracleConnection().Item2;
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
        }

    }
}
