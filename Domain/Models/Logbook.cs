using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Logbook
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// IP-адрес 
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Дата и время получения доступа
        /// </summary>
        [Column(TypeName = "timestamp without time zone")]
        public DateTime AccessAt { get; set; }
    }
}
