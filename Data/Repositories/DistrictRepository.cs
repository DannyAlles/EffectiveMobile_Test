using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly DeliveryDbContext _context;

        public DistrictRepository(DeliveryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<District?> GetDistrictByIdAsync(Guid id)
        {
            return await _context.Districts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }
    }
}
