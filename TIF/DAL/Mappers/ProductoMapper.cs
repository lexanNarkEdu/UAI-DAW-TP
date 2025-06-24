using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL.Mappers
{
    internal class ProductoMapper : BaseMapper<Producto>
    {
        public override Producto MapToEntity(DataRow row)
        {
            if (row == null) return null;
            
            var categoria = new CategoriaMapper().MapToEntity(row);
            
            return new Producto
            {
                ProductoId = Convert.ToInt32(row["producto_id"]),
                Nombre = row["producto_nombre"].ToString(),
                Precio = Convert.ToDecimal(row["producto_precio"]),
                Foto = row["producto_foto"].ToString(),
                Descripcion = row["producto_descripcion"].ToString(),
                Stock = Convert.ToInt32(row["producto_stock"]),
                CategoriaId = Convert.ToInt32(row["categoria_id"]),
                CondicionId = Convert.ToInt32(row["condicion_id"]),
                Activo = Convert.ToBoolean(row["producto_activo"]),
                FechaCreacion = Convert.ToDateTime(row["producto_fecha_creacion"]),
                UsuarioCreacion = row["producto_usuario_creacion"].ToString(),
                //VerificadorHorizontal = row["producto_verificador_horizontal"].ToString()
                CategoriaNombre = categoria.Nombre,
            };
        }
    }
}
