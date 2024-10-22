using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.ViewModels.Requests
{
    public class CreateOrderViewModel
    {
        [Required]
        public double Weight { get; set; }

        [Required]
        public Guid DistrictId { get; set; }

        [Required]
        public DateTimeOffset DeliveryTime { get; set; }
    }
}
