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

namespace Libreria_Rembooks.Views
{
    /// <summary>
    /// Lógica de interacción para BuscarClienteWindow.xaml
    /// </summary>
    public partial class BuscarClienteWindow : Window
    {
        ClienteService clienteService = new ClienteService();
        public Cliente? clienteSeleccionado { get; private set; }
        public BuscarClienteWindow()
        {
            InitializeComponent();
            cargarClientes();
            
        }
        public async void cargarClientes()
        {
            dgClientes.ItemsSource = await clienteService.ObtenerTodas();
           
        }
        private void btnNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is not Cliente cliente)
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente.",
                    "Seleccionar cliente",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                DialogResult = false;
                return;
            }
            clienteSeleccionado = cliente;

            DialogResult = true;


        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
