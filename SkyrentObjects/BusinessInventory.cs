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
        public List<Item> GetitemList()
        {
            List<Item> ItemLista = new();
            string sqlcommand = string.Format("SELECT * FROM Item");
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Item i = new();
                i.IdItem = (int)dr["IdItem"];
                i.SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM = (int)dr["SUB_FAMILIA_ITEM_IDSUB_FAMILIA_ITEM"];
                i.Descripcion = (dr["Descripcion"]).ToString();
                i.Valor = (int)dr["Valor"];
                i.Cantidad = (int)dr["Cantidad"];

                ItemLista.Add(i);
            }
            return ItemLista;
        }
    }
}
