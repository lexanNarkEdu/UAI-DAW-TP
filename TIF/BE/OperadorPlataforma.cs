using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class OperadorPlataforma : Usuario
    {
       
        public OperadorPlataforma(string username) : base(username)
        {
        }

        public OperadorPlataforma(string username, string nombre, string apellido, int dni, string email, int id = 0, int fallosAutenticacionConsecutivos = 0, bool bloqueado = false) : base(username, nombre, apellido, dni, email, id, fallosAutenticacionConsecutivos, bloqueado)
        {
        }
    }
}
