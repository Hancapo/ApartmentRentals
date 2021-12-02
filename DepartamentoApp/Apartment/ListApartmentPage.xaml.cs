using System.Windows;
using System.Windows.Controls;
using SkyrentBusiness;
using SkyrentObjects;
using Button = System.Windows.Controls.Button;
using MaterialDesignThemes.Wpf;

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
    }
}
