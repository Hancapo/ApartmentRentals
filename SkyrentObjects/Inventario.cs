using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Inventario
    {
        public int IdInventario { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int Descripcion { get; set; }
    }
}