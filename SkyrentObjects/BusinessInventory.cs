using SkyrentConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrentObjects
{
    public class BusinessInventory
    {
        public OracleSkyCon osc = new();
        public List<Item> GetItemList()
        {
            List<Item> ItemLista = new();
            string sqlcommand = string.Format("SELECT * FROM Item");
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Item i = new();
                i.IdItem = Convert.ToInt32(dr["IdItem"]);
                i.SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM = Convert.ToInt32(dr["SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM"]);
                i.Descripcion = (dr["Descripcion"]).ToString();
                i.Valor = Convert.ToInt32(dr["Valor"]);
                i.Cantidad = Convert.ToInt32(dr["Cantidad"]);

                ItemLista.Add(i);
            }
            return ItemLista;
        }
    }
}
