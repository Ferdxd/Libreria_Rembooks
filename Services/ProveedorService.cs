using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class ProveedorService:BaseService<Proveedor>
    {
        public async Task crearProveedor(string nit, string razonSocial,string direccion,string telefono)
        {
            if (string.IsNullOrEmpty(nit))
                throw new ArgumentException("El Nit Es Obligatorio");
          //  if (string.IsNullOrEmpty(razonSocial))
               // throw new ArgumentException("La Razon Social Es Obligatoria");

            nit = nit.Trim();
            razonSocial = razonSocial.Trim();
            direccion = direccion?.Trim();
            telefono = telefono?.Trim();

            using var db = CrearConexion();
            bool existe = await db.Proveedores
                .AnyAsync(c => c.Nit == nit);
            if (existe)
                throw new InvalidOperationException("Ya existe un proveedor con ese Nit");
            var proveedor = new Proveedor
            {
                Nit = nit,
                Razon_Social = razonSocial,
                Direccion = direccion,
                Telefono = telefono
            };
            await base.Crear(proveedor);
        }
        public async Task actualizarProveedor(int id, string nit, string razonSocial,string direccion, string telefono)
        {
            if (id <= 0)
                throw new ArgumentException("Proveedor Invalido");
            if (string.IsNullOrEmpty(nit) && string.IsNullOrEmpty(razonSocial))
                throw new ArgumentException("El Nit y Razon Social Son Obligatorios");
            nit = nit.Trim();
            razonSocial = razonSocial.Trim();
            direccion=direccion?.Trim();
            telefono=telefono?.Trim();

            using var db = CrearConexion();
            var proveedor = await db.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                throw new KeyNotFoundException("Proveedor No Encontrado");
            }
            bool existe = await db.Proveedores
                .AnyAsync(c => c.Id != id && c.Nit == nit);
            if (existe)
                throw new InvalidOperationException("Ya existe un proveedor con este Nit");
            proveedor.Id = id;
            proveedor.Nit = nit;
            proveedor.Razon_Social = razonSocial;
            proveedor.Direccion = direccion;
            proveedor.Telefono= telefono;

            await base.Actualizar(proveedor);
        }

        public async Task<List<Proveedor>> buscarPorNombre(string filtro)
        {
            using var db = CrearConexion();
            if (string.IsNullOrWhiteSpace(filtro))
                return await ObtenerTodas();
            filtro = filtro.Trim();
            return await db.Proveedores
                .AsNoTracking()
                .Where(c => EF.Functions.Like(c.Razon_Social, $"%{filtro}%"))
                .OrderBy(c => c.Razon_Social)
                .ToListAsync();
        }

       
        
    }
}
