using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Data
{
    public static class WordDbInitializer
    {
        public static void Seed(this WordDbContext dbContext)
        {
            if (!dbContext.Word.Any())
            {
                dbContext.Word.Add(new Word
                {
                    Name = "Leão",
                    Points = 5,
                    Active = true,
                   // DateCreated = DateTime.Now
                }) ;

                dbContext.Word.Add(new Word
                {
                    Name = "Gato",
                    Points = 2,
                    Active = true,
                    //DateCreated = DateTime.Now
                });

                dbContext.Word.Add(new Word
                {
                    Name = "Múmia",
                    Points = 10,
                    Active = true,
                    //DateCreated = DateTime.Now
                });

                dbContext.SaveChanges();
            }
        }

    }
}
