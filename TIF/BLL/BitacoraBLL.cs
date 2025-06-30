using System;
using System.Collections.Generic;
using BE;

namespace BLL
{
    public class BitacoraBLL
    {
        private readonly EventoBLL _eventosBLL;

        public BitacoraBLL()
        {
            _eventosBLL = new EventoBLL();
        }

        public int RegistrarEvento(EventoTipoEnum tipoEvento, string username, string descripcion = null, EventoCriticidadEnum criticidad = EventoCriticidadEnum.Baja)
        {
            try
            {
                return _eventosBLL.RegistrarEvento(username, tipoEvento, descripcion, criticidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar evento en bitácora: {ex.Message}");
            }
        }

        public List<Evento> ObtenerTodos()
        {
            return _eventosBLL.ObtenerTodos();
        }

        public List<Evento> ObtenerPorUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("El username es requerido");

            return _eventosBLL.ObtenerPorUsername(username);
        }

        public List<Evento> ObtenerPorTipoEvento(EventoTipoEnum tipoEvento)
        {
            return _eventosBLL.ObtenerPorTipoEvento(tipoEvento);
        }

        public List<Evento> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > fechaHasta)
                throw new Exception("La fecha desde no puede ser mayor a la fecha hasta");

            return _eventosBLL.ObtenerPorFecha(fechaDesde, fechaHasta);
        }

        public List<Evento> ObtenerPorCriticidad(EventoCriticidadEnum criticidad)
        {
            return _eventosBLL.ObtenerPorCriticidad(criticidad);
        }

        public List<Evento> ObtenerPorRangoFechas(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            DateTime desde = fechaDesde ?? DateTime.Today.AddDays(-30); // Por defecto últimos 30 días
            DateTime hasta = fechaHasta ?? DateTime.Now;

            return ObtenerPorFecha(desde, hasta);
        }

        // Métodos de conveniencia para registrar eventos comunes del sistema
        public void RegistrarLogin(string username)
        {
            _eventosBLL.RegistrarLogin(username);
        }

        public void RegistrarLogout(string username)
        {
            _eventosBLL.RegistrarLogout(username);
        }

        public void RegistrarCreacionUsuario(string usernameAdmin)
        {
            _eventosBLL.RegistrarCreacionUsuario(usernameAdmin);
        }

        public void RegistrarModificacionUsuario(string usernameAdmin)
        {
            _eventosBLL.RegistrarModificacionUsuario(usernameAdmin);
        }

        public void RegistrarEliminacionUsuario(string usernameAdmin)
        {
            _eventosBLL.RegistrarEliminacionUsuario(usernameAdmin);
        }

        public void RegistrarCambioPassword(string username)
        {
            _eventosBLL.RegistrarCambioPassword(username);
        }

        public void RegistrarAccesoNoAutorizado(string username)
        {
            _eventosBLL.RegistrarAccesoNoAutorizado(username);
        }

        public void RegistrarErrorSistema(string username)
        {
            _eventosBLL.RegistrarErrorSistema(username);
        }
    }
}