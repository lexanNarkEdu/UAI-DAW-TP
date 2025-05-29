using System;
using System.Collections.Generic;
using System.Linq;

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
        public DateTime FechaCreacion { get; set; }

        private readonly List<IComponentePermiso> _permisos;

        public Usuario(string username)
        {
            this.Username = username;
            this._permisos = new List<IComponentePermiso>();
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

            this._permisos = new List<IComponentePermiso>();
        }

        /// <summary>
        /// Agrega un permiso al usuario
        /// </summary>
        /// <param name="permiso">Permiso a agregar</param>
        public void AgregarPermiso(IComponentePermiso permiso)
        {
            if (permiso == null)
                throw new ArgumentNullException(nameof(permiso));

            // Evitar duplicados
            if (!_permisos.Any(p => p.Id == permiso.Id))
            {
                _permisos.Add(permiso);
            }
        }

        /// <summary>
        /// Remueve un permiso del usuario
        /// </summary>
        /// <param name="permiso">Permiso a remover</param>
        public void RemoverPermiso(IComponentePermiso permiso)
        {
            if (permiso == null)
                return;

            _permisos.RemoveAll(p => p.Id == permiso.Id);
        }

        /// <summary>
        /// Obtiene todos los permisos del usuario (incluyendo los heredados de roles)
        /// </summary>
        /// <returns>Lista de todos los permisos efectivos</returns>
        public List<IComponentePermiso> ObtenerTodosLosPermisos()
        {
            var todosLosPermisos = new List<IComponentePermiso>();

            foreach (var permiso in _permisos)
            {
                var permisosExpandidos = permiso.ObtenerTodosLosPermisos();
                foreach (var permisoExpandido in permisosExpandidos)
                {
                    // Evitar duplicados
                    if (!todosLosPermisos.Any(p => p.Id == permisoExpandido.Id))
                    {
                        todosLosPermisos.Add(permisoExpandido);
                    }
                }
            }

            return todosLosPermisos;
        }

        /// <summary>
        /// Verifica si el usuario tiene un permiso específico
        /// </summary>
        /// <param name="nombrePermiso">Nombre del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso</returns>
        public bool TienePermiso(string nombrePermiso)
        {
            if (string.IsNullOrEmpty(nombrePermiso))
                return false;

            return _permisos.Any(permiso => permiso.TienePermiso(nombrePermiso));
        }

        /// <summary>
        /// Obtiene solo los permisos simples (acciones) del usuario
        /// </summary>
        /// <returns>Lista de permisos simples</returns>
        public List<PermisoSimple> ObtenerPermisosSimples()
        {
            var permisosSimples = new List<PermisoSimple>();

            foreach (var permiso in _permisos)
            {
                if (permiso is PermisoSimple permisoSimple)
                {
                    permisosSimples.Add(permisoSimple);
                }
                else if (permiso is PermisoCompuesto permisoCompuesto)
                {
                    permisosSimples.AddRange(permisoCompuesto.ObtenerPermisosSimples());
                }
            }

            return permisosSimples.Distinct().ToList();
        }

        /// <summary>
        /// Obtiene los roles (permisos compuestos) del usuario
        /// </summary>
        /// <returns>Lista de roles</returns>
        public List<PermisoCompuesto> ObtenerRoles()
        {
            return _permisos.OfType<PermisoCompuesto>().ToList();
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico
        /// </summary>
        /// <param name="nombreRol">Nombre del rol a verificar</param>
        /// <returns>True si el usuario tiene el rol</returns>
        public bool TieneRol(string nombreRol)
        {
            return ObtenerRoles().Any(rol => string.Equals(rol.Nombre, nombreRol, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Limpia todos los permisos del usuario
        /// </summary>
        public void LimpiarPermisos()
        {
            _permisos.Clear();
        }

        /// <summary>
        /// Obtiene los permisos directos del usuario (sin expandir)
        /// </summary>
        /// <returns>Lista de permisos directos</returns>
        public List<IComponentePermiso> ObtenerPermisosDirectos()
        {
            return new List<IComponentePermiso>(_permisos);
        }

        public override string ToString()
        {
            return $"{Nombre} {Apellido} ({Username})";
        }
    }
}