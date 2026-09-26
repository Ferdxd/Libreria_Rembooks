using Libreria_Rembooks.Models;
using Libreria_Rembooks.Services;
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
using System.Windows.Shapes;

namespace Libreria_Rembooks.Views
{
    /// <summary>
    /// Lógica de interacción para BuscarProveedorWindow.xaml
    /// </summary>
    public partial class BuscarProveedorWindow : Window
    {
        public ProveedorService service = new ProveedorService();
        public Proveedor proveedor = new Proveedor();
        public BuscarProveedorWindow()
        {
            InitializeComponent();
            cargarProveedores();

        }
        public async void cargarProveedores()
        {
           dgProveedor.ItemsSource = await service.ObtenerTodas();
        }

        private async void btnBuscar(object sender, RoutedEventArgs e)
        {
            dgProveedor.ItemsSource = await service.buscarPorNombre(txtBuscar.Text);
            
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var productoSeleccionado = (Proveedor)dgProveedor.SelectedItem;
            if (productoSeleccionado != null)
            {
                proveedor.Id = productoSeleccionado.Id;
                proveedor.Nit = productoSeleccionado.Nit;
                proveedor.Razon_Social = productoSeleccionado.Razon_Social;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("No Ha Seleccionado Ningun Proveedor");
                return;
            }
           
            
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    
    }
}
