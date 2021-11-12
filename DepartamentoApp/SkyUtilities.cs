using SkyrentConnect;
using SkyrentObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Media.Imaging;

namespace DepartamentoApp
{
    public class SkyUtilities
    {
        private readonly OracleSkyCon osc = new();


        public BitmapImage ToImage(byte[] array)
        {
            var ms = new MemoryStream(array);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // Cache
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }


        public List<Departamento> GetDepartamentoList()
        {
            List<Departamento> DepLista = new();

            string sqlcommand = "SELECT d.iddepartamento AS \"IdDepartamento\", d.fotobig AS \"FotoBig\", d.tarifa_idtarifa AS \"TarifaID\", t.monto_noche AS \"PrecioNoche\", c.descripcion AS \"Comuna\", d.comuna_idcomuna AS \"IdComuna\", d.direccion AS \"Direccion\", d.descripcion as \"Descripcion\", d.titulodepart AS \"Titulo\" FROM DEPARTAMENTO d INNER JOIN COMUNA c ON d.comuna_idcomuna = c.idcomuna INNER JOIN TARIFA t ON d.tarifa_idtarifa = t.idtarifa";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {
                Departamento dede = new()
                {
                    IdTarifaDep = dr["TarifaID"].ToString(),
                    //TarifaDep = "$" + Convert.ToInt32(dr["PrecioNoche"]).ToString("N0") + " por noche",
                    ComunaDep = dr["Comuna"].ToString(),
                    DireccionDep = dr["Direccion"].ToString(),
                    DescripcionDep = dr["Descripcion"].ToString(),
                    FotoBig = ToImage((byte[])dr["FotoBig"]),
                    TituloDepartamento = dr["Titulo"].ToString(),
                    IdDepartamento = Convert.ToInt32(dr["IdDepartamento"])

                };

                DepLista.Add(dede);
            }

            
            return DepLista;
        }

        public byte[] ImagePathToBytes(string imagepath)
        {
            return File.ReadAllBytes(imagepath);
        }

        public void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            var fileStream = new FileStream(filePath, FileMode.Create);
            encoder.Save(fileStream);
        }
    }
}
