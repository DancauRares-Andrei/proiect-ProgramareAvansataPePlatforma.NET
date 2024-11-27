using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.History;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Common;

namespace proiect_ProgramareAvansataPePlatforma.NET.Models
{
    public class CustomHistoryContext : HistoryContext


    {
        public CustomHistoryContext(DbConnection existingConnection, string defaultSchema) : base(existingConnection, defaultSchema)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)


        {


            base.OnModelCreating(modelBuilder);


            modelBuilder.HasDefaultSchema("");


        }


    }
}