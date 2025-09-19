using FlipkartScraper.Services;
using FlipkartScraper.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlipkartScraper
{
    /// <summary>
    /// Main program class for Flipkart Electronics scraper

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Flipkart Electronics Scraper ===");
            Console.WriteLine("Starting scraping process...\n");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // ✅ Load appsettings.json configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // ✅ Get connection string from appsettings.json
                string connectionString = configuration.GetConnectionString("DefaultConnection");

                // ✅ Pass connection string to DatabaseService
                var databaseService = new DatabaseService(connectionString);
                var scrapingService = new ScrapingService();

                // Test database connection
                Console.WriteLine("Testing database connection...");
                bool connectionTest = await databaseService.TestConnectionAsync();

                if (!connectionTest)
                {
                    Console.WriteLine("Database connection failed. Please check your connection string.");
                    return;
                }
                Console.WriteLine("Database connection successful!\n");

                // Start scraping process
                Console.WriteLine("Initializing web scraper...");
                var products = await scrapingService.ScrapeFlipkartElectronicsAsync();

                if (products?.Count > 0)
                {
                    Console.WriteLine($"Found {products.Count} products. Saving to database...\n");

                    int successCount = 0;
                    foreach (var product in products)
                    {
                        try
                        {
                            int productId = await databaseService.InsertProductAsync(product);
                            Console.WriteLine($"✓ Saved: {product.Name} (ID: {productId})");
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"✗ Failed to save: {product.Name} - {ex.Message}");
                        }
                    }

                    Console.WriteLine($"\n=== Scraping Complete ===");
                    Console.WriteLine($"Total products found: {products.Count}");
                    Console.WriteLine($"Successfully saved: {successCount}");
                    Console.WriteLine($"Failed to save: {products.Count - successCount}");
                }
                else
                {
                    Console.WriteLine("No products found or scraping failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
