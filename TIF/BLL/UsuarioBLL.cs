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
        public Usuario validarUsuario(string username, string password) {
            return UsuarioDAL.validarUsuario(username, password);
        }


    }

}
