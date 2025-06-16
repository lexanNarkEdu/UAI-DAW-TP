using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Permisos
{
    public abstract class PermisoBE
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public PermisoENUMBE PermisoTipoEnum { get; set; }

        public abstract IList<PermisoBE> ListaDeHijos { get; }
        public abstract void AgregarHijo(PermisoBE bepermiso);
        public abstract void VaciarHijos();

        public override string ToString()
        {
            return Nombre;
        }
    }
}
