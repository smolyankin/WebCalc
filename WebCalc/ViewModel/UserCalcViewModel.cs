using System.Collections.Generic;
using WebCalc.Models;

namespace WebCalc.ViewModel
{
    /// <summary>
    /// модель представления расчета пользователя
    /// </summary>
    public class UserCalcViewModel
    {
        /// <summary>
        /// ип адрес пользователя
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// формула
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// результат расчета
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// история расчетов
        /// </summary>
        public List<UserCalculation> Calculations { get; set; } = new List<UserCalculation>();

        /// <summary>
        /// ид расчета для повтора
        /// </summary>
        public long RepeatId { get; set; }

        /// <summary>
        /// сообщение о результате расчета
        /// </summary>
        public string Message { get; set; }
    }
}