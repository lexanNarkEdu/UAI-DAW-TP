using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    /// <summary>
    /// Representa un permiso compuesto (rol que agrupa otros permisos) - Composite del patrón
    /// </summary>
    public class PermisoCompuesto : IComponentePermiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }

        private readonly List<IComponentePermiso> _permisos;

        public PermisoCompuesto()
        {
            _permisos = new List<IComponentePermiso>();
        }

        public PermisoCompuesto(int id, string nombre, string tipo) : this()
        {
            Id = id;
            Nombre = nombre;
            Tipo = tipo;
        }

        /// <summary>
        /// Obtiene todos los permisos incluyendo este y todos sus hijos recursivamente
        /// </summary>
        public List<IComponentePermiso> ObtenerTodosLosPermisos()
        {
            var todosLosPermisos = new List<IComponentePermiso> { this };

            foreach (var permiso in _permisos)
            {
                var permisosHijos = permiso.ObtenerTodosLosPermisos();
                foreach (var permisoHijo in permisosHijos)
                {
                    // Evitar duplicados
                    if (!todosLosPermisos.Any(p => p.Id == permisoHijo.Id))
                    {
                        todosLosPermisos.Add(permisoHijo);
                    }
                }
            }

            return todosLosPermisos;
        }

        /// <summary>
        /// Verifica si este permiso o alguno de sus hijos tiene el permiso buscado
        /// </summary>
        public bool TienePermiso(string nombrePermiso)
        {
            // Verificar si este permiso es el buscado
            if (string.Equals(Nombre, nombrePermiso, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Verificar en los permisos hijos
            return _permisos.Any(permiso => permiso.TienePermiso(nombrePermiso));
        }

        /// <summary>
        /// Agrega un permiso hijo al composite
        /// </summary>
        public void AgregarPermiso(IComponentePermiso permiso)
        {
            if (permiso == null)
                throw new ArgumentNullException(nameof(permiso));

            if (permiso.Id == this.Id)
                throw new InvalidOperationException("Un permiso no puede contenerse a sí mismo");

            // Evitar duplicados
            if (!_permisos.Any(p => p.Id == permiso.Id))
            {
                _permisos.Add(permiso);
            }
        }

        /// <summary>
        /// Remueve un permiso hijo del composite
        /// </summary>
        public void RemoverPermiso(IComponentePermiso permiso)
        {
            if (permiso == null)
                return;

            _permisos.RemoveAll(p => p.Id == permiso.Id);
        }

        /// <summary>
        /// Obtiene los permisos hijos directos
        /// </summary>
        public List<IComponentePermiso> ObtenerPermisosHijos()
        {
            return new List<IComponentePermiso>(_permisos);
        }

        /// <summary>
        /// Obtiene solo los permisos simples (acciones) de todo el árbol
        /// </summary>
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

        public override string ToString()
        {
            return $"PermisoCompuesto: {Nombre} (Tipo: {Tipo}, Hijos: {_permisos.Count})";
        }

        public override bool Equals(object obj)
        {
            if (obj is IComponentePermiso otroPermiso)
            {
                return Id == otroPermiso.Id && string.Equals(Nombre, otroPermiso.Nombre, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (Nombre?.GetHashCode() ?? 0);
        }
    }
}