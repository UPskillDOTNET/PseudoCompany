﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParqueAPICentral.Models;
using ParqueAPICentral.Repositories;

namespace ParqueAPICentral.Services
{
    public class ReservaCentralService
    {
        private readonly IReservaCentralRepository _repo;
        private readonly SubAluguerService _serviceS;

        public ReservaCentralService(IReservaCentralRepository repo, SubAluguerService serviceS) // causa erro
        {
            this._repo = repo;
            this._serviceS = serviceS;
        }


        public async Task<ActionResult<IEnumerable<Reserva>>> GetAllReservasCentralAsync()
        {

            return await _repo.GetAllReservasCentralAsync();
        }

        public async Task<ActionResult<Reserva>> UpdateReserva(Reserva reserva)
        {

            return await _repo.UpdateReserva(reserva);
        }



        public async Task<ActionResult<Reserva>> DeleteReservaCentral(long id)
    {
        return await _repo.DeleteReservaCentral(id);
    }
        
        public async Task<ActionResult<Reserva>> GetAllClienteByReservasCentral(long ParqueID, long id)
        {
            return await _repo.GetAllClienteByReservasCentralAsync(ParqueID, id);
        }

        public async Task<ActionResult<Reserva>> ParaSubALuguer(long id)
        {           
            var reserva = _repo.GetReservaByIdAsync(id).Result.Value;
            
            if (reserva.ParaSubAluguer == false)
            {
                reserva.ParaSubAluguer = true;
                await _serviceS.PostSubAluguer(new SubAluguer
                {
                    //Preco = 11,
                    ReservaID = id,
                    Reservado = false
                });
            }
            else
            {
                var sub = await _serviceS.FindSubAluguerById(id);

                if (sub.Value.Reservado == false)
                {
                    reserva.ParaSubAluguer = false;
                    await _serviceS.DeleteSubAluguer(id);
                }
                else
                    throw new Exception("Este subaluguer já foi reservado por outro utilizador e não pode ser eliminado.");
            }
            
        return await _repo.ParaSubALuguer(id);
        }

        public async Task<ActionResult<Reserva>>  CriarReservaCentral(Reserva reserva)
        {
                    
            return await _repo.CriarReservaCentral(reserva);

        }

        public async Task<ActionResult<Reserva>> GetReservaById(long id)
        {
            return await _repo.GetReservaByIdAsync(id);
        }
        
    }
}

