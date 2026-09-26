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
using System.Printing;
using System;
using System.Windows.Markup;



namespace Libreria_Rembooks.Views
{
    /// <summary>
    /// Lógica de interacción para VerDetalleCompraWindow.xaml
    /// </summary>
    public partial class VerDetalleCompraWindow : Window
    {
        DetalleCompraService detalle_Compra = new DetalleCompraService();
        CompraService compraService=new CompraService();
        ProveedorService proveedorService=new ProveedorService();
        public VerDetalleCompraWindow(int idCompra)
        {
            InitializeComponent();
            cargarDetalleCompra(idCompra);
        }
        /* 
            var ventana = new CrearProductoWindow();
            bool? resultado = ventana.ShowDialog();  */
        public async void cargarDetalleCompra(int id)
        {
            var compra = await compraService.ObtenerPorId(id);
            var proveedor = await proveedorService.ObtenerPorId(compra.ProveedorId);
            dgDetalleCompras.ItemsSource = await detalle_Compra.obtenerDetalleCompraxId(id);
            txtRazonSocial.Text = proveedor.Razon_Social;
            txtNoDocumento.Text = compra.Numero_Documento;
            txtSerie.Text = compra.Numero_Serie;
            txtFechaEmision.Text = compra.FechaEmision.ToShortDateString();
            txtblock_Total.Text = compra.total.ToString();
        }

        private void btnImprimirPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FixedDocument documento = CrearDocumentoFactura();

                DocumentViewer visor = new DocumentViewer
                {
                    Document = documento
                };

                Window ventanaVistaPrevia = new Window
                {
                    Title = "Vista previa de impresión",
                    Width = 1050,
                    Height = 750,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this,
                    Background = Brushes.White,
                    Content = visor
                };

                ventanaVistaPrevia.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo generar la vista previa.\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private FixedDocument CrearDocumentoFactura()
        {
            FacturaPanel.UpdateLayout();

            double anchoFactura = FacturaPanel.ActualWidth;
            double altoFactura = FacturaPanel.ActualHeight;

            // A4 horizontal a 96 DPI
            double anchoPagina = 1122.52;
            double altoPagina = 793.70;

            double margen = 40;

            double anchoDisponible = anchoPagina - (margen * 2);
            double altoDisponible = altoPagina - (margen * 2);

            double escalaX = anchoDisponible / anchoFactura;
            double escalaY = altoDisponible / altoFactura;

            double escala = Math.Min(escalaX, escalaY);

            // Evitamos ampliar demasiado el diseño
            if (escala > 1)
            {
                escala = 1;
            }

            double anchoFinal = anchoFactura * escala;
            double altoFinal = altoFactura * escala;

            double izquierda = (anchoPagina - anchoFinal) / 2;
            double arriba = (altoPagina - altoFinal) / 2;

            FixedDocument documento = new FixedDocument();

            documento.DocumentPaginator.PageSize =
                new Size(anchoPagina, altoPagina);

            FixedPage pagina = new FixedPage
            {
                Width = anchoPagina,
                Height = altoPagina,
                Background = Brushes.White
            };

            VisualBrush facturaVisual = new VisualBrush(FacturaPanel)
            {
                Stretch = Stretch.Fill
            };

            Rectangle factura = new Rectangle
            {
                Width = anchoFinal,
                Height = altoFinal,
                Fill = facturaVisual
            };

            FixedPage.SetLeft(factura, izquierda);
            FixedPage.SetTop(factura, arriba);

            pagina.Children.Add(factura);

            PageContent contenidoPagina = new PageContent();

            ((IAddChild)contenidoPagina).AddChild(pagina);

            documento.Pages.Add(contenidoPagina);

            return documento;
        }

        private void ImprimirFactura(PrintDialog printDialog)
        {
            double anchoImprimible = printDialog.PrintableAreaWidth;
            double altoImprimible = printDialog.PrintableAreaHeight;

            double anchoFactura = FacturaPanel.ActualWidth;
            double altoFactura = FacturaPanel.ActualHeight;

            if (anchoFactura <= 0 || altoFactura <= 0)
            {
                return;
            }

            double escalaX = anchoImprimible / anchoFactura;
            double escalaY = altoImprimible / altoFactura;

            double escala = Math.Min(escalaX, escalaY);

            // Evitar ampliar demasiado si ya cabe en la hoja
            if (escala > 1)
            {
                escala = 1;
            }

            double anchoFinal = anchoFactura * escala;
            double altoFinal = altoFactura * escala;

            double izquierda = (anchoImprimible - anchoFinal) / 2;
            double arriba = (altoImprimible - altoFinal) / 2;

            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawRectangle(
                    Brushes.White,
                    null,
                    new Rect(
                        0,
                        0,
                        anchoImprimible,
                        altoImprimible
                    )
                );

                dc.PushTransform(
                    new TranslateTransform(
                        izquierda,
                        arriba
                    )
                );

                dc.PushTransform(
                    new ScaleTransform(
                        escala,
                        escala
                    )
                );

                VisualBrush facturaBrush = new VisualBrush(FacturaPanel);

                dc.DrawRectangle(
                    facturaBrush,
                    null,
                    new Rect(
                        0,
                        0,
                        anchoFactura,
                        altoFactura
                    )
                );

                dc.Pop();
                dc.Pop();
            }

            printDialog.PrintVisual(
                visual,
                $"Compra {txtNoDocumento.Text}"
            );
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
            
        }
    }
}
