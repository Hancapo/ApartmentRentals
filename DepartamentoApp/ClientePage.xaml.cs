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
using SkyrentConnect;
using SkyrentObjects;
using MessageBox = System.Windows.MessageBox;
using SelectionMode = System.Windows.Controls.SelectionMode;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para ClientePage.xaml
    /// </summary>
    public partial class ClientePage : Page
    {
        private OracleSkyCon Osc = new();
        CommonBusiness Business = new CommonBusiness();
        bool editMode;
        public ClientePage()
        {
            InitializeComponent();
            DHSpamton.Visibility = Visibility.Hidden;
        }

        private void GridVerCliente_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
            //DgClienteGrid.ItemsSource = Osc.OracleToDataTable("SELECT c.rutcliente AS \"RUT de Cliente\", " +
            //    "us.nombreusuario AS \"Nombre de usuario\", co.nombre AS \"Nombre de Comuna\",c.nombre AS \"Nombres\",c.apellidop AS \"Apellido Paterno\"," +
            //    "c.apellidom AS \"Apellido Materno\" FROM COMUNA co INNER JOIN CLIENTE c ON co.idcomuna = c.comuna_idcomuna INNER JOIN USUARIO us ON c.usuario_idusuario = us.idusuario").DefaultView;
            //DgItemsGrid.ItemsSource = Osc.OracleToDataTable("SELECT IDITEM AS \"ID Item\", DESCRIPCION AS \"Nombre\", TO_CHAR(VALOR, '$9G999G999') AS \"Precio\", CANTIDAD AS \"Cantidad\" FROM ITEM").DefaultView;
            DgTarifasGrid.ItemsSource = Business.GetTarifaList();
        }

        private void btnCrearTarifa_Click(object sender, RoutedEventArgs e)
        {
            DHSpamton.Visibility = Visibility.Visible;
            editMode = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int TarifaINT = Int32.Parse(tbTarifa.Text);
            DHSpamton.Visibility = Visibility.Hidden;

            if (!editMode)
            {
                if (tbTarifa.Text != string.Empty)
                {
                    if (Business.CheckTarifa(TarifaINT))
                    {
                        if (Business.CreateTarifa(TarifaINT))
                        {
                            MessageBox.Show("La tarifa se ha creado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            DgTarifasGrid.ItemsSource = Business.GetTarifaList();
                            tbTarifa.Text = string.Empty;
                        }
                        else
                        {
                            MessageBox.Show("No se puede crear nuevamente una tarifa que ya existe, inténtelo nuevamente con un valor diferente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }

                    }
                    else
                    {
                        MessageBox.Show("La tarifa no se ha podido crear.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    MessageBox.Show("El campo no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                Tarifa TarifaFrom_ = ((Tarifa)DgTarifasGrid.SelectedItem);


                if (MessageBox.Show(string.Format("¿Desea modificar la tarifa con ID {0}?", TarifaFrom_.Id), "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    TarifaFrom_.Monto_Noche = Convert.ToInt32(tbTarifa.Text);

                    if (Business.ModifyTarifa(TarifaFrom_))
                    {
                        MessageBox.Show(String.Format("Se ha modificado la tarifa {0} con éxito.", TarifaFrom_.Id), "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        DgTarifasGrid.ItemsSource = Business.GetTarifaList();
                        tbTarifa.Text=string.Empty; 


                    }
                    else
                    {
                        MessageBox.Show(String.Format("No se ha podido modificar la tarifa {0}.", TarifaFrom_.Id), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            }
            



        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DHSpamton.Visibility = Visibility.Hidden;
            tbTarifa.Text = string.Empty;

        }

        private void DHSpamton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //DragMove();
            }
        }

        private void tbTarifa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Int32.TryParse(tbTarifa.Text, out int abc))
            {
                tbTarifa.Text = string.Empty;
            }
        }

        private void btnEliminarTarifa_Click(object sender, RoutedEventArgs e)
        {
            if (DgTarifasGrid.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una tarifa para borrar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                int Idcosa = ((Tarifa)DgTarifasGrid.SelectedItem).Id;

                if (Business.DeleteTarifa(Idcosa))
                {
                    MessageBox.Show(String.Format("Se ha eliminado la tarifa {0} con éxito.", Idcosa), "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    DgTarifasGrid.ItemsSource = Business.GetTarifaList();


                }
                else
                {
                    MessageBox.Show(String.Format("No se ha podido eliminar la tarifa {0}.", Idcosa), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }


            }
        }

        private void btnReloadTarifas_Click(object sender, RoutedEventArgs e)
        {
            DgTarifasGrid.ItemsSource = Business.GetTarifaList();
        }

        private void btnEditTarifas_Click(object sender, RoutedEventArgs e)
        {
            editMode = true;

            if (DgTarifasGrid.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una tarifa para modificar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                DHSpamton.Visibility = Visibility.Visible;
                Tarifa TarifaFrom_ = ((Tarifa)DgTarifasGrid.SelectedItem);

                tbTarifa.Text = TarifaFrom_.Monto_Noche.ToString();
            }

        }
    }
}
