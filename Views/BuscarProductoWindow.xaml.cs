using Libreria_Rembooks.Migrations;
using Libreria_Rembooks.Models;
using Libreria_Rembooks.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Lógica de interacción para BuscarProductoWindow.xaml
    /// </summary>
    public partial class BuscarProductoWindow : Window
    {
        public ProductoService service = new ProductoService();
        public DetalleCompraModelView detalle=new DetalleCompraModelView();
        
        public Producto productoObtenido{ get; private set; }
       
        public BuscarProductoWindow()
        {
            InitializeComponent();
          
            cargarProductos();
        }


        private async void cargarProductos()
        {
            dgProductos.ItemsSource = await service.ObtenerTodas();
        }
      
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            
                var productoSeleccionado = (Producto)dgProductos.SelectedItem;
                //Si No se  Selecciona Ningun Producto el Dato sera nulo y mostrara un mensaje de advertencia
                if (productoSeleccionado == null)
                {
                    MessageBox.Show("Tienes Que Seleccionar Un Item");
                    return;
                    
                }
                else
                {
                    productoObtenido = productoSeleccionado;
                    this.DialogResult = true;
                    this.Close();
                }
          
            
            
            /*
            //Convertimos el valor seleccionado en un objeto
            var productoSeleccionar = (Producto)dgProductos.SelectedItem;
            //Nos aseguramos que el usuario seleccione algun item antes de iniciar con los demas procesos
            if (productoSeleccionar== null)
            {
                MessageBox.Show("Tienes que seleccionar un item");
            }
            else
            {   //Primero se asegura que el usuario no ingrese una cantidad menor o igual a cero 
                if (decimal.Parse(txtCantidad.Text.ToString()) <= 0)
                {
                    MessageBox.Show($"La cantidad de una compra no puede ser {txtCantidad.Text}");
                }
                else
                {   //Cuando el precio de compra es el mismo que el de la ultima compra entonces de una vez se guardan los datos
                    if (productoSeleccionar.PrecioCompra==decimal.Parse(txtPrecio.Text))
                    {   //Validacion por si no existia un precio de compra anteriormente y el valor era igual a 0
                        if (decimal.Parse(txtPrecio.Text) <= 0)
                        {
                            MessageBox.Show("El Precio Del Producto No Puede Ser Menor O Igual A 0");
                            return;
                        }
                        guardarDetalle(productoSeleccionar);
                        this.DialogResult = true;
                        this.Close();
                    }
                 
                    else
                    {   
                        //Si el precio de compra no coincide con el anterior si se desea actualizar el precio
                        MessageBoxResult r = MessageBox.Show("Desea Actualizar El Precio Del Producto?",
                            "Confirmar",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);
                        if (r == MessageBoxResult.Yes)
                        {
                            guardarDetalle(productoSeleccionar);
                            this.DialogResult = true;
                            this.Close();
                        }

                    }
                }

            }
            */



        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                dgProductos.ItemsSource = await service.buscarCampo(1, txtNombre.Text);
            }
            else
            {
                dgProductos.ItemsSource = await service.buscarCampo(2, txtCodigo.Text);
            }
        }

        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var producto = dgProductos.SelectedItem as Producto;

        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }





    
}
