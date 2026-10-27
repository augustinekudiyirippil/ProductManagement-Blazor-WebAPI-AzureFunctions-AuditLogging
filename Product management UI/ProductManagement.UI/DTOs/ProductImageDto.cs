namespace ProductManagement.UI.DTOs
{
    public class ProductImageDto
    {
        public int Id { get; set; }

        public string FileName { get; set; }
            = string.Empty;

        public string BlobUrl { get; set; }
            = string.Empty;

        public string SasUrl { get; set; }
            = string.Empty;

        public bool IsPrimary { get; set; }
    }

}
