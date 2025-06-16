using BE.Permisos;
using BE;
using System.Collections.Generic;

namespace BLL
{
    public class RoLBLL
    {
        bool _estaPermisoEnRol(PermisoBE permiso, PermisoENUMBE permisotipoenum, bool existe)
        {
            if (permiso.PermisoTipoEnum.Equals(permisotipoenum))
            {
                existe = true;
            }
            else
            {
                foreach (var hijo in permiso.ListaDeHijos)
                {
                    existe = _estaPermisoEnRol(hijo, permisotipoenum, existe);
                    if (existe)
                    {
                        return true;
                    }
                }
            }
            return existe;
        }

        public bool EstaPermisoEnRol(List<PermisoBE> listadoDePermisos, PermisoENUMBE permisotipoenum)
        {
            bool existe = false;

            foreach (var item in listadoDePermisos)
            {
                if (item.PermisoTipoEnum.Equals(permisotipoenum))
                {
                    return true;
                }
                else
                {
                    existe = _estaPermisoEnRol(item, permisotipoenum, existe);
                    if (existe)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
