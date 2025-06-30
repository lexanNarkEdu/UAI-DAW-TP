using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL.Mappers;

namespace DAL
{
    public class CategoriaDAL
    {
        private readonly DAO _dao = DAO.GetDAO();
        private readonly CategoriaMapper _m = new CategoriaMapper();
        public List<Categoria> ObtenerTodas()
        {
            var dt = _dao.Read("SELECT c.categoria_id, c.categoria_nombre FROM Categoria c WHERE c.categoria_activo = 1", null, CommandType.Text);
            return _m.MapAll(dt).ToList();
        }
    }
}
