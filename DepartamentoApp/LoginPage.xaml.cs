using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SkyrentBusiness;
using SkyrentConnect;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para Page1.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly CommonBusiness bsnss = new();
        private readonly OracleSkyCon osc = new();
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {

            Ingresar(tbUsuario.Text, pbPassword.Password);
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

        public void Ingresar(string usr, string pwd)
        {

            if (!(string.IsNullOrEmpty(usr) && string.IsNullOrEmpty(pwd)))
            {
                if (bsnss.LoginProc(usr, pwd).Item1)
                {

                    if (bsnss.LoginProc(usr, pbPassword.Password).Item3)
                    {
                        int UserType = bsnss.LoginProc(usr, pwd).Item2;

                        //MessageBox.Show(string.Format("Inicio de sesión exitoso, bienvenido {0}", usr), "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        switch (UserType)
                        {
                            case 1:
                                MainMenuPage MmpAdmin = new(UserType);
                                NavigationService.Navigate(MmpAdmin);
                                //MessageBox.Show("Tipo de Usuario: Admin", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case 2:
                                MainMenuPage MmpCustomer = new(UserType);

                                NavigationService.Navigate(MmpCustomer);
                                //MessageBox.Show("Tipo de Usuario: Cliente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnCuenta_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage rp = new();
            NavigationService.Navigate(rp);
        }
    }
}
