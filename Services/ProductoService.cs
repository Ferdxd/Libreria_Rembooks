using Libreria_Rembooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_Rembooks.Services
{
    public class ProductoService:BaseService<Producto>
    {
        public async Task crearProducto(int categoriaId, string codigo ,string nombre, string marca, string descripcion, decimal precioPromedio, decimal precioCompra, decimal precioVenta,string unidadMedida, decimal Stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentNullException(nameof(nombre));
            }
            codigo = codigo.Trim();
            nombre= nombre.Trim();
            descripcion= descripcion?.Trim();
            unidadMedida= unidadMedida.Trim();
            var producto = new Producto()
            {
                CategoriaId = categoriaId,
                Codigo = codigo,
                Nombre=nombre,
                Marca=marca,
                Descripcion=descripcion,
                PrecioPromedio=precioPromedio,
                PrecioCompra=precioCompra,
                PrecioVenta=precioVenta,
                UnidadMedida=unidadMedida,
                Stock=Stock
            };
            await base.Crear(producto);

        }

        public async Task<List<Producto>> ObtenerProductoConCategoria()
        {
            using var db = CrearConexion();

            return await db.Productos
                .Include(c => c.Categoria)
                .ToListAsync();
        }
        public async Task<List<Producto>> buscarCampo(int option ,string filtro)
        {
            using var db = CrearConexion();
            if (string.IsNullOrWhiteSpace(filtro))
                return await ObtenerTodas();
            filtro = filtro.Trim();
            if (option == 1)
            {
                return await db.Productos
                .AsNoTracking().
                Where(c => EF.Functions.Like(c.Nombre, $"%{filtro}%"))
                .OrderBy(c => c.Nombre)
                .ToListAsync();
            }
            else
            {
                return await db.Productos
                .AsNoTracking().
                Where(c => EF.Functions.Like(c.Codigo, $"%{filtro}%"))
                .OrderBy(c => c.Nombre)
                .ToListAsync();
            }
            
        }

        //que necesito para actualizar el producto, la cantidad del producto y sumarla al que esta ingresando, el producto se tiene que buscar por id
        public async Task actualizarProducto(int id,string codigo, string nombre, string marca, string descripcion, decimal precioPromedio, decimal precioCompra, decimal precioVenta, string unidadMedida, decimal stock, int categoriaId)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id Invalido");
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El Nombre es obligatorio");
            }
            codigo = codigo?.Trim();
            nombre = nombre.Trim();
            descripcion = descripcion?.Trim();
            unidadMedida = unidadMedida.Trim();

            using var db = CrearConexion();
            var producto = await db.Productos.FindAsync(id);
            if (producto == null)
            {
                throw new KeyNotFoundException("Producto no encontrado");
            }
            bool existe = await db.Productos
                .AnyAsync(c => c.Codigo == codigo&& c.Id!=id);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe un producto con el mismo codigo");
            }
            producto.Codigo = codigo;
            producto.Nombre = nombre;
            producto.Marca = marca;
            producto.Descripcion= descripcion;
            producto.PrecioPromedio= precioPromedio;
            producto.PrecioCompra = precioCompra;
            producto.PrecioVenta= precioVenta;
            producto.UnidadMedida = unidadMedida;
            producto.Stock= stock;
            producto.CategoriaId = categoriaId;
            await base.Actualizar(producto);
        }

   
        public async Task actualizarStockYPrecio(int id, decimal precioCompra, decimal stock)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id Invalido");
            }
            using var db = CrearConexion();

            var producto = await db.Productos.FindAsync(id);

            if (producto == null)
                throw new Exception("Producto no encontrado");
            decimal StockAnterior = producto.Stock;
            decimal CostoPromedioAnterior = producto.PrecioPromedio??0.0m;
            decimal NuevoCostoPromedio = ((StockAnterior * CostoPromedioAnterior) + stock * precioCompra) / (StockAnterior + stock);
            producto.PrecioCompra = precioCompra;
            producto.Stock =producto.Stock+stock;
            producto.PrecioPromedio = NuevoCostoPromedio;

            await base.Actualizar(producto);
        }

        public async Task actualizarDescontarProducto(int id, decimal cantidad)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id Invalido");
            }
            using var db = CrearConexion();
            var producto= await db.Productos.FindAsync(id);
            if (producto == null)
            {
                throw new Exception("Producto No Encontrado");
            }
            producto.Stock = producto.Stock - cantidad;
            await base.Actualizar(producto);
        }

        public async Task<List<Producto>> buscarPorParametros(string codigo, string nombre, string categoria, bool agotados)
        {
            using var db= CrearConexion();
            var query = db.Productos.AsNoTracking()
                .Include(c => c.Categoria)
                .AsQueryable();
       
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                query = query.Where(c => EF.Functions.Like(c.Codigo, $"%{codigo}%"));
            }
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(c => EF.Functions.Like(c.Nombre, $"%{nombre}%"));
            }
            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(c => EF.Functions.Like(c.Categoria.Nombre, $"%{categoria}%"));
            }
            if(agotados)
            {
                query = query.Where(c => c.Stock <= 0);
            }
            return await query.ToListAsync();
        }
        
    }
}
