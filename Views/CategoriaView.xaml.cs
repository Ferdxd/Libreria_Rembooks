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
    /// Lógica de interacción para IngresoCategorias.xaml
    /// </summary>
    public partial class CategoriaView : UserControl
    {
        private CategoriaService _service = new CategoriaService();
        private Categoria _categoriaSeleccionada;
        public CategoriaView()
        {
            InitializeComponent();
            cambioBotones(true);
            CargarCategorias();
        }

       
        private async void CargarCategorias()
        {
            dgCategorias.ItemsSource = await _service.ObtenerTodas();
        }

        private void limpiarCampos()
        {
            txtDescripcion.Clear();
            txtNombre.Clear();
            txtBuscar.Clear();
            CargarCategorias();
        }

        private void cambioBotones(bool cambiar)
        {
            //Botones Por Default
            Btn_Guardar.Visibility = cambiar ? Visibility.Visible : Visibility.Collapsed;
            Btn_Limpiar.Visibility = cambiar ? Visibility.Visible : Visibility.Collapsed;
          //  Btn_Actualizar.Visibility = cambiar ? Visibility.Visible : Visibility.Collapsed;
            //Botones modo editar
            Btn_GuardarCambios.Visibility=cambiar? Visibility.Collapsed : Visibility.Visible;
            Btn_Cancelar.Visibility = cambiar ? Visibility.Collapsed : Visibility.Visible;
        }
        private async void Btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Es Obligatorio Ingresar Un Nombre!");
                return;
            }
            try
            {
                await _service.CrearCategoria(txtNombre.Text, txtDescripcion.Text);
                CargarCategorias();
                MessageBox.Show("Categoria Guardada Correctamente.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                limpiarCampos();
            }
        }

        private void Btn_Limpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }
        private void Btn_Editar_Click(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if(boton?.DataContext is Categoria categoria)
            {
                _categoriaSeleccionada = categoria;
                txtNombre.Text = _categoriaSeleccionada.Nombre;
                txtDescripcion.Text = _categoriaSeleccionada.Descripcion;
                cambioBotones(false);
            }
        }
        private async void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            dgCategorias.ItemsSource= await _service.Buscar(txtBuscar.Text);
        }

        private async void Btn_GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Es Obligatorio Tener Un Nombre!");
                return;
            }
            await _service.ActualizarCategoria(_categoriaSeleccionada.Id, txtNombre.Text, txtDescripcion.Text);
            cambioBotones(true);
            CargarCategorias();
            limpiarCampos();

        }
        private void Btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            cambioBotones(true);
            limpiarCampos();
        }
    }
}
