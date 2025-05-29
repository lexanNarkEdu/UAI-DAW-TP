using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
        public string Username { get; }
        public string Password { get; }
        public string Nombre { get; }
        public string Apellido { get; }
        public int Dni { get; }
        public string Email { get; }
        public string Domicilio { get; }
        public int FallosAutenticacionConsecutivos { get; set; }
        public bool Bloqueado { get; set; }


        private List<Rol> Roles { get; }


        public Usuario(string username)
        {
            this.Username = username;
            this.Roles = new List<Rol>();
        }

        public Usuario(string username, string password, string nombre, string apellido, int dni, string email, string domicilio, int fallosAutenticacionConsecutivos = 0, bool bloqueado = false)
        {
            this.Username = username;
            this.Password = password;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Dni = dni;
            this.Email = email;
            this.Domicilio = domicilio;
            this.FallosAutenticacionConsecutivos = fallosAutenticacionConsecutivos;
            this.Bloqueado = bloqueado;

            this.Roles = new List<Rol>();
        }

    }
}