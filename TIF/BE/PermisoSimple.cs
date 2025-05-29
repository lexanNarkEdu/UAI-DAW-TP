using System;
using System.Collections.Generic;

namespace BE
{
    /// <summary>
    /// Representa un permiso simple (acción específica) - Hoja del patrón Composite
    /// </summary>
    public class PermisoSimple : IComponentePermiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }

        public PermisoSimple()
        {
        }

        public PermisoSimple(int id, string nombre, string tipo)
        {
            Id = id;
            Nombre = nombre;
            Tipo = tipo;
        }

        /// <summary>
        /// Un permiso simple solo se retorna a sí mismo
        /// </summary>
        public List<IComponentePermiso> ObtenerTodosLosPermisos()
        {
            return new List<IComponentePermiso> { this };
        }

        /// <summary>
        /// Verifica si este permiso coincide con el nombre buscado
        /// </summary>
        public bool TienePermiso(string nombrePermiso)
        {
            return string.Equals(Nombre, nombrePermiso, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Los permisos simples no pueden tener hijos
        /// </summary>
        public void AgregarPermiso(IComponentePermiso permiso)
        {
            throw new InvalidOperationException("Un permiso simple no puede contener otros permisos");
        }

        /// <summary>
        /// Los permisos simples no pueden tener hijos
        /// </summary>
        public void RemoverPermiso(IComponentePermiso permiso)
        {
            throw new InvalidOperationException("Un permiso simple no puede contener otros permisos");
        }

        public override string ToString()
        {
            return $"PermisoSimple: {Nombre} (Tipo: {Tipo})";
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