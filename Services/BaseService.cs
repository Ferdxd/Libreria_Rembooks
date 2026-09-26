using Libreria_Rembooks.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class BaseService<T> where T : class
    {
        protected appDbContext CrearConexion()
        {
            return new appDbContext();
        }

        public async Task<List<T>> ObtenerTodas()
        {
            using var db = CrearConexion();

            return await db.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> ObtenerPorId(int id)
        {
            using var db = CrearConexion();

            return await db.Set<T>().FindAsync(id);
        }

        public async Task Crear(T entidad)
        {
            using var db = CrearConexion();

            db.Set<T>().Add(entidad);

            await db.SaveChangesAsync();
        }

        public async Task Actualizar(T entidad)
        {
            using var db = CrearConexion();

            db.Set<T>().Update(entidad);

            await db.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            using var db = CrearConexion();

            var entidad = await db.Set<T>().FindAsync(id);

            if (entidad != null)
            {
                db.Set<T>().Remove(entidad);
                await db.SaveChangesAsync();
            }
        }
    }
}