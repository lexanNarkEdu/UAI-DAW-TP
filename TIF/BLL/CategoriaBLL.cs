using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class CategoriaBLL
    {
        private readonly CategoriaDAL _dal = new CategoriaDAL();
        public List<Categoria> ObtenerTodas() => _dal.ObtenerTodas();
    }
}
