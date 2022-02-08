using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Data;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;

namespace MimicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordRepository _repository;

        public WordController(IWordRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Word
        [HttpGet]
        public ActionResult<IEnumerable<Word>> GetWord([FromQuery]WordUrlQuery query)
        {

            var item = _repository.GetWords(query);

            if (query.PageNumber > item.Pagination.TotalPages)
            {
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));

            return Ok(item.ToList());
        }

        // GET: api/Word/5
        [HttpGet("{id}")]
        public ActionResult<Word> GetWord(int id)
        {
            var word = _repository.GetWord(id);

            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        // PUT: api/Word/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutWord(int id, Word word)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
            {
                return NotFound();
            }

            word.WordID = id;
            _repository.PutWord(word);
            
            return Ok();
        }

        // POST: api/Word
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Word> PostWord(Word word)
        {

            _repository.PostWord(word);
            return CreatedAtAction("GetWord", new { id = word.WordID }, word);
        }

        // DELETE: api/Word/5
        [HttpDelete("{id}")]
        public ActionResult<Word> DeleteWord(int id)
        {
            var word = _repository.GetWord(id);
            if (word == null)
            {
                return NotFound();
            }

            _repository.DeleteWord(id);

            return NoContent();
        }

    }
}
