using SkyrentBusiness;
using SkyrentConnect;
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
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para NewLoginPage.xaml
    /// </summary>
    public partial class NewLoginPage : Page
    {
        private readonly CommonBusiness bsnss = new();
        private readonly OracleSkyCon osc = new();
        public NewLoginPage()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {

            Ingresar(tbUsuario.Text, pbPassword.Password);
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            if (osc.CheckDatabase())
            {
                MessageBox.Show("Se ha podido conectar con la base de datos", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("No se ha podido conectar con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void Ingresar(string usr, string pwd)
        {
            if (!(string.IsNullOrEmpty(usr) && string.IsNullOrEmpty(pwd)))
            {
                if (bsnss.LoginProc(usr, pwd).Item1)
                {

                    MainMenuPage MmpAdmin = new();
                    NavigationService.Navigate(MmpAdmin);
                               



                }
                else
                {
                    MessageBox.Show("Inicio de sesión fallido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Ingrese un usuario y contraseña para iniciar sesión.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        private void BtnCuenta_Click(object sender, RoutedEventArgs e)
        {
            //RegisterPage rp = new();
            //NavigationService.Navigate(rp);
        }


        private void tbUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Ingresar(tbUsuario.Text, pbPassword.Password);

            }
        }

        private void pbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Ingresar(tbUsuario.Text, pbPassword.Password);

            }
        }
    }
}
