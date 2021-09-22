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

        public List<FamiliaItem> returnFamiliaItems()
        {
            List<FamiliaItem> auxFamiliaItems = new();
            string returnFamiliaItems = string.Format("Select * FROM familia_item");
            foreach (DataRow dr in osc.RunOracleExecuteReader(returnFamiliaItems))
            {
                FamiliaItem auxFT = new();
                auxFT.IDFAMILIA_ITEM = dr["IDFAMILIA_ITEM"].ToString();
                auxFT.DESCRIPCION = dr["DESCRIPCION"].ToString();
                auxFamiliaItems.Add(auxFT);
            }
            return auxFamiliaItems;
        }
       

    }
}
