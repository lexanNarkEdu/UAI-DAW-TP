using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BLL
{
    public class EventoBLL
    {
        private readonly EventosDAL _dal;

        public EventoBLL()
            => _dal = new EventosDAL();

        public Evento ObtenerPorId(int eventoId)
        {
            return _dal.Obtener(eventoId);
        }

        public Evento ObtenerPorNombre(string nombre)
        {
            return _dal.ObtenerPorNombre(nombre);
        }

        public List<Evento> ObtenerTodos()
        {
            return _dal.ObtenerTodos();
        }

        public List<Evento> ObtenerPorUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("El username es requerido");

            return _dal.ObtenerPorUsername(username);
        }

        public List<Evento> ObtenerPorTipoEvento(EventoTipoEnum tipoEvento)
        {
            return _dal.ObtenerPorTipoEvento(tipoEvento);
        }

        public List<Evento> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > fechaHasta)
                throw new Exception("La fecha desde no puede ser mayor a la fecha hasta");

            return _dal.ObtenerPorFecha(fechaDesde, fechaHasta);
        }

        public List<Evento> ObtenerPorCriticidad(EventoCriticidadEnum criticidad)
        {
            return _dal.ObtenerPorCriticidad(criticidad);
        }

        public List<Evento> ObtenerPorRangoFechas(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            DateTime desde = fechaDesde ?? DateTime.Today.AddDays(-30); // Por defecto últimos 30 días
            DateTime hasta = fechaHasta ?? DateTime.Now;

            return ObtenerPorFecha(desde, hasta);
        }

        public int CrearEvento(Evento evento)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(evento.UsuarioUsername))
                throw new Exception("El usuario es requerido");

            if (string.IsNullOrWhiteSpace(evento.Descripcion))
                throw new Exception("La descripción del evento es requerida");

            if (evento.CriticidadId <= 0)
                throw new Exception("La criticidad del evento es requerida");

            return _dal.Crear(evento);
        }

        public int RegistrarEvento(string username, EventoTipoEnum tipoEvento, string descripcion = null, EventoCriticidadEnum criticidad = EventoCriticidadEnum.Baja)
        {
            try
            {
                // Si no se proporciona descripción, usar una por defecto
                if (string.IsNullOrEmpty(descripcion))
                {
                    descripcion = ObtenerDescripcionPorDefecto(tipoEvento);
                }

                return _dal.RegistrarEvento(username, tipoEvento, descripcion, criticidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar evento: {ex.Message}");
            }
        }

        private string ObtenerDescripcionPorDefecto(EventoTipoEnum tipoEvento)
        {
            switch (tipoEvento)
            {
                case EventoTipoEnum.Login:
                    return "Usuario inició sesión en el sistema";
                case EventoTipoEnum.Logout:
                    return "Usuario cerró sesión en el sistema";
                case EventoTipoEnum.CrearUsuario:
                    return "Se creó un nuevo usuario en el sistema";
                case EventoTipoEnum.ModificarUsuario:
                    return "Se modificó información de usuario";
                case EventoTipoEnum.EliminarUsuario:
                    return "Se eliminó un usuario del sistema";
                case EventoTipoEnum.CambiarPassword:
                    return "Usuario cambió su contraseña";
                case EventoTipoEnum.AccesoNoAutorizado:
                    return "Intento de acceso no autorizado";
                case EventoTipoEnum.ErrorSistema:
                    return "Error general del sistema";
                default:
                    return "Evento del sistema";
            }
        }

        // Métodos de conveniencia para registrar eventos comunes
        public void RegistrarLogin(string username)
        {
            RegistrarEvento(username, EventoTipoEnum.Login, criticidad: EventoCriticidadEnum.Baja);
        }

        public void RegistrarLogout(string username)
        {
            RegistrarEvento(username, EventoTipoEnum.Logout, criticidad: EventoCriticidadEnum.Baja);
        }

        public void RegistrarCreacionUsuario(string usernameAdmin)
        {
            RegistrarEvento(usernameAdmin, EventoTipoEnum.CrearUsuario, criticidad: EventoCriticidadEnum.Media);
        }

        public void RegistrarModificacionUsuario(string usernameAdmin)
        {
            RegistrarEvento(usernameAdmin, EventoTipoEnum.ModificarUsuario, criticidad: EventoCriticidadEnum.Media);
        }

        public void RegistrarEliminacionUsuario(string usernameAdmin)
        {
            RegistrarEvento(usernameAdmin, EventoTipoEnum.EliminarUsuario, criticidad: EventoCriticidadEnum.Alta);
        }

        public void RegistrarCambioPassword(string username)
        {
            RegistrarEvento(username, EventoTipoEnum.CambiarPassword, criticidad: EventoCriticidadEnum.Baja);
        }

        public void RegistrarAccesoNoAutorizado(string username)
        {
            RegistrarEvento(username, EventoTipoEnum.AccesoNoAutorizado, criticidad: EventoCriticidadEnum.Alta);
        }

        public void RegistrarErrorSistema(string username)
        {
            RegistrarEvento(username, EventoTipoEnum.ErrorSistema, criticidad: EventoCriticidadEnum.Alta);
        }
    }
}