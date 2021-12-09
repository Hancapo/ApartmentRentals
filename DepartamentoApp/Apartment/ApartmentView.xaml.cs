﻿using SkyrentObjects;
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
        bool EditLoad;
        List<Inventario> inventories = new();


        public ApartmentView(Departamento dep, bool CreationMode)
        {
            dep_ = dep;
            creationMode = CreationMode;
            InitializeComponent();
            if (creationMode)
            {
                cbEstado.ItemsSource = new List<string> { "En uso", "En mantención", "Disponible", "Deshabilitado" };

                GroupBookings.Visibility = Visibility.Hidden;
                GroupInventario.Visibility = Visibility.Hidden; 
                //Visibility settings
                //Ccalendario.Visibility = Visibility.Hidden;
                //lbCalend.Visibility = Visibility.Hidden;
                cbEditMode.Visibility = Visibility.Hidden;
                //SpMiscControls.Visibility = Visibility.Hidden;
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
                //lbTextoCambios.Content = "Agregar"; SE TIENE QUE CREAR

                TTitulo.Text = String.Empty;
                TDescripcion.Text = String.Empty;
                TbComuna.Content = "Seleccionar Comuna";
                tbDireccion.Text = String.Empty;
                CbTarifa.SelectedItem = null;
                CbTarifa.ItemsSource = NegocioComun.GetTarifaList().OrderBy(x => x.Monto_Noche).Select(x => x.Monto_Noche).ToList();

                //Misc settings
                ImBig.Source = new BitmapImage(new Uri(@"/TurismoReal;component/Apartment/emptyimage.jpg", UriKind.Relative));

            }
            else
            {
                cbEstado.ItemsSource = new List<string> { "En uso", "En mantención", "Disponible", "Deshabilitado" };

                SetValuesDep(dep_);
                EditModeControls(editMode);
                //dgInventario.ItemsSource = NegocioComun.GetFamiliaListByDepartamentoID(dep_.IdDepartamento).Select(i => new {i.Descripcion, i.Cantidad}).ToList();
                //xListReservas.ItemsSource = NegocioComun.GetReservaListByDepID(dep_.IdDepartamento);
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
            TbComuna.Content = dd.ComunaDep;
            TTitulo.Text = dd.TituloDepartamento;
            CbTarifa.SelectedIndex = Convert.ToInt32(dd.IdTarifaDep) - 1;
            cbEstado.SelectedIndex = Convert.ToInt32(dd.Estado) - 1;
            CbTarifa.ItemsSource = NegocioComun.GetTarifaList().OrderBy(x => x.Monto_Noche).Select(x => x.Monto_Noche).ToList();
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
            TbComuna.IsEnabled = true;
            TDescripcion.IsReadOnly = !checkEdit;

            tbDireccion.IsEnabled = checkEdit;
            TTitulo.IsEnabled = checkEdit;
            ImBig.IsEnabled = checkEdit;
            CbTarifa.IsEnabled = checkEdit;
            TbComuna.IsEnabled = checkEdit;
            cbEstado.IsEnabled = checkEdit;
            TDescripcion.IsEnabled = checkEdit;
            GroupBookings.IsEnabled = checkEdit;    
            GroupInventario.IsEnabled = checkEdit;  
            
            if (checkEdit)
            {
                btnSaveChanges.Visibility = Visibility.Visible;
                //Ccalendario.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Hidden;
                //SpMiscControls.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSaveChanges.Visibility = Visibility.Hidden;
                //Ccalendario.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                //SpMiscControls.Visibility = Visibility.Visible;
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
                        
                        isModified = NegocioComun.UpdateApartment(dep_.IdDepartamento, currency.ToString(), NegocioComun.GetIdComunaByName(TbComuna.Content.ToString()).ToString(), tbDireccion.Text, TDescripcion.Text, su.ImagePathToBytes(fileName), TTitulo.Text);

                    }
                    else
                    {
                        isModified = NegocioComun.UpdateApartmentWithoutIM(dep_.IdDepartamento, currency.ToString(), NegocioComun.GetIdComunaByName(TbComuna.ToString()).ToString(), tbDireccion.Text, TDescripcion.Text, TTitulo.Text);
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

                        bool isAdded = NegocioComun.InsertApartment(currency.ToString(), NegocioComun.GetIdComunaByName(TbComuna.Content.ToString()).ToString(), tbDireccion.Text, TDescripcion.Text, su.ImagePathToBytes(fileName), TTitulo.Text);


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
                if (NegocioComun.DeleteApartment(dep_.IdDepartamento.ToString()) && lbInventory.Items.Count == 0)
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

        //private void btnGoDate_Click(object sender, RoutedEventArgs e)
        //{
        //    Reserva reser = ((Button)sender).DataContext as Reserva;

        //    Ccalendario.DisplayDate = reser.FechaInicio;

        //    Ccalendario.SelectedDates.AddRange(reser.FechaInicio, reser.FechaFin);


        //}

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

            if (TbComuna.Content.ToString() == string.Empty)
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

            if (tbDorms.Text == string.Empty)
            {
                EmptyFields++;

            }

            if (tbBanos.Text == string.Empty)
            {
                EmptyFields++;

            }

            if (cbEstado.SelectedIndex != -1)
            {
                EmptyFields++;

            }

            if (tbCapacidad.Text == String.Empty)
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

        //************************INVENTARIO***************************************
        private void lbItems1_Loaded(object sender, RoutedEventArgs e)
        {
            lbItems1.ItemsSource = NegocioComun.GetItemList();
        }


        private void btnCreateInv_Click(object sender, RoutedEventArgs e) //******************************************************************OBTENER ID DE DEPTO
        {
            Inventario inv = new Inventario() { IdInventario = NegocioComun.CalculateID("Inventario", "IDINVENTARIO"), IdDepartamento= dep_.IdDepartamento, fechaCreacion = DateTime.Now, Descripcion = null };
            inventories.Add(inv);
            lbInventory.Items.Refresh();
        }

        private void btnDeleteInv_Click(object sender, RoutedEventArgs e)
        {
            inventories.Remove((Inventario)lbInventory.SelectedItem);
            lbInventory.Items.Refresh();
        }

        private void lbInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbInventory.SelectedItem != null)
            {
                lbItems2.Items.Clear();


                var SelectedInv = lbInventory.SelectedItem;

                var SelectedInvCasted = (Inventario)SelectedInv;


                if (SelectedInvCasted.items.Count != 0)
                {
                    foreach (var item in SelectedInvCasted.items)
                    {
                        lbItems2.Items.Add(item);
                    }
                }

                gridDetail.IsEnabled = true;
            }

        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (lbItems1.Items.Count == 0)
            {
                MessageBox.Show("El primer listado está vacío, no es posible realizar ninguna operación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                if (lbItems1.SelectedItem != null)
                {
                    var ItemToTransfer = lbItems1.SelectedItem;
                    var ItemAsItem = (Item)ItemToTransfer;
                    if (ItemAsItem.Cantidad > 0)
                    {
                        ItemAsItem.Cantidad--;
                        lbItems1.Items.Refresh();
                        if (lbItems2.Items.Count != 0)
                        {
                            bool ItemExistsInList2 = lbItems2.Items.Cast<Item>().Any(x => x.Descripcion == ItemAsItem.Descripcion);
                            if (!ItemExistsInList2)
                            {
                                lbItems2.Items.Add(new Item { Descripcion = ItemAsItem.Descripcion, Cantidad = 1 });
                            }
                            else
                            {
                                for (int i = 0; i < lbItems2.Items.Count; i++)
                                {
                                    if (((Item)lbItems2.Items[i]).Descripcion == ItemAsItem.Descripcion)
                                    {
                                        ((Item)lbItems2.Items[i]).Cantidad++;
                                        lbItems2.Items.Refresh();
                                    }
                                }
                            }
                        }
                        else
                        {
                            lbItems2.Items.Add(new Item { Descripcion = ItemAsItem.Descripcion, Cantidad = 1 });
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una fruta del primer listado para transferir", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            if (lbItems2.Items.Count == 0)
            {
                MessageBox.Show("El segundo listado está vacío, no es posible realizar ninguna operación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                if (lbItems2.SelectedItem != null)
                {
                    var ItemToSubstract = lbItems2.SelectedItem;
                    var ItemAsItem = (Item)ItemToSubstract;
                    if (ItemAsItem.Cantidad > 0)
                    {
                        ItemAsItem.Cantidad--;
                        lbItems2.Items.Refresh();
                        for (int i = 0; i < lbItems1.Items.Count; i++)
                        {
                            if (((Item)lbItems1.Items[i]).Descripcion == ItemAsItem.Descripcion)
                            {
                                ((Item)lbItems1.Items[i]).Cantidad++;
                                lbItems1.Items.Refresh();
                            }
                        }
                        if (ItemAsItem.Cantidad == 0)
                        {
                            lbItems2.Items.Remove(lbItems2.SelectedItem);
                            lbItems2.Items.Refresh();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una fruta del segundo listado para transferir", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnSubstractAll_Click(object sender, RoutedEventArgs e)
        {
            if (lbItems2.Items.Count != 0)
            {
                for (int i = 0; i < lbItems1.Items.Count; i++)
                {
                    Item f1 = (Item)lbItems1.Items[i];
                    for (int i1 = 0; i1 < lbItems2.Items.Count; i1++)
                    {
                        Item f2 = (Item)lbItems2.Items[i1];
                        if (f1.Descripcion == f2.Descripcion)
                        {
                            f1.Cantidad += f2.Cantidad;

                            lbItems2.Items.Remove(f2);
                        }
                    }
                }

                lbItems1.Items.Refresh();
                lbItems2.Items.Refresh();
            }
            else
            {
                MessageBox.Show("No se puede transferir todos los elementos, el segundo listado está vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnSaveInventory_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (lbInventory.SelectedItem != null)
            {
                lbItems2.Items.Clear();


                var SelectedInv = lbInventory.SelectedItem;

                var SelectedInvCasted = (Inventario)SelectedInv;

                //lbFrutas2.Items.Clear();

                if (SelectedInvCasted.fruits.Count != 0)
                {
                    foreach (var item in SelectedInvCasted.fruits)
                    {
                        lbItems2.Items.Add(item);
                    }
                }
                gridDetail.IsEnabled = true;
            }
            */
        } //*******************************************MANEJAR ARRAY 

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
            if (tbSearch.Text.Length > 0)
            {
                var amogus = tbSearch.Text.ToLowerInvariant();
                lbFrutas1.ItemsSource = StaticFruitList.Where(x => (x.FruitName.ToLowerInvariant().Contains(amogus)));
            }
            else
            {
                lbFrutas1.ItemsSource = StaticFruitList;
            }
            */
        }  //****************************************MANEJAR ARRAY

        private void btnGuardarInv_Click(object sender, RoutedEventArgs e) //***************************************************TERMINAR
        {

        }

        //**************************************************************************



        //***************COMUNA**********************************

        private void cbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editMode && EditLoad)
            {
                string RegionCode = NegocioComun.GetRegionList()[cbRegion.SelectedIndex].IdRegion;
                cbComuna.ItemsSource = null;
                cbCiudad.ItemsSource = null;
                cbCiudad.SelectedIndex = -1;
                cbComuna.SelectedIndex = -1;
                cbCiudad.ItemsSource = NegocioComun.GetCiudadListFromRegion(RegionCode).Select(x => x.NameCiudad);

            }
            else
            {
                string RegionCode = NegocioComun.GetRegionList()[cbRegion.SelectedIndex].IdRegion;

                cbCiudad.IsEnabled = true;
                cbCiudad.ItemsSource = NegocioComun.GetCiudadListFromRegion(RegionCode).Select(x => x.NameCiudad);
            }

        }

        private void cbCiudad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (editMode && cbCiudad.SelectedIndex != -1)
            {
                string NombreCiudad = (sender as System.Windows.Controls.ComboBox).SelectedItem as string;
                cbComuna.ItemsSource = NegocioComun.GetComunaListFromCiudad(NombreCiudad).Select(x => x.NombreComuna);
            }

            if ((editMode && cbCiudad.SelectedIndex == -1) && cbRegion.SelectedIndex != -1)
            {
                string RegionCode = NegocioComun.GetRegionList()[cbRegion.SelectedIndex].IdRegion;
                cbCiudad.ItemsSource = NegocioComun.GetCiudadListFromRegion(RegionCode).Select(x => x.NameCiudad);
            }

            if (!editMode)
            {
                cbComuna.IsEnabled = true;
                string ciudad = (sender as System.Windows.Controls.ComboBox).SelectedItem as string;
                cbComuna.ItemsSource = NegocioComun.GetComunaListFromCiudad(ciudad).Select(x => x.NombreComuna);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            TbComuna.Content = cbComuna.Text;
            popComuna.IsPopupOpen = false;
        }

        private void btnCerrarComuna_Click(object sender, RoutedEventArgs e)
        {
            popComuna.IsPopupOpen = false;
        }

        private void popComuna_Loaded(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                string comuna = TbComuna.Content.ToString();
                //Comuna List
                string CiudadFromComuna = NegocioComun.GetCiudadByComuna(comuna);
                cbComuna.ItemsSource = NegocioComun.GetComunaListFromCiudad(CiudadFromComuna).Select(x => x.NombreComuna);

                //Selected Comuna
                cbComuna.SelectedIndex = cbComuna.Items.IndexOf(comuna);

                //Ciudad List
                string RegionIdFromCiudad = NegocioComun.GetRegionIdByCiudad(CiudadFromComuna);
                cbCiudad.ItemsSource = NegocioComun.GetCiudadListFromRegion(RegionIdFromCiudad).Select(x => x.NameCiudad);

                //Selected Ciudad
                cbCiudad.SelectedIndex = cbCiudad.Items.IndexOf(CiudadFromComuna);


                string RegionNombreFromCiudad = NegocioComun.GetRegionByCiudad(CiudadFromComuna);


                //Region List
                cbRegion.ItemsSource = NegocioComun.GetRegionList().Select(x => x.Nombre);
                //Selected Region
                cbRegion.SelectedIndex = cbRegion.Items.IndexOf(RegionNombreFromCiudad);

                EditLoad = true;


            }
            else
            {
                cbCiudad.IsEnabled = false;
                cbComuna.IsEnabled = false;

                cbRegion.ItemsSource = NegocioComun.GetRegionList().Select(x => x.Nombre);
            }
        }

        private void lbInventory_Loaded(object sender, RoutedEventArgs e)
        {
            lbInventory.ItemsSource = NegocioComun.GetInventarioFromDepId(dep_.IdDepartamento);

            

        }

        //**********************************************************


    }
}
