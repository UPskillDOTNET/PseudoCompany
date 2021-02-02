﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueAPICentral.Data;
using ParqueAPICentral.Models;

namespace ParqueAPICentral.Controllers
{
    [Route("api/Moradas")]
    [ApiController]
    public class MoradasController : ControllerBase
    {
        private readonly APICentralContext _context;

        public MoradasController(APICentralContext context)
        {
            _context = context;
        }

        // GET: api/Moradas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Morada>>> GetMorada()
        {
            return await _context.Morada.ToListAsync();
        }

        // GET: api/Moradas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Morada>> GetMorada(long id)
        {
            var morada = await _context.Morada.FindAsync(id);

            if (morada == null)
            {
                return NotFound();
            }

            return morada;
        }

        // PUT: api/Moradas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMorada(long id, Morada morada)
        {
            if (id != morada.MoradaID)
            {
                return BadRequest();
            }

            _context.Entry(morada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoradaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Moradas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Morada>> PostMorada(Morada morada)
        {
            _context.Morada.Add(morada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMorada", new { id = morada.MoradaID }, morada);
        }

        // DELETE: api/Moradas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMorada(long id)
        {
            var morada = await _context.Morada.FindAsync(id);
            if (morada == null)
            {
                return NotFound();
            }

            _context.Morada.Remove(morada);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoradaExists(long id)
        {
            return _context.Morada.Any(e => e.MoradaID == id);
        }
    }
}