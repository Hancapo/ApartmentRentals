using System;
using System.Collections.Generic;
using System.Data;
using SkyrentConnect;
using SkyrentObjects;


namespace SkyrentBusiness
{
    public class CommonBusiness
    {

        private OracleSkyCon osc = new();

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

            string LookUpForIDCommand = string.Format("SELECT MAX({0})+1 FROM {1}", IdColumnName, TableName);
            int CountID = Convert.ToInt32(osc.RunOracleExecuteScalar(string.Format("SELECT COUNT({0}) FROM {1}", IdColumnName, TableName)));


            if (CountID <= 0)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(osc.RunOracleExecuteScalar(LookUpForIDCommand));

            }

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


        public List<Departamento> GetDepartamentoList()
        {
            List<Departamento> DepLista = new();

            string sqlcommand = "SELECT t.monto_noche AS \"PrecioNoche\", c.descripcion AS \"Comuna\", d.direccion AS \"Direccion\", d.descripcion as \"Descripcion\", d.titulodepart AS \"Titulo\" FROM DEPARTAMENTO d INNER JOIN COMUNA c ON d.comuna_idcomuna = c.idcomuna INNER JOIN TARIFA t ON d.tarifa_idtarifa = t.idtarifa";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Departamento dede = new() {
                    TarifaDep = "$" + Convert.ToInt32(dr["PrecioNoche"]).ToString("N0") + " por noche",
                    ComunaDep = dr["Comuna"].ToString(),
                    DireccionDep = dr["Direccion"].ToString(),
                    DescripcionDep = dr["Descripcion"].ToString(),
                    FotoSmall = null,
                    FotoBig = null,
                    TituloDepartamento = dr["Titulo"].ToString()
                    
                };

                DepLista.Add(dede);
            }


            return DepLista;
        }

    }
}
