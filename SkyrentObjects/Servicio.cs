using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Servicio
    {
        public int IdServicio { get; set; }
        public int DETAIL_ARRIENDO_IDDETAIL_ARRIENDO { get; set; }

        public int SUB_FAMILIA_SERVICIO_IDSUB_FAMILIA_SERVICIO { get; set; }
        
        public string DESCRIPCION { get; set; }

        public int Valor { get; set; }  
        
    }
}
