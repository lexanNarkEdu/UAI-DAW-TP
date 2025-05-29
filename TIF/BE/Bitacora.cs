using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Bitacora
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string Username { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Bitacora()
        {
            this.FechaCreacion = DateTime.Now;
        }

        public Bitacora(int eventoId, string username)
        {
            EventoId = eventoId;
            Username = username;
            FechaCreacion = DateTime.Now;
        }

        public Bitacora(int id, int eventoId, string username, DateTime fechaCreacion)
        {
            Id = id;
            EventoId = eventoId;
            Username = username;
            FechaCreacion = fechaCreacion;
        }
    }
}