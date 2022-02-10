using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Data;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;

namespace MimicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;

        public WordController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Word
        [HttpGet]
        public ActionResult<IEnumerable<Word>> GetWord([FromQuery]WordUrlQuery query)
        {

            var item = _repository.GetWord(query);

            if (query.PageNumber != null)
            {
                if (query.PageNumber > item.Pagination.TotalPages)
                {
                    return NotFound();
                }
            }

            if (query.PageNumber == null)
            {
                query.PageNumber = 0;

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

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links = new List<LinkDTO>();
            wordDTO.Links.Add(
                new LinkDTO("self", $"https://localhost:5001/api/Word/{wordDTO.WordID}", "GET"));

            return Ok(wordDTO);
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
