using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class Inventario
    {

        CommonBusiness CommonBusiness = new();

        public int IdInventario { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string Descripcion { get; set; }

        public List<Item>? items => CommonBusiness.GetItemListFromInventoryId(IdInventario);

        public List<Item> Items { get; set; }
    }
}