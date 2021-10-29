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
using SkyrentConnect;
using SkyrentObjects;
using MaterialDesignColors;
using SkyrentObjects;

namespace DepartamentoApp.Apartment
{
    /// <summary>
    /// Lógica de interacción para ListApartmentPage.xaml
    /// </summary>
    public partial class ListApartmentPage : Page
    {

        CommonBusiness cbb = new();

        public ListApartmentPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListaDepartamentos.ItemsSource = cbb.GetDepartamentoList();
        }

        private void BtnDepartmentEnter_Click(object sender, RoutedEventArgs e)
        {
            var idDep = (((Button)sender).DataContext as Departamento).IdDepartamento;
            MessageBox.Show(idDep.ToString());
        }
    }
}