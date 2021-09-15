using Esiur.Stores.EntityCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esiur.Examples.Monitor.Model
{
    class DB:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(App.ConnectionString)
                          .UseEsiur(App.EntityStore, () => new DB());
        }

    }
}
