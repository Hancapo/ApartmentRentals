using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkyrentConnect;
using SkyrentObjects;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para ClientePage.xaml
    /// </summary>
    public partial class ClientePage : Page
    {
        private OracleSkyCon Osc = new();
        public ClientePage()
        {
            InitializeComponent();
        }

        private void GridVerCliente_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
            DgClienteGrid.ItemsSource = Osc.OracleToDataTable("SELECT c.rutcliente AS \"RUT de Cliente\", " +
                "us.nombreusuario AS \"Nombre de usuario\", co.nombre AS \"Nombre de Comuna\",c.nombre AS \"Nombres\",c.apellidop AS \"Apellido Paterno\"," +
                "c.apellidom AS \"Apellido Materno\" FROM COMUNA co INNER JOIN CLIENTE c ON co.idcomuna = c.comuna_idcomuna INNER JOIN USUARIO us ON c.usuario_idusuario = us.idusuario").DefaultView;
            DgItemsGrid.ItemsSource = Osc.OracleToDataTable("SELECT IDITEM AS \"ID Item\", DESCRIPCION AS \"Nombre\", TO_CHAR(VALOR, '$9G999G999') AS \"Precio\", CANTIDAD AS \"Cantidad\" FROM ITEM").DefaultView;
            DgTarifasGrid.ItemsSource = Osc.OracleToDataTable("SELECT IDTARIFA AS \"ID Tarifa\", TO_CHAR(MONTO_NOCHE, '$9G999G999') AS \"Costo Noche\" FROM TARIFA").DefaultView;
        }
    }
}
