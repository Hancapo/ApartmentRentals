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
    /// Lógica de interacción para RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private CommonBusiness cbb = new();
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            LoginPage lp = new();
            NavigationService.Navigate(lp);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
            CbCiudad.IsEnabled = false;
            CbComuna.IsEnabled = false;

            CbRegion.ItemsSource = cbb.GetRegionList().Select(x => x.Nombre);
        }

        private void CbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string RegionCode = cbb.GetRegionList()[CbRegion.SelectedIndex].NumeroRegion;

            CbCiudad.IsEnabled = true;
            CbCiudad.ItemsSource = cbb.GetCiudadList(RegionCode);


        }

        private void CbCiudad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbComuna.IsEnabled = true;
            string ciudad = (sender as ComboBox).SelectedItem as string;
            CbComuna.ItemsSource = cbb.GetComunaList(ciudad);
        }

        private void BtnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            Cliente c = new() {ApellidoMaterno = TbApellidoM.Text, 
                ApellidoPaterno = TbApellidoP.Text, 
                ContrasenaUsuario = PbPassword.Password, 
                NombresCliente = TbNombres.Text, 
                NombreUsuario = tbNombreUsuario.Text, 
                RutCliente = TbRUT.Text, 
                ComunaCliente = cbb.GetIdComunaByName(CbComuna.Text).ToString()
            };
            int gogo = cbb.CreateUser(c);

            switch (gogo)
            {
                case 0:
                    MessageBox.Show("No se ha podido crear el usuario, revise los datos e inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 1:
                    MessageBox.Show("Usuario creado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 2:
                    MessageBox.Show("El usuario ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                default:
                    MessageBox.Show("Error desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    break;
            }

        }
    }
}
