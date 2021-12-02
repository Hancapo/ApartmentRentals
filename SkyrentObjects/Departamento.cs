using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using SkyrentConnect;

namespace SkyrentObjects
{



    public class Departamento
    {
        public OracleSkyCon osc = new();

        public int IdDepartamento { get; set; }
        
        public int IdTarifaDep { get; set; }

        //private int TarifaDep;
        public int TarifaDep => Convert.ToInt32(osc.RunOracleExecuteScalar($"SELECT MONTO_NOCHE FROM TARIFA WHERE IDTARIFA = '{IdTarifaDep}'"));
        public int IdComunaDep { get; set; }
        public string ComunaDep { get; set; }
        public string DireccionDep { get; set; }
        public string DescripcionDep { get; set; }
        public BitmapImage FotoBig { get; set; }
        public string TituloDepartamento { get; set; }



    }
}
