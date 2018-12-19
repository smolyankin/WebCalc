using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
//using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalc.Models
{
    [Table(Name = "UserCalculations")]
    public class UserCalculation
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id { get; set; }

        [Column]
        public string IpAddress { get; set; }

        [Column]
        public string Input { get; set; }

        [Column]
        public string Result { get; set; }

        [Column]
        public DateTime DateCreate { get; set; }
    }
}