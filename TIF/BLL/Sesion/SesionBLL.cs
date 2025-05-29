using BE;
using BE.Sesion;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SesionBLL
    {
        UsuarioDAL usuariodal = new UsuarioDAL();

        public LoginResultadoENUMBE LogIn(string nombredeusuario, string contrasenia)
        {
            if (SesionBE.ObtenerInstancia.Logueado())
            {
                throw new LoginExcepcionBE(LoginResultadoENUMBE.YaHayUnUsuarioLogueado);
            }
            else
            {
                var usuario = usuariodal.buscar(new UsuarioBE { Username = nombredeusuario, Password = contrasenia });
                if (usuario.Count != 0)
                {
                    UsuarioBE usuarioaux = usuario[0];
                    if (usuarioaux.Password == contrasenia)
                    {
                        new PermisoDAL().LlenarUsuarioPermisos(usuarioaux);
                        SesionBE.ObtenerInstancia.LogIn(usuarioaux);
                        return LoginResultadoENUMBE.LogInCorrecto;
                    }
                    else
                    {
                        throw new LoginExcepcionBE(LoginResultadoENUMBE.ContraseniaIncorrecta);
                    }
                }
                else
                {
                    throw new LoginExcepcionBE(LoginResultadoENUMBE.NombreDeUsuarioIncorrecto);
                }
            }
        }

        public void LogOut()
        {
            if (SesionBE.ObtenerInstancia.Logueado())
            {
                SesionBE.ObtenerInstancia.LogOut();
            }
            else
            {
                throw new Exception("No hay un usuario logueado");
            }
        }
    }
}
