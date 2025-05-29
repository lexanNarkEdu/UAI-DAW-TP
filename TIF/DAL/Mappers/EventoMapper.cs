using BE;
using System;
using System.Data;

namespace DAL.Mappers
{
    public class EventoMapper : BaseMapper<Evento>
    {
        public override Evento MapToEntity(DataRow row)
        {
            if (row is null)
                return null;

            return new Evento()
            {
                Id = Convert.ToInt32(row["evento_id"].ToString()),
                Nombre = (EventoTipoEnum)Enum.Parse(typeof(EventoTipoEnum), row["evento_nombre"].ToString()),                
                Descripcion = row["evento_descripcion"].ToString(),
                CriticidadId = (EventoCriticidadEnum)Enum.Parse(typeof(EventoCriticidadEnum), row["evento_criticidad_id"].ToString())
            };
        }
    }
}