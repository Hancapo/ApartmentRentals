using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using SkyrentBusiness;
using SkyrentConnect;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para InicioPage.xaml
    /// </summary>
    public partial class InicioPage : Page
    {
        CommonBusiness cb = new();
        public InicioPage()
        {
            InitializeComponent();

            CargarDatosRegionCantidad();
            

        }


        public void CargarDatosRegionCantidad()
        {
            ChartValues<int> CantidadPorRegion = new(cb.GetRegionDepGraph().Select(x => x.Cantidad).ToArray());

            SeriesCollection sc = new();

            ColumnSeries cs = new() { Values = new ChartValues<int>(CantidadPorRegion) };
            sc.Add(cs);

            ChartRegionesDep.Series = sc;

            AxisRegionsDep.Labels = cb.GetRegionDepGraph().Select(x => x.NombreRegion).ToArray();
        }

    }
}
