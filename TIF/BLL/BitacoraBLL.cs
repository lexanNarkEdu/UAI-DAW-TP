using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BLL
{
    public class BitacoraBLL
    {
        private readonly BitacoraDAL _dal;
        private readonly EventoBLL _eventosBLL;

        public BitacoraBLL()
        {
            _dal = new BitacoraDAL();
            _eventosBLL = new EventoBLL();
        }

        public int RegistrarEvento(EventoTipoEnum tipoEvento, string username)
        {
            try
            {
                // Buscar el evento por nombre usando el enum
                var evento = _eventosBLL.ObtenerPorNombre(tipoEvento.ToString());
                if (evento == null)
                    throw new Exception($"El evento '{tipoEvento}' no existe en el sistema");

                // Crear registro de bitácora
                var bitacora = new Bitacora(evento.Id, username);

                return _dal.RegistrarEvento(bitacora);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar evento en bitácora: {ex.Message}");
            }
        }

        public int RegistrarEvento(int eventoId, string username)
        {
            try
            {
                // Verificar que el evento existe
                var evento = _eventosBLL.ObtenerPorId(eventoId);
                if (evento == null)
                    throw new Exception($"El evento con ID {eventoId} no existe en el sistema");

                // Crear registro de bitácora
                var bitacora = new Bitacora(eventoId, username);

                return _dal.RegistrarEvento(bitacora);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar evento en bitácora: {ex.Message}");
            }
        }

        public List<Bitacora> ObtenerTodos()
        {
            return _dal.ObtenerTodos();
        }

        public List<Bitacora> ObtenerPorUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("");

            return _dal.ObtenerPorUsername(username);
        }

        public List<Bitacora> ObtenerPorEvento(int eventoId)
        {
            if (eventoId <= 0)
                throw new Exception("El ID de evento debe ser válido");

            return _dal.ObtenerPorEvento(eventoId);
        }

        public List<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > fechaHasta)
                throw new Exception("La fecha desde no puede ser mayor a la fecha hasta");

            return _dal.ObtenerPorFecha(fechaDesde, fechaHasta);
        }

        public List<Bitacora> ObtenerPorRangoFechas(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            DateTime desde = fechaDesde ?? DateTime.Today.AddDays(-30); // Por defecto últimos 30 días
            DateTime hasta = fechaHasta ?? DateTime.Now;

            return ObtenerPorFecha(desde, hasta);
        }

        // Método de conveniencia para registrar eventos comunes del sistema
        public void RegistrarLogin(string username)
        {
            RegistrarEvento(EventoTipoEnum.Login, username);
        }

        public void RegistrarLogout(string username)
        {
            RegistrarEvento(EventoTipoEnum.Logout, username);
        }

        public void RegistrarCreacionUsuario(string usernameAdmin)
        {
            RegistrarEvento(EventoTipoEnum.CrearUsuario, usernameAdmin);
        }

        public void RegistrarCambioPassword(string username)
        {
            RegistrarEvento(EventoTipoEnum.CambiarPassword, username);
        }

        public void RegistrarAccesoNoAutorizado(string username)
        {
            RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username);
        }

        public void RegistrarErrorSistema(string username)
        {
            RegistrarEvento(EventoTipoEnum.ErrorSistema, username);
        }
    }
}