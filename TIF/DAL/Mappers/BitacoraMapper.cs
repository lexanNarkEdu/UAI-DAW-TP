using BE;
using System;
using System.Data;

namespace DAL.Mappers
{
    public class BitacoraMapper : BaseMapper<Bitacora>
    {
        public override Bitacora MapToEntity(DataRow row)
        {
            if (row is null)
                return null;

            return new Bitacora()
            {
                Id = Convert.ToInt32(row["bitacora_id"].ToString()),
                EventoId = Convert.ToInt32(row["bitacora_evento_id"].ToString()),
                Username = row["bitacora_usuario_username"].ToString(),
                FechaCreacion = Convert.ToDateTime(row["bitacora_fecha_creacion_datetime"].ToString())
            };
        }
    }
}