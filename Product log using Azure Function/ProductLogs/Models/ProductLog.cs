using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductLogs.Models
{
    [Table("ProductLog")]
    public class ProductLog
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public string ActionType { get; set; } = string.Empty;

        public DateTime ActionDateTime { get; set; }

        public string? Description { get; set; }
    }
}
