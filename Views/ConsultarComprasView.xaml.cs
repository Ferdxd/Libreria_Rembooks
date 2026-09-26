using Libreria_Rembooks.Models;
using Libreria_Rembooks.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Libreria_Rembooks.Views
{
    /// <summary>
    /// Lógica de interacción para ConsultarComprasView.xaml
    /// </summary>
    public partial class ConsultarComprasView : UserControl
    {
        CompraService comprasService = new CompraService();
        DetalleCompraService detalle_Compra = new DetalleCompraService();
        int proveedorId = 0;
        public ConsultarComprasView()
        {
            InitializeComponent();
            cargarDatos();
        }
        private async void cargarDatos()
        {
            dgCompras.ItemsSource = await comprasService.ObtenerComprasConProveedor();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;
            if(window != null) 
                window.MainContent.Content = new ComprasView();

        }
        /*private void btnRegresar_Click(object sender, RoutedEventArgs e)
{
    ((MainWindow)Application.Current.MainWindow)
        .Navegar(new ComprasView());
}*/

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (dp_Desde.SelectedDate == null || dp_Hasta.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar fecha desde y fecha hasta.");
                return;
            }
            dgCompras.ItemsSource=await comprasService.obtenerCompraxFecha(dp_Desde.SelectedDate.Value, dp_Hasta.SelectedDate.Value, proveedorId);
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtProveedor.Text = string.Empty;
            dp_Desde.Text= string.Empty;
            dp_Hasta.Text = string.Empty;
            cargarDatos();
        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
           
            var compra = (Compra)dgCompras.SelectedItem;
            if (dgCompras.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una compra para ver el detalle.");
                return;
            }
            var ventana = new VerDetalleCompraWindow(compra.Id);
            bool? resultado = ventana.ShowDialog();
        }

        private void btnBuscarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new BuscarProveedorWindow();
            bool? resultado = ventana.ShowDialog();

            if (resultado == true)
            {
                txtProveedor.Text = ventana.proveedor.Razon_Social.ToString();
                proveedorId = ventana.proveedor.Id;

            }
        }
    }
}
