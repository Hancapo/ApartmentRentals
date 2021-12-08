using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkyrentConnect;

namespace SkyrentObjects
{



    public class Departamento
    {
        public OracleSkyCon osc = new();
        public CommonBusiness cbb = new();

        public int IdDepartamento { get; set; }

        public int IdTarifaDep { get; set; }

        public int TarifaDep => Convert.ToInt32(osc.RunOracleExecuteScalar($"SELECT MONTO_NOCHE FROM TARIFA WHERE IDTARIFA = '{IdTarifaDep}'"));
        public int IdComunaDep { get; set; }
        public string ComunaDep { get; set; }
        public string DireccionDep { get; set; }
        public string DescripcionDep { get; set; }
        public BitmapImage FotoBig { get; set; }
        public string TituloDepartamento { get; set; }

        public int Dormitorio { get; set; }

        public int Banos { get; set; }

        public int Capacidad { get; set; }

        public int Estado { get; set; }


        public Brush estadoColor()
        {
            return Estado switch
            {
                1 => (Brush)new BrushConverter().ConvertFrom("#" + "719FB0"),
                2 => (Brush)new BrushConverter().ConvertFrom("#" + "3D0000"),
                3 => (Brush)new BrushConverter().ConvertFrom("#" + "4E9F3D"),
                4 => (Brush)new BrushConverter().ConvertFrom("#" + "F05454"),
                _ => (Brush)new BrushConverter().ConvertFrom("#" + "2B2B2B"),
            };
        }

        public Brush EstadoColor => estadoColor();

        public string Ciudad => cbb.GetCiudadByComuna(ComunaDep);
        public string Region => cbb.GetRegionByCiudad(Ciudad);


    }
}
