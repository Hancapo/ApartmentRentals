using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace SkyrentConnect
{
    public class OracleSkyCon
    {
        static public (string, string, string) LoadCredentials()
        {
            string filepath = "config.ini";
            string _user = File.ReadAllLines(filepath)[0].Split('=')[1].Trim(); 
            string _pwd = File.ReadAllLines(filepath)[1].Split('=')[1].Trim(); 
            string _db = File.ReadAllLines(filepath)[2].Split('=')[1].Trim();


            return (_user, _pwd, _db);
        }

        static string user = LoadCredentials().Item1;
        static string pwd = LoadCredentials().Item2;
        static string db = LoadCredentials().Item3;

        static OracleConnection oracleConnection = new("User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";");
        static OracleCommand oracleCommand = oracleConnection.CreateCommand();

        public OracleConnection OracleConnection { get => oracleConnection; set => oracleConnection = value; }
        public OracleCommand OracleCommand { get => oracleCommand; set => oracleCommand = value; }




        public bool CheckDatabase()
        {

            if (OracleConnection.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                try
                {
                    OracleConnection.Open();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

            
        }
        public DataTable OracleToDataTable(string sqlcommand)
        {
            
            DataTable tb = new();
            if (CheckDatabase())
            {
                OracleDataReader reader = RunOracleExecuteReader(sqlcommand);
                tb.Load(reader);
                return tb;

            }

            return tb;
        }

        public object RunOracleExecuteScalar(string sqlcommand)
        {
            //Devuelve un solo objeto
            object newobj = null;
            if (CheckDatabase())
            {
                try
                {
                    OracleCommand.CommandText = sqlcommand;

                    newobj = OracleCommand.ExecuteScalar();
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
            if (CheckDatabase())
            {
                OracleCommand.CommandText = sqlcommand;
                OracleDataReader odr = OracleCommand.ExecuteReader();
                return odr;

            }

            return null;
        }

        public void RunOracleNonQuery(string sqlcommand)
        {
            if (CheckDatabase())
            {
                OracleCommand.CommandText = sqlcommand;
                OracleCommand.ExecuteNonQuery();


            }


        }


    }

        

    }

