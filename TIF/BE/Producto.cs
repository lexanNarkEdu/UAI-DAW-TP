using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Foto { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        
        public string CategoriaNombre { get; set; }
        public int CondicionId { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string VerificadorHorizontal { get; set; }

        public Producto() { }

        public Producto(
            int productoId,
            string nombre,
            decimal precio,
            string foto,
            string descripcion,
            int stock,
            int categoriaId,
            string categoriaNombre,
            int condicionId,
            bool activo,
            DateTime fechaCreacion,
            DateTime fechaModificacion,
            string usuarioCreacion,
            string verificadorHorizontal)
        {
            ProductoId = productoId;
            Nombre = nombre;
            Precio = precio;
            Foto = foto;
            Descripcion = descripcion;
            Stock = stock;
            CategoriaId = categoriaId;
            CategoriaNombre = categoriaNombre;
            CondicionId = condicionId;
            Activo = activo;
            FechaCreacion = fechaCreacion;
            UsuarioCreacion = usuarioCreacion;
            VerificadorHorizontal = verificadorHorizontal;
        }
    }
}
