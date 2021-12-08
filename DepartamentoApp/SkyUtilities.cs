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
        private OracleSkyCon osc = new();
        

        public BitmapImage ToImage(byte[] array)
        {
            try
            {
                var ms = new MemoryStream(array);
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnDemand; // Cache
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public List<Departamento> GetDepartamentoList()
        {
            List<Departamento> DepLista = new();

            string sqlcommand = "SELECT d.iddepartamento AS \"IdDepartamento\", d.banos, d.capacidad, d.estado, d.dormitorio, d.fotobig AS \"FotoBig\", d.tarifa_idtarifa AS \"TarifaID\", t.monto_noche AS \"PrecioNoche\", c.nombre AS \"Comuna\", d.comuna_idcomuna AS \"IdComuna\", d.direccion AS \"Direccion\", d.descripcion as \"Descripcion\", d.titulodepart AS \"Titulo\" FROM DEPARTAMENTO d INNER JOIN COMUNA c ON d.comuna_idcomuna = c.idcomuna INNER JOIN TARIFA t ON d.tarifa_idtarifa = t.idtarifa";
            foreach (DataRow dr in osc.OracleToDataTable(sqlcommand).Rows)
            {


                Departamento dede = new();


                dede.IdTarifaDep = Convert.ToInt32(dr["TarifaID"]);
                dede.ComunaDep = dr["Comuna"].ToString();
                dede.DireccionDep = dr["Direccion"].ToString();
                dede.DescripcionDep = dr["Descripcion"].ToString();
                dede.Estado = Convert.ToInt32(dr["ESTADO"]);
                dede.Banos = Convert.ToInt32(dr["BANOS"]);
                dede.Dormitorio = Convert.ToInt32(dr["DORMITORIO"]);
                dede.Capacidad = Convert.ToInt32(dr["CAPACIDAD"]);
                try
                {
                    //Tries to load the image
                    dede.FotoBig = ToImage((byte[])dr["FotoBig"]);

                }
                catch (Exception)
                {
                    //Fails to load the image
                    dede.FotoBig = new BitmapImage(new Uri(@"/TurismoReal;component/Apartment/emptyimage.jpg", UriKind.Relative));
                }
                dede.TituloDepartamento = dr["Titulo"].ToString();
                dede.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);

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
