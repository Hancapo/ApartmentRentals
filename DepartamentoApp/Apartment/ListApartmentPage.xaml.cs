﻿using System.Windows;
using System.Windows.Controls;
using SkyrentBusiness;
using SkyrentObjects;

namespace DepartamentoApp.Apartment
{
    /// <summary>
    /// Lógica de interacción para ListApartmentPage.xaml
    /// </summary>
    public partial class ListApartmentPage : Page
    {
        readonly SkyUtilities su = new();


        public ListApartmentPage() 
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListaDepartamentos.ItemsSource = su.GetDepartamentoList();
        }

        private void BtnDepartmentEnter_Click(object sender, RoutedEventArgs e)
        {
            //int idDep = (((Button)sender).DataContext as Departamento).IdDepartamento;
            //MessageBox.Show(idDep.ToString());

            Departamento depa = ((Button)sender).DataContext as Departamento;

            ApartmentView av = new(depa);
            NavigationService.Navigate(av);

            
        }
    }
}
