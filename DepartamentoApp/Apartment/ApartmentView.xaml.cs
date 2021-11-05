using SkyrentObjects;
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

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para ApartmentView.xaml
    /// </summary>
    public partial class ApartmentView : Page
    {
        public ApartmentView(Departamento dep)
        {
            InitializeComponent();
            SetValuesDep(dep);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SetValuesDep(Departamento dd)
        {
            tbDireccion.Text = dd.DireccionDep;
            ImBig.Source = dd.FotoBig;
            TbComuna.Text = dd.ComunaDep;
            TTitulo.Text = dd.TituloDepartamento;
            
            CbTarifa.Text = dd.tarifaDep.ToString();
            TDescripcion.Text = dd.DescripcionDep;

        }
    }
}
