using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using SkyrentConnect;
using SkyrentObjects;


namespace SkyrentBusiness
{
    public class CommonBusiness
    {

        public OracleSkyCon osc = new();
        

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

            //string LookUpForIDCommand = $"SELECT MAX({IdColumnName})+1 FROM {TableName}";
            //int CountID = Convert.ToInt32(osc.RunOracleExecuteScalar($"SELECT COUNT({IdColumnName}) FROM {TableName}"));
            //return CountID <= 0 ? 1 : Convert.ToInt32(osc.RunOracleExecuteScalar(LookUpForIDCommand));

            List<int> ListaID = new();
            string sqlcommand = $"SELECT {IdColumnName} FROM {TableName}";

            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                ListaID.Add(Convert.ToInt32(dataRow[IdColumnName]));
            }

            ListaID.Sort();

            int IdResult = 0;

            foreach (var item in ListaID)
            {
                IdResult++;

                if (item != IdResult)
                {
                    return IdResult;
                }
            }

            return 0;

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

        public List<Ciudad> GetCiudadListFromRegion(string CodeRegion)
        {
            List<Ciudad> CiudadLista = new();
            string sqlcommand = $"SELECT DESCRIPCION, IDCIUDAD FROM CIUDAD WHERE REGION_IDREGION = '{CodeRegion}'";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Ciudad ci = new();

                ci.NameCiudad = dr["Descripcion"].ToString();
                ci.IdCiudad = dr["IdCiudad"].ToString();
                CiudadLista.Add(ci);
            }
            return CiudadLista;
        }               

        public List<Comuna> GetComunaListFromCiudad(string Ciudad)
        {
            List<Comuna> RegionLista = new();
            string sqlcommand = $"SELECT comuna.descripcion, comuna.idcomuna FROM CIUDAD INNER JOIN COMUNA ON ciudad.idciudad = comuna.ciudad_idciudad WHERE ciudad.descripcion = '{Ciudad}'";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {

                Comuna com = new();
                com.Descripcion = dr["Descripcion"].ToString();
                com.IDComuna = dr["IDCOMUNA"].ToString();
                RegionLista.Add(com); ;

            }
            return RegionLista;
        }

        public int GetIdComunaByName(string NombreComuna)
        {

            string sqlcommand = $"SELECT idcomuna FROM COMUNA WHERE COMUNA.descripcion = '{NombreComuna}'";
            return Convert.ToInt32(osc.RunOracleExecuteScalar(sqlcommand));

        }


        public string GetCiudadByComuna(string NombreComuna)
        {
            string sqlcommand = $"SELECT ciudad.descripcion FROM COMUNA INNER JOIN CIUDAD ON ciudad.idciudad = comuna.ciudad_idciudad WHERE comuna.descripcion = '{NombreComuna}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();
            
        }


        public string GetRegionIdByCiudad(string NombreCiudad)
        {
            string sqlcommand = $"SELECT region.idregion FROM CIUDAD INNER JOIN REGION ON region.idregion = ciudad.region_idregion WHERE ciudad.descripcion = '{NombreCiudad}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();
        }

        public string GetRegionByCiudad(string NombreCiudad)
        {
            string sqlcommand = $"SELECT region.descripcion FROM CIUDAD INNER JOIN REGION ON region.idregion = ciudad.region_idregion WHERE ciudad.descripcion = '{NombreCiudad}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();
        }

        public List<Region> GetRegionListByCiudad(string NombreCiudad)
        {
            List<Region> RegionLista = new();
            string sqlcommand = $"SELECT region.descripcion, region.idregion FROM CIUDAD INNER JOIN REGION ON region.idregion = ciudad.region_idregion WHERE ciudad.descripcion = '{NombreCiudad}'";
            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Region reg = new();

                reg.NumeroRegion = dataRow["idregion"].ToString();
                reg.Nombre = dataRow["descripcion"].ToString();

                RegionLista.Add(reg);
            }
            return RegionLista;
        }

        public List<ItemFamilia> GetFamiliaListByDepartamentoID(int DepID)
        {
            List<ItemFamilia> familiaItems = new();
            string sqlcommand = $"SELECT it.DESCRIPCION AS \"Nombre Item\", di.CANTIDAD as \"Cantidad\" FROM DETALLE_INVENTARIO di INNER JOIN ITEM it ON it.iditem = di.item_iditem INNER JOIN INVENTARIO inv ON di.inventario_idinventario = inv.idinventario WHERE inv.departamento_iddepartamento = '{DepID}'";

            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                ItemFamilia fi = new();
                fi.NombreItem = dataRow["Nombre Item"].ToString();
                fi.Cantidad = Convert.ToInt32(dataRow["Cantidad"]);

                familiaItems.Add(fi);
            }

            return familiaItems;
        }

        public List<string> GetTarifaList()
        {
            List<string> TarifaList = new();
            string sqlcommand = "SELECT MONTO_NOCHE FROM tarifa";
            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                TarifaList.Add(dataRow["MONTO_NOCHE"].ToString());
            }

            return TarifaList;
        }

        public string GetTarifaIdFromTarifaPrice(int price)
        {
            string sqlcommand = $"SELECT IDTARIFA FROM TARIFA WHERE MONTO_NOCHE = '{price}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();
            
        }

        public bool InsertApartment(string tarifa, string idcomuna, string direccion, string descripcion, byte[] fotoBig, string TituloApart)
        {

            OracleCommand cmd = new("INSERT INTO DEPARTAMENTO (IdDepartamento, Tarifa_IdTarifa, Comuna_IdComuna, Direccion, Descripcion, FotoBig, TituloDepart) VALUES (:1, :2, :3, :4, :5, :6, :7)", osc.OracleConnection);
            cmd.Parameters.Add("1", OracleDbType.Varchar2, CalculateID("IDDEPARTAMENTO","DEPARTAMENTO"), ParameterDirection.Input);
            cmd.Parameters.Add("2", OracleDbType.Varchar2, GetTarifaIdFromTarifaPrice(Convert.ToInt32(tarifa)), ParameterDirection.Input);
            cmd.Parameters.Add("3", OracleDbType.Varchar2, idcomuna, ParameterDirection.Input);
            cmd.Parameters.Add("4", OracleDbType.Varchar2, direccion, ParameterDirection.Input);
            cmd.Parameters.Add("5", OracleDbType.Varchar2, descripcion, ParameterDirection.Input);
            cmd.Parameters.Add("6", OracleDbType.Blob, fotoBig, ParameterDirection.Input);
            cmd.Parameters.Add("7", OracleDbType.Varchar2, TituloApart, ParameterDirection.Input);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
                
            }
        }

        public bool UpdateApartment(int IDDEPARTAMENTO, string tarifa, string idcomuna, string direccion, string descripcion, byte[] fotoBig, string TituloApart)
        {
            OracleCommand cmd = new("UPDATE DEPARTAMENTO SET Tarifa_IdTarifa = :1, Comuna_IdComuna = :2, Direccion = :3 , Descripcion = :4, FotoBig =:5, TituloDepart =:6 WHERE IDDEPARTAMENTO = " + IDDEPARTAMENTO.ToString(), osc.OracleConnection);
            cmd.Parameters.Add("1", OracleDbType.Varchar2, GetTarifaIdFromTarifaPrice(Convert.ToInt32(tarifa)), ParameterDirection.Input);
            cmd.Parameters.Add("2", OracleDbType.Varchar2, idcomuna, ParameterDirection.Input);
            cmd.Parameters.Add("3", OracleDbType.Varchar2, direccion, ParameterDirection.Input);
            cmd.Parameters.Add("4", OracleDbType.Varchar2, descripcion, ParameterDirection.Input);
            cmd.Parameters.Add("5", OracleDbType.Blob, fotoBig, ParameterDirection.Input);
            cmd.Parameters.Add("6", OracleDbType.Varchar2, TituloApart, ParameterDirection.Input);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public bool UpdateApartmentWithoutIM(int IDDEPARTAMENTO, string tarifa, string idcomuna, string direccion, string descripcion, string TituloApart)
        {
            OracleCommand cmd = new("UPDATE DEPARTAMENTO SET Tarifa_IdTarifa = :1, Comuna_IdComuna = :2, Direccion = :3 , Descripcion = :4, TituloDepart =:5 WHERE IDDEPARTAMENTO = " + IDDEPARTAMENTO.ToString(), osc.OracleConnection);
            cmd.Parameters.Add("1", OracleDbType.Varchar2, GetTarifaIdFromTarifaPrice(Convert.ToInt32(tarifa)), ParameterDirection.Input);
            cmd.Parameters.Add("2", OracleDbType.Varchar2, idcomuna, ParameterDirection.Input);
            cmd.Parameters.Add("3", OracleDbType.Varchar2, direccion, ParameterDirection.Input);
            cmd.Parameters.Add("4", OracleDbType.Varchar2, descripcion, ParameterDirection.Input);
            cmd.Parameters.Add("5", OracleDbType.Varchar2, TituloApart, ParameterDirection.Input);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public bool DeleteApartment(string ApartID)
        {
            string sqlcommand = $"DELETE FROM DEPARTAMENTO WHERE iddepartamento = '{ApartID}'";
            try
            {
                osc.RunOracleNonQuery(sqlcommand);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public List<Reserva> GetReservaListByDepID(int DepartamentoID)
        {
            List<Reserva> reservers = new();
            string sqlcommandres = string.Format("SELECT det.FECHA_INICIO AS \"FECHAINICIO\", det.FECHA_FIN AS \"FECHAFIN\" FROM DETAIL_ARRIENDO det INNER JOIN DEPARTAMENTO dep ON dep.iddepartamento = det.departamento_iddepartamento WHERE det.departamento_iddepartamento = '{0}'", DepartamentoID);
            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommandres).Rows)
            {
                Reserva reserva = new();
                reserva.FechaInicio = DateTime.Parse(dataRow["FECHAINICIO"].ToString());
                reserva.FechaFin = DateTime.Parse(dataRow["FECHAFIN"].ToString());
                reservers.Add(reserva);

            }

            return reservers;
        }
    }
}
