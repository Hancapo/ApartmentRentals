using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Tarifa
    {
        CommonBusiness cb = new CommonBusiness();
        public int Id { get; set; }
        public int Monto_Noche { get; set; }
    }
}
