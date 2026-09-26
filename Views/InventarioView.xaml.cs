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
using ClosedXML.Excel;
using Microsoft.Win32;
using System.IO;
namespace Libreria_Rembooks.Views
{
    /// <summary>
    /// Lógica de interacción para InventarioView.xaml
    /// </summary>
    public partial class InventarioView : UserControl
    {
        ProductoService serviceProducto = new ProductoService();
        CategoriaService serviceCategoria=new CategoriaService();
        public InventarioView()
        {
            InitializeComponent();
            cargarProductos();
        }
        async void cargarProductos()
        {
            dgProductos.ItemsSource = await serviceProducto.ObtenerProductoConCategoria();
            cbCategorias.ItemsSource = await serviceCategoria.ObtenerTodas();

        }

        private async void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            bool? estado = checkAgotados.IsChecked;
            var categoria = (Categoria)cbCategorias.SelectedItem;
          
            dgProductos.ItemsSource = await serviceProducto.buscarPorParametros(txtCodigo.Text, txtNombre.Text,categoria?.Nombre, estado.Value);
            
           
        }

        private void btn_Limpiar_Click(object sender, RoutedEventArgs e)
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            checkAgotados.IsChecked = false;
            cargarProductos();
        }

        private void btn_Seleccionar(object sender, RoutedEventArgs e)
        {
            var productoSeleccionado = (Producto)dgProductos.SelectedItem;
            if(dgProductos.SelectedItem == null){
                MessageBox.Show("Debe Seleccionar el Producto que Desea Revisar");
                return;
            }
            var ventana = new GestionProductoWindow(productoSeleccionado.Id);
            bool? resultado = ventana.ShowDialog();
            if (resultado == true)
            {
                cargarProductos();
            }
        }

        private void btn_ExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProductos.Items.Count == 0)
                {
                    MessageBox.Show(
                        "No hay productos para exportar.",
                        "Exportar a Excel",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivo de Excel (*.xlsx)|*.xlsx",
                    FileName = $"Inventario_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    DefaultExt = ".xlsx"
                };

                if (saveFileDialog.ShowDialog() != true)
                {
                    return;
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Inventario");

                    // =====================================================
                    // TÍTULO
                    // =====================================================

                    worksheet.Cell(1, 1).Value = "REPORTE DE INVENTARIO";

                    worksheet.Range(1, 1, 1, 11).Merge();

                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    // =====================================================
                    // FECHA DE GENERACIÓN
                    // =====================================================

                    worksheet.Cell(2, 1).Value =
                        $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";

                    worksheet.Range(2, 1, 2, 11).Merge();

                    worksheet.Cell(2, 1).Style.Font.Italic = true;

                    // =====================================================
                    // ENCABEZADOS
                    // =====================================================

                    int filaEncabezado = 4;

                    worksheet.Cell(filaEncabezado, 1).Value = "ID";
                    worksheet.Cell(filaEncabezado, 2).Value = "Código";
                    worksheet.Cell(filaEncabezado, 3).Value = "Nombre";
                    worksheet.Cell(filaEncabezado, 4).Value = "Marca";
                    worksheet.Cell(filaEncabezado, 5).Value = "Descripción";
                    worksheet.Cell(filaEncabezado, 6).Value = "Categoría";
                    worksheet.Cell(filaEncabezado, 7).Value = "Unidad de Medida";
                    worksheet.Cell(filaEncabezado, 8).Value = "Stock";
                    worksheet.Cell(filaEncabezado, 9).Value = "Precio Promedio";
                    worksheet.Cell(filaEncabezado, 10).Value = "Precio Compra";
                    worksheet.Cell(filaEncabezado, 11).Value = "Precio Venta";

                    var encabezados = worksheet.Range(
                        filaEncabezado,
                        1,
                        filaEncabezado,
                        11);

                    encabezados.Style.Font.Bold = true;

                    encabezados.Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#2563EB");

                    encabezados.Style.Font.FontColor =
                        XLColor.White;

                    encabezados.Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    encabezados.Style.Alignment.Vertical =
                        XLAlignmentVerticalValues.Center;

                    worksheet.Row(filaEncabezado).Height = 24;

                    // =====================================================
                    // PRODUCTOS
                    // =====================================================

                    int fila = filaEncabezado + 1;

                    foreach (var item in dgProductos.Items)
                    {
                        if (item is Producto producto)
                        {
                            worksheet.Cell(fila, 1).Value =
                                producto.Id;

                            worksheet.Cell(fila, 2).Value =
                                producto.Codigo ?? "";

                            worksheet.Cell(fila, 3).Value =
                                producto.Nombre ?? "";

                            worksheet.Cell(fila, 4).Value =
                                producto.Marca ?? "";

                            worksheet.Cell(fila, 5).Value =
                                producto.Descripcion ?? "";

                            worksheet.Cell(fila, 6).Value =
                                producto.Categoria?.Nombre ?? "";

                            worksheet.Cell(fila, 7).Value =
                                producto.UnidadMedida ?? "";

                            worksheet.Cell(fila, 8).Value =
                                producto.Stock;

                            worksheet.Cell(fila, 9).Value =
                                producto.PrecioPromedio ?? 0;

                            worksheet.Cell(fila, 10).Value =
                                producto.PrecioCompra ?? 0;

                            worksheet.Cell(fila, 11).Value =
                                producto.PrecioVenta ?? 0;

                            fila++;
                        }
                    }

                    // =====================================================
                    // FORMATOS NUMÉRICOS
                    // =====================================================

                    if (fila > filaEncabezado + 1)
                    {
                        // STOCK
                        worksheet.Range(
                            filaEncabezado + 1,
                            8,
                            fila - 1,
                            8)
                            .Style.NumberFormat.Format = "#,##0.00";

                        // PRECIO PROMEDIO
                        worksheet.Range(
                            filaEncabezado + 1,
                            9,
                            fila - 1,
                            9)
                            .Style.NumberFormat.Format = "\"Q\" #,##0.00";

                        // PRECIO COMPRA
                        worksheet.Range(
                            filaEncabezado + 1,
                            10,
                            fila - 1,
                            10)
                            .Style.NumberFormat.Format = "\"Q\" #,##0.00";

                        // PRECIO VENTA
                        worksheet.Range(
                            filaEncabezado + 1,
                            11,
                            fila - 1,
                            11)
                            .Style.NumberFormat.Format = "\"Q\" #,##0.00";
                    }

                    // =====================================================
                    // BORDES
                    // =====================================================

                    if (fila > filaEncabezado + 1)
                    {
                        var tabla = worksheet.Range(
                            filaEncabezado,
                            1,
                            fila - 1,
                            11);

                        tabla.Style.Border.OutsideBorder =
                            XLBorderStyleValues.Thin;

                        tabla.Style.Border.InsideBorder =
                            XLBorderStyleValues.Thin;

                        tabla.Style.Border.OutsideBorderColor =
                            XLColor.FromHtml("#D1D5DB");

                        tabla.Style.Border.InsideBorderColor =
                            XLColor.FromHtml("#E5E7EB");
                    }

                    // =====================================================
                    // ALINEACIÓN
                    // =====================================================

                    if (fila > filaEncabezado + 1)
                    {
                        // ID
                        worksheet.Range(
                            filaEncabezado + 1,
                            1,
                            fila - 1,
                            1)
                            .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Center;

                        // STOCK
                        worksheet.Range(
                            filaEncabezado + 1,
                            8,
                            fila - 1,
                            8)
                            .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Right;

                        // PRECIOS
                        worksheet.Range(
                            filaEncabezado + 1,
                            9,
                            fila - 1,
                            11)
                            .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Right;
                    }

                    // =====================================================
                    // ANCHO DE COLUMNAS
                    // =====================================================

                    worksheet.Column(1).Width = 10;  // ID
                    worksheet.Column(2).Width = 16;  // Código
                    worksheet.Column(3).Width = 32;  // Nombre
                    worksheet.Column(4).Width = 20;  // Marca
                    worksheet.Column(5).Width = 40;  // Descripción
                    worksheet.Column(6).Width = 22;  // Categoría
                    worksheet.Column(7).Width = 20;  // Unidad medida
                    worksheet.Column(8).Width = 14;  // Stock
                    worksheet.Column(9).Width = 18;  // Precio promedio
                    worksheet.Column(10).Width = 18; // Precio compra
                    worksheet.Column(11).Width = 18; // Precio venta

                    // =====================================================
                    // AJUSTAR TEXTO
                    // =====================================================

                    worksheet.Column(3).Style.Alignment.WrapText = true;
                    worksheet.Column(5).Style.Alignment.WrapText = true;

                    // =====================================================
                    // CONGELAR ENCABEZADO
                    // =====================================================

                    worksheet.SheetView.FreezeRows(filaEncabezado);

                    // =====================================================
                    // FILTROS
                    // =====================================================

                    if (fila > filaEncabezado + 1)
                    {
                        worksheet.Range(
                            filaEncabezado,
                            1,
                            fila - 1,
                            11)
                            .SetAutoFilter();
                    }

                    // =====================================================
                    // GUARDAR
                    // =====================================================

                    workbook.SaveAs(saveFileDialog.FileName);
                }

                MessageBox.Show(
                    "El inventario se exportó correctamente.",
                    "Exportación completada",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al exportar el inventario:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
