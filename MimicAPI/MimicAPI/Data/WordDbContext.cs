using Microsoft.EntityFrameworkCore;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Data
{
    public class WordDbContext : DbContext
    {
        public WordDbContext(DbContextOptions options) : base(options) 
        {

        }

        public DbSet<Word> Word { get; set; }
    }
}
