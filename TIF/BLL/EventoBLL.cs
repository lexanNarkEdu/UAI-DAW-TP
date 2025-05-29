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

        public int CrearEvento(Evento evento)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(evento.Descripcion))
                throw new Exception("La descripción del evento es requerida");

            if (evento.CriticidadId <= 0)
                throw new Exception("La criticidad del evento es requerida");

            // Verificar que no existe un evento con el mismo nombre
            var eventoExistente = _dal.ObtenerPorNombre(evento.Nombre.ToString());
            if (eventoExistente != null)
                throw new Exception("Ya existe un evento con ese nombre");

            return _dal.Crear(evento);
        }

        public void SeedEventosBasicos()
        {
            var eventosBasicos = new List<Evento>
            {
                new Evento(EventoTipoEnum.Login, "Usuario inicia sesión en el sistema", EventoCriticidadEnum.Baja),
                new Evento(EventoTipoEnum.Logout , "Usuario cierra sesión en el sistema", EventoCriticidadEnum.Baja),
                new Evento(EventoTipoEnum.CrearUsuario , "Se crea un nuevo usuario en el sistema", EventoCriticidadEnum.Media),
                new Evento(EventoTipoEnum.ModificarUsuario , "Se modifica información de usuario", EventoCriticidadEnum.Media),
                new Evento(EventoTipoEnum.EliminarUsuario , "Se elimina un usuario del sistema", EventoCriticidadEnum.Alta),
                new Evento(EventoTipoEnum.CambiarPassword , "Usuario cambia su contraseña", EventoCriticidadEnum.Baja),
                new Evento(EventoTipoEnum.AccesoNoAutorizado , "Intento de acceso no autorizado", EventoCriticidadEnum.Alta),
                new Evento(EventoTipoEnum.ErrorSistema , "Error general del sistema", EventoCriticidadEnum.Alta)
            };

            foreach (var evento in eventosBasicos)
            {
                try
                {
                    // Solo crear si no existe
                    var existente = _dal.ObtenerPorNombre(evento.Nombre.ToString());
                    if (existente == null)
                    {
                        _dal.Crear(evento);
                    }
                }
                catch (Exception)
                {
                    // Continuar con el siguiente evento si hay error
                    continue;
                }
            }
        }
    }
}