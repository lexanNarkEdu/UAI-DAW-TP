using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class ProductoBLL
    {
        private readonly ProductoDAL _dal;

        public ProductoBLL()
        {
            _dal = new ProductoDAL();
        }
        public List<Producto> ObtenerTodos()
        {
            // Aquí podrías aplicar validaciones o reglas de negocio
            return _dal.ObtenerTodos();
        }

        public List<Producto> ObtenerTodosActivos()
        {
            // Aquí podrías aplicar validaciones o reglas de negocio
            return _dal.ObtenerTodosActivos();
        }

        public List<Producto> ObtenerPorCategoria(int categoriaId)
        {
            return _dal.ObtenerPorCategoria(categoriaId);
        }

        public List<Producto> ObtenerPorCategoriaYCondicion(int? categoriaId, int? condicionId, bool activo)
        {
            return _dal.ObtenerPorCategoriaYCondicion(categoriaId, condicionId, activo);
        }

        public Producto ObtenerPorId(int id)
        {
            return _dal.ObtenerPorId(id);
        }

        public void Agregar(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");
            // Nombre
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(producto.Nombre));
            // Precio
            if (producto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.", nameof(producto.Precio));
            // Descripción
            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                throw new ArgumentException("La descripción es obligatoria.", nameof(producto.Descripcion));
            // Stock
            if (producto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.", nameof(producto.Stock));
            // Foto (URL)
            if (string.IsNullOrWhiteSpace(producto.Foto))
                throw new ArgumentException("La URL de la foto es obligatoria.", nameof(producto.Foto));
            // Categoría
            if (producto.CategoriaId <= 0)
                throw new ArgumentException("Debes seleccionar una categoría válida.", nameof(producto.CategoriaId));
            // Condición
            if (producto.CondicionId <= 0)
                throw new ArgumentException("Debes seleccionar una condición válida.", nameof(producto.CondicionId));
            // (Con enum CondicionEnum en tu capa BE:
            // if (!Enum.IsDefined(typeof(CondicionEnum), producto.CondicionId))
            //     throw new ArgumentException("Condición no reconocida.", nameof(producto.CondicionId));
            // )
            // UsuarioCreacion
            if (string.IsNullOrWhiteSpace(producto.UsuarioCreacion))
                throw new ArgumentException("Información de usuario es obligatoria.", nameof(producto.UsuarioCreacion));

            _dal.Agregar(producto);
        }

        public void Eliminar(int idProducto)
        {
            throw new NotImplementedException("Eliminar todavía no está implementado.");
        }

        public void Modificar(Producto producto)
        {
            throw new NotImplementedException("Modificar todavía no está implementado.");
        }

    }
}