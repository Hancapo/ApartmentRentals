﻿using System;
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
using SkyrentBusiness.ApartmentBusiness;

namespace DepartamentoApp.Apartment
{
    /// <summary>
    /// Lógica de interacción para AddApartment.xaml
    /// </summary>
    public partial class AddApartment : Page
    {
        public AddApartment()
        {
            InitializeComponent();
        }

        private void familiaItemCB_Loaded(object sender, RoutedEventArgs e)
        {
            ApartmentBusiness auxAB = new();
            var auxLlenado = auxAB.ReturnFamiliaItems();
            familiaItemCB.ItemsSource = auxLlenado.Select(m => m.DESCRIPCION).ToList();
        }
    }
}
