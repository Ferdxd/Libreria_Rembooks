using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    class ClienteService:BaseService<Cliente>
    {
        public async Task crearCliente(string nit, string nombre, string direccion, string telefono)
        {
            if (string.IsNullOrWhiteSpace(nit))
                throw new ArgumentNullException("El tipo de dato nit es obligatorio");

            nit = nit.Trim();
            nombre = nombre.Trim();
            telefono = telefono.Trim();

            using var db = CrearConexion();
            bool existe = await db.Clientes.AnyAsync(c => c.Nit == nit);
            if (existe) throw new InvalidOperationException("El Nit Ya Existe En El Sistema");
            var cliente = new Cliente()
            {
                Nit = nit,
                Nombre=nombre,
                Direccion=direccion,
                Telefono=telefono
            };

            await base.Crear(cliente);

            
        }
    }
}
