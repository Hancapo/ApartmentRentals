using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;

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
