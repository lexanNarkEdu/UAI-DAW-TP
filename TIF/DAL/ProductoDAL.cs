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
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, categoria_id, 
                    p.condicion_id, producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion, p.producto_verificador_horizontal 
                FROM Producto p 
                ORDER BY p.producto_fecha_creacion DESC";

            DataTable dt = _dao.Read(sql, commandType: CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerTodosActivos()
        {
            const string sql = @"
                SELECT 
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, categoria_id, 
                    p.condicion_id, producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion, p.producto_verificador_horizontal 
                FROM Producto p 
                WHERE p.producto_activo = 1 
                ORDER BY p.producto_fecha_creacion DESC";

            DataTable dt = _dao.Read(sql, commandType: CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public List<Producto> ObtenerPorCategoria(int categoriaId)
        {
            const string sql = @"
              SELECT 
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, categoria_id, 
                    p.condicion_id, p.producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion, p.producto_verificador_horizontal 
              FROM Producto p 
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

        public List<Producto> ObtenerPorCategoriaYCondicion(int? categoriaId, int? condicionId)
        {
            var sql = new StringBuilder(@"
                SELECT
                    p.producto_id, p.producto_nombre, p.producto_precio, p.producto_foto, p.producto_descripcion, p.producto_stock, categoria_id, 
                    p.condicion_id, p.producto_activo, p.producto_fecha_creacion, p.producto_usuario_creacion, p.producto_verificador_horizontal 
                FROM Producto p
                WHERE p.producto_activo = 1 ");

            var parametros = new List<SqlParameter>();

            if (categoriaId.HasValue)
            {
                sql.Append(" AND p.categoria_id = @CategoriaId");
                parametros.Add(new SqlParameter("@CategoriaId", categoriaId.Value));
            }
            if (condicionId.HasValue)
            {
                sql.Append(" AND p.condicion_id = @CondicionId");
                parametros.Add(new SqlParameter("@CondicionId", condicionId.Value));
            }

            sql.Append(" ORDER BY p.producto_fecha_creacion DESC");

            var dt = _dao.Read(sql.ToString(), parametros, CommandType.Text);
            return _mapper.MapAll(dt).ToList();
        }

        public int Agregar(Producto p)
        {
            var sql = @"
                INSERT INTO Producto
                (producto_nombre, producto_precio, producto_foto,
                    producto_descripcion, producto_stock,
                    categoria_id, condicion_id,
                    producto_activo,
                    producto_fecha_creacion, producto_fecha_modificacion,
                    producto_usuario_creacion,
                    producto_verificador_horizontal)
                VALUES
                (@Nombre, @Precio, @Foto,
                    @Descripcion, @Stock,
                    @CategoriaId, @CondicionId,
                    1,
                    GETDATE(), GETDATE(),
                    @UsuarioCreacion,
                    '')";  // ajusta DVH según tu lógica
            int result = _dao.Write(sql,
                new List<SqlParameter>
                {
                _dao.CreateParameter("@Nombre", p.Nombre),
                _dao.CreateParameter("@Precio", p.Precio),
                _dao.CreateParameter("@Foto", p.Foto),
                _dao.CreateParameter("@Descripcion", p.Descripcion),
                _dao.CreateParameter("@Stock", p.Stock),
                _dao.CreateParameter("@CategoriaId", p.CategoriaId),
                _dao.CreateParameter("@CondicionId", p.CondicionId),
                _dao.CreateParameter("@UsuarioCreacion", p.UsuarioCreacion)
                },
                CommandType.Text);

            return result;

        }
    }
}
