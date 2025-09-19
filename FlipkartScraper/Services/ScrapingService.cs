using FlipkartScraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlipkartScraper.Services
{
  
    /// Service class for web scraping using Selenium   
    public class ScrapingService
    {
        private IWebDriver? _driver;
        private readonly int _timeoutSeconds = 10;
        private readonly int _delayBetweenActions = 3000; // 3 seconds
        private readonly int _pageLoadDelay = 3000; // 5 seconds for page load

       
        /// Initialize Chrome driver with custom options

        private void InitializeDriver()
        {
            try
            {
                var chromeOptions = new ChromeOptions();

                // Add Chrome options for better scraping



                chromeOptions.AddArgument("--headless"); // Commented out for debugging - uncomment for production
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("--disable-dev-shm-usage");
                chromeOptions.AddArgument("--disable-gpu");
                chromeOptions.AddArgument("--window-size=1920,1080");
                chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                // Disable notifications and location requests
                chromeOptions.AddArgument("--disable-notifications");
                chromeOptions.AddArgument("--disable-geolocation");

                // Performance optimizations
                chromeOptions.AddArgument("--disable-extensions");
                chromeOptions.AddArgument("--disable-plugins");


               
                _driver = new ChromeDriver(chromeOptions);
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30); // or 60
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_timeoutSeconds);
                // Add explicit waits for specific elements
                


                Console.WriteLine("Chrome driver initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Chrome driver: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Main method to scrape Flipkart Electronics category
        /// </summary>
        public async Task<List<Product>> ScrapeFlipkartElectronicsAsync()
        {
            var products = new List<Product>();

            try
            {
                InitializeDriver();

                Console.WriteLine("Navigating to Flipkart...");
                _driver!.Navigate().GoToUrl("https://www.flipkart.com");

                // Wait for page to load
                Thread.Sleep(_pageLoadDelay);

                // Try direct search approach first
                products = await SearchElectronicsProductsAsync();

                if (products.Count == 0)
                {
                    Console.WriteLine("Direct search failed, trying category navigation...");
                    products = await NavigateAndScrapeElectronicsAsync();
                }

                Console.WriteLine($"Successfully extracted {products.Count} products.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during scraping: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                // Clean up driver
                _driver?.Quit();
                _driver?.Dispose();
                Console.WriteLine("Chrome driver closed.");
            }

            return products;
        }

        /// <summary>
        /// Search for electronics products directly
        /// </summary>
        private async Task<List<Product>> SearchElectronicsProductsAsync()
        {
            var products = new List<Product>();

            try
            {
                // Find search box and search for electronics
                var searchSelectors = new[]
                {
                    "//input[@name='q']",
                    "//input[@placeholder='Search for Products, Brands and More']",
                    "//input[@class='Pke_EE']",
                    "//input[contains(@class, 'search')]"
                };

                IWebElement? searchBox = null;
                foreach (var selector in searchSelectors)
                {
                    try
                    {
                        searchBox = _driver!.FindElement(By.XPath(selector));
                        if (searchBox != null) break;
                    }
                    catch { continue; }
                }

                if (searchBox != null)
                {
              
                    searchBox.SendKeys("electronics");
                    searchBox.SendKeys(Keys.Enter);

                    Console.WriteLine("Performed search for 'electronics'");
                    //Thread.Sleep(_pageLoadDelay);

                    products = await ExtractProductsFromCurrentPageAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in search approach: {ex.Message}");
            }

            return products;
        }

        /// <summary>
        /// Navigate to electronics category and scrape
        /// </summary>
        private async Task<List<Product>> NavigateAndScrapeElectronicsAsync()
        {
            var products = new List<Product>();

            try
            {
                // Try direct URL approach
                var electronicsUrls = new[]
                {
                    "https://www.flipkart.com/electronics/pr?sid=6bo%2Cg0v&marketplace=FLIPKART",
                    "https://www.flipkart.com/electronics-store",
                    "https://www.flipkart.com/mobiles/pr?sid=tyy%2C4io&marketplace=FLIPKART",
                    "https://www.flipkart.com/search?q=electronics&otracker=search&otracker1=search&marketplace=FLIPKART"
                };

                foreach (var url in electronicsUrls)
                {
                    try
                    {
                        Console.WriteLine($"Trying URL: {url}");
                        _driver!.Navigate().GoToUrl(url);
                        Thread.Sleep(_pageLoadDelay);
                        products = await ExtractProductsFromCurrentPageAsync();
                        if (products.Count > 0)
                        {
                            Console.WriteLine($"Successfully found products using URL: {url}");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed with URL {url}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in navigation approach: {ex.Message}");
            }

            return products;
        }

        /// <summary>
        /// Close any popup dialogs
        /// </summary>

        /// <summary>
        /// Extract products from the current page with multiple selector strategies
        /// </summary>
        private async Task<List<Product>> ExtractProductsFromCurrentPageAsync()
        {
            var products = new List<Product>();

            try
            {
                Console.WriteLine("Looking for products on current page...");

                // Try multiple strategies to find product containers
                var productContainerSelectors = new[]
                {
            "//div[contains(@class,'slAVV4')]", // ✅ your snippet
            "//div[@data-id]",
            "//div[contains(@class,'_1AtVbE')]",
            "//div[contains(@class,'_13oc-S')]",
            "//div[contains(@class,'_2kHMtA')]",
            "//a[contains(@class,'CGtC98')]",
            "//div[contains(@class,'yKfJKb')]"
        };

                IList<IWebElement>? productElements = null;
                string usedSelector = "";

                foreach (var selector in productContainerSelectors)
                {
                    try
                    {
                        productElements = _driver!.FindElements(By.XPath(selector));
                        if (productElements.Count > 0)
                        {
                            usedSelector = selector;
                            Console.WriteLine($"Found {productElements.Count} product containers using selector: {selector}");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Selector failed: {selector} - {ex.Message}");
                    }
                }

                if (productElements == null || productElements.Count == 0)
                {
                    Console.WriteLine("No product containers found.");
                    return products;
                }

                // Limit to first 20 products to avoid overload
                int maxProducts = Math.Min(20, productElements.Count);
                Console.WriteLine($"Processing {maxProducts} products...");

                for (int i = 0; i < maxProducts; i++)
                {
                    try
                    {
                        var productElement = productElements[i];
                        var product = ExtractProductDetails(productElement, i + 1);

                        if (!string.IsNullOrEmpty(product.Name) && product.Name != "Unknown Product")
                        {
                            products.Add(product);  
                            Console.WriteLine($"✓ Extracted: {product.Name} - {product.Price} ");
                        }
                        else
                        {
                            Console.WriteLine($"✗ Skipped product {i + 1}: No valid name found");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error extracting product {i + 1}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Successfully extracted {products.Count} valid products out of {maxProducts} attempted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting products from page: {ex.Message}");
            }

            return products;
        }


        private Product ExtractProductDetails(IWebElement productElement, int productIndex)
        {
            var product = new Product();

            try
            {
                // Product Name
                product.Name = ExtractTextBySelectors(productElement, new[]
                {
            ".//a[contains(@class,'wjcEIp')]",
            ".//div[contains(@class,'KzDlHZ')]",
            ".//div[contains(@class,'_4rR01T')]"
        }) ?? "Unknown Product";

                // Product Price
                product.Price = ExtractTextBySelectors(productElement, new[]
                {
            ".//div[contains(@class,'Nx9bqj')]", // current price
            ".//div[contains(@class,'_30jeq3')]"
        }) ?? "Price not available";

                // Rating
                product.Rating = ExtractTextBySelectors(productElement, new[]
                {
            ".//div[contains(@class,'XQDdHH')]",
            ".//div[contains(@class,'_3LWZlK')]"
        }) ?? "No rating";
                // Description
                
                product.Description = ExtractTextBySelectors(productElement, new[]
                {
    ".//ul[contains(@class,'G4BRas')]",   // list style description
    ".//div[contains(@class,'yKfJKb')]",  // alternative desc block
    ".//div[contains(@class,'_3Djpdu')]"  // fallback
}) ?? "No description";

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting product details for item {productIndex}: {ex.Message}");
            }

            return product;
        }

        private string? ExtractTextBySelectors(IWebElement parent, string[] selectors)
        {
            foreach (var selector in selectors)
            {
                try
                {
                    var elements = parent.FindElements(By.XPath(selector));
                    foreach (var element in elements)
                    {
                        var text = element.Text?.Trim();
                        if (!string.IsNullOrEmpty(text) && text.Length > 2)
                        {
                            return text;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return null;
        }


    }
}