using System.IO;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class GeneralWindow : Window
    {
        public GeneralWindow()
        {
            InitializeComponent();
            if (!File.Exists("config.ini"))
            {
                MessageBox.Show("No se ha encontrado del archivo de configuración, el programa no puede continuar", "Error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            NewLoginPage lp = new();
            Main.NavigationService.Navigate(lp);
        }
         
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void gMainTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
