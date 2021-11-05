using System;
using System.Collections.Generic;
using System.Data;
using SkyrentConnect;
using SkyrentObjects;


namespace SkyrentBusiness
{
    public class CommonBusiness
    {

        private readonly OracleSkyCon osc = new();

        public (bool, int, bool) LoginProc(string usuario, string contrasena)
        {
            int UserType = 0x7FFF + 0xAC;
            bool IsConnected;
            bool AccountExists;

            if (osc.RunOracleExecuteScalar(string.Format("SELECT tipo_usuario_idtipo_usuario FROM USUARIO WHERE (usuariopassword = '{0}' AND nombreusuario = '{1}')", contrasena, usuario)) == null)
            {
                IsConnected = true;
                AccountExists = false;
            }
            else
            {
                UserType = Convert.ToInt32(osc.RunOracleExecuteScalar(string.Format("SELECT tipo_usuario_idtipo_usuario FROM USUARIO WHERE (usuariopassword = '{0}' AND nombreusuario = '{1}')", contrasena, usuario)));
                IsConnected = true;
                AccountExists = true;

            }


            return (IsConnected, UserType, AccountExists);

        }

        public int CreateUser(Cliente clie)
        {

            //0 - User couldn't be created.
            //1 - User has been created sucessfully.
            //2 - User already exists.



            int IDalgo = CalculateID("idusuario", "usuario");


            string CreateUserCommand = string.Format("INSERT INTO USUARIO (idusuario, usuariopassword, nombreusuario, tipo_usuario_idtipo_usuario) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}')", IDalgo, clie.ContrasenaUsuario, clie.NombreUsuario, 2);
            string CreateClienteCommand = string.Format("INSERT INTO CLIENTE (rutcliente, usuario_idusuario, comuna_idcomuna, nombre, apellidop, apellidom) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                clie.RutCliente, IDalgo, clie.ComunaCliente, clie.NombresCliente, clie.ApellidoPaterno, clie.ApellidoMaterno);
            string SearchUsersCommand = string.Format("SELECT COUNT(rutcliente) FROM CLIENTE WHERE rutcliente = '{0}'", clie.RutCliente);



            int UsersWithSameRUT = Convert.ToInt32(osc.RunOracleExecuteScalar(SearchUsersCommand));


            if (UsersWithSameRUT <= 0)
            {
                try
                {
                    osc.RunOracleNonQuery(CreateUserCommand);
                    osc.RunOracleNonQuery(CreateClienteCommand);
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }





        }

        public int CalculateID(string IdColumnName, string TableName) 
        {

            string LookUpForIDCommand = $"SELECT MAX({IdColumnName})+1 FROM {TableName}";
            int CountID = Convert.ToInt32(osc.RunOracleExecuteScalar(string.Format("SELECT COUNT({0}) FROM {1}", IdColumnName, TableName)));


            return CountID <= 0 ? 1 : Convert.ToInt32(osc.RunOracleExecuteScalar(LookUpForIDCommand));

        }

        public List<Region> GetRegionList()
        {
            List<Region> RegionLista = new();
            string sqlcommand = string.Format("SELECT * FROM REGION");
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Region r = new();
                r.Nombre = dr["DESCRIPCION"].ToString();
                r.NumeroRegion = dr["IDREGION"].ToString();

                RegionLista.Add(r);

            }
            return RegionLista;
        }

        public List<string> GetCiudadList(string CodeRegion)
        {
            List<string> CiudadLista = new();
            string sqlcommand = string.Format("SELECT DESCRIPCION FROM CIUDAD WHERE REGION_IDREGION = '{0}'", CodeRegion);
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                CiudadLista.Add(dr["Descripcion"].ToString());
            }
            return CiudadLista;
        }               

        public List<string> GetComunaList(string Ciudad)
        {
            List<string> RegionLista = new();
            string sqlcommand = string.Format("SELECT comuna.descripcion FROM CIUDAD INNER JOIN COMUNA ON ciudad.idciudad = comuna.ciudad_idciudad WHERE ciudad.descripcion = '{0}'", Ciudad);
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {

                RegionLista.Add(dr["Descripcion"].ToString());


            }
            return RegionLista;
        }

        public int GetIdComunaByName(string NombreComuna)
        {

            string sqlcommand = string.Format("SELECT idcomuna FROM COMUNA WHERE COMUNA.descripcion = '{0}'", NombreComuna);
            return Convert.ToInt32(osc.RunOracleExecuteScalar(sqlcommand));

        }



    }
}
