using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BE.Permisos;

namespace DAL
{
    public class PermisoDAL
    {
        internal AccesoBDDAL acceso = new AccesoBDDAL();

        public Array TraerTodosLosPermisos()
        {
            return Enum.GetValues(typeof(PermisoENUMBE));
        }

        public int AltaPermiso(PermisoBE permiso, bool esrol)
        {
            string commandText = "";

            if (esrol)
            {
                commandText = $"INSERT INTO Permiso VALUES ('{permiso.Nombre}', NULL";
            }
            else
            {
                commandText = $"INSERT INTO Permiso VALUES ('{permiso.Nombre}', Accion"; ;
            }

            acceso.AbrirConexion();
            int res = acceso.Escribir(commandText, null, false);
            acceso.CerrarConexion();

            return res;
        }
                
        public void AltaRol(RolBE rol)
        {
            acceso.AbrirConexion();

            List<SqlParameter> parametros = new List<SqlParameter>();
            SqlParameter p = acceso.CrearParametro("@id", rol.Id);
            parametros.Add(p);

            int res = acceso.Escribir("PERMISO_PERMISO_BORRAR", parametros);
            //DELETE FROM Permiso_Permiso WHERE permisopadre_id = @id

            List<SqlParameter> parametros2 = new List<SqlParameter>();
            foreach (var hijo in rol.ListaDeHijos)
            {
                SqlParameter p2 = acceso.CrearParametro("@permisopadre_id", rol.Id);
                parametros2.Add(p2);
                p2 = acceso.CrearParametro("@permisohijo_id", hijo.Id);
                parametros2.Add(p2);

                res = acceso.Escribir("PERMISO_PERMISO_INSERTAR", parametros2);
                parametros2.Clear();
                //INSERT INTO Permiso_Permiso VALUES (@permisopadre_id, @permisohijo_id) 
            }

            acceso.CerrarConexion();
        }
        
        //Permiso = Acciones
        public IList<PermisoBE> TraerTodo(string rol)
        {
            acceso.AbrirConexion();

            //var where = "IS NULL";
            var where = "NULL";
            if (!string.IsNullOrEmpty(rol))
            {
                where = rol;
            }

            IList<PermisoBE> listadepermisos = new List<PermisoBE>();

            string commandText = $@"
                WITH recursivo AS (
                    SELECT PP2.permisopadre_id, PP2.permisohijo_id FROM Permiso_Permiso PP2
                    WHERE PP2.permisopadre_id = {where} --acá se va variando la familia que busco
                    UNION ALL 
                    SELECT PP1.permisopadre_id, PP1.permisohijo_id FROM Permiso_Permiso PP1 
                    INNER JOIN recursivo rec ON rec.permisohijo_id = PP1.permisopadre_id
                )
                SELECT rec.permisopadre_id, rec.permisohijo_id, P.permiso_id, P.permiso_nombre, P.permiso_tipo
                FROM recursivo rec 
                INNER JOIN Permiso P ON rec.permisohijo_id = P.permiso_id";
            
            DataTable tabla = acceso.Leer(commandText, null, false);
            foreach (DataRow dr in tabla.Rows)
            {
                int idpadre = 0;
                if (dr["permisopadre_id"] != DBNull.Value)
                {
                    idpadre = Convert.ToInt32(dr["permisopadre_id"]);
                }

                var id = int.Parse(dr["permiso_id"].ToString());
                var permiso_nombre = dr["permiso_nombre"].ToString();

                var permiso_tipo = string.Empty;
                if (dr["permiso_tipo"] != DBNull.Value)
                {
                    permiso_tipo = dr["permiso_tipo"].ToString();
                }

                PermisoBE permisoaux;

                if (string.IsNullOrEmpty(permiso_tipo))
                {
                    permisoaux = new RolBE();
                }
                else
                {
                    permisoaux = new AccionBE();
                }

                permisoaux.Id = id;
                permisoaux.Nombre = permiso_nombre;
                if (!string.IsNullOrEmpty(permiso_tipo))
                {
                    permisoaux.PermisoTipoEnum = (PermisoENUMBE)Enum.Parse(typeof(PermisoENUMBE), permiso_nombre);
                }

                var padre = TraerUnPermiso(idpadre, listadepermisos);

                if (padre == null)
                {
                    listadepermisos.Add(permisoaux);
                }
                else
                {
                    padre.AgregarHijo(permisoaux);
                }
            }

            acceso.CerrarConexion();
            return listadepermisos;
        }
                        
        //Accion = Patente
        public IList<AccionBE> TraerTodasLasAcciones()
        {
            acceso.AbrirConexion();
            IList<AccionBE> listadeacciones = new List<AccionBE>();
            DataTable tabla = acceso.Leer("ACCION_LISTAR", null);
            acceso.CerrarConexion();

            /*
                SELECT * FROM Permiso WHERE permiso_tipo IS NOT NULL
                Me traigo los que no son NULL porque en la base los registros que en el campo permiso tengan el valor de NULL van a ser roles
                1	IgresarAVentas  Accion  --> Es una accion/patente
                2	VerVenta        Accion  --> Es una accion/patente
                3   Administrador   NULL    --> Es un rol/familia
            */

            foreach (DataRow dr in tabla.Rows)
            {
                var id = int.Parse(dr["id"].ToString());
                var permiso_nombre = dr["permiso_nombre"].ToString();
                var permiso = dr["permiso_tipo"].ToString();

                AccionBE accionaux = new AccionBE();
                accionaux.Id = id;
                accionaux.Nombre = permiso_nombre;
                accionaux.PermisoTipoEnum = (PermisoENUMBE)Enum.Parse(typeof(PermisoENUMBE), permiso_nombre);

                listadeacciones.Add(accionaux);
            }

            return listadeacciones;
        }

        
        //Rol = Familia
        public IList<RolBE> TraerTodosLosRoles()
        {
            acceso.AbrirConexion();
            IList<RolBE> listaderoles = new List<RolBE>();
            DataTable tabla = acceso.Leer("ROL_LISTAR", null);
            acceso.CerrarConexion();

            //SELECT * FROM Permiso WHERE permiso_tipo IS NULL
            foreach (DataRow dr in tabla.Rows)
            {
                var id = int.Parse(dr["id"].ToString());
                var permiso_nombre = dr["permiso_nombre"].ToString();

                RolBE rolaux = new RolBE();
                rolaux.Id = id;
                rolaux.Nombre = permiso_nombre;

                listaderoles.Add(rolaux);
            }
            return listaderoles;
        }
                
        private PermisoBE TraerUnPermiso(int id, IList<PermisoBE> listadepermisos)
        {
            PermisoBE permisoaux = listadepermisos != null ? listadepermisos.Where(i => i.Id.Equals(id)).FirstOrDefault() : null;

            if (permisoaux == null && listadepermisos != null)
            {
                foreach (var permiso in listadepermisos)
                {
                    var hijo = TraerUnPermiso(id, permiso.ListaDeHijos);
                    if (hijo != null && hijo.Id == id)
                    {
                        return hijo;
                    }
                    else
                    {
                        if (hijo != null)
                        {
                            return TraerUnPermiso(id, hijo.ListaDeHijos);
                        }
                    }
                }
            }
            return permisoaux;
        }
      
        public void LlenarUsuarioPermisos(UsuarioBE usuario)
        {

            string commandText = $"SELECT P.* FROM Usuario_Permiso UP INNER JOIN Permiso P ON UP.permiso_id = P.permiso_id WHERE usuario_username = '{usuario.Username}'";

            acceso.AbrirConexion();
            DataTable tabla = acceso.Leer(commandText, null, false);
            acceso.CerrarConexion();

            if (tabla.Rows.Count != 0)
            {
                usuario.ListaDePermisos.Clear();

                foreach (DataRow dr in tabla.Rows)
                {
                    int idpermiso = int.Parse(dr["permiso_id"].ToString());
                    string permiso_nombrepermiso = dr["permiso_nombre"].ToString();

                    var permisopermiso_tipo = string.Empty;
                    if (dr["permiso_tipo"] != DBNull.Value)
                    {
                        permisopermiso_tipo = dr["permiso_nombre"].ToString();
                    }

                    PermisoBE permisoaux;
                    if (!String.IsNullOrEmpty(permisopermiso_tipo))
                    {
                        permisoaux = new AccionBE();
                        permisoaux.Id = idpermiso;
                        permisoaux.Nombre = permiso_nombrepermiso;
                        permisoaux.PermisoTipoEnum = (PermisoENUMBE)Enum.Parse(typeof(PermisoENUMBE), permiso_nombrepermiso);
                        usuario.ListaDePermisos.Add(permisoaux);
                    }
                    else
                    {
                        permisoaux = new RolBE();
                        permisoaux.Id = idpermiso;
                        permisoaux.Nombre = permiso_nombrepermiso;

                        var f = TraerTodo(idpermiso.ToString());
                        foreach (var item in f)
                        {
                            permisoaux.AgregarHijo(item);
                        }
                        usuario.ListaDePermisos.Add(permisoaux);
                    }
                }
            }
            else
            {
                PermisoBE permisoaux;
                permisoaux = new AccionBE();
                permisoaux.Id = 0;
                permisoaux.Nombre = "SinPermisos";
                permisoaux.PermisoTipoEnum = (PermisoENUMBE)Enum.Parse(typeof(PermisoENUMBE), "SinPermisos");
                usuario.ListaDePermisos.Add(permisoaux);
            }
        }
                
        public void LlenarRolPermisos(RolBE rol)
        {
            rol.VaciarHijos();
            IList<PermisoBE> permisos = TraerTodo(rol.Id.ToString());

            foreach (var item in permisos)
            {
                rol.AgregarHijo(item);
            }
        }
    }
}
