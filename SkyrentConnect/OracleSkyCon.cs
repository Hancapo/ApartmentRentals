using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SkyrentConnect
{
    public class OracleSkyCon
    {

        private readonly string user = "C##SKYRENT";
        private readonly string pwd = "12456";
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

        public bool RunOracleExecuteScalar(string sqlcommand)
        {
            bool canRun = false;
            if (CheckOracleConnection().Item1)
            {
                OracleCommand cmd = CheckOracleConnection().Item2;
                cmd.CommandText = sqlcommand;
                try
                {
                    cmd.ExecuteScalar();
                    canRun = true;

                    cmd.Dispose();
                }
                catch (Exception)
                {
                    canRun = false;

                }

            }

            return canRun;
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
