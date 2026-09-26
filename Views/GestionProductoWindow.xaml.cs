using Libreria_Rembooks.Models;
using Libreria_Rembooks.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Libreria_Rembooks.Views
{
    public partial class GestionProductoWindow : Window
    {
        private readonly ProductoService productoService = new ProductoService();
        private readonly CategoriaService categoriaService = new CategoriaService();

        private bool bandera;
        private Producto? productoSeleccionado;

        public GestionProductoWindow(int id)
        {
            InitializeComponent();

            Loaded += async (_, _) => await InicializarVentana(id);
            CalcularDesdePrecioVenta();
        }

        private async Task InicializarVentana(int id)
        {
            try
            {
                bandera = false;

                await CargarCategorias();
                await CargarProducto(id);

                CalcularDesdePrecioVenta();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No fue posible cargar el producto.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                bandera = true;
            }
        }

        private async Task CargarProducto(int id)
        {
            productoSeleccionado = await productoService.ObtenerPorId(id);

            if (productoSeleccionado == null)
            {
                throw new InvalidOperationException(
                    "No se encontró el producto seleccionado.");
            }

            txtCodigo.Text = productoSeleccionado.Codigo ?? string.Empty;
            txtDescripcion.Text = productoSeleccionado.Descripcion ?? string.Empty;
            txtNombre.Text = productoSeleccionado.Nombre ?? string.Empty;
            txtMarca.Text = productoSeleccionado.Marca ?? string.Empty;
            txtResumenExistencia.Text = productoSeleccionado.Stock.ToString();
            txtUnidadMedida.Text = productoSeleccionado.UnidadMedida ?? string.Empty;

            decimal precioCompra = productoSeleccionado.PrecioCompra ?? 0m;

            decimal precioPromedio = productoSeleccionado.PrecioPromedio ?? 0m;

            decimal precioVenta = productoSeleccionado.PrecioVenta ?? 0m;

            txtUltimaCompra.Text = precioCompra.ToString("0.00");

            txtCostoPromedio.Text = precioPromedio.ToString("0.00");

            if (precioVenta <= 0)
            {
                precioVenta = precioPromedio > 0
                    ? precioPromedio
                    : precioCompra;
            }

            txtPrecioVenta.Text = precioVenta.ToString("0.00");

            cbCategoria.SelectedValue = productoSeleccionado.CategoriaId;
        }
        private async Task CargarCategorias()
        {
            cbCategoria.ItemsSource = await categoriaService.ObtenerTodas();
            cbCategoria.DisplayMemberPath = "Nombre";
            cbCategoria.SelectedValuePath = "Id";
        }

        private void CalcularDesdePorcentaje()
        {
            if (!decimal.TryParse(
                    txtCostoPromedio.Text,
                    out decimal precioPromedio) ||
                !decimal.TryParse(
                    txtPorcentajeAumento.Text,
                    out decimal porcentaje))
            {
                txtPrecioVenta.Text = "0.00";
                txtGananciaEstimada.Text = "0.00";
                return;
            }

            decimal ganancia =
                precioPromedio * (porcentaje / 100);

            decimal precioVenta =
                precioPromedio + ganancia;

            txtPrecioVenta.Text =
                precioVenta.ToString("0.00");

            txtGananciaEstimada.Text =
                ganancia.ToString("0.00");
        }

        private void CalcularDesdePrecioVenta()
        {
            if (!decimal.TryParse(
                    txtCostoPromedio.Text,
                    out decimal precioPromedio) ||
                !decimal.TryParse(
                    txtPrecioVenta.Text,
                    out decimal precioVenta) ||
                precioPromedio <= 0)
            {
                txtPorcentajeAumento.Text = "0.00";
                txtGananciaEstimada.Text = "0.00";
                return;
            }

            decimal ganancia =
                precioVenta - precioPromedio;

            decimal porcentaje =
                (ganancia / precioPromedio) * 100;

            txtPorcentajeAumento.Text =
                porcentaje.ToString("0.00");

            txtGananciaEstimada.Text =
                ganancia.ToString("0.00");
        }

        private void txtPrecioVenta_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (!bandera)
                return;

            bandera = false;
            CalcularDesdePrecioVenta();
            bandera = true;
        }

        private void txtPorcentajeAumento_TextChanged(object sender,TextChangedEventArgs e)
        {
            if (!bandera)
                return;

            bandera = false;
            CalcularDesdePorcentaje();
            bandera = true;
        }

        private void btn_AumentarAumento_Click(object sender, RoutedEventArgs e)
        {
            decimal porcentaje = Convert.ToDecimal(txtPorcentajeAumento.Text);
            porcentaje = porcentaje + 1;
            txtPorcentajeAumento.Text = (porcentaje.ToString());
        }

        private void btn_DisminuirAumento_Click(object sender, RoutedEventArgs e)
        {
            decimal porcentaje = Convert.ToDecimal(txtPorcentajeAumento.Text);
            porcentaje = porcentaje - 1;
            txtPorcentajeAumento.Text = (porcentaje.ToString());
        }

        private async void btn_GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (productoSeleccionado == null)
                {
                    MessageBox.Show("No se logro encontrar el producto a Actualizar",
                        "Producto No Encontrado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show(
                        "Debe ingresar el nombre del producto.",
                        "Datos incompletos",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    txtNombre.Focus();
                    return;
                }

                if (!decimal.TryParse(
                 txtPrecioVenta.Text,
                 out decimal precioVenta))
                {
                    MessageBox.Show(
                        "El precio de venta no es válido.",
                        "Precio incorrecto",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    txtPrecioVenta.Focus();
                    return;
                }
                if (cbCategoria.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar una categoría.",
                        "Categoría requerida",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    cbCategoria.Focus();
                    return;
                }
                int categoriaId = Convert.ToInt32(cbCategoria.SelectedValue);
                decimal precioPromedio = productoSeleccionado.PrecioPromedio ?? 0m;
                decimal precioCompra = productoSeleccionado.PrecioCompra ?? 0m;
                decimal stock = productoSeleccionado.Stock;

                await productoService.actualizarProducto(
                           productoSeleccionado.Id,
                           txtCodigo.Text.Trim(),
                           txtNombre.Text.Trim(),
                           txtMarca.Text.Trim(),
                           txtDescripcion.Text.Trim(),
                           precioPromedio,
                           precioCompra,
                           precioVenta,
                           txtUnidadMedida.Text.Trim(),
                           stock,
                           categoriaId
                       );

                MessageBox.Show(
                    "El producto se actualizó correctamente.",
                    "Cambios guardados",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No fue posible actualizar el producto.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}