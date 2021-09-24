using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Departamento
    {
        public int IdDepartamento { get; set; }
        public string TarifaDep { get; set; }
        public string ComunaDep { get; set; }
        public string DireccionDep { get; set; }
        public string DescripcionDep { get; set; }
        public byte[] FotoSmall { get; set; }
        public byte[] FotoBig { get; set; }
        public string TituloDepartamento { get; set; }

    }
}
