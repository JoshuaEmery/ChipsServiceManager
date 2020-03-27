using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.API
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationsController : ControllerBase
    {
        private readonly ChipsDbContext _context;

        public ConsultationsController(ChipsDbContext context)
        {
            _context = context;
        }

        // GET: api/Consultations
        [HttpGet]
        public IEnumerable<Consultation> GetConsultations()
        {
            return _context.Consultations;
        }

        // GET: api/Consultations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsultation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consultation = await _context.Consultations.FindAsync(id);

            if (consultation == null)
            {
                return NotFound();
            }

            return Ok(consultation);
        }

        // PUT: api/Consultations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsultation([FromRoute] int id, [FromBody] Consultation consultation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != consultation.Id)
            {
                return BadRequest();
            }

            _context.Entry(consultation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultationExists(id))
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

        // POST: api/Consultations
        [HttpPost]
        public async Task<IActionResult> PostConsultation([FromBody] Consultation consultation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsultation", new { id = consultation.Id }, consultation);
        }

        // DELETE: api/Consultations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }

            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();

            return Ok(consultation);
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultations.Any(e => e.Id == id);
        }
    }
}