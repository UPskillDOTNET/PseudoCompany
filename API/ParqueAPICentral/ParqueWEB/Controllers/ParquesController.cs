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
    [Route("api/Parques")]
    [ApiController]
    public class ParquesController : ControllerBase
    {
        private readonly APICentralContext _context;

        public ParquesController(APICentralContext context)
        {
            _context = context;
        }

        // GET: api/Parques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parque>>> GetParque()
        {
            return await _context.Parque.ToListAsync();
        }

        // GET: api/Parques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parque>> GetParque(long id)
        {
            var parque = await _context.Parque.FindAsync(id);

            if (parque == null)
            {
                return NotFound();
            }

            return parque;
        }

        // PUT: api/Parques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParque(long id, Parque parque)
        {
            if (id != parque.ParqueID)
            {
                return BadRequest();
            }

            _context.Entry(parque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParqueExists(id))
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

        // POST: api/Parques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parque>> PostParque(Parque parque)
        {
            _context.Parque.Add(parque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParque", new { id = parque.ParqueID }, parque);
        }

        // DELETE: api/Parques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParque(long id)
        {
            var parque = await _context.Parque.FindAsync(id);
            if (parque == null)
            {
                return NotFound();
            }

            _context.Parque.Remove(parque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParqueExists(long id)
        {
            return _context.Parque.Any(e => e.ParqueID == id);
        }
    }
}
