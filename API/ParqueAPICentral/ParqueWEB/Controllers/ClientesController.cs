﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParqueAPICentral.Models;
using ParqueAPICentral.Data;
using Microsoft.EntityFrameworkCore;

namespace ParqueAPICentral.Controllers
{
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly APICentralContext _context;

        public ClientesController(APICentralContext context)
        {
            _context = context;
        }

        // GET: api/Clientes : Obter Informação de um Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Cliente.ToListAsync();
        }

        // GET: api/Clientes/5  - Obter Informação de um Cliente por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(long id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }


    }
}
