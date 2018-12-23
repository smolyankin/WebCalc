using System.Data.Entity;
using WebCalc.Models;

namespace WebCalc.Context
{
    /// <summary>
    /// подключение к базе данных
    /// </summary>
    public class BaseContext : DbContext
    {
        /// <summary>
        /// по умолчанию
        /// </summary>
        public BaseContext() : base("DbConnection")
        {
            Database.SetInitializer<BaseContext>(null);
        }

        /// <summary>
        /// таблица расчетов
        /// </summary>
        public DbSet<UserCalculation> UserCalculations { get; set; }
    }
}