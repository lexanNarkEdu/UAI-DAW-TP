using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;
using DAL.Mappers;

namespace DAL
{
    public class EventosDAL
    {
        private readonly DAO _dao;
        private readonly EventoMapper _mapper;

        public EventosDAL()
        {
            _dao = DAO.GetDAO();
            _mapper = new EventoMapper();
        }

        public Evento Obtener(int eventoId)
        {
            var sql = "SELECT evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento WHERE evento_id = @eventoId";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@eventoId", eventoId)
                },
                CommandType.Text);

            return _mapper.MapToEntity(dataTable);
        }

        public Evento ObtenerPorNombre(string nombre)
        {
            var sql = "SELECT TOP 1 evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento WHERE evento_nombre = @nombre ORDER BY evento_fecha_hora DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@nombre", nombre)
                },
                CommandType.Text);

            return _mapper.MapToEntity(dataTable);
        }

        public List<Evento> ObtenerTodos()
        {
            var sql = "SELECT evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento ORDER BY evento_fecha_hora DESC";

            var dataTable = _dao.Read(sql, commandType: CommandType.Text);
            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Evento> ObtenerPorUsername(string username)
        {
            var sql = "SELECT evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento WHERE evento_usuario_username = @username ORDER BY evento_fecha_hora DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@username", username)
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Evento> ObtenerPorTipoEvento(EventoTipoEnum tipoEvento)
        {
            var sql = "SELECT evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento WHERE evento_nombre = @nombre ORDER BY evento_fecha_hora DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@nombre", tipoEvento.ToString())
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Evento> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            var sql = "SELECT evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento WHERE evento_fecha_hora BETWEEN @fechaDesde AND @fechaHasta ORDER BY evento_fecha_hora DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@fechaDesde", fechaDesde),
                    _dao.CreateParameter("@fechaHasta", fechaHasta)
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }

        public List<Evento> ObtenerPorCriticidad(EventoCriticidadEnum criticidad)
        {
            var sql = "SELECT evento_id, evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora FROM Evento WHERE evento_criticidad_id = @criticidad ORDER BY evento_fecha_hora DESC";

            var dataTable = _dao.Read(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@criticidad", (int)criticidad)
                },
                CommandType.Text);

            return _mapper.MapAll(dataTable).ToList();
        }

        public int Crear(Evento evento)
        {
            var sql = "INSERT INTO Evento (evento_usuario_username, evento_nombre, evento_descripcion, evento_criticidad_id, evento_fecha_hora) VALUES (@username, @nombre, @descripcion, @criticidadId, @fechaHora)";

            int result = _dao.Write(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@username", evento.UsuarioUsername),
                    _dao.CreateParameter("@nombre", evento.Nombre.ToString()),
                    _dao.CreateParameter("@descripcion", evento.Descripcion),
                    _dao.CreateParameter("@criticidadId", (int)evento.CriticidadId),
                    _dao.CreateParameter("@fechaHora", evento.FechaHora)
                },
                CommandType.Text);

            return result;
        }

        public int RegistrarEvento(string username, EventoTipoEnum tipoEvento, string descripcion, EventoCriticidadEnum criticidad)
        {
            var evento = new Evento(username, tipoEvento, descripcion, criticidad);
            return Crear(evento);
        }
    }
}