using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public int TIPO_USUARIO_IDTIPO_USUARIO { get; set; }

        public string Password { get; set; }    

        public string NombreUsuario { get; set; }   

        public string CorreoElectronico { get; set; }
    }
}
