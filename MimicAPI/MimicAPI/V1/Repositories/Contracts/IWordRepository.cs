using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories.Contracts
{
    public interface IWordRepository
    {
        PaginationList<Word> GetWord(WordUrlQuery query);
        Word GetWord(int id);
        void PostWord(Word word);
        void PutWord(Word word);
        void DeleteWord(int id);

    }
}
