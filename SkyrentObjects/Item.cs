using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Item
    {

        public int IdItem { get; set; }

        public int SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM { get; set; }

        public string Descripcion { get; set; }
        
        public int Valor { get; set; }  
        public int Cantidad { get; set; }
    }
}
