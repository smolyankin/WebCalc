using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebCalc.Models;

namespace WebCalc.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext() : base("DbConnection")
        {

        }

        public DbSet<UserCalculation> UserCalculations { get; set; }
    }
}