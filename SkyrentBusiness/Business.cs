using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using SkyrentConnect;
using SkyrentObjects;


namespace SkyrentBusiness
{
    public class Business
    {

        private OracleSkyCon osc = new();

        public (bool, int, bool) LoginProc(string usuario, string contrasena)
        {
            int UserType = 0x7FFF;
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

            string CreateUserCommand = string.Format("INSERT INTO USUARIO (idusuario, password_2, nombreusuario, tipo_usuario_idtipo_usario) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}')", CalculateID("idusuario", "usuario"), clie.ContrasenaUsuario, clie.NombreUsuario, 2);
            string SearchUsersCommand = string.Format("SELECT COUNT(rutcliente) FROM CLIENTE WHERE rutcliente = '{0}'", clie.RutCliente);



            int UsersWithSameRUT = Convert.ToInt32(osc.RunOracleExecuteScalar(SearchUsersCommand));


            if (UsersWithSameRUT <= 0)
            {
                try
                {
                    osc.RunOracleNonQuery(CreateUserCommand);
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
            int CountID = Convert.ToInt32(string.Format("SELECT COUNT({0}) FROM {1}", IdColumnName, TableName));


            if (CountID <= 0)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(osc.RunOracleExecuteScalar(LookUpForIDCommand));

            }

        }


    }
}
