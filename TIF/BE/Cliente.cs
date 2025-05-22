using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Cliente : Usuario
    {
        private int Telefono { get; }
        private string Domicilio { get; }   

        public Cliente (string username) : base(username)
        {

        }

        public Cliente(string username, string nombre, string apellido, int dni, string email, int telefono, string domicilio, int id = 0, int fallosAutenticacionConsecutivos = 0, bool bloqueado = false) : base(username, nombre, apellido, dni, email, id, fallosAutenticacionConsecutivos, bloqueado)
        {
            this.Telefono = telefono;
            this.Domicilio = domicilio; 
        }
    }
}
