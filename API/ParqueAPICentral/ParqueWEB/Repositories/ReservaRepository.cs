﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueAPICentral.Data;
using ParqueAPICentral.Models;
/*
namespace ParqueAPICentral.Repositories
{
    public class ReservaRepository : RepositoryBase<Reserva>, IReservaRepository
    {
        public ReservaRepository(APICentralContext RepContext) : base(RepContext)
        {
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> GetAllReservasAsync()
        {
            return await RepContext.Reserva.ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> PostReservaByDataAsync(String DataInicio, String DataFim, long ClienteID)
        {
            return await RepContext.Reserva.ToListAsync();
        }

        public async Task<ActionResult<Reserva>> CancelarReservaAsync(long id)
        {
            return await RepContext.CancelarReserva(id);
        }
    }
}
*/