using MimicAPI.Data;
using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimicAPI.Repositories.Contracts;

namespace MimicAPI.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly WordDbContext _context;
        public WordRepository(WordDbContext context)
        {
            _context = context;
        }
        public PaginationList<Word> GetWords(WordUrlQuery query)
        {
            var list = new PaginationList<Word>();
            var item = _context.Word.AsQueryable();

            if (query.Date.HasValue)
            {
                item = item.Where(a => a.DateCreated > query.Date.Value || a.DateUpdated > query.Date.Value);
            }

            if (query.PageNumber.HasValue)
            {
                var quantityTotalLines = item.Count();
                item = item.Skip((query.PageNumber.Value - 1) * query.LinesPerPage.Value).Take(query.LinesPerPage.Value);

                var pagination = new Pagination();
                pagination.PageNumber = query.PageNumber.Value;
                pagination.LinesPerPage = query.LinesPerPage.Value;
                pagination.TotalLines = quantityTotalLines;
                pagination.TotalPages = (int)Math.Ceiling((double)quantityTotalLines / query.LinesPerPage.Value);
            }
            list.AddRange(item.ToList());

            return list;
        }

        public Word GetWord(int id)
        {
            return _context.Word.Find(id);
        }
        public void PostWord(Word word)
        {
            _context.Word.Add(word);
            _context.SaveChanges();
        }
        public void PutWord(Word word)
        {
            _context.Word.Update(word);
            _context.SaveChanges();
        }
        public void DeleteWord(int id)
        {
            var word = GetWord(id);
            word.Active = false;
            _context.Word.Update(word);
            _context.SaveChanges();

        }

    }
}
