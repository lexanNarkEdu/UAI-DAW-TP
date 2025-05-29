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
            var sql = "SELECT evento_id, evento_nombre, evento_descripcion, evento_criticidad_id FROM Eventos WHERE evento_id = @eventoId";

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
            var sql = "SELECT evento_id, evento_nombre, evento_descripcion, evento_criticidad_id FROM Eventos WHERE evento_nombre = @nombre";

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
            var sql = "SELECT evento_id, evento_nombre, evento_descripcion, evento_criticidad_id FROM Eventos ORDER BY evento_nombre";

            var dataTable = _dao.Read(sql, commandType: CommandType.Text);
            return _mapper.MapAll(dataTable).ToList();
        }

        public int Crear(Evento evento)
        {
            var sql = "INSERT INTO Eventos (evento_nombre, evento_descripcion, evento_criticidad_id) VALUES (@nombre, @descripcion, @criticidadId)";
            int result = _dao.Write(sql,
                new List<SqlParameter>
                {
                    _dao.CreateParameter("@nombre", evento.Nombre.ToString()),
                    _dao.CreateParameter("@descripcion", evento.Descripcion),
                    _dao.CreateParameter("@criticidadId", (int)evento.CriticidadId)
                },
                CommandType.Text);

            return result;
        }
    }
}