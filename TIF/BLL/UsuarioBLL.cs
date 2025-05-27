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
        public Usuario obtenerUsuario(string username) {
            return UsuarioDAL.obtenerUsuario(username);
        }

        public void loginInvalido(Usuario usuario)
        {
            usuario.FallosAutenticacionConsecutivos++;
            if (usuario.FallosAutenticacionConsecutivos >= 3)
            {
                usuario.Bloqueado = true;
            }
            
            UsuarioDAL.loginInvalido(usuario);
        }

        public void loginValido(Usuario usuario)
        {
            usuario.FallosAutenticacionConsecutivos = 0;
            
            UsuarioDAL.loginValido(usuario);
        }


    }

}
