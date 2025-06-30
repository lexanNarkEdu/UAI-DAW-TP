using System;

namespace BE
{
    public class Evento
    {
        public int Id { get; set; }
        public string UsuarioUsername { get; set; }
        public EventoTipoEnum Nombre { get; set; }
        public string Descripcion { get; set; }
        public EventoCriticidadEnum CriticidadId { get; set; }
        public DateTime FechaHora { get; set; }

        public Evento()
        {
            this.FechaHora = DateTime.Now;
        }

        public Evento(string usuarioUsername, EventoTipoEnum nombre, string descripcion, EventoCriticidadEnum criticidadId)
        {
            UsuarioUsername = usuarioUsername;
            Nombre = nombre;
            Descripcion = descripcion;
            CriticidadId = criticidadId;
            FechaHora = DateTime.Now;
        }

        public Evento(int id, string usuarioUsername, EventoTipoEnum nombre, string descripcion, EventoCriticidadEnum criticidadId, DateTime fechaHora)
        {
            Id = id;
            UsuarioUsername = usuarioUsername;
            Nombre = nombre;
            Descripcion = descripcion;
            CriticidadId = criticidadId;
            FechaHora = fechaHora;
        }
    }

    public enum EventoCriticidadEnum
    {
        Baja = 1,
        Media = 2,
        Alta = 3
    }
}