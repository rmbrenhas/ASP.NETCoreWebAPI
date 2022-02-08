using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Contracts
{
    public interface IWordRepository
    {
        PaginationList<Word> GetWords(WordUrlQuery query);
        Word GetWord(int id);
        void PostWord(Word word);
        void PutWord(Word word);
        void DeleteWord(int id);

    }
}
