using System;
using System.Collections.Generic;
using System.Text;

namespace SendProductEmailFunction.DTO
{
    public class ProductEmailMessage
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ActionType { get; set; } = string.Empty;

        public int UserId { get; set; }

        public DateTime EventDate { get; set; }
    }
}
