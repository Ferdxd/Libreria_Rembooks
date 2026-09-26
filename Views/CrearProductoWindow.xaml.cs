using Libreria_Rembooks.Models;
using Libreria_Rembooks.Services;
using Microsoft.EntityFrameworkCore;
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
    /// Lógica de interacción para CrearProductoWindow.xaml
    /// </summary>
    public partial class CrearProductoWindow : Window
    {
        ProductoService _service = new ProductoService();
        CategoriaService _serviceCategoria = new CategoriaService();
        
        
        public CrearProductoWindow()
        {
            InitializeComponent();
            cargarCategorias();
            
        }
        public async void cargarCategorias()
        {
            cbCategoria.ItemsSource = await _serviceCategoria.ObtenerTodas();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var categoria = (Categoria)cbCategoria.SelectedItem;
            if (categoria == null)
            {
                MessageBox.Show("Es Obligatorio Que El Producto Pertenezca A Un Tipo De Categoria");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Es Obligatorio Añadirle un Nombre al Producto");
                return;
            }
            try
            {
                await _service.crearProducto(categoria.Id, txtCodigo.Text, txtNombre.Text, txtMarca.Text,
                    txtDescripcion.Text, (0), (0), (0), txtUnidad.Text, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                txtCodigo.Clear();
                txtNombre.Clear();
                txtDescripcion.Clear();
                txtMarca.Clear();
                txtUnidad.Clear();
                cbCategoria.SelectedIndex = 0;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
