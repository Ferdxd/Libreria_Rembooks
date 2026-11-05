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
    /// Lógica de interacción para VentasView.xaml
    /// </summary>
    public partial class VentasView : UserControl
    {
        private decimal TotalVenta = 0;
        DetalleVentaModelView detalle = new DetalleVentaModelView();
        ClienteService serviceCliente = new ClienteService();
        private int clienteId;
        DetalleVentaService _serviceDetalleVenta= new DetalleVentaService();
        VentaService _ventaService = new VentaService();
        ProductoService _productoService = new ProductoService();
        private decimal Cambio = 0;
        private decimal faltante=0;
        public VentasView()
        {
            InitializeComponent();
            txtMontoRecibido.IsEnabled = false;
            
        }

        private void btn_AñadirProducto_Click(object sender, RoutedEventArgs e)
        {
            var ventanaProducto = new BuscarProductoWindow();
            bool? resultado = ventanaProducto.ShowDialog();
            if (resultado==true)
            {
                detalle.Producto = ventanaProducto.productoObtenido;
                txtCodigoProducto.Text = detalle.Producto.Codigo;
                txtNombreProducto.Text = detalle.Producto.Nombre;
                txtPrecioProducto.Text = detalle.Producto.PrecioVenta.ToString();
            }
           
        }

        private void btnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
           var ventanaBuscarCliente=new BuscarClienteWindow();
            bool? resultado = ventanaBuscarCliente.ShowDialog();
            if (resultado==true)
            {
                clienteId = ventanaBuscarCliente.clienteSeleccionado.Id;
                txtNitCliente.Text = ventanaBuscarCliente.clienteSeleccionado.Nit;
                txtNombreCliente.Text = ventanaBuscarCliente.clienteSeleccionado.Nombre;
                txtTelefonoCliente.Text = ventanaBuscarCliente.clienteSeleccionado.Telefono;
                txtDireccionCliente.Text = ventanaBuscarCliente.clienteSeleccionado.Direccion;
            }
           
        }

        private async void btnCrearCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult resultado = MessageBox.Show("Desea Guardar Los Datos De El Cliente",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    await serviceCliente.crearCliente(txtNitCliente.Text, txtNombreCliente.Text, txtDireccionCliente.Text, txtTelefonoCliente.Text);
                    MessageBox.Show("El Cliente Fue Guardado Correctamente");
                }
                else
                {
                    MessageBox.Show("ok");
                }
            }
            catch
            {
                MessageBox.Show("ocurrio un error");
            }

        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            if (boton != null) boton.IsEnabled = false;
            try
            {
                if (clienteId <= 0) { MessageBox.Show("Seleccione un cliente registrado."); return; }
                if (dpFechaVenta.SelectedDate == null) { MessageBox.Show("Seleccione la fecha."); return; }
                if (!decimal.TryParse(txtMontoRecibido.Text, out decimal recibido)) { MessageBox.Show("Monto recibido inválido."); return; }
                var lineas = dgProductos.Items.OfType<DetalleVentaModelView>()
                    .Select(d => new LineaOperacion { ProductoId = d.Producto.Id, Cantidad = d.Cantidad, PrecioUnitario = d.Precio_Unitario }).ToList();
                int id = await new OperacionInventarioService().RegistrarVentaAsync(clienteId, dpFechaVenta.SelectedDate.Value.Date, recibido, lineas);
                MessageBox.Show($"Venta #{id} guardada correctamente.");
                dgProductos.Items.Clear();
                borrarCamposDetalle();
                recalcularTotal();
                txtMontoRecibido.Clear();
            }
            catch (Exception ex) { MessageBox.Show($"No se guardó la venta: {ex.Message}"); }
            finally { if (boton != null) boton.IsEnabled = true; }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void recalcularTotal()
        {
            TotalVenta = 0;
            foreach(var item in dgProductos.Items)
            {
                if(item is DetalleVentaModelView d)
                {
                    TotalVenta += d.SubTotalCalculado;
                }
            }
            txtTotalVenta.Text = TotalVenta.ToString("0.00");

            if (TotalVenta >0)
            {
                txtMontoRecibido.IsEnabled = true;
            }
            else
            {
                txtMontoRecibido.IsEnabled = false;
                txtMontoRecibido.Text="0.00";
                txb_Cambio.Text = "Cambio: Q0.00";
                txb_Cambio.Foreground = Brushes.Gray;
            }
            
        }

        private void btn_ConfirmarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (detalle == null)
                {
                    MessageBox.Show("Producto No Seleccionado");
                }
                else
                {
                    if (decimal.Parse(txtCantidadProducto.Text??"0.00") <= 0)
                    {
                        MessageBox.Show($"La Cantidad No Puede Ser {txtCantidadProducto.Text}");
                    }
                    else
                    {
                        if (decimal.Parse(txtPrecioProducto.Text??"0.00") <= 0)
                        {
                            MessageBox.Show($"El Precio No Puede Ser {txtPrecioProducto.Text}");
                        }
                        else
                        {
                            detalle.Cantidad = decimal.Parse(txtCantidadProducto.Text??"0.00");
                            detalle.Precio_Unitario = decimal.Parse(txtPrecioProducto.Text ?? "0.00");
                            dgProductos.Items.Add(detalle);
                            recalcularTotal();
                            borrarCamposDetalle();
                        }
                    }
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void borrarCamposDetalle()
        {
            txtPrecioProducto.Clear();
            txtNombreProducto.Clear();
            txtCantidadProducto.Clear();
            txtCodigoProducto.Clear();
            detalle = new DetalleVentaModelView();
            txtCodigoProducto.Focus();
        }

        private void txt_MontoRecibido_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularCambio();
        }


        private void CalcularCambio()
        {
            Cambio = 0;

            if (!decimal.TryParse(txtMontoRecibido.Text, out decimal montoRecibido) ||
                !decimal.TryParse(txtTotalVenta.Text, out decimal totalVenta))
            {
                txb_Cambio.Text = "Cambio: Q0.00";
                txb_Cambio.Foreground = Brushes.Gray;
                return;
            }

            if (montoRecibido < totalVenta)
            {
                faltante = totalVenta - montoRecibido;

                txb_Cambio.Text = $"Faltan: Q{faltante:0.00}";
                txb_Cambio.Foreground = Brushes.Red;
                return;
            }
          

            Cambio = montoRecibido - totalVenta;
            faltante = 0;
            txb_Cambio.Text = $"Cambio: Q{Cambio:0.00}";
            txb_Cambio.Foreground = Brushes.Green;
        }


        private void btn_EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            dgProductos.Items.Remove(dgProductos.SelectedItem);
            recalcularTotal();
        }

     
    }



    public class DetalleVentaModelView() : Detalle_Venta
    {
        public decimal SubTotalCalculado => Cantidad * Precio_Unitario;
    }
}
