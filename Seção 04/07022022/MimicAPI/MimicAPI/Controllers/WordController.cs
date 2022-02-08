using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Data;
using MimicAPI.Models;

namespace MimicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly WordDbContext _context;

        public WordController(WordDbContext context)
        {
            _context = context;
        }

        // GET: api/Word
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Word>>> GetWord()
        {
            return await _context.Word.ToListAsync();
        }

        // GET: api/Word/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> GetWord(int id)
        {
            var word = await _context.Word.FindAsync(id);

            if (word == null)
            {
                return NotFound();
            }

            return word;
        }

        // PUT: api/Word/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWord(int id, Word word)
        {
            if (id != word.WordID)
            {
                return BadRequest();
            }

            _context.Entry(word).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordExists(id))
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

        // POST: api/Word
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Word>> PostWord(Word word)
        {
            _context.Word.Add(word);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWord", new { id = word.WordID }, word);
        }

        // DELETE: api/Word/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Word>> DeleteWord(int id)
        {
            var word = await _context.Word.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            _context.Word.Remove(word);
            await _context.SaveChangesAsync();

            return word;
        }

        private bool WordExists(int id)
        {
            return _context.Word.Any(e => e.WordID == id);
        }
    }
}
