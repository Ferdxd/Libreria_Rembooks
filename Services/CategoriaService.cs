using Libreria_Rembooks.Data;
using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class CategoriaService : BaseService<Categoria>
    {
        // 🔹 Obtener categorías ordenadas por nombre
        public async Task<List<Categoria>> ObtenerTodasOrdenadas()
        {
            using var db = CrearConexion();

            return await db.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        // 🔹 Crear categoría con validaciones
        public async Task CrearCategoria(string nombre, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            nombre = nombre.Trim();
            descripcion = descripcion?.Trim();

            using var db = CrearConexion();

            bool existe = await db.Categorias
                .AnyAsync(c => c.Nombre == nombre);

            if (existe)
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

            var categoria = new Categoria
            {
                Nombre = nombre,
                Descripcion = descripcion
            };

            await base.Crear(categoria);
        }

        // 🔹 Actualizar categoría con validaciones
        public async Task ActualizarCategoria(int id, string nombre, string descripcion)
        {
            if (id <= 0)
                throw new ArgumentException("Categoría inválida.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            nombre = nombre.Trim();
            descripcion = descripcion?.Trim();

            using var db = CrearConexion();

            var categoria = await db.Categorias.FindAsync(id);

            if (categoria == null)
                throw new KeyNotFoundException("Categoría no encontrada.");

            bool existe = await db.Categorias
                .AnyAsync(c => c.Id != id && c.Nombre == nombre);

            if (existe)
                throw new InvalidOperationException("Ya existe otra categoría con ese nombre.");

            categoria.Nombre = nombre;
            categoria.Descripcion = descripcion;

            await base.Actualizar(categoria);
        }

        // 🔹 Buscar categorías por nombre
        public async Task<List<Categoria>> Buscar(string filtro)
        {
            using var db = CrearConexion();

            if (string.IsNullOrWhiteSpace(filtro))
                return await ObtenerTodasOrdenadas();

            filtro = filtro.Trim();

            return await db.Categorias
                .AsNoTracking()
                .Where(c => EF.Functions.Like(c.Nombre, $"%{filtro}%"))
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}