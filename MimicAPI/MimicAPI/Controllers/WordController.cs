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
        [HttpGet("", Name = "GetAll")]
        public ActionResult<IEnumerable<Word>> GetWord([FromQuery] WordUrlQuery query)
        {

            var item = _repository.GetWord(query);

            if (item.Results.Count == 0)
            {
                return NotFound();
            }

            PaginationList<WordDTO> list = CreateLinksToWordDTOList(query, item);

            return Ok(list);
        }

        private PaginationList<WordDTO> CreateLinksToWordDTOList(WordUrlQuery query, PaginationList<Word> item)
        {
            var list = _mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(item);

            foreach (var word in list.Results)
            {
                word.Links = new List<LinkDTO>();
                word.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = word.WordID }), "GET"));
            }

            list.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            if (item.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));

                if (query.PageNumber + 1 <= item.Pagination.TotalPages)
                {
                    var queryString = new WordUrlQuery() { PageNumber = query.PageNumber + 1, LinesPerPage = query.LinesPerPage, Date = query.Date };
                    list.Links.Add(new LinkDTO("next", Url.Link("GetAll", queryString), "GET"));
                }
                if (query.PageNumber - 1 > 0)
                {
                    var queryString = new WordUrlQuery() { PageNumber = query.PageNumber - 1, LinesPerPage = query.LinesPerPage, Date = query.Date };
                    list.Links.Add(new LinkDTO("prev", Url.Link("GetAll", queryString), "GET"));
                }

            }

            return list;
        }

        // GET: api/Word/5
        [HttpGet("{id}", Name = "GetWord")]
        public ActionResult<Word> GetWord(int id)
        {
            var word = _repository.GetWord(id);

            if (word == null)
            {
                return NotFound();
            }

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.WordID }), "GET"));
            wordDTO.Links.Add(new LinkDTO("update", Url.Link("UpdateWord", new { id = wordDTO.WordID }), "PUT"));
            wordDTO.Links.Add(new LinkDTO("delete", Url.Link("DeleteWord", new { id = wordDTO.WordID }), "DELETE"));

            return Ok(wordDTO);
        }

        // PUT: api/Word/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}", Name = "UpdateWord")]
        public IActionResult PutWord(int id, Word word)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
                return NotFound();

            if (word == null)
                return BadRequest();

            if (ModelState.IsValid)
                return UnprocessableEntity(ModelState);


            word.WordID = id;
            word.Active = obj.Active;
            word.DateCreated = obj.DateCreated;
            word.DateUpdated = DateTime.Now;
            _repository.PutWord(word);

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links.Add(new LinkDTO("update", Url.Link("UpdateWord", new { id = wordDTO.WordID }), "PUT"));


            return Ok(wordDTO);
        }

        // POST: api/Word
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Word> PostWord(Word word)
        {
            if (word == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            word.DateCreated = DateTime.Now;
            word.Active = true;
            _repository.PostWord(word);

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.WordID }), "GET"));


            return Created($"/api/Word/{word.WordID}", wordDTO);
        }

        // DELETE: api/Word/5
        [HttpDelete("{id}", Name = "DeleteWord")]
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
