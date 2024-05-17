using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dokimh8.Models;
using Microsoft.EntityFrameworkCore;

namespace Dokimh8.DataAccess
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options): base( options)
        {
            
        }

        public DbSet<Products> Products { get; set; }
    } 
}
