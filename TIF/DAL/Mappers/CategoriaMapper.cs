﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL.Mappers
{
    public class CategoriaMapper : BaseMapper<Categoria>
    {
        public override Categoria MapToEntity(DataRow row) => new Categoria
        {
            CategoriaId = Convert.ToInt32(row["Categoria_id"]),
            Nombre = row["Categoria_nombre"].ToString()
        };
    }
}
