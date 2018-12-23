using System;
using System.Data.Linq.Mapping;

namespace WebCalc.Models
{
    /// <summary>
    /// сущность расчета
    /// </summary>
    [Table(Name = "UserCalculations")]
    public class UserCalculation
    {
        /// <summary>
        /// ид
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id { get; set; }

        /// <summary>
        /// ип адрес пользователя
        /// </summary>
        [Column]
        public string IpAddress { get; set; }

        /// <summary>
        /// введенная формула
        /// </summary>
        [Column]
        public string Input { get; set; }

        /// <summary>
        /// результат расчета
        /// </summary>
        [Column]
        public string Result { get; set; }

        /// <summary>
        /// дата расчета
        /// </summary>
        [Column]
        public DateTime DateCreate { get; set; }
    }
}