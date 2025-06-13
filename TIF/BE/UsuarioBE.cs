using BE.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class UsuarioBE
    {
        public UsuarioBE()
        {
            this.listaDePermisos = new List<PermisoBE>();
        }

        public UsuarioBE(string username)
        {
            this.Username = username;
            this.listaDePermisos = new List<PermisoBE>();
        }

        public UsuarioBE(string username, string password, string nombre, string apellido, int dni, string email, string domicilio, int fallosAutenticacionConsecutivos = 0, bool bloqueado = false)
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

            this.listaDePermisos = new List<PermisoBE>();
        }

        public string Username { get; set; } //Funciona como identificador único (Id)
        public string Password { get; set; }
        public string Nombre { get; }
        public string Apellido { get; }
        public int Dni { get; }
        public string Email { get; }
        public string Domicilio { get; }   
        public int FallosAutenticacionConsecutivos { get; set; }
        public bool Bloqueado { get; set; }

        private List<PermisoBE> listaDePermisos;
        public IList<PermisoBE> ListaDePermisos //Método público para acceder a la lista de permisos privada
        {
            get { return listaDePermisos; }
        }
    }
}