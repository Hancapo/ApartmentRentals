using System.Windows;
using System.Windows.Controls;
using SkyrentObjects;
using Button = System.Windows.Controls.Button;
using MaterialDesignThemes.Wpf;
using System.Linq;
using ToolTip = System.Windows.Controls.ToolTip;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace DepartamentoApp.Apartment
{
    /// <summary>
    /// Lógica de interacción para ListApartmentPage.xaml
    /// </summary>
    public partial class ListApartmentPage : Page
    {
        readonly SkyUtilities su = new();


        public ListApartmentPage() 
        {
            InitializeComponent();
            cbFilterTypes.ItemsSource = new List<string>() { "Tarifa", "ID", "Título" };



        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {   
            ListaDepartamentos.ItemsSource = su.GetDepartamentoList();

        }

        private void DepartamentoCard_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Departamento depa = ((Card)sender).DataContext as Departamento;
            ApartmentView AvView = new(depa, false);
            NavigationService.Navigate(AvView);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ApartmentView AvCreation = new(null, true);
            NavigationService.Navigate(AvCreation);
        }

        private void cbFilterTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (cbFilterTypes.SelectedIndex)
            {
                case 0:
                    ListaDepartamentos.ItemsSource = su.GetDepartamentoList().OrderBy(x => x.TarifaDep);
                    break;
                case 1:
                    ListaDepartamentos.ItemsSource = su.GetDepartamentoList().OrderBy(x => x.IdDepartamento);
                    break;
                case 2:
                    ListaDepartamentos.ItemsSource = su.GetDepartamentoList().OrderBy(x => x.TituloDepartamento);
                    break;
                default:
                    break;
            }
        }

        private void DepartamentoCard_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            cbFilterTypes.SelectedIndex = -1;
            ListaDepartamentos.ItemsSource = su.GetDepartamentoList();
        }
    }
}
