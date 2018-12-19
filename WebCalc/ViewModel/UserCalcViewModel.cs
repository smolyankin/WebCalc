using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebCalc.Models;

namespace WebCalc.ViewModel
{
    public class UserCalcViewModel
    {
        public string IpAddress { get; set; }
        public string Input { get; set; }
        public string PreResult { get; set; }
        public string Result { get; set; }
        public bool IsResult { get; set; } = false;
        public List<UserCalculation> Calculations { get; set; } = new List<UserCalculation>();
    }
}