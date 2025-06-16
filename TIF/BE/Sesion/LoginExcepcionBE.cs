using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Sesion
{
    public class LoginExcepcionBE : Exception
    {
        public LoginResultadoENUMBE Resultado;

        public LoginExcepcionBE(LoginResultadoENUMBE resultado)
        {
            Resultado = resultado;
        }
    }
}
