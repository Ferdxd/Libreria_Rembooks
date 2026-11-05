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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Libreria_Rembooks.Views
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class ComprasView : UserControl
    {
        public decimal TotalCompra = 0;
        public string id_Proveedor;
        DetalleCompraService detalleCompraService = new DetalleCompraService();
        CompraService compraService = new CompraService();
        ProductoService productoService = new ProductoService();
        DetalleCompraModelView detalle=new DetalleCompraModelView();
     
        
        public ComprasView()
        {
            InitializeComponent();
            txtTotalCompra.Text = TotalCompra.ToString();
            dpFechaIngreso.SelectedDate = DateTime.Now;
            cbTipoDoc.Items.Add("Factura");
            txtCantidadProducto.Text = "0.00";
            txtPrecioProducto.Text = "0.00";
        }

 
        private void btn_AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new BuscarProductoWindow();
            bool? resultado = ventana.ShowDialog();
            if (resultado.Value)
            {
                detalle.Producto_Id = ventana.productoObtenido.Id;
               detalle.Producto = ventana.productoObtenido;
                txtCodigoProducto.Text = detalle.Producto.Codigo;
                txtNombreProducto.Text = detalle.Producto.Nombre;
                txtPrecioProducto.Text = detalle.Producto.PrecioCompra.ToString();
               
            }

        }

        private void btn_CrearProducto_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new CrearProductoWindow();
            bool? resultado = ventana.ShowDialog();
        }

        private void btn_BuscarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new BuscarProveedorWindow();
            bool? resultado = ventana.ShowDialog();
            
            if (resultado==true)
            {
                txtNit.Text = ventana.proveedor.Nit.ToString();
                txtNombreProveedor.Text = ventana.proveedor.Razon_Social.ToString();
                id_Proveedor = ventana.proveedor.Id.ToString();
            }


        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            borrarCampos();
        }

        private void borrarCampos()
        {
            
            txtNit.Clear();
            TotalCompra = 0;
            txtTotalCompra.Text= TotalCompra.ToString();
            txtNoDocumento.Clear();
            dgListaProductos.Items.Clear();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            if (boton != null) boton.IsEnabled = false;
            try
            {
                if (!int.TryParse(id_Proveedor, out int proveedorId) || proveedorId <= 0) { MessageBox.Show("Seleccione un proveedor."); return; }
                if (dpDate.SelectedDate == null || dpFechaIngreso.SelectedDate == null) { MessageBox.Show("Seleccione ambas fechas."); return; }
                var lineas = dgListaProductos.Items.OfType<DetalleCompraModelView>()
                    .Select(d => new LineaOperacion { ProductoId = d.Producto_Id, Cantidad = d.Cantidad, PrecioUnitario = d.Precio_Unitario }).ToList();
                int id = await new OperacionInventarioService().RegistrarCompraAsync(
                    proveedorId, dpDate.SelectedDate.Value.Date, dpFechaIngreso.SelectedDate.Value.Date,
                    txtSerie.Text, txtNoDocumento.Text, cbTipoDoc.Text, lineas);
                MessageBox.Show($"Compra #{id} guardada correctamente.");
                borrarCampos();
            }
            catch (Exception ex) { MessageBox.Show($"No se guardó la compra: {ex.Message}"); }
            finally { if (boton != null) boton.IsEnabled = true; }
        }

        private void btnConsultarCompras_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;

            if (window != null)
            {
                window.MainContent.Content = new ConsultarComprasView();
            }
        }

        private void btn_EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
           var producto = dgListaProductos.SelectedItem as DetalleCompraModelView;
            dgListaProductos.Items.Remove(dgListaProductos.SelectedItem);
            RecalcularTotal();
        }
        
        private void btn_CambiarProducto_Click(object sender, RoutedEventArgs e)
        {

            if (sender is not Button boton ||
            boton.DataContext is not DetalleCompraModelView productoActual)
            {
                MessageBox.Show(
                    "No se pudo identificar el producto seleccionado.",
                    "Cambiar producto",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var ventana = new BuscarProductoWindow();

            bool? resultado = ventana.ShowDialog();

            if (resultado != true || ventana.productoObtenido == null)
            {
                return;
            }

            Producto nuevoProducto = ventana.productoObtenido;

            productoActual.Producto_Id = nuevoProducto.Id;
            productoActual.Producto = nuevoProducto;
            productoActual.Precio_Unitario =
                nuevoProducto.PrecioCompra ?? 0;

            dgListaProductos.Items.Refresh();

            RecalcularTotal();
        }

        
        private void RecalcularTotal()
        {
            TotalCompra = 0;

            foreach (var item in dgListaProductos.Items)
            {
                if (item is DetalleCompraModelView detalle)
                {
                    TotalCompra += detalle.SubtotalCalculado;
                }
            }

            txtTotalCompra.Text = TotalCompra.ToString("0.00");
        }

        private void btn_ConfirmarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (detalle.Producto == null)
                {
                    MessageBox.Show("Producto no seleccionado");
                }
                else
                {
                    if (decimal.Parse(txtCantidadProducto.Text ?? "0.00") <= 0)
                    {
                        MessageBox.Show($"La Cantidad No Puede Ser {txtCantidadProducto.Text}");
                    }
                    else
                    {
                        if (decimal.Parse(txtPrecioProducto.Text ?? "0.00") <= 0)
                        {
                            MessageBox.Show($"El Precio De Compra No Puede Ser {txtPrecioProducto.Text}");
                        }
                        else
                        {
                            detalle.Cantidad = decimal.Parse(txtCantidadProducto.Text ?? "0.00");
                            detalle.Precio_Unitario = decimal.Parse(txtPrecioProducto.Text ?? "0.00");
                            dgListaProductos.Items.Add(detalle);
                            RecalcularTotal();
                            borrarCamposDetalle();
                        }

                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error:{ex.Message}");
            }
            
            
        }

        private void borrarCamposDetalle()
        {
            txtPrecioProducto.Clear();
            txtNombreProducto.Clear();
            txtCantidadProducto.Clear();
            txtCodigoProducto.Clear();
            detalle = new DetalleCompraModelView();
            txtCodigoProducto.Focus();
        }
    }
    public class DetalleCompraModelView() : Detalle_Compra
    {
        public decimal SubtotalCalculado => Cantidad * Precio_Unitario;
    }
    
}
