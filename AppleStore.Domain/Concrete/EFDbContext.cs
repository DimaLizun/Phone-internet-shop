using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppleStore.Domain.Entities;
using System.Data.Entity;


namespace AppleStore.Domain.Concrete
{
     public class EFDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
