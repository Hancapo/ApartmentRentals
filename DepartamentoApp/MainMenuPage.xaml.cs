using DepartamentoApp.Apartment;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MessageBox = System.Windows.MessageBox;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
            //AddApartment ap = new();
            //InicioFrame.NavigationService.Navigate(ap);
        }


        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NewLoginPage lp = new();
                NavigationService.Navigate(lp);
            }
        }

        private void TabItemInicio_Loaded(object sender, RoutedEventArgs e)
        {
            InicioPage ip = new();
            InicioFrame.NavigationService.Navigate(ip);
        }

        private void TabItemDeparta_Loaded(object sender, RoutedEventArgs e)
        {
            ListApartmentPage AddA = new();
            ApartmentFrame.NavigationService.Navigate(AddA);
        }

        private void TabItemCliente_Loaded(object sender, RoutedEventArgs e)
        {
            ClientePage cp = new();
            AdminFrame.NavigationService.Navigate(cp);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabItemInicio_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //MessageBox.Show("let's go");
        }
    }
}
