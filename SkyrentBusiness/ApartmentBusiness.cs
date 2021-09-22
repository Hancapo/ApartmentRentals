using SkyrentBusiness;
using SkyrentObjects;
using SkyrentConnect;
using System;
using System.Collections.Generic;
using System.Data;


namespace SkyrentBusiness.ApartmentBusiness
{
    public class ApartmentBusiness
    {
        private OracleSkyCon osc = new();

        public List<FamiliaItem> ReturnFamiliaItems()
        {
            List<FamiliaItem> auxFamiliaItems = new();
            string returnFamiliaItems = "SELECT * FROM familia_item";
            foreach (DataRow dr in osc.OracleToDataTable(returnFamiliaItems).Rows)
            {
                FamiliaItem auxFT = new() {
                    IDFAMILIA_ITEM = dr["IDFAMILIA_ITEM"].ToString(),
                    DESCRIPCION = dr["DESCRIPCION"].ToString()
                };

                auxFamiliaItems.Add(auxFT);
            }
            return auxFamiliaItems;
        }
       

    }
}
