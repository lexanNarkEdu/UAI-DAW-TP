using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Permisos
{
    public enum PermisoENUMBE
    {
        //Acá no van roles, van acciones
        SinPermisos,
        ABMProductos,
        AgregarProducto,
        BajarProducto,
        ModificarProducto,
        ABMUsuarios,
        GestionarBitacoraEventos,
        GestionarBitacoraCambios,
        GestionarBackup,
        VerProductos,
    }
}
