using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Permisos
{
    public class RolBE : PermisoBE
    {
        //También conocido como FAMILIA

        private IList<PermisoBE> listaDeHijos;

        public RolBE()
        {
            listaDeHijos = new List<PermisoBE>();
        }

        public override IList<PermisoBE> ListaDeHijos
        {
            get
            {
                return listaDeHijos.ToArray();
            }
        }

        public override void AgregarHijo(PermisoBE bepermiso)
        {
            listaDeHijos.Add(bepermiso);
        }

        public override void VaciarHijos()
        {
            listaDeHijos.Clear();
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
