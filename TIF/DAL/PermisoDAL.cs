using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;

namespace DAL
{
    public class PermisoDAL
    {
        private readonly DAO _dao;

        public PermisoDAL()
        {
            _dao = DAO.GetDAO();
        }

        /// <summary>
        /// Obtiene un permiso por su ID
        /// </summary>
        /// <param name="permisoId">ID del permiso</param>
        /// <returns>Permiso encontrado o null</returns>
        public IComponentePermiso ObtenerPorId(int permisoId)
        {
            var sql = "SELECT permiso_id, permiso_nombre, permiso_tipo FROM Permiso WHERE permiso_id = @permisoId";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@permisoId", permisoId)
                },
                CommandType.Text);

            if (dataTable.Rows.Count > 0)
            {
                return MapearPermiso(dataTable.Rows[0]);
            }

            return null;
        }

        /// <summary>
        /// Obtiene todos los permisos
        /// </summary>
        /// <returns>Lista de todos los permisos</returns>
        public List<IComponentePermiso> ObtenerTodos()
        {
            var sql = "SELECT permiso_id, permiso_nombre, permiso_tipo FROM Permiso ORDER BY permiso_nombre";

            var dataTable = _dao.Read(sql, commandType: CommandType.Text);
            var permisos = new List<IComponentePermiso>();

            foreach (DataRow row in dataTable.Rows)
            {
                permisos.Add(MapearPermiso(row));
            }

            return permisos;
        }

        /// <summary>
        /// Obtiene los permisos de un usuario específico con su estructura jerárquica completa
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de permisos con estructura completa</returns>
        public List<IComponentePermiso> ObtenerPermisosPorUsuario(string username)
        {
            var sql = @"
                SELECT DISTINCT 
                    p.permiso_id, 
                    p.permiso_nombre, 
                    p.permiso_tipo 
                FROM Permiso p
                INNER JOIN Usuario_Permiso up ON p.permiso_id = up.permiso_id
                WHERE up.usuario_username = @username
                ORDER BY p.permiso_nombre";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@username", username)
                },
                CommandType.Text);

            var permisosDirectos = new List<IComponentePermiso>();

            foreach (DataRow row in dataTable.Rows)
            {
                var permiso = MapearPermiso(row);

                // Si es un permiso compuesto, cargar sus hijos
                if (permiso is PermisoCompuesto permisoCompuesto)
                {
                    CargarPermisosHijos(permisoCompuesto);
                }

                permisosDirectos.Add(permiso);
            }

            return permisosDirectos;
        }

        /// <summary>
        /// Obtiene los permisos hijos de un permiso compuesto
        /// </summary>
        /// <param name="permisoId">ID del permiso padre</param>
        /// <returns>Lista de permisos hijos</returns>
        public List<IComponentePermiso> ObtenerPermisosHijos(int permisoId)
        {
            var sql = @"
                SELECT 
                    p.permiso_id, 
                    p.permiso_nombre, 
                    p.permiso_tipo 
                FROM Permiso p
                INNER JOIN Permiso_Permiso pp ON p.permiso_id = pp.permisohijo_id
                WHERE pp.permisopadre_id = @permisoId
                ORDER BY p.permiso_nombre";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@permisoId", permisoId)
                },
                CommandType.Text);

            var permisosHijos = new List<IComponentePermiso>();

            foreach (DataRow row in dataTable.Rows)
            {
                var permiso = MapearPermiso(row);

                // Si el hijo también es compuesto, cargar sus hijos recursivamente
                if (permiso is PermisoCompuesto permisoCompuesto)
                {
                    CargarPermisosHijos(permisoCompuesto);
                }

                permisosHijos.Add(permiso);
            }

            return permisosHijos;
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="nombrePermiso">Nombre del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso</returns>
        public bool UsuarioTienePermiso(string username, string nombrePermiso)
        {
            var permisos = ObtenerPermisosPorUsuario(username);
            return permisos.Any(p => p.TienePermiso(nombrePermiso));
        }

        /// <summary>
        /// Carga recursivamente los permisos hijos de un permiso compuesto
        /// </summary>
        /// <param name="permisoCompuesto">Permiso compuesto al que cargar los hijos</param>
        private void CargarPermisosHijos(PermisoCompuesto permisoCompuesto)
        {
            var permisosHijos = ObtenerPermisosHijos(permisoCompuesto.Id);

            foreach (var permisoHijo in permisosHijos)
            {
                permisoCompuesto.AgregarPermiso(permisoHijo);
            }
        }

        /// <summary>
        /// Mapea una fila de datos a un objeto permiso
        /// </summary>
        /// <param name="row">Fila de datos</param>
        /// <returns>Permiso mapeado</returns>
        private IComponentePermiso MapearPermiso(DataRow row)
        {
            int id = Convert.ToInt32(row["permiso_id"]);
            string nombre = row["permiso_nombre"].ToString();
            string tipo = row["permiso_tipo"]?.ToString();

            // Determinar si es simple o compuesto basado en el tipo
            // Si tipo es null o "PERMISO", es un rol (compuesto)
            // Si tipo es "Accion", es un permiso simple
            if (string.IsNullOrEmpty(tipo) || tipo.Equals("PERMISO", StringComparison.OrdinalIgnoreCase))
            {
                return new PermisoCompuesto(id, nombre, tipo);
            }
            else
            {
                return new PermisoSimple(id, nombre, tipo);
            }
        }

        /// <summary>
        /// Obtiene todos los permisos efectivos de un usuario (incluyendo herencia)
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de todos los permisos efectivos</returns>
        public List<IComponentePermiso> ObtenerTodosLosPermisosEfectivos(string username)
        {
            var permisosDirectos = ObtenerPermisosPorUsuario(username);
            var todosLosPermisos = new List<IComponentePermiso>();

            foreach (var permiso in permisosDirectos)
            {
                var permisosExpandidos = permiso.ObtenerTodosLosPermisos();
                foreach (var permisoExpandido in permisosExpandidos)
                {
                    if (!todosLosPermisos.Any(p => p.Id == permisoExpandido.Id))
                    {
                        todosLosPermisos.Add(permisoExpandido);
                    }
                }
            }

            return todosLosPermisos;
        }

        /// <summary>
        /// Obtiene solo los permisos simples (acciones) de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de permisos simples</returns>
        public List<PermisoSimple> ObtenerPermisosSimplesPorUsuario(string username)
        {
            var todosLosPermisos = ObtenerTodosLosPermisosEfectivos(username);
            return todosLosPermisos.OfType<PermisoSimple>().ToList();
        }
    }
}