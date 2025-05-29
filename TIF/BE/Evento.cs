using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Evento
    {
        public int Id { get; set; }
        public EventoTipoEnum Nombre { get; set; }
        public string Descripcion { get; set; }
        public EventoCriticidadEnum CriticidadId { get; set; }

        public Evento()
        {
        }

        public Evento(EventoTipoEnum evento, string descripcion, EventoCriticidadEnum criticidadId)
        {
            Nombre = evento;
            Descripcion = descripcion;
            CriticidadId = criticidadId;
        }
    }

    public enum EventoCriticidadEnum
    {
        Baja = 1,
        Media = 2,
        Alta = 3
    }
}