namespace ProductManagement.Api.DTOs
{
    public class ProductEmailMessage
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ActionType { get; set; }

        public int UserId { get; set; }

        public DateTime EventDate { get; set; }
    }
}
