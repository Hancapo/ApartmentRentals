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
        bool EditLoad;
        bool InventoryHasChanged;
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
            tbBanos.Text = dd.Banos.ToString();
            tbCapacidad.Text = dd.Capacidad.ToString();
            tbDorms.Text = dd.Dormitorio.ToString();    
            tbDireccion.Text = dd.DireccionDep;
            ImBig.Source = dd.FotoBig;
            lbComuna.Content = dd.ComunaDep;
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
                        
                        isModified = NegocioComun.UpdateApartment(dep_.IdDepartamento, currency.ToString(), NegocioComun.GetIdComunaByName(lbComuna.Content.ToString()).ToString(), tbDireccion.Text, TDescripcion.Text, su.ImagePathToBytes(fileName), TTitulo.Text
                            , Convert.ToInt32(tbBanos.Text), Convert.ToInt32(tbCapacidad.Text), cbEstado.SelectedIndex -1, Convert.ToInt32(tbDorms.Text));

                    }
                    else
                    {
                        isModified = NegocioComun.UpdateApartmentWithoutIM(dep_.IdDepartamento, currency.ToString(), NegocioComun.GetIdComunaByName(lbComuna.Content.ToString()).ToString(), tbDireccion.Text, TDescripcion.Text, TTitulo.Text
                            ,Convert.ToInt32(tbBanos.Text), Convert.ToInt32(tbCapacidad.Text), cbEstado.SelectedIndex - 1, Convert.ToInt32(tbDorms.Text));
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

                        Departamento d = new()
                        {
                            IdDepartamento = NegocioComun.CalculateID("IDDEPARTAMENTO", "DEPARTAMENTO"),
                            Banos = Convert.ToInt32(tbBanos.Text),
                            Capacidad = Convert.ToInt32(tbCapacidad.Text),
                            IdComunaDep = NegocioComun.GetIdComunaByName(TbComuna.Content.ToString()),
                            DescripcionDep = TDescripcion.Text,
                            IdTarifaDep = NegocioComun.GetTarifaIdFromTarifaPrice(currency),
                            TituloDepartamento = TTitulo.Text,
                            Estado = cbEstado.SelectedIndex + 1,
                            Dormitorio = Convert.ToInt32(tbDorms.Text),
                            DireccionDep = tbDireccion.Text

                            
                        };


                        bool isAdded = NegocioComun.NewInsertApartment(d, su.ImagePathToBytes(fileName));


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

            if (lbComuna.Content.ToString() == "Comuna")
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

            if (cbEstado.SelectedIndex == -1)
            {
                EmptyFields++;

            }

            if (tbCapacidad.Text == string.Empty)
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
            if (lbInventory.Items.Count == 0)
            {
                Inventario inv = new() { IdInventario = NegocioComun.CalculateID("IDINVENTARIO", "INVENTARIO"), IdDepartamento = dep_.IdDepartamento, fechaCreacion = DateTime.Now, Descripcion = null, Items = new() };
                inventories.Add(inv);
                lbInventory.Items.Add(inv); 
                lbInventory.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Solo puede existir un solo inventario por departamento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

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


                if (SelectedInvCasted.Items != null)
                {
                    foreach (var item in SelectedInvCasted.Items)
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
                                InventoryHasChanged = true;

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
                                InventoryHasChanged = true;

                            }
                        }
                        else
                        {
                            lbItems2.Items.Add(new Item {  Descripcion = ItemAsItem.Descripcion, Cantidad = 1, IdItem = ItemAsItem.IdItem });
                            InventoryHasChanged = true;

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un Item del primer listado para transferir", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                    InventoryHasChanged = true;

                }
                else
                {
                    MessageBox.Show("Seleccione un Item del segundo listado para transferir", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                InventoryHasChanged = true;

                lbItems1.Items.Refresh();
                lbItems2.Items.Refresh();
            }
            else
            {
                MessageBox.Show("No se ha podido transferir todos los elementos, el segundo listado está vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnSaveInventory_Click(object sender, RoutedEventArgs e)
        {

            if (lbInventory.SelectedItem != null)
            {


                var SelectedInv = lbInventory.SelectedItem;

                var SelectedInvCasted = (Inventario)SelectedInv;

                var SavedItemList = lbItems2.Items.Cast<Item>().ToList();


                //for (int i = 0; i < lbInventory.Items.Count; i++)
                //{
                //    if (lbInventory.Items[i].cas)
                //    {

                //    }
                //}

                SelectedInvCasted.Items = SavedItemList;

                InsertInvDep(SelectedInvCasted);




                gridDetail.IsEnabled = true;
            }

            lbItems2.Items.Clear();

            lbInventory.SelectedIndex = -1;


        }

        private void InsertInvDep(Inventario inv)
        {
            
            int InvCount = NegocioComun.GetInventoryCountFromDepId(dep_.IdDepartamento);

            if (InvCount > 1)
            {
                MessageBox.Show("No puede existir más de un inventario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                Inventario invv = new() {Descripcion = inv.Descripcion, fechaCreacion = DateTime.Now, IdInventario = inv.IdInventario, IdDepartamento = dep_.IdDepartamento, Items = inv.Items };

                //Crear registro en Tabla Inventario

                NegocioComun.CreateInventory(invv);

                //Crear registro en Tabla Detalle_Inventario

                foreach (Item item in invv.Items)
                {
                    DetalleInventario di = new() { Cantidad = item.Cantidad, IdDetalleInventario = NegocioComun.CalculateID("IDDETALLE_INVENTARIO", "DETALLE_INVENTARIO"), INVENTARIO_IDINVENTARIO = invv.IdInventario, Item_IdItem = item.IdItem };

                    NegocioComun.CreateDetailInventory(di);

                }

                MessageBox.Show("Se ha guardado el inventario correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);




            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (tbSearch.Text.Length > 0)
            {
                var NameSearch = tbSearch.Text.ToLowerInvariant();
                NegocioComun.GetItemList().Where(x => (x.Descripcion.ToLowerInvariant().Contains(NameSearch)));
            }
            else
            {
                NegocioComun.GetItemList();
            }

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
            lbComuna.Content = cbComuna.Text;
            TbComuna.IsPopupOpen = false;
        }

        private void btnCerrarComuna_Click(object sender, RoutedEventArgs e)
        {
            TbComuna.IsPopupOpen = false;
        }

        private void popComuna_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void lbInventory_Loaded(object sender, RoutedEventArgs e)
        {
            if (!creationMode)
            {
                foreach (var item in NegocioComun.GetInventarioFromDepId(dep_.IdDepartamento))
                {
                    lbInventory.Items.Add(item);
                }
            }
            


            

        }

        private void btnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TbComuna_Opened(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                string comuna = lbComuna.Content.ToString();
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

        //**********************************************************


    }
}
