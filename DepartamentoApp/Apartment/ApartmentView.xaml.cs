using SkyrentObjects;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using DepartamentoApp.Apartment;
using System.Globalization;
using SkyrentConnect;
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;

namespace DepartamentoApp
{
    /// <summary>
    /// Lógica de interacción para ApartmentView.xaml
    /// </summary>
    public partial class ApartmentView : Page
    {
        CommonBusiness NegocioComun = new();
        SkyUtilities su = new();
        bool editMode = false;
        bool ImageChanged = false;
        Departamento dep_;
        bool creationMode;
        string fileName;
        


        public ApartmentView(Departamento dep, bool CreationMode)
        {
            dep_ = dep;
            creationMode = CreationMode;
            InitializeComponent();
            if (creationMode)
            {
                //Visibility settings
                Ccalendario.Visibility = Visibility.Hidden;
                lbCalend.Visibility = Visibility.Hidden;
                cbEditMode.Visibility = Visibility.Hidden;
                SpMiscControls.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnSaveChanges.Visibility = Visibility.Visible;

                //IsEnable settings
                btnSaveChanges.IsEnabled = true;
                tbDireccion.IsEnabled = true;
                TbComuna.IsEnabled = true;
                CbTarifa.IsEnabled = true;
                TTitulo.IsEnabled = true;
                TDescripcion.IsEnabled = true;
                ImBig.IsEnabled = true;
                TbComuna.IsEnabled = true;
                CbTarifa.IsEnabled = true;

                //Text and Content settings
                lbTextoCambios.Content = "Agregar";

                TTitulo.Text = String.Empty;
                TDescripcion.Text = String.Empty;
                TbComuna.Text = String.Empty;
                tbDireccion.Text = String.Empty;
                CbTarifa.SelectedItem = null;
                CbTarifa.ItemsSource = NegocioComun.GetTarifaList();

                //Misc settings
                ImBig.Source = new BitmapImage(new Uri(@"/TurismoReal;component/Apartment/emptyimage.jpg", UriKind.Relative));

            }
            else
            {
                SetValuesDep(dep_);
                dgInventario.ItemsSource = NegocioComun.GetFamiliaListByDepartamentoID(dep_.IdDepartamento);
                xListReservas.ItemsSource = NegocioComun.GetReservaListByDepID(dep_.IdDepartamento);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                if (MessageBox.Show("Hay cambios sin guardar, ¿desea volver de todas formas?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    editMode = false;
                    NavigationService.GoBack();

                }

            }
            else
            {
                NavigationService.GoBack();

            }
        }

        private void SetValuesDep(Departamento dd)
        {
            cbEditMode.Background = System.Windows.Media.Brushes.DarkRed;

            tbDireccion.Text = dd.DireccionDep;
            ImBig.Source = dd.FotoBig;
            TbComuna.Text = dd.ComunaDep;
            TTitulo.Text = dd.TituloDepartamento;
            CbTarifa.SelectedIndex = Convert.ToInt32(dd.IdTarifaDep) - 1;
            CbTarifa.ItemsSource = NegocioComun.GetTarifaList();
            TDescripcion.Text = dd.DescripcionDep;

        }

        private void ImBig_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            OpenFileDialog ofd = new();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.JPEG)|*.BMP;*.JPG;*.GIF;*.PNG;*.JPEG";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                ImBig.Source = new BitmapImage(new Uri(fileName));
                ImageChanged = true;
            }
        }

        private void cbEditMode_Click(object sender, RoutedEventArgs e)
        {
            
            if (editMode == false)
            {
                if (MessageBox.Show("¿Activar modo edición?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    cbEditMode.Background = System.Windows.Media.Brushes.DarkGreen;
                    editMode = true;
                }

                
                else
                {
                    editMode = false;
                }
            }
            else
            {

                if (MessageBox.Show("¿Desactivar modo edición? Los cambios no guardados se perderán.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SetValuesDep(dep_);
                    cbEditMode.Background = System.Windows.Media.Brushes.DarkRed;

                    editMode = false;

                }
                else
                {
                    editMode = true;
                }
            }
            EditModeControls(editMode);



        }

        private void EditModeControls(bool checkEdit)
        {


            tbDireccion.IsReadOnly = !checkEdit;
            TbComuna.IsReadOnly = true;
            TDescripcion.IsReadOnly = !checkEdit;

            tbDireccion.IsEnabled = checkEdit;
            TTitulo.IsEnabled = checkEdit;
            ImBig.IsEnabled = checkEdit;
            CbTarifa.IsEnabled = checkEdit;
            TbComuna.IsEnabled = checkEdit;
            TDescripcion.IsEnabled = checkEdit;
            if (checkEdit)
            {
                btnSaveChanges.Visibility = Visibility.Visible;
                Ccalendario.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Hidden;
                SpMiscControls.Visibility = Visibility.Hidden;

            }
            else
            {
                btnSaveChanges.Visibility = Visibility.Visible;
                Ccalendario.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                SpMiscControls.Visibility = Visibility.Visible;




            }

        }

        private void TbComuna_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (editMode)
            {
                ComunaWindow cw = new(editMode, TbComuna.Text);
                cw.ShowDialog();

            }
            else
            {
                ComunaWindow cw = new(editMode, null);
                cw.ShowDialog();
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            bool isModified = false;
            if (editMode)
            {

                //EDIT MODE SAVE
                if (MessageBox.Show("¿Confirmar y enviar los cambios? Esta acción no puede ser revertida.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {

                    int currency = int.Parse(CbTarifa.Text, NumberStyles.Currency);

                    //if (fileName == null)
                    //{
                    //    var bitmap = (BitmapImage)ImBig;

                    //    su.Save(bitmap, Path.Join(Environment.CurrentDirectory, "currentimage"));
                    //}

                    if (ImageChanged)
                    {
                        
                        isModified = NegocioComun.UpdateApartment(dep_.IdDepartamento, currency.ToString(), NegocioComun.GetIdComunaByName(TbComuna.Text).ToString(), tbDireccion.Text, TDescripcion.Text, su.ImagePathToBytes(fileName), TTitulo.Text);

                    }
                    else
                    {
                        isModified = NegocioComun.UpdateApartmentWithoutIM(dep_.IdDepartamento, currency.ToString(), NegocioComun.GetIdComunaByName(TbComuna.Text).ToString(), tbDireccion.Text, TDescripcion.Text, TTitulo.Text);
                    }

                    if (CreationValidate())
                    {
                        if (isModified)
                        {
                            MessageBox.Show("El departamento ha sido editado exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigationService.GoBack();

                        }
                        else
                        {


                            MessageBox.Show("Hubo un error al guardar los cambios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                        }
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido ingresar el departamento, hay campos vacíos, rellénelos e inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    

                   


                    




                }

            }
            else
            {
                if (MessageBox.Show("¿Crear el registro? Puede ser editado más tarde.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {

                    if (CreationValidate())
                    {
                        int currency = int.Parse(CbTarifa.Text, NumberStyles.Currency);

                        bool isAdded = NegocioComun.InsertApartment(currency.ToString(), NegocioComun.GetIdComunaByName(TbComuna.Text).ToString(), tbDireccion.Text, TDescripcion.Text, su.ImagePathToBytes(fileName), TTitulo.Text);


                        if (isAdded)
                        {
                            MessageBox.Show("El departamento ha sido ingresado exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigationService.GoBack();


                        }
                        else
                        {
                            MessageBox.Show("Hubo un error en la inserción.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido ingresar el departamento, hay campos vacíos, rellénelos e inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }




                }
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de querer eliminar el departamento? Este cambio no es reversible.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (NegocioComun.DeleteApartment(dep_.IdDepartamento.ToString()) && dgInventario.Items.Count == 0)
                {

                    MessageBox.Show("El departamento ha sido borrado con éxito.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();

                }
                else
                {
                    MessageBox.Show("No se ha podido eliminar el departamento, es posible que tenga algún registro asignado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }

        }

        private void btnGoDate_Click(object sender, RoutedEventArgs e)
        {
            Reserva reser = ((Button)sender).DataContext as Reserva;

            Ccalendario.DisplayDate = reser.FechaInicio;

            Ccalendario.SelectedDates.AddRange(reser.FechaInicio, reser.FechaFin);


        }

        public bool CreationValidate()
        {
            int EmptyFields = 0;

            if (TTitulo.Text == string.Empty)
            {
                EmptyFields++;
            }

            if (TDescripcion.Text == string.Empty)
            {
                EmptyFields++;

            }

            if (CbTarifa.SelectedIndex == -1)
            {
                EmptyFields++;

            }

            if (TbComuna.Text == string.Empty)
            {
                EmptyFields++;

            }

            if (tbDireccion.Text == string.Empty)
            {
                EmptyFields++;
            }

            if (fileName == string.Empty)
            {
                EmptyFields++;

            }

            if (EmptyFields > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
