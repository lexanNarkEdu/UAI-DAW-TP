using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class UsuarioBLL
    {
        public BE.Usuario obtener(string username) {
            return UsuarioDAL.obtener(username);
        }


    }

}
