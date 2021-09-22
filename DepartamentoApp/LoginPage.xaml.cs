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

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para Page1.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Business bsnss = new();
        private OracleSkyCon osc = new();

        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            

            if (!(string.IsNullOrEmpty(tbUsuario.Text) && string.IsNullOrEmpty(pbPassword.Password)))
            {
                if (bsnss.LoginProc(tbUsuario.Text, pbPassword.Password).Item1)
                {

                    if (bsnss.LoginProc(tbUsuario.Text, pbPassword.Password).Item3)
                    {
                        bool Connected = bsnss.LoginProc(tbUsuario.Text, pbPassword.Password).Item1;
                        int UserType = bsnss.LoginProc(tbUsuario.Text, pbPassword.Password).Item2;

                        MessageBox.Show(string.Format("Inicio de sesión exitoso, bienvenido {0}", tbUsuario.Text), "Información", MessageBoxButton.OK, MessageBoxImage.Information);




                        switch (UserType)
                        {
                            case 1:
                                MainMenuPage MmpAdmin = new(UserType);
                                NavigationService.Navigate(MmpAdmin);
                                MessageBox.Show("Tipo de Usuario: Admin", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case 2:
                                MainMenuPage MmpCustomer = new(UserType);

                                NavigationService.Navigate(MmpCustomer);
                                MessageBox.Show("Tipo de Usuario: Cliente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("La cuenta ingresada no existe, verifica tus datos e inténtalo nuevamente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }



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

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            if (osc.CheckOracleConnection().Item1)
            {
                MessageBox.Show("Se ha podido conectar con la base de datos", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("No se ha podido conectar con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

    }
}
