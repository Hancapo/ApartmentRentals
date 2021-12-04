using SkyrentObjects;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application = System.Windows.Application;
using ComboBox = System.Windows.Controls.ComboBox;

namespace DepartamentoApp.Apartment
{
    /// <summary>
    /// Lógica de interacción para ComunaWindow.xaml
    /// </summary>
    public partial class ComunaWindow : Window
    {

        CommonBusiness cbb = new();

        bool editMode;
        string comuna;
        bool EditLoad;
        public ComunaWindow(bool EditMode, string Comuna)
        {
            // true = Modo edición; false = Modo edición
            editMode = EditMode;
            comuna = Comuna;
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (editMode)
            {

                //Comuna List
                string CiudadFromComuna = cbb.GetCiudadByComuna(comuna);
                cbComuna.ItemsSource = cbb.GetComunaListFromCiudad(CiudadFromComuna).Select(x => x.NombreComuna);

                //Selected Comuna
                cbComuna.SelectedIndex = cbComuna.Items.IndexOf(comuna);

                //Ciudad List
                string RegionIdFromCiudad = cbb.GetRegionIdByCiudad(CiudadFromComuna);
                cbCiudad.ItemsSource = cbb.GetCiudadListFromRegion(RegionIdFromCiudad).Select(x => x.NameCiudad);

                //Selected Ciudad
                cbCiudad.SelectedIndex = cbCiudad.Items.IndexOf(CiudadFromComuna);


                string RegionNombreFromCiudad = cbb.GetRegionByCiudad(CiudadFromComuna);


                //Region List
                cbRegion.ItemsSource = cbb.GetRegionList().Select(x => x.Nombre);
                //Selected Region
                cbRegion.SelectedIndex = cbRegion.Items.IndexOf(RegionNombreFromCiudad);

                EditLoad = true;


            }
            else
            {
                cbCiudad.IsEnabled = false; 
                cbComuna.IsEnabled = false;

                cbRegion.ItemsSource = cbb.GetRegionList().Select(x => x.Nombre);
            }
        }

        private void cbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editMode && EditLoad)
            {
                string RegionCode = cbb.GetRegionList()[cbRegion.SelectedIndex].IdRegion;
                cbComuna.ItemsSource = null;
                cbCiudad.ItemsSource = null;
                cbCiudad.SelectedIndex = -1;
                cbComuna.SelectedIndex = -1;
                cbCiudad.ItemsSource = cbb.GetCiudadListFromRegion(RegionCode).Select(x => x.NameCiudad);
                
            }
            else
            {
                string RegionCode = cbb.GetRegionList()[cbRegion.SelectedIndex].IdRegion;

                cbCiudad.IsEnabled = true;
                cbCiudad.ItemsSource = cbb.GetCiudadListFromRegion(RegionCode).Select(x => x.NameCiudad);
            }

        }

        private void cbCiudad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (editMode && cbCiudad.SelectedIndex != -1)
            {
                string NombreCiudad = (sender as ComboBox).SelectedItem as string;
                cbComuna.ItemsSource = cbb.GetComunaListFromCiudad(NombreCiudad).Select(x => x.NombreComuna);
            }

            if ((editMode && cbCiudad.SelectedIndex == -1) && cbRegion.SelectedIndex != -1)
            {
                string RegionCode = cbb.GetRegionList()[cbRegion.SelectedIndex].IdRegion;
                cbCiudad.ItemsSource = cbb.GetCiudadListFromRegion(RegionCode).Select(x => x.NameCiudad);
            }

            if (!editMode)
            {
                cbComuna.IsEnabled = true;
                string ciudad = (sender as ComboBox).SelectedItem as string;
                cbComuna.ItemsSource = cbb.GetComunaListFromCiudad(ciudad).Select(x => x.NombreComuna);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (editMode && EditLoad)
            {
                if (cbComuna.Text != string.Empty)
                {
                    var Window_ = Application.Current.MainWindow;

                    var ApartmentFrame_ = ((Window_ as GeneralWindow).Main.Content as MainMenuPage).ApartmentFrame;

                    var ApartmentView_ = ApartmentFrame_.Content as ApartmentView;

                    ApartmentView_.TbComuna.Text = cbComuna.Text;

                    Close();
                }
            }
            else
            {
                if (cbComuna.Text != string.Empty)
                {
                    var Window_ = Application.Current.MainWindow;

                    var ApartmentFrame_ = ((Window_ as GeneralWindow).Main.Content as MainMenuPage).ApartmentFrame;

                    var ApartmentView_ = ApartmentFrame_.Content as ApartmentView;

                    ApartmentView_.TbComuna.Text = cbComuna.Text; 

                    Close();
                }
            }
        }
    }
}
