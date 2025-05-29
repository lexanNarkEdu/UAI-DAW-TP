using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;
using DAL.Mappers;

namespace DAL
{
    public class BitacoraDAL
    {
        private readonly DAO _dao;
        private readonly BitacoraMapper _mapper;

        public BitacoraDAL()
        {
            _dao = DAO.GetDAO();
            _mapper = new BitacoraMapper();
        }

        public int RegistrarEvento(Bitacora bitacora)
        {
            var sql = "INSERT INTO Bitacora (bitacora_evento_id, bitacora_usuario_username, bitacora_fecha_creacion_datetime) VALUES (@eventosId, @username, @fechaCreacion)";


            int result = _dao.Write(
                sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@eventosId", bitacora.EventoId),
                    _dao.CreateParameter("@username", bitacora.Username),
                    _dao.CreateParameter("@fechaCreacion", bitacora.FechaCreacion)
                },
                CommandType.Text);

            return result;
        }

        public List<Bitacora> ObtenerTodos()
        {
            var sql = "SELECT bitacora_id, bitacora_evento_id, bitacora_usuario_username, bitacora_fecha_creacion_datetime FROM Bitacora ORDER BY bitacora_fecha_creacion_datetime DESC";

            var dataTable = _dao.Read(sql, commandType: CommandType.Text);;
            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Bitacora> ObtenerPorUsername(string username)
        {
            var sql = "SELECT bitacora_id, bitacora_evento_id, bitacora_usuario_username, bitacora_fecha_creacion_datetime FROM Bitacora WHERE bitacora_usuario_username = @username ORDER BY bitacora_fecha_creacion_datetime DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@username", username)
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Bitacora> ObtenerPorEvento(int eventoId)
        {
            var sql = "SELECT bitacora_id, bitacora_evento_id, bitacora_usuario_username, bitacora_fecha_creacion_datetime FROM Bitacora WHERE bitacora_evento_id = @eventoId ORDER BY bitacora_fecha_creacion_datetime DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@eventoId", eventoId)
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            var sql = "SELECT bitacora_id, bitacora_evento_id, bitacora_usuario_username, bitacora_fecha_creacion_datetime FROM Bitacora WHERE bitacora_fecha_creacion_datetime BETWEEN @fechaDesde AND @fechaHasta ORDER BY bitacora_fecha_creacion_datetime DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@fechaDesde", fechaDesde),
                    _dao.CreateParameter("@fechaHasta", fechaHasta)
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }
    }
}