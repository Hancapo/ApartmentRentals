using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class DetailArriendo
    {
        public int IdDetailArriendo { get; set; }

        public int Departamento_DepartamentoId { get; set; }

        public int MASTER_ARRIENDO_IDMASTER_ARRIENDO { get; set; }

        public DateTime FechaInicio { get; set; }   
        public DateTime FechaFin { get; set; }

        public int Monto { get; set; }

        public int EstadoPago { get; set; } 


    }
}
