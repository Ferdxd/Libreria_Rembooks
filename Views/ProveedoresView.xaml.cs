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
    /// Lógica de interacción para IngresoProveedores.xaml
    /// </summary>
    public partial class ProveedoresView : UserControl
    {
        private ProveedorService _service = new ProveedorService();
        private Proveedor _proveedorSeleccionado= new Proveedor();
        public ProveedoresView()
        {
            InitializeComponent();
            cargarProveedores();
            cambioBotones(true);
        }
        private async void cargarProveedores()
        {
            dgProveedores.ItemsSource = await _service.ObtenerTodas();
        }
        private void cambioBotones(bool cambiar)
        {
            //Botones Por Default
            btn_Guardar.Visibility = cambiar ? Visibility.Visible : Visibility.Collapsed;
            btn_Limpiar.Visibility = cambiar ? Visibility.Visible : Visibility.Collapsed;
            //Botones modo editar
            btn_GuardarCambios.Visibility = cambiar ? Visibility.Collapsed : Visibility.Visible;
            btn_Cancelar.Visibility = cambiar ? Visibility.Collapsed : Visibility.Visible;
        }
        private void limpiarCampos()
        {
            txtBuscar.Clear();
            txtDireccion.Clear();
            txtNit.Clear();
            txtRazonSocial.Clear();
            txtTelefono.Clear();
            cargarProveedores();
        }

        private async void btn_GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNit.Text) || string.IsNullOrWhiteSpace(txtRazonSocial.Text))
                MessageBox.Show("EL Campo Nit y Razon Social Son Obligatorios");
            try
            {
                await _service.actualizarProveedor(_proveedorSeleccionado.Id, txtNit.Text, txtRazonSocial.Text, txtDireccion.Text, txtTelefono.Text);
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cambioBotones(true);
                limpiarCampos();
                
            }
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            cambioBotones(true);
        }

        private async void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNit.Text))
            {
                MessageBox.Show("El Nit Es Obligatorio");
                return;
            }
/*
            if (string.IsNullOrWhiteSpace(txtRazonSocial.Text))
            {
                MessageBox.Show("La Razon Social Es Obligatoria");
                return;
            }
*/
            try
            {
                await _service.crearProveedor(txtNit.Text, txtRazonSocial.Text, txtDireccion.Text, txtTelefono.Text);
                cargarProveedores();
                MessageBox.Show("Proveedor Guardado Correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                limpiarCampos();
            }

        }

        private void btn_Limpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        private async void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            dgProveedores.ItemsSource = await _service.buscarPorNombre(txtBuscar.Text);
        }
        private void btn_Editar_Click(object sender, RoutedEventArgs e)
        {
            Button boton =sender as Button;
            if(boton?.DataContext is Proveedor proveedor)
            {
                _proveedorSeleccionado = proveedor;
                txtNit.Text = proveedor.Nit;
                txtRazonSocial.Text = proveedor.Razon_Social;
                txtDireccion.Text = proveedor.Direccion;
                txtTelefono.Text = proveedor.Telefono;
                cambioBotones(false);
            }
        }
    }
}
