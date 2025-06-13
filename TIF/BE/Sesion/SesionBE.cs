using BE.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Sesion
{
    public class SesionBE
    {
        //Esta clase me sirve para guardar el usuario logueado y verificar que solo haya una instancia

        private static SesionBE instancia;
        private static Object bloqueo = new Object();

        private UsuarioBE usuario;
        public UsuarioBE Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public static SesionBE ObtenerInstancia
        {
            get
            {
                lock (bloqueo)
                {
                    if (instancia == null)
                    {
                        instancia = new SesionBE();
                    }
                }
                return instancia;
            }
        }

        bool estaEnElRol(PermisoBE permiso, PermisoENUMBE permisotipoenum, bool existe)
        {
            if (permiso.PermisoTipoEnum.Equals(permisotipoenum))
            {
                existe = true;
            }
            else
            {
                foreach (var hijo in permiso.ListaDeHijos)
                {
                    existe = estaEnElRol(hijo, permisotipoenum, existe);
                    if (existe)
                    {
                        return true;
                    }
                }
            }
            return existe;
        }

        public bool EstaEnElRol(PermisoENUMBE permisotipoenum)
        {
            if (Usuario == null)
            {
                return false;
            }
            bool existe = false;
            foreach (var item in Usuario.ListaDePermisos)
            {
                if (item.PermisoTipoEnum.Equals(permisotipoenum))
                {
                    return true;
                }
                else
                {
                    existe = estaEnElRol(item, permisotipoenum, existe);
                    if (existe)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void LogIn(UsuarioBE usuario)
        {
            Usuario = usuario;
        }

        public void LogOut()
        {
            Usuario = null;
        }

        public bool Logueado()
        {
            return Usuario != null;
        }
    }
}
