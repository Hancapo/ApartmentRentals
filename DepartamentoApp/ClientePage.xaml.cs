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
using SkyrentBusiness;
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
            DgClienteGrid.ItemsSource = Osc.OracleToDataTable("SELECT cliente.rutcliente AS \"RUT de Cliente\", cliente.usuario_idusuario AS \"ID de Usuario\", cliente.comuna_idcomuna AS \"Comuna\", cliente.nombre AS \"Nombre\",cliente.apellidop AS \"Apellido Paterno\", cliente.apellidom AS \"Apellido Materno\" FROM CLIENTE").DefaultView;
        }
    }
}
