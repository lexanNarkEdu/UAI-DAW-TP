using BE;
using BE.Permisos;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PermisoBLL
    {
        PermisoDAL dalpermiso;

        public PermisoBLL()
        {
            dalpermiso = new PermisoDAL();
        }

        public bool ExistePermiso(PermisoBE permiso, int id)
        {
            bool existe = false;
            if (permiso.Id.Equals(id))
            {
                existe = true;
            }
            else
            {
                foreach (var item in permiso.ListaDeHijos)
                {
                    existe = ExistePermiso(item, id);
                    if (existe)
                    {
                        return true;
                    }
                }
            }
            return existe;
        }

        public Array BuscarTodosLosPermisos()
        {
            return dalpermiso.TraerTodosLosPermisos();
        }

        public int GuardarPermiso(PermisoBE permiso, bool esrol)
        {
            return dalpermiso.AltaPermiso(permiso, esrol);
        }

        public void GuardarRol(RolBE rol)
        {
            dalpermiso.AltaRol(rol);
        }

        public IList<AccionBE> BuscarTodasLasAcciones()
        {
            return dalpermiso.TraerTodasLasAcciones();
        }

        public IList<RolBE> BuscarTodosLosRoles()
        {
            return dalpermiso.TraerTodosLosRoles();
        }

        public IList<PermisoBE> BuscarTodo(string familia)
        {
            return dalpermiso.TraerTodo(familia);
        }

        public void LlenarUsuarioPermisos(UsuarioBE usuario)
        {
            dalpermiso.LlenarUsuarioPermisos(usuario);
        }

        public void LlenarRolPermisos(RolBE rol)
        {
            dalpermiso.LlenarRolPermisos(rol);
        }
    }
}
