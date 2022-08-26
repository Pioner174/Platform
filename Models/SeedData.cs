using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Models
{
    public class SeedData
    {
        private CalculationContext _context;
        private ILogger<SeedData> _logger;

        private static Dictionary<int, long> _data = new Dictionary<int, long>()
        {
            {1,1 },{2,3 },{3,6 },{4,10 },{5,15 },
            {6,21 },{7,28 },{8,36 },{9,45 },{10,55 }
        };

        public SeedData(CalculationContext context, ILogger<SeedData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SeedDatabase()
        {
            _context.Database.Migrate();

            if(_context.Calculatons.Count() == 0)
            {
                _logger.LogInformation("Подготовка базы данных");
                _context.Calculatons!.AddRange(_data.Select(kvp => 
                new Calculaton() { Count = kvp.Key, Result = kvp.Value }));

                _context.SaveChanges();

                _logger.LogInformation("БД заполнена начальными значениями");
            }
            else
            {
                _logger.LogInformation("БД имеет значения");
            }
            

        }
    }
}
