using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class District
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Название района
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Titile { get; set; }
    }
}
