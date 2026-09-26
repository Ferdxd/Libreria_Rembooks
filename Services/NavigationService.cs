using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Libreria_Rembooks.Services
{
    public static class NavigationService
    {
        private static ContentControl _mainContent;
        private static Stack<UserControl> historial = new();
        public static void Inicializar(ContentControl maincontent)
        {
            _mainContent= maincontent;
        }

        public static void Navegar(UserControl nuevaVista)
        {
            if(_mainContent.Content is UserControl vistaActual)
            {
                historial.Push(vistaActual);
            }
            _mainContent.Content = nuevaVista;
        }

        public static void Atras()
        {
            if (historial.Count > 0)
            {
                _mainContent.Content = historial.Pop();
            }
        }
    }
}
