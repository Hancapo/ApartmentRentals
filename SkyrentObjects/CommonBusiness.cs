﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
using Oracle.ManagedDataAccess.Client;
using SkyrentConnect;
using System.Linq;
using System.Collections.ObjectModel;

namespace SkyrentObjects
{
    public class CommonBusiness
    {

        public OracleSkyCon osc = new();




        public int LoginProc(string usuario, string contrasena)
        {
            string AccExistString = $"SELECT IDUSUARIO FROM USUARIO WHERE NOMBREUSUARIO = '{usuario}'";
            string AccExistsIsCustomer = $"SELECT IDUSUARIO FROM USUARIO WHERE NOMBREUSUARIO = '{usuario}' AND tipo_usuario_idtipo_usuario = 2";
            string AccExistsIsAdmin = $"SELECT IDUSUARIO FROM USUARIO WHERE NOMBREUSUARIO = '{usuario}' AND tipo_usuario_idtipo_usuario = 1";
            string AccExistWrongPwd = $"SELECT IDUSUARIO FROM USUARIO WHERE NOMBREUSUARIO = '{usuario}' AND password = '{contrasena}'";


            //0 -> La cuenta no existe.
            //1 -> La cuenta sí existe.
            //2 -> La cuenta existe pero la contraseña es incorrecta.
            //3 -> La cuenta existe pero corresponde a una cuenta de cliente.
            //4 -> La cuenta existe y corresponde a una cuenta de administrador.
            //5 -> La cuenta es de administrador y la contraseña es correcta.

            int LoginType;

            if (osc.RunOracleExecuteScalar(AccExistString) == null)
            {
                LoginType = 0;
            }
            else
            {
                LoginType = 1;
                if (osc.RunOracleExecuteScalar(AccExistsIsCustomer) != null)
                {
                    LoginType = 3;
                }
                else
                {
                    if (osc.RunOracleExecuteScalar(AccExistsIsAdmin) != null)
                    {
                        LoginType = 4;

                        if (osc.RunOracleExecuteScalar(AccExistWrongPwd) != null)
                        {
                            LoginType = 5;
                        }
                        else
                        {
                            LoginType = 2;
                        }
                    }
                }
            }

            return LoginType;

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

            return ListaID.Max() + 1;

        }

        public List<Region> GetRegionList()
        {
            List<Region> RegionLista = new();
            string sqlcommand = string.Format("SELECT * FROM REGION");
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Region r = new();
                r.Nombre = dr["NOMBRE"].ToString();
                r.IdRegion = dr["IDREGION"].ToString();

                RegionLista.Add(r);

            }
            return RegionLista;
        }

        public List<Ciudad> GetCiudadListFromRegion(string CodeRegion)
        {
            List<Ciudad> CiudadLista = new();
            string sqlcommand = $"SELECT NOMBRE, IDCIUDAD FROM CIUDAD WHERE REGION_IDREGION = '{CodeRegion}'";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Ciudad ci = new();

                ci.NameCiudad = dr["Nombre"].ToString();
                ci.IdCiudad = Convert.ToInt32(dr["IdCiudad"]);
                CiudadLista.Add(ci);
            }
            return CiudadLista;
        }

        public List<Comuna> GetComunaListFromCiudad(string Ciudad)
        {
            List<Comuna> RegionLista = new();
            string sqlcommand = $"SELECT comuna.nombre, comuna.idcomuna FROM CIUDAD INNER JOIN COMUNA ON ciudad.idciudad = comuna.ciudad_idciudad WHERE ciudad.nombre = '{Ciudad}'";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {

                Comuna com = new();
                com.NombreComuna = dr["Nombre"].ToString();
                com.IDComuna = Convert.ToInt32(dr["IDCOMUNA"]);
                RegionLista.Add(com); ;

            }
            return RegionLista;
        }

        public int GetIdComunaByName(string NombreComuna)
        {

            string sqlcommand = $"SELECT idcomuna FROM COMUNA WHERE COMUNA.nombre = '{NombreComuna}'";
            return Convert.ToInt32(osc.RunOracleExecuteScalar(sqlcommand));

        }


        public string GetCiudadByComuna(string NombreComuna)
        {
            string sqlcommand = $"SELECT ciudad.nombre FROM COMUNA INNER JOIN CIUDAD ON ciudad.idciudad = comuna.ciudad_idciudad WHERE comuna.nombre = '{NombreComuna}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();

        }


        public string GetRegionIdByCiudad(string NombreCiudad)
        {
            string sqlcommand = $"SELECT region.idregion FROM CIUDAD INNER JOIN REGION ON region.idregion = ciudad.region_idregion WHERE ciudad.nombre = '{NombreCiudad}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();
        }

        public string GetRegionByCiudad(string NombreCiudad)
        {
            string sqlcommand = $"SELECT region.nombre FROM CIUDAD INNER JOIN REGION ON region.idregion = ciudad.region_idregion WHERE ciudad.nombre = '{NombreCiudad}'";
            return osc.RunOracleExecuteScalar(sqlcommand).ToString();
        }

        public List<Region> GetRegionListByCiudad(string NombreCiudad)
        {
            List<Region> RegionLista = new();
            string sqlcommand = $"SELECT region.nombre, region.idregion FROM CIUDAD INNER JOIN REGION ON region.idregion = ciudad.region_idregion WHERE ciudad.nombre = '{NombreCiudad}'";
            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Region reg = new();

                reg.IdRegion = dataRow["idregion"].ToString();
                reg.Nombre = dataRow["nombre"].ToString();

                RegionLista.Add(reg);
            }
            return RegionLista;
        }

        public List<Item> GetFamiliaListByDepartamentoID(int DepID)
        {
            List<Item> familiaItems = new();
            string sqlcommand = $"SELECT it.DESCRIPCION AS \"Nombre Item\", di.CANTIDAD as \"Cantidad\" FROM DETALLE_INVENTARIO di INNER JOIN ITEM it ON it.iditem = di.item_iditem INNER JOIN INVENTARIO inv ON di.inventario_idinventario = inv.idinventario WHERE inv.departamento_iddepartamento = {DepID}";

            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Item fi = new();
                fi.Descripcion = dataRow["Nombre Item"].ToString();
                fi.Cantidad = Convert.ToInt32(dataRow["Cantidad"]);

                familiaItems.Add(fi);
            }

            return familiaItems;
        }

        public List<Tarifa> GetTarifaList()
        {
            List<Tarifa> TarifaList = new();
            string sqlcommand = "SELECT * FROM TARIFA ORDER BY TARIFA.idtarifa";
            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Tarifa t = new();
                t.Id = Convert.ToInt32(dataRow["IDTARIFA"]);
                t.Monto_Noche = Convert.ToInt32(dataRow["MONTO_NOCHE"]);
                TarifaList.Add(t);
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
            cmd.Parameters.Add("1", OracleDbType.Varchar2, CalculateID("IDDEPARTAMENTO", "DEPARTAMENTO"), ParameterDirection.Input);
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

        public List<RegionDepGraph> GetRegionDepGraph()
        {
            List<RegionDepGraph> regionDepGraphs = new List<RegionDepGraph>();

            string sqlcommand = "SELECT COUNT(dep.IDDEPARTAMENTO) AS \"Cantidad\", reg.nombre AS \"Region\" FROM DEPARTAMENTO dep INNER JOIN COMUNA comu ON comu.idcomuna = dep.comuna_idcomuna INNER JOIN CIUDAD ciu ON ciu.idciudad = comu.ciudad_idciudad INNER JOIN REGION reg ON ciu.region_idregion = reg.idregion GROUP BY reg.nombre";

            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                RegionDepGraph rdg = new();
                rdg.Cantidad = Convert.ToInt32(dataRow["Cantidad"]);
                rdg.NombreRegion = dataRow["Region"].ToString();

                regionDepGraphs.Add(rdg);
            }

            return regionDepGraphs;
        }

        public bool CreateTarifa(int Money)
        {
            Tarifa t = new() { Monto_Noche = Money, Id = CalculateID("IDTARIFA", "TARIFA") };
            OracleCommand orc = new("SP_CREARTARIFA", osc.OracleConnection);
            orc.CommandType = CommandType.StoredProcedure;
            orc.Parameters.Add("@P_idTarifa", OracleDbType.Int32, t.Id, ParameterDirection.Input);
            orc.Parameters.Add("@P_montoNoche", OracleDbType.Int32, t.Monto_Noche, ParameterDirection.Input);


            try
            {
                orc.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTarifa(int ID_)
        {
            OracleCommand orc = new("sp_EliminarTarifa", osc.OracleConnection);
            orc.CommandType = CommandType.StoredProcedure;
            orc.Parameters.Add("@p_idtarifa", OracleDbType.Int32, ID_, ParameterDirection.Input);

            try
            {
                orc.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DoesTarifaExists(int Price)
        {
            List<int> ListaPrecios = new();

            string sqlcommand = "SELECT * FROM TARIFA ORDER BY TARIFA.idtarifa";

            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                ListaPrecios.Add(Convert.ToInt32(dataRow["MONTO_NOCHE"]));
            }

            foreach (var item in ListaPrecios)
            {

                if (item == Price)
                {
                    return false;
                }

            }

            return true;
        }

        public bool ModifyTarifa(Tarifa t)
        {
            OracleCommand orc = new("sp_actualizaTarifa", osc.OracleConnection);
            orc.CommandType = CommandType.StoredProcedure;
            orc.Parameters.Add("@p_idTarifa", OracleDbType.Int32, t.Id, ParameterDirection.Input);
            orc.Parameters.Add("@P_montoNoche", OracleDbType.Int32, t.Monto_Noche, ParameterDirection.Input);

            try
            {
                orc.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<FamiliaItem> GetFamiliaItemList()
        {
            List<FamiliaItem> NewFamiliaItemList = new();
            string sqlcommand = "SELECT * FROM FAMILIA_ITEM";
            foreach (DataRow dataRow in osc.OracleToDataTable(sqlcommand).Rows)
            {
                FamiliaItem fi = new() { IdFamilia_Item = Convert.ToInt32(dataRow["idfamilia_item"]), Descripcion = dataRow["descripcion"].ToString() };

                NewFamiliaItemList.Add(fi);
            }

            return NewFamiliaItemList;
        }

        public List<SubFamiliaItem> GetNombreSubFamiliaItemFromID(int FamiliaID)
        {
            List<SubFamiliaItem> subFamiliaItems = new();

            string sqlcommand = $"SELECT sfi.NOMBRE, sfi.familia_item_idfamilia_item, sfi.idsub_familia_item FROM SUB_FAMILIA_ITEM sfi INNER JOIN familia_item fi ON fi.idfamilia_item = sfi.familia_item_idfamilia_item WHERE fi.idfamilia_item = {FamiliaID}";
            foreach (DataRow data in osc.OracleToDataTable(sqlcommand).Rows)
            {
                SubFamiliaItem subFamiliaItem = new()
                {
                    Nombre = data["NOMBRE"].ToString(),
                    Familia_Item_IdFamilia_Item = Convert.ToInt32(data["familia_item_idfamilia_item"]),
                    IdSub_Familia_Item = Convert.ToInt32(data["idsub_familia_item"])
                };

                subFamiliaItems.Add(subFamiliaItem);    
            }

            return subFamiliaItems;
        }

        public bool AddItem(Item it)
        {

            string sqlcommand = $"INSERT INTO ITEM (iditem, sub_familia_item_idsub_familia_item, descripcion, valor, cantidad) VALUES ({it.IdItem}, {it.SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM}, '{it.Descripcion}', {it.Valor}, {it.Cantidad})";

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

        public bool DeleteItem(Item it)
        {
            string sqlcommand = $"DELETE FROM ITEM WHERE ITEM.iditem = {it.IdItem}";

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

        public int GetFamiliaItemIdFromFamiliaItemName(string FamiliaName)
        {
            string sqlcommand = $"SELECT idfamilia_item FROM FAMILIA_ITEM WHERE descripcion='{FamiliaName}'";

            return Convert.ToInt32(osc.RunOracleExecuteScalar(sqlcommand));  
        }

        public int GetSubFamiliaItemIdFromSubFamiliaItemName(string SubFamiliaName)
        {
            string sqlcommand = $"SELECT IDSUB_FAMILIA_ITEM FROM SUB_FAMILIA_ITEM WHERE nombre='{SubFamiliaName}'";

            return Convert.ToInt32(osc.RunOracleExecuteScalar(sqlcommand));
        }

        public ObservableCollection<Item> GetItemList()
        {
            ObservableCollection<Item> ItemListo = new ObservableCollection<Item>();

            string sqlcommand = "SELECT * FROM ITEM ORDER BY IDITEM";

            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Item im = new() {
                    IdItem = Convert.ToInt32(dr["IDITEM"]),
                    SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM = Convert.ToInt32(dr["SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM"]),
                    Descripcion = dr["DESCRIPCION"].ToString(),
                    Valor = Convert.ToInt32(dr["VALOR"]),
                    Cantidad = Convert.ToInt32(dr["CANTIDAD"])
                };

                ItemListo.Add(im);  
            }

            return ItemListo;

        }

    }
}