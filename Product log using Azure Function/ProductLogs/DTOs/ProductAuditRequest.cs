using System;
using System.Collections.Generic;
using System.Text;

namespace ProductLogs.DTOs
{
    public class ProductAuditRequest
    {
        public int ProductId { get; set; }

        public int UserId { get; set; }

        public string ActionType { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
