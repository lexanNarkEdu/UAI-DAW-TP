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
                UsuarioUsername = row["evento_usuario_username"].ToString(),
                Nombre = (EventoTipoEnum)Enum.Parse(typeof(EventoTipoEnum), row["evento_nombre"].ToString()),
                Descripcion = row["evento_descripcion"].ToString(),
                CriticidadId = (EventoCriticidadEnum)Convert.ToInt32(row["evento_criticidad_id"].ToString()),
                FechaHora = Convert.ToDateTime(row["evento_fecha_hora"].ToString())
            };
        }
    }
}