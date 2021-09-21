using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using SkyrentConnect;


namespace SkyrentBusiness
{
    public class Business
    {

        private OracleSkyCon osc = new();

        public (bool, int, bool) IniciarSesion(string usuario, string contrasena)
        {
            int UserType = 32767;
            bool IsConnected;
            bool AccountExists;

            if (osc.RunOracleExecuteScalar(string.Format("SELECT tipo_usuario_idtipo_usuario FROM USUARIO WHERE (password_2 = '{0}' AND nombreusuario = '{1}')", contrasena, usuario)) == null)
            {
                IsConnected = true;
                AccountExists = false;
            }
            else
            {
                UserType = Convert.ToInt32(osc.RunOracleExecuteScalar(string.Format("SELECT tipo_usuario_idtipo_usuario FROM USUARIO WHERE (password_2 = '{0}' AND nombreusuario = '{1}')", contrasena, usuario)));
                IsConnected = true;
                AccountExists = true;

            }


            return (IsConnected, UserType, AccountExists);

        }


    }
}
