using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Permisos
{
    public class AccionBE: PermisoBE
    {        
        //También conocido como PATENTE

        public override IList<PermisoBE> ListaDeHijos
        {
            get
            {
                return new List<PermisoBE>();
            }
        }

        public override void AgregarHijo(PermisoBE bepermiso)
        {
            throw new NotImplementedException();
        }

        public override void VaciarHijos()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
