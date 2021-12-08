using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Invitado
    {
        public string RutInvitado { get; set; }

        public int DETAIL_ARRIENDO_IDDETAIL_ARRIENDO { get; set; }  
        
        public int COMUNA_IDCOMUNA { get; set; }

        public string NombreInvitado { get; set; }  

        public string ApellidoPaternoInvitado { get; set; } 
        public string ApellidoMaternoInvitado { get; set; }
    }
}
