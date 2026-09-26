using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class CompraService:BaseService<Compra>
    {
        public async Task<int> crearCompra(int proveedorId, DateTime fechaEmision, DateTime fechaIngreso, string numeroSerie, string numeroDocumento, string tipoDocumento, decimal Total)
        {
            if (proveedorId <= 0 || Total <= 0)
                throw new ArgumentException("Datos inválidos");
            numeroDocumento = numeroDocumento?.Trim();
            tipoDocumento = tipoDocumento?.Trim();

            using var db = CrearConexion();
            var compra = new Compra
            {
                ProveedorId = proveedorId,
               FechaEmision = fechaEmision,
               FechaIngreso= fechaIngreso,
               Numero_Serie = numeroSerie,
                Numero_Documento = numeroDocumento,
                Tipo_Documento = tipoDocumento,
                total = Total
            };
            await base.Crear(compra);
            return compra.Id;
        }
        //como obtengo el id de la compra? R/el id se genera automaticamente tendria que tener una funcion para rescatar el id generado como?
        //como relaciono la compra con el detalle de la compra? R/
        //al hacer el detalle de la compra tengo que actualizar los productos como?
        public async Task<List<Compra>> ObtenerComprasConProveedor()
        {
            using var db = CrearConexion();

            return await db.Compras
                .Include(c => c.Proveedor)
                .ToListAsync();
        }



        public async Task<List<Compra>> obtenerCompraxFecha(DateTime? desde, DateTime? hasta, int proveedor)
        {
            var db = CrearConexion();

            var query=db.Compras
                .AsNoTracking()
                .Include(c => c.Proveedor)
                .AsQueryable();

            if (desde.HasValue)
            {
                query = query.Where(c => c.FechaEmision >= desde.Value.Date);
            }
            if (hasta.HasValue)
            {
                var fechaHasta = hasta.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(c => c.FechaEmision <= fechaHasta);
            }
            if (proveedor!=0)
            {
                query=query.Where(c => c.ProveedorId==(proveedor));
            }
            return await query.ToListAsync();


            /*.Where(c => c.FechaEmision >= Desde.Date && c.FechaEmision <= Hasta.Date)
            .ToListAsync();*/

        }

    }
}
