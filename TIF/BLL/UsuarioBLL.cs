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
        UsuarioDAL usuariodal = new UsuarioDAL();

        public UsuarioBE obtenerUsuario(string username) {
            return usuariodal.obtenerUsuarioConUsername(username);
        }

        public void loginInvalido(UsuarioBE usuario)
        {
            usuario.FallosAutenticacionConsecutivos++;
            if (usuario.FallosAutenticacionConsecutivos >= 3)
            {
                usuario.Bloqueado = true;
            }

            usuariodal.loginInvalido(usuario);
        }

        public void loginValido(UsuarioBE usuario)
        {
            usuario.FallosAutenticacionConsecutivos = 0;

            usuariodal.loginValido(usuario);
        }
    }
}
