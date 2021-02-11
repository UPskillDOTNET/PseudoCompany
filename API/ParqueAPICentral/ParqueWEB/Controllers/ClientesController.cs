﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueAPICentral.Data;
using ParqueAPICentral.Models;
using ParqueAPICentral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueAPICentral.Controllers
{
    // [Authorize]
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
        //      [Authorize]
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

        // PUT: api/Clientes/{ClienteID}/{NomeCliente}{EmailCliente}/{NifCliente}/{MetodoPagamento}/{Credito}/{UserID} - Actualizar informação de um Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{ClienteID}/{NomeCliente}{EmailCliente}/{NifCliente}/{MetodoPagamento}/{Credito}/{UserID}")]
        public async Task<IActionResult> PutCliente(long ClienteID, string NomeCliente, string EmailCliente, int NifCliente, string MetodoPagamento, float Credito, long UserID)
        {
            var cliente = _context.Cliente.FirstOrDefault(n => n.ClienteID == ClienteID);

            if (cliente == null)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            cliente.NomeCliente = NomeCliente;
            cliente.EmailCliente = EmailCliente;
            cliente.NifCliente = NifCliente;
            cliente.MetodoPagamento = MetodoPagamento;
            cliente.Credito = Credito;
            cliente.Id = UserID;

            await _context.SaveChangesAsync();

            return Ok(cliente);
        }
        /*
         // PUT: api/Clientes/5 -  Actualizar informação de um Cliente pelo seu ID
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(long id, Cliente cliente)
        {
            if (id != cliente.ClienteID)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/

        // POST: api/Clientes/{NomeCliente}/{EmailCliente}/{NifCliente}/{MetodoPagamento}/{Credito}/{UserID} : Criação de um Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{NomeCliente}/{EmailCliente}/{NifCliente}/{MetodoPagamento}/{Credito}/{UserID}")]
        public async Task<ActionResult<Cliente>> PostCliente(string NomeCliente, string EmailCliente, int NifCliente, string MetodoPagamento, float Credito, long UserID)
        {
            Cliente cliente = new Cliente(NomeCliente, EmailCliente, NifCliente, MetodoPagamento, Credito, UserID);
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.ClienteID }, cliente);
        }
        /*
        // POST: api/Clientes : Criação de um Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.ClienteID }, cliente);
        }
        private bool ClienteExists(long id)
        {
            return _context.Cliente.Any(e => e.ClienteID == id);
        }*/

        // DELETE: api/Clientes/5
        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
    /*//  [Authorize]
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _service;
        public ClientesController(ClienteService service)
        {
            this._service = service;
        }

        // GET: api/Clientes : Obter Informação de um Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await this._service.GetAllClientes();
        }
        // GET: api/Clientes/5  - Obter Informação de um Cliente por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(long id)
        {
            return await this._service.FindClienteById(id);
        }
        // POST: api/Clientes : Criação de um Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            return await this._service.CreateCliente(cliente cliente);
        }*/
