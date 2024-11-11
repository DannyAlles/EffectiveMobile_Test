﻿using System.ComponentModel.DataAnnotations;

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
