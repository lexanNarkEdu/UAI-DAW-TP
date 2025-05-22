using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class Usuario
    {
        private int id { get; } 
        private string Username { get; }
        private string Nombre { get; }
        private string Apellido { get; }
        private int Dni { get; }
        private string Email { get; }
        private int FallosAutenticacionConsecutivos { get; }
        private bool Bloqueado { get; } 

        private List<Rol> Roles { get; }


        public Usuario(string username) {
            this.Username = username;
            this.Roles = new List<Rol>();
        }

        public Usuario(string username, string nombre, string apellido, int dni, string email, int id = 0, int fallosAutenticacionConsecutivos = 0, bool bloqueado = false)
        {
            this.id = id;
            this.Username = username;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Dni = dni;
            this.Email = email;
            this.FallosAutenticacionConsecutivos = fallosAutenticacionConsecutivos;
            this.Bloqueado = bloqueado;

            this.Roles = new List<Rol>();
        }

    }
}
