using System;

namespace FlipkartScraper.Models
{
    /// Product model representing scraped data from Flipkart
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    
        public Product()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

    }
}