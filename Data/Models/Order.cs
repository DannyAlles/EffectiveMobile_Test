using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Index(nameof(Number), IsUnique = true)]
    public class Order
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Номер заказа
        /// В реальном проекте это может быть формат {номер района}-{какой по счету заказ}, но тут просто буду генерировать 5 значное число
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Вес заказа
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Район заказа
        /// </summary>
        public Guid DistrictId { get; set; }
        public District District { get; set; }

        /// <summary>
        /// Время заказа
        /// Обычно использую UTC, но в ТЗ был указан формат yyyy-MM-dd HH:mm:ss
        /// </summary>
        [Column(TypeName = "timestamp without time zone")]
        public DateTime DeliveryTime { get; set; }
    }
}
