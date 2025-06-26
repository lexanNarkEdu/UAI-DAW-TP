using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL.Mappers;

namespace DAL
{
    public class ProductoDAL
    {
        private readonly DAO _dao;
        private readonly ProductoMapper _mapper;

        public ProductoDAL()
        {
            _dao = DAO.GetDAO();
            _mapper = new ProductoMapper();
        }
        public List<Producto> ObtenerTodos()
        {
            const string sql = @"
                SELECT 
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, p.categoria_id,
                    c.categoria_nombre, p.condicion_id, producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion
                FROM Producto p 
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id
                ORDER BY p.producto_fecha_creacion DESC";

            DataTable dt = _dao.Read(sql, commandType: CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerTodosActivos()
        {
            const string sql = @"
                SELECT 
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, p.categoria_id, 
                    c.categoria_nombre, p.condicion_id, producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion 
                FROM Producto p 
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id
                WHERE p.producto_activo = 1 
                ORDER BY p.producto_fecha_creacion DESC";

            DataTable dt = _dao.Read(sql, commandType: CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerPorCategoria(int categoriaId)
        {
            const string sql = @"
                SELECT 
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, p.categoria_id, 
                    c.categoria_nombre, p.condicion_id, p.producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion 
                FROM Producto p 
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id
                WHERE p.producto_activo = 1
                AND p.categoria_id = @CategoriaId
                ORDER BY p.producto_fecha_creacion DESC";
            var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@CategoriaId", categoriaId)
                };
            var dt = _dao.Read(sql, parametros, CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerPorCategoriaYCondicion(int? categoriaId, int? condicionId, bool activo)
        {
            var sql = new StringBuilder(@"
                SELECT
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion,
                    p.producto_stock, p.categoria_id, c.categoria_nombre,p.condicion_id, p.producto_activo, 
                    p.producto_fecha_creacion, p.producto_usuario_creacion, p.producto_path_banner
                FROM Producto p
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id ");

            var condiciones = new List<string>();
            var parametros = new List<SqlParameter>();

            // Condiciones dinámicas
            if (activo)
            {
                condiciones.Add("p.producto_activo = @activo");
                parametros.Add(new SqlParameter("@activo", activo));
            }
            if (categoriaId.HasValue)
            {
                condiciones.Add("p.categoria_id = @CategoriaId");
                parametros.Add(new SqlParameter("@CategoriaId", categoriaId.Value));
            }
            if (condicionId.HasValue)
            {
                condiciones.Add("p.condicion_id = @CondicionId");
                parametros.Add(new SqlParameter("@CondicionId", condicionId.Value));
            }

            if (condiciones.Count > 0)
            {
                sql.Append(" WHERE ");
                sql.Append(string.Join(" AND ", condiciones));
            }

            sql.Append(" ORDER BY p.producto_fecha_creacion DESC");

            var dt = _dao.Read(sql.ToString(), parametros, CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            const string sql = @"
                SELECT 
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, 
                    p.categoria_id, p.condicion_id, p.producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion
                FROM Producto p
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id
                WHERE p.producto_id = @id";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = id }
            };

            DataTable dt = _dao.Read(sql, parametros, CommandType.Text);

            if (dt.Rows.Count == 0)
                return null;

            return _mapper.MapToEntity(dt.Rows[0]);
        }
        public List<Producto> ObtenerBanner(int cantidad)
        {
            const string sql = @"
                SELECT TOP (@cantidad)
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, p.categoria_id, 
                    c.categoria_nombre, p.producto_activo, p.condicion_id, p.producto_path_banner 
                FROM Producto p 
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id 
                WHERE p.producto_activo = 1 AND p.producto_es_banner = 1
                ORDER BY p.producto_fecha_creacion DESC";
            var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@cantidad", cantidad)
                };
            var dt = _dao.Read(sql, parametros, CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerDestacados(int cantidad)
        {
            const string sql = @"
                SELECT TOP (@cantidad)
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, p.categoria_id, 
                    c.categoria_nombre, p.condicion_id, p.producto_activo, p.producto_path_banner
                FROM Producto p 
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id 
                WHERE p.producto_activo = 1 AND p.producto_es_promocionado = 1
                ORDER BY p.producto_fecha_creacion DESC";
            var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@cantidad", cantidad)
                };
            var dt = _dao.Read(sql, parametros, CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerUltimosIngresos(int cantidad)
        {
            const string sql = @"
                SELECT TOP (@cantidad)
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, p.categoria_id, 
                    c.categoria_nombre, p.condicion_id, p.producto_activo, p.producto_path_banner 
                FROM Producto p 
                INNER JOIN Categoria c
                ON p.categoria_id = c.categoria_id 
                WHERE p.producto_activo = 1 
                ORDER BY p.producto_fecha_creacion, p.producto_usuario_modificacion DESC";
            var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@cantidad", cantidad)
                };
            var dt = _dao.Read(sql, parametros, CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public int Agregar(Producto producto)
        {
            const string sql = @"
                INSERT INTO Producto
                (producto_nombre, producto_precio, producto_foto,
                    producto_descripcion, producto_stock,
                    categoria_id, condicion_id,
                    producto_activo,
                    producto_fecha_creacion, producto_fecha_modificacion,
                    producto_usuario_creacion)
                VALUES
                (@Nombre, @Precio, @Foto,
                    @Descripcion, @Stock,
                    @CategoriaId, @CondicionId,
                    1,
                    GETDATE(), GETDATE(),
                    @UsuarioCreacion,
                    '')";

            var parametros = new List<SqlParameter>
                {
                    _dao.CreateParameter("@Nombre", producto.Nombre),
                    _dao.CreateParameter("@Precio", producto.Precio),
                    _dao.CreateParameter("@Foto", producto.Foto),
                    _dao.CreateParameter("@Descripcion", producto.Descripcion),
                    _dao.CreateParameter("@Stock", producto.Stock),
                    _dao.CreateParameter("@CategoriaId", producto.CategoriaId),
                    _dao.CreateParameter("@CondicionId", producto.CondicionId),
                    _dao.CreateParameter("@UsuarioCreacion", producto.UsuarioCreacion)
                };

            return _dao.ExecuteNonQuery(sql, parametros, CommandType.Text);
        }


        public int Modificar(Producto producto)
        {
            const string sql = @"
                UPDATE Producto
                SET 
                    producto_nombre = @Nombre,
                    producto_precio = @Precio,
                    producto_foto = @Foto,
                    producto_descripcion = @Descripcion,
                    producto_stock = @Stock,
                    categoria_id = @CategoriaId,
                    condicion_id = @CondicionId,
                    producto_fecha_modificacion = GETDATE(),
                    producto_usuario_modificacion = @UsuarioModificacion
                WHERE producto_id = @Id";

                var parametros = new List<SqlParameter>
                {
                    _dao.CreateParameter("@Id", producto.ProductoId),
                    _dao.CreateParameter("@Nombre", producto.Nombre),
                    _dao.CreateParameter("@Precio", producto.Precio),
                    _dao.CreateParameter("@Foto", producto.Foto),
                    _dao.CreateParameter("@Descripcion", producto.Descripcion),
                    _dao.CreateParameter("@Stock", producto.Stock),
                    _dao.CreateParameter("@CategoriaId", producto.CategoriaId),
                    _dao.CreateParameter("@CondicionId", producto.CondicionId),
                    //_dao.CreateParameter("@UsuarioModificacion", producto.UsuarioModificacion)
                };

            return _dao.ExecuteNonQuery(sql, parametros, CommandType.Text);
        }

        public int Eliminar(int id, string usuarioEliminacion)
        {
            const string sql = @"
                UPDATE Producto
                SET 
                    producto_activo = 0,
                    producto_usuario_eliminacion = @UsuarioEliminacion,
                    producto_fecha_eliminacion = GETDATE()
                WHERE producto_id = @Id";

                    var parametros = new List<SqlParameter>
            {
                _dao.CreateParameter("@Id", id),
                _dao.CreateParameter("@UsuarioEliminacion", usuarioEliminacion)
            };

            return _dao.ExecuteNonQuery(sql, parametros, CommandType.Text);
        }
    }
}
