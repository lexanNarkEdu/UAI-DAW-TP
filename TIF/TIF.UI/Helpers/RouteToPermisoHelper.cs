using BE.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TIF.UI.Helpers
{
    public class PermisoToRouteHelper
    {
        public static PermisoENUMBE ToPermiso(string path)
        {
            var subPath = path.Split('~').Last();

            switch (subPath)
            {
                case "/Productos":
                    return PermisoENUMBE.VerProductos;
                case "/ABMUsuarios":
                    return PermisoENUMBE.ABMUsuarios;
                case "/ABMProductos":
                    return PermisoENUMBE.ABMProductos;
                case "/BajaProducto":
                    return PermisoENUMBE.BajarProducto;
                case "/Bitacora":
                    return PermisoENUMBE.GestionarBitacoraEventos;
                case "/Backup":
                    return PermisoENUMBE.GestionarBackup;
                default:
                    throw new NotSupportedException("No existe permiso especificado para dicha ruta");
            }
        }
    }
}