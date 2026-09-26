using Libreria_Rembooks.Data;
using Libreria_Rembooks.Models;
using Libreria_Rembooks.Views;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Libreria_Rembooks.Services;

namespace Libreria_Rembooks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

  

        private void Button_Categoria(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CategoriaView();
            txtBlock_Panel_Principal.Visibility = Visibility.Collapsed;
        }
        private void Button_Compra(object sender, RoutedEventArgs e)
        {
            
            MainContent.Content = new ComprasView();
            txtBlock_Panel_Principal.Visibility = Visibility.Collapsed;
            
        }
        private void Button_Proveedores(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProveedoresView();
            txtBlock_Panel_Principal.Visibility = Visibility.Collapsed;
        }

        private void Button_Ventas(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VentasView();
            txtBlock_Panel_Principal.Visibility=Visibility.Collapsed;
        }
        private void Button_Inventario(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new InventarioView();
            txtBlock_Panel_Principal.Visibility = Visibility.Collapsed;
        }
    }
}