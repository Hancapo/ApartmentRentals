using DepartamentoApp.Apartment;
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
    /// Lógica de interacción para MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        private int UserType;
        public MainMenuPage(int UserType)
        {
            InitializeComponent();
            SetUserType(UserType);
            AddApartment ap = new();
            InicioFrame.NavigationService.Navigate(ap);
        }


        public void SetUserType(int UsrType)
        {
            UserType = UsrType;
            switch (UsrType)
            {
                case 1:
                    LbType.Content = "Modo Administrador";
                    break;
                case 2:
                    LbType.Content = "Modo Cliente";
                    break;
                default:
                    break;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LoginPage lp = new();
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
            ClientePage cp = new(UserType);
            AdminFrame.NavigationService.Navigate(cp);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabItemInicio_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show("let's go");
        }
    }
}
