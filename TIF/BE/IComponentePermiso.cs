using System.Collections.Generic;

namespace BE
{
    /// <summary>
    /// Interfaz base para el patrón Composite de permisos
    /// </summary>
    public interface IComponentePermiso
    {
        int Id { get; set; }
        string Nombre { get; set; }
        string Tipo { get; set; }

        /// <summary>
        /// Obtiene todos los permisos, incluyendo los hijos en caso de ser compuesto
        /// </summary>
        /// <returns>Lista de todos los permisos</returns>
        List<IComponentePermiso> ObtenerTodosLosPermisos();

        /// <summary>
        /// Verifica si contiene un permiso específico
        /// </summary>
        /// <param name="nombrePermiso">Nombre del permiso a verificar</param>
        /// <returns>True si contiene el permiso</returns>
        bool TienePermiso(string nombrePermiso);

        /// <summary>
        /// Agrega un permiso hijo (solo para permisos compuestos)
        /// </summary>
        /// <param name="permiso">Permiso a agregar</param>
        void AgregarPermiso(IComponentePermiso permiso);

        /// <summary>
        /// Remueve un permiso hijo (solo para permisos compuestos)
        /// </summary>
        /// <param name="permiso">Permiso a remover</param>
        void RemoverPermiso(IComponentePermiso permiso);
    }
}