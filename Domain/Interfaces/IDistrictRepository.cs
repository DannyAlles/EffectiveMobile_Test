using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDistrictRepository
    {
        public Task<District?> GetDistrictByIdAsync(Guid id);
    }
}
